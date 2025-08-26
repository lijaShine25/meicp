using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
//using Microsoft.Reporting.WebForms;
using NPoco;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

public partial class Parts_Query : System.Web.UI.Page
{
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanApprove != "Y" && userInfo[0].allmaster != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadParts();
            LoadGridData();
            if (Request.QueryString.HasKeys())
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E";

            }
            else
            {
                //btnDelete.Enabled = false;
            }
        }
    }


    void LoadGridData()
    {
        string cond = string.Empty;

        if (ddlpart_slno.SelectedIndex > 0)
        {
            if (cond.Length > 0)
            {
                cond += " and pa.part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
            }
            else
            {
                cond += " pa.part_slno=" + Convert.ToInt32(ddlpart_slno.SelectedValue);
            }
        }
        if(ddlstatus.SelectedIndex > 0)
        {
            if(cond.Length >0)
            {
                cond += " and pa.del_status='" + ddlstatus.SelectedValue + "'";
            }
            else 
            {
                cond += " pa.del_status='" + ddlstatus.SelectedValue + "'";
            }

        }

         string qry = "select cf.CFTeamName, pa.*, case when pa.[del_status]='N' then 'ACTIVE' else 'IN-ACTIVE' end as [del_status] from parts pa, CFTeams cf where cf.CFTeamSlNo=pa.CftTeamSlNo And pa.Obsolete='N' ";
        //string qry = "select cf.CFTeamName, pa.* from parts pa, CFTeams cf where cf.CFTeamSlNo=pa.CftTeamSlNo And pa.Obsolete='N'";
       // string qry = "select cf.CFTeamName, pa.* from parts pa, CFTeams cf where cf.CFTeamSlNo=pa.CftTeamSlNo And pa.Obsolete='N' and pa.del_status='N'";

        if (cond.Length > 0)
        {
            qry += " AND " + cond;
        }
        using (Database db = new Database("connString"))
        {
            List<Class_parts> x = db.Query<Class_parts>(qry).ToList();

            if (x.Count > 0)
            {
                grdData.DataSource = x;
                grdData.DataBind();
            }
            else
            {
                grdData.DataSource = null;
                grdData.DataBind();
            }
        }

        //Crud_parts crud = new Crud_parts();
        //List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        ////List<Class_parts> lst = lst1.Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList();
        //List<Class_parts> lst = lst1.Where(x => x.Obsolete == "N").ToList();
        //grdData.DataSource = lst;
        //grdData.DataBind();
        //grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }


    protected void btnViewRptHistory_Click(object sender, EventArgs e)
    {
        LoadGridData();
    }

    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
       // List<Class_parts> lst = lst1.Where(x =>  x.Obsolete == "N" && x.del_status == "ACTIVE").ToList();
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

    
}