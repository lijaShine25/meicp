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
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using System.Web.UI.WebControls.WebParts;
using Syncfusion.JavaScript.DataVisualization.Models.Diagram;
using System.Text.RegularExpressions;
using System.Text;
using Syncfusion.JavaScript.Models;
using Syncfusion.Pdf.Parsing;
using System.IO.Packaging;
using Syncfusion.XlsIO;
using Image = System.Drawing.Image;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;


public partial class sop_mapping : System.Web.UI.Page
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
            LoadGrid();
            LoadEmployees();
            if (Request.QueryString.HasKeys())
            {
                btnDelete.Enabled = true;
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnEditMode.Value = "E";
                GetDetails();

                //if (CheckIfGenerateReport())
                //{ btnReport.Enabled = false; }
                //else { btnReport.Enabled = true; }

            }
            else
            {
                btnDelete.Enabled = false;
                btnirev.Enabled = false;
                btnSubmit.Enabled = false;
                btnApprove.Enabled = false;
            }

        }
    }
    void LoadEmployees()
    {
        string sql = "select * from Employees where del_status='N'";
        using (Database db = new Database("connString"))
        {
            List<Class_Employees> lstPrep = db.Fetch<Class_Employees>(sql).Where(x => x.CanPrepare == "Y").ToList();
            List<Class_Employees> lstAppr = db.Fetch<Class_Employees>(sql).Where(x => x.CanApprove == "Y").ToList();
            ddlPreparedBy.Items.Clear();
            ddlPreparedBy.DataSource = lstPrep;
            ddlPreparedBy.DataValueField = "employeeslno";
            ddlPreparedBy.DataTextField = "employeename";
            ddlPreparedBy.DataBind();

            ddlApprovedBy.Items.Clear();
            ddlApprovedBy.DataSource = lstAppr;
            ddlApprovedBy.DataValueField = "employeeslno";
            ddlApprovedBy.DataTextField = "employeename";
            ddlApprovedBy.DataBind();

            ddlPreparedBy.Items.Insert(0, new ListItem("Select", "0"));
            ddlApprovedBy.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void GetDetailsOld()
    {
        List<Class_sop_mapping> lst = new List<Class_sop_mapping>();
        List<Class_sop_header_new> lsthdr = new List<Class_sop_header_new>();
        List<Class_DCR_SOP> lstDCR = new List<Class_DCR_SOP>();
        Database db = new Database("connString");

        lst = db.Fetch<Class_sop_mapping>()
                .Where(x => x.Group_Id == Convert.ToInt32(hdnSlNo.Value) && x.del_status == "N" && x.Obsolete == "N")
                .ToList();

        lsthdr = db.Fetch<Class_sop_header_new>()
                   .Where(x => x.Group_Id == Convert.ToInt32(hdnSlNo.Value) && x.is_obsolete == false)
                   .ToList();
        hdnrevreason.Value = lst[0].rev_reason;

        hdnrevnumber.Value = lst[0].rev_no.ToString();
        hdnrevdt.Value = lst[0].rev_date;
        // Check if all records in lst have SubmitStatus = "Y"
        bool allSubmitted = lst.All(x => x.SubmitStatus == "Y");

        if (allSubmitted)
        {
            btnSubmit.Enabled = false;
            btnSave.Enabled = false;

            if (lsthdr.Count == 0)
            {
                hdnapprovestatus.Value = "N";
                // If no records in lsthdr, disable Approve button
                btnApprove.Enabled = false;
                btnrevision.Enabled = false;
            }
            else
            {
                // Check if all records in lsthdr have SubmitStatus = "Y" and is_approved = 0
                bool allSubmittedAndNotApproved = lsthdr.All(x => x.submitstatus == "Y" && !x.is_approved);

                if (allSubmittedAndNotApproved && (userInfo[0].EmployeeSlNo.ToString() == ddlApprovedBy.SelectedValue || userInfo[0].isAdmin == "Y"))
                {
                    hdnapprovestatus.Value = "N";
                    btnApprove.Enabled = true;
                    btnSubmit.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    // If any record does not have SubmitStatus = "Y" OR is already approved, disable Approve button
                    btnApprove.Enabled = false;

                }
            }
        }
        else
        {
            // If not all records in lst have SubmitStatus = "Y", enable Save & Submit buttons
            btnSubmit.Enabled = true;
            btnSave.Enabled = true;
            btnApprove.Enabled = false; // Ensure Approve is disabled in this case
        }


        lstDCR = db.Fetch<Class_DCR_SOP>()
               .Where(x => x.Group_Id == Convert.ToInt32(hdnSlNo.Value) && x.del_status == "N" && x.Submit_Status == "A")
               .ToList();

        if (lstDCR.Count > 0) // If Active DCR exists
        {
            bool allSopApproved = lsthdr.All(x => x.is_approved); // Check if all SOPs are approved
            if (lsthdr.Count == 0)
            {

                btnrevision.Enabled = false;
            }
            else
            {
                if (allSopApproved)
                {
                    btnirev.Enabled = true;  // ✅ Enable revision button only if all SOPs are approved
                }
                else
                {
                    btnirev.Enabled = false; // ❌ Disable revision button if any SOP is not approved
                }
            }
        }
        else
        {
            btnirev.Enabled = false; // No Active DCR, disable btnirev
        }
        // Populate UI fields
        txtGroupName.Text = lst[0].Group_Name;
        ddlTemplate.SelectedValue = lst[0].Template;
        txtFormatNo.Text = lst[0].Format_No;
    }


    protected void GetDetails()
    {
        List<Class_sop_mapping> lst = new List<Class_sop_mapping>();
        List<Class_sop_header_new> lsthdr = new List<Class_sop_header_new>();
        List<Class_DCR_SOP> lstDCR = new List<Class_DCR_SOP>();
        Database db = new Database("connString");

        int groupId = Convert.ToInt32(hdnSlNo.Value);

        // Step 1: Get the maximum rev_no for the given Group_Id
        int maxRev = db.Fetch<Class_sop_mapping>()
                       .Where(x => x.Group_Id == groupId && x.del_status == "N" && x.Obsolete == "N")
                       .Max(x => x.rev_no); // Get latest revision

        // Step 2: Fetch records from sop_mapping with max rev_no
        lst = db.Fetch<Class_sop_mapping>()
                .Where(x => x.Group_Id == groupId && x.del_status == "N" && x.Obsolete == "N" && x.rev_no == maxRev)
                .ToList();

        // Ensure lst is not empty before accessing elements
        if (lst.Count == 0)
            return; // No records found, exit the function

        // Step 3: Extract map_slno values from lst
        List<int> mapSlNos = lst.Select(x => x.Map_slno).ToList();

        // Step 4: Fetch data from sop_header_new using map_slno
        lsthdr = db.Fetch<Class_sop_header_new>()
                   .Where(x => mapSlNos.Contains(x.Map_slno) && !x.is_obsolete)
                   .ToList();

        // Assign values
        hdnrevreason.Value = lst[0].rev_reason;
        hdnrevnumber.Value = lst[0].rev_no.ToString();
        hdnrevdt.Value = lst[0].rev_date;
        hdndcr_slno.Value = lst[0].dcr_slno.ToString();

        LoadEmployees();
        ddlPreparedBy.SelectedValue = lst[0].PreparedBy.ToString();
        ddlApprovedBy.SelectedValue = lst[0].ApprovedBy.ToString();
        ddlActive.SelectedValue = lst[0].Active;

        txtRevNo.Text = lst[0].rev_no.ToString();
        txtrevdt.Text = lst[0].rev_date;
        txtchangenature.Text = lst[0].nature_of_Change;
        txtreasonforchange.Text = lst[0].reason_For_Change;


        // Check if all records in lst have SubmitStatus = "Y"
        bool allSubmitted = lst.All(x => x.SubmitStatus == "Y");

        if (allSubmitted)
        {
            btnSubmit.Enabled = false;
            btnSave.Enabled = false;

            if (lsthdr.Count == 0)
            {
                hdnapprovestatus.Value = "N";
                btnApprove.Enabled = false;
                btnrevision.Enabled = false;
            }
            else
            {
                // Check if all records in lsthdr have SubmitStatus = "Y" and is_approved = 0
                bool allSubmittedAndNotApproved = lsthdr.All(x => x.submitstatus == "Y" && !x.is_approved);

                if (allSubmittedAndNotApproved && (userInfo[0].EmployeeSlNo.ToString()==ddlApprovedBy.SelectedValue || userInfo[0].isAdmin=="Y"))
                {
                    hdnapprovestatus.Value = "N";
                    btnApprove.Enabled = true;
                    btnSubmit.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    btnApprove.Enabled = false;
                }
            }
        }
        else
        {
            btnSubmit.Enabled = true;
            btnSave.Enabled = true;
            btnApprove.Enabled = false;
        }

        // Fetch DCR records
        lstDCR = db.Fetch<Class_DCR_SOP>()
                   .Where(x => x.Group_Id == groupId && x.del_status == "N" && x.Submit_Status == "A")
                   .ToList();

        if (lstDCR.Count > 0) // If Active DCR exists
        {
            bool allSopApproved = lsthdr.All(x => x.is_approved);
            if (lsthdr.Count == 0)
            {
                btnrevision.Enabled = false;
            }
            else
            {
                btnirev.Enabled = allSopApproved; // Enable revision only if all SOPs are approved
            }
        }
        else
        {
            btnirev.Enabled = false;
        }

        // Populate UI fields
        txtGroupName.Text = lst[0].Group_Name;
        ddlTemplate.SelectedValue = lst[0].Template;
        txtFormatNo.Text = lst[0].Format_No;
        txtqualityChar.Text = lst[0].Qual_Char;


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
    protected void btnrevision_Click(object sender, EventArgs e)
    {
        txtUserRevNo.ReadOnly = false; txtUserRevDt.ReadOnly = false; txtreason.ReadOnly = false; txtnatureofchange.ReadOnly = false;
        int cntformail = 0;
        //string opn = ddloperation_slno.SelectedValue.ToString();
        using (Crud_ControlPlan cd = new Crud_ControlPlan())
        {
            Database db = new Database("connString");
            string sqlupdateCP = "select dcr_slno from DCR_SOP   where Group_Id=" + Convert.ToInt32(hdnSlNo.Value) + " and Submit_status='A' and del_status='N'";
            int dcr = db.ExecuteScalar<int>(sqlupdateCP);
            txtUserRevNo.Text = (Convert.ToInt32(hdnrevnumber.Value) + 1).ToString();
            txtUserRevDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            int rowsAffected1 = db.Execute(";exec usp_InitiateSOPRevision @@Group_Id=@0,@@rev_reason=@1, @@rev_no=@2, @@rev_date=@3, @@dcr_slno= @4,@@Reason_For_Change=@5,@@nature_of_change=@6", Convert.ToInt32(hdnSlNo.Value), txtrevreason.Text, Convert.ToInt32(txtUserRevNo.Text), txtUserRevDt.Text, dcr,
                hdnreas.Value, hdnnature.Value);

            string sqlUpdateDCRStatus = "Update DCR_SOP_Status set status='closed' where dcr_slno=" + dcr + " and Group_Id=" + Convert.ToInt32(hdnSlNo.Value);
            db.Execute(sqlUpdateDCRStatus);
            string sqlDeleteDCR = "UPDATE DCR_SOP SET  submit_status='R' " +
                                     "WHERE Group_Id=@Group_Id " +
                                     "AND Submit_status='A' " +
                                     "AND del_status='N' " +
                                     "AND dcr_slno=@dcr";



            // string qryinsertrev = ("INSERT INTO part_revision_history (rev_no, rev_reasons, rev_date, part_slno, change_nature, revision_done_in ) VALUES(@0,@1,@2,@3,@4,@5)");

            var parameters = new
            {
                Group_Id = Convert.ToInt32(hdnSlNo.Value),
                dcr = dcr
            };

            int rowsAffected = db.Execute(sqlDeleteDCR, parameters);

        }
        using (Database db = new Database("connString"))
        {


            string sqlCC = @"select emailid from  employees where del_status='N' and isAdmin='Y'";
            //                string sql = @"select e.emailid
            //from parts p
            //inner join employees e on e.employeeslno=p.approvedBy
            //where p.part_slno=@0";

            // List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql, ddlpart_slno.SelectedValue);
            List<Class_Employees> lstemplCC = db.Fetch<Class_Employees>(sqlCC);
            List<string> tomails = new List<string>();
            //if (lstempl.Count > 0)
            //{
            //    foreach (Class_Employees c1 in lstempl)
            //    {
            //        tomails.Add(c1.EmailId);
            //    }
            //}
            //else
            //{
            tomails.Add(userInfo[0].EmailId);
            // }

            List<string> ccmails = new List<string>();
            if (lstemplCC.Count > 0)
            {
                foreach (Class_Employees c1 in lstemplCC)
                {
                    tomails.Add(c1.EmailId);
                }
            }
            ccmails.Add(userInfo[0].EmailId);
            tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
            ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));

            hdnrevnumber.Value = txtUserRevNo.Text;
            hdnrevreason.Value = txtrevreason.Text;
            hdnrevdt.Value = txtUserRevDt.Text;
            PrepareForMail(tomails, ccmails, "revision");



        }
        Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Revision Initiated');window.location='sop_mapping.aspx';", true);
    }

    private List<Class_operations> GetOperationData()
    {
        List<Class_operations> allop = GetAllOperations();
        List<Class_operations> filteredoperation = allop.Where(x => x.del_status == "N").ToList();
        return filteredoperation;
    }
    private List<Class_operations> GetAllOperations()
    {
        Database db = new Database("connString");
        List<Class_operations> alloperations = new List<Class_operations>();
        alloperations = db.Fetch<Class_operations>().ToList();
        return alloperations;
    }

    private List<Class_parts> GetPartData()
    {
        List<Class_parts> allParts = GetAllParts();
        List<Class_parts> filteredParts = allParts.Where(x => x.del_status == "N" && x.Obsolete != "Y").ToList();
        return filteredParts;
    }
    private List<Class_parts> GetAllParts()
    {
        Database db = new Database("connString");
        List<Class_parts> allParts = new List<Class_parts>();
        allParts = db.Fetch<Class_parts>().ToList();
        return allParts;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        SaveData("N");
        ClearData();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        SaveData("Y");
        ClearData();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteData();
        // ClearData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }
    //protected void btnQuery_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/sop_mapping_qry.aspx");
    //}
    void LoadGrid()
    {


        string qry = @"SELECT  DISTINCT Group_Id,Group_Name FROM SOP_Mapping where del_status='N' order by Group_Name asc";


        using (Database db = new Database("connString"))
        {
            List<Class_sop_mapping> lst = db.Query<Class_sop_mapping>(qry).ToList();

            if (lst.Count > 0)
            {
                grdData.DataSource = lst;
                grdData.DataBind();
            }
            else
            {
                grdData.DataSource = null;
                grdData.DataBind();
            }
        }
    }
    void ClearData()
    {
        Response.Redirect("~/sop_mapping.aspx");
    }
    void DeleteData()
    {
        using (Database db = new Database("connString"))
        {
            string sqlheader = "select * from sop_header_new where group_id=@0 ";

            List<Class_sop_header_new> lsthdr = db.Fetch<Class_sop_header_new>(sqlheader, Convert.ToInt32(hdnSlNo.Value));
            if (lsthdr.Count > 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Cannot delete if referred elsewhere.');", true);
            }

            else
            {
                db.DeleteWhere<Class_sop_mapping>(" group_id=@0", Convert.ToInt32(hdnSlNo.Value));
                // Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Data Deleted');", true);
                ClearData();
            }
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

        switch (mailFor)
        {
            case "revision":
                msg = "Revision is intiated for SOP. ";
                subject = "Revision Initiated for  SOP Group " + txtGroupName.Text + "(" + ddlTemplate.SelectedItem.Text + ")";
                break;
            case "for_approval":
                msg = "The below SOP is Submitted and pending for your approval. ";
                subject = "Approval Pending for  SOP Group " + txtGroupName.Text + "(" + ddlTemplate.SelectedItem.Text + ")";
                break;
            case "approved":
                msg = "The below SOP is Approved. ";
                subject = "SOP Approved for  SOP Group " + txtGroupName.Text + "(" + ddlTemplate.SelectedItem.Text + ")";
                break;

        }


        string styl = "<style>table{max-width:100%;background-color:transparent;font-size:14px}th{text-align:left}.table{width:100%;margin-bottom:20px}.table>tbody>tr>td,.table>tbody>tr>th,.table>tfoot>tr>td,.table>tfoot>tr>th,.table>thead>tr>td,.table>thead>tr>th{padding:8px;line-height:1.428571429;vertical-align:top;border-top:1px solid #ddd}.table>thead>tr>th{vertical-align:bottom;border-bottom:2px solid #ddd}.table>tbody+tbody{border-top:2px solid #ddd}.table .table{background-color:#fff}.table-striped>tbody>tr:nth-child(odd)>td,.table-striped>tbody>tr:nth-child(odd)>th{background-color:#f9f9f9}body{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:14px;line-height:1.428571429;color:#333;background-color:#fff}h2{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:500;line-height:1.1;color:inherit;text-align:center}</style>";

        string bodycnt = "<center><b><label style='background-color:#0198FF; color:White;font-family:Calibri;font-size:medium;'>&nbsp;THIS IS AN AUTOGENERATED MAIL. DO NOT REPLY TO THIS!! &nbsp;</label></b></center>" +
            "<body style='font-family:Calibri;font-size:medium;'>Dear Sir / Madam, <br/><br/>" + msg + "Please Login to the software and do the needful." +
            "<table class='table table-striped'>" +
            "<tr><th>Template</th><td>" + ddlTemplate.SelectedItem.Text + "</td></tr>" +
            "<tr><th>Group</Th><td>" + txtGroupName.Text + "</td></tr>" +
        "<tr><th>Document Number</Th><td>" + txtFormatNo.Text + "</td></tr>" +
        "<tr><th>Product Characteristics</Th><td>" + txtqualityChar.Text + "</td></tr>" +
        "<tr><th>Prepared By</Th><td>" + ddlPreparedBy.SelectedItem.Text + "</td></tr>" +
         " <tr><th>Revision No</th><td>" + txtRevNo.Text + "</td></tr>" +
           " <tr><th>Revision Date</th><td>" + txtrevdt.Text + "</td></tr>" +
             " <tr><th>Revision Reason</th><td>" + txtreasonforchange.Text + "</td></tr>";
             if(mailFor=="approved")
             { bodycnt= bodycnt+" <tr><th>Approved By</th><td>" + ddlApprovedBy.SelectedItem.Text + "</td></tr>"; }
       

        string mailBody = "<html><head>" + styl + "</head><body>" + bodycnt + "</body></html>";

        List<string> newToId = tomails.Distinct().ToList();
        List<string> newCcId = ccmails.Distinct().ToList();


        // send mail
        mail ml = new mail();
        ml.SendMail(toMailId: newToId, mailSubject: subject, bodyText: mailBody, ccMailId: newCcId);
    }
    void SaveData(string substatus)
    {
        Class_sop_mapping clsmap = new Class_sop_mapping();
        clsmap.SubmitStatus = substatus;
        clsmap.Obsolete = "N";
        clsmap.Group_Name = txtGroupName.Text;
        clsmap.Template = ddlTemplate.SelectedValue;
        clsmap.Format_No = txtFormatNo.Text;
        clsmap.Group_Id = (hdnEditMode.Value == "I") ? GenerateGroupId() : Convert.ToInt32(hdnSlNo.Value);
        clsmap.rev_no = 0;
        clsmap.Qual_Char = txtqualityChar.Text;
        if (!string.IsNullOrEmpty(hdndcr_slno.Value))
            clsmap.dcr_slno = Convert.ToInt32(hdndcr_slno.Value);
        clsmap.PreparedBy = Convert.ToInt32(ddlPreparedBy.SelectedValue);
        clsmap.ApprovedBy = Convert.ToInt32(ddlApprovedBy.SelectedValue);
        clsmap.Active = ddlActive.SelectedValue;
        if (!string.IsNullOrWhiteSpace(hdnchild.Value) && hdnchild.Value != "-1")
        {
            var cline = JsonConvert.DeserializeObject<List<Class_sop_mapping>>(hdnchild.Value);

            using (Database db1 = new Database("connString"))
            {
                if (hdnEditMode.Value == "I")
                {
                    if (!CheckIfExists())
                    {
                        foreach (var item in cline)
                        {
                            if (item.part_slno > 0)
                            {

                                clsmap.part_slno = item.part_slno;
                                clsmap.operation_slno = item.operation_slno;
                                clsmap.machine_slno = item.machine_slno;
                                clsmap.del_status = "N";
                                if (substatus == "Y")
                                { clsmap.SubmitStatus = "Y"; }
                                else { clsmap.SubmitStatus = "N"; }
                                clsmap.Obsolete = "N";
                                clsmap.rev_no = 0;
                                clsmap.rev_date = DateTime.Now.ToString("dd/MM/yyyy");
                                clsmap.nature_of_Change = "New Format Introduced - Operating Grouping SOP";
                                clsmap.rev_reason = "System Improvement";
                                clsmap.reason_For_Change = "System Improvement";

                                db1.Insert(clsmap);
                            }
                        }
                    }
                    else
                    {
                        ShowMessage("Group Name already Exists.", MessageType.Warning, false);
                        return;
                    }
                }
                else if (hdnEditMode.Value == "E")
                {
                    List<Class_sop_mapping> lsthdr = db1.Fetch<Class_sop_mapping>("SELECT * FROM sop_mapping WHERE Group_Id = @0   and rev_no=(SELECT MAX(rev_no) FROM SOP_Mapping WHERE Group_Id = @0)  and obsolete='N'   and del_status='N'", clsmap.Group_Id);

                    foreach (var item in cline)
                    {
                        clsmap.part_slno = item.part_slno;
                        clsmap.operation_slno = item.operation_slno;
                        clsmap.machine_slno = item.machine_slno;
                        clsmap.Format_No = txtFormatNo.Text;
                        clsmap.Qual_Char = txtqualityChar.Text;
                        clsmap.del_status = "N";
                        if (substatus == "Y")
                        { clsmap.SubmitStatus = "Y"; }
                        else { clsmap.SubmitStatus = "N"; }
                        clsmap.Obsolete = "N";
                        clsmap.rev_no = Convert.ToInt32(hdnrevnumber.Value);
                        clsmap.rev_reason = hdnrevreason.Value;
                        clsmap.rev_date = hdnrevdt.Value;
                        clsmap.nature_of_Change = txtchangenature.Text;
                        clsmap.reason_For_Change = txtreasonforchange.Text;
                        if (!string.IsNullOrEmpty(hdndcr_slno.Value))
                            clsmap.dcr_slno = Convert.ToInt32(hdndcr_slno.Value);
                        var existingRecord = lsthdr.FirstOrDefault(x => x.part_slno == item.part_slno && x.operation_slno == item.operation_slno && x.machine_slno == item.machine_slno);

                        if (existingRecord != null)
                        {
                            clsmap.Map_slno = existingRecord.Map_slno;
                            db1.Update(clsmap);
                        }
                        else
                        {
                            db1.Insert(clsmap);
                        }
                    }

                    var deletedRecords = lsthdr.Where(x => !cline.Any(c => c.part_slno == x.part_slno && c.operation_slno == x.operation_slno && c.machine_slno == x.machine_slno)).ToList();

                    foreach (var delItem in deletedRecords)
                    {
                        delItem.del_status = "Y";
                        db1.Update(delItem);
                    }
                }
            }
        }
        else
        {
            ShowMessage("Data not Saved.", MessageType.Warning, false);
            return;
        }

        LoadGrid();
        if (substatus == "Y")
        {
            using (Database db2 = new Database("connString"))
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
                string sqlTo = "";
                string sqlCc = "";
                if (Convert.ToInt32(ddlApprovedBy.SelectedValue) > 0)
                {
                    sqlTo = @"select emailid from  employees where del_status='N' and EmployeeSlNo=" + Convert.ToInt32(ddlApprovedBy.SelectedValue);
                }
                else
                {
                    sqlTo = @"select emailid from  employees where del_status='N' and isAdmin='Y'";
                }

                List<string> lstemplTo = db2.Fetch<string>(sqlTo).ToList();


                if (Convert.ToInt32(ddlPreparedBy.SelectedValue) > 0)
                {
                    sqlCc = @"select emailid from  employees where del_status='N' and EmployeeSlNo=" + Convert.ToInt32(ddlPreparedBy.SelectedValue);
                    sqlCc = sqlCc + " union select emailid from  employees where del_status='N' and isAdmin='Y'";
                }
                else
                {
                    sqlCc = @"select emailid from  employees where del_status='N' and isAdmin='Y'";
                }


                List<string> lstempCc = db2.Fetch<string>(sqlCc).ToList();


                lstempCc.Add(userInfo[0].EmailId);
                lstemplTo.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                lstempCc.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                PrepareForMail(lstemplTo, lstempCc, "for_approval");
            }
        }
    }






    protected bool CheckIfExists()
    {
        List<Class_sop_mapping> lst = new List<Class_sop_mapping>().Where(x => x.Group_Name.ToUpper() == txtGroupName.Text.ToUpper()).ToList();

        if (hdnEditMode.Value == "I")
        {
            if (lst.Count > 0)
                return true;
            else
                return false;
        }
        else
        {
            List<Class_sop_mapping> lst1 = lst.Where(x => x.Group_Id != Convert.ToInt32(hdnSlNo.Value)).ToList();
            if (lst1.Count > 0)
                return true;
            else
                return false;
        }


    }
    protected void ShowMessage(string Message, MessageType type, bool cls = false, string url = null)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "','" + cls + "','" + url + "');", true);
    }
    protected int GenerateGroupId()
    {
        string sql = "select isnull( max(isnull(Group_id,0))+1,1) as Group_id from SOP_Mapping ";

        int sl = -1;
        using (Database db = new Database("connString"))
        {
            string sql1 = "select Group_id from SOP_Mapping  where Group_name='" + txtGroupName.Text + "'";
            sl = db.ExecuteScalar<int>(sql1);
            if (sl != 0)
            {
                return sl;
            }
            else
            {
                sl = db.ExecuteScalar<int>(sql);
                return sl;
            }

        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            using (Database db = new Database("connString"))
            {
                // No records found, exit the function

                int groupId = Convert.ToInt32(hdnSlNo.Value);
                int maxRev = db.Fetch<Class_sop_mapping>()
                               .Where(x => x.Group_Id == groupId && x.del_status == "N" && x.Obsolete == "N")
                               .Max(x => x.rev_no); // Get latest revision

                List<Class_sop_mapping> lst = db.Fetch<Class_sop_mapping>()
                .Where(x => x.Group_Id == Convert.ToInt32(hdnSlNo.Value) && x.del_status == "N" && x.Obsolete == "N" && x.rev_no == maxRev)
                .ToList();

                if (lst.Count == 0)
                    return;

                // Step 3: Extract map_slno values from lst
                List<int> mapSlNos = lst.Select(x => x.Map_slno).ToList();
                // Update is_approved to 1 for the given Group_Id
                string mapSlNosCsv = string.Join(",", mapSlNos);
                string query = "UPDATE sop_header_new SET is_approved = 1 WHERE Group_Id = @0   and map_slno in(" + mapSlNosCsv + ")";
                db.Execute(query, groupId);



                int dcslno = string.IsNullOrEmpty(hdndcr_slno.Value) ? 0 : Convert.ToInt32(hdndcr_slno.Value);
                if (dcslno > 0)
                {

                    string sqlDeleteDCR = "UPDATE DCR_SOP SET  submit_status='O' , del_status='Y' " +
                                        "WHERE Group_Id=@Group_Id " +
                                        "AND Submit_status='R' " +
                                        "AND del_status='N' " +
                                        "AND dcr_slno=@dcr";

                    var parameters = new
                    {
                        Group_Id = groupId,
                        dcr = dcslno
                    };

                    int rowsAffected = db.Execute(sqlDeleteDCR, parameters);
                }

                ShowMessage("Approval successful.", MessageType.Success, true);
                // Ensure lst is not empty before accessing elements
                using (Database db2 = new Database("connString"))
                {
                    btnSave.Enabled = false;
                    btnSubmit.Enabled = false;
                    btnApprove.Enabled = false;
                    string sqlTo = "";
                    string sqlCc = "";
                    if (Convert.ToInt32(ddlPreparedBy.SelectedValue) > 0)
                    {
                        sqlTo = @"select emailid from  employees where del_status='N' and EmployeeSlNo=" + Convert.ToInt32(ddlPreparedBy.SelectedValue);
                    }
                    else
                    {
                        sqlTo = @"select emailid from  employees where del_status='N' and isAdmin='Y'";
                    }

                    List<string> lstemplTo = db2.Fetch<string>(sqlTo).ToList();


                    if (Convert.ToInt32(ddlApprovedBy.SelectedValue) > 0)
                    {
                        sqlCc = @"select emailid from  employees where del_status='N' and EmployeeSlNo=" + Convert.ToInt32(ddlApprovedBy.SelectedValue);
                        sqlCc = sqlCc + " union select emailid from  employees where del_status='N' and isAdmin='Y'";
                    }
                    else
                    {
                        sqlCc = @" union select emailid from  employees where del_status='N' and isAdmin='Y'";
                    }


                    List<string> lstempCc = db2.Fetch<string>(sqlCc).ToList();


                    lstempCc.Add(userInfo[0].EmailId);
                    lstemplTo.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                    lstempCc.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                    PrepareForMail(lstemplTo, lstempCc, "approved");
                }


                // Refresh details to reflect changes
                GetDetails();
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Error during approval: " + ex.Message, MessageType.Error, false);
        }

    }
    protected bool CheckIfGenerateReport()
    {
        int sl = 0;
        using (Database db = new Database("connString"))
        {

            string qryIfSOPApproved = "Select Count(*) from sop_header_new   where Group_Id=@0   and is_approved=0";
            sl = db.ExecuteScalar<int>(qryIfSOPApproved, Convert.ToInt32(hdnSlNo.Value));
            if (sl == 0)
                return true;
            else
                return false;
        }

    }
    //int ColumnWidthToPixels(double excelColumnWidth)
    //{
    //    if (excelColumnWidth <= 1)
    //        return (int)(excelColumnWidth * 12);
    //    else
    //        return (int)(excelColumnWidth * 7 + 5);
    //}

    //int RowHeightToPixels(double excelRowHeight)
    //{
    //    return (int)(excelRowHeight * 0.75); // Row height is in points, 1 point = 0.75 pixels approximately
    //}

    string SafeString(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        return new string(input.Where(c => !char.IsControl(c) || c == '\n' || c == '\r').ToArray());
    }

    public static int ColumnWidthToPixelsNEW(double excelColumnWidth)
    {
        return (int)Math.Round(excelColumnWidth * 7); // Approximation
    }

    public static int RowHeightToPixelsNEW(double excelRowHeight)
    {
        return (int)Math.Round(excelRowHeight * 0.75); // Approximation
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        int partscount = 0;
        // string[] imagePaths = [];
        string filePath = "";
        string worksheetName = "";
        int startColumn = 0;
        int endColumn = 0;
        int startRow = 0;
        int endrow = 0;
        string pdfname = "";
        var ret = "";
        int imgno = 0;
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "SOP_" + txtGroupName.Text + ".xlsx";
            string filepath = Server.MapPath("~/pdftemp/" + fname);

            FileInfo excelFile = new FileInfo(filepath);
            // FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/SOPTemp.xlsx"));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/SOPNew.xlsx"));
            //ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile);
            string sqlheader = "select hdr.*,p.mstPartNo,p.PartDescription,o.operationDesc,oprev.operationDesc as prevoperation," +
                " onext.operationDesc as nextoperation,  m.MachineDesc ,pmap.process_no  ,smap.rev_no,smap.Format_No,smap.Qual_Char,e1.EmployeeName  as " +
                " ApprovedByName,e.EmployeeName as PreparedByName ,smap.Active " +
                " from sop_header_new hdr  inner join SOP_Mapping smap on smap.Group_id=hdr.Group_Id  and smap.Map_slno=hdr.Map_slno " +
                "  inner join parts p on p.part_slno=hdr.part_slno  " +
                " inner join operations  o on o.operation_slno=hdr.operation_slno inner join machines" +
                " m on m.machine_slno=hdr.machine_slno left outer join operations  oprev on oprev.operation_slno=hdr.prev_oprn_slno  " +
                " left outer join operations  onext on onext.operation_slno=hdr.next_oprn_slno " +
                " inner join PartsMapping pmap on pmap.part_slno=hdr.part_slno and pmap.operation_slno=hdr.operation_slno and pmap.machine_slno=hdr.machine_slno" +
                " left outer Join employees e on e.EmployeeSlNo=smap.PreparedBy  left outer Join employees e1 on e1.EmployeeSlNo=smap.ApprovedBy  where hdr.group_id=@0" +
                "  and smap.rev_no=(Select  MAX(rev_no) from SOP_Mapping where Group_Id=@0)  and hdr.is_obsolete=0 order by Map_slno";
            string sqlprc = " Select  prc.*, p.partIssueNo,p.mstPartno,p.partIssueDt from sop_ProcessParameternew prc inner join sop_header_new hdr  " +
            " on hdr.sop_id=prc.sop_id inner join SOP_Mapping smap on smap.Group_id=hdr.Group_Id  and smap.Map_slno=hdr.Map_slno inner join parts p on p.part_slno=hdr.part_slno where prc.group_id=@0  and hdr.is_obsolete=0  and smap.rev_no=(Select  MAX(rev_no) from SOP_Mapping  where Group_Id=@0) order by Map_slno";
            string sqltool = " Select t.*,p.mstPartNo,map.part_slno ,pmap.process_no,map.Group_Name   from sop_toolings_new t inner join sop_header_new hdr " +
                "  on hdr.sop_id=t.sop_id  inner join SOP_Mapping map on map.map_slno=t.map_slno and map.Group_id=hdr.Group_Id " +
                "  inner join parts p on p.part_slno=map.part_slno   " +
                " inner join PartsMapping pmap on pmap.part_slno=map.part_slno and pmap.operation_slno=map.operation_slno and pmap.machine_slno=map.machine_slno  " +
              "  where t.group_id=@0  and hdr.is_obsolete=0 and map.rev_no=(Select  MAX(rev_no) from SOP_Mapping  where Group_Id=@0)  order by t.Map_slno,t.sop_tool_slno";
            string sqlrevhistory = " select distinct rev_no,rev_date,rev_reason,nature_of_Change,reason_For_Change,Format_No,Qual_Char from SOP_Mapping  where Group_id =@0 order by rev_no desc";
            Logger.LogError(sqlheader);
            Logger.LogError(sqlprc);
            Logger.LogError(sqltool);
            pdfname = fname.Replace(".xlsx", ".pdf");
            using (Database db = new Database("connString"))
            {
                List<Class_sop_header_new> lsthdr = db.Fetch<Class_sop_header_new>(sqlheader, Convert.ToInt32(hdnSlNo.Value));
                List<Class_sop_ProcessParameter_new> lstprc = db.Fetch<Class_sop_ProcessParameter_new>(sqlprc, Convert.ToInt32(hdnSlNo.Value));
                List<Class_sop_toolings_new> lsttool = db.Fetch<Class_sop_toolings_new>(sqltool, Convert.ToInt32(hdnSlNo.Value));
                List<Class_sop_mapping> lsttrevhistory = db.Fetch<Class_sop_mapping>(sqlrevhistory, Convert.ToInt32(hdnSlNo.Value));
                string FormatNumber = string.Empty;
                string QualChar = string.Empty;
                int Rev_no = 0;
                string revdate = string.Empty;
                if (lsttrevhistory.Count > 0)
                {
                    var latestEntry = lsttrevhistory.First(); // Sort once and get top record

                    QualChar = latestEntry.Qual_Char;
                    FormatNumber = latestEntry.Format_No;
                    Rev_no = latestEntry.rev_no;
                    revdate = latestEntry.rev_date;

                }
                using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                {
                    ExcelWorksheet wsheader = xlPackage.Workbook.Worksheets[1];
                    ExcelWorksheet wsprocess = xlPackage.Workbook.Worksheets[2];
                    ExcelWorksheet wstooling = xlPackage.Workbook.Worksheets[3];
                    ExcelWorksheet wstooling1 = xlPackage.Workbook.Worksheets[4];
                    ExcelWorksheet wstooling2 = xlPackage.Workbook.Worksheets[5];

                    var distinctPartNos = lstprc.Select(p => p.mstPartno).Distinct().ToList();
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Parameter", typeof(string));
                    foreach (var partNo in distinctPartNos)
                    {
                        dataTable.Columns.Add(partNo, typeof(string));
                    }
                    dataTable.Columns.Add("CheckingMethod", typeof(string));
                    dataTable.Columns.Add("Frequency", typeof(string));
                    dataTable.Columns.Add("Control_Method", typeof(string));

                    var distinctParameters = lstprc
                    .Select(p => new { p.Parameter, p.sop_id, p.mstPartno, p.Param_Value, p.CheckingMethod, p.Frequency, p.Control_Method })
                    .Distinct();

                    foreach (var parameter in distinctParameters)
                    {

                        DataRow row = dataTable.AsEnumerable()
                                                .FirstOrDefault(r => r["Parameter"].ToString() == parameter.Parameter);

                        if (row == null)
                        {

                            row = dataTable.NewRow();
                            row["Parameter"] = parameter.Parameter;
                            row["CheckingMethod"] = parameter.CheckingMethod;
                            row["Frequency"] = parameter.Frequency;
                            row["Control_Method"] = parameter.Control_Method;

                            dataTable.Rows.Add(row);
                        }
                        row[parameter.mstPartno] = parameter.Param_Value;
                    }
                    int rowstartPrc = 7;
                    int rowstartTooling = 7;
                    if (lsthdr.Count > 0)
                    {
                        wsheader.Cells["L1"].Value = FormatNumber; // L1 -> FormatNumber
                        wsheader.Cells["L3"].Value = Rev_no.ToString();      // L3 -> revdate
                        wsheader.Cells["L2"].Value = revdate;
                        wsheader.Cells["A27"].Value = SafeString("PRODUCT CHARACTERISTICS: " + QualChar);
                        int instructionIndex = 0;
                        //for (int i = 0; i < lsthdr.Count; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(lsthdr[i].instruction_1) ||
                        //        !string.IsNullOrEmpty(lsthdr[i].instruction_2) ||
                        //        !string.IsNullOrEmpty(lsthdr[i].instruction_3) ||
                        //        !string.IsNullOrEmpty(lsthdr[i].instruction_4) ||
                        //        !string.IsNullOrEmpty(lsthdr[i].instruction_5) ||
                        //        !string.IsNullOrEmpty(lsthdr[i].instruction_6))
                        //    {
                        //        instructionIndex = i;
                        //        break;  // Exit loop once a record with filled instructions is found
                        //    }
                        //}
                        double xlrowht = 50;
                        string operations = string.Join(",  ",
    lsthdr.Select(x => x.operationdesc)
           .Where(x => !string.IsNullOrWhiteSpace(x))
           .Distinct());

                        string machine = string.Join(",  ",
    lsthdr.Select(x => x.machinedesc)
           .Where(x => !string.IsNullOrWhiteSpace(x))
           .Distinct());
                        if (instructionIndex >= 0)
                        {
                            wsheader.Cells[5, 1].Value = SafeString("Part Name:" + lsthdr[instructionIndex].PartDescription);
                            //wsheader.Cells[5, 7].Value = "Operation No :" + lsthdr[instructionIndex].process_no;
                            wsheader.Cells[5, 1].Style.WrapText = true;
                            wsheader.Cells[5, 5].Value = "Operation Name :" + operations;
                            wsheader.Cells[5, 5].Style.WrapText = true;

                            // wsheader.Cells[7, 5].Value = lsthdr[instructionIndex].operationdesc;

                            wsheader.Cells[5, 10].Value = "Machine Name :" + machine;
                            wsheader.Cells[5, 10].Style.WrapText = true;
                            wsheader.Cells[7, 1].Value = "Objective:  :" + lsthdr[instructionIndex].objective;
                            // wsheader.Cells[7, 5].Value = lsthdr[instructionIndex].prevoperation;
                            // wsheader.Cells[7, 11].Value = lsthdr[instructionIndex].nextoperation;

                            //wsheader.Cells[7, 5].Value = null;
                            //wsheader.Cells[7, 11].Value = null;
                            //wsheader.Cells[6, 5].Value = null;
                            //wsheader.Cells[6, 11].Value = null;

                            //wsheader.Cells[6, 7].Value = null;
                            //wsheader.Cells[6, 10].Value = null;


                            //wsheader.Cells[5, 8].Value = "Operation Name:"+lsthdr[instructionIndex].operationdesc;
                            wsheader.Cells[28, 1].Value = wsheader.Cells[28, 1].Value + lsthdr[instructionIndex].PreparedByName;
                            wsheader.Cells[28, 7].Value = wsheader.Cells[28, 7].Value + lsthdr[instructionIndex].ApprovedByName;
                            // wsheader.Cells[7, 5].Value = operations;
                            wsheader.Cells[7, 5].Style.WrapText = true;
                            wsheader.Cells[7, 11].Style.WrapText = true;
                            wsheader.Cells[7, 8].Style.WrapText = true;
                            wsheader.Cells[16, 1].Value = lsthdr[instructionIndex].comment_1;
                            wsheader.Cells[16, 4].Value = lsthdr[instructionIndex].comment_2;
                            wsheader.Cells[16, 7].Value = lsthdr[instructionIndex].comment_3;
                            wsheader.Cells[26, 1].Value = lsthdr[instructionIndex].comment_4;
                            wsheader.Cells[26, 4].Value = lsthdr[instructionIndex].comment_5;
                            wsheader.Cells[26, 7].Value = lsthdr[instructionIndex].comment_6;
                            string img = lsthdr[instructionIndex].instruction_1;
                            string img2 = lsthdr[instructionIndex].instruction_2;
                            string img3 = lsthdr[instructionIndex].instruction_3;
                            string img4 = lsthdr[instructionIndex].instruction_4;
                            string img5 = lsthdr[instructionIndex].instruction_5;
                            string img6 = lsthdr[instructionIndex].instruction_6;
                            string pth = Server.MapPath("~/Documents/SOP/" + img);
                            string pth2 = Server.MapPath("~/Documents/SOP/" + img2);
                            string pth3 = Server.MapPath("~/Documents/SOP/" + img3);
                            string pth4 = Server.MapPath("~/Documents/SOP/" + img4);
                            string pth5 = Server.MapPath("~/Documents/SOP/" + img5);
                            string pth6 = Server.MapPath("~/Documents/SOP/" + img6);
                            string[] imagePaths = { pth, pth2, pth3, pth4, pth5, pth6 };
                            //for (int i = 0; i < imagePaths.Length; i++)
                            //{

                            //    FileInfo imgFile = new FileInfo(imagePaths[i]);
                            //    if (imgFile.Exists)
                            //    {
                            //        imgno += 1;
                            //        Bitmap image = new Bitmap(imagePaths[i]);
                            //        ExcelPicture excelPicture = wsheader.Drawings.AddPicture("chkPictureName" + imgno + i, image);
                            //        int rowIndex = i < 3 ? 9 : 19;  // Rows 9  for images 1-3, Rows 19 for images 4-6
                            //        int columnIndex = (i % 3) * 3 + 1; // Columns 1, 4, and 7 for images 1-3, Columns 1, 4, and 7 for images 
                            //        ExcelRangeBase cell = wsheader.Cells[rowIndex, columnIndex];
                            //        excelPicture.SetPosition((cell.Start.Row - 1), 0, (cell.Start.Column - 1), 0);


                            //        excelPicture.SetSize(700, 700);
                            //    }
                            //}
                            for (int i = 0; i < imagePaths.Length; i++)
                            {

                                string currentPath = imagePaths[i];
                                FileInfo imgFile = new FileInfo(currentPath);

                                if (imgFile.Exists)
                                {
                                    imgno++;

                                    Bitmap image = new Bitmap(currentPath);
                                    ExcelPicture excelPicture = wsheader.Drawings.AddPicture("chkPictureName" + imgno + i, image);

                                    int rowIndex = i < 3 ? 9 : 19;           // First 3 images: row 9, next 3: row 19
                                    int columnIndex = (i % 3) * 3 + 1;       // Column: 1, 4, 7

                                    // Set the cell size larger to accommodate image
                                    wsheader.Row(rowIndex).Height = 120;     // Height in points (approx. 1.33 * pixels)
                                    wsheader.Column(columnIndex).Width = 30; // Width in characters

                                    // Set larger image size
                                    excelPicture.SetSize(600, 600);          // Size in pixels (try 140+ if needed)

                                    // Center better: increase offsets
                                    int rowOffsetPixels = 200;  // vertical padding
                                    int colOffsetPixels = 60;  // horizontal padding

                                    excelPicture.SetPosition(rowIndex - 1, rowOffsetPixels, columnIndex - 1, colOffsetPixels);
                                }
                            }





                            wsheader.Cells[9, 10].Value = lsthdr[instructionIndex].oprn_instruction.TrimEnd();
                            wsheader.Cells[9, 10].Style.WrapText = true;

                            ExcelRange cell1;

                            cell1 = wsheader.Cells[16, 10];
                            cell1.Style.Font.Bold = false;
                            cell1.RichText.Clear();
                            var rt1 = cell1.RichText.Add("LIST OF CHECK POINTS:\n");
                            rt1.Bold = true;
                            var rt2 = cell1.RichText.Add(lsthdr[instructionIndex].checkpoints_list ?? "");
                            cell1.Style.WrapText = true;
                            rt2.Bold = false;

                            cell1 = wsheader.Cells[18, 10];
                            cell1.Style.Font.Bold = false;
                            cell1.RichText.Clear();
                            rt1 = cell1.RichText.Add("WORK HOLDING METHOD:\n");
                            rt1.Bold = true;
                            rt2 = cell1.RichText.Add(lsthdr[instructionIndex].workholding_method ?? "");
                            cell1.Style.WrapText = true;
                            rt2.Bold = false;

                            cell1 = wsheader.Cells[19, 10];
                            cell1.Style.Font.Bold = false;
                            cell1.RichText.Clear();
                            rt1 = cell1.RichText.Add("FIRST OFF APPROVAL:\n");
                            rt1.Bold = true;
                            rt2 = cell1.RichText.Add(lsthdr[instructionIndex].firstoff_approval ?? "");
                            cell1.Style.WrapText = true;
                            rt2.Bold = false;

                            cell1 = wsheader.Cells[22, 10];
                            cell1.Style.Font.Bold = false;
                            cell1.RichText.Clear();
                            rt1 = cell1.RichText.Add("REACTION PLAN:\n");
                            rt1.Bold = true;
                            rt2 = cell1.RichText.Add(lsthdr[instructionIndex].reaction_plan ?? "");
                            cell1.Style.WrapText = true;
                            rt2.Bold = false;

                            cell1 = wsheader.Cells[24, 10];
                            cell1.Style.Font.Bold = false;
                            cell1.RichText.Clear();
                            rt1 = cell1.RichText.Add("COOLANT USED:\n");
                            rt1.Bold = true;
                            rt2 = cell1.RichText.Add(lsthdr[instructionIndex].coolant_used ?? "");
                            cell1.Style.WrapText = true;
                            rt2.Bold = false;

                            cell1 = wsheader.Cells[25, 10];
                            cell1.Style.Font.Bold = false;
                            cell1.RichText.Clear();
                            rt1 = cell1.RichText.Add("NOTE:\n");
                            rt1.Bold = true;
                            rt2 = cell1.RichText.Add(lsthdr[instructionIndex].notes ?? "");
                            cell1.Style.WrapText = true;
                            rt2.Bold = false;

                            if (lstprc.Count > 0)
                            {
                                wsprocess.Cells["O1"].Value = FormatNumber; // L1 -> FormatNumber
                                wsprocess.Cells["O3"].Value = Rev_no.ToString();      // L3 -> revdate
                                wsprocess.Cells["O2"].Value = revdate;
                                string processNo = lsthdr[instructionIndex].process_no;
                                wsprocess.Cells[27, 1].Value = wsprocess.Cells[27, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                wsprocess.Cells[27, 10].Value = wsprocess.Cells[27, 10].Value + lsthdr[instructionIndex].ApprovedByName;
                                //double processNoNumeric;

                                //// Check if the process_no is in the format of "10" or "10.2"
                                //if (double.TryParse(processNo, out processNoNumeric))
                                //{
                                //    // If successful, set the numeric value to the cell
                                //    wsprocess.Cells[6, 2].Value = processNoNumeric;
                                //}
                                //else
                                //{
                                //    // If not a valid numeric format, set the value as string
                                wsprocess.Cells[6, 2].Value = processNo;
                                //}
                                //wsprocess.Cells[6, 2].Value = Convert.ToDouble(lsthdr[instructionIndex].process_no);
                                wsprocess.Cells[7, 2].Value = txtGroupName.Text;
                                for (int i = 0; i < distinctPartNos.Count; i++)
                                {
                                    wsprocess.Cells[8, i + 2].Value = distinctPartNos[i];
                                    wsprocess.Cells[9, i + 2].Value = lstprc.FirstOrDefault(p => p.mstPartno == distinctPartNos[i]).partIssueNo;
                                    wsprocess.Cells[9, i + 2].Value = wsprocess.Cells[9, i + 2].Value + " / " + lstprc.FirstOrDefault(p => p.mstPartno == distinctPartNos[i]).partIssueDt;
                                    wsprocess.Cells[8, i + 2].Style.WrapText = true;
                                    wsprocess.Cells[9, i + 2].Style.WrapText = true;

                                }
                                int row = 10;

                                for (int i = 0; i < dataTable.Rows.Count; i++)
                                {
                                    wsprocess.Cells[row, 1].Value = dataTable.Rows[i]["Parameter"];
                                    int startcolnum = 2;
                                    int endcolnum = distinctPartNos.Count + 1;
                                    for (int j = 0; j < distinctPartNos.Count; j++)
                                    {
                                        if (startcolnum <= endcolnum)
                                        {
                                            wsprocess.Cells[row, startcolnum].Value = dataTable.Rows[i][distinctPartNos[j]];
                                            wsprocess.Cells[row, startcolnum].Style.WrapText = true;
                                        }
                                        startcolnum = startcolnum + 1;
                                    }
                                    wsprocess.Cells[row, 12].Value = dataTable.Rows[i]["CheckingMethod"];
                                    wsprocess.Cells[row, 13].Value = dataTable.Rows[i]["Frequency"];
                                    wsprocess.Cells[row, 14].Value = dataTable.Rows[i]["Control_Method"];
                                    wsprocess.Cells[row, 12].Style.WrapText = true;
                                    wsprocess.Cells[row, 13].Style.WrapText = true;
                                    wsprocess.Cells[row, 14].Style.WrapText = true;
                                    // wsprocess.Cells[row, 14, row, 15].Merge = true;
                                    row = row + 1;

                                }
                                if (row < 26)
                                { wsprocess.DeleteRow(row, (26 - row)); }
                            }

                            if (lsttool.Count > 0)
                            {
                                var groupedParts = lsttool.GroupBy(x => x.part_slno).Take(12).ToList();
                                partscount = groupedParts.Count;
                                int distinctPartCount = groupedParts.Count;
                                var worksheets = new[]
                                {
        xlPackage.Workbook.Worksheets[3],
        xlPackage.Workbook.Worksheets[4],
        xlPackage.Workbook.Worksheets[5]
    };

                                // Metadata cell mapping
                                string[,] metaCells = new string[,]
                                {
        { "B6", "B7", "B8" },    // Part 1
        { "L6", "L7", "L8" },    // Part 2
        { "B20", "B21", "B22" },// Part 3
        { "L20", "L21", "L22" },// Part 4
                                };

                                // Row placement mapping
                                int[] startRows = { 10, 10, 24, 24 }; // Parts 1 & 2 start at 10, Parts 3 & 4 at 24
                                int[] endRows = { 19, 19, 33, 33 }; // Maximum of 10 rows per part

                                // Column mapping
                                string[] startCols = { "A", "K", "A", "K" }; // Part 1 & 3 → A, Part 2 & 4 → K

                                for (int i = 0; i < groupedParts.Count; i++)
                                {
                                    var partGroup = groupedParts[i];
                                    int sheetIndex = i / 4;
                                    int blockIndex = i % 4;
                                    var sheet = worksheets[sheetIndex];

                                    // Write metadata cells
                                    var firstRow = partGroup.First();

                                    sheet.Cells[metaCells[blockIndex, 0]].Value = firstRow.process_no;   // Operation No
                                    sheet.Cells[metaCells[blockIndex, 1]].Value = firstRow.Group_Name;    // Group No
                                    sheet.Cells[metaCells[blockIndex, 2]].Value = firstRow.mstPartNo;   // Part No

                                    // Determine row & column placement
                                    int startRowtool = startRows[blockIndex];
                                    //string startCol = startCols[blockIndex];
                                    int startColIndex = GetColumnNumber(startCols[blockIndex]);
                                    int rowIndex = startRowtool;
                                    int slno = 1;
                                    foreach (var row in partGroup.Take(10)) // Ensure max 10 rows per part
                                    {

                                        int columnOffset = 0;

                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = slno++;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.Operation;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.tool_holder_name;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.tool;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.cutting_speed;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.feed_rate;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.per_corner;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.no_of_corners;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.total_nos;
                                        sheet.Cells[ExcelCellAddress.GetColumnLetter(startColIndex + columnOffset++) + rowIndex.ToString()].Value = row.control_method;
                                        rowIndex++;
                                    }
                                }


                                if (distinctPartCount <= 4)
                                {
                                    xlPackage.Workbook.Worksheets.Delete(wstooling1);
                                    xlPackage.Workbook.Worksheets.Delete(wstooling2);
                                    wstooling.Cells[34, 1].Value = wstooling.Cells[34, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                    wstooling.Cells[34, 16].Value = wstooling.Cells[34, 16].Value + lsthdr[instructionIndex].ApprovedByName;
                                    wstooling.Cells["T1"].Value = FormatNumber;
                                    wstooling.Cells["T3"].Value = Rev_no.ToString();
                                    wstooling.Cells["T2"].Value = revdate;
                                    wstooling.Cells["T4"].Value = " 3 of 3";
                                    wsheader.Cells["L4"].Value = " 1 of 3";
                                    wsprocess.Cells["O4"].Value = " 2 of 3";
                                    int rowrstart = 37;
                                    if (lsttrevhistory.Count > 0)
                                    {
                                        var orderedList = lsttrevhistory.OrderBy(x => x.rev_no).ToList();
                                        for (int i = 0; i < orderedList.Count; i++)
                                        {
                                            wstooling.Cells[(rowrstart + i), 1].Value = i + 1;
                                            wstooling.Cells[(rowrstart + i), 2].Value = orderedList[i].rev_no.ToString();
                                            wstooling.Cells[(rowrstart + i), 3].Value = orderedList[i].rev_date;

                                            wstooling.Cells[(rowrstart + i), 4].Value = orderedList[i].nature_of_Change;
                                            wstooling.Cells[(rowrstart + i), 13].Value = orderedList[i].reason_For_Change;
                                        }
                                    }
                                    wstooling.PrinterSettings.PrintArea = wstooling.Cells["A1:T46"];
                                }
                                else if (distinctPartCount <= 8 && distinctPartCount > 4)
                                {
                                    wstooling.Cells[34, 1].Value = wstooling.Cells[34, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                    wstooling.Cells[34, 16].Value = wstooling.Cells[34, 16].Value + lsthdr[instructionIndex].ApprovedByName;

                                    wstooling1.Cells[34, 1].Value = wstooling1.Cells[34, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                    wstooling1.Cells[34, 16].Value = wstooling1.Cells[34, 16].Value + lsthdr[instructionIndex].ApprovedByName;


                                    wstooling.Cells["T1"].Value = FormatNumber;
                                    wstooling.Cells["T3"].Value = Rev_no.ToString();
                                    wstooling.Cells["T2"].Value = revdate;

                                    wstooling1.Cells["T1"].Value = FormatNumber;
                                    wstooling1.Cells["T3"].Value = Rev_no.ToString();
                                    wstooling1.Cells["T2"].Value = revdate;

                                    wstooling1.Cells["T4"].Value = " 4 of 4";
                                    wstooling.Cells["T4"].Value = " 3 of 4";
                                    wsheader.Cells["L4"].Value = " 1 of 4";
                                    wsprocess.Cells["O4"].Value = " 2 of 4";




                                    int rowrstart = 37;
                                    if (lsttrevhistory.Count > 0)
                                    {
                                        var orderedList = lsttrevhistory.OrderBy(x => x.rev_no).ToList();
                                        for (int i = 0; i < orderedList.Count; i++)
                                        {
                                            wstooling1.Cells[(rowrstart + i), 1].Value = i + 1;
                                            wstooling1.Cells[(rowrstart + i), 2].Value = orderedList[i].rev_no.ToString();
                                            wstooling1.Cells[(rowrstart + i), 3].Value = orderedList[i].rev_date;

                                            wstooling1.Cells[(rowrstart + i), 4].Value = orderedList[i].nature_of_Change;
                                            wstooling1.Cells[(rowrstart + i), 13].Value = orderedList[i].reason_For_Change;
                                        }
                                    }
                                    wstooling.PrinterSettings.PrintArea = wstooling.Cells["A1:T34"];
                                    var rangeToClear = wstooling.Cells["A35:T46"];
                                    rangeToClear.Clear();
                                    wstooling1.PrinterSettings.PrintArea = wstooling1.Cells["A1:T46"];
                                    xlPackage.Workbook.Worksheets.Delete(wstooling2);
                                }
                                else if (distinctPartCount > 8)
                                {
                                    wstooling.Cells[34, 1].Value = wstooling.Cells[34, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                    wstooling.Cells[34, 16].Value = wstooling.Cells[34, 16].Value + lsthdr[instructionIndex].ApprovedByName;

                                    wstooling1.Cells[34, 1].Value = wstooling1.Cells[34, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                    wstooling1.Cells[34, 16].Value = wstooling1.Cells[34, 16].Value + lsthdr[instructionIndex].ApprovedByName;
                                    wstooling2.Cells[34, 1].Value = wstooling2.Cells[34, 1].Value + lsthdr[instructionIndex].PreparedByName;
                                    wstooling2.Cells[34, 16].Value = wstooling2.Cells[34, 16].Value + lsthdr[instructionIndex].ApprovedByName;


                                    wstooling.Cells["T1"].Value = FormatNumber;
                                    wstooling.Cells["T3"].Value = Rev_no.ToString();
                                    wstooling.Cells["T2"].Value = revdate;

                                    wstooling1.Cells["T1"].Value = FormatNumber;
                                    wstooling1.Cells["T3"].Value = Rev_no.ToString();
                                    wstooling1.Cells["T2"].Value = revdate;

                                    wstooling2.Cells["T1"].Value = FormatNumber;
                                    wstooling2.Cells["T3"].Value = Rev_no.ToString();
                                    wstooling2.Cells["T2"].Value = revdate;

                                    wstooling2.Cells["T4"].Value = " 5 of 5";
                                    wstooling1.Cells["T4"].Value = " 4 of 5";
                                    wstooling.Cells["T4"].Value = " 3 of 5";
                                    wsheader.Cells["L4"].Value = " 1 of 5";
                                    wsprocess.Cells["O4"].Value = " 2 of 5";



                                    wsheader.PrinterSettings.Orientation = eOrientation.Landscape;
                                    wstooling.PrinterSettings.PrintArea = wstooling.Cells["A1:T34"];
                                    var rangeToClear = wstooling.Cells["A35:T46"];
                                    rangeToClear.Clear();
                                    wstooling1.PrinterSettings.PrintArea = wstooling1.Cells["A1:T34"];
                                    var rangeToClear1 = wstooling1.Cells["A35:T46"];
                                    rangeToClear1.Clear();
                                    wstooling2.PrinterSettings.PrintArea = wstooling2.Cells["A1:T46"];
                                    int rowrstart = 37;
                                    if (lsttrevhistory.Count > 0)
                                    {
                                        var orderedList = lsttrevhistory.OrderBy(x => x.rev_no).ToList();
                                        for (int i = 0; i < orderedList.Count; i++)
                                        {
                                            wstooling2.Cells[(rowrstart + i), 1].Value = i + 1;
                                            wstooling2.Cells[(rowrstart + i), 2].Value = orderedList[i].rev_no.ToString();
                                            wstooling2.Cells[(rowrstart + i), 3].Value = orderedList[i].rev_date;

                                            wstooling2.Cells[(rowrstart + i), 4].Value = orderedList[i].nature_of_Change;
                                            wstooling2.Cells[(rowrstart + i), 13].Value = orderedList[i].reason_For_Change;
                                        }
                                    }

                                }


                            }

                        }
                        else
                        {
                            ShowMessage("Data Unavailable for the Group .", MessageType.Info);
                            return;
                        }
                        wsheader.PrinterSettings.PrintArea = wsheader.Cells["A1:L28"];
                        int endrowprc = 7 + dataTable.Rows.Count + 4;
                        wsprocess.PrinterSettings.PrintArea = wsprocess.Cells["A1:O" + endrowprc];
                        // wsprocess.PrinterSettings.PrintArea = wsprocess.Cells["A1:O27"];
                        //int endrowtoolings = 7 + dataTable.Rows.Count + 4;
                        //wstooling.PrinterSettings.PrintArea = wstooling.Cells["A1:T46"];
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }

                        AdjustRowHeightsForAllSheets(xlPackage);

                        wsheader.Row(17).PageBreak = true;


                        wsheader.Row(17).PageBreak = true;


                        xlPackage.SaveAs(excelFile);
                        xlPackage.Dispose();

                        string filePath1 = excelFile.ToString();

                        string worksheetName1 = "General ";
                        int startColumn1 = 1;
                        int endColumn1 = 13;
                        // AutofitRowHeight(filePath1, worksheetName1, 5, 19, startColumn1, endColumn1, (List<Class_Employees>)userinfo);

                        worksheetName1 = "Process Parameter";
                        startColumn1 = 1;
                        endColumn1 = 15;
                        AutofitRowHeight(filePath1, worksheetName1, 6, endrowprc, startColumn1, endColumn1, (List<Class_Employees>)userinfo);




                        worksheetName1 = "Tooling Parameter";
                        startColumn1 = 1;
                        endColumn1 = 20;
                        AutofitRowHeight(filePath1, worksheetName1, 6, 34, startColumn1, endColumn1, (List<Class_Employees>)userinfo);
                        if (partscount > 8)
                        {

                            worksheetName1 = "Sheet1";
                            startColumn1 = 1;
                            endColumn1 = 20;
                            AutofitRowHeight(filePath1, worksheetName1, 6, 34, startColumn1, endColumn1, (List<Class_Employees>)userinfo);

                            worksheetName1 = "Sheet2";
                            startColumn1 = 1;
                            endColumn1 = 20;
                            AutofitRowHeight(filePath1, worksheetName1, 6, 34, startColumn1, endColumn1, (List<Class_Employees>)userinfo);

                        }
                        else if (partscount > 4 && partscount <= 8)
                        {
                            worksheetName1 = "Sheet1";
                            startColumn1 = 1;
                            endColumn1 = 20;
                            AutofitRowHeight(filePath1, worksheetName1, 6, 34, startColumn1, endColumn1, (List<Class_Employees>)userinfo);
                        }
                    }
                    else
                    {
                        ShowMessage("Data Unavailable for the Group .", MessageType.Info);
                        return;
                    }

                    //Response.Clear();
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=" + Path.GetFileName(filepath));
                    //Response.WriteFile(filepath);
                    //Response.Flush();
                    //Response.End();
                    //Response.Clear();
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", ("attachment;  filename=" + fname));
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());
                    //Response.End();


                }
                // Response.Redirect("FiledownloadHandler.ashx?fname=" + fname);
                Session["fname"] = fname;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating SOP: " + ex.ToString());

        }
        DownloadFile();
    }

    private void AutofitRowHeight(string filePath, string worksheetName, int startRow, int endRow, int startColumn, int endColumn, List<Class_Employees> userinfo)
    {
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(filePath);
            IWorksheet worksheet = workbook.Worksheets[worksheetName];

            for (int row = startRow; row <= endRow; row++)
            {
                worksheet.AutofitRow(row);

            }

            workbook.Save();
            int defaultRowHt = 75;

            for (int row = startRow; row <= endRow; row++)
            {
                double currentRowHt = worksheet.GetRowHeight(row);
                if (currentRowHt < Convert.ToDouble(defaultRowHt))
                {

                    worksheet.SetRowHeight(row, (defaultRowHt - 10));
                }

                //else if (currentRowHt == Convert.ToDouble(defaultRowHt))
                //{
                //    worksheet.SetRowHeight(row, defaultRowHt+10);
                //}
            }


            workbook.Save();


        }
    }
    public static void AdjustRowHeightsForAllSheetsOLD(ExcelPackage package)
    {
        foreach (var ws in package.Workbook.Worksheets)
        {
            if (ws.Dimension == null) continue;

            int maxRow = ws.Dimension.End.Row;
            int maxCol = ws.Dimension.End.Column;

            for (int row = 5; row <= maxRow; row++) // Start from row 5 (skip first 4)
            {
                double requiredHeight = ws.Row(row).Height > 0 ? ws.Row(row).Height : 15;
                double maxHeightInRow = requiredHeight;

                for (int col = 1; col <= maxCol; col++)
                {
                    var cell = ws.Cells[row, col];
                    if (cell == null || string.IsNullOrWhiteSpace(cell.Text))
                        continue;


                    // Enable wrap text for measurement (only if not already wrapped)
                    cell.Style.WrapText = true;

                    string cellText = cell.Text.Trim();
                    double fontSize = cell.Style.Font.Size > 0 ? cell.Style.Font.Size : 11;
                    double lineHeight = fontSize * 1.2;

                    double colWidth = ws.Column(col).Width;
                    if (colWidth <= 0) colWidth = 8.43; // default column width

                    // Characters that can fit per line
                    double charsPerLine = colWidth * 1.2;
                    charsPerLine = (colWidth - 0.99) / 0.12;
                    // Estimate wrapped lines based on newline + width overflow
                    int totalLines = 0;
                    foreach (var logicalLine in cellText.Split('\n'))
                    {
                        int lineLength = logicalLine.Length;
                        int wrappedLines = (int)Math.Ceiling(lineLength / charsPerLine);

                        totalLines += wrappedLines > 0 ? wrappedLines : 1;
                    }

                    double estimatedHeight = totalLines * lineHeight + 2; // slight padding

                    if (estimatedHeight > maxHeightInRow)
                    {
                        maxHeightInRow = estimatedHeight;
                    }
                }

                // Reduce overshoot — don't stretch small content too much
                double currentHeight = ws.Row(row).Height;
                if (Math.Abs(maxHeightInRow - currentHeight) > 1)
                {
                    ws.Row(row).Height = maxHeightInRow;
                }
            }
        }
    }
    public static void AdjustRowHeightsForAllSheets(ExcelPackage package)
    {
        foreach (var ws in package.Workbook.Worksheets)
        {
            if (ws.Dimension == null) continue;

            int maxRow = ws.Dimension.End.Row;
            int maxCol = ws.Dimension.End.Column;

            for (int row = 5; row <= maxRow; row++) // Start from row 5 (skip first 4)
            {
                double requiredHeight = ws.Row(row).Height > 0 ? ws.Row(row).Height : 15;
                double maxHeightInRow = requiredHeight;

                for (int col = 1; col <= maxCol; col++)
                {
                    var cell = ws.Cells[row, col];
                    if (cell == null) continue;

                    // ✅ Use RichText if present, otherwise fall back to Text
                    string cellText = "";
                    if (cell.RichText.Count > 0)
                    {
                        cellText = string.Join("", cell.RichText.Select(r => r.Text));
                    }
                    else
                    {
                        //cellText = cell.Text?.Trim() ?? "";
                        cellText = cell.Text == null ? "" : cell.Text.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(cellText))
                        continue;

                    // Enable wrap text
                    cell.Style.WrapText = true;

                    // Font size (fallback = 11)
                    double fontSize = cell.Style.Font.Size > 0 ? cell.Style.Font.Size : 11;
                    double lineHeight = fontSize * 1.2;

                    // Column width (fallback = default Excel width 8.43)
                    double colWidth = ws.Column(col).Width;
                    if (colWidth <= 0) colWidth = 8.43;

                    // Approximate characters per line
                    double charsPerLine = (colWidth - 0.99) / 0.12;

                    // Count wrapped lines
                    int totalLines = 0;
                    foreach (var logicalLine in cellText.Split('\n'))
                    {
                        int lineLength = logicalLine.Length;
                        int wrappedLines = (int)Math.Ceiling(lineLength / charsPerLine);
                        totalLines += wrappedLines > 0 ? wrappedLines : 1;
                    }

                    double estimatedHeight = totalLines * lineHeight + 2; // padding

                    if (estimatedHeight > maxHeightInRow)
                    {
                        maxHeightInRow = estimatedHeight;
                    }
                }

                // Apply row height if different enough
                double currentHeight = ws.Row(row).Height;
                if (Math.Abs(maxHeightInRow - currentHeight) > 1)
                {
                    ws.Row(row).Height = maxHeightInRow;
                }

                if (row == 18)
                { ws.Row(row).Height = maxHeightInRow + 20; }
            }
        }
    }



    public static string GetColumnLetter(int columnNumber)
    {
        if (columnNumber < 1)
            throw new ArgumentOutOfRangeException("columnNumber", "Column number must be greater than zero.");

        var columnLetter = new StringBuilder();
        while (columnNumber > 0)
        {
            columnNumber--;
            columnLetter.Insert(0, (char)('A' + (columnNumber % 26)));
            columnNumber /= 26;
        }

        return columnLetter.ToString();
    }

    public static int GetColumnNumber(string columnLetter)
    {
        if (string.IsNullOrEmpty(columnLetter))
            throw new ArgumentNullException("columnLetter", "Column letter cannot be null or empty.");

        columnLetter = columnLetter.ToUpperInvariant();
        int sum = 0;

        foreach (char c in columnLetter)
        {
            if (c < 'A' || c > 'Z')
                throw new ArgumentException("Invalid column letter", "columnLetter");

            sum *= 26;
            sum += (c - 'A' + 1);
        }

        return sum;
    }


    void DownloadFile()
    {
        string filename = (string)Session["fname"];
        string filepath = HttpContext.Current.Server.MapPath("~/pdftemp/" + filename);

        if (File.Exists(filepath))
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            HttpContext.Current.Response.TransmitFile(filepath);
            HttpContext.Current.Response.End();
        }
        else
        {
            // File not found, handle the error accordingly
            HttpContext.Current.Response.Write("File not found.");
            HttpContext.Current.Response.End();
        }
    }

}
