using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Elmah;
using Newtonsoft.Json;
using NPoco;


public partial class MasterPage : System.Web.UI.MasterPage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Class_Employees> udet = (List<Class_Employees>)Session["UserInfo"];

        if (!IsPostBack)
        {
            if (udet != null && udet.Count > 0)
            {
                //lblUserName3.Text = udet[0].empl_name;
                lblUserName4.Text = udet[0].EmployeeName;
                //if (udet[0].isAdmin != "Y")
                //{ li_cpapproval.Visible = false; }
                //else { li_cpapproval.Visible = true; }
                
            }
            if (udet[0].isAdmin != "Y" && udet[0].CanPrepare != "Y" && udet[0].CanApprove != "Y" && udet[0].allmaster != "Y")
            {
                mnuMasters.Visible = false;
                mnuReports.Visible = false;
                mnuChangepwd.Visible = false;
            }
        }
    }


}
