using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;
using Microsoft.SqlServer.Management.Smo;

public partial class EvaluationTechnique : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
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

    bool CheckEvalTech()
    {
        bool exists = false;
        using (Crud_EvaluationTech crud = new Crud_EvaluationTech())
        {
            if (hdnMode.Value == "I")
            {
                List<Class_EvaluationTech> lst = crud.usp_EvaluationTechSelect()
                    .Where(x => x.evalTech.ToUpper().Trim() == txtEvalTech.Text.ToUpper().Trim()).ToList();
                if (lst.Count > 0)
                    exists = true;
                else exists = false;
            }
            else if (hdnMode.Value == "E")
            {
                List<Class_EvaluationTech> lst = crud.usp_EvaluationTechSelect()
                    .Where(x => x.evalTech.ToUpper().Trim() == txtEvalTech.Text.ToUpper().Trim() &&
                                x.evalTech_slno != Convert.ToInt32(hdnSlNo.Value)).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
            return exists;
        }

    }

    /// <summary>
    /// saves the business locations data
    /// </summary>
    void SaveData()
    {
        using (Class_EvaluationTech cls = new Class_EvaluationTech())
        {
            cls.evalTech = txtEvalTech.Text.Trim();
            cls.del_status = ddlActiveInactive.SelectedValue;

            //check if evaluation tech already exists
            bool chkExists = CheckEvalTech();
            if (chkExists == true)
            {
                ShowMessage("Data alreay exists!", MessageType.Info);
                return;
            }

            using (Crud_EvaluationTech crud = new Crud_EvaluationTech())
            {
                if (hdnMode.Value == "I")
                {
                    // check if data exists
                    List<Class_EvaluationTech> lst = crud.usp_EvaluationTechSelect().ToList();
                    bool descExists = lst.Any(x => x.evalTech.ToUpper().Trim() == txtEvalTech.Text.ToUpper().Trim());
                    if (descExists)
                    {
                        ShowMessage("Data already exists!!", MessageType.Info);
                        return;
                    }
                    else
                    {
                        crud.usp_EvaluationTechInsert(cls);
                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                    }
                }
                else if (hdnMode.Value == "E")
                {
                    List<Class_EvaluationTech> lst = crud.usp_EvaluationTechSelect()
                    .Where(x => x.evalTech.ToUpper().Trim() == txtEvalTech.Text.ToUpper().Trim() &&
                                x.evalTech_slno != Convert.ToInt32(hdnSlNo.Value)).ToList();
                    if (lst.Count > 0)
                    {
                        ShowMessage("Data already exists!!", MessageType.Info);
                        return;
                    }
                    else
                    {
                        cls.evalTech_slno = Convert.ToInt16(hdnSlNo.Value);
                        crud.usp_EvaluationTechUpdate(cls);
                        ShowMessage("Record Updated Successfully", MessageType.Success);
                    }

                }
            }
        }
    }

    /// <summary>
    /// deletes the business location data
    /// </summary>
    void DeleteData()
    {
        using (Class_EvaluationTech cls = new Class_EvaluationTech())
        {
            cls.evalTech_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_EvaluationTech crud = new Crud_EvaluationTech())
            {
                //Crud_ControlPlan_Child crudcp = new Crud_ControlPlan_Child();
                //List<Class_ControlPlan_Child> lst = crudcp.usp_ControlPlan_ChildSelect().Where(x => x.evalTech_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

                //if (lst.Count > 0)
                //{
                //    ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
                //}
                //else
                //{
                //    crud.usp_EvaluationTechDelete(cls);
                //    ShowMessage("Record Deleted Successfully", MessageType.Success);
                //}

                crud.usp_EvaluationTechDelete(cls);
                ShowMessage("Record Deleted Successfully", MessageType.Success);
            }
        }

    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        txtEvalTech.Text = string.Empty;
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
        ddlActiveInactive.SelectedIndex = 0;
    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>
    void LoadGridData()
    {
        Crud_EvaluationTech crud = new Crud_EvaluationTech();
        List<Class_EvaluationTech> lst = crud.usp_EvaluationTechSelect().ToList();
        //List<Class_EvaluationTech> lst = lst1.Where(x => x.del_status == "N").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_EvaluationTech crud = new Crud_EvaluationTech();
        List<Class_EvaluationTech> lst1 = crud.usp_EvaluationTechSelect().ToList();
        List<Class_EvaluationTech> lst = lst1.Where(x => x.evalTech_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtEvalTech.Text = lst[0].evalTech;
        ddlActiveInactive.SelectedValue = lst[0].status1.ToString();
        ddlActiveInactive.Enabled = true;
    }
}