using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NPoco;
using Elmah;
using System.Web.Security;

public partial class LoginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                string a = Request.QueryString[0];
                if (a.Contains("/"))
                {

                }
                else
                {
                    byte[] b = Convert.FromBase64String(a);
                    string c = System.Text.Encoding.Unicode.GetString(b);
                    if (c == "Logout")
                    {
                        FormsAuthentication.SignOut();
                        Session.Clear();
                        Session.Abandon();
                    }
                }
            }
        }
    }

    protected void chkUser()
    {
        using (Database db = new Database("connString"))
        {
            var usr = db.Query<Class_Employees>("Select * from Employees where login_id=@0 and login_pwd=@1 and del_status='N'", txtuser.Value, txtpassword.Value).ToList();

            if (usr!=null && usr.Count >0)
            {
                Class_Employees clsemp = usr[0];
                Session["UserInfo"] = usr;
                FormsAuthentication.SetAuthCookie(txtuser.Value, false);
                if (clsemp.isAdmin != "Y" && clsemp.CanPrepare != "Y" && clsemp.CanApprove != "Y" && clsemp.allmaster != "Y")
                {
                   // Response.Redirect("~/OpenViewer.aspx");
Response.Redirect("~/DashboardMain.aspx");
                }
                else
                {
                    Response.Redirect("~/DashboardMain.aspx");
                }
            }
            else
            {
                lblmsg.Text = "Invalid User Name/ Password";
                txtuser.Value = "";
                txtpassword.Value = "";
            }
        }
    }

    protected void btnsubmit_Click1(object sender, EventArgs e)
    {
        chkUser();
    }
}