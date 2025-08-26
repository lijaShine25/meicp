using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPoco;
using Newtonsoft.Json;

public partial class Settings : System.Web.UI.Page
{
    List<Class_Employees> userInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];

        //if (userInfo[0].isAdmin != "Y" && userInfo[0].allmaster !="Y")
        if (userInfo[0].isAdmin != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadSettingsData();
        }

    }
    protected void LoadSettingsData()
    {
        CrudSettings csData = new CrudSettings();
        List<Class_Settings> settingList = JsonConvert.DeserializeObject<List<Class_Settings>>(csData.SelectSettings());

        txtFromMail.Text = settingList[0].from_mailid;
        hdnPwd.Value = settingList[0].from_mailpwd;
        txtSmtp.Text = settingList[0].smtp_address;
        txtPort.Text = Convert.ToString(settingList[0].smtp_port);
        ddlAdsl.SelectedValue = settingList[0].enable_adsl;

        ddlReminders.SelectedValue = settingList[0].enable_reminders;
        ddlMailTrigger.SelectedValue = settingList[0].enable_trigger;

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtMailPwd.Text.Length > 0)
        {
            hdnPwd.Value = txtMailPwd.Text;
        }
        using (Database db = new Database("connString"))
        {
            
            db.Update<Class_Settings>("set from_mailid=@0, from_mailpwd=@1, smtp_address=@2, smtp_port=@3, enable_adsl=@4, enable_trigger=@5, enable_reminders=@6 ",
                txtFromMail.Text, hdnPwd.Value, txtSmtp.Text, txtPort.Text, ddlAdsl.SelectedValue, ddlMailTrigger.SelectedValue, ddlReminders.SelectedValue );
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Message",
                    "alert('Updated Successfully !!');window.location ='Default.aspx';", true);
    }

    protected void btnDefault_Click(object sender, EventArgs e)
    {
        txtPort.Text = "25";
        txtSmtp.Text = "smtp.sandvik.com";
        txtFromMail.Text = "vidya.sagar@sandvik.com";
        ddlAdsl.SelectedIndex = 0;
        ddlMailTrigger.SelectedIndex = 0;
        ddlReminders.SelectedIndex = 0;
    }
    protected void btnTestMail_Click(object sender, EventArgs e)
    {
        List<string>toMail = new List<string>();
        toMail.Add( txtToMail.Text);
        mail ml = new mail() ;
        ml.SendMail(toMail, "Test mail", "Test mail from the recently launched application of QSQSPL");

    }
}