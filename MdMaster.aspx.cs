using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;


public partial class MdMaster : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    //List<Class_mdMaster> userInfo = null;
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
        using (Class_mdMaster cls = new Class_mdMaster())
        {
            cls.equip_number = txtequipno.Text;
            cls.md_category = txtmdcat.Text;
            
            using (Crud_mdMaster crud = new Crud_mdMaster())
            {
                List<Class_mdMaster> lst = crud.SelectAll()
                    .Where(x => x.equip_number.ToUpper().Trim() == txtequipno.Text.ToUpper().Trim()).ToList();
                if (hdnMode.Value == "I")
                {
                    if (lst.Count > 0)
                    {
                        ShowMessage("Record Already Exist..!", MessageType.Info);
                    }
                    else
                    {
                        crud.Insert(cls);
                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                    }
                }
                else if (hdnMode.Value == "E")
                {
                    cls.md_slno = Convert.ToInt16(hdnSlNo.Value);
                    crud.Update(cls);
                    ShowMessage("Record Updated Successfully", MessageType.Success);
                }
            }
        }
    }


    void DeleteData()
    {
        using (Class_mdMaster cls = new Class_mdMaster())
        {
            using (Crud_mdMaster crud = new Crud_mdMaster())
            {
                crud.Delete(Convert.ToInt32(hdnSlNo.Value));
                ShowMessage("Record Deleted Successfully", MessageType.Success);
            }
        }
    }

    void ClearData()
    {
        txtequipno.Text = string.Empty;
        txtmdcat.Text = string.Empty;
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
    }

    void LoadGridData()
    {
        Crud_mdMaster crud = new Crud_mdMaster();
        List<Class_mdMaster> lst = crud.SelectAll().ToList();
        //List<Class_mdMaster> lst = lst1.Where(x => x.del_status == "ACTIVE").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_mdMaster crud = new Crud_mdMaster();
        Class_mdMaster c= crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));

        txtequipno.Text = c.equip_number;
        txtmdcat.Text = c.md_category;
    }
}