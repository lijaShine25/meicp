using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using NPoco;

public partial class PartDocuments : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    //List<Class_machines> userInfo = null;
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo == null)
        {
            Response.Redirect("~/LogInPage.aspx");
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

            LoadParts();
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
                hdnMode.Value = "I";
                btnDelete.Enabled = false;
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }

    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst = crud.usp_partsSelect().Where(x => x.status1 == "N" && x.Obsolete=="N").ToList();
        ddlpart.Items.Clear();
        if (lst.Count > 0)
        {
            for (int cnt = 0; cnt < lst.Count; cnt++)
            {
                ddlpart.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
            }
            ddlpart.Items.Insert(0, "Select...");
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

    protected void ddlparts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ShowPartDetails();
        LoadGridData();
    }


    void SaveData()
    {
        Class_parts_documents cls = new Class_parts_documents();
        string filesavepath = Server.MapPath("~/Documents/");

        //uploadfile1
        string fn1 = string.Empty;
        if (!uploadfile1.HasFile && lbluploadfile1.Text.Length == 0)
        {
            cls.doc_filename = string.Empty;
            ShowMessage("No file selected for uploading!", MessageType.Info);
            return;
        }
        else if (uploadfile1.HasFile)
        {
            if(Path.GetExtension(uploadfile1.FileName.ToLower()) != ".pdf")
            {
                ShowMessage("Only PDF files can be uploaded!", MessageType.Info);
                return;
            }
            else
            {
                fn1 = Guid.NewGuid() + Path.GetExtension(uploadfile1.FileName);
                cls.doc_filename = fn1;
            }
            
        }
        else if (lbluploadfile1.Text.Length > 0)
        {
            cls.doc_filename = lbluploadfile1.Text;
            fn1 = lbluploadfile1.Text;
        }

        if (uploadfile1.HasFile)
        {
            uploadfile1.SaveAs(filesavepath + "/" + fn1);
        }

        
        cls.doc_title = txtdoctitle.Text;
        cls.part_slno = Convert.ToInt32(ddlpart.SelectedValue);
        cls.doc_filename = fn1;

        using (Crud_parts_documents crud = new Crud_parts_documents())
        {
            // check if title already exists
            List<Class_parts_documents>lstdoc = crud.SelectAll().
                Where(x=>x.part_slno.ToString()==ddlpart.SelectedValue
                && x.doc_title==txtdoctitle.Text)
                .ToList();
            if(hdnMode.Value == "I")
            {
                if (lstdoc.Count > 0)
                {
                    ShowMessage("Doc. title already exists!", MessageType.Info);
                    return;
                }
                else
                {
                    crud.Insert(cls);
                }
            }
            else if(hdnMode.Value == "E")
            {
                cls.part_doc_slno = Convert.ToInt32(hdnSlNo.Value);
                crud.Update(cls);
            }
            ShowMessage("Data saved successfully!", MessageType.Success);
        }
    }


    void DeleteData()
    {

        using(Crud_parts_documents crud = new Crud_parts_documents()) 
        {
            Class_parts_documents cls = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
            // delete the data
            crud.Delete(Convert.ToInt32(hdnSlNo.Value));
            // delete the physical file
            string fn = cls.doc_filename;
            FileInfo f = new FileInfo(Server.MapPath("~/Documents/" + fn));
            if (f.Exists)
            {
                f.Delete();
                lbluploadfile1.Text = "";
            }
            ShowMessage("Data deleted successfully!", MessageType.Success);
        }
    }

    void ClearData()
    {
        txtdoctitle.Text = string.Empty;
        ddlpart.SelectedIndex = 0;
        lbluploadfile1.Text = string.Empty;
        uploadfile1.Dispose();
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
    }

    void LoadGridData()
    {
        string sql = @"select pd.part_doc_slno, pd.part_doc_slno,pd.doc_title, pd.doc_filename,
p.mstPartNo, p.PartDescription, p.partIssueNo, p.partIssueDt
from parts_documents pd
inner join parts p on p.part_slno = pd.part_slno";

        if(ddlpart.SelectedIndex > 0)
        {
            sql += " where p.part_slno=" + ddlpart.SelectedValue;
        }

       
        using (NPoco.Database db = new NPoco.Database("connString"))
        {
            List<Class_parts_documents> lst = db.Query<Class_parts_documents>(sql).ToList();
            if (lst.Count > 0)
            {
                grdData.DataSource = lst;
                grdData.DataBind();
                grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                grdData.DataSource = new string[] { };
                grdData.DataBind();
            }
        }
    }

    void GetDetails()
    {
        using(Crud_parts_documents crud = new Crud_parts_documents())
        {
            Class_parts_documents cls = crud.SelectOne(Convert.ToInt32(hdnSlNo.Value));
            if(cls != null)
            {
                ddlpart.SelectedValue = cls.part_slno.ToString();
                txtdoctitle.Text = cls.doc_title;
                lbluploadfile1.Text = cls.doc_filename;
                ShowPartDetails();
            }
        }
    }

    protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltl = (Literal)e.Row.FindControl("ltlfilename");
            Label lblfn1 = (Label)e.Row.FindControl("lblfname1");

            ltl.Text = string.Empty;

            if (!string.IsNullOrEmpty(lblfn1.Text))
            {
                // Generate the URL for the file handler
                string fileHandlerUrl = "FileHandler.ashx?fileName=" + HttpUtility.UrlEncode(lblfn1.Text);

                // Generate the hyperlink HTML
                ltl.Text += "<a href='" + fileHandlerUrl + "' target='_blank'>" + lblfn1.Text + "</a>";
            }
        }
    }



    void ShowPartDetails()
    {
        using (Crud_parts crud = new Crud_parts())
        {
            Class_parts cls = crud.SelectOne(Convert.ToInt32(ddlpart.SelectedValue));
            if(cls != null)
            {
                lblpartissdt.Text = cls.partIssueDt.ToString();
                lblpartissno.Text = cls.partIssueNo;
                lblcustname.Text = cls.Customer_name;
                lblcustpartno.Text = cls.customerPartNo;
                lblpartno.Text = cls.mstPartNo;
            }
        }
    }

    protected void btnclearFilter_Click(object sender, EventArgs e)
    {
        ddlpart.SelectedIndex = 0;
        LoadGridData();
    }
}