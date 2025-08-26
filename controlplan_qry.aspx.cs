using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPoco;
public partial class controlplan_qry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            LoadParts();
            LoadControlPlan();
        }
    }
    void LoadControlPlan()
    {
        using (Database db = new Database("connString"))
        {
            string qry = "SELECT * FROM controlplan WHERE obsolete = 'N'";
            List<Class_ControlPlan> lst = db.Query<Class_ControlPlan>(qry).ToList();


            var operations = db.Query<Class_operations>("SELECT * FROM operations").ToList();
            var parts = db.Query<Class_parts>("SELECT * FROM parts").ToList();
            var machines = db.Query<Class_machines>("SELECT * FROM machines").ToList();
            // Apply dropdown filters (from ddlpart_slno and ddloperation_slno)
            if (ddlpart_slno.SelectedIndex > 0)
            {
                int partSlno = Convert.ToInt32(ddlpart_slno.SelectedValue);
                lst = lst.Where(x => x.part_slno == partSlno).ToList();
            }

            if (ddloperation_slno.SelectedIndex > 0)
            {
                int opSlno = Convert.ToInt32(ddloperation_slno.SelectedValue);
                lst = lst.Where(x => x.operation_slno == opSlno).ToList();
            }

            // Keep only latest rev_no per part_slno + operation_slno + machine_slno
            var filteredList = lst
                .GroupBy(x => new { x.part_slno, x.operation_slno, x.machine_slno })
                .Select(g => g.OrderByDescending(x => x.rev_no).First())
                .OrderBy(x => x.part_slno)
                .ThenBy(x => Convert.ToDecimal(x.process_no))
                .ToList();
            var result = filteredList
            .Select(x => new
            {
                x.cp_slno,
                x.part_slno,
                x.operation_slno,
                x.machine_slno,
                x.rev_no,
                x.process_no,
                operationdesc = operations.FirstOrDefault(o => o.operation_slno == x.operation_slno).OperationDesc,
                mstPartNo = parts.FirstOrDefault(p => p.part_slno == x.part_slno).mstPartNo,
                machinedesc = machines.FirstOrDefault(m => m.machine_slno == x.machine_slno).MachineDesc,
                ApproveStatus = x.Submitstatus == "A" ? "Approved" :
                    x.Submitstatus == "Y" ? "Submitted" :
                    x.Submitstatus == "N" ? "Not Approved" : "Not Approved"
            })
            .OrderBy(x => x.part_slno)
            .ThenBy(x => Convert.ToDecimal(x.process_no))
            .ToList();
            // Bind to grid
            if (result.Any())
            {
                grdData.DataSource = result;
                grdData.DataBind();
            }
            else
            {
                grdData.DataSource = null;
                grdData.DataBind();
            }
        }
    }


    void LoadControlPlanold() {
        
//        string cond = string.Empty;
//        if (ddlpart_slno.SelectedIndex > 0)
//        {
//            if (cond.Length > 0)
//            {
//                cond += " and c.part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
//            }
//            else
//            {
//                cond += " c.part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
//            }
//        }

//        if (ddloperation_slno.SelectedIndex > 0)
//        {
//            if (cond.Length > 0)
//            {
//                cond += " and c.operation_slno = " + Convert.ToString(ddloperation_slno.SelectedValue);
//            }
//            else
//            {
//                cond += " c.operation_slno=" + Convert.ToString(ddloperation_slno.SelectedValue);
//            }
//        }


//        //string qry = "SELECT  c.* FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " + 
//        //                "c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) ";

//        //string qry = "SELECT DISTINCT c.*,opn.operationdesc,mp.machine_slno as machine, case when c.Submitstatus='N' then 'Not Approved' " +
//        //    " when c.Submitstatus='A' then 'Approved' else 'Not Approved' end as ApproveStatus FROM controlplan c " +
//        //    " inner join PartsMapping mp on mp.operation_slno=c.operation_slno and mp.part_slno=c.part_slno "+
//        //    " right outer join operations opn on  c.operation_slno=opn.operation_slno" +
//        //    " where c.obsolete='N' and c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE " + 
//        //                " c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )";

////        string qry = @"SELECT
////  DISTINCT c.*,
////  opn.operationdesc,cast(c.process_no as decimal) as process_no,c.process_no,
////  m.machinedesc,
////  p.mstPartNo,
////  case
////    when c.Submitstatus = 'N' then 'Not Approved'
////    when c.Submitstatus = 'A' then 'Approved'
////    else 'Not Approved'
////  end as ApproveStatus
////FROM
////  controlplan c
////  inner join operations opn on c.operation_slno = opn.operation_slno
////  inner join parts p on p.part_slno = c.part_slno
////  inner join machines m on m.machine_slno=c.machine_slno
////where
////  c.obsolete = 'N'   and c.rev_no=(select max(rev_no) from controlplan )";
////   string     qry=@"; WITH MaxRev AS(
////    SELECT
////        part_slno,
////        operation_slno,
////        machine_slno,
////        MAX(rev_no) AS max_rev_no
////    FROM
////        controlplan
////    WHERE
////        obsolete = 'N'
////    GROUP BY
////        part_slno, operation_slno, machine_slno
////)
////SELECT
////    DISTINCT c.*,
////    opn.operationdesc,
////    CAST(c.process_no AS DECIMAL) AS process_no,
////    c.process_no,
////    m.machinedesc,
////    p.mstPartNo,
////    CASE
////        WHEN c.Submitstatus = 'N' THEN 'Not Approved'
////        WHEN c.Submitstatus = 'A' THEN 'Approved'
////        ELSE 'Not Approved'
////    END AS ApproveStatus
////FROM
////    controlplan c
////    INNER JOIN operations opn ON c.operation_slno = opn.operation_slno
////    INNER JOIN parts p ON p.part_slno = c.part_slno
////    INNER JOIN machines m ON m.machine_slno = c.machine_slno
////    INNER JOIN MaxRev mr ON c.part_slno = mr.part_slno
////                            AND c.operation_slno = mr.operation_slno
////                            AND c.machine_slno = mr.machine_slno
////                            AND c.rev_no = mr.max_rev_no
////WHERE
////    c.obsolete = 'N'  and rev_no=mr.max_rev_no";
        
//        var filteredList = lst
//    .GroupBy(x => new { x.part_slno, x.machine_slno, x.operation_slno })
//    .Select(g => g.OrderByDescending(x => x.rev_no).First())
//    .ToList();
//        if (cond.Length > 0)
//        {
//            qry += " AND " + cond + " order by cast(c.process_no as decimal)   ";
//        }

//        else
//        { qry += qry + " order by cast(c.process_no as decimal) , p.mstPartNo"; }
       
//        using (Database db=new Database("connString"))
//        {
//            List<Class_ControlPlan> lst = db.Query<Class_ControlPlan>(qry).ToList();

//            if (lst.Count > 0)
//            {
//                grdData.DataSource = lst;
//                grdData.DataBind();
//            }
//            else
//            {
//                grdData.DataSource = null;
//                grdData.DataBind();
//            }     
//        }
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