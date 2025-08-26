using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPoco;
public partial class dcr_sop_qry : System.Web.UI.Page
{
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (!IsPostBack)
        {
            LoadGroups();

            LoadGrid();
        }
    }

    void LoadGrid()
    {

        string cond = string.Empty;
       

        string qry = string.Empty;
        if (userInfo[0].isAdmin != "Y")
        {
            if (ddlgroup.SelectedIndex > 0)
            {

                cond += " and c.Group_Id=" + Convert.ToInt32(ddlgroup.SelectedValue);


            }
            qry = @"SELECT distinct dcr_number,c.*,p.Group_Name,e.employeename ,case when c.Submit_Status = 'N' then 'Not Submitted'
        when c.Submit_Status = 'Y' then 'Submitted' when c.Submit_Status = 'A' then 'Approved'  when c.Submit_Status = 'O' then 'Obsolete'  when c.Submit_Status = 'R' then 'Revision Initiated' 
        end as Submit_Status FROM dcr_sop c 
        inner join sop_mapping p on p.Group_Id = c.Group_Id inner join employees e on e.employeeslno=c.Request_By  where c.del_status ='N' or submit_status<>'O' ";
            if (cond.Length > 0)
            {
                qry += " AND " + cond + " order by  c.dcr_slno ";
            }
            else
            { qry += " order by  c.dcr_slno "; }
        }
        if (userInfo[0].isAdmin == "Y")
        {
            if (ddlgroup.SelectedIndex > 0)
            {

                cond += "   where  c.Group_Id=" + Convert.ToInt32(ddlgroup.SelectedValue);


            }
            qry = @"SELECT distinct dcr_number,c.*,p.Group_Name,e.employeename, case when c.Submit_Status = 'N' then 'Not Submitted'
        when c.Submit_Status = 'Y' then 'Submitted' when c.Submit_Status = 'A' then 'Approved'  when c.Submit_Status = 'O' then 'Obsolete' when c.Submit_Status = 'R' then 'Revision Initiated' 
        end as Submit_Status FROM dcr_sop c 
        inner join sop_mapping p on p.Group_Id = c.Group_Id inner join employees e on e.employeeslno=c.Request_By    ";
            if (cond.Length > 0)
            {
                qry +=  cond + " order by  c.dcr_slno ";
            }
            else
            { qry += " order by  c.dcr_slno "; }
        }
       
        using (Database db = new Database("connString"))
        {
            List<Class_DCR_SOP> lst = db.Query<Class_DCR_SOP>(qry).ToList();

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

    protected void btnViewRptHistory_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }

    void LoadGroups()
    {
        using (Database db = new Database("connString"))
        {
            List<Class_sop_mapping> lst = db.Query<Class_sop_mapping>("Select distinct Group_Id,Group_Name From Sop_mapping where del_status='N'").ToList();
        ddlgroup.Items.Clear();
            if (lst.Count > 0)
            {
                for (int cnt = 0; cnt < lst.Count; cnt++)
                {
                    ddlgroup.Items.Add(new ListItem(lst[cnt].Group_Name, Convert.ToString(lst[cnt].Group_Id)));
                }

                ddlgroup.Items.Insert(0, "Select...");
            }
        }
    }

}