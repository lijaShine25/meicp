using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using NPoco;
using System.Web.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using System.Web.UI.WebControls.WebParts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
//using System.Activities.Expressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Syncfusion.XlsIO;
//using Syncfusion.Compression;
//using Syncfusion.Pdf;
//using Syncfusion.ExcelToPdfConverter;
using System.Runtime.InteropServices;

public partial class OpenViewer : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        // Bypass authentication for this page
        HttpContext.Current.SkipAuthorization = true;
        // if session exists then check if user is admin
        if (Session["UserInfo"] != null)
        {
            userInfo = (List<Class_Employees>)Session["UserInfo"];
            //if (userInfo[0].isAdmin != "Y" && userInfo[0].CanApprove != "Y")
            //{
            //    Response.Redirect("~/AccessDenied.aspx");
            //}
            hdnemplslno.Value = userInfo[0].EmployeeSlNo.ToString();

            lnkxl.Visible = true;
            if (userInfo[0].isAdmin == "Y")
            { hdnIsAdmin.Value = "Y"; }
            else { hdnIsAdmin.Value = "N"; }
        }

        if (!Page.IsPostBack)
        {
            LoadParts();

            if (Request.QueryString.HasKeys())
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E";
                //GetDetails();
                //btnDelete.Enabled = true;
            }
            else
            {
                //btnDelete.Enabled = false;
            }
            lnkxl.Visible = false;
            if (userInfo[0].isAdmin != "Y")
            {
                txtRowHeight.Enabled = false;
            }
            else
            {
                txtRowHeight.Enabled = true;
            }
        }


    }
    protected void ddlfiles_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string rowheight = string.Empty;
        Database db = new Database("connString");
        if (ddloperation_slno.SelectedValue != "All")
        {
            if (userInfo[0].isAdmin == "Y")
                txtRowHeight.Enabled = true;
            else txtRowHeight.Enabled = false;
            if (ddlfiles.SelectedItem.Text == "Control Plan")
            {
                txtRowHeight.Enabled = false;
                //sql = @"SELECT  c.rowHeight FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                //              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                //              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                //rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }
            else if (ddlfiles.SelectedItem.Text == "PMC")
            {
                txtRowHeight.Enabled = false;
                //sql = @"SELECT  c.PMC_rowHeight FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                //              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                //              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                //rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }
            else if (ddlfiles.SelectedItem.Text == "FOI")
            {
                sql = @"SELECT  c.FOI_rowHeight FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }
            else if (ddlfiles.SelectedItem.Text == "Dock Audit")
            {
                sql = @"SELECT  c.DOC_rowHeight FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }
            else if (ddlfiles.SelectedItem.Text == "PCC")
            {
                //  txtRowHeight.Enabled = false;
                sql = @"SELECT  c.PCC_rowHeight FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }
            else if (ddlfiles.SelectedItem.Text == "PDI Summary")
            {
                //txtRowHeight.Enabled = false;
                sql = @"SELECT  c.PDI_rowHeight FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }

            else if (ddlfiles.SelectedItem.Text == "Control Chart")
            {
                txtRowHeight.Enabled = false;
                //sql = @"SELECT  c.CC_rowHeight FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                //              " p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND " +
                //              " c.machine_slno=p.machine_slno ) and c.part_slno=@0 and c.operation_slno=@1  ";
                //rowheight = db.ExecuteScalar<string>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue), Convert.ToInt32(ddloperation_slno.SelectedValue));
            }
            else if (ddlfiles.SelectedItem.Text == "PDI Checksheet-2 Per Skid")
            {
                txtRowHeight.Enabled = false;
            }
            else if (ddlfiles.SelectedItem.Text == "PDI Checksheet-Every Hour")
            {
                txtRowHeight.Enabled = false;
            }
            else if (ddlfiles.SelectedItem.Text == "PCC Bridge")
            {
                txtRowHeight.Enabled = false;
            }
            else if (ddlfiles.SelectedItem.Text == "PCC IIR")
            {
                txtRowHeight.Enabled = false;
            }
        }
        else { txtRowHeight.Enabled = false; }

        if (!string.IsNullOrEmpty(rowheight))
        {
            if (Convert.ToInt32(rowheight) > 0)
                txtRowHeight.Text = rowheight;
        }
        else txtRowHeight.Text = "75";
    }
    void LoadParts()
    {
        using (Database db = new Database("connString"))
        {
            var x = db.Query<Class_ControlPlan>("select distinct mstpartno, part_slno from parts where del_status='N' and obsolete='N'");
            //var x = db.Query<Class_ControlPlan>(" select distinct c.part_slno,p.mstPartNo from [ControlPlan] c inner join parts p on p.part_slno=c.part_slno ");
            ddlpart_slno.Items.Clear();
            ddlpart_slno.DataSource = x;
            ddlpart_slno.DataTextField = "mstPartNo";
            ddlpart_slno.DataValueField = "part_slno";
            ddlpart_slno.DataBind();

            ddlpart_slno.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }
    void LoadOperationsFromCP()
    {
        using (Database db = new Database("connString"))
        {
            string sql = @"select distinct o.operation_slno,o.operationdesc
from controlplan c
inner join operations o on o.operation_slno=c.operation_slno
inner join parts p on p.part_slno=c.part_slno
where c.part_slno=@0";

            var x = db.Query<Class_operations>(sql, Convert.ToInt32(ddlpart_slno.SelectedValue));
            ddloperation_slno.Items.Clear();
            ddloperation_slno.DataSource = x;
            ddloperation_slno.DataTextField = "OperationDesc";
            ddloperation_slno.DataValueField = "operation_slno";
            ddloperation_slno.DataBind();
            if (ddloperation_slno.Items.Count > 0)
                ddloperation_slno.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "All"));
            ddloperation_slno.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

        }
    }
    void LoadChars()
    {
        using (Database db = new Database("connString"))
        {
            string @sql = "";
            if (ddloperation_slno.SelectedValue == "All")
            {
                txtRowHeight.Enabled = false;
                @sql = @"select distinct product_char 
from ControlPlan_Child cpc
inner join ControlPlan cp on cp.cp_slno = cpc.cp_slno
where cp.part_slno = " + ddlpart_slno.SelectedValue + " and tol_min is not null and tol_max is not null";
            }
            else if (userInfo[0].isAdmin == "Y")
            {
                txtRowHeight.Enabled = true;
                @sql = @"select distinct product_char 
from ControlPlan_Child cpc
inner join ControlPlan cp on cp.cp_slno = cpc.cp_slno
where cp.part_slno = " + ddlpart_slno.SelectedValue + " and cp.operation_slno = " + ddloperation_slno.SelectedValue
    + " and max_spec is not null and min_spec is not null";
            }

            List<Class_ControlPlan_Child> lst = db.Query<Class_ControlPlan_Child>(sql).ToList();
            if (lst.Count > 0)
            {
                ddlchar.Items.Clear();
                ddlchar.DataSource = lst;
                ddlchar.DataTextField = "product_char";
                ddlchar.DataValueField = "product_char";
                ddlchar.DataBind();
                ddlchar.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }


    }

    void LoadOtherDocuments()
    {
        ddlpartdoc.Items.Clear();
        ddlpartdoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        // ddlpartdoc.SelectedIndex = 1;

        string sql = @"select * from parts_documents where part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
        using (Database db = new Database("connString"))
        {
            List<Class_parts_documents> lst = db.Fetch<Class_parts_documents>(sql);
            if (lst.Count > 0)
            {
                ddlpartdoc.DataSource = lst;
                ddlpartdoc.DataTextField = "doc_title";
                ddlpartdoc.DataValueField = "doc_filename";
                ddlpartdoc.DataBind();
                ddlpartdoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }

    }
    void LoadPartDocuments()
    {
        ddlfiles.Items.Clear();
        int slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
        //        string sql = @"select uploadfile1 as part_doc from parts where part_slno=" + slno + " and uploadfile1 is not null "
        //+ " union "
        //+ " select uploadfile2 as part_doc from parts where part_slno=" + slno + " and uploadfile2 is not null "
        //+ " union "
        //+ " select uploadfile3 as part_doc from parts where part_slno=" + slno + " and uploadfile3 is not null "
        //+ " union "
        //+ " select uploadfile4 as part_doc from parts where part_slno=" + slno + " and uploadfile4 is not null";

        string sql = @"select * from parts_documents where part_slno=" + slno;
        ddlfiles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));
        ddlfiles.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Control Plan", "2"));
        ddlfiles.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dock Audit", "3"));
        ddlfiles.Items.Insert(3, new System.Web.UI.WebControls.ListItem("FOI", "4"));
        //ddlfiles.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Layout Inspection", "5"));
        //ddlfiles.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Packing cum Layer Audit", "6"));
        ddlfiles.Items.Insert(4, new System.Web.UI.WebControls.ListItem("PCC", "5"));
        ddlfiles.Items.Insert(5, new System.Web.UI.WebControls.ListItem("PDI Summary", "6"));
        ddlfiles.Items.Insert(6, new System.Web.UI.WebControls.ListItem("PMC", "7"));
        ddlfiles.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Control Chart", "8"));
        //ddlfiles.Items.Insert(8, new System.Web.UI.WebControls.ListItem("SOP", "9"));
        ddlfiles.Items.Insert(8, new System.Web.UI.WebControls.ListItem("PDI Checksheet-2 Per Skid", "9"));
        ddlfiles.Items.Insert(9, new System.Web.UI.WebControls.ListItem("PDI Checksheet-Every Hour", "10"));
        ddlfiles.Items.Insert(10, new System.Web.UI.WebControls.ListItem("PCC Bridge", "11"));
        ddlfiles.Items.Insert(11, new System.Web.UI.WebControls.ListItem("PCC IIR", "12"));
        // System.Web.UI.WebControls.ListItem separator = new System.Web.UI.WebControls.ListItem("-------------------------", "");

        //using (Database db = new Database("connString"))
        //{
        //    List<Class_parts_documents> lst = db.Fetch<Class_parts_documents>(sql);
        //    if(lst.Count> 0)
        //    {
        //        ddlfiles.Items.Clear();



        //        ddlfiles.DataSource= lst;
        //        ddlfiles.DataTextField = "doc_title";
        //        ddlfiles.DataValueField = "doc_filename";
        //        ddlfiles.DataBind();

        //        ddlfiles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));
        //        ddlfiles.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Control Plan", "2"));
        //        ddlfiles.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dock Audit", "3"));
        //        ddlfiles.Items.Insert(3, new System.Web.UI.WebControls.ListItem("FOI", "4"));
        //        //ddlfiles.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Layout Inspection", "5"));
        //        //ddlfiles.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Packing cum Layer Audit", "6"));
        //        ddlfiles.Items.Insert(4, new System.Web.UI.WebControls.ListItem("PCC", "5"));
        //        ddlfiles.Items.Insert(5, new System.Web.UI.WebControls.ListItem("PDI", "6"));
        //        ddlfiles.Items.Insert(6, new System.Web.UI.WebControls.ListItem("PMC", "7"));
        //        ddlfiles.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Control Chart", "8"));
        //        ddlfiles.Items.Insert(8, new System.Web.UI.WebControls.ListItem("SOP", "9"));

        //        separator.Attributes.Add("disabled", "true");
        //        ddlfiles.Items.Insert(9, separator);

        //    }
        //    ddlfiles.SelectedIndex = 0;
        //}

    }
    protected void ddlpart_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddloperation_slno.Enabled = true;
        ddlfiles.Enabled = true;
        txtRowHeight.Text = "75";
        LoadOperationsFromCP();
        LoadPartDocuments();
        LoadOtherDocuments();
    }
    protected void btnCp_OnClick(object sender, EventArgs e)
    {
        //GenerateCP();
    }

    void LoadDOCTYPES()
    {
        string sqlcp = string.Empty;
        string sqlcp1 = string.Empty;
        List<Class_Temp_RptControlPlan> lst = null;
        int cp_slno = 0;

        string sqlDOC = string.Empty;
        // string sqlCC = string.Empty;
        string sqlPMC = string.Empty;
        string sqlPCC = string.Empty;
        string sqlFOI = string.Empty;
        string sqlPDISUMMARY = string.Empty;
        string sqlPDI2perskid = string.Empty;
        string sqlPDIEveryHour = string.Empty;

        string sqlPCCBridge = string.Empty;
        string sqlPCCIIR = string.Empty;

        using (Database db = new Database("connString"))
        {

            if (ddloperation_slno.SelectedItem.Text == "All")
            {
                sqlcp = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                       " p.Submitstatus='A'  AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.part_slno=" + ddlpart_slno.SelectedValue;
                cp_slno = db.ExecuteScalar<int>(sqlcp);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", null, ddlpart_slno.SelectedValue, null);
                sqlcp1 = @"Select * from temp_rptControlPlan order by cast(process_no as decimal)";

                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp1);

                if (lst.Count > 0)
                {
                    ddlfiles.Items.Clear();
                    ddlfiles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));
                    ddlfiles.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Control Plan", "2"));
                }

            }
            else
            {//p.Submitstatus='A' AND 
                ddlfiles.Items.Clear();
                ddlfiles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));

                sqlcp = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                       " c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.part_slno=" + ddlpart_slno.SelectedValue + " and c.operation_slno= " + ddloperation_slno.SelectedValue + "";
                cp_slno = db.ExecuteScalar<int>(sqlcp);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, ddlpart_slno.SelectedValue, ddloperation_slno.SelectedValue);
                sqlcp1 = @"Select * from temp_rptControlPlan order by cast(process_no as decimal)";

                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp1);

                if (lst.Count > 0)
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("Control Plan", (ddlfiles.Items.Count + 1).ToString()));
                }


                sqlPMC = @"Select * from temp_rptControlPlan  where methodDesc='PMC' " +
                        " and (product_char <>'-' or product_char  <>'')";

                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPMC);
                if (lst.Count > 0)
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PMC", (ddlfiles.Items.Count + 1).ToString()));
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("Control Chart", (ddlfiles.Items.Count + 1).ToString()));
                }



                sqlFOI = @"Select * from temp_rptControlPlan where methodDesc='FOI'";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlFOI);
                if (lst.Count > 0)
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("FOI", (ddlfiles.Items.Count + 1).ToString()));
                }

                sqlPCC = @"Select * from temp_rptControlPlan where methodDesc2='PCC'";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPCC);
                if (lst.Count > 0)
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PCC", (ddlfiles.Items.Count + 1).ToString()));
                }


                sqlPDISUMMARY = @"Select * from temp_rptControlPlan where sampleFreq='Per Skid' or sampleFreq='100%' order by cast(process_no as decimal) ";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPDISUMMARY);
                if (lst.Count > 0 && (ddloperation_slno.SelectedItem.Text.Contains("Final Inspection") || (ddloperation_slno.SelectedItem.Text.Contains("Stage Inspection"))))
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PDI Summary", (ddlfiles.Items.Count + 1).ToString()));
                }



                sqlPDI2perskid = @"Select * from temp_rptControlPlan  where sampleFreq='Per Skid' order by cast(process_no as decimal) ";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPDI2perskid);
                if (lst.Count > 0 && ddloperation_slno.SelectedItem.Text.Contains("Final Inspection"))
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PDI Checksheet-2 Per Skid", (ddlfiles.Items.Count + 1).ToString()));
                }




                sqlPDIEveryHour = @"Select * from temp_rptControlPlan  where sampleFreq='Every Hour' order by cast(process_no as decimal) ";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPDIEveryHour);
                if (lst.Count > 0 && ddloperation_slno.SelectedItem.Text.Contains("Final Inspection"))
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PDI Checksheet-Every Hour", (ddlfiles.Items.Count + 1).ToString()));
                }

                sqlDOC = @"Select * from temp_rptControlPlan";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlDOC);
                if (lst.Count > 0 && ddloperation_slno.SelectedItem.Text == "Dock Audit")
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("Dock Audit", (ddlfiles.Items.Count + 1).ToString()));
                }



                sqlPCCBridge = @"Select * from temp_rptControlPlan where methodDesc='PCC(ADB)'";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPCCBridge);
                if (lst.Count > 0)
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PCC Bridge", (ddlfiles.Items.Count + 1).ToString()));
                }




                sqlPCCIIR = @"Select * from temp_rptControlPlan where methodDesc='PCC(IIR)'";
                lst = db.Fetch<Class_Temp_RptControlPlan>(sqlPCCIIR);
                if (lst.Count > 0)
                {
                    ddlfiles.Items.Insert(ddlfiles.Items.Count, new System.Web.UI.WebControls.ListItem("PCC IIR", (ddlfiles.Items.Count + 1).ToString()));
                }






            }


        }
    }

    protected void ddlpartdoc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtRowHeight.Enabled = false;
        ddloperation_slno.SelectedIndex = 0;
        ddlfiles.SelectedIndex = 0;
        ddloperation_slno.Enabled = false;
        ddlfiles.Enabled = false;
        lnkxl.Disabled = true;

        hdncheck.Value = "otherDocs";
        btnopen.Enabled = true;
        lnkpdf.Disabled = false;
        lnkxl.Disabled = true;
        if (ddlpartdoc.SelectedIndex == 0)
        {
            ddloperation_slno.Enabled = true;
            ddlfiles.Enabled = true;
            lnkxl.Disabled = false;
        }
    }
    protected void ddloprn_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        LoadChars();
        LoadDOCTYPES();
        //LoadDocTypes();
        if (ddlpartdoc.SelectedIndex > 0)
            hdncheck.Value = "otherDocs";
        else hdncheck.Value = "docs";
        CheckIfCPApproved();
    }

    void CheckIfCPApproved()
    {
        string sql = string.Empty;
        List<Class_ControlPlan> lst = null;
        int flag = 0;
        if (ddloperation_slno.SelectedValue != "All")
        {
            sql = @"SELECT  * FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE  p.Submitstatus='A' and p.obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.obsolete='N'  and c.part_slno=" + ddlpart_slno.SelectedValue + " and c.operation_slno= " + ddloperation_slno.SelectedValue;
        }
        else { sql = @"SELECT  * FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT)) from controlplan p WHERE  p.Submitstatus='A' and p.obsolete='N' AND c.part_slno=p.part_slno and c.machine_slno=p.machine_slno )  and c.obsolete='N'  and c.part_slno=" + ddlpart_slno.SelectedValue; }
        using (Database db = new Database("connString"))
        {
            lst = db.Query<Class_ControlPlan>(sql).ToList();
        }
        if (hdncheck.Value == "docs")
        {
           if (userInfo[0].isAdmin != "Y")
           {
                foreach (Class_ControlPlan c in lst)
                {
                    if (c.is_approved == false)
                    {
                        flag = 1;
                    }
                }
            }

            if (flag == 1 )
            {


                lnkpdf.Disabled = true;
                lnkxl.Disabled = true;
                btnopen.Enabled = false;
            }
            else if(flag == 1 )

            {
                lnkpdf.Disabled = false;
                lnkxl.Disabled = false;
                btnopen.Enabled = true;

            }
        }
        else
        {
            lnkpdf.Disabled = false;
            lnkxl.Disabled = true;
            btnopen.Enabled = true;
        }
    }
    void LoadDocTypes()
    {
        using (Database dbfiletypes = new Database("connString"))
        {
            string @sql = "";
            if (ddloperation_slno.SelectedItem.Text != "All")
                @sql = @"select *  from operations where operation_slno = " + ddloperation_slno.SelectedValue;
            List<Class_operations> lst = dbfiletypes.Query<Class_operations>(sql).ToList();

            if (ddloperation_slno.SelectedValue == "All")
            {
                ddlfiles.Items.Clear();
                ddlfiles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));
                ddlfiles.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Control Plan", "2"));
                ddlfiles.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dock Audit", "3"));
                ddlfiles.Items.Insert(3, new System.Web.UI.WebControls.ListItem("FOI", "4"));
                //ddlfiles.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Layout Inspection", "5"));
                //ddlfiles.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Packing cum Layer Audit", "6"));
                ddlfiles.Items.Insert(4, new System.Web.UI.WebControls.ListItem("PCC", "5"));
                ddlfiles.Items.Insert(5, new System.Web.UI.WebControls.ListItem("PDI Summary", "6"));
                ddlfiles.Items.Insert(6, new System.Web.UI.WebControls.ListItem("PMC", "7"));
                ddlfiles.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Control Chart", "8"));
               // ddlfiles.Items.Insert(8, new System.Web.UI.WebControls.ListItem("SOP", "9"));
                ddlfiles.Items.Insert(8, new System.Web.UI.WebControls.ListItem("PDI Checksheet-2 Per Skid", "9"));
                ddlfiles.Items.Insert(9, new System.Web.UI.WebControls.ListItem("PDI Checksheet-Every Hour", "10"));

                ddlfiles.Items.Insert(10, new System.Web.UI.WebControls.ListItem("PCC Bridge", "11"));
                ddlfiles.Items.Insert(11, new System.Web.UI.WebControls.ListItem("PCC IIR", "12"));
            }

            else
            {
                if (lst.Count > 0)
                {
                    int i = 2;
                    ddlfiles.Items.Clear();
                    ddlfiles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));
                    ddlfiles.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Control Plan", "2"));
                    foreach (Class_operations c in lst)
                    {
                        if (ddloperation_slno.SelectedValue == c.operation_slno.ToString())
                        {
                            if (c.DOCK == true)
                            {
                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Dock Audit", (i + 1).ToString()));
                                i += 1;
                            }
                            if (c.PCC == true)
                            {

                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("PCC", (i + 1).ToString()));
                                i += 1;

                            }
                            if (c.PDI == true)
                            {
                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("PDI Summary", (i + 1).ToString()));
                                i += 1;
                            }
                            if (c.FOI == true)
                            {
                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("FOI", (i + 1).ToString()));
                                i += 1;
                            }
                            if (c.PMC == true)
                            {
                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("PMC", (i + 1).ToString()));
                                i += 1;

                            }
                            if (c.SOP == true)
                            {

                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("SOP", (i + 1).ToString()));
                                i += 1;
                            }

                            if (c.PDI_2PerSkid == true)
                            {

                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("PDI Checksheet-2 Per Skid", (i + 1).ToString()));
                                i += 1;
                            }

                            if (c.PDI_EveryHour == true)
                            {

                                ddlfiles.Items.Insert(i, new System.Web.UI.WebControls.ListItem("PDI Checksheet-Every Hour", (i + 1).ToString()));
                                i += 1;
                            }

                        }
                    }
                }
            }
        }
    }

    protected void ddlRevNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //
    }

    //void ConvertXlstoPdf(string docfile, string pdffile)
    //{
    //    // reference: https://help.syncfusion.com/file-formats/xlsio/excel-to-pdf-conversion

    //    using (ExcelEngine excelEngine = new ExcelEngine())
    //    {
    //        IApplication application = excelEngine.Excel;
    //        application.DefaultVersion = ExcelVersion.Xlsx;

    //        string docpath = Server.MapPath("~/pdftemp/" + docfile);

    //        IWorkbook workbook = application.Workbooks.Open(docpath, ExcelOpenType.Automatic);
    //        IWorksheet sheet = workbook.Worksheets[0];

    //        //convert the sheet to PDF
    //        ExcelToPdfConverter converter = new ExcelToPdfConverter(sheet);

    //        Syncfusion.Pdf.PdfDocument pdfDocument = new Syncfusion.Pdf.PdfDocument();
    //        pdfDocument = converter.Convert();
    //        string pdfpath = Server.MapPath("~/pdftemp/" + pdffile);
    //        pdfDocument.Save(pdfpath);


    //    }
    //}
}
