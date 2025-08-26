using Newtonsoft.Json;
using NPoco;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;



public partial class ControlPlan2 : System.Web.UI.Page
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
        hdnemplslno.Value = userInfo[0].EmployeeSlNo.ToString();
        hdnuser.Value = userInfo[0].CanApprove;
        if (!Page.IsPostBack)
        {
            LoadParts();

            if (Request.QueryString.HasKeys())
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                string submitst = GetSubmitStat(hdnSlNo.Value);

                if (userInfo[0].isAdmin == "Y" || userInfo[0].CanApprove == "Y")
                {
                    if (submitst == "Y")
                    {
                        //if(ddlAppd.SelectedIndex > 0)
                        //{
                        //    if(Convert.ToInt16(ddlAppd.SelectedValue)== userInfo[0].EmployeeSlNo)
                        //    {
                        //        btnApproved.Enabled = true;
                        //    }
                        //    else
                        //        btnApproved.Enabled = true;
                        //}
                        //else
                           // btnApproved.Enabled = true;
                    }

                    if (submitst == "A")
                    {
                        btnirev.Enabled = true;
                        btnDelete.Enabled = false;
                        btnSave.Enabled = false;
                        btnSubmit.Enabled = false;
                    }
                }
                //if (userInfo[0].CanPrepare == "Y")
                //{
                //    btnSubmit.Enabled = false;
                //}
                //else 
                //{
                //    btnSubmit.Enabled = true;                
                //}

                //btnSubmit.Enabled = true;
                if (userInfo[0].CanPrepare == "Y")
                {
                    btnSave.Enabled = true;
                }
                else btnSave.Enabled = false;
                hdnEditMode.Value = "E";
                // GetDetails();
                LoadData();
                if (userInfo[0].isAdmin == "Y")
                    btnDelete.Enabled = true;
                else
                    btnDelete.Enabled = false;
            }
            else
            {
                btnirev.Enabled = false;
                btnDelete.Enabled = false;
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

        ddlpart_slno.Items.Insert(0, "Select...");

    }
    void LoadData()
    {
        using (Crud_ControlPlan cp = new Crud_ControlPlan())
        {
            Class_ControlPlan c = cp.usp_ControlPlanSelect().Where(x => x.cp_slno == Convert.ToInt32(hdnSlNo.Value)).FirstOrDefault();
            ListItem ctlst = ddlcpType.Items.FindByText(c.cpType);
            if (ctlst != null)
            {
                ddlcpType.SelectedIndex = ddlcpType.Items.IndexOf(ctlst);
            }
            ListItem li = ddlpart_slno.Items.FindByValue(c.part_slno.ToString());
            if (li != null)
            {
                LoadParts();
                ListItem oli2 = ddlpart_slno.Items.FindByValue(c.part_slno.ToString());
                if (oli2 != null)
                {
                    ddlpart_slno.SelectedIndex = ddlpart_slno.Items.IndexOf(oli2);
                }
                //ddlpart_slno.SelectedIndex = ddlpart_slno.Items.IndexOf(li);
                LoadOperationsFromparts();

                ListItem oli = ddloperation_slno.Items.FindByValue(c.operation_slno.ToString());
                if (oli != null)
                {
                    ddloperation_slno.SelectedIndex = ddloperation_slno.Items.IndexOf(oli);
                }

                LoadMachinesFromparts();

                ListItem mli = ddlmachine_slno.Items.FindByValue(c.machine_slno.ToString());
                if (mli != null)
                {
                    ddlmachine_slno.SelectedIndex = ddlmachine_slno.Items.IndexOf(mli);
                }

                //ListItem prepli = ddlPrepd.Items.FindByValue(c.preparedBy.ToString());
                //ListItem appdli = ddlAppd.Items.FindByValue(c.approvedBy.ToString());
                //if (prepli != null)
                //{
                //    ddlPrepd.SelectedIndex = ddlPrepd.Items.IndexOf(prepli);
                //}
                //if (appdli != null)
                //{
                //    ddlAppd.SelectedIndex = ddlAppd.Items.IndexOf(appdli);
                //}
            }
            hdnsubmitstatus.Value = c.Submitstatus;
            hdnuserrevno.Value = c.user_revNo;
            hdnuserrevdate.Value = c.user_revDt;
            hdnrevreason.Value = c.rev_reason;
            hdnchangenature.Value = c.nature_of_Change;
            hdnrevnumber.Value = c.rev_no.ToString(); 
            hdnrevdt.Value = c.rev_date;

            hdndcr_slno.Value = c.dcr_slno.ToString();


            // enable disable buttons
            if (c.is_approved == true && (c.Submitstatus == "Y" || c.Submitstatus == "A"))
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
                btnApproved.Enabled = false;
               // btnrevision.Enabled = true;
            }
            else if (c.is_approved == false && c.Submitstatus == "Y")
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
                btnApproved.Enabled = true;
               // btnrevision.Enabled = false;
            }
            else if (c.is_approved == false && c.Submitstatus == "N")
            {
                btnSave.Enabled = true;
                btnSubmit.Enabled = true;
                btnApproved.Enabled = false;
                //btnrevision.Enabled = false;
            }
            if (userInfo[0].isAdmin == "Y")
                hdnadmin.Value = "Y";
            else
                hdnadmin.Value = "N";

           


        }
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
                hdnCpType.Value = cpType;
            }
        }
    }

    void LoadCustomerSl()
    {
        string sql = @"select cust_slno from parts p 
inner join customers c on c.cust_name=p.Customer_name
where p.part_slno=@0";

        using (Database db = new Database("connString"))
        {
            hdnCustSl.Value = db.ExecuteScalar<string>(sql, ddlpart_slno.SelectedValue);
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

                ddlmachine_slno.SelectedIndex = 0;
                ddlmachine_slno.Enabled = false;
            }
        }
    }

    //void LoadMachinesFromparts()
    //void  LoadProcessNumFromMapping()
    //{
    //    using (Database db = new Database("connString"))
    //    {
    //        if (ddloperation_slno.SelectedIndex > 0 && ddlpart_slno.SelectedIndex > 0)
    //        {
    //            string PrNo = db.ExecuteScalar<string>("select PM.process_no FROM parts P, partsmapping PM where P.part_slno =PM.part_slno and P.part_slno=@0 and PM.operation_slno =@1", Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
    //            hdnProcessNo.Value = PrNo;

    //        }
    //    }
    //}
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
        }


        ddloperation_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddloperation_slno.Items.Add(new ListItem(lst[cnt].OperationDesc, Convert.ToString(lst[cnt].operation_slno)));
        }

        ddloperation_slno.Items.Insert(0, "Select...");

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


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteData();
        ShowMessage("Records Deleted Successfully", MessageType.Success);
        // LoadGridData();
        ClearData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }

    void ClearData()
    {
        //txtMachineCode.Text = string.Empty;
        //txtMachineDesc.Text = string.Empty;
        //ddloperation_slno.SelectedIndex = 0;
        //hdnMode.Value = "I";
        hdnSlNo.Value = "";
        hdnProcessNo.Value = "";
        //LoadGridData();
        hdnsubmitstatus.Value = "N";
        hdnEditMode.Value = "I";
        btnSubmit.Enabled = false;
        btnDelete.Enabled = false;
        ddlmachine_slno.SelectedIndex = 0;
        ddloperation_slno.SelectedIndex = 0;
        ddlpart_slno.SelectedIndex = 0;
        ddlcpType.SelectedIndex = 0;
        btnApproved.Enabled = false;

    }

    void DeleteData()
    {
        using (Database db = new Database("connString"))
        {
            // get partsl no, machine slno, opern
            Crud_ControlPlan crudcp = new Crud_ControlPlan();
            List<Class_ControlPlan> lstcp = crudcp.usp_ControlPlanSelect().Where(x => x.cp_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

            int partslno = 0;
            int opernslno = 0;
            int machineslno = 0;

            if (lstcp.Count > 0)
            {
                partslno = lstcp[0].part_slno;
                opernslno = lstcp[0].operation_slno;
                machineslno = lstcp[0].machine_slno;
            }

            //Delete control plan
            db.DeleteWhere<Class_ControlPlan_Child>(" cp_slno=@0", Convert.ToInt32(hdnSlNo.Value));
            db.DeleteWhere<Class_ControlPlan>(" cp_slno=@0", Convert.ToInt32(hdnSlNo.Value));

            // set Obsolete ='N' for previous revision cp

            string qryupdate = "update controlplan set Obsolete ='N', Submitstatus='N' where part_slno=" + partslno + " and operation_slno=" + opernslno + "  and  machine_slno=" + machineslno + " and "
                               + " rev_no =(SELECT MAX(CAST(c.rev_no AS INT)) from controlplan c where c.part_slno=controlplan.part_slno AND c.operation_slno=controlplan.operation_slno)";

            db.Execute(qryupdate);
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();
    }
    void SaveData()
    {
        try
        {
            //System.Data.SqlClient.SqlTransaction transaction;

            using (Database db = new Database("connString"))
            {
                Class_ControlPlan CData = new Class_ControlPlan();
                Class_ControlMethods MData = new Class_ControlMethods();
                CData.cpType = ddlcpType.SelectedValue;
                CData.part_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
                CData.partno = ddlpart_slno.Text;
                CData.machine_slno = Convert.ToInt32(ddlmachine_slno.SelectedValue);
                CData.operation_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);
                CData.Submitstatus = "N";
                CData.Obsolete = "N";
                CData.is_approved = false;
                if (!string.IsNullOrEmpty(hdnrevnumber.Value))
                    CData.rev_no = Convert.ToInt32(hdnrevnumber.Value);
                CData.rev_date = hdnrevdt.Value;
                //Load Process Number
                if (ddloperation_slno.SelectedIndex > 0 && ddlpart_slno.SelectedIndex > 0)
                {
                    string process_no = db.ExecuteScalar<string>("select PM.process_no FROM parts P, partsmapping PM where P.part_slno =PM.part_slno and P.part_slno=@0 and PM.operation_slno =@1", Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
                    // string cpType = db.ExecuteScalar<string>("select cptype from parts where part_slno=@0", Convert.ToInt16(ddlpart_slno.SelectedValue));   
                    hdnProcessNo.Value = process_no;
                }

                CData.process_no = hdnProcessNo.Value;
                var cline = JsonConvert.DeserializeObject<List<Class_ControlPlan_Child>>(hdnchild.Value);
                if (cline.Count > 0)
                {
                    if (hdnEditMode.Value == "I")
                    {
                        string qry = "select * from controlplan where part_slno=@0 and obsolete='N' and operation_slno=@1 ";
                        qry += " and rev_no =(SELECT MAX(cast(c.rev_no AS INT)) from controlplan c where c.part_slno=controlplan.part_slno AND c.operation_slno=controlplan.operation_slno)";

                        int rv = db.ExecuteScalar<int>(qry, Convert.ToInt16(ddlpart_slno.SelectedValue), Convert.ToInt16(ddloperation_slno.SelectedValue));
                        if (rv == 0)
                        {
                            var slno = db.Insert<Class_ControlPlan>(CData);
                            hdnSlNo.Value = slno.ToString();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Record Already exists for this part and operation combination');window.location='controlplan_qry.aspx';", true);
                        }
                    }

                    else
                    {
                        //CData = db.SingleOrDefaultById<Class_ControlPlan>(hdnSlNo.Value);
                        //CData.cpType = ddlcpType.SelectedValue;

                        CData.cp_slno = Convert.ToInt16(hdnSlNo.Value);
                        if (!string.IsNullOrEmpty(hdnrevnumber.Value) && !string.IsNullOrEmpty(hdnrevdt.Value))
                        {
                            CData.user_revNo = (hdnrevnumber.Value);
                            CData.user_revDt = hdnrevdt.Value;
                            CData.rev_reason = hdnrevreason.Value;
                            CData.reason_For_Change= hdnrevreason.Value;
                            CData.nature_of_Change = hdnchangenature.Value;
                            CData.rev_no = Convert.ToInt32(hdnrevnumber.Value);
                            CData.rev_date = hdnrevdt.Value;
                            if (!string.IsNullOrWhiteSpace(hdndcr_slno.Value))
                                CData.dcr_slno = Convert.ToInt32(hdndcr_slno.Value);
                        }
                        db.Update(CData);
                    }
                }
                db.DeleteWhere<Class_ControlPlan_Child>(" cp_slno=@0", Convert.ToInt32(hdnSlNo.Value));
                foreach (Class_ControlPlan_Child cl in cline)
                {
                    cl.cp_slno = Convert.ToInt32(hdnSlNo.Value);
                    if (cl.FreqDesc != null)
                    {
                        if (cl.FreqDesc != "" && cl.FreqDesc.Trim().Length > 0)
                        {
                            cl.freq_slno = GetFreqslno(cl.FreqDesc);
                        }
                    }
                    if (cl.methodDesc != null)
                    {
                        if (cl.methodDesc != "" && cl.methodDesc.Trim().Length > 0)
                        {
                            cl.method_slno = GetControlslno(cl.methodDesc);
                        }
                    }
                    if (cl.evalTech != null)
                    {

                        if (cl.evalTech != "" && cl.evalTech.Trim().Length > 0)
                        {
                            cl.evalTech_slno = GetEveltechslno(cl.evalTech);
                        }
                    }

                    if (cl.splfilename != null)
                    {

                        if (cl.splfilename != "" && cl.splfilename.Trim().Length > 0)
                        {
                            cl.splChar_slno = GetSplcharSlno(cl.splfilename);
                        }
                    }

                    // cl.pdt_slno = db.SingleOrDefault<clsProduct>(" where pdt_code=@0", cl.pdt_code).Pdt_Slno;

                    if (cl.methodDesc2 != null)
                    {
                        if (cl.methodDesc2 != "" && cl.methodDesc2.Trim().Length > 0)
                        {
                            cl.method_slno2 = GetControlslno(cl.methodDesc2);
                        }
                    }
                    if (cl.evalTech2 != null)
                    {
                        if (cl.evalTech2 != "" && cl.evalTech2.Trim().Length > 0)
                        {
                            cl.evalTech_slno2 = GetEveltechslno(cl.evalTech2);
                        }
                    }
                    if (cl.FreqDesc2 != null)
                    {
                        if (cl.FreqDesc2 != "" && cl.FreqDesc2.Trim().Length > 0)
                        {
                            cl.freq_slno2 = GetFreqslno(cl.FreqDesc2);
                        }
                    }

                    db.Insert<Class_ControlPlan_Child>(cl);
                    btnSubmit.Enabled = true;
                }
            }
            //    ShowMessage("Updated", MessageType.Info);
            Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Updated');", true);
        }
        catch (Exception ex2)
        {
            //  ShowMessage("Enter Valid Data" + ex2.Message, MessageType.Info);
            throw ex2;
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        using (Database db = new Database("connString"))
        {
            string sqlselectcountformail = string.Empty;
            int cnt = 0;
            if (!string.IsNullOrEmpty(hdndcr_slno.Value))
            {
                sqlselectcountformail = "Select count(*) from ControlPlan where  part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) + " and dcr_slno=" + Convert.ToInt32(hdndcr_slno.Value) + " and ( Submitstatus IS NULL or Submitstatus='N') ";
                cnt = db.ExecuteScalar<int>(sqlselectcountformail);
            }
            else
            {
                sqlselectcountformail = "Select count(*) from ControlPlan where  part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) + " and ( Submitstatus IS NULL or Submitstatus='N') ";
                cnt = db.ExecuteScalar<int>(sqlselectcountformail);
            }
                Class_ControlPlan c = db.SingleOrDefaultById<Class_ControlPlan>(hdnSlNo.Value);
            c.Submitstatus = "Y";
            c.CP_Submit_DateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(hdnrevnumber.Value))
                c.rev_no = Convert.ToInt32(hdnrevnumber.Value);
            c.rev_date = hdnrevdt.Value;
            c.rev_reason = hdnrevreason.Value;
            if (!string.IsNullOrEmpty(hdndcr_slno.Value))
                c.dcr_slno = Convert.ToInt32(hdndcr_slno.Value);
            db.Update(c);
            string ccmail = string.Empty;
            if (!string.IsNullOrEmpty(hdndcr_slno.Value))
            {
                string ccsql = "select emailid from employees where employeeslno= (select  Request_By from DCR where dcr_slno=" + Convert.ToInt32(hdndcr_slno.Value) + ")";
                ccmail = db.ExecuteScalar<string>(ccsql);
            }
            // get approval mail ids
            string sql = @"select e.emailid
from parts p
inner join employees e on e.employeeslno=p.approvedBy
where p.part_slno=@0";

            List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql, ddlpart_slno.SelectedValue);
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
            //ccmails.Add(userInfo[0].EmailId);
            if (!string.IsNullOrEmpty(ccmail))
                ccmails.Add(ccmail);

            string sqladmin = @"select emailid from employees where del_status='N' and isAdmin='Y'";

            List<Class_Employees> lstempladmin = db.Fetch<Class_Employees>(sqladmin);
            if (lstempladmin.Count > 0)
            {
                foreach (Class_Employees c1 in lstempladmin)
                {
                    ccmails.Add(c1.EmailId);
                }
            }
            else
            {
                ccmails.Add(userInfo[0].EmailId);
            }
            tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
            ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
            //string sqlselectcountformail = string.Empty;
            //int cnt = 0;
            //if (!string.IsNullOrEmpty(hdndcr_slno.Value))
            //{
            //    sqlselectcountformail = "Select count(*) from DCR_Status where dcr_slno=" + Convert.ToInt32(hdndcr_slno.Value) + " and status = 'pending'";
            //    cnt = db.ExecuteScalar<int>(sqlselectcountformail);
            //}
            if (cnt == 1)
            {

                PrepareForMail(tomails, ccmails, "for_approval");
            }
        }
        ShowMessage("Updated", MessageType.Info);
        ClearData();
    }

    protected void btnApproved_Click(object sender, EventArgs e)
    {

        using (Database db = new Database("connString"))
        {
            string sqlselectcountformail = string.Empty;
            int cnt = 0;
            if (!string.IsNullOrEmpty(hdndcr_slno.Value))
            {
                sqlselectcountformail = "Select count(*) from ControlPlan where  part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) + " and dcr_slno=" + Convert.ToInt32(hdndcr_slno.Value) + " and ( Submitstatus IS NULL or Submitstatus='N' or Submitstatus='Y') ";
                cnt = db.ExecuteScalar<int>(sqlselectcountformail);
            }
            else
            {
                sqlselectcountformail = "Select count(*) from ControlPlan where  part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) + " and ( Submitstatus IS NULL or Submitstatus='N' or Submitstatus='Y') ";
                cnt = db.ExecuteScalar<int>(sqlselectcountformail);
            }

                Class_ControlPlan c = db.SingleOrDefaultById<Class_ControlPlan>(hdnSlNo.Value);
            c.Submitstatus = "A";
            c.CP_Approved_DateTime = DateTime.Now;
            c.is_approved = true;
            if (!string.IsNullOrEmpty(hdnrevnumber.Value))
                c.rev_no = Convert.ToInt32(hdnrevnumber.Value);
            c.rev_date = hdnrevdt.Value;
            c.rev_reason = hdnrevreason.Value;
            if (!string.IsNullOrEmpty(hdndcr_slno.Value))
                c.dcr_slno = Convert.ToInt32(hdndcr_slno.Value);
            //c.approvedby_dt = DateTime.Today.ToString("dd/MM/yyyy");
            //c.approvedBy = Convert.ToInt32(ddlAppd.SelectedValue);
            db.Update(c);

            string opn = ddloperation_slno.SelectedValue.ToString();
            string sqlselect = "";

            int cntofDCRclosed = 0;

            object parametersa;

            if (c.dcr_slno.HasValue)
            {
                sqlselect = "SELECT COUNT(*) FROM ControlPlan WHERE dcr_slno = @dcr_slno AND is_approved = 0";
                parametersa = new { dcr_slno = c.dcr_slno.Value };
            }
            else
            {
                sqlselect = "SELECT COUNT(*) FROM ControlPlan WHERE dcr_slno IS NULL AND is_approved = 0";
                parametersa = null;
            }
            cntofDCRclosed = db.ExecuteScalar<int>(sqlselect, parametersa);
      

            string qry = "update Controlplan  set Obsolete='Y'  where part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) + "  and  operation_slno=" + Convert.ToInt32(ddloperation_slno.SelectedValue) + " and machine_slno=" + Convert.ToInt32(ddlmachine_slno.SelectedValue) + "    and cp_slno <> " + Convert.ToInt32(hdnSlNo.Value);
            db.Execute(qry);
            if (cntofDCRclosed <= 0)
            {
                string likePattern = "%," + opn + ",%";

                string sqlDeleteDCR = "UPDATE DCR SET  submit_status='O',del_status='Y' " +
                                      "WHERE part_slno=@partSlno " +
                                      "AND (',' + operations + ',') LIKE @likePattern " +
                                      "AND change_area='CP' " +
                                      "AND Submit_status='R' " +
                                      "AND del_status='N' " +
                                      "AND dcr_slno=@dcr";

                var parameters = new
                {
                    partSlno = Convert.ToInt32(ddlpart_slno.SelectedValue),
                    likePattern, // This includes the wildcards
                    dcr = c.dcr_slno
                };

                int rowsAffected = db.Execute(sqlDeleteDCR, parameters);
            }
            string ccmail = string.Empty;
            if (!string.IsNullOrEmpty(hdndcr_slno.Value))
            {
                string ccsql = "select emailid from employees where employeeslno= (select  Request_By from DCR where dcr_slno=" + Convert.ToInt32(hdndcr_slno.Value) + ")";
                ccmail = db.ExecuteScalar<string>(ccsql);
            }
            // get approval mail ids
            string sql2 = @"select e.emailid from parts p
inner join employees e on e.employeeslno = p.preparedby
where p.part_slno=@0";

            string sql3 = @"select e.emailid from parts p
inner join employees e on e.employeeslno = p.approvedby
where p.part_slno=@0";

            string sqladmin = @"select emailid from employees  where del_status='N'   and isAdmin='Y'";
            List<Class_Employees> lstempladmin = db.Fetch<Class_Employees>(sqladmin);
            List<Class_Employees> lstempl1 = db.Fetch<Class_Employees>(sql2, Convert.ToInt32(ddlpart_slno.SelectedValue));
            List<string> tomails = new List<string>();
            if (lstempl1.Count > 0)
            {
                foreach (Class_Employees c1 in lstempl1)
                {
                    tomails.Add(c1.EmailId);
                }
            }
        

            if (tomails.Count == 0)
            {
                tomails.Add(userInfo[0].EmailId);
            }
            if (lstempladmin.Count > 0)
            {
                foreach (Class_Employees c1 in lstempladmin)
                {
                    tomails.Add(c1.EmailId);
                }
            }
            tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));

            List<Class_Employees> lstempl2 = db.Fetch<Class_Employees>(sql3, Convert.ToInt32(ddlpart_slno.SelectedValue));
            List<string> ccmails = new List<string>();
            if (lstempl2.Count > 0)
            {
                foreach (Class_Employees c2 in lstempl2)
                {
                    ccmails.Add(c2.EmailId);
                }
            }
            else
            {
                ccmails.Add(userInfo[0].EmailId);
            }
            if (!string.IsNullOrEmpty(ccmail))
                ccmails.Add(ccmail);
            ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));

            if (cnt == 1)
            {
             
                if (!string.IsNullOrEmpty(hdndcr_slno.Value))
                {
                    Class_part_revision_history partrev = new Class_part_revision_history();
                    partrev.part_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
                    partrev.rev_no = hdnrevnumber.Value;
                    partrev.rev_date = hdnrevdt.Value;
                    partrev.rev_reasons = c.reason_For_Change;
                    partrev.change_nature = c.nature_of_Change;
                    partrev.revision_done_in = "CP";
                    db.Insert<Class_part_revision_history>(partrev);
                }
                PrepareForMail(tomails, ccmails, "approved");
                if (!string.IsNullOrEmpty(hdndcr_slno.Value))
                {
                    string sql = "Update Parts set cp_revno=@0 , cp_revdt=@1,[rev_reason]=@2   ,[change_nature]=@3  where part_slno=@4  and obsolete ='N'  and del_status='N'";
                    db.Execute(sql, hdnrevnumber.Value, hdnrevdt.Value, c.reason_For_Change, c.nature_of_Change, Convert.ToInt32(ddlpart_slno.SelectedValue));
                }
            }

        }

        ShowMessage("Approved", MessageType.Info);
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
    void LoadGridData()
    {
        Crud_machines crud = new Crud_machines();
        List<Class_machines> lst = crud.usp_machinesSelect().ToList();

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
        LoadCustomerSl();
    }

    protected void ddloperation_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        //string qry = "select * from controlplan where part_slno=@0  and operation_slno=@1 and (submitstatus IS null OR submitstatus='N') ";
        string qry = "select * from controlplan where part_slno=@0 and obsolete='N' and operation_slno=@1 ";

        qry += " and rev_no =(SELECT MAX(cast(c.rev_no AS INT)) from controlplan c where c.part_slno=controlplan.part_slno AND c.operation_slno=controlplan.operation_slno)";


        using (Database db = new Database("connString"))
        {

            int rv = db.ExecuteScalar<int>(qry, Convert.ToInt16(ddlpart_slno.SelectedValue), Convert.ToInt16(ddloperation_slno.SelectedValue));
            if (rv > 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Record Already exisits for this part and operation combination');window.location='controlplan_qry.aspx';", true);
            }
        }


        LoadMachinesFromparts();

        //LoadMachinesFromparts();
    }

    int GetEveltechslno(string evltech)
    {

        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_EvaluationTech>(" where evalTech=@0", evltech).evalTech_slno;
        }
    }

    int GetFreqslno(string freq)
    {

        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_SampleFrequency>(" where FreqDesc=@0", freq).freq_slno;
        }
    }

    int GetControlslno(string ctrl)
    {

        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_ControlMethods>(" where methodDesc=@0", ctrl).method_slno;
        }
    }

    int GetSplcharSlno(string filename)
    {

        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_SpecialChars>(" where splCharFile=@0", filename).splChar_slno;
        }
    }

    protected void btnrevision_Click(object sender, EventArgs e)
    {
        int dcr = 0;
        txtUserRevNo.ReadOnly = false; txtUserRevDt.ReadOnly = false; txtreason.ReadOnly = false; txtnatureofchange.ReadOnly = false;
        int cntformail = 0; int cnt = 0;
        string opn = ddloperation_slno.SelectedValue.ToString();
        using (Crud_ControlPlan cd = new Crud_ControlPlan())
        {
            Database db = new Database("connString");
            string sqlupdateCP = "select dcr_slno from DCR   where part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) +
               "  AND (',' + operations + ',') LIKE '%,' + @opn + ',%'    and  change_area='CP' " +
               " and Submit_status='A' and del_status='N'";
             dcr = db.ExecuteScalar<int>(sqlupdateCP, new { opn });
            //cd.usp_InitiateControlPlanRevision(Convert.ToInt32(hdnSlNo.Value), txtrevreason.Text, txtUserRevNo.Text, txtUserRevDt.Text, dcr);
            //db.Execute(";exec usp_InitiateControlPlanRevision @@cp_slno=@0,@@rev_reason = @1, @@rev_no = @2, @@rev_date = @3, @@dcr_slno = @4,@@Reason_For_Change = @5,@@nature_of_change = @6", Convert.ToInt32(hdnSlNo.Value), txtrevreason.Text, txtUserRevNo.Text, txtUserRevDt.Text, dcr, txtreason.Text,txtnatureofchange.Text);
            using (Database db1 = new Database("connString"))
            {
               // string sql = "select COALESCE(MAX(CAST(NULLIF(LTRIM(RTRIM(rev_no)), '') AS INT)), 0) + 1  from ControlPlan  where part_slno = @0  and obsolete='N'";
                string sql = "select COALESCE(MAX(CAST(NULLIF(LTRIM(RTRIM(cp_revno)), '') AS INT)), 0) + 1  from parts  where part_slno = @0  and obsolete='N' and  del_status='N'";
                //AND operation_slno = @oprnslno";


                int rev = db1.ExecuteScalar<int>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue));
                hdnrevnumber.Value = rev.ToString();
            }
            txtUserRevNo.Text = Convert.ToInt32(hdnrevnumber.Value).ToString();
            txtUserRevDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //int rowsAffected1 = db.Execute(";exec usp_InitiateControlPlanRevision @@cp_slno=@0,@@rev_reason=@1, @@rev_no=@2, @@rev_date=@3, @@dcr_slno= @4,@@Reason_For_Change=@5,@@nature_of_change=@6", Convert.ToInt32(hdnSlNo.Value), txtrevreason.Text, txtUserRevNo.Text, txtUserRevDt.Text, dcr, 
            //    hdnreas.Value, hdnnature.Value);


            //int rowsAffected1 = db.Execute(";exec usp_CpRevisionNew @@part_slno=@0,@@rev_reason=@1, @@rev_no=@2, @@rev_date=@3, @@dcr_slno= @4,@@Reason_For_Change=@5,@@nature_of_change=@6", Convert.ToInt32(ddlpart_slno.SelectedValue), txtrevreason.Text, Convert.ToInt32(txtUserRevNo.Text), txtUserRevDt.Text, dcr,
            //   hdnreas.Value, hdnnature.Value);
            int rowsAffected1 = db.Execute(
                ";exec usp_CpRevisionNew @part_slno, @rev_date, @rev_reason, @dcr_slno, @Reason_For_Change, @nature_of_change, @rev_no",
                new
                {
                    part_slno = Convert.ToInt32(ddlpart_slno.SelectedValue),
                    rev_date = DateTime.ParseExact(txtUserRevDt.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                    rev_reason = txtrevreason.Text,
                    dcr_slno = dcr,
                    Reason_For_Change = hdnreas.Value,
                    nature_of_change = hdnnature.Value,
                    rev_no = Convert.ToInt32(txtUserRevNo.Text)
                }
            );


            string sqlUpdateDCRStatus = "Update DCR_Status set status='closed' where dcr_slno=" + dcr;
            //+ " and operation_slno=" + opn;
            db.Execute(sqlUpdateDCRStatus);
            string sqlselect = "Select count(*) from DCR_Status where dcr_slno=" + dcr + " and status = 'pending'";
            string sqlselectcountformail = "Select count(*) from DCR_Status where dcr_slno=" + dcr + " and status = 'closed'";
             cnt = db.ExecuteScalar<int>(sqlselect);
            cntformail = db.ExecuteScalar<int>(sqlselectcountformail);
            //string rep = " LIKE '%,'" + "'" + opn + "'" + "',%'";
            //Delete the DCR for this cp
            //if (cnt <= 0)
            //{
                //string sqlDeleteDCR = "Update DCR set del_status='Y',submit_status='O' where part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue) + " and(',' + operations + ',') "+rep+"   and  change_area='CP'  and Submit_status='A' and del_status='N' and dcr_slno=" + dcr;

                //db.Execute(sqlDeleteDCR);
                // string likePattern = "'%,'+'" + opn + "'+',%'";
                string likePattern = "%," + opn + ",%";

                //string sqlDeleteDCR = "UPDATE DCR SET del_status='Y', submit_status='O' " +
                //                      "WHERE part_slno=@partSlno " +
                //                      "AND (',' + operations + ',') LIKE @likePattern " +
                //                      "AND change_area='CP' " +
                //                      "AND Submit_status='A' " +
                //                      "AND del_status='N' " +
                //                      "AND dcr_slno=@dcr";

                string sqlDeleteDCR = "UPDATE DCR SET  submit_status='R' " +
                                     "WHERE part_slno=@partSlno " +
                                     "AND (',' + operations + ',') LIKE @likePattern " +
                                     "AND change_area='CP' " +
                                     "AND Submit_status='A' " +
                                     "AND del_status='N' " +
                                     "AND dcr_slno=@dcr";



                var parameters = new
                {
                    partSlno = Convert.ToInt32(ddlpart_slno.SelectedValue),
                    likePattern, // This includes the wildcards
                    dcr = dcr
                };

                int rowsAffected = db.Execute(sqlDeleteDCR, parameters);

           // }

        }
        using (Database db = new Database("connString"))
        {
            if (cnt == 0)
            {
                string sqlCC = @"select emailid from  employees where del_status='N' and isAdmin='Y'";
                string sql = @"select e.emailid
from parts p
inner join employees e on e.employeeslno=p.approvedBy
where p.part_slno=@0";

                List<Class_Employees> lstempl = db.Fetch<Class_Employees>(sql, ddlpart_slno.SelectedValue);
                List<Class_Employees> lstemplCC = db.Fetch<Class_Employees>(sqlCC);
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
                if (lstemplCC.Count > 0)
                {
                    foreach (Class_Employees c1 in lstemplCC)
                    {
                        ccmails.Add(c1.EmailId);
                    }
                }
                ccmails.Add(userInfo[0].EmailId);
                tomails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                ccmails.RemoveAll(email => !EmailValidator.IsValidEmail(email));
                hdndcr_slno.Value = dcr.ToString();
                PrepareForMail(tomails, ccmails, "revision");

            }

        }
        Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Revision Initiated');window.location='ControlPlan2.aspx';", true);
    }


    String GetSubmitStat(string cpno)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_ControlPlan>(" where cp_slno=@0", cpno).Submitstatus;
        }
    }

    //string GetProcessNum(string ProcessNo)
    //{
    //    using (Database db = new Database("connString"))
    //    {
    //        return db.SingleOrDefault<Class_PartsMapping>(" where map_slno=@0", ProcessNo).process_no;
    //    }
    //}

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
                msg = "Revision is intiated for Control Plan. ";
                subject = "Revision Initiated for " + ddlpart_slno.SelectedItem;
                break;
            case "for_approval":
                msg = "The below Control Plan is pending for your approval. ";
                subject = "Approval Pending - " + ddlpart_slno.SelectedItem;
                break;
            case "approved":
                msg = "The below Control Plan is approved. ";
                subject = "Control Plan Approved - " + ddlpart_slno.SelectedItem;
                break;
            default:
                msg = "Unknown condition for sending mail triggered. ";
                subject = "Unknown mail condition: " + ddlpart_slno.SelectedItem;
                break;
        }
        Database db1 = new Database("connString");
       string operations= db1.ExecuteScalar<string>(@"SELECT STUFF((  SELECT ',' + o.OperationDesc  FROM operations o WHERE o.operation_slno IN(
      SELECT TRY_CAST(LTRIM(RTRIM(value)) AS INT) FROM STRING_SPLIT((SELECT operations FROM DCR WHERE dcr_slno = @0), ',') ) FOR XML PATH(''), TYPE
               ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS OperationDescriptions",Convert.ToInt32(hdndcr_slno.Value));


        string styl = "<style>table{max-width:100%;background-color:transparent;font-size:14px}th{text-align:left}.table{width:100%;margin-bottom:20px}.table>tbody>tr>td,.table>tbody>tr>th,.table>tfoot>tr>td,.table>tfoot>tr>th,.table>thead>tr>td,.table>thead>tr>th{padding:8px;line-height:1.428571429;vertical-align:top;border-top:1px solid #ddd}.table>thead>tr>th{vertical-align:bottom;border-bottom:2px solid #ddd}.table>tbody+tbody{border-top:2px solid #ddd}.table .table{background-color:#fff}.table-striped>tbody>tr:nth-child(odd)>td,.table-striped>tbody>tr:nth-child(odd)>th{background-color:#f9f9f9}body{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:14px;line-height:1.428571429;color:#333;background-color:#fff}h2{font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:500;line-height:1.1;color:inherit;text-align:center}</style>";

        string bodycnt = "<center><b><label style='background-color:#0198FF; color:White;font-family:Calibri;font-size:medium;'>&nbsp;THIS IS AN AUTOGENERATED MAIL. DO NOT REPLY TO THIS!! &nbsp;</label></b></center>" +
            "<body style='font-family:Calibri;font-size:medium;'>Hi, <br/><br/>" + msg + "Please find the details below:" +
            "<table class='table table-striped'>" +
            "<tr><th>Part No</th><td>" + ddlpart_slno.SelectedItem + "</td></tr>" +
            "<tr><th>Operation</Th><td>" + operations + "</td></tr>";

        if (mailFor == "revision")
        {
            bodycnt = bodycnt + " <tr><th>Revision No</th><td>" + hdnrevnumber.Value + "</td></tr><tr><th>Revision Date</th><td>" + hdnrevdt.Value + "</td></tr><tr><th>Revision Reason</th><td>" + hdnrevreason.Value + "</td></tr>";
        }
        else
        {
            bodycnt = bodycnt + " <tr><th>Revision No</th><td>" + hdnrevnumber.Value + "</td></tr><tr><th>Revision Date</th><td>" + hdnrevdt.Value + "</td></tr>";
        }

        string mailBody = "<html><head>" + styl + "</head><body>" + bodycnt + "</body></html>";

        List<string> newToId = tomails.Distinct().ToList();
        List<string> newCcId = ccmails.Distinct().ToList();


        // send mail
        mail ml = new mail();
        ml.SendMail(toMailId: newToId, mailSubject: subject, bodyText: mailBody, ccMailId: newCcId);
    }

}