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
using DataBinder = System.Web.UI.DataBinder;
using System.Windows.Forms;

public partial class DCR_Approval_Admin : System.Web.UI.Page
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

        if (userInfo[0].isAdmin != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }

        if (!Page.IsPostBack)
        {

            LoadParts();
            LoadGridData();
        }
    }

    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {

        ddlpart_slno.SelectedIndex = 0;
        ddlstatus.SelectedIndex = 0;
        LoadGridData();

    }


    protected void btnFilder_Click(object sender, EventArgs e)
    {
        LoadGridData();
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("ControlPlan_Approval.aspx");
    }

    void ClearData()
    {
        ddlpart_slno.SelectedIndex = 0;
        ddlstatus.SelectedIndex = 0;
        LoadGridData();

    }
    public List<Class_DCR> PendingDCRApproval()
    {
        string sql = @"select distinct d.dcr_slno, p.mstPartNo,d.dcr_number, p.PartDescription,d.DCR_Submit_DateTime,d.DCR_Approved_DateTime,
STUFF((
        SELECT ', ' + opn.OperationDesc
        FROM operations opn
        WHERE opn.operation_slno IN (
            SELECT Value 
            FROM dbo.SplitString(d.operations, ',')
        )
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS OperationDesc,d.Request_Date,d.change_area, e.employeename,d.part_slno,CASE 
        WHEN Submit_Status = 'Y' THEN 'Submitted'
        WHEN Submit_Status = 'A' THEN 'Approved'
         WHEN Submit_Status = 'O' THEN 'Obsolete'
 WHEN Submit_Status = 'N' THEN 'Not Submitted'
    END AS   Submit_Status
from dcr d inner join controlplan cp on cp.part_slno=d.part_slno and cp.operation_slno IN (SELECT Value FROM dbo.SplitString(d.operations, ','))
inner join parts p on p.part_slno=d.part_slno
inner join employees e on e.employeeslno=d.Request_By
left outer join machines  m on m.machine_slno=cp.machine_slno
where  cp.Obsolete<>'Y' ";
        using (Database db = new Database("connString"))
        {
            List<Class_DCR> lst = db.Fetch<Class_DCR>(sql);
            return lst;
        }
    }
    void LoadGridData()
    {

        List<Class_DCR> lst = PendingDCRApproval();
        List<Class_DCR> lst2 = lst;
        if (ddlstatus.SelectedIndex > 0 && ddlpart_slno.SelectedIndex > 0)
        {
            lst2 = lst2.Where(x => x.Submit_Status == ddlstatus.SelectedItem.Text && x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)).ToList();
        }
        else
        {
            if (ddlpart_slno.SelectedIndex > 0)
            {

                if (!string.IsNullOrEmpty(ddlpart_slno.SelectedValue))
                {
                    int selectedValue;
                    if (int.TryParse(ddlpart_slno.SelectedValue, out selectedValue))
                    {
                        lst2 = lst2.Where(x => x.part_slno == selectedValue).ToList();
                    }
                }
            }
            if (ddlstatus.SelectedIndex > 0)
            {
                lst2 = lst2.Where(x => x.Submit_Status == ddlstatus.SelectedItem.Text).ToList();
            }
        }
            if (lst2.Count > 0)
        {
            grdData.DataSource = lst2;
            grdData.DataBind();
        }
        else
        {
            grdData.DataSource = new string[] { };
            grdData.DataBind();
        }



    }


    void LoadParts()
    {
        using (Database db = new Database("connString"))
        {
            string sql = "select distinct d.part_slno ,p.mstpartNo  from dcr d inner join parts p on p.part_slno=d.part_slno  where p.del_status='N'";
            List<Class_parts> lst = db.Query<Class_parts>(sql).ToList();



            for (int cnt = 0; cnt < lst.Count; cnt++)
            {
                ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
            }
            //ddlpart_slno.Items.Insert(0, "Select...");
        }
    }


}