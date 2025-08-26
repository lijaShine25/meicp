using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;


public partial class Machines : System.Web.UI.Page
{
    
    public enum MessageType { Success, Error, Info, Warning };
    //List<Class_machines> userInfo = null;
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

            LoadOperations();
            LoadGridData();

            if (Request.QueryString.HasKeys() && Request.QueryString != null)
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E"; btnSubmit.Enabled = false;
                GetDetails();
                btnDelete.Enabled = true;
            }
            else
            {
                hdnMode.Value = "I";
                btnDelete.Enabled = false;
            }
        }       
     }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }
    
    void LoadOperations()
    {
        Crud_operations crud = new Crud_operations();
        List<Class_operations> lst1 = crud.usp_operationsSelect().ToList();
        List<Class_operations> lst = lst1.Where(x => x.del_status == "ACTIVE").ToList();
        ddloperation_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddloperation_slno.Items.Add(new ListItem(lst[cnt].OperationDesc, Convert.ToString(lst[cnt].operation_slno)));
        }
        ddloperation_slno.Items.Insert(0, "Select...");
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

       
    void SaveData()
    {
        using (Class_machines cls = new Class_machines())
        {
            cls.MachineCode = txtMachineCode.Text ;
            cls.MachineDesc = txtMachineDesc.Text;      
            cls.del_status = ddlActiveInactive.SelectedValue;
            cls.operation_slno = int.Parse(ddloperation_slno.SelectedValue);
                        
            using (Crud_machines crud = new Crud_machines())
            {
                List<Class_machines> lst = crud.usp_machinesSelect().Where(x => x.MachineCode.ToUpper().Trim() == txtMachineCode.Text.ToUpper().Trim() && x.MachineDesc.ToUpper().Trim() == txtMachineDesc.Text.ToUpper().Trim() && x.operation_slno == int.Parse(ddloperation_slno.SelectedValue)).ToList();
                if (hdnMode.Value == "I")
                {
                    if (lst.Count > 0)
                    {
                        ShowMessage("Record Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        crud.usp_machinesInsert(cls);
                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                    }
                }
                else if (hdnMode.Value == "E")
                {
                    List<Class_machines> lst2 = crud.usp_machinesSelect().Where(x => x.machine_slno == int.Parse(hdnSlNo.Value)).ToList();

                    cls.machine_slno = Convert.ToInt16(hdnSlNo.Value);
                    crud.usp_machinesUpdate(cls);
                    ShowMessage("Record Updated Successfully", MessageType.Success);
                 
                }
            }
        }
    }

   
    void DeleteData()
    {
        using (Class_machines cls = new Class_machines())
        {
            cls.machine_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_machines crud = new Crud_machines())
            {
                //Crud_ControlPlan crudcp = new Crud_ControlPlan();
                //List<Class_ControlPlan> lst = crudcp.usp_ControlPlanSelect().Where(x => x.machine_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

                //if (lst.Count > 0)
                //{
                //    ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
                //}
                //else
                //{
                //    crud.usp_machinesDelete(cls);
                //    ShowMessage("Record Deleted Successfully", MessageType.Success);
                //}

                crud.usp_machinesDelete(cls);
                ShowMessage("Record Deleted Successfully", MessageType.Success);

            }
        }      
    }  
       
    void ClearData()
    {
        txtMachineCode.Text = string.Empty;
        txtMachineDesc.Text = string.Empty;
        ddloperation_slno.SelectedIndex = 0;       
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        ddlActiveInactive.SelectedIndex = 0;
        btnDelete.Enabled = false;
    }
   
    void LoadGridData()
    {
        Crud_machines crud = new Crud_machines();
        List<Class_machines> lst = crud.usp_machinesSelect().ToList();
        //List<Class_machines> lst = lst1.Where(x => x.del_status == "ACTIVE").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    
    void GetDetails()
    {
        Crud_machines crud = new Crud_machines();
        List<Class_machines> lst1 = crud.usp_machinesSelect().ToList();
        List<Class_machines> lst = lst1.Where(x => x.machine_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtMachineCode.Text = lst[0].MachineCode;
        txtMachineDesc.Text = lst[0].MachineDesc;       
        ddloperation_slno.SelectedValue = lst[0].operation_slno.ToString();
        ddlActiveInactive.SelectedValue = lst[0].status1;
        ddlActiveInactive.Enabled = true;
 
    }
}