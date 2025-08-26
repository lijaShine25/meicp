using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;

public partial class CPViewers : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo[0].isAdmin != "Y" && userInfo[0].allmaster != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            //if (userInfo[0].is_ET_User != "Y")
            //{
            //    Response.Redirect("~/AccessDenied.aspx");
            //}

            LoadGridData();

            if (Request.QueryString.HasKeys() && Request.QueryString != null)
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E";
                GetDetails();
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
            }
        }
    }

    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData();
        LoadGridData();
        ClearData();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteData();
        LoadGridData();
        ClearData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }


    void SaveData()
    {
        using (Class_Employees cls = new Class_Employees())
        {
            cls.EmployeeRollNo = txtEmployeeRollNo.Text;
            cls.EmployeeName = txtEmployeeName.Text;
            cls.EmailId = String.Empty;
            cls.CanPrepare = "N";
            cls.CanApprove = "N";
            cls.isAdmin = "N";
            cls.allmaster = "N";
            cls.del_status = ddlActiveInactive.SelectedValue;
            cls.login_id = txtLoginId.Text;

            if (txtPassword.Text.Length == 0 || txtPassword.Text != hdnPassword.Value)
            {
                cls.login_pwd = hdnPassword.Value;
            }
            else
            {
                cls.login_pwd = txtPassword.Text;
            }
            
            using (Crud_Employees crud = new Crud_Employees())
            {
                if (hdnMode.Value == "I")
                {
                    crud.usp_EmployeesInsert(cls);
                    ShowMessage("Record Inserted Successfully", MessageType.Success);
                }
                else if (hdnMode.Value == "E")
                {
                    cls.EmployeeSlNo = Convert.ToInt16(hdnSlNo.Value);
                    crud.usp_EmployeesUpdate(cls);
                    ShowMessage("Record Updated Successfully", MessageType.Success);
                }
            }
        }
    }

    void DeleteData()
    {
        using (Class_Employees cls = new Class_Employees())
        {
            cls.EmployeeSlNo = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_Employees crud = new Crud_Employees())
            {
                crud.usp_EmployeesDelete(cls);
            }
        }
        ShowMessage("Record Deleted Successfully", MessageType.Success);
    }

    void ClearData()
    {
        txtEmployeeRollNo.Text = "";
        txtEmployeeName.Text = "";
        txtEmailId.Text = "";
        ddlCanPrepare.SelectedIndex = 1;
        ddlCanApprove.SelectedIndex = 1;
        ddlisAdmin.SelectedIndex = 1;
        ddlallmaster.SelectedIndex = 1;
        txtLoginId.Text = "";
        txtPassword.Text = "";
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
        hdnPassword.Value = string.Empty;
        ddlActiveInactive.SelectedIndex = 0;

    }

    void LoadGridData()
    {
        Crud_Employees crud = new Crud_Employees();
        List<Class_Employees> lst1 = crud.usp_EmployeesSelect().ToList();
        List<Class_Employees> lst = lst1.Where(x =>  x.CanApprove=="N" && x.CanPrepare=="N" && x.allmaster=="N" && x.isAdmin=="N" ).ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_Employees crud = new Crud_Employees();
        List<Class_Employees> lst1 = crud.usp_EmployeesSelect().ToList();
        List<Class_Employees> lst = lst1.Where(x => x.EmployeeSlNo == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtEmployeeRollNo.Text = lst[0].EmployeeRollNo;
        txtEmployeeName.Text = lst[0].EmployeeName;
        txtEmailId.Text = lst[0].EmailId;
        txtLoginId.Text = lst[0].login_id;
        txtPassword.Text = lst[0].login_pwd;
        hdnPassword.Value = lst[0].login_pwd;
        ddlCanPrepare.SelectedValue = lst[0].CanPrepare.ToString();
        ddlCanApprove.SelectedValue = lst[0].CanApprove.ToString();
        ddlisAdmin.SelectedValue = lst[0].isAdmin.ToString();
        ddlallmaster.SelectedValue = lst[0].allmaster.ToString();
        ddlActiveInactive.SelectedValue = lst[0].status1;    
    }
}