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
using System.Text.RegularExpressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Org.BouncyCastle.Asn1.X509;
using Syncfusion.XlsIO.FormatParser;
using System.Data.SqlTypes;
using System.Windows.Forms;

public partial class dcr_sop : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {

            LoadGroups();
            LoadEmployees();
            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtdcr_number.Text = generateDCRNumber().ToString(); 

            if (Request.QueryString.HasKeys())
            {
                if (userInfo[0].isAdmin == "Y" || userInfo[0].CanApprove == "Y")
                {
                    btnApprove.Enabled = true;
                }
                else
                {
                    btnApprove.Enabled = false;
                }

                btnDelete.Enabled = true;
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnEditMode.Value = "E";
                GetDetails();
            }

        }
    }
    void LoadGroups()
    {
        string qry =@"SELECT distinct Group_Name, Group_Id, rev_no
FROM SOP_Mapping m
WHERE m.Obsolete = 'N' AND m.del_status = 'N'
  AND m.rev_no = (
      SELECT MAX(m2.rev_no)
      FROM SOP_Mapping m2
      WHERE m2.Group_Id = m.Group_Id AND m2.Obsolete = 'N' AND m2.del_status = 'N'
  )
  AND m.Group_Id IN(
      SELECT DISTINCT h.Group_Id
      FROM sop_header_new h
      WHERE h.is_approved = '1' AND h.is_obsolete = '0'
  );";
        Database db = new Database("connString");
        var lst = db.Fetch<Class_sop_mapping>(qry).ToList();

        if (lst.Count > 0)
        {
            ddlgroup.DataSource = lst;
            ddlgroup.DataTextField = "Group_Name";
            ddlgroup.DataValueField = "Group_Id";
            ddlgroup.DataBind();
        }

        ddlgroup.Items.Insert(0, new ListItem("Select...", "0"));
    }



    void LoadGroupsOLD()
    {
        ddlgroup.Items.Clear();
        Crud_sop_mapping crud = new Crud_sop_mapping();

        List<Class_sop_mapping> lst = crud.SelectAll()
            .Where(x => x.del_status == "N" && x.Obsolete == "N")
            .GroupBy(x => x.Group_Id)  // Group by Group_Id to remove duplicates
            .Select(g => g.First())
            .ToList();

        List<Class_sop_mapping> validGroups = new List<Class_sop_mapping>();

        foreach (var group in lst)
        {
            int groupId = group.Group_Id;

            // Check if Group_Id exists in sop_header_new
            Crud_sop_header_new cr = new Crud_sop_header_new();
           bool existsInHeader = cr.SelectAll()
                .Any(h => h.Group_Id == groupId);

            if (!existsInHeader)
                continue; // Skip adding if no records in sop_header_new

            // Check if all records for the Group_Id in sop_header_new have is_approved = 1
            bool allApproved = cr.SelectAll()
                .Where(h => h.Group_Id == groupId)
                .All(h => h.is_approved == true);

            if (allApproved)
            {
                validGroups.Add(group);
            }
        }

        if (validGroups.Count > 0)
        {
            ddlgroup.DataSource = validGroups;
            ddlgroup.DataTextField = "Group_Name"; // Field for display
            ddlgroup.DataValueField = "Group_Id";  // Field for value
            ddlgroup.DataBind();
        }

        ddlgroup.Items.Insert(0, new ListItem("Select...", "0"));
    }

    void LoadGroupsold()
    {
        ddlgroup.Items.Clear();
        Crud_sop_mapping crud = new Crud_sop_mapping();
        List<Class_sop_mapping> lst = crud.SelectAll()
            .Where(x => x.del_status == "N" && x.Obsolete == "N")
             .GroupBy(x => x.Group_Id)  // Group by Group_Id to remove duplicates
        .Select(g => g.First())
            .ToList();
        if (lst.Count > 0)
        {
            ddlgroup.DataSource = lst;
            ddlgroup.DataTextField = "Group_Name"; // Correct field for display
            ddlgroup.DataValueField = "Group_Id";  // Correct field for value
            ddlgroup.DataBind();
        }
        ddlgroup.Items.Insert(0, new ListItem("Select...", "0"));
    }
    void GetDetails()
    {
        Crud_dcr_sop crud = new Crud_dcr_sop();
        Class_DCR_SOP cls = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
        LoadEmployees();
        ddlEmployees.SelectedValue = cls.Request_By.ToString();
        txtRequestDate.Text = cls.Request_Date;
        LoadGroups();
        ddlgroup.SelectedValue = cls.Group_Id.ToString();
        txtdcr_number.Text = cls.dcr_number;
       
        ddlchangearea.SelectedValue = cls.change_area;
        if (cls.Prc_Characteristics == "Y")
            chkprcchar.Checked = true;
        else chkprcchar.Checked = false;
        if (cls.Prd_Characteristics == "Y")
            chkprdchar.Checked = true;
        else chkprdchar.Checked = false;
        if (cls.Process_Name == "Y")
            chkProcess.Checked = true;
        else chkProcess.Checked = false;
        if (cls.Process_Number == "Y")
            chkOpNo.Checked = true;
        else chkOpNo.Checked = false;
        if (cls.Specification == "Y")
            chkSpecificaiton.Checked = true;
        else chkSpecificaiton.Checked = false;
        if (cls.Measurement_Tech == "Y")
            chkMeasurement_Tech.Checked = true;
        else chkMeasurement_Tech.Checked = false;
        if (cls.Sample_Size == "Y")
            chkSample_size.Checked = true;
        else chkSample_size.Checked = false;
        if (cls.Sample_Frequency == "Y")
            chkSampleFreq.Checked = true;
        else chkSampleFreq.Checked = false;
        if (cls.Control_Method == "Y")
            chkControlMethod.Checked = true;
        else chkControlMethod.Checked = false;
        if (cls.Others == "Y")
            chkOthers.Checked = true;
        else chkOthers.Checked = false;
        txtReason.Text = cls.Reason_For_Change;
        txtExisting.Text = cls.Existing;
        txtChanges.Text = cls.Changes_Required;
        txtnatureOfChange.Text = cls.nature_of_change;
        hdnsubmitstatus.Value = cls.Submit_Status;
        if (cls.Submit_Status == "Y")
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
        }
        else if (cls.Submit_Status == "N")
        {
            btnApprove.Enabled = false;
            btnSubmit.Enabled = true;
        }
        else if (cls.Submit_Status == "A" || cls.Submit_Status=="O")
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            btnApprove.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
    protected void LoadEmployees()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst = null;
        using (Database db = new Database("connString"))
        {


            List<Class_Employees> lstemp = null;


            string sqlemp = "Select EmployeeSlno,EmployeeName from employees where del_status='N'";
            lstemp = db.Query<Class_Employees>(sqlemp).ToList();

            ddlEmployees.Items.Clear();

            ddlEmployees.DataSource = lstemp;
            ddlEmployees.DataTextField = "EmployeeName";
            ddlEmployees.DataValueField = "EmployeeSlno";
            ddlEmployees.DataBind();

            ddlEmployees.SelectedValue = userInfo[0].EmployeeSlNo.ToString();


        }
        ddlEmployees.Items.Insert(0, new ListItem("Select...", "0"));
        

    }

   
    void SaveData()
    {
        string sqlstatdcr = "";
        if(!string.IsNullOrEmpty(hdnSlNo.Value))
        sqlstatdcr = "Select * from DCR_SOP where  group_id=" + ddlgroup.SelectedValue + "  and del_status<>'Y'  and Submit_Status <>'O'  and dcr_slno<>"+Convert.ToInt32(hdnSlNo.Value);
        else
            sqlstatdcr = "Select * from DCR_SOP where  group_id=" + ddlgroup.SelectedValue + "  and del_status<>'Y'  and Submit_Status <>'O' ";
        List<Class_DCR_SOP> lstdcrsop = new List<Class_DCR_SOP>();
        using (Database db1 = new Database("connString"))
        { lstdcrsop = db1.Query<Class_DCR_SOP>(sqlstatdcr).ToList(); }
        if (lstdcrsop.Count > 0)
        { 
            ShowMessage("An active DCR for the selected Group already Exists.",MessageType.Info);
            return; 
        }
        try
        {
            using (Class_DCR_SOP clsdcr = new Class_DCR_SOP())
            {

                clsdcr.dcr_number = txtdcr_number.Text;
                clsdcr.Request_By = Convert.ToInt32(ddlEmployees.SelectedValue);
                clsdcr.Request_Date = txtRequestDate.Text.ToString();
               
                clsdcr.Existing = txtExisting.Text;
                clsdcr.Changes_Required = txtChanges.Text;
                clsdcr.Reason_For_Change = txtReason.Text;
                clsdcr.change_area = ddlchangearea.SelectedValue;
                clsdcr.nature_of_change = txtnatureOfChange.Text;
                clsdcr.Group_Id = Convert.ToInt32(ddlgroup.SelectedValue);
                clsdcr.Submit_Status = hdnsubmitstatus.Value;
                clsdcr.del_status = "N";
                clsdcr.Process_Name = chkProcess.Checked ? "Y" : "N";
                clsdcr.Process_Number = chkOpNo.Checked ? "Y" : "N";
                clsdcr.Prc_Characteristics = chkprcchar.Checked ? "Y" : "N";
                clsdcr.Prd_Characteristics = chkprdchar.Checked ? "Y" : "N";
                clsdcr.Sample_Frequency = chkSampleFreq.Checked ? "Y" : "N";
                clsdcr.Sample_Size = chkSample_size.Checked ? "Y" : "N";
                clsdcr.Control_Method = chkControlMethod.Checked ? "Y" : "N";
                clsdcr.Specification = chkSpecificaiton.Checked ? "Y" : "N";
                clsdcr.Others = chkOthers.Checked ? "Y" : "N";
                clsdcr.Measurement_Tech = chkMeasurement_Tech.Checked ? "Y" : "N";
                
                    if (hdnsubmitstatus.Value == "Y")
                    clsdcr.DCR_Submit_DateTime = DateTime.Now;
                if (hdnEditMode.Value == "I")
                {

                    using (Crud_dcr_sop crud = new Crud_dcr_sop())
                    {
                        crud.Insert(clsdcr);
                        ShowMessage("Record Saved Successfully", MessageType.Success);
                    }
                }
                else
                {
                    clsdcr.dcr_slno = Convert.ToInt32(hdnSlNo.Value);
                    using (Crud_dcr_sop crud = new Crud_dcr_sop())
                    {
                        crud.Update(clsdcr);
                        ShowMessage("Record Updated Successfully", MessageType.Success);
                    }
                }
                using (Database db = new Database("connString"))
                {
                    string qrypartsinGroup = "Select distinct part_slno,operation_slno from SOP_Mapping where Group_id=@0   and del_status='N'   and obsolete='N'   and rev_no=(select max(rev_no) from SOP_Mapping" +
                        " where  del_status='N'   and obsolete='N' and Group_id=@0 )";
                    List<Class_sop_mapping> lstpartsmap = db.Query<Class_sop_mapping>(qrypartsinGroup,Convert.ToInt32(ddlgroup.SelectedValue)).ToList();
                  
                    Class_DCR_SOP_Status dcrstatus = new Class_DCR_SOP_Status();
                    List<Class_DCR_SOP_Status> lstdcrstatus = new List<Class_DCR_SOP_Status>();



                    string sqlstat = "Select * from DCR_SOP_Status where  dcr_slno=" + clsdcr.dcr_slno;
                    lstdcrstatus = db.Query<Class_DCR_SOP_Status>(sqlstat).ToList();
                    if (lstdcrstatus != null && lstdcrstatus.Count > 0)
                    {
                        using (Crud_DCR_SOP_Status crudst = new Crud_DCR_SOP_Status())
                        {
                            crudst.Delete(clsdcr.dcr_slno);
                        }
                    }


                    for (int i = 0; i < lstpartsmap.Count; i++)
                    {
                        dcrstatus.Group_Id = Convert.ToInt32(ddlgroup.SelectedValue);
                        dcrstatus.dcr_slno = clsdcr.dcr_slno;
                        dcrstatus.part_slno = lstpartsmap[i].part_slno;
                        dcrstatus.operation_slno = lstpartsmap[i].operation_slno;
                        dcrstatus.status = "pending";
                        using (Crud_DCR_SOP_Status crudst = new Crud_DCR_SOP_Status())
                        {
                            crudst.Insert(dcrstatus);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            ShowMessage(e.Message, MessageType.Error);
        }
    }
    void ClearData()
    {
        Response.Redirect("dcr.aspx");

    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {

        using (Class_DCR_SOP cls = new Class_DCR_SOP())
        {
            cls.dcr_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_dcr_sop crud = new Crud_dcr_sop())
            {
                string sqlPM = "update DCR_SOP set  Submit_Status='A' ,DCR_Approved_DateTime=@0 where  dcr_slno=" + Convert.ToInt32(hdnSlNo.Value);

                using (Database db = new Database("connString"))
                {
                    db.Execute(sqlPM, DateTime.Now);

                }
                ShowMessage("Record Approved Successfully", MessageType.Success);
                using (Database db = new Database("connString"))
                {
                    string sql = @"select emailid from  employees where del_status='N' and isAdmin='Y'";
//                    string sqlCC = @"select e.emailid
//from parts p
//inner join employees e on e.employeeslno=p.approvedBy
//where p.part_slno=@0";

                    List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql);
                  //  List<Class_Employees> lstemplCC = db.Fetch<Class_Employees>(sqlCC, ddlmstPartNo.SelectedValue);
                    List<string> tomails = new List<string>();
                    if (lstempl.Count > 0)
                    {
                        foreach (Class_Employees c1 in lstempl)
                        {
                            tomails.Add(c1.EmailId);
                        }
                    }
                    else
                    {
                        tomails.Add(userInfo[0].EmailId);
                    }

                    List<string> ccmails = new List<string>();
                    //if (lstemplCC.Count > 0)
                    //{
                    //    foreach (Class_Employees c1 in lstemplCC)
                    //    {
                    //        ccmails.Add(c1.EmailId);
                    //    }
                    //}
                    string ccsql = "select emailid from employees where employeeslno=" + Convert.ToInt32(ddlEmployees.SelectedValue);
                    string ccmail = db.ExecuteScalar<string>(ccsql);
                    ccmails.Add(userInfo[0].EmailId);
                    ccmails.Add(ccmail);
                    tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                    ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                    PrepareForMail(tomails, ccmails, "approved");
                }
            }
        }
    }

    protected void PrepareForMail(List<string> tomails, List<string> ccmails, string mailFor)
    {
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
        string changearea = "Control Plan";
        if (ddlchangearea.SelectedItem.Text == "SOP")
            changearea = "SOP";
        string s = string.Empty;

        string subject = string.Empty;

        // string[] parts = lblPartDtls.Text.Split(';');

        string msg = string.Empty;

        switch (mailFor)
        {
            case "submit":
                msg = "DCR for SOP is submitted in the portal and is pending for your approval. Please find the details below:";
                subject = " DCR submitted for " + changearea + "- (Group: " + ddlgroup.SelectedItem.Text + ")";
                break;
            case "approved":
                msg = "The below DCR for SOP is approved  and is pending for your Action.";
                subject = "DCR Approved for " + changearea + "- (Group: " + ddlgroup.SelectedItem.Text + ")";
                break;
            default:
                msg = "Unknown condition for sending mail triggered.";
                subject = "Unknown mail condition: " + ddlgroup.SelectedItem.Text;
                break;
        }


        string styl = "<style>table{max-width:100%;background-color:transparent;font-size:14px}th{text-align:left}.table{width:100%;margin-bottom:20px}.table>tbody>tr>td,.table>tbody>tr>th,.table>tfoot>tr>td,.table>tfoot>tr>th,.table>thead>tr>td,.table>thead>tr>th{padding:8px;line-height:1.428571429;vertical-align:top;border-top:1px solid #ddd}.table>thead>tr>th{vertical-align:bottom;border-bottom:2px solid #ddd}.table>tbody+tbody{border-top:2px solid #ddd}.table .table{background-color:#fff}.table-striped>tbody>tr:nth-child(odd)>td,.table-striped>tbody>tr:nth-child(odd)>th{background-color:#f9f9f9}body{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:14px;line-height:1.428571429;color:#333;background-color:#fff}h2{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:500;line-height:1.1;color:inherit;text-align:center}</style>";

        string bodycnt = "<center><b><label style='background-color:#0198FF; color:White;font-family:Calibri;font-size:medium;'>&nbsp;THIS IS AN AUTOGENERATED MAIL. DO NOT REPLY TO THIS!! &nbsp;</label></b></center>" +
            "<body style='font-family:Calibri;font-size:medium;'>Hi, <br/><br/>" + msg +
            "<table class='table table-striped'>" +
             "<tr><th>DCR Number</th><td>" + txtdcr_number.Text + "</td></tr>" +
            "<tr><th>Group</th><td>" + ddlgroup.SelectedItem.Text + "</td></tr>" +
            "<tr><th>Parts</Th><td>" + hdnopstxt.Value + "</td></tr>" +
            "<tr><th>Request By</th><td>" + ddlEmployees.SelectedItem.Text + "</td></tr>" +
            "<tr><th>Changes Required</th><td>" + txtChanges.Text + "</td></tr>" +
            "<tr><th>Reason for Change</th><td>" + txtReason.Text + "</td></tr>" +
        "<tr><th>Reason for Change</th><td>" + txtnatureOfChange.Text + "</td></tr>";




        string mailBody = "<html><head>" + styl + "</head><body>" + bodycnt + "</body></html>";

        List<string> newToId = tomails.Distinct().ToList();
        List<string> newCcId = ccmails.Distinct().ToList();


        // send mail
        mail ml = new mail();
        ml.SendMail(toMailId: newToId, mailSubject: subject, bodyText: mailBody, ccMailId: newCcId);

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        hdnsubmitstatus.Value = "Y";
        SaveData();
        using (Database db = new Database("connString"))
        {
//            string sql = @"select e.emailid
//from parts p
//inner join employees e on e.employeeslno=p.approvedBy
//where p.part_slno=@0";
            string sql = @"select e.emailid
from employees e where isAdmin='Y'  and del_status='N'";
          //  List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql, ddlmstPartNo.SelectedValue);
            List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql);
            List<string> tomails = new List<string>();
            if (lstempl.Count > 0)
            {
                foreach (Class_Employees c1 in lstempl)
                {
                    tomails.Add(c1.EmailId);
                }
            }
            else
            {
                tomails.Add(userInfo[0].EmailId);
            }

            List<string> ccmails = new List<string>();

            string ccsql = "select emailid from employees where employeeslno=" + Convert.ToInt32(ddlEmployees.SelectedValue);
            string ccmail = db.ExecuteScalar<string>(ccsql);


            ccmails.Add(userInfo[0].EmailId);
            ccmails.Add(ccmail);
            tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
            ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
            PrepareForMail(tomails, ccmails, "submit");
        }
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        hdnsubmitstatus.Value = "N";
        SaveData();

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
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }



    public string generateDCRNumber()
    {
        string dcrNumber = "DCR-SOP-";
        DateTime currentDate = DateTime.Now;
        string month = currentDate.ToString("MMM").ToUpper();
        string year = currentDate.ToString("yyyy");

        // Generate the base DCR number pattern
        dcrNumber = dcrNumber + month + year + "-";

        // Check if there is a DCR number already generated for this month and year
        string lastDCRNumber = getLastDCRNumberFromDB(month, year);

        int newNumber;
        if (string.IsNullOrEmpty(lastDCRNumber)) // No records found for this month-year
        {
            newNumber = 1; // Start from 1 for the first number in the month
        }
        else
        {
            // Extract the serial number from the last DCR number (after the last '-')
            string lastSerial = lastDCRNumber.Substring(lastDCRNumber.LastIndexOf('-') + 1);
            newNumber = Convert.ToInt32(lastSerial) + 1; // Increment the serial number
        }

        // Ensure that the generated DCR number doesn't already exist
        string newDCRNumber = dcrNumber + newNumber.ToString();

        // Now we check if the generated DCR number already exists in the database
        // If it does, increment the serial number until we find a unique one.
        int attemptCounter = 0;
        while (checkIfDCRNumberExists(newDCRNumber))  // Check if the generated number already exists
        {
            newNumber++; // Increment the serial number
            newDCRNumber = dcrNumber + newNumber.ToString(); // Generate a new DCR number
            attemptCounter++;

            // Avoid infinite loop, we can limit attempts for performance purposes
            if (attemptCounter > 5)
            {
                throw new Exception("Unable to generate unique DCR number after multiple attempts.");
            }
        }

        return newDCRNumber;
    }

    public string getLastDCRNumberFromDB(string mth, string yr)
    {
        // Query to fetch the last DCR number for the given month and year
        string sql = "SELECT TOP 1 dcr_number FROM dcr_sop WHERE dcr_number LIKE 'DCR-SOP-" + mth + yr + "-%' ORDER BY dcr_slno DESC";
        string lastDigits = string.Empty;

        using (Crud_dcr_sop crud = new Crud_dcr_sop())
        {
            using (Database db = new Database("connString"))
            {
                // Execute the query and retrieve the DCR number
                string dcrnumber = db.ExecuteScalar<string>(sql);

                if (!string.IsNullOrEmpty(dcrnumber))
                {
                    // Extract the serial number part from the DCR number
                    int lastHyphenIndex = dcrnumber.LastIndexOf('-');
                    if (lastHyphenIndex != -1)
                    {
                        lastDigits = dcrnumber.Substring(lastHyphenIndex + 1);
                    }
                }
            }
        }

        return lastDigits;
    }

    // Function to check if a DCR number already exists in the database
    public bool checkIfDCRNumberExists(string dcrNumber)
    {
        bool exists = false;
        string sql = "SELECT COUNT(1) FROM dcr_sop WHERE dcr_number = @dcrNumber";

        using (Crud_dcr_sop crud = new Crud_dcr_sop())
        {
            using (Database db = new Database("connString"))
            {
                exists = db.ExecuteScalar<int>(sql, new { dcrNumber }) > 0;
            }
        }

        return exists;
    }
    protected void ddlgroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    void DeleteData()
    {

        using (Class_DCR cls = new Class_DCR())
        {
            using (Crud_dcr_sop crud = new Crud_dcr_sop())
            {
                using (Database db = new Database("connString"))
                {
                    //string sqlcheckStatus = "select * from  dcr where (Submit_Status='A' or Submit_Status='O')  and  dcr_slno=" + Convert.ToInt32(hdnSlNo.Value);
                    //List<Class_DCR> lst = db.Fetch<Class_DCR>(sqlcheckStatus).ToList();
                    //if (lst.Count > 0)
                    //{
                    //    ShowMessage("DCR is Already Approved.Cannot delete.", MessageType.Success);
                    //    return;
                    //}
                    //else
                    //{
                        cls.dcr_slno = Convert.ToInt16(hdnSlNo.Value);

                        string sqlPM = "update dcr set del_status='Y'  where  dcr_slno=" + Convert.ToInt32(hdnSlNo.Value);


                        db.Execute(sqlPM);


                        ShowMessage("Record Deleted Successfully", MessageType.Success);
                    //}

                }
            }
        }
    }


}
