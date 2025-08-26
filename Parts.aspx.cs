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
using System.Windows.Interop;
using System.Data.SqlTypes;
using System.Data;

public partial class Parts : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;
    int flag = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo == null)
        {
            Response.Redirect("~/LoginPage.aspx");
        }

        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanPrepare != "Y" && userInfo[0].CanApprove != "Y" && userInfo[0].allmaster != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }

        if (!Page.IsPostBack)
        {
            LoadCFteams();
            LoadKeyContacts();
            LoadCustomers();
            LoadEmployees();

            if (Request.QueryString.HasKeys())
            {
                hdnSlNo.Value = Request.QueryString["slno"];

                string delst = GetDelStat(hdnSlNo.Value);
                hdnMode.Value = "E";
                GetDetails();
                btnDelete.Enabled = true;
                btnDelete.Visible = true;
                //txtPartNo.Enabled = false;

                if (delst == "N")
                {
                    btnRevise.Enabled = true;
                }
            }
            else
            {
                btnDelete.Enabled = false;
                txtPartNo.Enabled = true;
                txtpartIssueNo.Enabled = true;
                txtpartIssueDt.Enabled = true;

                txtcustomerIssueNo.Enabled = true;
                txtcustomerIssueDt.Enabled = true;
                txtrevno.Enabled = true;
                txtrevdt.Enabled = true;
                txtrevreason.Enabled = true;
                txtchangenature.Enabled = true;
            }
        }
    }

    /// <summary>
    /// displays user messages in the top of the screen
    /// </summary>
    /// <param name="Message">Message content</param>
    /// <param name="type">Message type as Success, Error, Info, Warning</param>
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }

    void LoadEmployees()
    {
        string sql = "select * from Employees where del_status='N'";
        using (Database db = new Database("connString"))
        {
            List<Class_Employees> lstPrep = db.Fetch<Class_Employees>(sql).Where(x => x.CanPrepare == "Y").ToList();
            List<Class_Employees> lstAppr = db.Fetch<Class_Employees>(sql).Where(x => x.CanApprove == "Y").ToList();
            ddlPrepd.Items.Clear();
            ddlPrepd.DataSource = lstPrep;
            ddlPrepd.DataValueField = "employeeslno";
            ddlPrepd.DataTextField = "employeename";
            ddlPrepd.DataBind();

            ddlAppd.Items.Clear();
            ddlAppd.DataSource = lstAppr;
            ddlAppd.DataValueField = "employeeslno";
            ddlAppd.DataTextField = "employeename";
            ddlAppd.DataBind();

            ddlPrepd.Items.Insert(0, new ListItem("Select", "0"));
            ddlAppd.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    void LoadKeyContacts()
    {
        Crud_Employees crud = new Crud_Employees();
        List<Class_Employees> lst = null;

        if (hdnMode.Value == "I")
        {
            lst = crud.usp_EmployeesSelect().ToList().Where(x => x.del_status == "ACTIVE" && x.key_contact == "Y").ToList();
        }
        else if (hdnMode.Value == "E")
        {
            lst = crud.usp_EmployeesSelect().ToList();
        }
        ddlkeycont.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlkeycont.Items.Add(new ListItem(lst[cnt].EmployeeName, Convert.ToString(lst[cnt].EmployeeName)));
        }

        ddlkeycont.Items.Insert(0, "Select...");

    }

    void LoadCustomers()
    {
        Crud_customers crud = new Crud_customers();
        List<Class_customers> lst = null;

        if (hdnMode.Value == "I")
        {
            lst = crud.SelectAll().Where(x => x.del_status == false).ToList();
        }
        else if (hdnMode.Value == "E")
        {
            lst = crud.SelectAll().ToList();
        }
        ddlcustomername.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlcustomername.Items.Add(new ListItem(lst[cnt].cust_name, Convert.ToString(lst[cnt].cust_name)));
        }
        ddlcustomername.Items.Insert(0, "Select...");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData();
        ClearData();
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

    /// <summary>
    /// checks if file already exists in the server
    /// </summary>
    /// <returns>true if exists and false if not</returns>
    bool CheckFileAlreadyExists()
    {
        Boolean fileExists = false;
        string presentFiles = string.Empty;

        string filename = Path.GetFileName(uploadfile1.FileName);
        FileInfo serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
        if (serverFile.Exists)
        {
            presentFiles += filename + " / ";
        }

        filename = Path.GetFileName(uploadfile2.FileName);
        serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
        if (serverFile.Exists)
        {
            presentFiles += filename + " / ";
        }

        filename = Path.GetFileName(uploadfile3.FileName);
        serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
        if (serverFile.Exists)
        {
            presentFiles += filename + " / ";
        }

        filename = Path.GetFileName(uploadfile4.FileName);
        serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
        if (serverFile.Exists)
        {
            presentFiles += filename + " / ";
        }

        if (presentFiles.Length > 0)
        {
            ShowMessage("These files already exists..<br />" + presentFiles, MessageType.Info);
            fileExists = true;
        }
        return fileExists;
    }

    /// <summary>
    /// checks if all uploaded files are pdf format
    /// </summary>
    /// <returns>returns true if yes and false if not</returns>
    bool AllPdfs()
    {
        Boolean onlyPdf = true;

        string filename = string.Empty;
        FileInfo serverFile = null;
        if (uploadfile1.HasFile)
        {
            filename = Path.GetFileName(uploadfile1.FileName);
            serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);

            if (serverFile.Extension.ToLower() == ".pdf")
            {
                onlyPdf = true;
            }
            else
            {
                onlyPdf = false;
            }
        }

        if (uploadfile2.HasFile)
        {
            filename = Path.GetFileName(uploadfile2.FileName);
            serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
            if (serverFile.Extension.ToLower() == ".pdf")
            {
                onlyPdf = true;
            }
            else
            {
                onlyPdf = false;
            }
        }

        if (uploadfile3.HasFile)
        {
            filename = Path.GetFileName(uploadfile3.FileName);
            serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
            if (serverFile.Extension.ToLower() == ".pdf")
            {
                onlyPdf = true;
            }
            else
            {
                onlyPdf = false;
            }
        }

        if (uploadfile4.HasFile)
        {
            filename = Path.GetFileName(uploadfile4.FileName);
            serverFile = new FileInfo(Server.MapPath("~/Documents/") + filename);
            if (serverFile.Extension.ToLower() == ".pdf")
            {
                onlyPdf = true;
            }

            else
            {
                onlyPdf = false;
            }
        }
        if (!onlyPdf)
        {
            ShowMessage("Uploaded file(s) are not in PDF format", MessageType.Info);
        }
        return onlyPdf;
    }

    /// <summary>
    /// saves the business locations data
    /// </summary>
    void SaveData()
    {

        if (CheckFileAlreadyExists())
        {
            return;
        }
        if (!AllPdfs())
        {
            return;
        }
        using (Class_parts cls = new Class_parts())
        {
            cls.mstPartNo = txtPartNo.Text;
            cls.PartDescription = txtPartDescription.Text;
            cls.partIssueNo = txtpartIssueNo.Text;
            cls.partIssueDt = txtpartIssueDt.Text;
            cls.CftTeamSlNo = Convert.ToInt16(ddlCftTeamSlNo.SelectedValue);

            cls.Customer_name = ddlcustomername.SelectedValue;
            cls.customerPartNo = txtcustomerPartNo.Text;
            cls.customerIssueNo = txtcustomerIssueNo.Text;
            cls.customerIssueDt = txtcustomerIssueDt.Text;

            cls.keyContact = ddlkeycont.SelectedValue;
            cls.keyContactPhone = txtkeyContactPhone.Text;
            cls.originalDt = txtoriginalDt.Text;

            cls.organization = txtorganization.Text;
            cls.orgApprovalDt = txtorgApprovalDt.Text;
            cls.orgDate = txtorgDate.Text;

            cls.custApproval = txtcustapproval.Text;
            cls.custApprovalDt = txtcustapprovaldt.Text;
            cls.custQaApproval = txtcustQaApproval.Text;
            cls.custQaApprovalDt = txtcustQaApprovalDt.Text;

            cls.otherApproval = txtotherApproval.Text;
            cls.otherApprovalDt = txtotherApprovalDt.Text;
            cls.cpType = ddlcpType.SelectedValue;
            cls.cp_number = txtcpnumber.Text.ToUpper();
            cls.cp_revno = txtrevno.Text;
            cls.cp_revdt = txtrevdt.Text;
            cls.supplier_code = txtsuppcode.Text;
            cls.proc_spec = txtprocspec.Text;
            cls.ih_testing_ref = txtihref.Text;
            cls.preparedBy = Convert.ToInt16(ddlPrepd.SelectedValue);
            cls.approvedBy = Convert.ToInt16(ddlAppd.SelectedValue);
            cls.rev_reason = txtrevreason.Text;
            cls.change_nature = txtchangenature.Text;

            string filesavepath = Server.MapPath("~/Documents/");

            //uploadfile1
            string fn1 = string.Empty;
            if (!uploadfile1.HasFile && lbluploadfile1.Text.Length == 0)
            {
                cls.uploadfile1 = string.Empty;
            }
            else if (uploadfile1.HasFile)
            {
                fn1 = uploadfile1.FileName;
                cls.uploadfile1 = fn1;

            }
            else if (lbluploadfile1.Text.Length > 0)
            {
                cls.uploadfile1 = lbluploadfile1.Text;
            }

            if (uploadfile1.HasFile)
            {
                uploadfile1.SaveAs(filesavepath + "/" + fn1);
            }

            //uploadfile2
            string fn2 = string.Empty;
            if (!uploadfile2.HasFile && lbluploadfile2.Text.Length == 0)
            {
                cls.uploadfile2 = string.Empty;
            }
            else if (uploadfile2.HasFile)
            {
                fn2 = uploadfile2.FileName;
                cls.uploadfile2 = fn2;
            }
            else if (lbluploadfile2.Text.Length > 0)
            {
                cls.uploadfile2 = lbluploadfile2.Text;
            }

            if (uploadfile2.HasFile)
            {
                uploadfile2.SaveAs(filesavepath + "/" + fn2);
            }

            //uploadfile3           
            string fn3 = string.Empty;
            if (!uploadfile3.HasFile && lbluploadfile3.Text.Length == 0)
            {
                cls.uploadfile3 = string.Empty;
            }
            else if (uploadfile3.HasFile)
            {
                fn3 = uploadfile3.FileName;
                cls.uploadfile3 = fn3;

            }
            else if (lbluploadfile3.Text.Length > 0)
            {
                cls.uploadfile3 = lbluploadfile3.Text;
            }

            if (uploadfile3.HasFile)
            {
                uploadfile3.SaveAs(filesavepath + "/" + fn3);
            }

            //uploadfile4                  
            string fn4 = string.Empty;
            if (!uploadfile4.HasFile && lbluploadfile4.Text.Length == 0)
            {
                cls.uploadfile4 = string.Empty;
            }
            else if (uploadfile4.HasFile)
            {
                fn4 = uploadfile4.FileName;
                cls.uploadfile4 = fn4;
            }
            else if (lbluploadfile4.Text.Length > 0)
            {
                cls.uploadfile4 = lbluploadfile4.Text;
            }

            if (uploadfile4.HasFile)
            {
                uploadfile4.SaveAs(filesavepath + "/" + fn4);
            }

            cls.del_status = ddlActiveInactive.SelectedValue;
            cls.Remarks = txtRemarks.Text;

            using (Crud_parts crud = new Crud_parts())
            {
                //List<Class_parts> lstP1 = crud.usp_partsSelect().Where(x => x.mstPartNo.ToUpper().Trim() == txtPartNo.Text.ToUpper().Trim() ).ToList();

                List<Class_parts> lstP1 = crud.usp_partsSelect()
                    .Where(x => x.mstPartNo.ToUpper().Trim() == txtPartNo.Text.ToUpper().Trim()).ToList();
                //.Where(x => x.mstPartNo.ToUpper().Trim() == txtPartNo.Text.ToUpper().Trim()
                //            && x.partIssueNo.ToUpper().Trim() == txtpartIssueNo.Text.ToUpper()
                //.Trim()).ToList();

                if (hdnMode.Value == "I")
                {
                    if (hdnrevclick.Value == "R")
                    {

                        lstP1 = crud.usp_partsSelect()
                                       .Where(x => x.partIssueNo.ToUpper().Trim() == txtpartIssueNo.Text.ToUpper().Trim() && x.mstPartNo == txtPartNo.Text && x.cp_revno == txtrevno.Text).ToList();

                    }
                    if (lstP1.Count > 0)
                    {
                        ShowMessage("Record Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        cls.Obsolete = "N";
                        if (string.IsNullOrEmpty(txtrevno.Text))
                            cls.cp_revno = "0";
                        if (string.IsNullOrEmpty(txtrevdt.Text))
                            cls.cp_revdt = DateTime.Today.ToString("dd/MM/yyyy");
                        List<Class_parts> lstPt = crud.usp_partsInsert(cls).ToList();
                        hdnNewSl.Value = Convert.ToString(lstPt[0].part_slno);

                        //Added this code to enter revision when new part part is eneterd on 5th july 2025
                        Class_part_revision_history partrev = new Class_part_revision_history();
                        partrev.part_slno = lstPt[0].part_slno;
                        if (!string.IsNullOrEmpty(txtrevno.Text))
                            partrev.rev_no = txtrevno.Text;
                        else { partrev.rev_no = "0"; }
                        if (!string.IsNullOrEmpty(txtrevdt.Text))
                            partrev.rev_date = txtrevdt.Text;
                        else { partrev.rev_date = DateTime.Today.ToString("dd/MM/yyyy"); }
                        partrev.rev_reasons = txtrevreason.Text;
                        partrev.change_nature = txtchangenature.Text;
                        partrev.revision_done_in = "Parts";
                        using (Database db = new Database("connString"))
                        {
                            db.Insert<Class_part_revision_history>(partrev);
                        }





                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                        if (hdnrevclick.Value != "R")
                        {
                            ClearData();
                        }
                    }


                    //cls.Obsolete = "N";
                    //List<Class_parts> lstPt = crud.usp_partsInsert(cls).ToList();
                    //hdnNewSl.Value = Convert.ToString(lstPt[0].part_slno);
                    //ShowMessage("Record Inserted Successfully", MessageType.Success);
                    //if (hdnrevclick.Value != "R")
                    //{
                    //    ClearData();

                }
                else if (hdnMode.Value == "E")
                {
                    List<Class_parts> lstP2 = crud.usp_partsSelect()
                    .Where(x => x.mstPartNo.ToUpper().Trim() == txtPartNo.Text.ToUpper().Trim() && x.part_slno != Convert.ToInt32(hdnSlNo.Value) && x.Obsolete == "N").ToList();
                    if (lstP2.Count > 0)
                    {
                        ShowMessage("Part Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        cls.part_slno = Convert.ToInt16(hdnSlNo.Value);
                        crud.usp_partsUpdate(cls);
                        string updateDCR = "Update DCR set mstPartNo= '" + txtPartNo.Text.Trim() + "' ,partDescription='" + txtPartDescription.Text + "' where part_slno=" + Convert.ToInt32(hdnSlNo.Value);
                        using (Database db = new Database("connString"))
                        {
                            db.Execute(updateDCR);

                        }

                        ShowMessage("Record Updated Successfully", MessageType.Success);
                        ClearData();
                        flag = 1;
                    }

                }
            }
        }
    }

    /// <summary>
    /// deletes the business location data
    /// </summary>

    void DeleteData()
    {

        using (Class_parts cls = new Class_parts())
        {
            cls.part_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_parts crud = new Crud_parts())
            {

                Crud_ControlPlan crudcp = new Crud_ControlPlan();
                List<Class_ControlPlan> lst = crudcp.usp_ControlPlanSelect()
                    .Where(x => x.part_slno == Convert.ToInt16(hdnSlNo.Value))
                    .ToList();

                if (lst.Count > 0)
                {
                    ShowMessage("Can not delete the data as used in Control Plan..!", MessageType.Info);
                }
                else
                {
                    string sqlPM = "Delete from PartsMapping where  part_slno=" + Convert.ToInt32(hdnSlNo.Value);
                    string sqlP = "Delete from parts  where  part_slno=" + Convert.ToInt32(hdnSlNo.Value);
                    using (Database db = new Database("connString"))
                    {
                        db.Execute(sqlPM);
                        db.Execute(sqlP);
                    }
                    ShowMessage("Record Deleted Successfully", MessageType.Success);
                }

                //crud.usp_partsDelete(cls);
                //ShowMessage("Record Deleted Successfully", MessageType.Success);
            }
        }
    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        txtPartNo.Text = "";
        txtPartDescription.Text = "";
        txtpartIssueNo.Text = "";
        txtpartIssueDt.Text = string.Empty;
        ddlCftTeamSlNo.SelectedValue = null;
        txtcustomerPartNo.Text = "";
        txtcustomerIssueNo.Text = "";
        txtcustomerIssueDt.Text = string.Empty;
        ddlkeycont.SelectedIndex = 0;
        txtkeyContactPhone.Text = "";
        txtorgDate.Text = string.Empty;
        txtorganization.Text = "";
        txtorgApprovalDt.Text = string.Empty;
        txtoriginalDt.Text = string.Empty;
        txtorgDate.Text = string.Empty;
        txtcustapproval.Text = string.Empty;
        txtcustapprovaldt.Text = string.Empty;
        txtcustQaApproval.Text = string.Empty;
        txtcustQaApprovalDt.Text = string.Empty;
        txtotherApproval.Text = string.Empty;
        txtotherApprovalDt.Text = string.Empty;
        ddlcustomername.SelectedIndex = 0;
        ddlcpType.SelectedIndex = 0;
        ddlActiveInactive.SelectedIndex = 0;
        lbluploadfile1.Text = "";
        lbluploadfile2.Text = "";
        lbluploadfile3.Text = "";
        lbluploadfile4.Text = "";
        txtRemarks.Text = "";
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        btnDelete.Enabled = false;
        btnRevise.Enabled = false;
        hdnNewSl.Value = string.Empty;
        hdnrevclick.Value = string.Empty;
        txtcpnumber.Text = string.Empty;
        txtrevno.Text = string.Empty;
        txtrevdt.Text = string.Empty;
        txtsuppcode.Text = string.Empty;
        txtprocspec.Text = string.Empty;
        txtihref.Text = string.Empty;
        ddlPrepd.SelectedIndex = 0;
        ddlAppd.SelectedIndex = 0;
        txtrevreason.Text = string.Empty;
        txtchangenature.Text = string.Empty;
    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>

    protected void delUploadFile1_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/" + lbluploadfile1.Text));
        if (f.Exists)
        {
            f.Delete();
            lbluploadfile1.Text = "";
            // SaveData();
        }
    }

    protected void delUploadFile2_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/" + lbluploadfile2.Text));
        if (f.Exists)
        {
            f.Delete();
            lbluploadfile2.Text = "";
            // SaveData();
        }
    }

    protected void delUploadFile3_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/" + lbluploadfile3.Text));
        if (f.Exists)
        {
            f.Delete();
            lbluploadfile3.Text = "";
            // SaveData();
        }
    }

    protected void delUploadFile4_Click(object sender, EventArgs e)
    {
        FileInfo f = new FileInfo(Server.MapPath("~/Documents/" + lbluploadfile4.Text));
        if (f.Exists)
        {
            f.Delete();
            lbluploadfile4.Text = "";
            // SaveData();
        }
    }

    void LoadCFteams()
    {
        Crud_CFTeams crud = new Crud_CFTeams();
        List<Class_CFTeams> lst1 = crud.usp_CFTeamsSelect().ToList();
        List<Class_CFTeams> lst = lst1.Where(x => x.del_status == "ACTIVE").ToList();
        ddlCftTeamSlNo.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlCftTeamSlNo.Items.Add(new ListItem(lst[cnt].CFTeamName, Convert.ToString(lst[cnt].CFTeamSlNo)));
        }
        ddlCftTeamSlNo.Items.Insert(0, "Select...");
    }

    void GetDetails()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        List<Class_parts> lst = lst1.Where(x => x.part_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtrevno.Text = lst[0].cp_revno;
        hdnrevno.Value = lst[0].cp_revno;
        txtrevdt.Text = lst[0].cp_revdt;
        txtrevreason.Text = lst[0].rev_reason;
        txtchangenature.Text = lst[0].change_nature;
        txtrevdt.Text = lst[0].cp_revdt;
        //txtPartNo.Enabled = false;
        //txtPartDescription.Enabled = false;
        //ddlcustomername.Enabled = false;
        //txtcustomerPartNo.Enabled = false;
        List<Class_DCR> lstdcr = DCRExists();
        if (lstdcr.Count > 0)
        {
            hdndcrslno.Value = lstdcr[0].dcr_slno.ToString();
            btnSubmit.Enabled = false;
            btnDelete.Enabled = false;
            txtpartIssueNo.Enabled = true;
            txtpartIssueDt.Enabled = true;

            txtcustomerIssueNo.Enabled = true;
            txtcustomerIssueDt.Enabled = true;
            if (!string.IsNullOrEmpty(hdnrevno.Value))
                txtrevno.Text = (Convert.ToInt32(hdnrevno.Value) + 1).ToString();
            else
                txtrevno.Text = "1";

            txtrevdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtrevreason.Text = lstdcr[0].Reason_For_Change;
            txtchangenature.Text = lstdcr[0].nature_of_change;
        }
        else
        {
            txtpartIssueNo.Enabled = false;
            txtpartIssueDt.Enabled = false;

            txtcustomerIssueNo.Enabled = false;
            txtcustomerIssueDt.Enabled = false;
        }
        txtPartNo.Text = lst[0].mstPartNo;
        txtPartDescription.Text = lst[0].PartDescription;
        txtpartIssueNo.Text = lst[0].partIssueNo;
        txtpartIssueDt.Text = lst[0].partIssueDt;
        ddlCftTeamSlNo.SelectedValue = lst[0].CftTeamSlNo.ToString();
        txtcustomerPartNo.Text = lst[0].customerPartNo;
        txtcustomerIssueNo.Text = lst[0].customerIssueNo;
        txtcustomerIssueDt.Text = lst[0].customerIssueDt;
        ddlkeycont.SelectedValue = lst[0].keyContact;
        txtkeyContactPhone.Text = lst[0].keyContactPhone;
        txtorgDate.Text = lst[0].orgDate;
        txtorganization.Text = lst[0].organization;
        txtorgApprovalDt.Text = lst[0].orgApprovalDt;
        txtoriginalDt.Text = lst[0].originalDt;
        txtorgDate.Text = lst[0].orgDate;
        txtcustapproval.Text = lst[0].custApproval;
        txtcustapprovaldt.Text = lst[0].custApprovalDt;
        txtcustQaApproval.Text = lst[0].custQaApproval;
        txtcustQaApprovalDt.Text = lst[0].custQaApprovalDt;
        txtotherApproval.Text = lst[0].otherApproval;
        txtotherApprovalDt.Text = lst[0].otherApprovalDt;
        ddlcustomername.SelectedValue = lst[0].Customer_name;
        ddlcpType.SelectedValue = lst[0].cpType;
        ddlCftTeamSlNo.SelectedValue = lst[0].CFTeamName.ToString();
        txtRemarks.Text = lst[0].Remarks;
        // ddlCftTeamSlNo.SelectedItem.Text = lst[0].CFTeamName.ToString();
        // ddlCftTeamSlNo.SelectedItem.Value = lst[0].CFTeamName.ToString();

        lbluploadfile1.Text = lst[0].uploadfile1;
        hrefrcFile1.HRef = "~/Documents/" + lst[0].uploadfile1;

        lbluploadfile2.Text = lst[0].uploadfile2;
        hrefrcFile2.HRef = "~/Documents/" + lst[0].uploadfile2;

        lbluploadfile3.Text = lst[0].uploadfile3;
        hrefrcFile3.HRef = "~/Documents/" + lst[0].uploadfile3;

        lbluploadfile4.Text = lst[0].uploadfile4;
        hrefrcFile4.HRef = "~/Documents/" + lst[0].uploadfile4;

        ddlActiveInactive.SelectedValue = lst[0].status1;

        Crud_CFTeamEmployees crd = new Crud_CFTeamEmployees();
        txtCftMembers.Text = crd.GetMembersList(lst[0].CftTeamSlNo);
        ddlActiveInactive.Enabled = true;
        txtcpnumber.Text = lst[0].cp_number;
        //txtrevno.Text = lst[0].cp_revno;
        //hdnrevno.Value= lst[0].cp_revno; 

        txtsuppcode.Text = lst[0].supplier_code;
        txtprocspec.Text = lst[0].proc_spec;
        txtihref.Text = lst[0].ih_testing_ref;
        ddlPrepd.SelectedValue = lst[0].preparedBy.ToString();
        ddlAppd.SelectedValue = lst[0].approvedBy.ToString();

    }
    protected List<Class_DCR> DCRExists()
    {
        string qry = "Select * from DCR where part_slno=" + Convert.ToInt32(hdnSlNo.Value) + " and change_area='Drawing Revision'   and  [del_status]='N' and [Submit_Status]='A'";
        using (Database db = new Database("connString"))
        {
            List<Class_DCR> lst = db.Query<Class_DCR>(qry).ToList();
            return lst;
        }

    }
    protected void btnRevise_Click(object sender, EventArgs e)
    {
        //TODO: 1. check if part issue no is changed
        //TODO: 2. check if cp rev. no is changed
        //TODO: 3. check if cp rev. dt is changed -- 2 revisions can happen on same date. Ignoring this
        //TODO: 4. rev. reason is entered
        //TODO: 5. check if control plan number is NOT changed
        //TOTD: 6. insert data in parts master
        //TODO: 7. insert data to part revision history
        //TODO: 8. update parts mapping with new sl.no
        //TODO: 9. insert control plan data for all operations and make old data obsolete
        //TODO:10. parts master set old data to obsolete
        //TODO:11. send mail to cft team about revision

        // validate if key values are changed
        using (Crud_parts crud = new Crud_parts())
        {
            Class_parts cls = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
            if (cls != null)
            {
                List<Class_DCR> lst = DCRExists();
                if (lst.Count > 0)
                {

                    if (txtpartIssueNo.Text.Equals(cls.partIssueNo) && txtpartIssueDt.Text.Equals(cls.partIssueDt) && txtcustomerIssueNo.Text.Equals(cls.customerIssueNo) && txtcustomerIssueDt.Text.Equals(cls.customerIssueDt))
                    {
                        ShowMessage("Part Issue No., Part Issue date, Customer Part Issue No. &  Customer Part Issue Date is same as of previous!", MessageType.Info);
                        return;
                    }



                    //if (txtpartIssueNo.Text.Equals(cls.partIssueNo) )
                    //{
                    //    ShowMessage("Part Issue No. is same as of previous!", MessageType.Info);
                    //    return;
                    //}
                    //if (txtrevno.Text.Equals(cls.cp_revno))
                    //{
                    //    ShowMessage("Control Plan Rev.No. is same as of previous", MessageType.Info);
                    //    return;
                    //}
                    //if (string.IsNullOrWhiteSpace(txtrevreason.Text))
                    //{
                    //    ShowMessage("Revision Reason is not entered", MessageType.Info);
                    //    return;
                    //}
                    //if (!txtcpnumber.Text.Equals(cls.cp_number))
                    //{
                    //    ShowMessage("Control Plan is changed", MessageType.Info);
                    //    return;
                    //}
                    //if (string.IsNullOrWhiteSpace(txtchangenature.Text))
                    //{
                    //    ShowMessage("Nature of Change is not entered", MessageType.Info);
                    //    return;
                    //}
                    //}
                    //}
                    // insert data in parts master
                    hdnMode.Value = "I";
                    hdnrevclick.Value = "R";
                    if (!string.IsNullOrEmpty(hdnrevno.Value))
                        txtrevno.Text = (Convert.ToInt32(hdnrevno.Value) + 1).ToString();
                    else
                        txtrevno.Text = "1";

                    txtrevdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtrevreason.Text = lst[0].Reason_For_Change;
                    txtchangenature.Text = lst[0].nature_of_change;
                    string partslno = hdnSlNo.Value;
                    SaveData();  // new slno is available in hndnewsl.value
                    if (hdnNewSl.Value != "0" || flag == 1)
                    {
                        // update parts mapping with new sl.no
                        string sqlPM = "update PartsMapping set part_slno=" + hdnNewSl.Value +
                               " where part_slno=" + hdnSlNo.Value + " ";
                        using (Database db = new Database("connString"))
                        {
                            db.Execute(sqlPM);
                        }

                        // insert data to part revision history
                        Class_part_revision_history clsRev = new Class_part_revision_history
                        {
                            part_slno = Convert.ToInt32(hdnNewSl.Value),
                            rev_no = txtrevno.Text,
                            rev_date = DateTime.Now.ToString("dd/MM/yyyy"),
                            rev_reasons = txtrevreason.Text,
                            change_nature = txtchangenature.Text,
                            revision_done_in = "Parts"

                        };
                        //using (Crud_part_revision_history crudRev = new Crud_part_revision_history())
                        //{
                        //    crudRev.Insert(clsRev);
                        //}

                        // insert control plan data for all operations and make old data obsolete
                        using (Database db = new Database("connString"))
                        {
                            db.Execute("; exec usp_CpRevision_insert @@old_partslno=@0,@@new_partslno=@1,@@newrev_date=@2,@@newrev_reason=@3,@@newdcr_slno=@4,@@Reason_For_Change=@5,@@nature_of_change=@6,@@rev_no=@7;", hdnSlNo.Value, hdnNewSl.Value, clsRev.rev_date, clsRev.rev_reasons, Convert.ToInt32(hdndcrslno.Value), clsRev.rev_reasons, clsRev.change_nature, txtrevno.Text);

                            //                        db.Execute("; exec Revise_SOP_For_New_Part1 @@old_partslno=@0, @@new_partslno=@1, @@rev_date=@2, @@rev_reason=@3, @@dcr_slno=@4,@@nature_of_Change=@5,@@reason_For_Change=@5,@@rev_no=@6;",
                            //hdnSlNo.Value, hdnNewSl.Value, clsRev.rev_date, clsRev.rev_reasons, Convert.ToInt32(hdndcrslno.Value),txtchangenature.Text,txtrevreason.Text, Convert.ToInt32(txtrevno.Text));

                            //                        db.Execute("exec Revise_SOP_For_New_Part1 @part_slno=@0, @rev_no=@1, @rev_date=@2, @rev_reason=@3, @dcr_slno=@4, @nature_of_Change=@5, @reason_For_Change=@6;",
                            //hdnSlNo.Value, Convert.ToInt32(txtrevno.Text), clsRev.rev_date, clsRev.rev_reasons, Convert.ToInt32(hdndcrslno.Value), txtchangenature.Text, txtrevreason.Text);
                            string qryGetGroups = "Select distinct  Group_Id  from Sop_Mapping where Obsolete='N'   and del_status='N'  and part_slno=@0";
                            List<int> group_ids = db.Fetch<int>(qryGetGroups, hdnSlNo.Value);
                            foreach (int i in group_ids)
                            {





                                string gr = i.ToString();

                                string sql = "select  COALESCE(MAX(rev_no), 0) + 1 from SOP_Mapping  where Group_Id=@gr";


                                int rev = db.ExecuteScalar<int>(sql, new { gr });


                                int rowsAffected1 = db.Execute(";EXEC usp_InitiateSOPRevision @@Group_Id=@0, @@rev_reason=@1, @@rev_no=@2, @@rev_date=@3, @@dcr_slno=@4, @@Reason_For_Change=@5, @@nature_of_change=@6",
                                    i, clsRev.rev_reasons, rev, clsRev.rev_date, Convert.ToInt32(hdndcrslno.Value), clsRev.rev_reasons, clsRev.change_nature);

                                string updatenewpart_slno = @"
        UPDATE SOP_Mapping
        SET part_slno = @0
        WHERE Group_Id = @1
          AND part_slno = @2
          AND rev_no = (
              SELECT MAX(rev_no)
              FROM SOP_Mapping
              WHERE Group_Id = @3
          )";

                                db.Execute(updatenewpart_slno, hdnNewSl.Value, i, hdnSlNo.Value, i);

                                 updatenewpart_slno = @"
        UPDATE sop_header_new
        SET part_slno = @0
        WHERE Group_Id = @1
          AND part_slno = @2
          AND map_slno in = (
              SELECT map_slno 
              FROM SOP_Mapping
              WHERE Group_Id = @3   and obsolete='N' and del_status='N'
          )";

                                db.Execute(updatenewpart_slno, hdnNewSl.Value, i, hdnSlNo.Value, i);



                            }


                            string sqlDeleteDCR = "UPDATE DCR SET del_status='Y', submit_status='O' " +
                                     "WHERE part_slno=@partSlno " +
                                     "AND change_area='Drawing Revision' " +
                                     "AND dcr_slno=@dcr";
                            var parameters = new
                            {
                                partSlno = Convert.ToInt32(partslno),
                                dcr = Convert.ToInt32(hdndcrslno.Value)
                            };
                            db.Execute(sqlDeleteDCR, parameters);

                        }

                        // parts master set old data to obsolete
                        //string sqlPt = "update parts set obsolete='Y' where part_slno <>" + hdnNewSl.Value + " and mstPartNo='" + txtPartNo.Text + "'";
                        string sqlPt = "update parts set obsolete='Y' where part_slno =" + hdnSlNo.Value;
                        using (Database db = new Database("connString"))
                        {
                            db.Execute(sqlPt);
                        }

                        // send mail to cft team about revision
                        //  PrepareForMail();
                        flag = 0;
                    }
                }
                else { ShowMessage("Please raise a request through DCR", MessageType.Info); }
            }
        }
    }
    protected void btnRevise_Click2(object sender, EventArgs e)
    {
        hdnMode.Value = "I";
        hdnrevclick.Value = "R";
        SaveData();

        // update the control plan tables
        string sqlCP = "update controlplan set Submitstatus= 'N', part_slno=" + hdnNewSl.Value +
                    " where part_slno=" + hdnSlNo.Value + " and obsolete='N' ";
        string sqlPM = "update PartsMapping set part_slno=" + hdnNewSl.Value +
                   " where part_slno=" + hdnSlNo.Value + " ";

        string sqlPt = "update parts set obsolete='Y' where part_slno <>" + hdnNewSl.Value + " and mstPartNo='" + txtPartNo.Text + "'";

        using (Database db = new Database("connString"))
        {
            db.Execute(sqlCP);
            db.Execute(sqlPM);
            db.Execute(sqlPt);
        }
        ClearData();
    }

    String GetDelStat(string part_slno)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_parts>(" where part_slno=@0", part_slno).del_status;
        }
    }

    void PrepareForMail()
    {
        // get cft member list
        List<string> tomails = new List<string>();
        List<string> ccmails = new List<string>();
        using (Crud_CFTeamEmployees crud = new Crud_CFTeamEmployees())
        {
            string emails = crud.GetMemberMailList(ddlCftTeamSlNo.SelectedValue);
            tomails = emails.Split(',').ToList();
        }

        // get prpeared by and approved by in cc list
        using (Database db = new Database("connString"))
        {
            string sql1 = "select emailid from employees where employeeslno=@0";
            string prepmail = db.ExecuteScalar<string>(sql1, ddlPrepd.SelectedValue);
            string appdmail = db.ExecuteScalar<string>(sql1, ddlAppd.SelectedValue);

            if (!string.IsNullOrWhiteSpace(prepmail))
            {
                ccmails.Add(prepmail);
            }
            if (!string.IsNullOrWhiteSpace(appdmail))
            {
                ccmails.Add(appdmail);
            }
        }

        string mailsubj = "Part " + txtPartNo.Text + " is revised !";
        string styl = "<style>table{max-width:100%;background-color:transparent;font-size:14px}th{text-align:left}.table{width:100%;margin-bottom:20px}.table>tbody>tr>td,.table>tbody>tr>th,.table>tfoot>tr>td,.table>tfoot>tr>th,.table>thead>tr>td,.table>thead>tr>th{padding:8px;line-height:1.428571429;vertical-align:top;border-top:1px solid #ddd}.table>thead>tr>th{vertical-align:bottom;border-bottom:2px solid #ddd}.table>tbody+tbody{border-top:2px solid #ddd}.table .table{background-color:#fff}.table-striped>tbody>tr:nth-child(odd)>td,.table-striped>tbody>tr:nth-child(odd)>th{background-color:#f9f9f9}body{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:14px;line-height:1.428571429;color:#333;background-color:#fff}h2{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:500;line-height:1.1;color:inherit;text-align:center}</style>";

        // Get the current HTTP address (URL)
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
        string httpAddress = baseUrl + "/meicp/loginpage.aspx";

        string bodycnt = "<center><b><label style='background-color:#0198FF; color:White;font-family:Calibri;font-size:medium;'>&nbsp;THIS IS AN AUTOGENERATED MAIL. DO NOT REPLY TO THIS!! &nbsp;</label></b></center>" +
            "<body style='font-family:Calibri;font-size:medium;'>Hi, <br/><br/>Please find the revised details below:" +
            "<table class='table table-striped'>" +
            "<tr><th>Part No</th><td>" + txtPartNo.Text + "</td></tr>" +
            "<tr><th>Part Description</Th><td>" + txtPartDescription.Text + "</td></tr>" +
            "<tr><th>Part Issue No.</th><td>" + txtpartIssueNo.Text + "</td></tr>" +
            "<tr><th>Part Issue Date</th><td>" + txtpartIssueDt.Text + "</td></tr>" +
            "<tr><th>Customer Part Issue No.</th><td>" + txtcustomerIssueNo.Text + "</td></tr>" +
            "<tr><th>Customer Part Issue Date</th><td>" + txtcustomerIssueDt.Text + "</td></tr>" +
            "<tr><th>Control Plan Rev.No.</th><td>" + txtrevno.Text + "</td></tr>" +
            "<tr><th>Control Plan Rev.Date</th><td>" + txtrevdt.Text + "</td></tr>" +
            "<tr><th>Revision Reason</th><td>" + txtrevreason.Text + "</td></tr></table>" +
            "<p><b>Note:</b>Control Plan for all operations of this part number are revised and is in Draft mode. You need to submit for approval.</p>" +
            "<p><a href='" + httpAddress + "'>Click to open portal</a></p>";
        string mailBody = "<html><head>" + styl + "</head><body>" + bodycnt + "</body></html>";

        // send mail
        mail ml = new mail();
        ml.SendMail(toMailId: tomails, mailSubject: mailsubj, bodyText: mailBody, ccMailId: ccmails);
    }

}
