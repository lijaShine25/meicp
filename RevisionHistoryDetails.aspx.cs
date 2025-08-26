using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using NPoco;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

public partial class RevisionHistoryDetails : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };

    void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadParts();
            LoadGridData();
            if (Request.QueryString.HasKeys())
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E";
                //GetDetails();              
                //btnDelete.Enabled = true;
            }
            else
            {
                //btnDelete.Enabled = false;
            }
        }

    }

    void LoadGridData()
    {
        string cond = string.Empty;
        if (ddlpart_slno.SelectedIndex > 0)
        {
            if (cond.Length > 0)
            {
                cond += " and p.part_slno=" + ddlpart_slno.SelectedValue;
            }
            else
            {
                cond += " p.part_slno=" + ddlpart_slno.SelectedValue;
            }
        }


        string qry = @"select p.mstPartNo, p.PartDescription, r.rev_no, r.rev_date, r.rev_reasons, r.change_nature
from part_revision_history r
inner join parts p on p.part_slno=r.part_slno
where 1=1";

        if (cond.Length > 0)
        {
            qry += " AND " + cond;
        }
        using (Database db = new Database("connString"))
        {
            List<Class_part_revision_history> x = db.Query<Class_part_revision_history>(qry).ToList();

            if (x.Count > 0)
            {
                grdData.DataSource = x;
                grdData.DataBind();
            }
            else
            {
                grdData.DataSource = null;
                grdData.DataBind();
            }
        }

    }

    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        List<Class_parts> lst = lst1.Where(x => x.Obsolete == "N" && x.del_status=="ACTIVE").ToList();
        ddlpart_slno.Items.Clear();
        if (lst.Count > 0)
        {
            for (int cnt = 0; cnt < lst.Count; cnt++)
            {
                ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
            }

            ddlpart_slno.Items.Insert(0, "Select...");
        }
    }

    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }

    protected void btnExportXL_Click(object sender, EventArgs e)
    {
        ExcelExport();
    }

    void ExcelExport()
    {
        if (grdData.Rows.Count == 0)
        {
            ShowMessage("No Record to Export !!", MessageType.Warning);
            return;
        }
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=RevisionHistoryReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            ////To Export all pages
            //grdData.AllowPaging = false;
            //this.grdData();

            grdData.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in grdData.HeaderRow.Cells)
            {
                cell.BackColor = grdData.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grdData.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grdData.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grdData.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grdData.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    protected void grdData_PreRender(object sender, EventArgs e)
    {
        if (grdData.Rows.Count > 0)
        {
            grdData.UseAccessibleHeader = true;
            grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
            //  grdData.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // base.VerifyRenderingInServerForm(control);
    }

    protected void btnViewRptHistory_Click(object sender, EventArgs e)
    {
        LoadGridData();

    }
}