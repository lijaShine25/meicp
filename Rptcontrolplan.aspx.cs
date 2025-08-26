using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using NPoco;


public partial class Rptcontrolplan : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanApprove != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        hdnemplslno.Value = userInfo[0].EmployeeSlNo.ToString();

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
        }

    }
    void LoadParts()
    {
        using (Database db = new Database("connString"))
        {
            var x = db.Query<Class_ControlPlan>("select distinct mstpartno, part_slno from parts where del_status='N' and obsolete='N'");
            ddlpart_slno.Items.Clear();
            ddlpart_slno.DataSource = x;
            ddlpart_slno.DataTextField = "mstPartNo";
            ddlpart_slno.DataValueField = "part_slno";
            ddlpart_slno.DataBind();

            ddlpart_slno.Items.Insert(0, new ListItem("Select", "0"));
        }

        //Crud_parts crud = new Crud_parts();
        ////List<Class_parts> lst = crud.usp_partsSelect().ToList();

        //List<Class_parts> lst = crud.usp_partsSelect().ToList().Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList();

        //ddlpart_slno.Items.Clear();

        //for (int cnt = 0; cnt < lst.Count; cnt++)
        //{
        //    ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
        //}

        //ddlpart_slno.Items.Insert(0, "Select...");
    }

    void LoadOperationsFromCP()
    {
        using (Database db = new Database("connString"))
        {
            var x = db.Query<Class_operations>("select op.operation_slno, op.OperationDesc from operations op inner join PartsMapping pm on pm.operation_slno=op.operation_slno where pm.part_slno=@0", Convert.ToInt32(ddlpart_slno.SelectedValue));
            ddloperation_slno.Items.Clear();
            ddloperation_slno.DataSource = x;
            ddloperation_slno.DataTextField = "OperationDesc";
            ddloperation_slno.DataValueField = "operation_slno";
            ddloperation_slno.DataBind();

            ddloperation_slno.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    void LoadRevisions()
    {
        if (ddlpart_slno.SelectedIndex > 0 && ddloperation_slno.SelectedIndex > 0)
        {
            using (Database db = new Database("connString"))
            {
                
                string sql = "SELECT * FROM ControlPlan cp\n"
           + "INNER JOIN parts p  ON p.part_slno = cp.part_slno\n"
           + "WHERE p.mstPartNo='" + ddlpart_slno.SelectedItem.Text + "' AND cp.operation_slno=" + ddloperation_slno.SelectedValue ;

                var x = db.Query<Class_ControlPlan>(sql);
                ddlRevNo.Items.Clear();
                ddlRevNo.DataSource = x;
                ddlRevNo.DataTextField = "user_revno";
                ddlRevNo.DataValueField = "rev_no";
                ddlRevNo.DataBind();
                ddlRevNo.Items.Insert(0, new ListItem("Latest", "0"));
            }
        }
    }
    protected void ddlpart_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOperationsFromCP();
    }

    protected void ddlRevNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRevisions();
    }

    void reportHTML()
    {
        int parts_slno = 0;
        int oper_slno = 0;

        if (ddlpart_slno.SelectedIndex > 0)
        {
            parts_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
        }
        if (ddloperation_slno.SelectedIndex > 0)
        {
            oper_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);
        }

        int cp_slno;
        string cp_type;

        string qry = "SELECT  c.* FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                      "c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.part_slno=" + parts_slno + " and c.operation_slno= " + oper_slno + "";

        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan> lst = db.Query<Class_ControlPlan>(qry).ToList();
            cp_slno = lst[0].cp_slno;
            cp_type = lst[0].cpType;

            db.Execute(";exec SP_Temp_RptControlPlan @0, @1, @2", cp_slno, parts_slno, oper_slno);

            Response.Redirect("~/RPTControlPlan_XLS.aspx?mode=H");
        }


    }

    void report()
    {

        int parts_slno = 0;
        int oper_slno = 0;

        if (ddlpart_slno.SelectedIndex > 0)
        {
            parts_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
        }
        if (ddloperation_slno.SelectedIndex > 0)
        {
            oper_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);
        }

        ReportViewer1.Visible = true;
        int cp_slno;
        string cp_type;

        string qry = "SELECT  c.* FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                      "c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.part_slno=" + parts_slno + " and c.operation_slno= " + oper_slno + "";

        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan> lst = db.Query<Class_ControlPlan>(qry).ToList();
            if (lst.Count == 0)
            {
                ShowMessage("No Data Found !!", MessageType.Info);
                return;
            }
            cp_slno = lst[0].cp_slno;
            cp_type = lst[0].cpType;


        }

        ReportViewer1.LocalReport.ReportPath = null;
        ReportViewer1.LocalReport.ReportPath = "RptControlPlan.rdlc";

        ReportParameter[] rptParams = new ReportParameter[4];

        ReportViewer1.LocalReport.EnableExternalImages = true;

        string imagePath = new Uri(Server.MapPath("~/css/images/CheckBox-Checked.png")).AbsoluteUri;
        string imagePath1 = new Uri(Server.MapPath("~/css/images/CheckBox-UnChecked.png")).AbsoluteUri;

        if (cp_type == "PROTOTYPE")
        {


            rptParams[0] = new ReportParameter("ImagePath1", imagePath);
            rptParams[1] = new ReportParameter("ImagePath2", imagePath1);
            rptParams[2] = new ReportParameter("ImagePath3", imagePath1);


        }
        else if (cp_type == "PRE-LAUNCH")
        {

            rptParams[0] = new ReportParameter("ImagePath1", imagePath1);
            rptParams[1] = new ReportParameter("ImagePath2", imagePath);
            rptParams[2] = new ReportParameter("ImagePath3", imagePath1);


        }
        else if (cp_type == "PRODUCTION")
        {
            rptParams[0] = new ReportParameter("ImagePath1", imagePath1);
            rptParams[1] = new ReportParameter("ImagePath2", imagePath1);
            rptParams[2] = new ReportParameter("ImagePath3", imagePath);


        }

        rptParams[3] = new ReportParameter("CURRDATE", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));

        ReportViewer1.LocalReport.SetParameters(rptParams);

        ReportViewer1.LocalReport.Refresh();
        ReportViewer1.Visible = true;



        try
        {
            string strcon = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlConnection DbConnection = new SqlConnection(strcon);

            //List lst = db.Fetch (";exec SP_Temp_RptControlPlan @@cp_slno=@0, @@part_slno=@1, @@oper_slno=@2", 19,4,3).ToList();
            //db.Execute(
            DbConnection.Open();
            SqlCommand cmd = new SqlCommand("SP_Temp_RptControlPlan", DbConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cp_slno", cp_slno);
            cmd.Parameters.AddWithValue("@part_slno", parts_slno);
            cmd.Parameters.AddWithValue("@oper_slno", oper_slno);
            cmd.ExecuteNonQuery();
            DbConnection.Close();


        }
        catch
        { }
    }

    void report_line()
    {
        //ReportViewer1.Visible = true;
        int cp_slno;

        int parts_slno = 0;
        int oper_slno = 0;

        if (ddlpart_slno.SelectedIndex > 0)
        {
            parts_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
        }
        if (ddloperation_slno.SelectedIndex > 0)
        {
            oper_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);
        }

        ReportViewer1.Visible = true;

        string qry = "SELECT  c.* FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                      "c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.part_slno=" + parts_slno + " and c.operation_slno= " + oper_slno + "";


        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan> lst = db.Query<Class_ControlPlan>(qry).ToList();
            cp_slno = lst[0].cp_slno;

        }
        //int cp_slno = Convert.ToInt32(ddlcpType.SelectedValue);

        ReportViewer1.LocalReport.ReportPath = null;
        ReportViewer1.LocalReport.ReportPath = "RptLineInspection.rdlc";
        ReportParameter[] rptParams = new ReportParameter[1];
        rptParams[0] = new ReportParameter("CURRDATE", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        ReportViewer1.LocalReport.SetParameters(rptParams);
        ReportViewer1.LocalReport.Refresh();
        ReportViewer1.Visible = true;
        try
        {
            string strcon = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlConnection DbConnection = new SqlConnection(strcon);

            //List lst = db.Fetch (";exec SP_Temp_RptControlPlan @@cp_slno=@0, @@part_slno=@1, @@oper_slno=@2", 19,4,3).ToList();
            //db.Execute(
            DbConnection.Open();
            SqlCommand cmd = new SqlCommand("SP_Temp_RptControlPlan", DbConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cp_slno", cp_slno);
            cmd.Parameters.AddWithValue("@part_slno", parts_slno);
            cmd.Parameters.AddWithValue("@oper_slno", oper_slno);
            cmd.ExecuteNonQuery();
            DbConnection.Close();

        }
        catch
        { }
    }

    protected void btnExportXL_Click(object sender, EventArgs e)
    {

        int cp_slno;
        int parts_slno = 0;
        int oper_slno = 0;
        int rev_no = 0;

        if (ddlpart_slno.SelectedIndex > 0)
        {
            parts_slno = Convert.ToInt32(ddlpart_slno.SelectedValue);
        }
        if (ddloperation_slno.SelectedIndex > 0)
        {
            oper_slno = Convert.ToInt32(ddloperation_slno.SelectedValue);
        }

        if (ddlRevNo.SelectedIndex > 0)
        {
            rev_no = Convert.ToInt32(ddlRevNo.SelectedValue);
        }

        string qry = "SELECT  c.* FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " +
                        "c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno and c.process_no=p.process_no ) and c.part_slno=" + parts_slno + " and c.operation_slno= " + oper_slno + "";


        if (ddlRevNo.SelectedIndex > 0)
        {
            qry = "SELECT  c.* FROM controlplan c where c.rev_no =" + rev_no + " and c.part_slno=" + parts_slno + " and c.operation_slno= " + oper_slno + "";
        }

        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan> lst = db.Query<Class_ControlPlan>(qry).ToList();
            if (lst.Count == 0)
            {
                ShowMessage("No Data Found !!", MessageType.Info);
                return;
            }
            cp_slno = lst[0].cp_slno;

        }


        string strcon = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        SqlConnection DbConnection = new SqlConnection(strcon);

        //List lst = db.Fetch (";exec SP_Temp_RptControlPlan @@cp_slno=@0, @@part_slno=@1, @@oper_slno=@2", 19,4,3).ToList();
        //db.Execute(
        DbConnection.Open();
        SqlCommand cmd = new SqlCommand("SP_Temp_RptControlPlan", DbConnection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@cp_slno", cp_slno);
        cmd.Parameters.AddWithValue("@part_slno", parts_slno);
        cmd.Parameters.AddWithValue("@oper_slno", oper_slno);
        cmd.ExecuteNonQuery();
        DbConnection.Close();

        string cf = "N";

        if (ddloperation_slno.SelectedValue == "1" || ddloperation_slno.SelectedValue == "9")
        {
            cf = "Y";
        }

        Response.Redirect("~/RPTControlPlan_XLS.aspx?changeFile=" + cf);

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
}

