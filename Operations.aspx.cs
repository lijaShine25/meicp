using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class Operations : System.Web.UI.Page
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
            if (Request.QueryString.HasKeys()  && Request.QueryString!=null ) 
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
        using (Class_operations cls = new Class_operations())
        {
            cls.OperationDesc = txtOperationDesc.Text;
            cls.del_status = ddlActiveInactive.SelectedValue;

            using (Crud_operations crud = new Crud_operations())
            {
                if (hdnMode.Value == "I")
                {
                    List<Class_operations> lst = crud.usp_operationsSelect().Where(x=> x.OperationDesc.ToUpper().Trim() == txtOperationDesc.Text.ToUpper().Trim()) .ToList();

                    if (lst.Count > 0)
                    {
                        ShowMessage("Record Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        crud.usp_operationsInsert(cls);
                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                    }
                }
                else if (hdnMode.Value == "E")
                {
                    // commented by SBK -- 18/07/2023
                    //List<Class_operations> lst = crud.usp_operationsSelect().Where(x => x.OperationDesc.ToUpper().Trim() == txtOperationDesc.Text.ToUpper().Trim()).ToList();
                    //List<Class_operations> lst1 = crud.usp_operationsSelect().Where(x => x.operation_slno == int.Parse(hdnSlNo.Value)).ToList();

                    //    if (lst.Count == 0)
                    //    {
                    //        cls.operation_slno = Convert.ToInt16(hdnSlNo.Value);
                    //        crud.usp_operationsUpdate(cls);
                    //        ShowMessage("Record Updated Successfully", MessageType.Success);                           
                    //    }
                    //    else
                    //    {
                    //        if (txtOperationDesc.Text.ToUpper() == lst1[0].OperationDesc.ToUpper().Trim())
                    //        {                              
                    //            ShowMessage("Record Updated Successfully", MessageType.Success);
                    //        }
                    //        else
                    //        {
                    //            ShowMessage("Record Already Exist..!", MessageType.Info);
                    //        }
                    //    }

                    List<Class_operations> lst = crud.usp_operationsSelect()
                        .Where(x => x.OperationDesc.ToUpper().Trim() == txtOperationDesc.Text.ToUpper().Trim() &&
                                        x.operation_slno!=Convert.ToInt32(hdnSlNo.Value))
                        .ToList();
                    if (lst.Count > 0)
                    {
                        ShowMessage("Record Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        cls.operation_slno = Convert.ToInt16(hdnSlNo.Value);
                        crud.usp_operationsUpdate(cls);
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
        using (Class_operations cls = new Class_operations())
        {
            cls.operation_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_operations crud = new Crud_operations())
            {
                //-------- Checks before deleting data
                //Crud_ControlPlan crudcp = new Crud_ControlPlan();
                //List<Class_ControlPlan> lst = crudcp.usp_ControlPlanSelect().Where(x => x.operation_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

                //if (lst.Count > 0)
                //{
                //    ShowMessage("Can not delete the data if Referred elsewhere.!", MessageType.Info);
                //}
                //else
                //{
                //    crud.usp_operationsDelete(cls);
                //    ShowMessage("Record Deleted Successfully", MessageType.Success);
                //}


                // ------------- delete data without checking
                crud.usp_operationsDelete(cls);
                ShowMessage("Record Deleted Successfully", MessageType.Success);
            }
        }      
 
    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        txtOperationDesc.Text = String.Empty;
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
        Crud_operations crud = new Crud_operations();
        List<Class_operations> lst = crud.usp_operationsSelect().ToList();
       // List<Class_operations> lst = lst1.Where(x => x.del_status == "N").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_operations crud = new Crud_operations();
        List<Class_operations> lst1 = crud.usp_operationsSelect().ToList();
        List<Class_operations> lst = lst1.Where(x => x.operation_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtOperationDesc.Text = lst[0].OperationDesc;
        ddlActiveInactive.SelectedValue = lst[0].status1.ToString();
        ddlActiveInactive.Enabled = true;
    } 
   
}