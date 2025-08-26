using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPoco;
public partial class sop_qry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            LoadParts();
            LoadControlPlan();
        }
    }

    void LoadControlPlan() {
        
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
                cond += " and c.operation_slno = " + Convert.ToString(ddloperation_slno.SelectedValue);
            }
            else
            {
                cond += " c.operation_slno=" + Convert.ToString(ddloperation_slno.SelectedValue);
            }
        }



        string qry = @"SELECT
  DISTINCT c.*,
  opn.operationdesc,
  m.machinedesc,
  p.mstPartNo,
  case
    when c.is_Approved = 0 then 'Not Approved'
    when c.is_Approved = 1 then 'Approved'
    else 'Not Approved'
  end as ApprovedStatus
FROM
  sop_header c
  inner join operations opn on c.operation_slno = opn.operation_slno
  inner join parts p on p.part_slno = c.part_slno
  inner join machines m on m.machine_slno=c.machine_slno
where
  c.is_obsolete = 0";


        if (cond.Length > 0)
        {
            qry += " AND " + cond + " order by  p.mstPartNo,operation_slno ";
        }

        else
        { qry += " order by  p.mstPartNo ,operation_slno"; }
       
        using (Database db=new Database("connString"))
        {
            List<Class_sop_header> lst = db.Query<Class_sop_header>(qry).ToList();

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

  public  string GetOpDescription(string slno) 
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