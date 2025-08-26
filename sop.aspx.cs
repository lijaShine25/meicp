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

public partial class sop : System.Web.UI.Page
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

        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanPrepare != "Y" && userInfo[0].CanApprove != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }

        if (!Page.IsPostBack)
        {
            LoadParts();

            if (Request.QueryString.HasKeys())
            {

                if (userInfo[0].isAdmin == "Y" || userInfo[0].CanApprove == "Y")
                {
                    btnApproved.Enabled = true;
                }
                else
                {
                    btnApproved.Enabled = false;
                }

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
                hdnEditMode.Value = "E";
                GetDetails();


            }
            else
            {
                btnDelete.Enabled = false;
            }
        }
    }

    protected void delFile1_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile1.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile1.Text = "";
            SaveData();
        }
    }

    protected void delFile2_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/SOP/" + lblFile2.Text));
        if (f.Exists)
        {
            f.Delete();
            lblFile2.Text = "";
            SaveData();
        }
    }

    protected void btnApproved_Click(object sender, EventArgs e)
    {

        using (Database db = new Database("connString"))
        {
            Class_sop_header c = db.SingleOrDefaultById<Class_sop_header>(hdnSlNo.Value);
            c.submitstatus = "A";
            c.is_approved = true;
            db.Update(c);

            ShowMessage("Approved", MessageType.Info);
            ClearData();
        }
    }

    void GetDetails()
    {
        Crud_sop_header crud = new Crud_sop_header();
        Class_sop_header clshdr = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
        // Class_sop_toolings clstoolings = null;
        LoadParts();
        ddlpart_slno.SelectedValue = clshdr.part_slno.ToString();
        hdnprevoprnslno.Value = clshdr.prev_oprn_slno.ToString();
        hdnnextoprnslno.Value = clshdr.next_oprn_slno.ToString();
        ddlcpType.SelectedValue = clshdr.cp_type;
        LoadOperations();
        ddloperation_slno.SelectedValue = clshdr.operation_slno.ToString();
        LoadMachinesFromparts();
        ddlmachine_slno.SelectedValue = clshdr.machine_slno.ToString();


        txtobjective.Text = clshdr.objective;
        lblFile1.Text = clshdr.before_machine_filename;
        lblFile2.Text = clshdr.after_machine_filename;
        txtoprninstruction.Text = clshdr.oprn_instruction;
        txtochkpoints.Text = clshdr.checkpoints_list;
        txtworkholding.Text = clshdr.workholding_method;
        txtfirstoff.Text = clshdr.firstoff_approval;
        txtcoolant.Text = clshdr.coolant_used;
        txtreacionplan.Text = clshdr.reaction_plan;
        txtnotes.Text = clshdr.notes;
        hrefFile1.HRef = "~/Documents/SOP/" + clshdr.before_machine_filename;
        hrefFile2.HRef = "~/Documents/SOP/" + clshdr.after_machine_filename;
        imgFile1.ImageUrl = hrefFile1.HRef;
        imgFile2.ImageUrl = hrefFile2.HRef;
        if (imgFile1.ImageUrl == "~/Documents/SOP/")
        { imgFile1.ImageUrl = "~/dist/img/boxed-bg.jpg"; }
        if (imgFile2.ImageUrl == "~/Documents/SOP/")
        { imgFile2.ImageUrl = "~/dist/img/boxed-bg.jpg"; }
        //hdnsubmitstatus.Value = clshdr.submitstatus;if (hdnsubmitstatus.Value == "A" & clshdr.is_approved == true)
        //{ btnirev.Enabled = true; }
        //else
        //{ btnirev.Enabled =false; }

        LoadProcessGrid();
        LoadQualityGrid();
        if (ddloperation_slno.SelectedItem.Text == "Painting" || ddloperation_slno.SelectedItem.Text == "Induction Hardening")
        {
            divgridprocess.Visible = true;
            divHandsonProcess.Visible = false;
        }
        else
        {
            divgridprocess.Visible = false;
            divHandsonProcess.Visible = true;
        }
    }
    void LoadProcessGrid()
    {
        string qry = "select ch.*,e.evalTech,f.FreqDesc,m.MethodDesc from ControlPlan cp inner join ControlPlan_Child ch on ch.[cp_slno]=cp.[cp_slno] inner join EvaluationTech e on e.evalTech_slno = ch.evalTech_slno " +
            "  inner join SampleFrequency f on f.freq_slno=ch.freq_slno  inner Join ControlMethods m on m.method_slno=ch.method_slno  where cp.part_slno= " + ddlpart_slno.SelectedValue + " and operation_slno=" + ddloperation_slno.SelectedValue + " and ((ch.process_char<>'' or ch.process_char is not null  or ch.process_char<>'-' ) and (ch.process_char not like '%tool%' or ch.process_char not like '%Tool%') and (ch.product_char='' or ch.product_char is null  or ch.product_char='-'))";
        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan_Child> x = db.Query<Class_ControlPlan_Child>(qry).ToList();

            if (x.Count > 0)
            {
                grdProcessParam.DataSource = x;
                grdProcessParam.DataBind();
            }
            else
            {

                grdProcessParam.DataSource = null;
                grdProcessParam.DataBind();
            }
        }
    }
    protected void btnrevision_Click(object sender, EventArgs e)
    {

    }

    void LoadQualityGrid()
    {
        string qry = "select ch.*,e.evalTech,Concat(ch.SampleSize,'/',f.FreqDesc) as FreqDesc,m.MethodDesc,sp.splCharFile as splfilename from ControlPlan cp inner join ControlPlan_Child ch on ch.[cp_slno]=cp.[cp_slno] inner join EvaluationTech e on e.evalTech_slno = ch.evalTech_slno2 " +
            "  inner join SampleFrequency f on f.freq_slno=ch.freq_slno2  inner Join ControlMethods m on m.method_slno=ch.method_slno2  inner join  SpecialChars sp on sp.splChar_slno=ch.splChar_slno where cp.part_slno= " + ddlpart_slno.SelectedValue + " and operation_slno=" + ddloperation_slno.SelectedValue + " and (ch.product_char<>'' or ch.product_char is not null  or ch.product_char<>'-')  and m.MethodDesc in('PMC','PCC')";
        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan_Child> x = db.Query<Class_ControlPlan_Child>(qry).ToList();

            if (x.Count > 0)
            {
                grdQualityChar.DataSource = x;
                grdQualityChar.DataBind();
            }
            else
            {

                grdQualityChar.DataSource = null;
                grdQualityChar.DataBind();
            }
        }
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
                var x = db.Query<Class_operations>("select * FROM operations o,partsmapping p where p.operation_slno=o.operation_slno AND o.del_status = 'N' AND p.part_slno=@0", Convert.ToInt32(ddlpart_slno.SelectedValue));
                ddloperation_slno.Items.Clear();
                ddloperation_slno.DataSource = x;
                ddloperation_slno.DataTextField = "OperationDesc";
                ddloperation_slno.DataValueField = "operation_slno";
                ddloperation_slno.DataBind();

                ddloperation_slno.Items.Insert(0, new ListItem("Select", "0"));

                // load control plan type
                string cpType = db.ExecuteScalar<string>("select cptype from parts where part_slno=@0", Convert.ToInt16(ddlpart_slno.SelectedValue));
                ddlcpType.SelectedValue = cpType;
                ddlcpType.Enabled = false;
                //hdnCpType.Value = cpType;
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

    protected void ddlpart_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // load rev no details
        using (Crud_parts crud = new Crud_parts())
        {
            Class_parts cls = crud.SelectOne(Convert.ToInt32(ddlpart_slno.SelectedValue));
            hdnrevno.Value = cls.partIssueNo;
        }
        LoadOperationsFromparts();

    }

    protected void ddloperation_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMachinesFromparts();
        string sql = "Select cp.*,op.operationdesc from Controlplan cp inner join operations op on op.operation_slno=cp.operation_slno  where cp.part_slno=@0  order by cast(cp.process_no as int)";
        List<Class_ControlPlan> lst = new List<Class_ControlPlan>();
        using (Database db = new Database("connString"))
        {

            lst = db.Query<Class_ControlPlan>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue)).ToList();

            int index = lst.FindIndex(item => item.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue));
            if (index - 1 >= 0)
            {
                int prevslno = lst[index - 1].operation_slno;
                string previous = lst[index - 1].operationdesc;
                txtprevoprn.Text = previous;
                hdnprevoprnslno.Value = prevslno.ToString();
            }
            else
            {
                txtprevoprn.Text = string.Empty;
                hdnprevoprnslno.Value = "";
            }
            if (index + 1 < lst.Count)
            {
                string next = lst[index + 1].operationdesc;
                int nextslno = lst[index + 1].operation_slno;
                txtnextoprn.Text = next;
                hdnnextoprnslno.Value = nextslno.ToString();
            }
            else
            {
                txtnextoprn.Text = string.Empty;
                hdnnextoprnslno.Value = "";
            }



        }
        if (ddloperation_slno.SelectedItem.Text == "Painting" || ddloperation_slno.SelectedItem.Text == "Induction Hardening")
        {
            divgridprocess.Visible = true;
            divHandsonProcess.Visible = false;
        }
        else {
            divgridprocess.Visible = false;
            divHandsonProcess.Visible = true;
        }
    }


    protected void btnCP_Click(object sender, EventArgs e)
    {
        LoadProcessGrid();
        LoadQualityGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        hdnsubmitstatus.Value = "N";
        SaveData();
        ClearData();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {


        using (Database db = new Database("connString"))
        {
            Class_sop_header c = db.SingleOrDefaultById<Class_sop_header>(hdnSlNo.Value);
            c.submitstatus = "Y";
            db.Update(c);

            ShowMessage("Updated", MessageType.Info);
            ClearData();
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
        imgFile1.ImageUrl = string.Empty;
        imgFile2.ImageUrl = string.Empty;
        //  hdnchild.Value= string.Empty;
        hdnSlNo.Value = "0";
        LoadProcessGrid();
        LoadQualityGrid();
    }
    void DeleteData()
    {
        using (Database db = new Database("connString"))
        {
            Crud_sop_header crud = new Crud_sop_header();
            Class_sop_header clshdr = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));

            int partslno = 0;
            int opernslno = 0;
            int machineslno = 0;


            partslno = clshdr.part_slno;
            opernslno = clshdr.operation_slno;
            machineslno = clshdr.machine_slno;


            //Delete control plan
            db.DeleteWhere<Class_sop_toolings>(" sop_id=@0", Convert.ToInt32(hdnSlNo.Value));
            db.DeleteWhere<Class_sop_header>(" sop_id=@0", Convert.ToInt32(hdnSlNo.Value));

            // set Obsolete ='N' for previous revision cp

            string qryupdate = "update sop_header set Obsolete ='N', Submitstatus='N' where part_slno=" + partslno + " and operation_slno=" + opernslno + "  and  machine_slno=" + machineslno + " and "
                               + " rev_no =(SELECT MAX(c.rev_no) from sop_header c where c.part_slno=sop_header.part_slno AND c.operation_slno=sop_header.operation_slno)";

            db.Execute(qryupdate);
        }
    }

    void SaveData()
    {
        Class_sop_toolings clstooling = new Class_sop_toolings();
        Class_sop_ProcessParameter clsprc = new Class_sop_ProcessParameter();
        string slno = "";
        using (Class_sop_header clshdr = new Class_sop_header())
        {
            clshdr.part_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
            clshdr.operation_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);

            clshdr.machine_slno = Convert.ToInt32(ddlmachine_slno.SelectedValue);
            if (!string.IsNullOrEmpty(hdnprevoprnslno.Value))
                clshdr.prev_oprn_slno = Convert.ToInt32(hdnprevoprnslno.Value);
            if (!string.IsNullOrEmpty(hdnnextoprnslno.Value))
                clshdr.next_oprn_slno = Convert.ToInt32(hdnnextoprnslno.Value);
            clshdr.objective = txtobjective.Text;
            clshdr.before_machine_filename = "";
            clshdr.after_machine_filename = "";
            clshdr.oprn_instruction = txtoprninstruction.Text;
            clshdr.checkpoints_list = txtochkpoints.Text;
            clshdr.workholding_method = txtworkholding.Text;
            clshdr.firstoff_approval = txtfirstoff.Text;
            clshdr.coolant_used = txtcoolant.Text;
            clshdr.reaction_plan = txtreacionplan.Text;
            clshdr.notes = txtnotes.Text;
            clshdr.cp_type = ddlcpType.SelectedValue;
            string savePath = Server.MapPath("~/Documents/SOP/");

            HttpPostedFile File1 = FileUpload1.PostedFile;
            HttpPostedFile File2 = FileUpload2.PostedFile;
            if (File1 != null && File1.ContentLength > 0)
            {
                try
                {
                    string fileName1 = Path.GetFileName(File1.FileName);
                    clshdr.before_machine_filename = fileName1;

                    // Save the file to a specified path

                    string filePath = Path.Combine(savePath, fileName1);
                    File1.SaveAs(filePath);

                    // Display a success message or perform any other actions you need
                    //lblFile1.Text = "File uploaded successfully.";
                    imgFile1.ImageUrl = "~/Documents/SOP/" + fileName1;
                    hrefFile1.HRef = "~/Documents/SOP/" + fileName1;
                }
                catch (Exception ex)
                {
                    // Handle errors, display an error message, log the error, etc.
                    lblFile1.Text = "Error uploading file: " + ex.Message;
                }

            }
            else if (!string.IsNullOrEmpty(lblFile1.Text))
            {
                clshdr.before_machine_filename = lblFile1.Text;

            }



            if (File2 != null && File2.ContentLength > 0)
            {
                try
                {
                    string fileName2 = Path.GetFileName(File2.FileName);
                    clshdr.after_machine_filename = fileName2;
                    // Save the file to a specified path

                    string filePath = Path.Combine(savePath, fileName2);
                    File1.SaveAs(filePath);

                    // Display a success message or perform any other actions you need
                    //lblFile2.Text = "File uploaded successfully.";
                    imgFile2.ImageUrl = "~/Documents/SOP/" + fileName2;
                    hrefFile2.HRef = "~/Documents/SOP/" + fileName2;
                }
                catch (Exception ex)
                {
                    // Handle errors, display an error message, log the error, etc.
                    lblFile2.Text = "Error uploading file: " + ex.Message;
                }

            }
            else if (!string.IsNullOrEmpty(lblFile2.Text))
            {
                clshdr.after_machine_filename = lblFile2.Text;

            }
            if (hdnEditMode.Value == "I")
            {
                clshdr.submitstatus = "N";
                using (Crud_sop_header crud = new Crud_sop_header())
                {
                    crud.Insert(clshdr);
                }
            }
            else
            {

                clshdr.sop_id = Convert.ToInt32(hdnSlNo.Value);
                using (Crud_sop_header crud = new Crud_sop_header())
                {
                    if (hdnsubmitstatus.Value == "Y")
                        clshdr.submitstatus = "Y";
                    else clshdr.submitstatus = "N";
                    crud.Update(clshdr);
                }
                using (var db = new Database("connString"))
                {
                    db.Delete<Class_sop_toolings>("where sop_id=@0", clshdr.sop_id);
                }
            }
            var cline = JsonConvert.DeserializeObject<List<Class_sop_toolings>>(hdnchild.Value);
            var clinePRC = JsonConvert.DeserializeObject<List<Class_sop_ProcessParameter>>(hdnchildPrc.Value);
            if (cline.Count > 0)
            {

                for (int i = 0; i < cline.Count; i++)
                {
                    if (cline[i].tool != null)
                    {

                        clstooling.sop_id = Convert.ToInt32(clshdr.sop_id);
                        clstooling.tool_holder_name = cline[i].tool_holder_name;
                        clstooling.tool = cline[i].tool;
                        clstooling.cutting_speed = cline[i].cutting_speed;
                        clstooling.feed_rate = cline[i].feed_rate;
                        clstooling.per_corner = cline[i].per_corner;
                        clstooling.no_of_corners = cline[i].no_of_corners;
                        clstooling.total_nos = cline[i].total_nos;
                        clstooling.control_method = cline[i].control_method;


                        using (Crud_sop_toolings crud = new Crud_sop_toolings())
                        {
                            crud.Insert(clstooling);
                        }
                    }
                }

            }
            if (clinePRC.Count > 0)
            {

                for (int i = 0; i < clinePRC.Count; i++)
                {
                    if (clinePRC[i].ProcessParameter != null)
                    {

                        clsprc.sop_id = Convert.ToInt32(clshdr.sop_id);
                        clsprc.ProcessParameter = clinePRC[i].ProcessParameter;
                        clsprc.Specification = clinePRC[i].Specification;
                        clsprc.CheckingMethod = clinePRC[i].CheckingMethod;
                        clsprc.Frequency = clinePRC[i].Frequency;
                        clsprc.ToolOfControl = clinePRC[i].ToolOfControl;                      
                        using (Crud_sop_ProcessParameter crud = new Crud_sop_ProcessParameter())
                        {
                            crud.Insert(clsprc);
                        }
                    }
                }

            }

        }
    }
}
