using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPoco;
using Org.BouncyCastle.Tls.Crypto;
public partial class sop_qry_new : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadParts();
            LoadGroups();
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
                cond += " and c.operation_slno = " + Convert.ToString(ddloperation_slno.SelectedValue);
            }
            else
            {
                cond += " c.operation_slno=" + Convert.ToString(ddloperation_slno.SelectedValue);
            }
        }

        if (ddlGroup.SelectedIndex > 0)
        {
            if (cond.Length > 0)
            {
                cond += " and c.group_id = " + Convert.ToString(ddlGroup.SelectedValue);
            }
            else
            {
                cond += " c.group_id=" + Convert.ToString(ddlGroup.SelectedValue);
            }
        }

        string qry = @"SELECT DISTINCT 
    c.*, 
    opn.operationdesc, 
    c.Template, 
    m.machinedesc, 
    p.mstPartNo, 
    map.Group_Name, 
    map.Group_Id, 
    map.rev_no,map.PreparedBy, map.ApprovedBy,map.Active,
    CASE
        WHEN c.is_Approved = 0 THEN 'Not Approved'
        WHEN c.is_Approved = 1 THEN 'Approved'
        ELSE 'Not Approved'
    END AS ApprovedStatus
FROM sop_header_new c
INNER JOIN operations opn ON c.operation_slno = opn.operation_slno
INNER JOIN parts p ON p.part_slno = c.part_slno
INNER JOIN machines m ON m.machine_slno = c.machine_slno
INNER JOIN sop_mapping map ON map.Group_id = c.Group_id AND map.Map_slno = c.Map_slno
INNER JOIN (
    SELECT Group_Id, MAX(rev_no) AS max_rev
    FROM sop_mapping
    WHERE del_status = 'N' AND Obsolete = 'N'
    GROUP BY Group_Id
) latestRev ON latestRev.Group_Id = map.Group_Id AND latestRev.max_rev = map.rev_no
WHERE 
    c.is_obsolete = 0 AND 
    map.del_status = 'N'  and  map.obsolete='N'   ";





        if (cond.Length > 0)
        {
            qry += " AND " + cond + " order by  map.Group_Id ";
        }

        else
        { qry += " order by  map.Group_Id"; }

        using (Database db = new Database("connString"))
        {
            List<Class_sop_header_new> lst = db.Query<Class_sop_header_new>(qry).ToList();
            lst = lst.GroupBy(x => new { x.Group_Id, x.part_slno, x.operation_slno })
                 .Select(grp => grp.OrderByDescending(x => x.rev_no).FirstOrDefault())
                 .ToList();

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
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ddlGroup.SelectedIndex = 0;
        ddlpart_slno.SelectedIndex = 0;
        ddloperation_slno.SelectedIndex = 0;
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
    void LoadGroups()
    {
        string qry = "select distinct hdr.Group_Id,m.Group_Name from sop_header_new hdr inner join  SOP_Mapping m on m.group_id=hdr.group_id where m.del_status='N'";
        using (Database db = new Database("connString"))
        {
            List<Class_sop_mapping> lst = db.Query<Class_sop_mapping>(qry).ToList();
            ddlGroup.Items.Clear();
            if (lst.Count > 0)
            {
                for (int cnt = 0; cnt < lst.Count; cnt++)
                {
                    ddlGroup.Items.Add(new ListItem(lst[cnt].Group_Name, Convert.ToString(lst[cnt].Group_Id)));
                }

                ddlGroup.Items.Insert(0, "Select...");
            }
        }

    }
    void LoadOperationsFromparts()
    {
        if (ddlpart_slno.SelectedIndex > 0)
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
    }
    protected void ddlpart_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOperationsFromparts();

    }
}