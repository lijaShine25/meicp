using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;

public partial class ControlMethods : System.Web.UI.Page
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
    
    /// <summary>
    /// saves the business locations data
    /// </summary>
    void SaveData()
    {
      
        using (Class_ControlMethods cls = new Class_ControlMethods())
        {
            cls.methodDesc = txtmethodDesc.Text.ToUpper().Trim();
            cls.del_status = ddlActiveInactive.SelectedValue;
           
            using (Crud_ControlMethods crud = new Crud_ControlMethods())
            {
                  if (hdnMode.Value == "I")
                    {
                        // check if data exists
                        List<Class_ControlMethods> lst = crud.usp_ControlMethodsSelect().ToList();
                        bool descExists = lst.Any(x => x.methodDesc.ToUpper().Trim() == txtmethodDesc.Text.ToUpper().Trim());

                        if (descExists)
                        {
                            ShowMessage("Data already exists!!", MessageType.Info);
                            return;
                        }

                        else
                        {
                        cls.methodDesc = txtmethodDesc.Text.Trim();
                           crud.usp_ControlMethodsInsert(cls);
                            ShowMessage("Record Inserted Successfully", MessageType.Success);
                        }
                    }
                    else if (hdnMode.Value == "E")
                    {
                      
                        cls.method_slno = Convert.ToInt16(hdnSlNo.Value);
                      cls.methodDesc = txtmethodDesc.Text.Trim();
                       crud.usp_ControlMethodsUpdate(cls);
                        ShowMessage("Record Updated Successfully", MessageType.Success);
                    }
                
            }
        }
    }

    /// <summary>
    /// deletes the business location data
    /// </summary>
    void DeleteData()
    {
        //using (Class_ControlMethods cls = new Class_ControlMethods())
        //{
        //    cls.method_slno = Convert.ToInt16(hdnSlNo.Value);
        //    using (Crud_ControlMethods crud = new Crud_ControlMethods())
        //    {

        //        Crud_ControlPlan_Child crudcp = new Crud_ControlPlan_Child();
        //        List<Class_ControlPlan_Child> lst = crudcp.usp_ControlPlan_ChildSelect().Where(x => x.method_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

        //        if (lst.Count > 0)
        //        {
        //            ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
        //        }
        //        else
        //        {
        //            crud.usp_ControlMethodsDelete(cls);
        //            ShowMessage("Record Deleted Successfully", MessageType.Success);
        //        }

                
        //    }
        //}


        using (Class_ControlMethods cls = new Class_ControlMethods())
        {
            cls.method_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_ControlMethods crud = new Crud_ControlMethods())
            {
                crud.usp_ControlMethodsDelete(cls);
            }
        }
        ShowMessage("Record Deleted Successfully", MessageType.Success);

    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        txtmethodDesc.Text = "";      
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
        Crud_ControlMethods crud = new Crud_ControlMethods();
        List<Class_ControlMethods> lst = crud.usp_ControlMethodsSelect().ToList();
        //List<Class_ControlMethods> lst = lst1.Where(x => x.del_status == "N").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_ControlMethods crud = new Crud_ControlMethods();
        List<Class_ControlMethods> lst1 = crud.usp_ControlMethodsSelect().ToList();
        List<Class_ControlMethods> lst = lst1.Where(x => x.method_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtmethodDesc.Text = lst[0].methodDesc;
        ddlActiveInactive.SelectedValue = lst[0].status1.ToString();  
        ddlActiveInactive.Enabled = true;

    }
}