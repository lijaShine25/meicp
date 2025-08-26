using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;


public partial class SampleFreq : System.Web.UI.Page
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

            LoadGridData();
            if (Request.QueryString.HasKeys() && Request.QueryString != null)
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E";
                btnSubmit.Enabled = false;
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
        using (Class_SampleFrequency cls = new Class_SampleFrequency())
        {
            cls.FreqDesc = txtFreqDesc.Text.ToUpper().Trim();
            using (Crud_SampleFrequency crud = new Crud_SampleFrequency())
            {
                if (hdnMode.Value == "I")
                {
                    // check if data exists
                    List<Class_SampleFrequency> lst = crud.usp_SampleFrequencySelect().ToList();
                    bool descExists = lst.Any(x => x.FreqDesc.ToUpper().Trim() == txtFreqDesc.Text.ToUpper().Trim());
                    if (descExists)
                    {
                        ShowMessage("Data already exists!!", MessageType.Info);
                        return;
                    }

                    else
                    {
                        cls.del_status = "N";
                        //cls.foi = chkfoi.Checked;
                        //cls.pcc = chkpcc.Checked;
                        //cls.pmc = chkpmc.Checked;
                        //cls.material_test_report = chkmtl.Checked;
                        //cls.sample_size = txtsamplsize.Text.ToUpper();
                        //cls.packing = chkpacking.Checked;
                        //cls.dockaudit = chkdock.Checked;
                        cls.FreqDesc = txtFreqDesc.Text;
                        crud.usp_SampleFrequencyInsert(cls);
                        ShowMessage("Record Inserted Successfully", MessageType.Success);
                    }
                }
                else if (hdnMode.Value == "E")
                {
                    cls.del_status = ddlActiveInactive.SelectedValue;
                    cls.freq_slno = Convert.ToInt16(hdnSlNo.Value);
                    //cls.foi = chkfoi.Checked;
                    //cls.pcc = chkpcc.Checked;
                    //cls.pmc = chkpmc.Checked;
                    //cls.material_test_report = chkmtl.Checked;
                    //cls.sample_size = txtsamplsize.Text.ToUpper();
                    //cls.packing = chkpacking.Checked;
                    //cls.dockaudit = chkdock.Checked;
                    cls.FreqDesc = txtFreqDesc.Text.Trim();
                    crud.usp_SampleFrequencyUpdate(cls);
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
        using (Class_SampleFrequency cls = new Class_SampleFrequency())
        {
            cls.freq_slno = Convert.ToInt16(hdnSlNo.Value);
            using (Crud_SampleFrequency crud = new Crud_SampleFrequency())
            {
                //Crud_ControlPlan_Child crudcp = new Crud_ControlPlan_Child(); 
                //List<Class_ControlPlan_Child> lst = crudcp.usp_ControlPlan_ChildSelect().Where(x => x.freq_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

                //if (lst.Count > 0)
                //{
                //    ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
                //}
                //else
                //{
                //    crud.usp_SampleFrequencyDelete(cls);
                //    ShowMessage("Record Deleted Successfully", MessageType.Success);
                //}

                crud.usp_SampleFrequencyDelete(cls);
                ShowMessage("Record Deleted Successfully", MessageType.Success);
            }
        }
    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        txtFreqDesc.Text = "";
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
        //chkfoi.Checked = false;
        //chkpmc.Checked = false;
        //chkpcc.Checked = false;
        //chkmtl.Checked = false;
        //txtsamplsize.Text = string.Empty;
        //chkpacking.Checked = false;
        //chkdock.Checked = false;
    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>
    void LoadGridData()
    {
        Crud_SampleFrequency crud = new Crud_SampleFrequency();
        List<Class_SampleFrequency> lst = crud.usp_SampleFrequencySelect().ToList();
        //List<Class_SampleFrequency> lst = lst1.Where(x => x.del_status == "N").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    void GetDetails()
    {
        Crud_SampleFrequency crud = new Crud_SampleFrequency();
        List<Class_SampleFrequency> lst1 = crud.usp_SampleFrequencySelect().ToList();
        List<Class_SampleFrequency> lst = lst1.Where(x => x.freq_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        txtFreqDesc.Text = lst[0].FreqDesc;
        ddlActiveInactive.SelectedValue = lst[0].status1.ToString();
        ddlActiveInactive.Enabled = true;
       // chkfoi.Checked = lst[0].foi_txt == "Y" ? true : false;
        //chkpmc.Checked = lst[0].pmc_txt == "Y" ? true : false;
       // chkpcc.Checked = lst[0].pcc_txt == "Y" ? true : false;
       // chkmtl.Checked = lst[0].mtl_txt == "Y" ? true : false;
       // chkdock.Checked = lst[0].docaudit_txt=="Y"?true : false;
       // txtsamplsize.Text = lst[0].sample_size;
       // chkpacking.Checked = lst[0].packing_txt == "Y" ? true : false;
    }
}