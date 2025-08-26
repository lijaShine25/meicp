using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;

public partial class PageTemplate1 : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //if (userInfo[0].is_ET_User != "Y")
            //{
            //    Response.Redirect("~/AccessDenied.aspx");
            //}
            if (Request.QueryString.HasKeys())
            {
                //hdnSlNo.Value = Request.QueryString["slno"];
                //hdnMode.Value = "E";
                //GetDetails();
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
        LoadGridData ();
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
        //using (Class_business_units clsBu = new Class_business_units())
        //{
        //    clsBu.bu_name = txtUnitName.Text;
        //    clsBu.bl_slno = Convert.ToInt16(ddlLocationName.SelectedValue);
        //    clsBu.del_status = "N";

        //    using (Crud_BusinessUnits crudBu = new Crud_BusinessUnits())
        //    {
        //        if (hdnMode.Value == "I")
        //        {
        //            crudBu.Insert_BusinessUnits(clsBu);
        //            ShowMessage("Record Inserted Successfully", MessageType.Success);
        //        }
        //        else if (hdnMode.Value == "E")
        //        {
        //            clsBu.bu_slno = Convert.ToInt16(hdnSlNo.Value);
        //            crudBu.Update_BusinessUnits(clsBu);
        //            ShowMessage("Record Updated Successfully", MessageType.Success);
        //        }
        //    }
        //}
    }

    /// <summary>
    /// deletes the business location data
    /// </summary>
    void DeleteData()
    {
        //using (Class_business_units clsBu = new Class_business_units())
        //{
        //    clsBu.bu_slno = Convert.ToInt16(hdnSlNo.Value);
        //    using (Crud_BusinessUnits crudBu = new Crud_BusinessUnits())
        //    {
        //        crudBu.Delete_BusinessUnits(clsBu);
        //    }
        //}
        //ShowMessage("Record Deleted Successfully", MessageType.Success);
    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        //ddlLocationName.SelectedIndex = 0;
        //txtUnitName.Text = "";
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>
    void LoadGridData()
    {
     
        //Crud_BusinessUnits bu = new Crud_BusinessUnits();
        //List<Class_business_units> buList = JsonConvert.DeserializeObject<List<Class_business_units>>(bu.Select_BusinessUnits(buSlno: -1, delStatus: "N", blSlNo: blSlNo));
        //grdData.DataSource = buList;
        //grdData.DataBind();
        //grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
}