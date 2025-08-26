using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Elmah;
using Newtonsoft.Json;
using NPoco;

public partial class ChangePassword : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
    }

    /// <summary>
    /// updates the employee master with isET User as Y
    /// </summary>
    void UpdateEmployees()
    {
        // check if old password is correct
        string sqlOldPwd = "select login_pwd from employees where employeeslno=" + userInfo[0].EmployeeSlNo;
        using (Database db = new Database("connString"))
        {
            string oldPwd = db.ExecuteScalar<string>(sqlOldPwd);

            if (oldPwd == txtOldPassword.Text.Trim())
            {
                string sqlUpd = "update employees set login_pwd='" + txtNewPassword.Text + "' where employeeslno=" + userInfo[0].EmployeeSlNo;
                db.Execute(sqlUpd);
                ShowMessage("Password Updated Successfully !!", MessageType.Success);
            }
            else
            {
                ShowMessage("Old Password does not match!!", MessageType.Info);
                ClearData();
            }
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

    void ClearData()
    {
        txtOldPassword.Text = "";
        txtNewPassword.Text = "";
        txtNewPassword2.Text = "";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        UpdateEmployees();
        // ShowMessage("Password Reset Successfully", MessageType.Success);
        ClearData();
    }

}