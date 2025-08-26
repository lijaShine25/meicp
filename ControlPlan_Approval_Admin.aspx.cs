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

public partial class ControlPlan_Approval_Admin : System.Web.UI.Page
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
      
        if (userInfo[0].isAdmin != "Y" )
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

    void LoadGridData()
    {
        Crud_PartsMapping crud = new Crud_PartsMapping();
        List<Class_PartsMapping> lst = null;

        List<Class_PartsMapping> lst1 = crud.usp_PartsMappingSelect_CPApproval().ToList();
        if (ddlstatus.SelectedIndex > 0 && ddlpart_slno.SelectedIndex > 0)
        {
            lst = lst1.Where(x => x.is_approved == ddlstatus.SelectedItem.Text && x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)).ToList();
        }
        else
        {
            if (ddlpart_slno.SelectedIndex > 0)
            {
                List<Class_PartsMapping> lst2 = lst1.Where(x => x.part_slno == Convert.ToInt16(ddlpart_slno.SelectedValue)).ToList();
                lst = lst2;

            }
            else if (ddlstatus.SelectedIndex > 0)
            {
               lst = lst1.Where(x => x.is_approved == ddlstatus.SelectedItem.Text).ToList();
                
            }
            else
                lst = lst1.ToList();
        }

        grdData.DataSource = lst;
        grdData.DataBind();
        
    }

    protected void grdData_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            int cpdoneColumnIndex = 5; 
            System.Web.UI.WebControls.Label lblCpdone = (System.Web.UI.WebControls.Label)e.Row.Cells[cpdoneColumnIndex].FindControl("CPSlNo");
            TableCell cell = e.Row.Cells[8];

            string isApproved = cell.Text;

            if (lblCpdone.Text == "0") 
            {
                e.Row.BackColor = System.Drawing.Color.Orange;
              

            }

        }

  

    }
  
    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
       // List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        List<Class_parts> lst1 = crud.usp_partsSelect_mapping().ToList();
        
        List<Class_parts> lst = lst1.Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList();
        ddlpart_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
        }
        ddlpart_slno.Items.Insert(0, "Select...");
    }



}