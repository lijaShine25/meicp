using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

public partial class Parts_Reports : System.Web.UI.Page
{

    public enum MessageType { Success, Error, Info, Warning };

    protected void Page_Load(object sender, EventArgs e)
    {
        Crud_parts crud = new Crud_parts();
        //List<Class_parts> lst1 = crud.usp_partsSelect().ToList();    

        List<Class_parts> lst = crud.usp_partsSelect().ToList().Where(x => x.Obsolete == "N").ToList();
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
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
        Response.AddHeader("content-disposition", "attachment;filename=PartMasterReport.xls");
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

}