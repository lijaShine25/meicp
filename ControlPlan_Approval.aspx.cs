using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using NPoco;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using DataBinder = System.Web.UI.DataBinder;
using System.Windows.Forms;

public partial class ControlPlan_Approval : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo == null)
        {
            Response.Redirect("~/LoginPage.aspx");
        }
        int em = GetAccessUsers();
        if (userInfo[0].isAdmin != "Y" && em <= 0)
        {
            Response.Redirect("~/AccessDenied.aspx");
        }

        if (!Page.IsPostBack)
        {

            LoadParts();
            LoadGridData();

            if (userInfo[0].isAdmin == "Y" && em <= 0)
            {
                btnSubmit.Enabled = false;
            }
            else if (em > 0)
            {
                btnSubmit.Enabled = true;
            }
            if (userInfo[0].isAdmin == "Y")
                btnviewall.Enabled = true;
            else btnviewall.Enabled = false;
        }
    }
    protected int GetAccessUsers()
    {
        string sql = "select EmployeeSlNo from Employees where EmployeeSlNo in(select distinct approvedby from parts where  Obsolete='N'  and approvedby<>0 and approvedby is not null)  and EmployeeSlNo=@0";

        Database db = new Database("connString");
        int emp = db.ExecuteScalar<int>(sql, userInfo[0].EmployeeSlNo);
        return emp;
    }

    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData();
        ddlpart_slno.SelectedIndex = 0;
        LoadGridData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
        //if (userInfo[0].isAdmin == "Y")
        //{
        //    grdData.Columns[9].Visible = true; btnSubmit.Enabled = true;
        //}
    }

    void SaveData()
    {
        try
        {
            foreach (GridViewRow row in grdData.Rows)
            {
                // Find the checkbox and label controls in the current row
                System.Web.UI.WebControls.CheckBox chkSelect = row.FindControl("chkSelect") as System.Web.UI.WebControls.CheckBox;
                System.Web.UI.WebControls.Label cp_slno = row.FindControl("CPSlNo") as System.Web.UI.WebControls.Label;

                // Check if the checkbox is checked and retrieve the label value
                if (chkSelect != null && (cp_slno != null && cp_slno.Text != "0") && chkSelect.Checked)
                {
                    string slnoValue = cp_slno.Text;
                    string qry = "update controlPlan set Submitstatus='A' , is_approved='1',CP_Approved_DateTime=@0 where cp_slno=@1";
                    Database db = new Database("connString");
                    {
                        db.Execute(qry,DateTime.Now, Convert.ToInt32(slnoValue));

                    }
                }

            }
            ShowMessage("Data Updated Successfully!!", MessageType.Success);
        }
        catch (Exception e)
        {
            ShowMessage(e.InnerException.ToString(), MessageType.Info);
        }
    }
    protected void btnFilder_Click(object sender, EventArgs e)
    {
        LoadGridData();
    }
    protected void btnviewall_Click(object sender, EventArgs e)
    {
        Response.Redirect("ControlPlan_Approval_Admin.aspx");
    }

    void ClearData()
    {
        ddlpart_slno.SelectedIndex = 0;
        LoadGridData();
    }
    void LoadGridData()
    {
        Crud_PartsMapping crud = new Crud_PartsMapping();
        List<Class_PartsMapping> lst = null;
        List<Class_PartsMapping> lst1 = crud.usp_PartsMappingSelect_CPApproval().ToList();
        if (ddlpart_slno.SelectedIndex > 0)
        {
            List<Class_PartsMapping> lst2 = lst1.Where(x => x.part_slno == Convert.ToInt16(ddlpart_slno.SelectedValue) && x.approvedby == userInfo[0].EmployeeSlNo).ToList();
            lst = lst2;

        }
        else
            lst = lst1.Where(x => x.approvedby == userInfo[0].EmployeeSlNo).ToList();

        grdData.DataSource = lst;
        grdData.DataBind();

    }

    protected void grdData_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Assuming cpdone is a column index, adjust it if needed
            int cpdoneColumnIndex = 5; // Index of the "Is Control Plan Done" column
            int chkindex = 9;
            // Find the control in the specified column
            System.Web.UI.WebControls.Label lblCpdone = (System.Web.UI.WebControls.Label)e.Row.Cells[cpdoneColumnIndex].FindControl("CPSlNo");
            System.Web.UI.WebControls.CheckBox chkAll = (System.Web.UI.WebControls.CheckBox)e.Row.Cells[chkindex].FindControl("chkSelect");
            TableCell cell = e.Row.Cells[8];

            // Get the text (value) of the cell
            string isApproved = cell.Text;
            // Check the value of cpdone and set row color accordingly
            if (lblCpdone.Text == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Orange;
                chkAll.Enabled = false;
            }
            if (isApproved == "Approved")
                chkAll.Enabled = false;
        }
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.CheckBox chkAll = (System.Web.UI.WebControls.CheckBox)grdData.HeaderRow.FindControl("chkSelectAll");

        foreach (GridViewRow row in grdData.Rows)
        {
            System.Web.UI.WebControls.CheckBox chkRow = (System.Web.UI.WebControls.CheckBox)row.FindControl("chkSelect");
            System.Web.UI.WebControls.Label lblCpdone = (System.Web.UI.WebControls.Label)row.FindControl("CPSlNo");
            TableCell cell = row.Cells[8];

            // Get the text (value) of the cell
            string isApproved = cell.Text;
            // Check the value of cpdone and set row color accordingly
            if (lblCpdone.Text != "0" && isApproved != "Approved")
            {
                chkRow.Checked = chkAll.Checked;
            }
        }
    }
    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        // List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        List<Class_parts> lst1 = crud.usp_partsSelect_mapping().ToList();
        List<Class_parts> lst = lst1.Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N" && x.approvedBy == userInfo[0].EmployeeSlNo).ToList();
        ddlpart_slno.Items.Clear();
        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
        }
        ddlpart_slno.Items.Insert(0, "Select...");
    }



}