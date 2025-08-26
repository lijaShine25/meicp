using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NPoco;
using Elmah;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Drawing;
using System.Drawing;


public partial class RptControlPlan_XLS : System.Web.UI.Page
{
    int lastRow;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                hdnSaveAs.Value = Request.QueryString["mode"];
            }
            PrepareReport();
            //  ExportFile();
        }
    }

    void PrepareReport()
    {
        //Crud_NCD_NCDetails crud = new Crud_NCD_NCDetails();
        //List<Class_Temp_RptControlPlan> lst = JsonConvert.DeserializeObject<List<Class_Temp_RptControlPlan>>(crud.Select_NCDetails(ncdSlNo: Convert.ToInt16(sl)));

        using (Database db = new Database("connString"))
        {
            List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>("Select * from temp_rptControlPlan").ToList();

            if (lst.Count > 0)
            {
                // fpath,templatefile dbt
                string PartIssuedate; 
                string CPIDt;
                string OrgDt;
                string RevDt;

                string fname = "mei_" + lst[0].cp_slno + ".xlsx";
                string fpath = "~/Documents/";
                FileInfo excelFile = new FileInfo(Server.MapPath(fpath + fname));
                FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/ControlPlanTemplate.xlsx"));
                if (Request.QueryString["changeFile"] == "Y")
                {
                    templateFile = new FileInfo(Server.MapPath("~/App_Data/FI_ControlPlanTemplate.xlsx"));
                }


                using (ExcelPackage pck = new ExcelPackage(excelFile, templateFile))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets[1];                                         
          
                        if (lst[0].cpType == "PROTOTYPE") 
                        {
                            ws.Cells["C2"].Value = " ";
                            ws.Cells["E2"].Value = " ";
                        }
                        else if (lst[0].cpType == "PRE-LAUNCH") 
                        {
                            ws.Cells["A2"].Value = " ";
                            ws.Cells["E2"].Value = " ";
                        }
                        else if (lst[0].cpType == "PRODUCTION")
                        {
                            ws.Cells["A2"].Value = " ";
                            ws.Cells["C2"].Value = " ";
                        }
                    PartIssuedate = lst[0].partIssueDt;
                    CPIDt = lst[0].customerIssueDt;
                    OrgDt = lst[0].originalDt;
                    RevDt = lst[0].rev_date;                
                 
                    ws.Cells["A4"].LoadFromText(lst[0].mstPartNo);
                    ws.Cells["D4"].LoadFromText(lst[0].partIssueNo);
                    //PartIssuedate
                    ws.Cells["E4"].Value = PartIssuedate;                    
                    ws.Cells["H4"].LoadFromText((lst[0].keyContact) + " /" + (lst[0].keyContactPhone));
                    ws.Cells["M4"].LoadFromText((lst[0].organization) + " " + (lst[0].orgApprovalDt) + " " + (lst[0].orgDate));
                    //OrgDt
                    ws.Cells["R4"].Value = OrgDt;
                    ws.Cells["S4"].LoadFromText(lst[0].rev_no.ToString());
                    //RevDt
                    ws.Cells["T4"].Value = RevDt;

                    ws.Cells["A6"].LoadFromText(lst[0].customerPartNo);
                    ws.Cells["D6"].LoadFromText(lst[0].customerIssueNo);
                    //CPIDt
                    ws.Cells["E6"].Value = CPIDt;

                    ws.Cells["H6"].LoadFromText( lst[0].CFTeamName);

                    Crud_CFTeamEmployees crd = new Crud_CFTeamEmployees();
                   // txtCftMembers.Text = crd.GetMembersList(lst[0].CftTeamSlNo);
                    ws.Cells["H6"].Value = crd.GetMembersList(Convert.ToInt16( lst[0].CftTeamSlNo));

                    ws.Cells["M6"].LoadFromText((lst[0].custApproval) + " " + (lst[0].custApprovalDt));

                    ws.Cells["C7"].LoadFromText(lst[0].organization);
                    ws.Cells["C8"].LoadFromText(lst[0].Customer_name);
                    
                    ws.Cells["H8"].LoadFromText(lst[0].PartDescription);
                    ws.Cells["M8"].LoadFromText((lst[0].custQaApproval) + " " + (lst[0].custQaApprovalDt));

                    ws.Cells["A10"].LoadFromText(Convert.ToString(lst[0].process_no));
                    ws.Cells["C10"].LoadFromText(lst[0].OperationDesc);
                    ws.Cells["H10"].LoadFromText(lst[0].MachineDesc);

                    ws.Cells["M10"].LoadFromText((lst[0].otherApproval) + " " + (lst[0].otherApprovalDt));

                    ExcelWorksheet ws2 = pck.Workbook.Worksheets[2];

                    int row = 17;
                    int startRow = 16;

                    for (int i = 0; i < lst.Count; i++)
                    {
                        // copy the first row below and start filling the values
                        ExcelRange rng1 = ws.Cells[startRow + ":" + startRow];
                        ExcelRange rng2 = ws.Cells[row + ":" + row];
                        rng1.Copy(rng2);

                        ws.Row(row).StyleID = ws2.Row(startRow).StyleID;

                        ws.Row(row).Height = 45.75;

                        ws.Cells[row, 1].LoadFromText(lst[i].dimn_no);
                        ws.Cells[row, 2].LoadFromText(lst[i].product_char);
                        ws.Cells[row, 3].LoadFromText(lst[i].process_char);
                        //ws.Cells[row, 5].LoadFromText(lst[i].splChar_slno.ToString());

                        //-- image to be inserted

                        if (lst[i].splChar_slno != null)
                        {
                            string imgName = GetImageName(lst[i].splChar_slno.ToString());
                            if (imgName != null)
                            {
                                if (imgName.Length > 0)
                                {
                                    string imagePath = Server.MapPath("~/Documents/" + imgName);
                                    FileInfo fl = new FileInfo(imagePath);
                                    if (fl.Exists)
                                    {
                                        Bitmap image = new Bitmap(imagePath);
                                        ExcelPicture excelImage = null;

                                        if (image != null)
                                        {
                                            excelImage = ws.Drawings.AddPicture(lst[i].splChar_slno.ToString()+row, image);
                                            excelImage.From.Column = 4;
                                            excelImage.From.Row = row-3;
                                            excelImage.SetSize(40, 40);
                                            // 2x2 px space for better alignment
                                            excelImage.From.ColumnOff = Pixel2MTU(20);
                                            excelImage.From.RowOff = Pixel2MTU(20);
                                        }
                                    }
                                }
                            }
                        }

                                       
                       // ws.Cells[row, 6].LoadFromText(lst[i].tol_min);
                        ws.Cells[row, 6].RichText.Add(lst[i].tol_min);
                       // ws.Cells[row, 7].LoadFromText(lst[i].tol_max);
                        ws.Cells[row, 7].RichText.Add(lst[i].tol_max);
                       
                        // merge cells if max is blank
                        if (lst[i].tol_max == null || lst[i].tol_max.Length == 0)
                        {
                            ws.Cells["G" + row + ":H" + row].Merge = false;
                            ws.Cells["F" + row + ":H" + row].Merge = true;
                        }
                        ws.Cells[row, 9].LoadFromText(lst[i].measurementTech);
                        ws.Cells[row, 11].LoadFromText(lst[i].gaugeCode);
                        ws.Cells[row, 12].LoadFromText(lst[i].sampleSize);
                        ws.Cells[row, 13].LoadFromText(lst[i].sampleFreq);
                        ws.Cells[row, 15].LoadFromText(lst[i].res);
                        ws.Cells[row, 16].LoadFromText(lst[i].methodDesc);
                        ws.Cells[row, 18].LoadFromText(lst[i].reactionPlan);

                        row++;
                    }

                    lastRow = row;

                    // delete the initial blank row and the lastrow
                    ws.DeleteRow(startRow);
                    ws.DeleteRow(lastRow);
                    ws.DeleteRow(startRow-1);

                    // copy the footer from sheet 2
                    ExcelRange colRng1 = ws2.Cells["17:17"];

                    ExcelRange colRng2 = ws.Cells[row + ":" + row];
                    colRng1.Copy(colRng2);

                    ws.Cells[lastRow, 3].Value = (lst[0].preparedBy);
                    ws.Cells[lastRow, 18].Value = (lst[0].approvedBy);

                    ws.Row(lastRow).Height = 45.75;

                    hdnSourceFile.Value = Server.MapPath(fpath + fname);

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fname);
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }
        }
    }

    public string GetImageName(string imgSl)
    {
        string sql = "select splCharFile from SpecialChars where splChar_slno=" + imgSl;
        using (Database db = new Database("connString"))
        {
            string imgName = db.ExecuteScalar<string>(sql);
            return imgName;
        }
    }
    
    public int Pixel2MTU(int pixels)
    {
        int mtus = pixels * 9525;
        return mtus;
    }

    //void ExportFile()
    //{


    //    // if required to print save the excel file as pdf and send to print

    //    Process printjob = new Process();

    //printjob.StartInfo.FileName = @"D:\R&D\Changes to be made.pdf" //path of your file;

    //printjob.StartInfo.Verb = "Print";

    //printjob.StartInfo.CreateNoWindow = true;

    //printjob.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

    //PrinterSettings setting = new PrinterSettings();

    //setting.DefaultPageSettings.Landscape = true;

    //printjob.Start();

    //}

}