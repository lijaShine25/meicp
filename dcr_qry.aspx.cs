using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPoco;
public partial class dcr_qry : System.Web.UI.Page
{
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (!IsPostBack)
        {
            LoadParts();
            
            LoadControlPlan();
        }
    }

    void LoadControlPlan()
    {

        string cond = string.Empty;
        if (ddlpart_slno.SelectedIndex > 0)
        {
            if (cond.Length > 0)
            {
                cond += " and c.part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
            }
            else
            {
                cond += " c.part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
            }
        }

        if (ddloperation_slno.SelectedIndex > 0)
        {
            if (cond.Length > 0)
            {
                cond += " and c.operation_slno in(" + Convert.ToString(ddloperation_slno.SelectedValue)+")";
            }
            else
            {
                cond += " c.operation_slno in(" + Convert.ToString(ddloperation_slno.SelectedValue) + ")";
            }
        }
        string qry = string.Empty;
        if (userInfo[0].isAdmin != "Y")
        {
            qry = @"SELECT DISTINCT c.*,p.mstPartNo  as mstpartno,e.employeename,
            STUFF((
        SELECT ', ' + opn.OperationDesc
        FROM operations opn
        WHERE opn.operation_slno IN (
            SELECT Value 
            FROM dbo.SplitString(c.operations, ',')
        )
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS OperationDesc,case when c.Submit_Status = 'N' then 'Not Submitted'
        when c.Submit_Status = 'Y' then 'Submitted' when c.Submit_Status = 'A' then 'Approved'  when c.Submit_Status = 'O' then 'Obsolete'  when c.Submit_Status='R' then 'Revision Initiated'
        end as Submit_Status FROM dcr c 
        inner join parts p on p.part_slno = c.part_slno inner join employees e on e.employeeslno=c.Request_By  where c.del_status ='N' or submit_status<>'O' ";
        }
        if (userInfo[0].isAdmin == "Y")
        {
            qry = @"SELECT DISTINCT c.*,p.mstPartNo  as mstpartno, e.employeename,
            STUFF((
        SELECT ', ' + opn.OperationDesc
        FROM operations opn
        WHERE opn.operation_slno IN (
            SELECT Value 
            FROM dbo.SplitString(c.operations, ',')
        )
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS OperationDesc,case when c.Submit_Status = 'N' then 'Not Submitted'
        when c.Submit_Status = 'Y' then 'Submitted' when c.Submit_Status = 'A' then 'Approved' when c.Submit_Status = 'O' then 'Obsolete'  when c.Submit_Status='R' then 'Revision Initiated'
        end as Submit_Status FROM dcr c 
        inner join parts p on p.part_slno = c.part_slno inner join employees e on e.employeeslno=c.Request_By";
        }
        if (cond.Length > 0)
        {
            qry += " AND " + cond + " order by  c.dcr_slno ";
        }
        else
        { qry += " order by  c.dcr_slno "; }
        using (Database db = new Database("connString"))
        {
            List<Class_DCR> lst = db.Query<Class_DCR>(qry).ToList();

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

    public string GetOpDescription(string slno)
    {

        using (Database db = new Database("connString"))
        {

            return db.SingleOrDefault<Class_operations>(" where operation_slno=@0", slno).OperationDesc;
        }
    }

    public string GetMcDescription(string slno)
    {

        using (Database db = new Database("connString"))
        {

            return db.SingleOrDefault<Class_machines>(" where machine_slno=@0", slno).MachineDesc;
        }
    }

    public string GetPartDescription(string slno)
    {

        using (Database db = new Database("connString"))
        {

            return db.SingleOrDefault<Class_parts>(" where part_slno=@0", slno).mstPartNo;
        }
    }
    protected void btnViewRptHistory_Click(object sender, EventArgs e)
    {
        LoadControlPlan();
    }

    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        List<Class_parts> lst = lst1.Where(x => x.Obsolete == "N").ToList();
        ddlpart_slno.Items.Clear();
        if (lst.Count > 0)
        {
            for (int cnt = 0; cnt < lst.Count; cnt++)
            {
                ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
            }

            ddlpart_slno.Items.Insert(0, "Select...");
        }

    }
   
    void LoadOperationsFromparts()
    {
        using (Database db = new Database("connString"))
        {
            var x = db.Query<Class_operations>("select * FROM operations o,partsmapping p where p.operation_slno=o.operation_slno AND p.part_slno=@0", Convert.ToInt32(ddlpart_slno.SelectedValue.ToString()));
            ddloperation_slno.Items.Clear();
            ddloperation_slno.DataSource = x;
            ddloperation_slno.DataTextField = "OperationDesc";
            ddloperation_slno.DataValueField = "operation_slno";
            ddloperation_slno.DataBind();

            ddloperation_slno.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlpart_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOperationsFromparts();

    }
}