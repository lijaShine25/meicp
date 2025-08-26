using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;

public partial class Cfteams : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };

    List<Class_CFTeams> TeamInfo = null;
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];

        if (userInfo == null)
        {
            Response.Redirect("~/LoginPage.aspx");
        }
        if (userInfo[0].isAdmin != "Y" && userInfo[0].allmaster != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (userInfo[0].isAdmin != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }

        if (!Page.IsPostBack)
        {
            //if (userInfo[0].is_ET_User != "Y")
            //{
            //    Response.Redirect("~/AccessDenied.aspx");
            //}
            LoadGridEmpls();
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

    /// <summary>
    /// displays user messages in the top of the screen
    /// </summary>
    /// <param name="Message">Message content</param>
    /// <param name="type">Message type as Success, Error, Info, Warning</param>
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (AtleastOneSelected())
        {
            if (!CheckNameExists())
            {
                SaveData();
                LoadGridEmpls();
                LoadGridData();
                ClearData();
            }
            else
            {
                ShowMessage("Team Name already exists!!", MessageType.Info);
            }
        }
        else
        {
            ShowMessage("Select atleast one employee for the team", MessageType.Info);
        }
    }

    bool CheckNameExists()
    {
        if (hdnMode.Value == "I")
        {
            using (Crud_CFTeams crud = new Crud_CFTeams())
            {
                List<Class_CFTeams> lst = crud.usp_CFTeamsSelect().ToList();
                List<Class_CFTeams> lst1 = lst.Where(x => x.CFTeamName == txtCFTeamName.Text.ToUpper()).ToList();
                if (lst1.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteData();
        LoadGridEmpls();
        LoadGridData();
        ClearData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }

    /// <summary>
    /// checks if atleast one employee in the grid is selected
    /// </summary>
    /// <returns>true if atleast one is selected; false if none is selected</returns>
    bool AtleastOneSelected()
    {
        bool chkd = false;
        foreach (GridViewRow row in grdEmpls.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkSel");
            if (chk.Checked)
            {
                chkd = true;
                break;
            }
        }
        return chkd;
    }

    /// <summary>
    /// saves the business locations data
    /// </summary>

    void SaveData()
    {
        using (Class_CFTeams cls = new Class_CFTeams())
        {
            cls.CFTeamName = txtCFTeamName.Text;
            cls.del_status = ddlActiveInactive.SelectedValue;

            using (Crud_CFTeams crud = new Crud_CFTeams())
            {
                List<Class_CFTeams> lstC1 = crud.usp_CFTeamsSelect().Where(x => x.CFTeamName.ToUpper().Trim() == txtCFTeamName.Text.ToUpper().Trim()).ToList();


                if (hdnMode.Value == "I")
                {
                    if (lstC1.Count > 0)
                    {
                        ShowMessage("Record Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        crud.usp_CFTeamsInsert(cls);
                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                    }
                    List<Class_CFTeams> lst = crud.usp_CFTeamsSelect().ToList();

                    if (lst.Count > 0)
                    {
                        List<Class_CFTeams> lst1 = lst.Where(x => x.CFTeamName.ToUpper() == txtCFTeamName.Text.ToUpper()).ToList();
                        if (lst1.Count > 0)
                        {
                            hdnSlNo.Value = Convert.ToString(lst1[0].CFTeamSlNo);
                            cls.CFTeamSlNo = Convert.ToInt32(hdnSlNo.Value);
                        }
                        else
                        {
                            hdnSlNo.Value = string.Empty;
                        }
                    }
                }
                else if (hdnMode.Value == "E")
                {

                    cls.CFTeamSlNo = Convert.ToInt16(hdnSlNo.Value);
                    crud.usp_CFTeamsUpdate(cls);
                    ShowMessage("Record Updated Successfully", MessageType.Success);


                }
                SaveData_TeamMembers();
            }
        }
    }

    void SaveData_TeamMembers()
    {
        using (Class_CFTeamEmployees cls = new Class_CFTeamEmployees())
        {
            if (!string.IsNullOrEmpty(hdnSlNo.Value))
                cls.CFTeamSlNo = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_CFTeamEmployees crud = new Crud_CFTeamEmployees())
            {
                // delete all existing data
                crud.usp_CFTeamEmployeesDelete(cls);

                // insert the employees
                foreach (GridViewRow row in grdEmpls.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSel");
                    if (chk.Checked)
                    {
                        Label lbl = (Label)row.FindControl("lblSlNo");
                        cls.EmployeeSlNo = Convert.ToInt16(lbl.Text);
                        crud.usp_CFTeamEmployeesInsert(cls);
                    }
                }
            }
        }
    }

    void DeleteData()
    {
        using (Class_CFTeams cls = new Class_CFTeams())
        {
            cls.CFTeamSlNo = Convert.ToInt32(hdnSlNo.Value);

            using (Crud_CFTeams crud = new Crud_CFTeams())
            {
                Crud_parts crudcp = new Crud_parts();
                List<Class_parts> lst = crudcp.usp_partsSelect().Where(x => x.CftTeamSlNo == Convert.ToInt16(hdnSlNo.Value)).ToList();

                if (lst.Count > 0)
                {
                    ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
                }
                else
                {
                    //crud.usp_CFTeamsDelete(cls);
                    //ShowMessage("Record Deleted Successfully", MessageType.Success);

                    using (Class_CFTeamEmployees cls1 = new Class_CFTeamEmployees())
                    {
                        cls1.CFTeamSlNo = Convert.ToInt32(hdnSlNo.Value);
                        using (Crud_CFTeamEmployees crud1 = new Crud_CFTeamEmployees())
                        {
                            crud1.usp_CFTeamEmployeesDelete(cls1);
                        }
                    }
                    crud.usp_CFTeamsDelete(cls);
                    ShowMessage("Record Deleted Successfully", MessageType.Success);
                }

            }
        }
    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        txtCFTeamName.Text = "";
        hdnMode.Value = "I";
        hdnSlNo.Value = "";

        LoadGridData();
        LoadGridEmpls();
        ddlActiveInactive.SelectedIndex = 0;
        btnDelete.Enabled = false;

    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>
    void LoadGridData()
    {
        Crud_CFTeams crud = new Crud_CFTeams();
        List<Class_CFTeams> lst = crud.usp_CFTeamsSelect().ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void LoadGridEmpls()
    {
        Crud_Employees crud = new Crud_Employees();
        List<Class_Employees> lst1 = crud.usp_EmployeesSelect().ToList();
        List<Class_Employees> lst = lst1.Where(x => x.del_status == "ACTIVE").ToList();
        grdEmpls.DataSource = lst;
        grdEmpls.DataBind();
        grdEmpls.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_CFTeams crud = new Crud_CFTeams();
        List<Class_CFTeams> lst1 = crud.usp_CFTeamsSelect().ToList();


        if (lst1.Count > 0)
        {
            List<Class_CFTeams> lst = lst1.Where(x => x.CFTeamSlNo == Convert.ToInt16(hdnSlNo.Value)).ToList();
            ddlActiveInactive.SelectedValue = lst[0].status2.ToString();
            ddlActiveInactive.Enabled = true;

            if (lst.Count > 0)
            {
                txtCFTeamName.Text = lst[0].CFTeamName;
                GetDetailsMembers();
            }

        }


    }

    void GetDetailsMembers()
    {
        using (Crud_CFTeamEmployees crud = new Crud_CFTeamEmployees())
        {
            Class_CFTeams cls = new Class_CFTeams();
            List<Class_CFTeamEmployees> lst1 = crud.usp_CFTeamEmployeesSelect().ToList();
            List<Class_CFTeamEmployees> lst = lst1.Where(x => x.CFTeamSlNo == Convert.ToInt16(hdnSlNo.Value)).ToList();
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    foreach (GridViewRow row in grdEmpls.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("chkSel");
                        Label lbl = (Label)row.FindControl("lblSlNo");
                        if (Convert.ToInt16(lbl.Text) == lst[i].EmployeeSlNo)
                        {
                            chk.Checked = true;
                        }
                    }
                }
            }
        }
    }
}