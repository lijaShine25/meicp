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

public partial class PendingApproval : System.Web.UI.Page
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

        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanApprove != "Y" )
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadGridData();
            LoadDCRGridData();
        }
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

    void LoadGridData()
    {
        using(Crud_ControlPlan crud = new Crud_ControlPlan()) 
        {
            List<Class_ControlPlan> lst = crud.PendingApproval(userInfo[0].EmployeeSlNo);
            if (lst.Count > 0)
            {
                grdData.DataSource = lst;
                grdData.DataBind();
            }
            else
            {
                grdData.DataSource = new string[] { };
                grdData.DataBind();
            }
        }
    }


    void LoadDCRGridData()
    {
        using (Crud_dcr crud = new Crud_dcr())
        {
            List<Class_DCR> lst = crud.PendingDCRApproval(userInfo[0].EmployeeSlNo);
            if (lst.Count > 0)
            {
                grdDCR.DataSource = lst;
                grdDCR.DataBind();
            }
            else
            {
                grdDCR.DataSource = new string[] { };
                grdDCR.DataBind();
            }
        }
    }
}