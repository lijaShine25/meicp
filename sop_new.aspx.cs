using Newtonsoft.Json;
using NPoco;
using System.IO;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.SqlServer.Management.Smo;
using Database = NPoco.Database;
using System.Web.Services;
using System.Data;
using Org.BouncyCastle.Asn1.Cmp;
using System.Windows.Forms;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;
using System.Text.RegularExpressions;
//using static Microsoft.SqlServer.Management.Sdk.Sfc.RequestObjectInfo;

public partial class sop_new : System.Web.UI.Page
{
    protected DataTable dtGridProcess; // DataTable for GridView data
    protected DataTable dtGridTooling;
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo == null)
        {
            Response.Redirect("~/LoginPage.aspx");
        }
        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanPrepare != "Y" && userInfo[0].CanApprove != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadGroups();
            LoadParts();
            LoadToolingsGrid();
            LoadProcessData();
            if (Request.QueryString.HasKeys())
            {

                //if (userInfo[0].isAdmin == "Y" || userInfo[0].CanApprove == "Y")
                //{
                //    btnApproved.Enabled = true;
                //}
                //else
                //{
                //    btnApproved.Enabled = false;
                //}
                if (userInfo[0].CanPrepare == "Y")
                {
                    btnSave.Enabled = true;
                    btnSubmit.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSubmit.Enabled = false;
                }
                btnDelete.Enabled = true;
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnTemplate.Value = Request.QueryString["template"];
                if (hdnTemplate.Value == "Template 2" || hdnTemplate.Value == "Template 3")
                {
                    // Response.Redirect("sop_new_Template2.aspx?slno=" + hdnSlNo.Value + "&template=" + hdnTemplate.Value);
                }
                hdnEditMode.Value = "E";
                GetDetails();
            }
            else
            {
                btnDelete.Enabled = false;
            }

        }


    }
    void LoadGroups()
    {
        Database db = new Database("connString");
        List<Class_sop_mapping> lst = db.Fetch<Class_sop_mapping>("Select Distinct Group_name,Group_Id from sop_Mapping where del_status='N'  ").ToList();
        ddlGroup.DataSource = lst;
        ddlGroup.DataTextField = "Group_name";
        ddlGroup.DataValueField = "Group_Id";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, new ListItem("Select", "0"));
    }
    void LoadToolingsGrid()
    {
        if ((string.IsNullOrEmpty(hdnSlNo.Value) || hdnSlNo.Value == "-1") && hdnPreviousSlNo_ForCopy.Value == "0")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SerialNumber");
            dt.Columns.Add("Operation");
            dt.Columns.Add("tool_holder_name");
            dt.Columns.Add("tool");
            dt.Columns.Add("cutting_speed");
            dt.Columns.Add("feed_rate");
            dt.Columns.Add("per_corner");
            dt.Columns.Add("no_of_corners");
            dt.Columns.Add("total_nos");
            dt.Columns.Add("control_method");
            dt.Rows.Add(dt.NewRow());
            //dt.Columns[0].DefaultValue = "1";
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        else
        {
            Database db = new Database("connString");
            List<Class_sop_toolings_new> lst = new List<Class_sop_toolings_new>();

            if (Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value) > 0)
                lst = db.Fetch<Class_sop_toolings_new>("Select * from  sop_toolings_new  where sop_id=@0",
                Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value)).ToList();
            else
                lst = db.Fetch<Class_sop_toolings_new>("Select * from  sop_toolings_new  where sop_id=@0",
                    Convert.ToInt32(hdnSlNo.Value)).ToList();
            //   List<Class_sop_toolings_new> lst = new List<Class_sop_toolings_new>().Where(x => x.sop_id == Convert.ToInt32(hdnSlNo.Value)).ToList();
            if (lst.Count > 0)
            {
                GridView1.DataSource = lst;

                GridView1.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SerialNumber");
                dt.Columns.Add("Operation");
                dt.Columns.Add("tool_holder_name");
                dt.Columns.Add("tool");
                dt.Columns.Add("cutting_speed");
                dt.Columns.Add("feed_rate");
                dt.Columns.Add("per_corner");
                dt.Columns.Add("no_of_corners");
                dt.Columns.Add("total_nos");
                dt.Columns.Add("control_method");
                dt.Rows.Add(dt.NewRow());
                //dt.Columns[0].DefaultValue = "1";
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

    }

    protected bool IsCurrentRowFilled(GridView gridView)
    {
        if (gridView == GridView1)
        {
            // Get the index of the last row
            int rowIndex = gridView.Rows.Count - 1;

            // Find the TextBox controls in the current row and check if they are filled
            TextBox txtOperation = (TextBox)gridView.Rows[rowIndex].FindControl("txtOperation");
            TextBox txtHolderName = (TextBox)gridView.Rows[rowIndex].FindControl("txtHolderName");
            TextBox txtToolInsert = (TextBox)gridView.Rows[rowIndex].FindControl("txtToolInsert");
            TextBox txtCuttingSpeed = (TextBox)gridView.Rows[rowIndex].FindControl("txtCuttingSpeed");
            TextBox txtFeed = (TextBox)gridView.Rows[rowIndex].FindControl("txtFeed");
            TextBox txtToolLifePerCorner = (TextBox)gridView.Rows[rowIndex].FindControl("txtToolLifePerCorner");
            TextBox txtToolLifeNoOfCorner = (TextBox)gridView.Rows[rowIndex].FindControl("txtToolLifeNoOfCorner");
            TextBox txtToolLifeTotal = (TextBox)gridView.Rows[rowIndex].FindControl("txtToolLifeTotal");
            TextBox txtControlMethod = (System.Web.UI.WebControls.TextBox)gridView.Rows[rowIndex].FindControl("txtControlMethod");

            // Check if any of the TextBox controls is empty
            if (string.IsNullOrWhiteSpace(txtOperation.Text) ||
                string.IsNullOrWhiteSpace(txtHolderName.Text) ||
                string.IsNullOrWhiteSpace(txtToolInsert.Text) ||
                string.IsNullOrWhiteSpace(txtCuttingSpeed.Text) ||
                string.IsNullOrWhiteSpace(txtFeed.Text) ||
                string.IsNullOrWhiteSpace(txtToolLifePerCorner.Text) ||
                string.IsNullOrWhiteSpace(txtToolLifeNoOfCorner.Text) ||
                string.IsNullOrWhiteSpace(txtToolLifeTotal.Text) ||
                string.IsNullOrWhiteSpace(txtControlMethod.Text))
            {
                return false; // Current row is not filled
            }

            return true; // Current row is filled
        }
        else if (gridView == GridViewprc)
        {
            // Get the index of the last row
            int rowIndex = gridView.Rows.Count - 1;

            TextBox txtParameter = (TextBox)gridView.Rows[rowIndex].FindControl("txtParameter");
            TextBox txtParamValue = (TextBox)gridView.Rows[rowIndex].FindControl("txtParamValue");
            TextBox txtCheckingMethod = (TextBox)gridView.Rows[rowIndex].FindControl("txtCheckingMethod");
            TextBox txtFrequency = (TextBox)gridView.Rows[rowIndex].FindControl("txtFrequency");
            TextBox txtControl_Method = (TextBox)gridView.Rows[rowIndex].FindControl("txtControl_Method");

            // Check if any of the TextBox controls is empty
            if (string.IsNullOrWhiteSpace(txtParameter.Text) ||
                string.IsNullOrWhiteSpace(txtParamValue.Text) ||
                string.IsNullOrWhiteSpace(txtCheckingMethod.Text) ||
                string.IsNullOrWhiteSpace(txtFrequency.Text) ||
                string.IsNullOrWhiteSpace(txtControl_Method.Text))
            {
                return false; // Current row is not filled
            }

            return true; // Current row is filled
        }
        else
            return false;
    }
    protected void GridViewprc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the row index from CommandArgument
            DeleteRowFromPRCGridView(rowIndex);
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the row index from CommandArgument
            DeleteRowFromTOOLGridView(rowIndex);
        }
    }

    protected void DeleteRowFromPRCGridView(int rowIndex)
    {

        DataTable dt = GetGridViewDataPrc(); // Method to retrieve current GridView data
        DataRow row = dt.Rows[rowIndex];
        dt.Rows.Remove(row);
        UpdateSerialNumbers(dt);
        GridViewprc.DataSource = dt;
        GridViewprc.DataBind();
    }

    protected void DeleteRowFromTOOLGridView(int rowIndex)
    {

        DataTable dt = GetGridViewData(); // Method to retrieve current GridView data
        DataRow row = dt.Rows[rowIndex];
        dt.Rows.Remove(row);
        UpdateSerialNumbers(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void UpdateSerialNumbers(DataTable dt)
    {
        // Update the "SerialNumber" column for all rows in the DataTable
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["SerialNumber"] = i + 1; // Set the "SerialNumber" to the updated sequence
        }
    }





    protected void btnAddNewRow_Click(object sender, EventArgs e)
    {
        if (IsCurrentRowFilled(GridView1))
        {
            DataTable dt = GetGridViewData(); // Method to retrieve current GridView data
            DataRow newRow = dt.NewRow();
            dt.Rows.Add(newRow);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        else
        {
            // Show alert message to fill current row data
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please fill the current row data.');", true);
        }
    }

    protected void btnAddNewRowPrc_Click(object sender, EventArgs e)
    {
        if (IsCurrentRowFilled(GridViewprc))
        {
            DataTable dt = GetGridViewDataPrc(); // Method to retrieve current GridView data
            DataRow newRow = dt.NewRow();
            dt.Rows.Add(newRow);
            GridViewprc.DataSource = dt;
            GridViewprc.DataBind();
        }
        else
        {
            // Show alert message to fill current row data
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please fill the current row data.');", true);
        }
    }

    // Method to retrieve current GridView data (if any)
    private DataTable GetGridViewData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SerialNumber");
        dt.Columns.Add("Operation");
        dt.Columns.Add("tool_holder_name");
        dt.Columns.Add("tool");
        dt.Columns.Add("cutting_speed");
        dt.Columns.Add("feed_rate");
        dt.Columns.Add("per_corner");
        dt.Columns.Add("no_of_corners");
        dt.Columns.Add("total_nos");
        dt.Columns.Add("control_method");
        // Iterate through the GridView's rows and add data to the DataTable
        foreach (GridViewRow row in GridView1.Rows)
        {
            DataRow newRow = dt.NewRow();
            newRow["SerialNumber"] = row.RowIndex + 1;
            newRow["Operation"] = ((TextBox)row.FindControl("txtOperation")).Text;
            newRow["tool_holder_name"] = ((TextBox)row.FindControl("txtHolderName")).Text;
            newRow["tool"] = ((TextBox)row.FindControl("txtToolInsert")).Text;
            newRow["cutting_speed"] = ((TextBox)row.FindControl("txtCuttingSpeed")).Text;
            newRow["feed_rate"] = ((TextBox)row.FindControl("txtFeed")).Text;
            newRow["per_corner"] = ((TextBox)row.FindControl("txtToolLifePerCorner")).Text;
            newRow["no_of_corners"] = ((TextBox)row.FindControl("txtToolLifeNoOfCorner")).Text;
            newRow["total_nos"] = ((TextBox)row.FindControl("txtToolLifeTotal")).Text;
            newRow["control_method"] = ((TextBox)row.FindControl("txtControlMethod")).Text;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable GetGridViewDataPrc()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SerialNumber");
        dt.Columns.Add("Parameter");
        dt.Columns.Add("Param_Value");
        dt.Columns.Add("CheckingMethod");
        dt.Columns.Add("Frequency");
        dt.Columns.Add("Control_Method");

        foreach (GridViewRow row in GridViewprc.Rows)
        {
            DataRow newRow = dt.NewRow();
            newRow["SerialNumber"] = row.RowIndex + 1;
            newRow["Parameter"] = ((TextBox)row.FindControl("txtParameter")).Text;
            newRow["Param_Value"] = ((TextBox)row.FindControl("txtParamValue")).Text;
            newRow["CheckingMethod"] = ((TextBox)row.FindControl("txtCheckingMethod")).Text;
            newRow["Frequency"] = ((TextBox)row.FindControl("txtFrequency")).Text;
            newRow["Control_Method"] = ((TextBox)row.FindControl("txtControl_Method")).Text;

            dt.Rows.Add(newRow);
        }
        return dt;
    }

    protected void delFile1_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile1.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile1.Text = string.Empty;
            SaveData();
        }
    }
    protected void delFile2_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile2.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile2.Text = string.Empty;
            SaveData();
        }
    }
    protected void delFile3_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile3.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile3.Text = string.Empty;
            SaveData();
        }
    }
    protected void delFile4_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile4.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile4.Text = string.Empty;
            SaveData();
        }
    }
    protected void delFile5_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile5.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile5.Text = string.Empty;
            SaveData();
        }
    }
    protected void delFile6_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile6.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile6.Text = string.Empty;
            SaveData();
        }
    }
    protected void btnApproved_Click(object sender, EventArgs e)
    {
        using (Database db = new Database("connString"))
        {
            Class_sop_header_new c = db.SingleOrDefaultById<Class_sop_header_new>(hdnSlNo.Value);
            c.submitstatus = "A";
            c.is_approved = true;
            db.Update(c);

            ShowMessage("Approved", MessageType.Info);
            ClearData();
        }
    }
    void GetDetails(string slnotocopy = null)
    {
        Crud_sop_header_new crud = new Crud_sop_header_new();

        Class_sop_header_new clshdr = null;
        if (Convert.ToInt32(slnotocopy) > 0)
        {
            clshdr = crud.SelectOne(Convert.ToInt32(slnotocopy));
        }
        else
        {
            clshdr = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
            LoadGroups();
            ddlGroup.SelectedValue = clshdr.Group_Id.ToString();
            ddlGroup.Enabled = false;
            LoadParts();
            hdnMapSlno.Value = clshdr.Map_slno.ToString();
            ddlpart_slno.SelectedValue = clshdr.part_slno.ToString();
            ddlpart_slno.Enabled = false;
            hdnprevoprnslno.Value = clshdr.prev_oprn_slno.ToString();
            hdnnextoprnslno.Value = clshdr.next_oprn_slno.ToString();
            ddlcpType.SelectedValue = clshdr.cp_type;
            txtTemplate.Text = clshdr.Template;
            LoadOperations();
            ddloperation_slno.SelectedValue = clshdr.operation_slno.ToString();
            LoadMachinesFromparts();
            ddlmachine_slno.SelectedValue = clshdr.machine_slno.ToString();

        }

        txtobjective.Text = clshdr.objective;
        lblFile1.Text = clshdr.instruction_1;
        lblFile2.Text = clshdr.instruction_2;
        lblFile3.Text = clshdr.instruction_3;
        lblFile4.Text = clshdr.instruction_4;
        lblFile5.Text = clshdr.instruction_5;
        lblFile6.Text = clshdr.instruction_6;
        //hrefFile1.HRef = "~/Documents/SOP/" + clshdr.instruction_1;
        //hrefFile2.HRef = "~/Documents/SOP/" + clshdr.instruction_2;
        //hrefFile3.HRef = "~/Documents/SOP/" + clshdr.instruction_3;
        //hrefFile4.HRef = "~/Documents/SOP/" + clshdr.instruction_4;
        //hrefFile5.HRef = "~/Documents/SOP/" + clshdr.instruction_5;
        //hrefFile6.HRef = "~/Documents/SOP/" + clshdr.instruction_6;



        hrefFile1.HRef = "~/Documents/SOP/" + clshdr.instruction_1;
        hrefFile1.Target = "_blank";

        hrefFile2.HRef = "~/Documents/SOP/" + clshdr.instruction_2;
        hrefFile2.Target = "_blank";

        hrefFile3.HRef = "~/Documents/SOP/" + clshdr.instruction_3;
        hrefFile3.Target = "_blank";

        hrefFile4.HRef = "~/Documents/SOP/" + clshdr.instruction_4;
        hrefFile4.Target = "_blank";

        hrefFile5.HRef = "~/Documents/SOP/" + clshdr.instruction_5;
        hrefFile5.Target = "_blank";

        hrefFile6.HRef = "~/Documents/SOP/" + clshdr.instruction_6;
        hrefFile6.Target = "_blank";


        txtcomment1.Text = clshdr.comment_1;
        txtcomment2.Text = clshdr.comment_2;
        txtcomment3.Text = clshdr.comment_3;
        txtcomment4.Text = clshdr.comment_4;
        txtcomment5.Text = clshdr.comment_5;
        txtcomment6.Text = clshdr.comment_6;
        txtoprninstruction.Text = clshdr.oprn_instruction;
        txtochkpoints.Text = clshdr.checkpoints_list;
        txtworkholding.Text = clshdr.workholding_method;
        txtfirstoff.Text = clshdr.firstoff_approval;
        txtcoolant.Text = clshdr.coolant_used;
        txtreacionplan.Text = clshdr.reaction_plan;
        txtnotes.Text = clshdr.notes;
        if (clshdr.submitstatus == "Y")
        { 
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
        }
        if (clshdr.is_approved == true)
        { btnDelete.Enabled = false; }
        else { btnDelete.Enabled = true; }
        LoadProcessData();
        LoadToolingsGrid();
    }
    void LoadProcessData()
    {
        if ((string.IsNullOrEmpty(hdnSlNo.Value) || hdnSlNo.Value == "-1") && hdnPreviousSlNo_ForCopy.Value == "0")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SerialNumber");
            dt.Columns.Add("Parameter");
            dt.Columns.Add("Param_Value");
            dt.Columns.Add("CheckingMethod");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Control_Method");
            dt.Rows.Add(dt.NewRow());
            GridViewprc.DataSource = dt;
            GridViewprc.DataBind();
        }
        else
        {

            // List<Class_sop_ProcessParameter_new> lst = new List<Class_sop_ProcessParameter_new>().Where(x =>x.sop_id==Convert.ToInt32(hdnSlNo.Value)).ToList();
            Database db = new Database("connString");
            List<Class_sop_ProcessParameter_new> lst = new List<Class_sop_ProcessParameter_new>();
            if (Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value) > 0)
                lst = db.Fetch<Class_sop_ProcessParameter_new>("Select * from  sop_ProcessParameternew  where sop_id=@0",
                Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value)).ToList();
            else
                lst = db.Fetch<Class_sop_ProcessParameter_new>("Select * from  sop_ProcessParameternew  where sop_id=@0",
                    Convert.ToInt32(hdnSlNo.Value)).ToList();
            if (lst.Count > 0)
            {
                GridViewprc.DataSource = lst;
                GridViewprc.DataBind();

            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SerialNumber");
                dt.Columns.Add("Parameter");
                dt.Columns.Add("Param_Value");
                dt.Columns.Add("CheckingMethod");
                dt.Columns.Add("Frequency");
                dt.Columns.Add("Control_Method");
                dt.Rows.Add(dt.NewRow());
                GridViewprc.DataSource = dt;
                GridViewprc.DataBind();
            }
        }

    }

    //void LoadProcessData()
    //{
    //    List<Class_sop_ProcessParameter_new> lst = new List<Class_sop_ProcessParameter_new>().Where(x => x.sop_id == Convert.ToInt32(hdnSlNo.Value)).ToList();
    //    if (lst.Count > 0)
    //    {
    //        txtprogramNo.Text = lst[0].Program_No;
    //        txtHSP.Text = lst[0].Hydraulic_System_Pressure;
    //        txtCoolant_pressure.Text = lst[0].Through_Coolant_Pressure;
    //        txtCoolantConcentration.Text = lst[0].Coolant_Concentration;
    //        txtcomponentOrient.Text = lst[0].Component_Orientation;
    //        txtcheckingMethod1.Text = lst[0].CheckingMethod;
    //        txtCheckingFrequency1.Text = lst[0].Frequency;
    //        txtControlMethod1.Text = lst[0].Control_Method;
    //    }

    //}
    protected void btnrevision_Click(object sender, EventArgs e)
    {

    }
    void LoadQualityGrid()
    {
    }
    void LoadMachines()
    {
        using (Database db = new Database("connString"))
        {
            if (ddloperation_slno.SelectedIndex > 0 && ddlpart_slno.SelectedIndex > 0)
            {
                List<Class_machines> x = db.Query<Class_machines>("select * FROM machines m, partsmapping p where p.machine_slno =m.machine_slno and p.part_slno=@0 and p.operation_slno =@1", Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue)).ToList();
                ddlmachine_slno.Items.Clear();
                if (x.Count > 0)
                {
                    ddlmachine_slno.DataSource = x;
                    ddlmachine_slno.DataTextField = "MachineDesc";
                    ddlmachine_slno.DataValueField = "machine_slno";
                    ddlmachine_slno.DataBind();

                    ddlmachine_slno.SelectedIndex = 0;
                    ddlmachine_slno.Enabled = false;
                }
            }
        }
    }
    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst = null;
        if (hdnEditMode.Value == "I")
        {
            lst = crud.usp_partsSelect().ToList().Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList();
            //lst = crud.usp_partsSelect().ToList().Where(x => x.Obsolete == "N").ToList();
            //lst = crud.usp_partsSelect().ToList();
        }
        else if (hdnEditMode.Value == "E")
        {
            lst = crud.usp_partsSelect().ToList();
        }
        ddlpart_slno.Items.Clear();
        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
        }
        ddlpart_slno.Items.Insert(0, new ListItem("Select", "0"));
    }
    void LoadOperationsFromparts()
    {
        using (Database db = new Database("connString"))
        {
            if (ddlpart_slno.SelectedIndex > 0)
            {

                List<Class_sop_mapping> lst = db.Fetch<Class_sop_mapping>("Select Distinct o.OperationDesc,m.operation_slno,Map_slno from sop_Mapping m inner join parts p  on p.part_slno=m.part_slno inner join operations o on o.operation_slno=m.operation_slno where m.del_status='N'   and Group_Id=@0  and  m.part_slno=@1", Convert.ToInt32(ddlGroup.SelectedValue), Convert.ToInt32(ddlpart_slno.SelectedValue)).ToList();
                ddloperation_slno.Items.Clear();
                if (lst.Count > 0)
                {
                    ddloperation_slno.DataSource = lst;
                    ddloperation_slno.DataTextField = "operationDesc";
                    ddloperation_slno.DataValueField = "operation_slno";
                    ddloperation_slno.DataBind();
                    ddloperation_slno.Items.Insert(0, new ListItem("Select", "0"));
                    ddloperation_slno.SelectedIndex = 1;
                    hdnMapSlno.Value = lst[0].Map_slno.ToString();
                }
            }
        }

        LoadMachinesFromparts();
        string sql = "Select cp.*,op.operationdesc from Controlplan cp inner join operations op on op.operation_slno=cp.operation_slno  where cp.part_slno=@0  and cp.rev_no =(SELECT MAX(c.rev_no) from controlplan c where c.part_slno=cp.part_slno AND c.operation_slno=cp.operation_slno) order by cast(cp.process_no as decimal) ";
        List<Class_ControlPlan> lst1 = new List<Class_ControlPlan>();
        using (Database db = new Database("connString"))
        {
            lst1 = db.Query<Class_ControlPlan>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue)).ToList();
            int index = lst1.FindIndex(item => item.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue));
            if (index - 1 >= 0)
            {
                int prevslno = lst1[index - 1].operation_slno;
                string previous = lst1[index - 1].operationdesc;
                txtprevoprn.Text = previous;
                hdnprevoprnslno.Value = prevslno.ToString();
            }
            else
            {
                txtprevoprn.Text = string.Empty;
                hdnprevoprnslno.Value = "";
            }
            if (index + 1 < lst1.Count)
            {
                string next = lst1[index + 1].operationdesc;
                int nextslno = lst1[index + 1].operation_slno;
                txtnextoprn.Text = next;
                hdnnextoprnslno.Value = nextslno.ToString();
            }
            else
            {
                txtnextoprn.Text = string.Empty;
                hdnnextoprnslno.Value = "";
            }
        }
    }
    void LoadMachinesFromparts()
    {
        using (Database db = new Database("connString"))
        {
            if (ddloperation_slno.SelectedIndex > 0 && ddlpart_slno.SelectedIndex > 0)
            {
                var x = db.Query<Class_machines>("select * FROM machines m, partsmapping p where p.machine_slno =m.machine_slno and p.part_slno=@0 and p.operation_slno =@1", Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));

                ddlmachine_slno.DataSource = x;
                ddlmachine_slno.DataTextField = "MachineDesc";
                ddlmachine_slno.DataValueField = "machine_slno";
                ddlmachine_slno.DataBind();
                ddlmachine_slno.Items.Insert(0, new ListItem("Select", "0"));
                if (x != null)
                    ddlmachine_slno.SelectedIndex = 1;
                else
                    ddlmachine_slno.SelectedIndex = 0;
                ddlmachine_slno.Enabled = false;
            }
        }
    }
    void LoadOperations()
    {
        Crud_operations crud = new Crud_operations();
        List<Class_operations> lst = null;
        if (hdnEditMode.Value == "I")
        {
            lst = crud.usp_operationsSelect().ToList().Where(x => x.del_status == "N").ToList(); ;
        }
        else if (hdnEditMode.Value == "E")
        {
            lst = crud.usp_operationsSelect().ToList();
            if (!string.IsNullOrEmpty(hdnprevoprnslno.Value))
            {
                List<Class_operations> lstprev = lst.Where(x => x.operation_slno == Convert.ToInt32(hdnprevoprnslno.Value)).ToList();
                txtprevoprn.Text = lstprev[0].OperationDesc;
            }
            if (!string.IsNullOrEmpty(hdnnextoprnslno.Value))
            {
                List<Class_operations> lstnext = lst.Where(x => x.operation_slno == Convert.ToInt32(hdnnextoprnslno.Value)).ToList();
                txtnextoprn.Text = lstnext[0].OperationDesc;
            }
        }
        ddloperation_slno.Items.Clear();
        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddloperation_slno.Items.Add(new ListItem(lst[cnt].OperationDesc, Convert.ToString(lst[cnt].operation_slno)));
        }
        ddloperation_slno.Items.Insert(0, new ListItem("Select", "0"));
    }
    void LoadMachines2()
    {
        Crud_machines crud = new Crud_machines();
        List<Class_machines> lst = null;
        if (hdnEditMode.Value == "I")
        {
            lst = crud.usp_machinesSelect().ToList().Where(x => x.del_status == "N").ToList(); ;
        }
        else if (hdnEditMode.Value == "E")
        {
            lst = crud.usp_machinesSelect().ToList();
        }
        ddlmachine_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlmachine_slno.Items.Add(new ListItem(lst[cnt].MachineDesc, Convert.ToString(lst[cnt].machine_slno)));
        }
        ddlmachine_slno.Items.Insert(0, "Select...");
    }
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }
    protected void ddlgroup_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Database db = new Database("connString");
        List<Class_sop_mapping> lst = db.Fetch<Class_sop_mapping>("Select Distinct p.mstpartNo,m.part_slno,Template from sop_Mapping m inner join parts p  on p.part_slno=m.part_slno  where m.del_status='N'   and Group_Id=@0", Convert.ToInt32(ddlGroup.SelectedValue)).ToList();
        if (lst[0].Template == "Template 1")
        {
            ddlpart_slno.Items.Clear();
            ddlpart_slno.DataSource = lst;
            ddlpart_slno.DataTextField = "mstPartNo";
            ddlpart_slno.DataValueField = "part_slno";
            ddlpart_slno.DataBind();
            ddlpart_slno.Items.Insert(0, new ListItem("Select", "0"));
            txtTemplate.Text = lst[0].Template;
        }
        else if (lst[0].Template == "Template 2")
        {
            // Response.Redirect("sop_new_Template2.aspx?GroupId=" + Convert.ToInt32(ddlGroup.SelectedValue));
        }
    }
    protected void ddlpart_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        using (Crud_parts crud = new Crud_parts())
        {
            Class_parts cls = crud.SelectOne(Convert.ToInt32(ddlpart_slno.SelectedValue));
            hdnrevno.Value = cls.partIssueNo;
        }
        LoadOperationsFromparts();
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {

        Database db = new Database("connString");

        string QryToGetSopId = "select Max(sop_id) from sop_header_new where group_id=@0 and Template=@1";
        hdnPreviousSlNo_ForCopy.Value = db.ExecuteScalar<string>(QryToGetSopId, Convert.ToInt16(ddlGroup.SelectedValue), txtTemplate.Text);
        if (!string.IsNullOrEmpty(hdnPreviousSlNo_ForCopy.Value))
        {
            List<Class_sop_header_new> lstsopheader = db.Query<Class_sop_header_new>("Select * from  sop_header_new  where group_id=@0 and Template=@1  and sop_id=@2", Convert.ToInt16(ddlGroup.SelectedValue), txtTemplate.Text, Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value)).ToList();
            List<Class_sop_ProcessParameter_new> lstProcessParameter = db.Query<Class_sop_ProcessParameter_new>("Select * from  sop_ProcessParameternew  where group_id=@0 and Template=@1 and sop_id=@2", Convert.ToInt16(ddlGroup.SelectedValue), txtTemplate.Text, Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value)).ToList();
            List<Class_sop_toolings_new> lsttoolings = db.Query<Class_sop_toolings_new>("Select * from  sop_toolings_new  where group_id=@0 and Template=@1 and sop_id=@2", Convert.ToInt16(ddlGroup.SelectedValue), txtTemplate.Text, Convert.ToInt32(hdnPreviousSlNo_ForCopy.Value)).ToList();
            GetDetails(hdnPreviousSlNo_ForCopy.Value);
        }
        else ShowMessage("No Records To Copy from.", MessageType.Info);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        hdnsubmitstatus.Value = "N";
        SaveData();

        //ClearData();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        using (Database db = new Database("connString"))
        {
            hdnsubmitstatus.Value = "Y";
            SaveData();





            //Class_sop_header_new c = db.SingleOrDefaultById<Class_sop_header_new>(hdnSlNo.Value);
            //c.submitstatus = "Y";
            //db.Update(c);
            //ShowMessage("Data Submitted", MessageType.Info);
            //ClearData();
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteData();
        ClearData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }
    void ClearData()
    {
        ddlGroup.SelectedIndex = 0;
        txtTemplate.Text = string.Empty;
        LoadParts();
        ddlpart_slno.SelectedValue = "0";
        hdnprevoprnslno.Value = string.Empty;
        hdnnextoprnslno.Value = string.Empty;
        txtprevoprn.Text = string.Empty;
        txtnextoprn.Text = string.Empty;
        ddlcpType.SelectedValue = "0";
        ddloperation_slno.SelectedValue = "0";
        LoadMachinesFromparts();
        ddlmachine_slno.SelectedValue = "0";
        txtobjective.Text = string.Empty;
        lblFile1.Text = string.Empty;
        lblFile2.Text = string.Empty;
        txtoprninstruction.Text = string.Empty;
        txtochkpoints.Text = string.Empty;
        txtworkholding.Text = string.Empty;
        txtfirstoff.Text = string.Empty;
        txtcoolant.Text = string.Empty;
        txtreacionplan.Text = string.Empty;
        txtnotes.Text = string.Empty;
        hrefFile1.HRef = string.Empty;
        hrefFile2.HRef = string.Empty;
        hdnSlNo.Value = "0";
        LoadToolingsGrid();
        LoadProcessData();
        txtcomment1.Text = string.Empty;
        txtcomment2.Text = string.Empty;
        txtcomment3.Text = string.Empty;
        txtcomment4.Text = string.Empty;
        txtcomment5.Text = string.Empty;
        txtcomment6.Text = string.Empty;
    }
    void DeleteData()
    {
        using (Database db = new Database("connString"))
        {
            Crud_sop_header_new crud = new Crud_sop_header_new();
            Class_sop_header_new clshdr = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
            int partslno = 0;
            int opernslno = 0;
            int machineslno = 0;
            partslno = clshdr.part_slno;
            opernslno = clshdr.operation_slno;
            machineslno = clshdr.machine_slno;
            db.DeleteWhere<Class_sop_toolings_new>(" sop_id=@0", Convert.ToInt32(hdnSlNo.Value));
            db.DeleteWhere<Class_sop_ProcessParameter_new>(" sop_id=@0", Convert.ToInt32(hdnSlNo.Value));
            db.DeleteWhere<Class_sop_header_new>(" sop_id=@0", Convert.ToInt32(hdnSlNo.Value));
            //string qryupdate = "update sop_header_new set  is_obsolete = 0, Submitstatus='N'  where part_slno=" + partslno + " and operation_slno=" + opernslno + "  and  machine_slno=" + machineslno + " and  rev_no =(SELECT MAX(c.rev_no) from sop_header_new c where c.part_slno=sop_header_new.part_slno AND c.operation_slno=sop_header_new.operation_slno)";
            //db.Execute(qryupdate);
        }
    }

    //protected void CheckIfSopAlreadyExists()
    //{
    //    Database db = new Database("connString");

    //    int partSlNo = Convert.ToInt32(ddlpart_slno.SelectedValue);
    //    int operationSlNo = Convert.ToInt32(ddloperation_slno.SelectedValue);

    //    List<Class_sop_header_new> lst = db.Fetch<Class_sop_header_new>("Select * from sop_header_new where part_slno="+partSlNo+"  and operation_slno="+ operationSlNo + " and Group_Id="+Convert.ToInt32(ddlGroup.SelectedValue)).ToList();


    //    if (lst.Count>0)
    //    {
    //        ShowMessage("An unapproved SOP already exists for the selected Part and Operation.", MessageType.Error);
    //        return; // Exit the function early
    //    }
    //}
    void SaveData()
    {
        Database db = new Database("connString");

        int partSlNo = Convert.ToInt32(ddlpart_slno.SelectedValue);
        int operationSlNo = Convert.ToInt32(ddloperation_slno.SelectedValue);

        List<Class_sop_header_new> lst = db.Fetch<Class_sop_header_new>("Select * from sop_header_new inner join sop_mapping on sop_mapping.Group_Id=sop_header_new.Group_Id and  sop_mapping.Map_slno=sop_header_new.Map_slno  where sop_mapping.Obsolete<>'Y'  and sop_header_new.part_slno=" + partSlNo + " and sop_header_new.is_obsolete=0 and sop_header_new.operation_slno=" + operationSlNo + " and sop_header_new.Group_Id=" + Convert.ToInt32(ddlGroup.SelectedValue)).ToList();
        if (hdnEditMode.Value == "E")
        {
            lst = db.Fetch<Class_sop_header_new>("Select * from sop_header_new inner join sop_mapping on sop_mapping.Group_Id=sop_header_new.Group_Id and  sop_mapping.Map_slno=sop_header_new.Map_slno  where sop_mapping.Obsolete<>'Y' and sop_header_new.part_slno=" + partSlNo + "   and sop_header_new.is_obsolete=0 and sop_header_new.operation_slno=" + operationSlNo + " and sop_header_new.Group_Id=" + Convert.ToInt32(ddlGroup.SelectedValue) + "   and  sop_header_new.sop_id != " + Convert.ToInt32(hdnSlNo.Value)).ToList();
        }

        if (lst.Count > 0)
        {
            ShowMessage("An unapproved SOP already exists for the selected Part and Operation.", MessageType.Error);
            return; // Exit the function early
        }
        if (GridViewprc.Rows.Count == 0 || GridView1.Rows.Count == 0)
        {
            ShowMessage("Please fill at least one row of data in both the Tables before saving.", MessageType.Error);
            return; // Stop execution if validation fails



        }
        else if (GridViewprc.Rows.Count == 1 || GridView1.Rows.Count == 1)
        {
            if (!IsCurrentRowFilled(GridView1))
            {
                ShowMessage("Please fill at least one row of data in both the Tables before saving.", MessageType.Error);
                return;
            }
            if (!IsCurrentRowFilled(GridViewprc))
            {
                ShowMessage("Please fill at least one row of data in both the Tables before saving.", MessageType.Error);
                return;
            }
        }


        try
        {
            Class_sop_toolings_new clstooling = new Class_sop_toolings_new();
            Class_sop_ProcessParameter_new clsprc = new Class_sop_ProcessParameter_new();
            string slno = "";
            using (Class_sop_header_new clshdr = new Class_sop_header_new())
            {
                clshdr.Map_slno = Convert.ToInt32(hdnMapSlno.Value);
                clshdr.Group_Id = Convert.ToInt32(ddlGroup.SelectedValue);
                clshdr.part_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
                clshdr.operation_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);
                clshdr.machine_slno = Convert.ToInt32(ddlmachine_slno.SelectedValue);
                if (!string.IsNullOrEmpty(hdnprevoprnslno.Value))
                    clshdr.prev_oprn_slno = Convert.ToInt32(hdnprevoprnslno.Value);
                if (!string.IsNullOrEmpty(hdnnextoprnslno.Value))
                    clshdr.next_oprn_slno = Convert.ToInt32(hdnnextoprnslno.Value);
                clshdr.objective = txtobjective.Text;
                //clshdr.instruction_1 = lblFile1.Text;
                //clshdr.instruction_2 = lblFile2.Text;
                //clshdr.instruction_3 = lblFile3.Text;
                //clshdr.instruction_4 = lblFile4.Text;
                //clshdr.instruction_5 = lblFile5.Text;
                //clshdr.instruction_6 = lblFile6.Text;
                clshdr.comment_1 = txtcomment1.Text;
                clshdr.comment_2 = txtcomment2.Text;
                clshdr.comment_3 = txtcomment3.Text;
                clshdr.comment_4 = txtcomment4.Text;
                clshdr.comment_5 = txtcomment5.Text;
                clshdr.comment_6 = txtcomment6.Text;
                clshdr.oprn_instruction = txtoprninstruction.Text;
                clshdr.checkpoints_list = txtochkpoints.Text;
                clshdr.workholding_method = txtworkholding.Text;
                clshdr.firstoff_approval = txtfirstoff.Text;
                clshdr.coolant_used = txtcoolant.Text;
                clshdr.reaction_plan = txtreacionplan.Text;
                clshdr.notes = txtnotes.Text;
                clshdr.cp_type = ddlcpType.SelectedValue;
                clshdr.Template = txtTemplate.Text;
                string savePath = Server.MapPath("~/Documents/SOP/");
                HttpPostedFile File1 = FileUpload1.PostedFile;
                HttpPostedFile File2 = FileUpload2.PostedFile;
                HttpPostedFile File3 = FileUpload3.PostedFile;
                HttpPostedFile File4 = FileUpload4.PostedFile;
                HttpPostedFile File5 = FileUpload5.PostedFile;
                HttpPostedFile File6 = FileUpload6.PostedFile;

                if (File1 != null && File1.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(lblFile1.Text))
                        {
                            string oldFilePath = Path.Combine(Server.MapPath("~/Documents/SOP/"), lblFile1.Text);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath); // Delete the old file
                            }
                        }
                        string fileName1 = ddlGroup.SelectedValue + "_" + Path.GetFileName(File1.FileName);
                        clshdr.instruction_1 = fileName1;
                        string filePath = Path.Combine(savePath, fileName1);
                        File1.SaveAs(filePath);
                        hrefFile1.HRef = "~/Documents/SOP/" + fileName1;
                    }
                    catch (Exception ex)
                    {
                        lblFile1.Text = "Error uploading file: " + ex.Message;
                    }
                }
                else if (!string.IsNullOrEmpty(lblFile1.Text))
                {
                    clshdr.instruction_1 = lblFile1.Text;
                }
                if (File2 != null && File2.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(lblFile2.Text))
                        {
                            string oldFilePath = Path.Combine(Server.MapPath("~/Documents/SOP/"), lblFile2.Text);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath); // Delete the old file
                            }
                        }
                        string fileName2 = ddlGroup.SelectedValue + "_" + Path.GetFileName(File2.FileName);
                        clshdr.instruction_2 = fileName2;
                        string filePath = Path.Combine(savePath, fileName2);
                        File2.SaveAs(filePath);
                        hrefFile2.HRef = "~/Documents/SOP/" + fileName2;
                    }
                    catch (Exception ex)
                    {
                        lblFile2.Text = "Error uploading file: " + ex.Message;
                    }

                }
                else if (!string.IsNullOrEmpty(lblFile2.Text))
                {
                    clshdr.instruction_2 = lblFile2.Text;
                }

                if (File3 != null && File3.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(lblFile3.Text))
                        {
                            string oldFilePath =  Path.Combine(Server.MapPath("~/Documents/SOP/"), lblFile3.Text);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath); // Delete the old file
                            }
                        }
                        string fileName3 = ddlGroup.SelectedValue + "_" + Path.GetFileName(File3.FileName);
                        clshdr.instruction_3 = fileName3;
                        string filePath = Path.Combine(savePath, fileName3);
                        File3.SaveAs(filePath);
                        hrefFile3.HRef = "~/Documents/SOP/" + fileName3;
                    }
                    catch (Exception ex)
                    {
                        lblFile3.Text = "Error uploading file: " + ex.Message;
                    }

                }
                else if (!string.IsNullOrEmpty(lblFile3.Text))
                {
                    clshdr.instruction_3 = lblFile3.Text;
                }


                if (File4 != null && File4.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(lblFile4.Text))
                        {
                            string oldFilePath = Path.Combine(Server.MapPath("~/Documents/SOP/"), lblFile4.Text);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath); // Delete the old file
                            }
                        }
                        string fileName4 = ddlGroup.SelectedValue + "_" + Path.GetFileName(File4.FileName);
                        clshdr.instruction_4 = fileName4;
                        string filePath = Path.Combine(savePath, fileName4);
                        File4.SaveAs(filePath);
                        hrefFile4.HRef = "~/Documents/SOP/" + fileName4;
                    }
                    catch (Exception ex)
                    {
                        lblFile4.Text = "Error uploading file: " + ex.Message;
                    }

                }
                else if (!string.IsNullOrEmpty(lblFile4.Text))
                {
                    clshdr.instruction_4 = lblFile4.Text;
                }

                if (File5 != null && File5.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(lblFile5.Text))
                        {
                            string oldFilePath =  Path.Combine(Server.MapPath("~/Documents/SOP/"), lblFile5.Text);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath); // Delete the old file
                            }
                        }
                        string fileName5 = ddlGroup.SelectedValue + "_" + Path.GetFileName(File5.FileName);
                        clshdr.instruction_5 = fileName5;
                        string filePath = Path.Combine(savePath, fileName5);
                        File5.SaveAs(filePath);
                        hrefFile5.HRef = "~/Documents/SOP/" + fileName5;
                    }
                    catch (Exception ex)
                    {
                        lblFile5.Text = "Error uploading file: " + ex.Message;
                    }

                }
                else if (!string.IsNullOrEmpty(lblFile5.Text))
                {
                    clshdr.instruction_5 = lblFile5.Text;
                }


                if (File6 != null && File6.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(lblFile6.Text))
                        {
                            string oldFilePath = Path.Combine(Server.MapPath("~/Documents/SOP/"), lblFile6.Text);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath); // Delete the old file
                            }
                        }
                        string fileName6 = ddlGroup.SelectedValue + "_" + Path.GetFileName(File6.FileName);
                        clshdr.instruction_6 = fileName6;
                        string filePath = Path.Combine(savePath, fileName6);
                        File6.SaveAs(filePath);
                        hrefFile6.HRef = "~/Documents/SOP/" + fileName6;
                    }
                    catch (Exception ex)
                    {
                        lblFile6.Text = "Error uploading file: " + ex.Message;
                    }

                }
                else if (!string.IsNullOrEmpty(lblFile6.Text))
                {
                    clshdr.instruction_6 = lblFile6.Text;
                }


                if (hdnEditMode.Value == "I")
                {
                    clshdr.submitstatus = "N";

                    using (Crud_sop_header_new crud = new Crud_sop_header_new())
                    {
                        crud.Insert(clshdr);
                        SaveProcessParam(clshdr.Group_Id, clshdr.sop_id, clshdr.Template, clshdr.Map_slno);
                        SaveToolings(clshdr.Group_Id, clshdr.sop_id, clshdr.Template, clshdr.Map_slno);
                    }
                }
                else
                {
                    clshdr.sop_id = Convert.ToInt32(hdnSlNo.Value);
                    using (Crud_sop_header_new crud = new Crud_sop_header_new())
                    {
                        if (hdnsubmitstatus.Value == "Y")
                            clshdr.submitstatus = "Y";
                        else clshdr.submitstatus = "N";
                        crud.Update(clshdr);
                        SaveProcessParam(clshdr.Group_Id, clshdr.sop_id, clshdr.Template, clshdr.Map_slno);
                        SaveToolings(clshdr.Group_Id, clshdr.sop_id, clshdr.Template, clshdr.Map_slno);
                    }

                }

            }
        }
        catch (Exception ex)
        {
            string errmsg = ex.Message;
            ShowMessage(errmsg, MessageType.Error);
        }
        if (hdnsubmitstatus.Value == "Y")
        {
            string sql = "Select h.* from SOP_header_new h inner  join SOP_Mapping m on m.map_slno=h.map_slno where h.is_obsolete='0' and  h.submitstatus <> 'Y'  and  h.Group_Id=" + Convert.ToInt32(ddlGroup.SelectedValue) + "  and m.rev_no=(select max(rev_no) from Sop_Mapping  where Group_id=" + Convert.ToInt32(ddlGroup.SelectedValue) + "   and obsolete='N' AND del_status = 'N')";
            List<Class_sop_header_new> lstmail = db.Query<Class_sop_header_new>(sql).ToList();
            if (lst.Count == 0)
            {
                string sqlCC = @"select emailid from  employees where del_status='N' and isAdmin='Y'";


                //  List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql, ddlpart_slno.SelectedValue);
                List<Class_Employees> lstemplCC = db.Fetch<Class_Employees>(sqlCC);
                List<string> tomails = new List<string>();
                if (lstemplCC.Count > 0)
                {
                    foreach (Class_Employees c1 in lstemplCC)
                    {
                        tomails.Add(c1.EmailId);
                    }
                }
                else
                {
                    tomails.Add(userInfo[0].EmailId);
                }

                List<string> ccmails = new List<string>();

                ccmails.Add(userInfo[0].EmailId);
                tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                string sqlqryIsLastSubmit = " select COUNT(*) from sop_header_new where Map_slno in(select Map_slno from SOP_Mapping  \r\n  where Group_id=@0  and Obsolete='N'   and del_status='N')   and submitstatus='N'  and is_obsolete='0' ";
                int cnt = db.ExecuteScalar<int>(sqlqryIsLastSubmit, Convert.ToInt32(ddlGroup.SelectedValue));
                if(cnt==0)
                PrepareForMail(tomails, ccmails, "revision");
            }

        }
        ShowMessage("Data Saved Successfully!!", MessageType.Success);
        ClearData();
    }
    public class EmailValidator
    {
        // Regex pattern for basic email validation
        private static readonly string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private static readonly Regex EmailPattern = new Regex(EmailRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Method to validate an email address
        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && EmailPattern.IsMatch(email);
        }
    }
    void PrepareForMail(List<string> tomails, List<string> ccmails, string mailFor)
    {
        //.check if mail triggers are enabled
        string sql = "select enable_trigger from settings";
        using (Database db = new Database("connString"))
        {
            var x = db.ExecuteScalar<string>(sql);
            if (x == null)
            {
                return;
            }
            else if (x == "N")
            {
                return;
            }
        }

        string s = string.Empty;

        string subject = string.Empty;

        // string[] parts = lblPartDtls.Text.Split(';');

        string msg = string.Empty;


        msg = "All SOP data  for the Group " + ddlGroup.SelectedItem.Text + " has been submitted  and is Pending for Your Approval. Please Login to the portal to Approve. ";
        subject = "SOP  Group " + ddlGroup.SelectedItem.Text + " Pending for  Approval ";



        string styl = "<style>table{max-width:100%;background-color:transparent;font-size:14px}th{text-align:left}.table{width:100%;margin-bottom:20px}.table>tbody>tr>td,.table>tbody>tr>th,.table>tfoot>tr>td,.table>tfoot>tr>th,.table>thead>tr>td,.table>thead>tr>th{padding:8px;line-height:1.428571429;vertical-align:top;border-top:1px solid #ddd}.table>thead>tr>th{vertical-align:bottom;border-bottom:2px solid #ddd}.table>tbody+tbody{border-top:2px solid #ddd}.table .table{background-color:#fff}.table-striped>tbody>tr:nth-child(odd)>td,.table-striped>tbody>tr:nth-child(odd)>th{background-color:#f9f9f9}body{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:14px;line-height:1.428571429;color:#333;background-color:#fff}h2{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:500;line-height:1.1;color:inherit;text-align:center}</style>";

        string bodycnt = "<center><b><label style='background-color:#0198FF; color:White;font-family:Calibri;font-size:medium;'>&nbsp;THIS IS AN AUTOGENERATED MAIL. DO NOT REPLY TO THIS!! &nbsp;</label></b></center>" +
            "<body style='font-family:Calibri;font-size:medium;'>Dear Sir / Madam, <br/><br/>" + msg + "";


        string mailBody = "<html><head>" + styl + "</head><body>" + bodycnt + "</body></html>";

        List<string> newToId = tomails.Distinct().ToList();
        List<string> newCcId = ccmails.Distinct().ToList();


        // send mail
        mail ml = new mail();
        ml.SendMail(toMailId: newToId, mailSubject: subject, bodyText: mailBody, ccMailId: newCcId);
    }
    void SaveProcessParam(int Group_Id, int sop_id, string Template, int Map_slno)
    {
        string qrydel = "Delete from sop_ProcessParameternew where Map_Slno=" + Map_slno + " and  sop_id=" + sop_id + "   and Template='" + Template + "'  and  Group_Id=" + Group_Id;
        Class_sop_ProcessParameter_new clstool = new Class_sop_ProcessParameter_new();
        int flag1 = 0;
        foreach (GridViewRow row in GridViewprc.Rows)
        {
            TextBox txtParameter = (TextBox)row.FindControl("txtParameter");
            TextBox txtParamValue = (TextBox)row.FindControl("txtParamValue");
            TextBox txtCheckingMethod = (TextBox)row.FindControl("txtCheckingMethod");
            TextBox txtFrequency = (TextBox)row.FindControl("txtFrequency");
            TextBox txtControl_Method = (TextBox)row.FindControl("txtControl_Method");

            if (!string.IsNullOrWhiteSpace(txtParameter.Text) ||
                !string.IsNullOrWhiteSpace(txtParamValue.Text) ||
                !string.IsNullOrWhiteSpace(txtCheckingMethod.Text) ||
                !string.IsNullOrWhiteSpace(txtFrequency.Text) ||
                !string.IsNullOrWhiteSpace(txtControl_Method.Text))
            {
                flag1 = 1;
                break;
            }
        }

        if (flag1 == 1)
        {
            using (Crud_sop_ProcessParameter_new crud = new Crud_sop_ProcessParameter_new())
            {
                clstool.Map_slno = Map_slno;
                clstool.sop_id = sop_id;
                clstool.Template = Template;
                clstool.Group_Id = Group_Id;
                Database db = new Database("connString");
                List<Class_sop_ProcessParameter_new> lst = new List<Class_sop_ProcessParameter_new>();
                lst = db.Fetch<Class_sop_ProcessParameter_new>().Where(x => x.Map_slno == clstool.Map_slno && x.sop_id == clstool.sop_id && x.Template == clstool.Template && x.Group_Id == clstool.Group_Id).ToList();
                if (lst.Count > 0)
                {
                    //Database db = new Database("connString");
                    db.Execute(qrydel);
                    // db.Delete<Class_sop_ProcessParameter_new>("Delete from sop_ProcessParameternew where Map_Slno=@0 and  sop_id=@1  and Template=@2  and  Group_Id=@3 ", Map_slno, sop_id, Template, Group_Id);

                }
                foreach (GridViewRow row in GridViewprc.Rows)
                {
                    TextBox txtParameter = (TextBox)row.FindControl("txtParameter");
                    TextBox txtParamValue = (TextBox)row.FindControl("txtParamValue");
                    TextBox txtCheckingMethod = (TextBox)row.FindControl("txtCheckingMethod");
                    TextBox txtFrequency = (TextBox)row.FindControl("txtFrequency");
                    TextBox txtControl_Method = (TextBox)row.FindControl("txtControl_Method");

                    clstool.Parameter = txtParameter.Text;
                    clstool.Param_Value = txtParamValue.Text;
                    clstool.CheckingMethod = txtCheckingMethod.Text;
                    clstool.Frequency = txtFrequency.Text;
                    clstool.Control_Method = txtControl_Method.Text;
                    if (string.IsNullOrWhiteSpace(txtParameter.Text) &&
                 string.IsNullOrWhiteSpace(txtParamValue.Text) &&
                 string.IsNullOrWhiteSpace(txtCheckingMethod.Text) &&
                 string.IsNullOrWhiteSpace(txtFrequency.Text) &&
                 string.IsNullOrWhiteSpace(txtControl_Method.Text)) { }
                    else
                        crud.Insert(clstool);
                }
            }
        }
    }

    //void SaveProcessParam(int Group_Id, int sop_id, string Template, int Map_slno)
    //{
    //    Class_sop_ProcessParameter_new clsprc = new Class_sop_ProcessParameter_new();
    //    int flag1 = 0;
    //    if (txtprogramNo.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Program_No = txtprogramNo.Text;
    //    }
    //    if (txtHSP.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Hydraulic_System_Pressure = txtHSP.Text;
    //    }
    //    if (txtCoolant_pressure.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Through_Coolant_Pressure = txtCoolant_pressure.Text;
    //    }
    //    if (txtCoolantConcentration.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Coolant_Concentration = txtCoolantConcentration.Text;
    //    }
    //    if (txtcomponentOrient.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Component_Orientation = txtcomponentOrient.Text;
    //    }
    //    if (txtcheckingMethod1.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.CheckingMethod = txtcheckingMethod1.Text;
    //    }

    //    if (txtCheckingFrequency1.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Frequency = txtCheckingFrequency1.Text;
    //    }
    //    if (txtControlMethod1.Text.Length > 0)
    //    {
    //        flag1 = 1;
    //        clsprc.Control_Method = txtControlMethod1.Text;
    //    }

    //    if (flag1 == 1)
    //    {
    //        using (Crud_sop_ProcessParameter_new crud = new Crud_sop_ProcessParameter_new())
    //        {
    //            clsprc.Map_slno = Map_slno;
    //            clsprc.sop_id = sop_id;
    //            clsprc.Template = Template;
    //            clsprc.Group_Id = Group_Id;
    //            List<Class_sop_ProcessParameter_new> lst = new List<Class_sop_ProcessParameter_new>().Where(x => x.Map_slno == clsprc.Map_slno && x.sop_id == clsprc.sop_id && x.Template == clsprc.Template && x.Group_Id == clsprc.Group_Id).ToList();
    //            if (lst.Count > 0)
    //                crud.Update(clsprc);
    //            else
    //                crud.Insert(clsprc);
    //        }
    //    }
    //}
    void SaveToolings(int Group_Id, int sop_id, string Template, int Map_slno)
    {
        string qrydel = "Delete from sop_toolings_new where Map_Slno=" + Map_slno + " and  sop_id=" + sop_id + "   and Template='" + Template + "'  and  Group_Id=" + Group_Id;
        Class_sop_toolings_new clstool = new Class_sop_toolings_new();
        int flag1 = 0;
        foreach (GridViewRow row in GridView1.Rows)
        {
            TextBox txtOperation = (TextBox)row.FindControl("txtOperation");
            TextBox txtHolderName = (TextBox)row.FindControl("txtHolderName");
            TextBox txtToolInsert = (TextBox)row.FindControl("txtToolInsert");
            TextBox txtCuttingSpeed = (TextBox)row.FindControl("txtCuttingSpeed");
            TextBox txtFeed = (TextBox)row.FindControl("txtFeed");
            TextBox txtToolLifePerCorner = (TextBox)row.FindControl("txtToolLifePerCorner");
            TextBox txtToolLifeNoOfCorner = (TextBox)row.FindControl("txtToolLifeNoOfCorner");
            TextBox txtToolLifeTotal = (TextBox)row.FindControl("txtToolLifeTotal");
            TextBox txtControlMethod = (TextBox)row.FindControl("txtControlMethod");
            if (!string.IsNullOrWhiteSpace(txtOperation.Text) ||
                !string.IsNullOrWhiteSpace(txtHolderName.Text) ||
                !string.IsNullOrWhiteSpace(txtToolInsert.Text) ||
                !string.IsNullOrWhiteSpace(txtCuttingSpeed.Text) ||
                !string.IsNullOrWhiteSpace(txtFeed.Text) ||
                !string.IsNullOrWhiteSpace(txtToolLifePerCorner.Text) ||
                !string.IsNullOrWhiteSpace(txtToolLifeNoOfCorner.Text) ||
                !string.IsNullOrWhiteSpace(txtToolLifeTotal.Text) ||
                !string.IsNullOrWhiteSpace(txtControlMethod.Text))
            {
                flag1 = 1;
                break;
            }
        }

        if (flag1 == 1)
        {
            using (Crud_sop_toolings_new crud = new Crud_sop_toolings_new())
            {
                clstool.Map_Slno = Map_slno;
                clstool.sop_id = sop_id;
                clstool.Template = Template;
                clstool.Group_Id = Group_Id;
                Database db = new Database("connString");

                List<Class_sop_toolings_new> lst = new List<Class_sop_toolings_new>();
                lst = db.Fetch<Class_sop_toolings_new>().Where(x => x.Map_Slno == clstool.Map_Slno && x.sop_id == clstool.sop_id && x.Template == clstool.Template && x.Group_Id == clstool.Group_Id).ToList();
                if (lst.Count > 0)
                {
                    // Database db = new Database("connString");
                    db.Execute(qrydel);
                    //    db.Delete<Class_sop_toolings_new>("Delete from sop_toolings_new where Map_Slno=@0 and  sop_id=@1  and Template=@2  and  Group_Id=@3 ", Map_slno, sop_id, Template, Group_Id);

                }
                foreach (GridViewRow row in GridView1.Rows)
                {
                    TextBox txtOperation = (TextBox)row.FindControl("txtOperation");
                    TextBox txtHolderName = (TextBox)row.FindControl("txtHolderName");
                    TextBox txtToolInsert = (TextBox)row.FindControl("txtToolInsert");
                    TextBox txtCuttingSpeed = (TextBox)row.FindControl("txtCuttingSpeed");
                    TextBox txtFeed = (TextBox)row.FindControl("txtFeed");
                    TextBox txtToolLifePerCorner = (TextBox)row.FindControl("txtToolLifePerCorner");
                    TextBox txtToolLifeNoOfCorner = (TextBox)row.FindControl("txtToolLifeNoOfCorner");
                    TextBox txtToolLifeTotal = (TextBox)row.FindControl("txtToolLifeTotal");
                    TextBox txtControlMethod = (TextBox)row.FindControl("txtControlMethod");

                    clstool.Operation = txtOperation.Text;
                    clstool.tool_holder_name = txtHolderName.Text;
                    clstool.tool = txtToolInsert.Text;
                    clstool.feed_rate = txtFeed.Text;
                    clstool.cutting_speed = txtCuttingSpeed.Text;
                    clstool.per_corner = txtToolLifePerCorner.Text;
                    clstool.no_of_corners = txtToolLifeNoOfCorner.Text;
                    clstool.total_nos = txtToolLifeTotal.Text;
                   // int perCorner;
                    //if (!int.TryParse(txtToolLifePerCorner.Text, out perCorner))
                    //{
                    //    perCorner = 0; // Set to 0 if invalid input
                    //}
                    //clstool.per_corner = perCorner;

                    //// Attempt to parse txtToolLifeNoOfCorner value
                    //int noOfCorners;
                    //if (!int.TryParse(txtToolLifeNoOfCorner.Text, out noOfCorners))
                    //{
                    //    noOfCorners = 0; // Set to 0 if invalid input
                    //}
                    //clstool.no_of_corners = noOfCorners;

                    //// Attempt to parse txtToolLifeTotal value
                    //int totalNos;
                    //if (!int.TryParse(txtToolLifeTotal.Text, out totalNos))
                    //{
                    //    totalNos = 0; // Set to 0 if invalid input
                    //}
                    //clstool.total_nos = totalNos;
                    clstool.control_method = txtControlMethod.Text;


                    if (string.IsNullOrWhiteSpace(txtOperation.Text) &&
               string.IsNullOrWhiteSpace(txtHolderName.Text) &&
               string.IsNullOrWhiteSpace(txtToolInsert.Text) &&
               string.IsNullOrWhiteSpace(txtCuttingSpeed.Text) &&
               string.IsNullOrWhiteSpace(txtFeed.Text) &&
               string.IsNullOrWhiteSpace(txtToolLifePerCorner.Text) &&
               string.IsNullOrWhiteSpace(txtToolLifeNoOfCorner.Text) &&
               string.IsNullOrWhiteSpace(txtToolLifeTotal.Text) &&
               string.IsNullOrWhiteSpace(txtControlMethod.Text))
                    { }
                    else
                        crud.Insert(clstool);
                }
            }
        }
    }


    protected void GridViewprc_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        // Update serial numbers after row deletion
        UpdateSerialNumbers();
    }

    private void UpdateSerialNumbers()
    {
        int serialNumber = 1;
        foreach (GridViewRow row in GridViewprc.Rows)
        {
            Label lblSerialNumber = (Label)row.FindControl("lblSerialNumber");
            if (lblSerialNumber != null)
            {
                lblSerialNumber.Text = serialNumber.ToString();
                serialNumber++;
            }
        }
    }

}
