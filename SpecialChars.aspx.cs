using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;
using System.IO;
using System.Text;

using NPoco;
using System.Net;

public partial class SpecialChars : System.Web.UI.Page
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
            LoadCustomers();
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

    void LoadCustomers()
    {
        Crud_customers crud = new Crud_customers();
        List<Class_customers> lst = null;

        if (hdnMode.Value == "I")
        {
            lst = crud.SelectAll().Where(x => x.del_status == false).ToList();
        }
        else if (hdnMode.Value == "E")
        {
            lst = crud.SelectAll().ToList();
        }
        ddlcustomername.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlcustomername.Items.Add(new ListItem(lst[cnt].cust_name, lst[cnt].cust_slno.ToString()));
        }
        ddlcustomername.Items.Insert(0, "Select...");
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
    /// 


    void SaveData()
    {
        try
        {
            // check if legend cound = 4
            //int cnt = CheckLegendCount();
            //if (cnt >= 4)
            //{
            //    ShowMessage("Only 4 legends per customer can be added!", MessageType.Info);
            //    return;
            //}

            //if (splCharFile.HasFile)
            {
                using (Class_SpecialChars cls = new Class_SpecialChars())
                {
                    string filesavepath = Server.MapPath("~/Documents/");
                    //uploadfile1
                    string fn1 = string.Empty;
                    if (!splCharFile.HasFile && lblsplCharFile.Text.Length == 0)
                    {
                        cls.splCharFile = string.Empty;
                    }
                    else if (splCharFile.HasFile)
                    {
                        fn1 = splCharFile.FileName;
                        cls.splCharFile = fn1;
                    }
                    else if (lblsplCharFile.Text.Length > 0)
                    {
                        cls.splCharFile = lblsplCharFile.Text;
                    }

                    if (splCharFile.HasFile)
                    {
                        splCharFile.SaveAs(filesavepath + "/" + fn1);
                    }

                    cls.del_status = ddlActiveInactive.SelectedValue;
                    cls.spl_char_desc = txtdesc.Text;
                    cls.cust_slno = Convert.ToInt32(ddlcustomername.SelectedValue);
                    cls.show_in_legend = ddlInlegend.SelectedValue == "Y" ? true : false;
                    int fileValue = 0;

                    string qry = "select * from SpecialChars where splCharFile='" + fn1 + "'";
                                      

                    using (Database db = new Database("connString"))
                    {
                        fileValue = db.ExecuteScalar<int>(qry);
                    }

                    if (fileValue == 0)
                    {
                        using (Crud_SpecialChars crud = new Crud_SpecialChars())
                        {
                            if (hdnMode.Value == "I")
                            {
                                crud.usp_SpecialCharsInsert(cls);
                                ShowMessage("Uploaded Successfully", MessageType.Success);
                            }
                            else if (hdnMode.Value == "E")
                            {
                                cls.splChar_slno = Convert.ToInt16(hdnSlNo.Value);
                                crud.usp_SpecialCharsUpdate(cls);
                                ShowMessage("Record Updated Successfully", MessageType.Success);
                                ClearData();
                            }
                        }
                    }
                    else
                    {
                        ShowMessage("File Name Exist Already.!!", MessageType.Info); 
                    }
                }
            }
            //else
            //{
            //    ShowMessage("File Not Selected.!!", MessageType.Info);
            //}

        }

        catch (Exception ex)
        {
            ShowMessage("Upload status: The file could not be uploaded. The following error occured: " + ex.Message, MessageType.Info);
        }

    }

    /// <summary>
    /// deletes the business location data
    /// </summary>
    void DeleteData()
    {
        using (Class_SpecialChars clsspl = new Class_SpecialChars())
        {
            using (Crud_SpecialChars crud = new Crud_SpecialChars())
            {
                clsspl.splChar_slno = Convert.ToInt16(hdnSlNo.Value);

                Crud_ControlPlan_Child crudcp = new Crud_ControlPlan_Child();
                List<Class_ControlPlan_Child> lst = crudcp.usp_ControlPlan_ChildSelect().Where(x => x.splChar_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

                if (lst.Count > 0)
                {
                    ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
                }
                else
                {
                    crud.usp_SpecialCharsDelete(clsspl);
                    ShowMessage("Record Deleted Successfully", MessageType.Success);
                }

                //crud.usp_SpecialCharsDelete(clsspl);
                //ShowMessage("Record Deleted Successfully", MessageType.Success);

            }
        }
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
        lblsplCharFile.Text = "";
        ddlActiveInactive.SelectedIndex = 0;
        txtdesc.Text = string.Empty;
        ddlcustomername.SelectedIndex = 0;
        ddlInlegend.SelectedIndex = 1;
    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>
    void LoadGridData()
    {
        Crud_SpecialChars crud = new Crud_SpecialChars();
        List<Class_SpecialChars> lst = crud.usp_SpecialCharsSelect().ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    //public static string  Right(this string value, int length)
    // {
    //     string val= value.Substring(value.Length - length).ToString();
    //     return val;
    // }
    void GetDetails()
    {
        Crud_SpecialChars crud = new Crud_SpecialChars();
        List<Class_SpecialChars> lst = crud.usp_SpecialCharsSelect().ToList();
        List<Class_SpecialChars> lst1 = lst.Where(x => x.splChar_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

        lblsplCharFile.Text = lst1[0].splCharFile;
        hrefrcFile1.HRef = "~/Documents/" + lst1[0].splCharFile;

        ddlActiveInactive.SelectedValue = lst1[0].status1.ToString();
        ddlActiveInactive.Enabled = true;
        txtdesc.Text = lst1[0].spl_char_desc;
        LoadCustomers();
        ddlcustomername.SelectedValue = lst1[0].cust_slno.ToString();
        ddlInlegend.SelectedValue = lst1[0].show_in_legend ? "Y" : "N";

        

    }

    protected void linkBtnsplCharFile_Click(object sender, System.EventArgs e)
    {
        Crud_SpecialChars crud = new Crud_SpecialChars();
        List<Class_SpecialChars> lst = crud.usp_SpecialCharsSelect().ToList();
        List<Class_SpecialChars> lst1 = lst.Where(x => x.splChar_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();

        string fullPath = lst1[0].splCharFile;
        string path3 = fullPath.Substring(fullPath.Length - 3);
        string path4 = fullPath.Substring(fullPath.Length - 4);
        // string pat = Server.MapPath("~/Documents/") + fullPath;

        if (path3 == "DOC" | path4 == "DOCX")
        {
            string filePath = lblsplCharFile.Text;
            string[] parts = lblsplCharFile.Text.Split(new char[] { '/' });
            string part = null;
            string filename = null;
            foreach (string part_loopVariable in parts)
            {
                part = part_loopVariable;
                filename = "";
                filename = part;
            }

            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            Response.TransmitFile(Server.MapPath("~/Documents/") + filePath);
            Response.End();
        }

        else if (path3 == "XLS" | path4 == "XLSX")
        {
            string filePath = lblsplCharFile.Text;
            string[] parts = lblsplCharFile.Text.Split(new char[] { '/' });
            string part = null;
            string filename = null;
            foreach (string part_loopVariable in parts)
            {
                part = part_loopVariable;
                filename = "";
                filename = part;
            }
            filePath = Server.MapPath("Documents/" + filePath);
            FileInfo file = new FileInfo(filePath);

            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + lblsplCharFile.Text);
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.xls";
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.WriteFile(file.FullName);
                Response.End();
            }
        }
        else
        {
            string filepath = fullPath;
            string url = "" + filepath + "',null,'height=500,width=772,status=yes,toolbar=no,menubar=no,location=no,scrollbar=yes";
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.open('");
            sb.Append(url);
            sb.Append("');");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());
        }
    }

    Boolean CheckValidImageFile()
    {
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
        string ext = System.IO.Path.GetExtension(splCharFile.PostedFile.FileName);
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile)
        {
            ShowMessage("Not a valid Image File", MessageType.Info);
            isValidFile = false;
        }
        return isValidFile;
    }

    int CheckLegendCount()
    {
        string sql = "select count(*) as cnt from specialchars where cust_slno=@0 and show_in_legend=@1";
        using (Database db = new Database("connString"))
        {
            int cnt = db.ExecuteScalar<int>(sql, ddlcustomername.SelectedValue, true);
            return cnt;
        }
    }

}


