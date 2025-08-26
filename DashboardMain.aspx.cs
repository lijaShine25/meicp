using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Database = NPoco.Database;
public partial class DashboardMain : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            CheckPendingApprovals();
            GetTotalCP();
            GetTotalSOP();

        }
    }

    void GetTotalSOP()
    {
        using (Database db = new Database("connString"))
        {
            // Query 1: TOTAL SOP
            string query1 = @"SELECT  COUNT(DISTINCT group_id) from SOP_Mapping  where Obsolete='N'   and del_status='N'   and Active In ('Active' ,'In Active') ";
            int totalSop = db.ExecuteScalar<int>(query1);

            // Query 2: ACTIVE SOP
            string query2 = @"SELECT  COUNT(DISTINCT group_id) from SOP_Mapping  where Obsolete='N'   and del_status='N' and Active='Active'";
            int activeSop = db.ExecuteScalar<int>(query2);

            // Query 3: INACTIVE SOP
            string query3 = @"SELECT  COUNT(DISTINCT group_id) from SOP_Mapping  where   Active='In Active' and Obsolete='N'   and del_status='N'";
            int inactiveSop = db.ExecuteScalar<int>(query3);

            // Query 4: TOTAL APPROVED SOP
            string query4 = @"   Select COUNT(distinct m.Group_id)  from [SOP_Mapping] m inner join sop_header_new h on m.Map_slno=h.Map_slno 
  where is_approved=1  and h.is_obsolete=0  and m.Obsolete='N'  and m.del_status='N'";
            int totalApprovedSop = db.ExecuteScalar<int>(query4);

            // Query 5: NOT SUBMITTED SOP
            string query5 = @"  Select COUNT(distinct m.Group_id)  from [SOP_Mapping] m  inner join sop_header_new h on m.Map_slno=h.Map_slno 
  where  h.is_obsolete=0  and m.Obsolete='N'  and m.del_status='N' and  h.submitstatus='N'   union select count(distinct sm.group_id)
from SOP_Mapping sm
where sm.obsolete = 'N'
  and sm.del_status = 'N'
  and (
        not exists (  -- no header at all
            select 1
            from sop_header_new sh
            where sh.group_id = sm.group_id
        )
        or not exists ( -- all headers obsolete (no non-obsolete)
            select 1
            from sop_header_new sh
            where sh.group_id = sm.group_id
              and sh.is_obsolete = '0'
        )
      );
";

            query5 = @"select 
(
    -- first count
    select count(distinct m.group_id)
    from SOP_Mapping m
    inner join sop_header_new h on m.Map_slno = h.Map_slno
    where h.is_obsolete = 0
      and m.obsolete = 'N'
      and m.del_status = 'N'
      and h.submitstatus = 'N'
)
+
(
    -- second count
    select count(distinct sm.group_id)
    from SOP_Mapping sm
    where sm.obsolete = 'N'
      and sm.del_status = 'N'
      and (
            not exists (
                select 1
                from sop_header_new sh
                where sh.group_id = sm.group_id
            )
            or not exists (
                select 1
                from sop_header_new sh
                where sh.group_id = sm.group_id
                  and sh.is_obsolete = 0
            )
          )
) as TotalCount;";

            int notSubmittedSop = db.ExecuteScalar<int>(query5);

            // Query 6: SUBMITTED SOP
            string query6 = @" Select COUNT(distinct m.Group_id)  from [SOP_Mapping] m inner join sop_header_new h on m.Map_slno=h.Map_slno 
  where  h.is_obsolete=0  and m.Obsolete='N'  and m.del_status='N' and  h.submitstatus='Y' and m.SubmitStatus='Y'";
            int submittedSop = db.ExecuteScalar<int>(query6);

            // Set the label text with the values from the query results
            lblTotalSop.Text = totalSop.ToString();
            lblActiveSop.Text = activeSop.ToString();
            lblInactiveSop.Text = inactiveSop.ToString();
            lblTotalApprovedSop.Text = totalApprovedSop.ToString();
            lblNotSubmittedSop.Text = notSubmittedSop.ToString();
            lblSubmittedSop.Text = submittedSop.ToString();
        }
    }

    void GetTotalCP()
    {
        using (Database db = new Database("connString"))
        {
            // Query 1: INACTIVE - CONTROL PLAN
            string query1 = @"select count(distinct c.part_slno) from ControlPlan c inner join parts p on p.part_slno=c.part_slno   where p.Obsolete='N'   and c.Obsolete='N'   and p.del_status='Y'";

            int inactiveControlPlan = db.ExecuteScalar<int>(query1);

            // Query 2: ACTIVE - CONTROL PLAN
            string query2 = @"select count(distinct c.part_slno) from ControlPlan c inner join parts p on p.part_slno=c.part_slno   where p.Obsolete='N'   and c.Obsolete='N'   and p.del_status='N' ";
            int activeControlPlan = db.ExecuteScalar<int>(query2);

            // Query 3: TOTAL APPROVED - CONTROL PLAN
            string query3 = @";WITH ApprovedParts AS (
    SELECT 
        part_slno,
        COUNT(*) AS TotalOps,
        SUM(CASE WHEN Submitstatus = 'A' AND is_approved = 1 THEN 1 ELSE 0 END) AS ApprovedOps
    FROM ControlPlan
    WHERE Obsolete = 'N'  -- exclude obsolete if needed
    GROUP BY part_slno
)
SELECT COUNT(*) AS ApprovedControlPlanCount
FROM ApprovedParts
WHERE TotalOps = ApprovedOps; ";
            int totalApprovedControlPlan = db.ExecuteScalar<int>(query3);

            // Query 4: NOT SUBMITTED - CONTROL PLAN
            string query4 = @";WITH PartOps AS (
    SELECT 
        part_slno, 
        operation_slno, 
        LTRIM(RTRIM(ISNULL(SubmitStatus, ''))) AS SubmitStatus
    FROM ControlPlan
    WHERE ISNULL(Obsolete,'N') = 'N'
),
PartSummary AS (
    SELECT 
        part_slno,
        COUNT(*) AS TotalOps,
        SUM(CASE WHEN SubmitStatus IN ('Y','A') THEN 1 ELSE 0 END) AS SubmittedOps
    FROM PartOps
    GROUP BY part_slno
)
SELECT COUNT(*) AS NotSubmittedControlPlans
FROM PartSummary
WHERE SubmittedOps < TotalOps;
";
            int notSubmittedControlPlan = db.ExecuteScalar<int>(query4);

            // Query 5: SUBMITTED - CONTROL PLAN
            string query5 = @";WITH PartOps AS (
    SELECT 
        part_slno, 
        LTRIM(RTRIM(ISNULL(SubmitStatus, ''))) AS SubmitStatus
    FROM ControlPlan
    WHERE ISNULL(Obsolete,'N') = 'N'
),
PartSummary AS (
    SELECT 
        part_slno,
        COUNT(*) AS TotalOps,
        SUM(CASE WHEN SubmitStatus = 'Y' THEN 1 ELSE 0 END) AS YCount,
        SUM(CASE WHEN SubmitStatus = 'A' THEN 1 ELSE 0 END) AS ACount,
        SUM(CASE WHEN SubmitStatus IN ('N','') OR SubmitStatus IS NULL THEN 1 ELSE 0 END) AS InvalidOps
    FROM PartOps
    GROUP BY part_slno
)
SELECT COUNT(*) AS SubmittedControlPlans
FROM PartSummary
WHERE 
    -- must have at least one Y
    YCount > 0
    -- all others must be A
    AND (YCount + ACount) = TotalOps
    -- no N, NULL, empty
    AND InvalidOps = 0;";
;
            int submittedControlPlan = db.ExecuteScalar<int>(query5);

            // Query 6: TOTAL CONTROL PLAN
            string query6 = @" select count(distinct c.part_slno)
    from ControlPlan c
    inner join parts p on p.part_slno = c.part_slno
    where p.Obsolete = 'N'
      and c.Obsolete = 'N'";
            int totalControlPlan = db.ExecuteScalar<int>(query6);

            // Set the label text with the values from the query results
            lblInactiveControlPlan.Text = inactiveControlPlan.ToString();
            lblActiveControlPlan.Text = activeControlPlan.ToString();
            lblTotalApprovedControlPlan.Text = totalApprovedControlPlan.ToString();
            lblNotSubmittedControlPlan.Text = notSubmittedControlPlan.ToString();
            lblSubmittedControlPlan.Text = submittedControlPlan.ToString();
            lblTotalControlPlan.Text = totalControlPlan.ToString();
        }
    }

    void CheckPendingApprovals()
    {
        using (Crud_ControlPlan crud = new Crud_ControlPlan())
        {
            List<Class_ControlPlan> lst = crud.PendingApproval(userInfo[0].EmployeeSlNo);
            List<Class_DCR> lstdcr = null;
            using (Crud_dcr cruddcr = new Crud_dcr())
            {
                lstdcr = cruddcr.PendingDCRApproval(userInfo[0].EmployeeSlNo);
            }
            if (lst.Count == 0 && lstdcr.Count == 0)
            {
                pending.Visible = false;
            }
            else
            {
                pending.Visible = true;
            }
        }
    }



}