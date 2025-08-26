using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

using System.IO;
using Syncfusion.XlsIO;
using Syncfusion.Pdf;
using OfficeOpenXml;

using System.Configuration;
using System.Data;
using NPoco;

using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.ExcelToPdfConverter;
using OfficeOpenXml.Style;
using System.Runtime.Remoting.Contexts;
using Serilog.Core;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using OfficeOpenXml.Drawing;

using System.Drawing;
using Syncfusion.JavaScript.Models;
using Syncfusion.JavaScript;
using System.Windows.Forms;
using iTextSharp.xmp.impl.xpath;
using System.Web.Script.Services;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using System.Activities.Statements;
using Syncfusion.JavaScript.DataVisualization.Models;
using Syncfusion.CompoundFile.XlsIO.Native;
using System.Activities.Expressions;
using Google.Protobuf;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System.IO.Packaging;
using NPoco.FluentMappings;
using Org.BouncyCastle.Ocsp;
using Google.Protobuf.WellKnownTypes;
using Microsoft.SqlServer.Management.Smo;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Syncfusion.XlsIO.Interfaces;
using Database = NPoco.Database;
using System.Windows.Forms.VisualStyles;
using Org.BouncyCastle.Asn1;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font = iTextSharp.text.Font;
using Azure;
using Serilog;
using NPoco.Expressions;
using System.Diagnostics;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Org.BouncyCastle.Utilities;
//using PdfDocument = iTextSharp.text.pdf.PdfDocument;
using iTextSharp.text.pdf.parser;
using Syncfusion.XPS;
using Paragraph = iTextSharp.text.Paragraph;
using Rectangle = iTextSharp.text.Rectangle;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using K4os.Compression.LZ4.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.ServiceModel.Web;
using Syncfusion.JavaScript.DataVisualization.Models.Diagram;

//using static iTextSharp.text.pdf.XfaXpathConstructor;

///using static NPoco.Snapshot<T>;


// using static System.Net.Mime.MediaTypeNames;
/// <summary>
/// Summary description for data
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class data : System.Web.Services.WebService
{
    static string varcp = string.Empty;
    static int rowvallength = 0;
    static string rowvallengthpart = string.Empty;
    static string prodchar = string.Empty;
    static int rowAPPMachining = 0;
    static int rowAPPCasting = 0;
    static int FOIrevstatuscolumn = 0;
    static int FOIOPcolumn = 0;

    public data()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 

    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public string GetSpecialChars()
    {

        using (Crud_SpecialChars cd = new Crud_SpecialChars())
        {
            return JsonConvert.SerializeObject(cd.usp_SpecialCharsSelect());
        }
    }

    [WebMethod]
    public string GetSpecialChars2(int custsl)
    {
        using (Crud_SpecialChars cd = new Crud_SpecialChars())
        {
            return JsonConvert.SerializeObject(cd.usp_SpecialCharsSelect().Where(x => x.cust_slno == custsl));
        }
    }

    [WebMethod]
    public string GetGauges()
    {
        using (Crud_mdMaster crud = new Crud_mdMaster())
        {
            List<Class_mdMaster> lst = crud.SelectAll();
            return JsonConvert.SerializeObject(lst);
        }
    }

    [WebMethod]
    public string GetFrequencies()
    {

        using (Crud_SampleFrequency cd = new Crud_SampleFrequency())
        {
            return JsonConvert.SerializeObject(cd.usp_SampleFrequencySelect());
        }
    }

    [WebMethod]
    public string GetControlmethods()
    {

        using (Crud_ControlMethods cd = new Crud_ControlMethods())
        {
            return JsonConvert.SerializeObject(cd.usp_ControlMethodsSelect());
        }
    }

    [WebMethod]
    public string GetDCRData(int partslno, int oprnslno, string changearea)
    {

        //using (Crud_dcr cd = new Crud_dcr())
        //{                   
        //   List<Class_DCR> lst = cd.SelectAll().Where(x=>x.part_slno==partslno && x.operation_slno==oprnslno && x.change_area== changearea && x.del_status=="N").ToList();
        //    return JsonConvert.SerializeObject(lst);
        //}
        string opn = oprnslno.ToString();
        using (Database db = new Database("connString"))
        {
            string sql = "select d.dcr_slno,d.dcr_number,d.[partDescription],d.[part_slno],d.[mstPartNo],d.[operation_slno] ,d.Changes_Required    ,d.[change_area],emp.EmployeeName,  CASE WHEN d.Process_Number = 'Y'    THEN 'Process Number,'    ELSE ''      END + CASE  WHEN d.Process_Name = 'Y'    THEN 'Process Name, '         ELSE ''   END  + CASE  WHEN d.Prc_Characteristics = 'Y'    THEN 'Process Characteristics, '     ELSE ''   END + CASE WHEN d.Prd_Characteristics='Y'    then  'Product Characteristics,'    ELSE ''   END + CASE  WHEN d.Specification='Y'   then  'Specification,'    ELSE ''   END + CASE  WHEN d.Measurement_Tech='Y'   then  'Measurement Tech,'    ELSE ''   END + CASE  WHEN d.Sample_Size='Y'   then  'Sample Size,'    ELSE ''   END + CASE  WHEN d.Sample_Frequency='Y'   then 'Sample Frequency,'   ELSE ''   END + CASE  WHEN d.Control_Method='Y'   then 'Control Method,'   ELSE ''   END + CASE  WHEN d.Others='Y'   then 'Others'   ELSE ''   END AS changes, [Existing]     ,[Changes_Required]     ,[Reason_For_Change]     ,nature_of_change,d.[Request_By]     ,[Request_Date]     ,d.[del_status]     ,d.[Submit_Status] from dcr d inner join  employees emp on d.Request_By=emp.EmployeeSlNo where d.part_slno=@partslno" +
                "  AND (',' + operations + ',') LIKE '%,' + @opn + ',%'   and d.change_area=@changearea  and d.del_status='N' and d.Submit_Status='A'";

            //var lst = db.Fetch<Class_DCR>(sql, partslno, oprnslno, changearea).ToList();
            var lst = db.Fetch<Class_DCR>(sql, new { partslno, opn, changearea }).ToList();
            return JsonConvert.SerializeObject(lst);
        }

    }
    [WebMethod]
    public string GetDCRSOPData(int Group_Id)
    {

       
        using (Database db = new Database("connString"))
        {
            string sql = "select distinct d.dcr_slno,d.dcr_number,d.Group_Id  ,map.Group_Name ,'SOP' as [change_area], emp.EmployeeName,  CASE WHEN d.Process_Number = 'Y'    THEN 'Process Number,'    ELSE ''      END + CASE  WHEN d.Process_Name = 'Y'    THEN 'Process Name, '         ELSE ''   END  + CASE  WHEN d.Prc_Characteristics = 'Y'    THEN 'Process Characteristics, '     ELSE ''   END + CASE WHEN d.Prd_Characteristics='Y'    then  'Product Characteristics,'    ELSE ''   END + CASE  WHEN d.Specification='Y'   then  'Specification,'    ELSE ''   END + CASE  WHEN d.Measurement_Tech='Y'   then  'Measurement Tech,'    ELSE ''   END + CASE  WHEN d.Sample_Size='Y'   then  'Sample Size,'    ELSE ''   END + CASE  WHEN d.Sample_Frequency='Y'   then 'Sample Frequency,'   ELSE ''   END + CASE  WHEN d.Control_Method='Y'   then 'Control Method,'   ELSE ''   END + CASE  WHEN d.Others='Y'   then 'Others'   ELSE ''   END AS changes, d.[Existing]     ,d.[Changes_Required]     ,d.[Reason_For_Change]     ,d.nature_of_change,d.[Request_By]     ,[Request_Date]     ,d.[del_status]     ,d.[Submit_Status] from dcr_sop d inner join  employees emp on d.Request_By=emp.EmployeeSlNo inner join Sop_mapping map on map.Group_Id=d.Group_Id  where  d.Group_Id=@Group_Id  and d.del_status='N' and d.Submit_Status='A'";

           
            var lst = db.Fetch<Class_DCR>(sql, new { Group_Id }).ToList();
            return JsonConvert.SerializeObject(lst);
        }

    }

    [WebMethod]
    public int GetRevNum(int partslno, int oprnslno, string changearea)
    {

        
        string opn = oprnslno.ToString();
        using (Database db = new Database("connString"))
        {
            // string sql = "select COALESCE(MAX(CAST(NULLIF(LTRIM(RTRIM(rev_no)), '') AS INT)), 0) + 1  from ControlPlan  where part_slno = @partslno  and obsolete='N'";
            //AND operation_slno = @oprnslno";

            string sql = "select COALESCE(MAX(CAST(NULLIF(LTRIM(RTRIM(cp_revno)), '') AS INT)), 0) + 1  from parts  where part_slno = @partslno  and obsolete='N' and  del_status='N'";
            int rev = db.ExecuteScalar<int>(sql, new { partslno});
            //int rev = db.ExecuteScalar<int>(sql, new { partslno, oprnslno });
            return rev;
        }

    }


    [WebMethod]
    public int GetSOPMAPPINGRevNum(int Group_Id)
    {


        string gr = Group_Id.ToString();
        using (Database db = new Database("connString"))
        {
            string sql = "select  COALESCE(MAX(rev_no), 0) + 1 from SOP_Mapping  where Group_Id=@gr";


            int rev = db.ExecuteScalar<int>(sql, new { gr });
            return rev;
        }

    }




    [WebMethod]
    public string GetControlplanChildData(int cp_slno)
    {

        using (Crud_ControlPlan_Child cd = new Crud_ControlPlan_Child())
        {
            return JsonConvert.SerializeObject(cd.GetChildData(cp_slno));
        }
    }
    [WebMethod]
    public string LoadSOPToolingsData(int slno)
    {
        string sql = "  select   tool_holder_name ,tool,cutting_speed,per_corner ,feed_rate,no_of_corners ,total_nos ,control_method  from sop_toolings inner join sop_header on sop_header.sop_id=sop_toolings.sop_id where sop_header.sop_id=@0";
        using (Database db = new Database("connString"))
        {
            var lst = db.Fetch<Class_sop_toolings>(sql, slno).ToList();
            return JsonConvert.SerializeObject(lst);
        }

    }


    [WebMethod]
    public string LoadSOPPrcParamData(int slno)
    {
        string sql = "  select  sop_ProcessParameter.* from sop_ProcessParameter inner join sop_header on sop_header.sop_id=sop_ProcessParameter.sop_id where sop_ProcessParameter.sop_id=@0";
        using (Database db = new Database("connString"))
        {
            var lst = db.Fetch<Class_sop_ProcessParameter>(sql, slno).ToList();
            return JsonConvert.SerializeObject(lst);
        }

    }
    [WebMethod]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
    public string GetPartsMapping()
    {
        try
        {
            using (Database db = new Database("connString"))
            {
                string sql = "select distinct parts.mstPartNo  from PartsMapping inner join parts on parts.part_slno=partsMapping.part_slno  where parts.del_status='N'   and parts.Obsolete='N'"; List<Class_parts> lst = db.Fetch<Class_parts>(sql);
                return JsonConvert.SerializeObject(lst);
            }
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { error = true, message = ex.Message });
        }

    }
    [WebMethod]
    public string Getpartslno(string partname)
    {
        using (Database db = new Database("connString"))
        {
            string sql = "select part_slno  from Parts where mstPartno=@0 and Obsolete='N'";
            List<Class_parts> lst = db.Fetch<Class_parts>(sql, partname);
            return JsonConvert.SerializeObject(lst);
        }
    }

    [WebMethod]
    public string Getoperationslno(string opname)
    {
        using (Database db = new Database("connString"))
        {
            string sql = "select operation_slno  from Operations where operationDesc=@0";
            List<Class_operations> lst = db.Fetch<Class_operations>(sql, opname);
            return JsonConvert.SerializeObject(lst);
        }
    }

    [WebMethod]
    public string GetOperationsMapping(string partslno)
    {
        string qry = string.Empty;
        using (Database db = new Database("connString"))
        {
            qry = "select distinct OperationDesc from operations op inner join  partsMapping pm on op.operation_slno=pm.operation_slno inner join parts p on p.part_slno=pm.part_slno where p.Obsolete='N' and p.del_status='N' ";
            var lst = db.Fetch<Class_operations>(qry).ToList();

            if (!string.IsNullOrEmpty(partslno))
            {
                qry = "select OperationDesc from operations op inner join  partsMapping pm on op.operation_slno=pm.operation_slno  inner join parts p on p.part_slno=pm.part_slno where p.Obsolete='N' and p.del_status='N' and p.part_slno=@0 ";
                lst = db.Fetch<Class_operations>(qry, partslno).ToList();
            }
            return JsonConvert.SerializeObject(lst);
        }
    }
    [WebMethod]
    public string GetMachine(string opslno, string pslno)
    {
        string qry = string.Empty;
        using (Database db = new Database("connString"))
        {
            qry = "select mch.MachineDesc,pm.machine_slno from machines mch inner join  partsMapping pm on mch.machine_slno=pm.machine_slno  inner join parts p on p.part_slno=pm.part_slno where p.Obsolete='N' and p.del_status='N' and pm.part_slno=@0  and pm.operation_slno=@1 ";
            var lst = db.Fetch<Class_machines>(qry, pslno, opslno).ToList();
            return JsonConvert.SerializeObject(lst);
        }
    }
    [WebMethod]
    public string GetSOPMappingChildData(int slno)
    {

        using (Crud_sop_mapping crud = new Crud_sop_mapping())
        {
            return JsonConvert.SerializeObject(crud.GetSOPChildData(slno));
        }
    }

    [WebMethod]
    public string GetEvalTech()
    {

        using (Crud_EvaluationTech ce = new Crud_EvaluationTech())
        {

            return JsonConvert.SerializeObject(ce.usp_EvaluationTechSelect());
        }
    }

    [WebMethod]
    public int GetEveltechslno(string evltech)
    {

        using (Database db = new Database("connString"))
        {
            //return db.SingleOrDefault<Class_EvaluationTech>(" where evalTech=@0", evltech).evalTech_slno;
            var obj = db.FetchWhere<Class_EvaluationTech>(x => x.evalTech == evltech);
            if (obj != null && obj.Count > 0)
            {
                if (obj[0].evalTech_slno == null)
                {
                    return -1;
                }
                else
                {
                    return obj[0].evalTech_slno;
                }
            }
            else
            {
                return -1;
            }
        }
    }

    [WebMethod]
    public int GetFreqslno(string freq)
    {

        using (Database db = new Database("connString"))
        {
            //return db.SingleOrDefault<Class_SampleFrequency>(" where FreqDesc=@0", freq).freq_slno;
            var obj = db.FetchWhere<Class_SampleFrequency>(x => x.FreqDesc == freq);
            if (obj != null && obj.Count > 0)
            {
                if (obj[0].freq_slno == null)
                {
                    return -1;
                }
                else
                {
                    return obj[0].freq_slno;
                }
            }
            else
            {
                return -1;
            }
        }
    }

    [WebMethod]
    public string GetCFTMembers(int slno)
    {
        Crud_CFTeamEmployees c = new Crud_CFTeamEmployees();
        return c.GetMembersList(slno);
    }

    [WebMethod]
    public int GetControlslno(string ctrl)
    {

        using (Database db = new Database("connString"))
        {
            //return db.SingleOrDefault<Class_ControlMethods>(" where methodDesc=@0", ctrl).method_slno;
            var obj = db.FetchWhere<Class_ControlMethods>(x => x.methodDesc == ctrl);
            if (obj != null && obj.Count > 0)
            {
                if (obj[0].method_slno == null)
                {
                    return -1;
                }
                else
                {
                    return obj[0].method_slno;
                }
            }
            else
            {
                return -1;
            }
        }
    }

    [WebMethod(EnableSession = true)]
    public string GenerateCP(int partsl, string oprnsl,string Isadmin)
    {
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];

            string sql = "";
            string sqlrevdtls = "";
            string sqllegend = "";
            int imgno = 0;
            //if (oprnsl == "All")
            //{
            //    sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( cast(p.rev_no as int) ) from controlplan p WHERE  p.Submitstatus='A'  AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )   and  c.part_slno=" + partsl;
            //    if (Isadmin == "Y")
            //    { 
            //        sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( cast(p.rev_no as int) ) from controlplan p WHERE  c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )   and  c.part_slno=" + partsl; 
            //    }
            //}
            //else
            //{
            //    sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( cast(p.rev_no as int) ) from controlplan p WHERE  p.Submitstatus='A'  AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )  and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";

            //    if (Isadmin == "Y")
            //    { sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( cast(p.rev_no as int) ) from controlplan p WHERE  c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )  and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";
            //    }
            //}
            if (oprnsl == "All")
            {
                sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE  p.Submitstatus='A'  AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )   and  c.part_slno=" + partsl;

            }
            else
            {
                sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT)) from controlplan p WHERE  p.Submitstatus='A'  AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno )  and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";

            }


            sqlrevdtls = @"select part_revision_history.* from part_revision_history inner join parts on parts.part_slno=part_revision_history.part_slno where (part_revision_history.part_slno=@0 or parts.mstPartNo=@1 ) and ISNULL(LTRIM(RTRIM(part_revision_history.revision_done_in)), '') <> 'SOP'  and parts.del_status='N'   and parts.Obsolete='N'";

            sqllegend = @"select distinct top 4 s.splchar_slno, s.splCharFile, s.spl_char_desc 
from SpecialChars s
inner join customers c on c.cust_slno=s.cust_slno
inner join parts p on p.Customer_name=c.cust_name
where p.part_slno=" + partsl + " and s.show_in_legend=1";

            string fname = "cp2_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/ControlPlanTemplate2.xlsx"));
            var userInfoad = HttpContext.Current.Session["UserInfo"] as List<Class_Employees>;

            if (userInfoad != null && userInfoad.Count > 0)
            {
                string isAdmin = userInfoad[0].isAdmin;
                if (isAdmin == "Y")
                    templateFile = new FileInfo(Server.MapPath("~/App_Data/ControlPlanTemplate2_Admin.xlsx"));
            }
            Logger.LogError(sql);
            string cp_slno = string.Empty;
            int startRow = 8;
            int endrow = startRow;

            using (Database db = new Database("connString"))
            {
                cp_slno = db.ExecuteScalar<string>(sql);


                if (oprnsl == "All")
                {
                    db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", null, partsl, null);
                }
                else
                {
                    db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                }
               
                string sqlcp = @"Select * from temp_rptControlPlan where Obsolete='N' order by cast(process_no as decimal)  ";

                string sqlpart = @"Select remarks from parts where part_slno=@0";
                string remarks = db.ExecuteScalar<string>(sqlpart, partsl);
                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                //if (lst.Count <= 0)
                //    sqlcp = @"Select * from temp_rptControlPlan where Obsolete='Y'  order by cast(process_no as decimal)";
                // lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);


                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where   Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0  and  Obsolete='Y'     ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }






                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                if (lst.Count > 0)
                {
                    List<Class_part_revision_history> lstrev = db.Fetch<Class_part_revision_history>(sqlrevdtls, partsl, lst[0].mstPartNo);
                    int xlrowht = 75;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                        ws.Cells[3, 1].RichText.Add("PROTOTYPE");
                        ws.Cells[3, 2].RichText.Add("PRE-LAUNCH");
                        ws.Cells[3, 3].RichText.Add("PRODUCTION");

                        if (lst[0].cpType == "PROTOTYPE")
                        {
                            ws.Cells[3, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            ws.Cells[3, 1].Style.Font.Size = 19;
                        }
                        else if (lst[0].cpType == "PRE-LAUNCH")
                        {
                            ws.Cells[3, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            ws.Cells[3, 2].Style.Font.Size = 19;
                        }
                        else if (lst[0].cpType == "PRODUCTION")
                        {
                            ws.Cells[3, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[3, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            ws.Cells[3, 3].Style.Font.Size = 19;
                        }

                        ws.Cells[2, 1].Value = "Control Plan Number: " + lst[0].cp_number;
                        ws.Cells[4, 1].Value = "Part Number & Rev No & Rev Date: " + lst[0].mstPartNo + " / " + lst[0].partIssueNo + "-" + lst[0].partIssueDt.Replace("/", ".");
                        rowvallengthpart = ws.Cells[4, 1].Value.ToString();
                        ws.Cells[5, 1].Value = "Part Name/Description: " + lst[0].PartDescription;
                        rowvallength = lst[0].PartDescription.Length;
                        ws.Cells[5, 4].Value = "Organization / Plant Approval/ Date: ";
                        ws.Cells[6, 1].Value = "Organization: " + lst[0].organization; //supplier 
                        ws.Cells[2, 4].Value = "Key Contact / Phone: " + lst[0].keyContact + " / " + lst[0].keyContactPhone;
                        ws.Cells[3, 4].Value = "Core Team: " + lst[0].CFTeamName; //core team
                        ws.Cells[4, 4].Value = "Organization Code: " + lst[0].supplier_code; //supplier code
                        ws.Cells[4, 9].Value = "Customer Name: " + lst[0].Customer_name; //supplier code
                        ws.Cells[6, 4].Value = "Other Approval / Date (if Req'd): " + lst[0].otherApproval + " " + lst[0].otherApprovalDt;
                        ws.Cells[5, 9].Value = "Process Specification: "; //process specification
                        ws.Cells[6, 9].Value = "IH Metallurgy testing reference: " + lst[0].ih_testing_ref;
                        ws.Cells[2, 14].Value = "Date(Original): " + lst[0].originalDt.Replace("/", ".");
                        ws.Cells[4, 14].Value = "Customer Engineering Approval / Date (if Req'd):" + lst[0].custApprovalDt.Replace("/", ".");
                        if (lst[0].cp_revno != null)
                        {
                            ws.Cells[3, 14].Value = "CP Rev No / Date: " + lst[0].cp_revno.ToString() + " / " + lst[0].cp_revdt.Replace("/", "."); //cp rev no
                        }
                        else
                        {
                            ws.Cells[3, 14].Value = "CP Rev No / Date: ";
                        }

                        ws.Cells[5, 14].Value = "Customer Quality Approval (if Req'd): " + lst[0].custQaApproval;
                        ws.Cells[6, 14].Value = "Other Approval / Date (if Req'd): " + lst[0].otherApproval + " " + lst[0].otherApprovalDt.Replace("/", ".");
                        //ws.Cells["10:A12"].Merge = true;
                        ws.Cells[10, 1].Value = "Remarks:" + Environment.NewLine + remarks;
                        int row = 9;

                        string imgPath = Server.MapPath("~/Documents/");
                        string lgndPath = Server.MapPath("~/Documents/legend/");
                        int no = 1;

                        // remove the legend based on the client
                        int rowToDelete1 = 0, rowToDelete2 = 0, rowToDelete3 = 0, rowToDelete4 = 0;

                        if (lst[0].Customer_name.ToLower().Trim() == "meritor")
                        {
                            rowToDelete1 = 15;
                            rowToDelete2 = 16;
                            rowToDelete3 = 17;
                            rowToDelete4 = 18;
                        }
                        else if (lst[0].Customer_name.ToLower().Trim() == "eu meritor")
                        {
                            rowToDelete1 = 14;
                            rowToDelete2 = 16;
                            rowToDelete3 = 17;
                            rowToDelete4 = 18;
                        }
                        else if (lst[0].Customer_name.ToLower().Trim() == "axle tech international")
                        {
                            rowToDelete1 = 14;
                            rowToDelete2 = 15;
                            rowToDelete3 = 17;
                            rowToDelete4 = 18;
                        }
                        else if (lst[0].Customer_name.ToLower().Trim() == "volvo")
                        {
                            rowToDelete1 = 14;
                            rowToDelete2 = 15;
                            rowToDelete3 = 16;
                            rowToDelete4 = 18;

                        }
                        else if (lst[0].Customer_name.ToLower().Trim() == "apa")
                        {
                            rowToDelete1 = 14;
                            rowToDelete2 = 15;
                            rowToDelete3 = 16;
                            rowToDelete4 = 17;
                        }
                        if (rowToDelete1 > 0 && rowToDelete2 > 0 && rowToDelete3 > 0 && rowToDelete4 > 0)
                        {
                            ws.DeleteRow(rowToDelete4);
                            ws.DeleteRow(rowToDelete3);
                            ws.DeleteRow(rowToDelete2);
                            ws.DeleteRow(rowToDelete1);
                            ws.Cells[15, 1].Value = "Prepared By: " + lst[0].preparedBy;
                            ws.Cells[15, 11].Value = "Approved By: " + lst[0].approvedBy;

                        }
                        else
                        {
                            ws.Cells[19, 1].Value = "Prepared By: " + lst[0].preparedBy;
                            ws.Cells[19, 11].Value = "Approved By: " + lst[0].approvedBy;
                        }

                        int index = (row + (lst.Count - 1) + 5);
                        ws.InsertRow(row + 1, lst.Count - 1);

                        int i = 0;
                        int partNumStartRow = row;
                        int oprnstartrow = row;
                        int oprnendrow = 0;

                        // display the legend images

                        if (lst[0].Customer_name.ToLower().Trim().Contains("meritor") ||
                            lst[0].Customer_name.ToLower().Trim().Contains("axle")) // meritor and eu meritor
                        {
                            string lgndimg1 = "Picture1.png";
                            string lgndpth = (lgndPath + lgndimg1);
                            FileInfo imgFilelgnd1 = new FileInfo(lgndpth);
                            if (imgFilelgnd1.Exists)
                            {
                                Bitmap image = new Bitmap(lgndpth);
                                ExcelPicture excelPicture = ws.Drawings.AddPicture("legend1", image);
                                ExcelRangeBase cell = ws.Cells[index - 1, 2];
                                excelPicture.SetPosition(cell.Start.Row, 14, cell.Start.Column, 6);
                                // excelPicture.SetSize(50, 50);
                            }

                            string lgndimg2 = "Picture2.png";
                            string lgndpth2 = (lgndPath + lgndimg2);
                            FileInfo imgFilelgnd2 = new FileInfo(lgndpth2);
                            if (imgFilelgnd2.Exists)
                            {
                                Bitmap image = new Bitmap(lgndpth2);
                                ExcelPicture excelPicture = ws.Drawings.AddPicture("legend2", image);
                                ExcelRangeBase cell = ws.Cells[index - 1, 7];
                                excelPicture.SetPosition(cell.Start.Row, 14, cell.Start.Column, 6);
                                // excelPicture.SetSize(50, 50);
                            }

                            string lgndimg3 = "Picture3.png";
                            string lgndpth3 = (lgndPath + lgndimg3);
                            FileInfo imgFilelgnd3 = new FileInfo(lgndpth3);
                            if (imgFilelgnd3.Exists)
                            {
                                Bitmap image = new Bitmap(lgndpth3);
                                ExcelPicture excelPicture = ws.Drawings.AddPicture("legend3", image);
                                ExcelRangeBase cell = ws.Cells[index - 1, 11];
                                excelPicture.SetPosition(cell.Start.Row, 14, cell.Start.Column, 6);
                                // excelPicture.SetSize(50, 50);
                            }

                            string lgndimg4 = "Picture4.png";
                            string lgndpth4 = (lgndPath + lgndimg4);
                            FileInfo imgFilelgnd4 = new FileInfo(lgndpth4);
                            if (imgFilelgnd4.Exists)
                            {
                                Bitmap image = new Bitmap(lgndpth4);
                                ExcelPicture excelPicture = ws.Drawings.AddPicture("legend4", image);
                                ExcelRangeBase cell = ws.Cells[index - 1, 15];
                                excelPicture.SetPosition(cell.Start.Row, 14, cell.Start.Column, 6);
                                // excelPicture.SetSize(50, 50);
                            }
                        }
                        else if (lst[0].Customer_name.ToLower().Trim().Contains("volvo") ||
                            lst[0].Customer_name.ToLower().Trim().Contains("apa"))
                        {
                            string lgndimg3 = "Picture3.png";
                            string lgndpth3 = (lgndPath + lgndimg3);
                            FileInfo imgFilelgnd3 = new FileInfo(lgndpth3);
                            if (imgFilelgnd3.Exists)
                            {
                                Bitmap image = new Bitmap(lgndpth3);
                                ExcelPicture excelPicture = ws.Drawings.AddPicture("legend3", image);
                                ExcelRangeBase cell = ws.Cells[index - 1, 11];
                                excelPicture.SetPosition(cell.Start.Row, 14, cell.Start.Column, 6);
                                // excelPicture.SetSize(50, 50);
                            }

                            string lgndimg4 = "Picture4.png";
                            string lgndpth4 = (lgndPath + lgndimg4);
                            FileInfo imgFilelgnd4 = new FileInfo(lgndpth4);
                            if (imgFilelgnd4.Exists)
                            {
                                Bitmap image = new Bitmap(lgndpth4);
                                ExcelPicture excelPicture = ws.Drawings.AddPicture("legend4", image);
                                ExcelRangeBase cell = ws.Cells[index - 1, 15];
                                excelPicture.SetPosition(cell.Start.Row, 14, cell.Start.Column, 6);
                                // excelPicture.SetSize(50, 50);
                            }
                        }
                        int rwstart = 0;
                        int rwfreqstart = 0;
                        int rwfreq1start = 0;
                        int flag = 0;
                        int flagfreq = 0;
                        int flagfreq1 = 0;


                        while (i <= lst.Count - 1)
                        {
                            string op = lst[i].OperationDesc;
                            int rowmerge = 0;
                            no = 1;
                            oprnstartrow = row;
                            string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                            string mach = db.ExecuteScalar<string>(sqlmachinecode, lst[i].machine_slno);
                            ws.Cells[row, 1].Value = lst[i].process_no;//process number
                            ws.Cells[row, 2].Value = lst[i].OperationDesc;
                            if (mach == null || mach == "" || mach == "-")
                                ws.Cells[row, 3].Value = lst[i].MachineDesc;
                            else
                                ws.Cells[row, 3].Value = lst[i].MachineDesc + " / " + mach;
                            string pdtchar = string.Empty;
                            string prcchar = string.Empty;
                            while (op == lst[i].OperationDesc)
                            {

                                Class_Temp_RptControlPlan c = lst[i];

                                pdtchar = c.product_char;
                                prcchar = c.process_char;
                                ws.Cells[row, 4].Value = c.dimn_no;
                                ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[row, 5].Value = c.product_char;

                                //ws.Cells[row, 6, row, 7].Merge = false;
                                //ws.Cells[row, 6, row, 7].Merge = true;

                                ws.Cells[row, 6].Value = c.process_char;

                                if (c.splChar_slno > 0)
                                {
                                    string img = GetFileName(c.splChar_slno.ToString());
                                    string pth = (imgPath + img);
                                    FileInfo imgFile = new FileInfo(pth);
                                    if (imgFile.Exists)
                                    {
                                        imgno += 1;
                                        Bitmap image = new Bitmap(pth);
                                        ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                        ExcelRangeBase cell = ws.Cells[row - 1, 7];

                                        excelPicture.SetPosition(cell.Start.Row, 20, cell.Start.Column, 7);
                                        excelPicture.SetSize(50, 50);

                                    }
                                }
                                ws.Cells[row, 8].Value = string.Empty; //special characteristics 
                                ws.Cells[row, 9].Value = c.spec1;
                                ws.Cells[row, 10].Value = c.measurementTech;
                                ws.Cells[row, 11].Value = c.sampleSize;
                                ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[row, 12].Value = c.sampleFreq;
                                ws.Cells[row, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                                // Merge columns sample size and freq if any one is empty
                                if (!string.IsNullOrEmpty(c.sampleSize) && !string.IsNullOrWhiteSpace(c.sampleFreq))
                                {
                                    ws.Cells[row, 11].Value = c.sampleSize;
                                    ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[row, 12].Value = c.sampleFreq;
                                    ws.Cells[row, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }
                                else
                                {
                                    // ws.Cells[row, 11, row, 12].Merge = true;
                                    if (!string.IsNullOrWhiteSpace(c.sampleSize))
                                    {
                                        ws.Cells[row, 11].Value = c.sampleSize;
                                        ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                    else
                                    {
                                        ws.Cells[row, 11].Value = c.sampleFreq;
                                        ws.Cells[row, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }

                                }
                                if (!string.IsNullOrEmpty(lst[i].sampleFreq))
                                {
                                    if (lst[i].sampleFreq.ToLower().Contains("every setting /"))
                                    { rwfreq1start = row; flagfreq1 = 1; }
                                }
                                ws.Cells[row, 13].Value = c.methodDesc;
                                ws.Cells[row, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[row, 14].Value = c.measurementTech2;
                                ws.Cells[row, 15].Value = c.sampleSize2;
                                ws.Cells[row, 16].Value = c.sampleFreq2;

                                // Merge columns sample size and freq if any one is empty
                                if (!string.IsNullOrEmpty(c.sampleSize2) && !string.IsNullOrWhiteSpace(c.sampleFreq2))
                                {
                                    ws.Cells[row, 15].Value = c.sampleSize2;
                                    ws.Cells[row, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[row, 16].Value = c.sampleFreq2;
                                    ws.Cells[row, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }
                                else
                                {
                                    //ws.Cells[row, 15, row, 16].Merge = true;
                                    if (!string.IsNullOrWhiteSpace(c.sampleSize2))
                                    {
                                        ws.Cells[row, 15].Value = c.sampleSize2;
                                        ws.Cells[row, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    }
                                    //else
                                    //{
                                    //    ws.Cells[row, 16].Value = c.sampleFreq2;
                                    //    if (i == 0) {
                                    //        if (c.sampleFreq2.Contains("Every Setting"))
                                    //        {
                                    //            flagfreq = 1;
                                    //        }

                                    //    }

                                    //}
                                }

                                ws.Cells[row, 16].Value = c.sampleFreq2;
                                ws.Cells[row, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                if (!string.IsNullOrEmpty(lst[i].sampleFreq2))
                                {
                                    if (lst[i].sampleFreq2.ToLower().Contains("every setting /"))
                                    { rwfreqstart = row; flagfreq = 1; }
                                }


                                ws.Cells[row, 17].Value = c.methodDesc2;
                                ws.Cells[row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                if (c.product_char == "Material Test")
                                {
                                    ws.Cells[row, 18].Merge = false;
                                    ws.Cells[row, 18, row + 1, 18].Merge = true;
                                    ws.Cells[row, 18].Value = c.reactionPlan;
                                    ws.Cells[row, 18, row + 1, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                }
                                else if (c.product_char.Contains("Refer Supplier Report"))
                                {
                                    ws.Cells[row, 18].Merge = false;
                                    ws.Cells[row, 18, row, 18].Merge = true;
                                    ws.Cells[row, 18].Value = c.reactionPlan;
                                    ws.Cells[row, 18, row + 1, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                }
                                else
                                {
                                    ws.Cells[row, 18].Value = c.reactionPlan;
                                }


                                // if only prod char or proc char exists then merge the rest of columns
                                if (!string.IsNullOrWhiteSpace(pdtchar) || !string.IsNullOrWhiteSpace(prcchar))
                                {
                                    // Get the columns I and Q as their column indexes
                                    int columnI = 8; // Column "H" is the 8th column (index starts from 1)
                                    int columnQ = 17; // Column "Q" is the 17th column

                                    if (string.IsNullOrWhiteSpace(prcchar))
                                    {
                                        columnI = 6;
                                    }

                                    // Check if any cell has a value between columns I and Q (inclusive)
                                    bool hasValue = false;
                                    for (int col = columnI; col <= columnQ; col++)
                                    {
                                        if (ws.Cells[row, col].Value != null && !string.IsNullOrWhiteSpace(ws.Cells[row, col].Text))
                                        {
                                            hasValue = true;
                                            break;
                                        }
                                    }
                                    // If no cell has a value, merge the columns I to Q
                                    if (!hasValue)
                                    {
                                        var range = ws.Cells[row, columnI, row, columnQ];
                                        range.Merge = false;
                                        range.Merge = true;
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrWhiteSpace(lst[i].process_char))
                                        {
                                            if (lst[i].process_char.ToLower().Contains("ref sop") || lst[i].process_char.ToLower().Contains(" sop") || lst[i].process_char.ToLower().Contains("refer sop"))
                                            {
                                                string celladr2 = "F" + row + ":" + "G" + row;
                                                ws.Select(celladr2);
                                                ws.SelectedRange.Merge = false;
                                                ws.SelectedRange.Merge = true;
                                                rwstart = row;
                                                flag = 1;
                                            }
                                            else
                                            {
                                                string celladr2 = "F" + row + ":" + "G" + row;
                                                ws.Select(celladr2);
                                                ws.SelectedRange.Merge = false;
                                                ws.SelectedRange.Merge = true;
                                            }
                                        }
                                        else
                                        {
                                            string celladr2 = "F" + row + ":" + "G" + row;
                                            ws.Select(celladr2);
                                            ws.SelectedRange.Merge = false;
                                            ws.SelectedRange.Merge = true;
                                        }


                                    }
                                }

                                else
                                {
                                    string celladr = "E" + row + ":" + "Q" + row;
                                    ws.Select(celladr);
                                    ws.SelectedRange.Merge = false;
                                    ws.SelectedRange.Merge = true;

                                }

                                if (string.IsNullOrWhiteSpace(ws.Cells[row, 6].Text) && ws.Cells[row, 6, row, 17].Merge == false && ws.Cells[row, 6, row, 7].Merge == false && flag != 1)
                                {
                                    string celladr2 = "F" + row + ":" + "G" + row;
                                    ws.Select(celladr2);
                                    ws.SelectedRange.Merge = false;
                                    ws.SelectedRange.Merge = true;
                                }
                                // merge the cells for other columns


                                // Vertical middle the columns
                                ws.Cells[row, 4, row, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                ws.Cells[row, 5, row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[row, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[row, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[row, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[row, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                row += 1;

                                ws.Row(row).Height = xlrowht;
                                no++;
                                i++;
                                if (i == lst.Count) break;
                            }

                            // when operation changes, merge the required columns
                            oprnendrow = row - 1;
                            ws.Cells[oprnstartrow, 1, oprnendrow, 1].Merge = true;
                            ws.Cells[oprnstartrow, 2, oprnendrow, 2].Merge = true;
                            ws.Cells[oprnstartrow, 3, oprnendrow, 3].Merge = true;

                            //if (flag == 1)
                            //{
                            //    ws.Cells[rwstart, 6, oprnendrow, 7].Merge = true;
                            //}
                            //if (flagfreq == 1)
                            //{
                            //    ws.Cells[rwfreqstart, 16, oprnendrow, 16].Merge = true;
                            //}
                            //if (flagfreq1 == 1)
                            //{
                            //    ws.Cells[rwfreq1start, 12, oprnendrow, 12].Merge = true;
                            //}
                            if (flag == 1)
                            {
                                //bool isAlreadyMerged = false;
                                //foreach (var mergedRange in ws.MergedCells)
                                //{

                                //    ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
                                //    int mergedStartColumn = mergedCells.Start.Column;
                                //    int mergedEndColumn = mergedCells.End.Column;
                                //    int mergedStartRow = mergedCells.Start.Row;
                                //    int mergedEndRow = mergedCells.End.Row;

                                //    // Check if column 12 is within the merged range in the specific row range
                                //    if (rwstart >= mergedStartRow && rwstart <= mergedEndRow)
                                //    {
                                //        if( (6 >= mergedStartColumn && 6 <= mergedEndColumn) || (7 >= mergedStartColumn && 7 <= mergedEndColumn))
                                //        {
                                //            isAlreadyMerged = true;
                                //            break; // Exit the loop as we found that column 12 is already merged
                                //        }
                                //    }
                                //}
                                bool isAlreadyMerged = ws.MergedCells.Cast<string>().Any(mergedRange =>
                                {
                                    ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
                                    int mergedStartColumn = mergedCells.Start.Column;
                                    int mergedEndColumn = mergedCells.End.Column;

                                    // Check if columns 6 and 7 are within the merged range
                                    return (6 >= mergedStartColumn && 6 <= mergedEndColumn) || (7 >= mergedStartColumn && 7 <= mergedEndColumn);
                                });
                                if (!isAlreadyMerged)
                                {
                                    ws.Cells[rwstart, 6, oprnendrow, 7].Merge = true;
                                }
                                //else
                                //{
                                //    for (int rowm = rwstart; rowm <= oprnendrow; rowm++)
                                //    {
                                //        string cellAddress = "F" + rowm + ":" + "G" + rowm;
                                //        ws.Select(cellAddress);
                                //        ws.SelectedRange.Merge = false;
                                //    }
                                //    ws.Cells[rwstart, 6, oprnendrow, 7].Merge = true;

                                //}
                            }
                            if (flagfreq == 1)
                            {
                                //bool isAlreadyMerged = ws.MergedCells.Cast<string>().Any(mergedRange =>
                                //{
                                //    ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
                                //    int mergedStartColumn = mergedCells.Start.Column;
                                //    int mergedEndColumn = mergedCells.End.Column;

                                //    // Check if column 16 is within the merged range
                                //    return (16 >= mergedStartColumn && 16 <= mergedEndColumn);
                                //});

                                bool isAlreadyMerged = false;
                                foreach (var mergedRange in ws.MergedCells)
                                {

                                    ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
                                    int mergedStartColumn = mergedCells.Start.Column;
                                    int mergedEndColumn = mergedCells.End.Column;
                                    int mergedStartRow = mergedCells.Start.Row;
                                    int mergedEndRow = mergedCells.End.Row;

                                    // Check if column 12 is within the merged range in the specific row range
                                    //if (rwfreqstart >= mergedStartRow && rwfreqstart <= mergedEndRow)
                                    //{
                                    //    if (16 >= mergedStartColumn && 16 <= mergedEndColumn)
                                    //    {
                                    //        isAlreadyMerged = true;
                                    //        break; // Exit the loop as we found that column 12 is already merged
                                    //    }
                                    //}
                                    bool overlap1 = rwfreqstart <= mergedEndRow && oprnendrow >= mergedStartRow &&
                   16 >= mergedStartColumn && 16 <= mergedEndColumn;
                                    if (overlap1)
                                    {
                                        isAlreadyMerged = true;
                                        break; // Exit the loop as we found that the range is already merged
                                    }
                                }


                                if (isAlreadyMerged == false)
                                    ws.Cells[rwfreqstart, 16, oprnendrow, 16].Merge = true;

                            }
                            if (flagfreq1 == 1)
                            {
                                //bool isAlreadyMerged = ws.MergedCells.Cast<string>().Any(mergedRange =>
                                //{
                                //    ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
                                //    int mergedStartColumn = mergedCells.Start.Column;
                                //    int mergedEndColumn = mergedCells.End.Column;

                                //    // Check if column 12 is within the merged range
                                //    return (12 >= mergedStartColumn && 12 <= mergedEndColumn);
                                //});
                                bool isAlreadyMerged = false;
                                foreach (var mergedRange in ws.MergedCells)
                                {
                                    ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
                                    int mergedStartColumn = mergedCells.Start.Column;
                                    int mergedEndColumn = mergedCells.End.Column;
                                    int mergedStartRow = mergedCells.Start.Row;
                                    int mergedEndRow = mergedCells.End.Row;

                                    // Check if column 12 is within the merged range in the specific row range
                                    //if (rwfreq1start >= mergedStartRow && rwfreq1start <= mergedEndRow)
                                    //{
                                    //    if (12 >= mergedStartColumn && 12 <= mergedEndColumn)
                                    //    {
                                    //        isAlreadyMerged = true;
                                    //        break; // Exit the loop as we found that column 12 is already merged
                                    //    }
                                    //}

                                    bool overlap = rwfreq1start <= mergedEndRow && oprnendrow >= mergedStartRow &&
                      12 >= mergedStartColumn && 12 <= mergedEndColumn;

                                    if (overlap)
                                    {
                                        isAlreadyMerged = true;
                                        break; // Exit the loop as we found that the range is already merged
                                    }
                                }
                                if (isAlreadyMerged == false)
                                    ws.Cells[rwfreq1start, 12, oprnendrow, 12].Merge = true;

                            }
                            rwstart = 0;
                            rwfreqstart = 0;
                            rwfreq1start = 0;
                            flagfreq = 0;
                            flag = 0;
                            flagfreq1 = 0;

                            ws.Cells[oprnstartrow, 1, oprnendrow, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                            ws.Cells[oprnstartrow, 2, oprnendrow, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                            ws.Cells[oprnstartrow, 3, oprnendrow, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                            ws.Cells[oprnstartrow, 6, oprnendrow, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[oprnstartrow, 16, oprnendrow, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ws.Cells[oprnstartrow, 1, oprnendrow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 2, oprnendrow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 3, oprnendrow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 6, oprnendrow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[oprnstartrow, 16, oprnendrow, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            ws.Cells[oprnstartrow, 11, oprnendrow, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 12, oprnendrow, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 13, oprnendrow, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 15, oprnendrow, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[oprnstartrow, 17, oprnendrow, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            if (!op.ToLower().Contains("receiving inspection") && !op.ToLower().Contains("induction hardening") && !op.ToLower().Contains("tempering"))
                            {
                                if (!pdtchar.ToLower().Contains("material characteristics") &&
                                   !pdtchar.ToLower().Contains("dimensional characteristics") &&
                                   !pdtchar.ToLower().Contains("vendor control plan"))
                                {

                                    ws.Cells[oprnstartrow, 18, oprnendrow, 18].Merge = true;
                                }
                                else
                                {
                                    // 
                                }

                                // align the merged cells to the top

                                ws.Cells[oprnstartrow, 18, oprnendrow, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                            }

                            if (i == lst.Count) break;
                        }
                        endrow = row - 1;

                        int revindex = 16 + lst.Count + 5;
                        int revstart = revindex;

                        if (lstrev.Count > 0)
                        {
                            foreach (Class_part_revision_history c in lstrev)
                            {
                                ws.Row(revindex).Height = xlrowht;
                                ws.Cells[revindex, 1].Value = c.rev_no;
                                ws.Cells[revindex, 2].Value = c.rev_date;
                                ws.Cells[revindex, 3].Value = c.change_nature;
                                ws.Cells[revindex, 3, revindex, 10].Merge = true;

                                ws.Cells[revindex, 11].Value = c.rev_reasons;
                                ws.Cells[revindex, 11, revindex, 18].Merge = true;
                                ws.Cells[revindex, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[revindex, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[revindex, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                ws.Cells[revindex, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                ws.Cells[revindex, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                ws.Cells[revindex, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                revindex += 1;
                            }
                            //var userInfoad = HttpContext.Current.Session["UserInfo"] as List<Class_Employees>;

                            //if (userInfoad != null && userInfoad.Count > 0)
                            //{
                            //    string isAdmin = userInfoad[0].isAdmin;
                            //    if (isAdmin == "Y")
                            //        ws.Cells[revindex, 1].Value = " \"CONTROLLED COPY\"  RELEASED BY MEI - QMS  Downloaded on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                            //    else
                            //        ws.Cells[revindex, 1].Value = " \"UNCONTROLLED WHEN PRINTED\"  Downloaded on" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                            //    ws.Cells[revindex, 1].Style.Font.Size = 20;
                            //    ws.Cells[revindex, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            //   ws.Cells[revindex, 1, revindex, 18].Merge = true;
                            //    ws.Cells[revindex, 1, revindex, 18].Style.Font.Color.SetColor(System.Drawing.Color.Gray);
                            //}

                            // ws.DeleteRow(16 + lst.Count + 1);
                            ws.DeleteRow(16 + lst.Count + 1);
                            ws.DeleteRow(16 + lst.Count + 1);
                            ws.DeleteRow(16 + lst.Count + 1);
                            // ws.Cells[revstart, 1, revstart, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                        }
                        int newRowCount = ws.Dimension.End.Row - (ws.Dimension.End.Row - revindex);
                        int currentEndRow = newRowCount;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:R" + (currentEndRow - 3)];

                        var newrange = ws.Cells["A1:R" + currentEndRow];
                        var border = newrange.Style.Border;
                        // Set border style and color for all sides of the cells
                        border.Top.Style = border.Right.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;
                        border.Top.Color.SetColor(System.Drawing.Color.Black);
                        border.Right.Color.SetColor(System.Drawing.Color.Black);
                        border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        border.Left.Color.SetColor(System.Drawing.Color.Black);
                        //newrange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        newrange.Style.WrapText = true;
                        // Logic to merge Product starts here
                        int flagforproduct = 0;
                        row = partNumStartRow;
                        oprnstartrow = row;
                        oprnendrow = 0;
                        i = 0;
                        while (i < lst.Count - 1)
                        {
                            string op = lst[i].OperationDesc;
                            int rowmerge = 0;
                            int rwstart1 = 0;
                            int rwend = 0;
                            oprnstartrow = row;
                            int freqstarthere = 0;
                            int freqendhere = 0;
                            //  string prev_pdtchar = string.Empty;
                            while (op == lst[i].OperationDesc)
                            {
                                Class_Temp_RptControlPlan c = lst[i];
                                string pdtchar = c.product_char;
                                if (row >= oprnstartrow)
                                {
                                    if (c.product_char == null || c.product_char == "-" || c.product_char == string.Empty)
                                    {
                                        if (flagforproduct == 0) { rwstart1 = row; }
                                        flagforproduct = 1;
                                        rwend = row;
                                    }
                                    else
                                    {
                                        if (flagforproduct == 1 && rwstart1 != 0 && rwend != 0)
                                        {
                                            ws.Cells[rwstart1, 5, rwend, 5].Merge = true;
                                            flagforproduct = 0;
                                        }
                                    }

                                }
                                row += 1;
                                i++;
                                if (i == lst.Count) break;
                            }
                            oprnendrow = row - 1;
                        }
                        // Logic to merge Product ends here 
                        // Logic to merge sample Freq and Sample Size when Sample Freq=100% starts(updated on 25th Jan 2024 )
                        row = partNumStartRow;
                        i = 0;
                        while (i <= lst.Count - 1)
                        {
                            Class_Temp_RptControlPlan c = lst[i];
                            if (!string.IsNullOrWhiteSpace(c.sampleFreq))
                            {
                                if (!string.IsNullOrWhiteSpace(c.sampleSize))
                                {
                                    if ((c.sampleFreq.Trim() == "100%" && (c.sampleSize.Trim() == "100%" || c.sampleSize.Trim() == "-")) || (c.sampleFreq.Trim() == "50%" && (c.sampleSize.Trim() == "50%" || c.sampleSize.Trim() == "-")))
                                    {
                                        if (!IsRangeMerged(ws, row, 11, row, 12))
                                        {
                                            ws.Cells[row, 11, row, 12].Merge = true;
                                        }
                                        ws.Cells[row, 11].Value = c.sampleFreq;
                                        ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }
                                else if (c.sampleFreq.Trim() == "100%" || c.sampleFreq.Trim() == "50%")
                                {
                                    if (!IsRangeMerged(ws, row, 11, row, 12))
                                    {
                                        ws.Cells[row, 11, row, 12].Merge = true;
                                    }
                                    ws.Cells[row, 11].Value = c.sampleFreq;
                                    ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(c.sampleFreq2))
                            {
                                if (!string.IsNullOrWhiteSpace(c.sampleSize2))
                                {
                                    if ((c.sampleFreq2.Trim() == "100%" && (c.sampleSize2.Trim() == "100%" || c.sampleSize2.Trim() == "-")) || (c.sampleFreq2.Trim() == "50%" && (c.sampleSize2.Trim() == "50%" || c.sampleSize2.Trim() == "-")))
                                    {
                                        if (!IsRangeMerged(ws, row, 15, row, 16))
                                        {
                                            ws.Cells[row, 15, row, 16].Merge = true;
                                        }
                                        ws.Cells[row, 15].Value = c.sampleFreq2;
                                        ws.Cells[row, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }
                                else if (c.sampleFreq2.Trim() == "100%" || c.sampleFreq2.Trim() == "50%")
                                {
                                    if (!IsRangeMerged(ws, row, 15, row, 16))
                                    {
                                        ws.Cells[row, 15, row, 16].Merge = true;
                                    }
                                    ws.Cells[row, 15].Value = c.sampleFreq2;
                                    ws.Cells[row, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }
                            }
                            row += 1;
                            i++;
                            if (i == lst.Count) break;
                        }
                        // Logic to merge sample Freq and Sample Size when Sample Freq=100% ends here

                        ws.Protection.AllowEditObject = false;

                        xlPackage.SaveAs(excelFile);

                        // reopen the file to adjust the row height
                        //  SetCellValuesAndAdjustRowHeight(excelFile.ToString(), startRow, endrow, revstart, revindex - 1);

                        // using syncfusion.xlsio to autofit rows
                        string filePath = excelFile.ToString();
                        string worksheetName = "Sheet1";
                        int startColumn = 1;
                        int endColumn = 17;
                        startRow += 1;

                        //AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn);
                        varcp = "CP";
                        AutofitRowHeight(filePath, worksheetName, 3, currentEndRow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        varcp = string.Empty;
                        rowvallength = 0;
                        rowvallengthpart = string.Empty;
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        //var ret = ConvertXlstoPdf(fname, pdfname);
                        //return pdfname;
                    }
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }
    }

    private bool IsRangeMerged(ExcelWorksheet ws, int startRow, int startColumn, int endRow, int endColumn)
    {
        foreach (var mergedRange in ws.MergedCells)
        {
            ExcelAddressBase mergedCells = new ExcelAddressBase(mergedRange);
            int mergedStartRow = mergedCells.Start.Row;
            int mergedStartColumn = mergedCells.Start.Column;
            int mergedEndRow = mergedCells.End.Row;
            int mergedEndColumn = mergedCells.End.Column;

            // Check if the proposed merge range overlaps with any existing merged range
            if (startRow <= mergedEndRow && endRow >= mergedStartRow &&
                startColumn <= mergedEndColumn && endColumn >= mergedStartColumn)
            {
                return true; // Overlap found
            }
        }
        return false; // No overlap
    }

    private void AutofitRowHeight(string filePath, string worksheetName, int startRow, int endRow, int startColumn, int endColumn, List<Class_Employees> userinfo, int rwhgt)
    {
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(filePath);
            IWorksheet worksheet = workbook.Worksheets[worksheetName];

            for (int row = startRow; row <= endRow; row++)
            {
                worksheet.AutofitRow(row);

            }
            workbook.Save();
            int defaultRowHt = 75;
            //if (varcp == "CP")
            //{
            //    for (int row = 1; row <= 8; row++)
            //    {
            //        worksheet.SetRowHeight(row, 75);
            //    }
            //    startRow = 9;
            //}
            for (int row = startRow; row <= endRow; row++)
            {

                double currentRowHt = worksheet.GetRowHeight(row);
                if (rwhgt != Convert.ToDouble(defaultRowHt))
                {

                    worksheet.SetRowHeight(row, rwhgt);
                }
                else
                    worksheet.SetRowHeight(row, (defaultRowHt));

            }
            if (varcp == "FOI")
            {
                if (rowAPPCasting > 0 && rowAPPMachining > 0)
                {
                    worksheet.SetRowHeight(rowAPPCasting, (defaultRowHt));
                    worksheet.SetRowHeight(rowAPPMachining, (defaultRowHt));
                }
            }
            bool isProtectWindow = true;
            bool isProtectContent = true;
            // Read the password from web.config
            string password = ConfigurationManager.AppSettings["xlpwd"];
            if (userinfo[0].isAdmin == "N")
            {
                //Protect Workbook
                workbook.Unprotect(password);
                //Protecting the Worksheet by using a Password
                worksheet.Protect(password);
                workbook.Protect(isProtectWindow, isProtectContent, password);
            }
            if (varcp == "FOI")
            {
                double currentRowHt = worksheet.GetRowHeight(5);
                double currentRowHt4 = worksheet.GetRowHeight(4);

                if (currentRowHt <= 75 && (rowvallength > 70 || FOIrevstatuscolumn > 70))
                {
                    worksheet.SetRowHeight(5, currentRowHt + 50);

                }
                if (currentRowHt4 <= 75 && (rowvallengthpart.Length > 70 || FOIOPcolumn > 70))
                {
                    worksheet.SetRowHeight(4, currentRowHt4 + 50);
                }

            }
            workbook.Save();
        }
    }
    private void AutofitRowHeight(string filePath, string worksheetName, int startRow, int endRow, int startColumn, int endColumn, List<Class_Employees> userinfo)
    {
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(filePath);
            IWorksheet worksheet = workbook.Worksheets[worksheetName];

            for (int row = startRow; row <= endRow; row++)
            {
                worksheet.AutofitRow(row);

            }

            workbook.Save();
            int defaultRowHt = 75;

            for (int row = startRow; row <= endRow; row++)
            {
                double currentRowHt = worksheet.GetRowHeight(row);
                if (currentRowHt < Convert.ToDouble(defaultRowHt))
                {
                    if (varcp == "CP")
                        worksheet.SetRowHeight(row, defaultRowHt);
                    else
                        worksheet.SetRowHeight(row, (defaultRowHt - 10));
                }

                //else if (currentRowHt == Convert.ToDouble(defaultRowHt))
                //{
                //    worksheet.SetRowHeight(row, defaultRowHt+10);
                //}
            }
            bool isProtectWindow = true;
            bool isProtectContent = true;
            // Read the password from web.config
            string password = ConfigurationManager.AppSettings["xlpwd"];
            if (userinfo[0].isAdmin == "N")
            {
                //Protect Workbook
                workbook.Unprotect(password);
                //Protecting the Worksheet by using a Password
                worksheet.Protect(password);
                workbook.Protect(isProtectWindow, isProtectContent, password);
            }
            if (varcp == "CP")
            {
                double currentRowHt = worksheet.GetRowHeight(5);
                double currentRowHt4 = worksheet.GetRowHeight(4);

                if (currentRowHt <= 75 && rowvallength > 70)
                {
                    worksheet.SetRowHeight(5, currentRowHt + 100);
                }
                if (currentRowHt4 <= 75 && rowvallengthpart.Length > 70)
                {
                    worksheet.SetRowHeight(4, currentRowHt4 + 100);
                }
            }
            workbook.Save();


        }
    }
    string GetFileName(string imgSlNo)
    {
        string sql = @"select splCharFile from SpecialChars where splchar_slno=" + imgSlNo;
        using (Database db = new Database("connString"))
        {
            string imgnm = db.ExecuteScalar<string>(sql);
            return imgnm;
        }
    }
    protected int Pixel2MTU(int pixels)
    {
        int mtus = pixels * 9525;
        return mtus;
    }
    protected void addimage(string imagename, int rowindex, int colindex, ExcelWorksheet ws, string pic)
    {
        string path = HttpContext.Current.Server.MapPath("~/Documents/" + imagename);
        System.Drawing.Image logo = System.Drawing.Image.FromFile(path);
        var picture = ws.Drawings.AddPicture(pic, logo);
        picture.From.Column = colindex;
        picture.From.Row = rowindex - 1;
        picture.SetSize(50, 50);
        picture.From.ColumnOff = Pixel2MTU(2);
        picture.From.RowOff = Pixel2MTU(10);
    }
    protected void addimagePDI(string imagename, int rowindex, int colindex, ExcelWorksheet ws, string pic)
    {
        string path = HttpContext.Current.Server.MapPath("~/Documents/" + imagename);
        System.Drawing.Image logo = System.Drawing.Image.FromFile(path);
        var picture = ws.Drawings.AddPicture(pic, logo);
        picture.From.Column = colindex;
        picture.From.Row = rowindex - 1;
        picture.SetSize(40, 40);
        picture.From.ColumnOff = Pixel2MTU(10);
        picture.From.RowOff = Pixel2MTU(2);
        if (imagename == "pdicheck.png")
        {
            picture.SetSize(80, 40);
            picture.From.ColumnOff = Pixel2MTU(20);
            picture.From.RowOff = Pixel2MTU(7);
        }
    }
    protected void addimagePCC(string imagename, int rowindex, int colindex, ExcelWorksheet ws, string pic)
    {
        string path = HttpContext.Current.Server.MapPath("~/Documents/" + imagename);
        System.Drawing.Image logo = System.Drawing.Image.FromFile(path);
        var picture = ws.Drawings.AddPicture(pic, logo);
        picture.From.Column = colindex;
        picture.From.Row = rowindex - 2;
        picture.SetSize(50, 50);
        picture.From.ColumnOff = Pixel2MTU(2);
        picture.From.RowOff = Pixel2MTU(10);
    }

    [WebMethod(EnableSession = true)]
    public string GenerateDoc(int partsl, int oprnsl, string rwhght)
    {
        int imgno = 0;
        try
        {
            string sqlrowheightinsert = "";
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "doc_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            //FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/DocAudit.xlsx"));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/DockAuditReport.xlsx"));
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";
            string sqlrevdtls = @"SELECT rev_no, rev_date, rev_reason FROM controlplan c
              where c.part_slno =" + partsl + " and c.operation_slno= " + oprnsl + "";
            sqlrowheightinsert = "update  controlplan set DOC_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";
            Logger.LogError(sql);
            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                db.Execute(sqlrowheightinsert);
                List<Class_ControlPlan> lstrev = db.Fetch<Class_ControlPlan>(sqlrevdtls);
                // string sqlcp = @"Select * from temp_rptControlPlan";
                //List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                string sqlcp = @"Select * from temp_rptControlPlan where Obsolete='N'";
                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0 and Obsolete='Y' ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }
                if (lst.Count > 0)
                {
                    double xlrowht = 90;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                        //   ws.Cells[1, 1].Value = ws.Cells[1, 1].Value + " - " + lst[0].PartDescription;
                        //   ws.Cells[1, 1].Style.WrapText = true;
                        ws.Cells[2, 1].Value = ws.Cells[2, 1].Value + " " + lst[0].PartDescription;
                        ws.Cells[2, 4].Value = ws.Cells[2, 4].Value + " ";
                        ws.Cells[2, 7].Value = ws.Cells[2, 7].Value + " ";
                        ws.Cells[2, 9].Value = "Customer Name:" + lst[0].Customer_name;
                        ws.Cells[3, 1].Value = ws.Cells[3, 1].Value + " " + lst[0].mstPartNo + " - " + lst[0].partIssueNo + " / " + lst[0].partIssueDt.Replace('/', '.');
                        ws.Cells[3, 1].Style.WrapText = true;
                        ws.Cells[2, 1].Style.WrapText = true;
                        ws.Cells[3, 4].Value = ws.Cells[3, 4].Value + " ";
                        ws.Cells[3, 7].Value = ws.Cells[3, 7].Value + " ";
                        ws.Cells[3, 10].Value = ws.Cells[3, 10].Value + " ";

                        //  ws.Cells[4, 1].Value = ws.Cells[4, 1].Value + " " + lst[0].mstPartNo + " - " + lst[0].partIssueNo + " / " + lst[0].partIssueDt.Replace('/', '.');
                        // ws.Cells[4, 4].Value = ws.Cells[4, 4].Value + " ";
                        // ws.Cells[4, 7].Value = ws.Cells[4, 7].Value + " ";
                        //ws.Cells[4, 10].Value = ws.Cells[4, 10].Value + " ";

                        int endrow = 6 + lst.Count;
                        ws.InsertRow(6, lst.Count);
                        int index = 6 + lst.Count + 2;
                        int row = 6;
                        int startRow = 5;
                        string imgPath = Server.MapPath("~/Documents/");
                        int serial_no = 1;
                        ws.Cells[index, 1].Value = "Inspected By: ";
                        ws.Cells[index, 8].Value = "Approved By: ";
                        foreach (Class_Temp_RptControlPlan c in lst)
                        {
                            ws.Cells[startRow + ":" + startRow].Copy(ws.Cells[row + ":" + row]);
                            ws.Cells[row, 1].Value = serial_no;
                            ws.Cells[row, 2].Value = c.product_char;
                            if (c.splChar_slno > 0)
                            {
                                string img = GetFileName(c.splChar_slno.ToString());
                                string pth = (imgPath + img);
                                FileInfo imgFile = new FileInfo(pth);
                                if (imgFile.Exists)
                                {
                                    imgno += 1;
                                    Bitmap image = new Bitmap(pth);
                                    ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                    ExcelRangeBase cell = ws.Cells[row - 2, 2];
                                    if (row == 7)
                                    {
                                        excelPicture.SetPosition(cell.Start.Row, 18, cell.Start.Column, 12);
                                        excelPicture.SetSize(46, 46);
                                    }
                                    else
                                    {
                                        excelPicture.SetPosition(cell.Start.Row, 28, cell.Start.Column, 13);
                                        excelPicture.SetSize(50, 50);
                                    }
                                }
                            }
                            ws.Cells[row, 4].Value = c.spec1;
                            ws.Cells[row, 5].Value = c.measurementTech;
                            ws.Cells[row, 6].Value = c.sampleSize + " " + c.sampleFreq;
                            string[] cp = c.cp_number.Split('/');
                            ws.Cells[4 + lst.Count + 5, 1].Value = "F/Q/012 - Rev : 0";
                            ws.Cells[4 + lst.Count + 5, 8].Value = cp[1] + cp[2] + " / " + c.process_no + "/DAR - Rev No:0";
                            ws.Row(row).Height = xlrowht;
                            ws.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            //// ---------- added to set the row height
                            //// Find the cell with the maximum content length
                            //ExcelRangeBase cellWithMaxContent = FindCellWithMaxContent(row, ws);

                            //// Calculate the required row height based on the cell content
                            //int requiredRowHeight = GetRequiredRowHeight(ws, cellWithMaxContent);

                            //// Set the row height to the calculated required height (if it's greater than the default height)
                            //int finalRowHeight = (int)Math.Max(ws.DefaultRowHeight, requiredRowHeight);
                            //ws.Row(cellWithMaxContent.Start.Row).Height = finalRowHeight;
                            //// ---------------
                            row += 1;
                            serial_no++;
                        }

                        ws.DeleteRow(5);
                        int revindex = 5 + lst.Count + 7;
                        //if (lstrev.Count > 0)
                        //{

                        //    foreach (Class_ControlPlan c in lstrev)
                        //    {
                        //        ws.Cells[revindex, 1].Value = c.rev_no;
                        //        ws.Cells[revindex, 2].Value = c.rev_date;
                        //        ws.Cells[revindex, 3].Value = c.rev_reason;
                        //        revindex += 1;
                        //    }
                        //}

                        // set the print area
                        int newRowCount = revindex;
                        //int currentEndRow = ws.Dimension.End.Row;
                        int currentEndRow = newRowCount;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:I" + (currentEndRow)];
                        ws.Protection.AllowEditObject = false;
                        xlPackage.SaveAs(excelFile);
                        // using syncfusion.xlsio to autofit rows
                        string filePath = excelFile.ToString();
                        string worksheetName = "Sheet1";
                        int startColumn = 1;
                        int endColumn = 12;
                        // startRow = 5;
                        startRow = 2;

                        // AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        // var ret = ConvertXlstoPdf(fname, pdfname);
                        //return pdfname;
                    }
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GeneratePDI_2PerSkid(int partsl, int oprnsl, string rwhght)
    {

        string filePath = "";
        string worksheetName = "";
        int startColumn = 0;
        int endColumn = 0;
        int startRow = 0;
        int endrow = 0;
        string pdfname = "";
        var ret = "";
        int imgno = 0;
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "PDI_2Perskid_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/PDI_Checksheet2PerSkid.xlsx"));
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl + " and cp.operation_slno =" + oprnsl + " and sp.show_in_legend = 1";
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no=(SELECT MAX( CAST(p.rev_no AS INT)) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl;


            Logger.LogError(sql);
            string cp_slno = string.Empty;
            pdfname = fname.Replace(".xlsx", ".pdf");
            using (Database db = new Database("connString"))
            {
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                string sqlcp_chksheet_top = @"Select * from temp_rptControlPlan  where sampleFreq='Per Skid' and obsolete='N'  order by cast(process_no as decimal) ";
                List<Class_Temp_RptControlPlan> lsttop = db.Fetch<Class_Temp_RptControlPlan>(sqlcp_chksheet_top);
                if (lsttop.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where  sampleFreq='Per Skid'    and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp_chksheet_top = "Select * from temp_rptControlPlan where  cp_slno=@0 and sampleFreq='Per Skid'    and  Obsolete='Y'";
                    lsttop = db.Fetch<Class_Temp_RptControlPlan>(sqlcp_chksheet_top, cp);

                }
                ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile);
                ExcelWorksheet ws_pdichecksheet = xlPackage.Workbook.Worksheets[1];
                int rowbottom = 0;
                int row = 7;
                int ind = 1;
                if (lsttop.Count > 0)
                {
                    double xlrowht = 50;
                    ws_pdichecksheet.Cells[2, 1].Value = "Part Name:" + lsttop[0].PartDescription;
                    ws_pdichecksheet.Cells[2, 5].Value = "Part No:" + lsttop[0].mstPartNo + " - " + lsttop[0].partIssueNo + " / " + lsttop[0].partIssueDt.Replace('/', '.');
                    ws_pdichecksheet.Cells[2, 9].Value = "Customer Name:" + lsttop[0].Customer_name;
                    int rowtop = 5;
                    int startRowtop = 5;
                    int serialno_top = 1;
                    ws_pdichecksheet.InsertRow(6, lsttop.Count - 1);
                    string imgPathtop = Server.MapPath("~/Documents/");
                    foreach (Class_Temp_RptControlPlan ctop in lsttop)
                    {
                        ws_pdichecksheet.Cells[startRowtop + ":" + startRowtop].Copy(ws_pdichecksheet.Cells[rowtop + ":" + rowtop]);
                        ws_pdichecksheet.Cells[rowtop, 1].Value = serialno_top;
                        ws_pdichecksheet.Cells[rowtop, 2].Value = ctop.product_char;
                        ws_pdichecksheet.Cells[rowtop, 2].Style.WrapText = true;
                        ws_pdichecksheet.Row(rowtop).Height = 50;
                        if (ctop.splChar_slno > 0)
                        {
                            string img = GetFileName(ctop.splChar_slno.ToString());
                            string pth = (imgPathtop + img);
                            FileInfo imgFile = new FileInfo(pth);
                            if (imgFile.Exists)
                            {
                                imgno += 1;
                                Bitmap image = new Bitmap(pth);
                                ExcelPicture excelPicture = ws_pdichecksheet.Drawings.AddPicture("chkPictureName" + imgno, image);
                                ExcelRangeBase cell = ws_pdichecksheet.Cells[(rowtop - 1), 2];
                                excelPicture.SetPosition(cell.Start.Row, 15, cell.Start.Column, 13);
                                excelPicture.SetSize(40, 40);
                            }
                        }
                        ws_pdichecksheet.Cells[rowtop, 4].Value = ctop.spec1;
                        ws_pdichecksheet.Cells[rowtop, 5].Value = ctop.measurementTech;
                        ws_pdichecksheet.Cells[rowtop, 6].Value = ctop.sampleFreq;
                        ws_pdichecksheet.Cells["G" + rowtop + ":H" + rowtop].Merge = true;
                        ws_pdichecksheet.Cells[rowtop, 7].Value = ctop.sampleSize;
                        rowtop += 1;//9
                        serialno_top++;
                    }
                    // ws_pdichecksheet.DeleteRow(5);
                    rowbottom = rowtop;//9

                    ws_pdichecksheet.Cells["A" + rowtop + ":H" + rowtop].Merge = true;
                    // ws_pdichecksheet.Cells["I" + rowtop + ":Q" + rowtop].Merge = true;
                    ws_pdichecksheet.Cells["A" + (rowtop + 1) + ":H" + (rowtop + 1)].Merge = true;
                    ws_pdichecksheet.Cells["I" + (rowtop + 1) + ":Q" + (rowtop + 1)].Merge = true;
                    ws_pdichecksheet.Cells["A" + (rowtop + 2) + ":N" + (rowtop + 2)].Merge = true;
                    ws_pdichecksheet.Cells["O" + (rowtop + 2) + ":Q" + (rowtop + 2)].Merge = true;
                    ws_pdichecksheet.Cells[rowtop, 1].Value = "Inspected By (PDI Inspector):";
                    ws_pdichecksheet.Cells[(rowtop + 1), 1].Value = "Verified By (PDI Supervisor):";
                    ws_pdichecksheet.Cells[rowtop, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws_pdichecksheet.Cells[(rowtop + 1), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws_pdichecksheet.Cells[(rowtop + 2), 1].Value = "F / Q / 014 - Rev: 0";
                    string[] cp = lsttop[0].cp_number.Split('/');
                    ws_pdichecksheet.Cells[(rowtop + 2), 15].Value = cp[1] + cp[2] + " / " + lsttop[0].process_no + "/PDI - Rev No:0";

                    if (lsttop.Count == 0)
                    {
                        ws_pdichecksheet.Cells[5, 2].Value = "NO RECORDS FOUND.";
                        ws_pdichecksheet.Cells["A" + (6) + ":H" + (6)].Merge = true;
                        // ws_pdichecksheet.Cells["I" + (6) + ":Q" + (6)].Merge = true;
                        ws_pdichecksheet.Cells["A" + (7) + ":H" + (7)].Merge = true;
                        ws_pdichecksheet.Cells["I" + (7) + ":Q" + (7)].Merge = true;
                        ws_pdichecksheet.Cells["A" + (8) + ":N" + (8)].Merge = true;
                        ws_pdichecksheet.Cells["O" + (8) + ":Q" + (8)].Merge = true;
                        ws_pdichecksheet.Cells[(6), 1].Value = "Inspected By (PDI Inspector):";
                        ws_pdichecksheet.Cells[(7), 1].Value = "Verified By (PDI Supervisor):";
                        ws_pdichecksheet.Cells[(6), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        ws_pdichecksheet.Cells[(7), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        ws_pdichecksheet.Cells[(8), 1].Value = "F / Q / 014 - Rev: 0";
                    }






                    int currentEndRowchk = ws_pdichecksheet.Dimension.End.Row;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Font.Size = 18;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    ws_pdichecksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws_pdichecksheet.PrinterSettings.PrintArea = ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk];


                    ws_pdichecksheet.Protection.AllowEditObject = false;

                    xlPackage.SaveAs(excelFile);
                    filePath = excelFile.ToString();
                    worksheetName = "Sheet1";
                    startColumn = 1;
                    endColumn = 12;
                    startRow = 2;
                    endrow = 6 + lsttop.Count;
                    AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                    pdfname = fname.Replace(".xlsx", ".pdf");
                    //ret = ConvertXlstoPdf(fname, pdfname);
                    //return pdfname;
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }
    }
    [WebMethod(EnableSession = true)]
    public string GeneratePDI_EveryHour(int partsl, int oprnsl, string rwhght)
    {

        string filePath = "";
        string worksheetName = "";
        int startColumn = 0;
        int endColumn = 0;
        int startRow = 0;
        int endrow = 0;
        string pdfname = "";
        var ret = "";
        int imgno = 0;
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "PDI_EveryHour_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/PDI_ChecksheetEveryHour.xlsx"));
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl + " and cp.operation_slno =" + oprnsl + " and sp.show_in_legend = 1";
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no=(SELECT MAX(CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl;


            Logger.LogError(sql);
            string cp_slno = string.Empty;
            pdfname = fname.Replace(".xlsx", ".pdf");
            using (Database db = new Database("connString"))
            {

                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);

                string sqlcp_chksheet_bottom = @"Select * from temp_rptControlPlan  where sampleFreq='Every Hour'   and obsolete='N' order by cast(process_no as decimal) ";

                List<Class_Temp_RptControlPlan> lstbottom = db.Fetch<Class_Temp_RptControlPlan>(sqlcp_chksheet_bottom);

                if (lstbottom.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where sampleFreq='Every Hour'    and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp_chksheet_bottom = "Select * from temp_rptControlPlan where  cp_slno=@0 and sampleFreq='Every Hour'   and  Obsolete='Y'";
                    lstbottom = db.Fetch<Class_Temp_RptControlPlan>(sqlcp_chksheet_bottom, cp);

                }

                ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile);
                ExcelWorksheet ws_pdichecksheet = xlPackage.Workbook.Worksheets[1];
                int rowbottom = 3;
                int row = 7;
                int ind = 1;
                if (lstbottom.Count > 0)
                {
                    ws_pdichecksheet.Cells[2, 1].Value = "Part Name:" + lstbottom[0].PartDescription;
                    ws_pdichecksheet.Cells[2, 5].Value = "Part No:" + lstbottom[0].mstPartNo + " - " + lstbottom[0].partIssueNo + " / " + lstbottom[0].partIssueDt.Replace('/', '.');
                    ws_pdichecksheet.Cells[2, 9].Value = "Customer Name:" + lstbottom[0].Customer_name;
                    double xlrowht = 50;

                    int sum = 0;
                    string imgPathbottom = Server.MapPath("~/Documents/");
                    int serialno_bottom = 1;
                    //ws_pdichecksheet.Cells["A" + (rowbottom) + ":A" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["A" + (rowbottom)].Value = "SI. No";
                    //ws_pdichecksheet.Cells["B" + (rowbottom) + ":B" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["B" + (rowbottom)].Value = "Description";
                    //ws_pdichecksheet.Cells["C" + (rowbottom) + ":C" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["C" + (rowbottom)].Value = "Spl. Char.";
                    //ws_pdichecksheet.Cells["D" + (rowbottom) + ":D" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["D" + (rowbottom)].Value = "Specification";
                    //ws_pdichecksheet.Cells["E" + (rowbottom) + ":E" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["E" + (rowbottom)].Value = "Method of Check";
                    //ws_pdichecksheet.Cells["F" + (rowbottom) + ":F" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["F" + (rowbottom)].Value = "Frequency";

                    //ws_pdichecksheet.Cells["G" + (rowbottom) + ":H" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["G" + (rowbottom)].Value = "Sample";
                    //ws_pdichecksheet.Cells["I" + (rowbottom) + ":P" + rowbottom].Merge = true;
                    //ws_pdichecksheet.Cells["I" + (rowbottom)].Value = "Hour";
                    //ws_pdichecksheet.Cells["Q" + (rowbottom) + ":Q" + (rowbottom + 1)].Merge = true;
                    //ws_pdichecksheet.Cells["Q" + (rowbottom)].Value = "Remarks";
                    //ws_pdichecksheet.Cells["I" + (rowbottom + 1)].Value = "1";
                    //ws_pdichecksheet.Cells["J" + (rowbottom + 1)].Value = "2";
                    //ws_pdichecksheet.Cells["K" + (rowbottom + 1)].Value = "3";
                    //ws_pdichecksheet.Cells["L" + (rowbottom + 1)].Value = "4";
                    //ws_pdichecksheet.Cells["M" + (rowbottom + 1)].Value = "5";
                    //ws_pdichecksheet.Cells["N" + (rowbottom + 1)].Value = "6";
                    //ws_pdichecksheet.Cells["O" + (rowbottom + 1)].Value = "7";
                    //ws_pdichecksheet.Cells["P" + (rowbottom + 1)].Value = "8";
                    //ws_pdichecksheet.Cells["I" + (rowbottom + 1) + ":P" + (rowbottom + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["I" + (rowbottom + 1) + ":P" + (rowbottom + 1)].Style.VerticalAlignment
                    //    = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["A" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["A" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["B" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["C" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["C" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["D" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["D" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["E" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["E" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["F" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["F" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["G" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["G" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["I" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["I" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //ws_pdichecksheet.Cells["Q" + (rowbottom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws_pdichecksheet.Cells["Q" + (rowbottom)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    foreach (Class_Temp_RptControlPlan cbottom in lstbottom)
                    {
                        ws_pdichecksheet.Cells["A" + (rowbottom + 2)].Value = serialno_bottom;
                        ws_pdichecksheet.Cells["B" + (rowbottom + 2)].Value = cbottom.product_char;
                        if (cbottom.splChar_slno > 0)
                        {
                            string img = GetFileName(cbottom.splChar_slno.ToString());
                            string pth = (imgPathbottom + img);
                            FileInfo imgFile = new FileInfo(pth);
                            if (imgFile.Exists)
                            {
                                imgno += 1;
                                Bitmap image = new Bitmap(pth);
                                ExcelPicture excelPicture = ws_pdichecksheet.Drawings.AddPicture("chkPictureName" + imgno, image);
                                ExcelRangeBase cell = ws_pdichecksheet.Cells[(rowbottom + 1), 2];
                                excelPicture.SetPosition(cell.Start.Row, 25, cell.Start.Column, 13);
                                excelPicture.SetSize(40, 40);

                            }
                        }
                        ws_pdichecksheet.Cells["D" + (rowbottom + 2)].Value = cbottom.spec1;
                        ws_pdichecksheet.Cells["E" + (rowbottom + 2)].Value = cbottom.measurementTech;
                        ws_pdichecksheet.Cells["E" + (rowbottom + 2)].Style.WrapText = true;
                        ws_pdichecksheet.Cells["F" + (rowbottom + 2)].Value = cbottom.sampleFreq;

                        ws_pdichecksheet.Cells["A" + (rowbottom + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws_pdichecksheet.Cells["A" + (rowbottom + 2)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws_pdichecksheet.Cells["B" + (rowbottom + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws_pdichecksheet.Cells["B" + (rowbottom + 2)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws_pdichecksheet.Cells["D" + (rowbottom + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws_pdichecksheet.Cells["D" + (rowbottom + 2)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        ws_pdichecksheet.Cells["E" + (rowbottom + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws_pdichecksheet.Cells["E" + (rowbottom + 2)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        ws_pdichecksheet.Cells["F" + (rowbottom + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws_pdichecksheet.Cells["F" + (rowbottom + 2)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;



                        ws_pdichecksheet.Cells["A" + (rowbottom + 2) + ":A" + ((rowbottom + 2) + 19)].Merge = true;
                        ws_pdichecksheet.Cells["B" + (rowbottom + 2) + ":B" + ((rowbottom + 2) + 19)].Merge = true;
                        ws_pdichecksheet.Cells["C" + (rowbottom + 2) + ":C" + ((rowbottom + 2) + 19)].Merge = true;
                        ws_pdichecksheet.Cells["D" + (rowbottom + 2) + ":D" + ((rowbottom + 2) + 19)].Merge = true;
                        ws_pdichecksheet.Cells["E" + (rowbottom + 2) + ":E" + ((rowbottom + 2) + 19)].Merge = true;
                        ws_pdichecksheet.Cells["F" + (rowbottom + 2) + ":F" + ((rowbottom + 2) + 19)].Merge = true;


                        ws_pdichecksheet.Cells["G" + (rowbottom + 2) + ":G" + (rowbottom + 3)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 2)].Value = "1";

                        ws_pdichecksheet.Cells["H" + (rowbottom + 2)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 3)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 4) + ":G" + (rowbottom + 5)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 4)].Value = "2";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 4)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 5)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 6) + ":G" + (rowbottom + 7)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 6)].Value = "3";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 6)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 7)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 8) + ":G" + (rowbottom + 9)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 8)].Value = "4";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 8)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 9)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 10) + ":G" + (rowbottom + 11)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 10)].Value = "5";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 10)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 11)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 12) + ":G" + (rowbottom + 13)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 12)].Value = "6";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 12)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 13)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 14) + ":G" + (rowbottom + 15)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 14)].Value = "7";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 14)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 15)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 16) + ":G" + (rowbottom + 17)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 16)].Value = "8";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 16)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 17)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 18) + ":G" + (rowbottom + 19)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 18)].Value = "9";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 18)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 19)].Value = "Max";
                        ws_pdichecksheet.Cells["G" + (rowbottom + 20) + ":G" + (rowbottom + 21)].Merge = true;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 20)].Value = "10";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 20)].Value = "Min";
                        ws_pdichecksheet.Cells["H" + (rowbottom + 21)].Value = "Max";


                        ws_pdichecksheet.Cells["G" + (rowbottom + 2) + ":G" + (rowbottom + 20)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws_pdichecksheet.Cells["G" + (rowbottom + 2) + ":G" + (rowbottom + 20)].Style.VerticalAlignment
                            = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws_pdichecksheet.Cells["H" + (rowbottom + 2) + ":H" + (rowbottom + 21)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws_pdichecksheet.Cells["H" + (rowbottom + 2) + ":H" + (rowbottom + 21)].Style.VerticalAlignment
                            = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        sum = rowbottom + 21;
                        //int x = sum - rowbottom;
                        rowbottom = (sum + 1) - 2;
                        serialno_bottom += 1;
                    }

                    ws_pdichecksheet.Cells["A" + (sum + 1) + ":H" + (sum + 1)].Merge = true;
                    // ws_pdichecksheet.Cells["I" + (sum + 1) + ":Q" + (sum + 1)].Merge = true;
                    ws_pdichecksheet.Cells["A" + (sum + 2) + ":H" + (sum + 2)].Merge = true;
                    ws_pdichecksheet.Cells["I" + (sum + 2) + ":Q" + (sum + 2)].Merge = true;
                    ws_pdichecksheet.Cells["A" + (sum + 3) + ":N" + (sum + 3)].Merge = true;
                    ws_pdichecksheet.Cells["O" + (rowbottom + 24) + ":Q" + (rowbottom + 24)].Merge = true;
                    ws_pdichecksheet.Cells[(sum + 1), 1].Value = "Inspected By (PDI Inspector):";
                    ws_pdichecksheet.Cells[(sum + 2), 1].Value = "Verified By (PDI Supervisor):";
                    ws_pdichecksheet.Cells[(sum + 1), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws_pdichecksheet.Cells[(sum + 2), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws_pdichecksheet.Cells[(sum + 3), 1].Value = "F / Q / 014 - Rev: 0";
                    string[] cp = lstbottom[0].cp_number.Split('/');
                    ws_pdichecksheet.Cells[(sum + 3), 15].Value = cp[1] + cp[2] + " / " + lstbottom[0].process_no + "/PDI - Rev No:0";

                    if (lstbottom.Count == 0)
                    {
                        ws_pdichecksheet.Cells[3, 2].Value = "NO RECORDS FOUND.";
                        ws_pdichecksheet.Cells["A" + (4) + ":H" + (4)].Merge = true;
                        // ws_pdichecksheet.Cells["I" + (sum + 1) + ":Q" + (sum + 1)].Merge = true;
                        ws_pdichecksheet.Cells["A" + (5) + ":H" + (5)].Merge = true;
                        ws_pdichecksheet.Cells["I" + (5) + ":Q" + (5)].Merge = true;
                        ws_pdichecksheet.Cells["A" + (6) + ":N" + (6)].Merge = true;
                        ws_pdichecksheet.Cells["O" + (6) + ":Q" + (6)].Merge = true;
                        ws_pdichecksheet.Cells[(4), 1].Value = "Inspected By (PDI Inspector):";
                        ws_pdichecksheet.Cells[(5), 1].Value = "Verified By (PDI Supervisor):";
                        ws_pdichecksheet.Cells[(4), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        ws_pdichecksheet.Cells[(5), 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        ws_pdichecksheet.Cells[(6), 1].Value = "F / Q / 014 - Rev: 0";
                    }

                    int currentEndRowchk = ws_pdichecksheet.Dimension.End.Row;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Font.Size = 18;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    ws_pdichecksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws_pdichecksheet.PrinterSettings.PrintArea = ws_pdichecksheet.Cells["A1:Q" + currentEndRowchk];


                    ws_pdichecksheet.Protection.AllowEditObject = false;

                    xlPackage.SaveAs(excelFile);
                    filePath = excelFile.ToString();
                    worksheetName = "Sheet1";
                    startColumn = 1;
                    endColumn = 12;
                    startRow = 5;
                    endrow = 2 + (lstbottom.Count * 20) + 5;
                    AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                    pdfname = fname.Replace(".xlsx", ".pdf");
                    // ret = ConvertXlstoPdf(fname, pdfname);
                    // return pdfname;
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GeneratePDISummary(int partsl, int oprnsl, string rwhght)
    {

        string sqlrowheightinsert = "";
        string filePath = "";
        string worksheetName = "";
        int startColumn = 0;
        int endColumn = 0;
        int startRow = 0;
        int endrow = 0;

        var ret = "";
        int imgno = 0;
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "PDI_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/PDI_Summary.xlsx"));
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl + " and cp.operation_slno =" + oprnsl + " and sp.show_in_legend = 1";
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no=(SELECT MAX( CAST(p.rev_no AS INT)) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and  c.Obsolete='N' and  c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl;
            sqlrowheightinsert = "update  controlplan set PDI_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";

            Logger.LogError(sql);
            string cp_slno = string.Empty;
            // pdfname = fname.Replace(".xlsx", ".pdf");
            using (Database db = new Database("connString"))
            {
                db.Execute(sqlrowheightinsert);
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                //string sqlcp = @"Select * from temp_rptControlPlan where sampleFreq='Per Skid' or sampleFreq='100%' order by cast(process_no as decimal) ";
                //string sqlcp = @"Select * from temp_rptControlPlan where sampleFreq='Per Skid' or sampleFreq='100%' or sampleFreq='50%' or ( sampleSize like '%5%' and sampleFreq='Every Hour')  order by cast(process_no as decimal) ";

                //Commented above query and added below query as per customer requirement on 25_01_2025 
                string sqlcp = @"Select * from temp_rptControlPlan where Obsolete='N'";
                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0 and Obsolete='Y'  ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp,cp);

                }


                int rowbottom = 0;
                int row = 7;
                int ind = 1;
                if (lst.Count > 0)
                {
                    double xlrowht = 50;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                        if (lst.Count > 0)
                        {
                            ws.Cells[2, 1].Value = "Part Name:" + lst[0].PartDescription;
                            ws.Cells[2, 3].Value = "Part No:" + lst[0].mstPartNo + " - " + lst[0].partIssueNo + " / " + lst[0].partIssueDt.Replace('/', '.');
                            ws.Cells[2, 5].Value = "Customer Name:" + lst[0].Customer_name;
                            //int row = 7;
                            ws.InsertRow(7, lst.Count);
                            int startRows = 6;
                            string imgPath = Server.MapPath("~/Documents/");
                            int serial_no = 1;
                            int index = (row + (lst.Count) + 2);
                            int legendcount = lstlegend.Count;
                            int flag1 = 0;

                            ws.Cells[2, 1].Style.WrapText = true;
                            ws.Cells[2, 3].Style.WrapText = true;
                            if (lstlegend.Count > 0)
                            {
                                if (!string.IsNullOrWhiteSpace(lstlegend[0].splCharFile))
                                {
                                    addimagePDI(lstlegend[0].splCharFile, index - 1, 4, ws, "0");
                                }
                                if (lstlegend.Count > 1)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstlegend[1].splCharFile))
                                    {
                                        addimagePDI(lstlegend[1].splCharFile, index - 1, 8, ws, "1");
                                    }
                                }
                            }
                            foreach (Class_Temp_RptControlPlan c in lst)
                            {
                                if (c.product_char.Contains("Casting") && c.product_char.Contains("Appearance"))
                                {
                                    //ws.InsertRow(row+1, 1);
                                    ws.Cells[row, 1, row, 12].Merge = true;
                                    ws.Cells[row, 1].Value = "SUPPLIER DEFECTS: ";
                                    ws.Cells[row, 1].Style.Font.Bold = true;
                                    ws.Row(row).Height = 10;
                                    flag1 = 1;
                                }
                                if (flag1 == 0)
                                {
                                    ws.Cells[startRows + ":" + startRows].Copy(ws.Cells[row + ":" + row]);
                                    ws.Cells[row, 1].Value = serial_no;
                                    ws.Cells[row, 2].Value = c.product_char;
                                    ws.Row(row).Height = 50;
                                    if (c.splChar_slno > 0)
                                    {
                                        string img = GetFileName(c.splChar_slno.ToString());
                                        string pth = (imgPath + img);
                                        FileInfo imgFile = new FileInfo(pth);
                                        if (imgFile.Exists)
                                        {
                                            imgno += 1;
                                            Bitmap image = new Bitmap(pth);
                                            ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                            ExcelRangeBase cell = ws.Cells[row - 2, 2];
                                            excelPicture.SetPosition(cell.Start.Row, 25, cell.Start.Column, 13);
                                            if (row == 7)
                                            {
                                                excelPicture.SetSize(40, 20);
                                            }
                                            else
                                            {
                                                excelPicture.SetSize(40, 40);
                                            }
                                        }
                                    }
                                    ws.Cells[row, 4].Value = c.spec1;
                                    ws.Cells[row, 5].Value = c.measurementTech;
                                    ws.Cells[row, 6].Value = c.sampleSize + " / " + c.sampleFreq;
                                    if (!string.IsNullOrEmpty(c.sampleSize))
                                    {
                                        if (c.sampleSize.Contains("%") && (c.sampleFreq == "-" || c.sampleFreq == string.Empty || c.sampleFreq == null))
                                        { ws.Cells[row, 6].Value = c.sampleSize; }
                                    }
                                    //if (c.sampleFreq == "100%" || c.sampleFreq == "50%" || (c.sampleFreq == "Every Hour"))
                                    if (c.sampleFreq == "100%" || c.sampleFreq == "50%")
                                    {
                                        ws.Cells[row, 6].Value = c.sampleFreq;
                                    }
                                    else if (c.sampleFreq == "Per Skid" && (c.sampleSize != string.Empty || !c.sampleSize.Contains("%") || c.sampleSize != null || c.sampleSize != "-"))
                                    {
                                        ws.Cells[row, 6].Value = c.sampleSize + " No's / " + c.sampleFreq;
                                        ws.Cells[row, 12].Value = "Check & Update PDI Check Sheet";
                                    }
                                    else if (c.sampleFreq == "Every Hour" && (c.sampleSize != string.Empty || !c.sampleSize.Contains("%") || c.sampleSize != null || c.sampleSize != "-") && c.sampleSize == "5")
                                    {
                                        ws.Cells[row, 6].Value = c.sampleSize + " / " + c.sampleFreq;
                                        ws.Cells[row, 12].Value = "Check & Update PDI Check Sheet";
                                    }
                                    if (c.PDI_type == "Variable")
                                    {
                                        ws.Cells[row, 9].Value = string.Empty;
                                        ws.Cells[row, 10].Value = string.Empty;
                                        ws.Cells[row, 11].Value = "N/A";
                                        ws.Cells[row, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        ws.Cells[row, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    }
                                    else if (c.PDI_type == "Attribute")
                                    {
                                        ws.Cells[row, 9].Value = "N/A";
                                        ws.Cells[row, 10].Value = "N/A";
                                        ws.Cells[row, 11].Value = string.Empty;
                                        ws.Cells[row, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[row, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[row, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        ws.Cells[row, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                    ws.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    ws.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row, 2].Style.WrapText = true;
                                    ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row, 4].Style.WrapText = true;
                                    ws.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    row += 1;
                                    serial_no++;
                                }
                                ws.Cells["A6:L" + row].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                ws.Cells["A6:L" + row].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells["A6:L" + row].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                if (flag1 == 1)
                                {
                                    ws.Cells[row + 1, 1].Value = serial_no;
                                    ws.Cells[row + 1, 2].Value = c.product_char;
                                    if (c.splChar_slno > 0)
                                    {
                                        string img = GetFileName(c.splChar_slno.ToString());
                                        string pth = (imgPath + img);
                                        FileInfo imgFile = new FileInfo(pth);
                                        if (imgFile.Exists)
                                        {
                                            imgno += 1;
                                            Bitmap image = new Bitmap(pth);
                                            ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                            ExcelRangeBase cell = ws.Cells[row - 1, 2];
                                            excelPicture.SetPosition(cell.Start.Row, 10, cell.Start.Column, 13);
                                            excelPicture.SetSize(40, 40);
                                        }
                                    }
                                    ws.Cells[row + 1, 4].Value = c.spec1;
                                    ws.Cells[row + 1, 5].Value = c.measurementTech;
                                    ws.Cells[row + 1, 6].Value = c.sampleSize + " / " + c.sampleFreq;
                                    if (!string.IsNullOrEmpty(c.sampleSize))
                                    {
                                        if (c.sampleSize.Contains("%") && (c.sampleFreq == "-" || c.sampleFreq == string.Empty || c.sampleFreq == null))
                                        { ws.Cells[row + 1, 6].Value = c.sampleSize; }
                                    }
                                    // if (c.sampleFreq == "100%" || c.sampleFreq == "50%" ||  (c.sampleFreq == "Every Hour"))
                                    if (c.sampleFreq == "100%" || c.sampleFreq == "50%")
                                    {
                                        ws.Cells[row + 1, 6].Value = c.sampleFreq;
                                    }
                                    else if (c.sampleFreq == "Per Skid" && (c.sampleSize != string.Empty || !c.sampleSize.Contains("%") || c.sampleSize != null || c.sampleSize != "-"))
                                    {
                                        ws.Cells[row + 1, 6].Value = c.sampleSize + " No's/ " + c.sampleFreq;
                                        ws.Cells[row + 1, 12].Value = "Check & Update PDI Check Sheet";
                                    }
                                    else if (c.sampleFreq == "Every Hour" && (c.sampleSize != string.Empty || !c.sampleSize.Contains("%") || c.sampleSize != null || c.sampleSize != "-") && c.sampleSize == "5")
                                    {
                                        ws.Cells[row + 1, 6].Value = c.sampleSize + " / " + c.sampleFreq;
                                        ws.Cells[row + 1, 12].Value = "Check & Update PDI Check Sheet";
                                    }
                                    if (c.PDI_type == "Variable")
                                    {
                                        ws.Cells[row + 1, 9].Value = string.Empty;
                                        ws.Cells[row + 1, 10].Value = string.Empty;
                                        ws.Cells[row + 1, 11].Value = "N/A";
                                        ws.Cells[row + 1, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        ws.Cells[row + 1, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    }
                                    else if (c.PDI_type == "Attribute")
                                    {
                                        ws.Cells[row + 1, 9].Value = "N/A";
                                        ws.Cells[row + 1, 10].Value = "N/A";
                                        ws.Cells[row + 1, 11].Value = string.Empty;
                                        ws.Cells[row + 1, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[row + 1, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[row + 1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        ws.Cells[row + 1, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                    ws.Cells[row + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[row + 1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row + 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    ws.Cells[row + 1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row + 1, 2].Style.WrapText = true;
                                    ws.Cells[row + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    ws.Cells[row + 1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row + 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[row + 1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    ws.Cells[row + 1, 4].Style.WrapText = true;
                                    row += 1;
                                    serial_no++;
                                }
                            }
                            ws.DeleteRow(6);
                        }
                        else
                        {
                            ws.DeleteRow(6);
                            ws.Cells[6, 2].Value = "NO RECORDS FOUND.";
                        }


                        if (lst.Count > 0)
                        {
                            string[] cp = lst[0].cp_number.Split('/');
                            ws.Cells[row + 5, 10].Value = cp[1] + cp[2] + " / " + lst[0].process_no + "/PDS - Rev No:0";
                        }

                        addimagePDI("pdicheck.png", (row + 2), 3, ws, "2");
                        ws.Cells["A" + row].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        int currentEndRow = ws.Dimension.End.Row;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:L" + currentEndRow];

                        ws.PrinterSettings.Orientation = eOrientation.Landscape;


                        ws.Protection.AllowEditObject = false;


                        xlPackage.SaveAs(excelFile);
                        filePath = excelFile.ToString();
                        worksheetName = "Sheet1";
                        startColumn = 1;
                        endColumn = 12;
                        //startRow = 6;
                        startRow = 2;
                        endrow = 6 + lst.Count;
                        //AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));

                        pdfname = fname.Replace(".xlsx", ".pdf");
                        //ret = ConvertXlstoPdf(fname, pdfname);

                        //return pdfname;

                    }
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

    }
    public string ConvertXlstoPdf(string docfile, string pdffile)
    {
        string docpath = Server.MapPath("~/pdftemp/" + docfile);
        FileStream readFile = File.OpenRead(docpath);

        Logger.LogError(docfile);

        // reference: https://asp.syncfusion.com/demos/web/pdf/exceltopdf.aspx
        //Convert the input Excel document to a PDF file
        #region Convert Excel to PDF

        try
        {
            ExcelToPdfConverter converter = null;

            converter = new ExcelToPdfConverter(readFile);

            //Intialize the PdfDocument Class
            Syncfusion.Pdf.PdfDocument pdfDoc = new Syncfusion.Pdf.PdfDocument();

            //Intialize the ExcelToPdfConverterSettings class
            ExcelToPdfConverterSettings settings = new ExcelToPdfConverterSettings();

            // only one value of the below can be set true
            bool chk1 = false; // no scaling
            bool chk2 = true; // fit all columns on one page
            bool chk3 = false; // fit all rows on one page
            bool chk4 = false; // fit sheet on one page


            //Set the Layout Options for the output Pdf page.
            if (chk1)
                settings.LayoutOptions = LayoutOptions.NoScaling;
            else if (chk3)
                settings.LayoutOptions = LayoutOptions.FitAllRowsOnOnePage;
            else if (chk2)
                settings.LayoutOptions = LayoutOptions.FitAllColumnsOnOnePage;
            else
                settings.LayoutOptions = LayoutOptions.FitSheetOnOnePage;
            //Enable EmbedFonts
            settings.EmbedFonts = true;
            //Enable AutoDetectComplexScript property
            settings.AutoDetectComplexScript = true;


            //Assign the output PdfDocument to the TemplateDocument property of ExcelToPdfConverterSettings 
            settings.TemplateDocument = pdfDoc;
            settings.DisplayGridLines = GridLinesDisplayStyle.Invisible;
            //Convert the Excel document to PDf
            pdfDoc = converter.Convert(settings);

            //Save the pdf file            

            // pdfDoc.Save("ExceltoPDF.pdf", Response, HttpReadType.Save);

            string pdfpath = Server.MapPath("~/pdftemp/" + pdffile);
            pdfDoc.Save(pdfpath);
            // readFile.Close();
            return pdfpath;


        }
        catch (Exception ex)
        {
            Logger.LogError(" Error when converting XLSX to PDF: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

        #endregion
    }
    [WebMethod(EnableSession = true)]
    public string GeneratePcc(int partsl, int oprnsl, string rwhght)
    {
        try
        {
            string sqlrowheightinsert = "";
            var userinfo = HttpContext.Current.Session["UserInfo"];
            int imgno = 0;
            string fname = "pcc_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));

            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/PCCTemplate.xlsx"));
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl
                                + " and sp.show_in_legend = 1";
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";
            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                sqlrowheightinsert = "update  controlplan set PCC_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";
                db.Execute(sqlrowheightinsert);
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);

                string sqlcp = @"Select * from temp_rptControlPlan where methodDesc2='PCC'   and Obsolete='N'
                              -- tr inner join SampleFrequency sf on sf.freq_slno =tr.freq_slno 
                                --  where sf.pcc=1";

                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where  methodDesc2='PCC'   and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0  and where  methodDesc2='PCC'   and  Obsolete='Y' ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }



                //List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                string mach = db.ExecuteScalar<string>(sqlmachinecode, lst[0].machine_slno);
                if (lst.Count > 0)
                {
                    string imgPath = Server.MapPath("~/Documents/");
                    double xlrowht = 50;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                        ws.Cells["A2"].Value = "Part Name: " + lst[0].PartDescription;
                        ws.Cells["E2"].Value = "Part No : " + lst[0].mstPartNo;
                        ws.Cells["I2"].Value = "Rev status : " + lst[0].partIssueNo + " - " + lst[0].partIssueDt.Replace('/', '.');
                        ws.Cells["M2"].Value = "Operation No./ Name : " + lst[0].process_no + " / " + lst[0].OperationDesc;
                        ws.Cells["M2"].Style.WrapText = true;
                        if (mach == null || mach == "" || mach == "-")
                            ws.Cells["Q2"].Value = "Machine Description/ Code : " + lst[0].MachineDesc;
                        else
                            ws.Cells["Q2"].Value = "Machine Description/ Code : " + lst[0].MachineDesc + "/ " + mach;

                        ws.Cells["Q2"].Style.WrapText = true;
                        ws.Cells["I2"].Style.WrapText = true;
                        int sl_no = 1;
                        int row = 6;
                        ws.InsertRow(row, lst.Count);
                        int index = (5 + (lst.Count) + 3);
                        int legendcount = lstlegend.Count;
                        if (lstlegend.Count > 0)
                        {
                            if (lst[0].Customer_name.ToLower().Trim() == "meritor")
                            {
                                if (!string.IsNullOrWhiteSpace(lstlegend[0].splCharFile))
                                {
                                    addimagePCC(lstlegend[0].splCharFile, index + 1, 24, ws, "0");
                                    ws.Cells[index, 27].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    // ws.Cells[index, 27].Value = lstlegend[0].spl_char_desc;
                                }
                                if (lstlegend.Count > 1)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstlegend[1].splCharFile))
                                    {
                                        addimagePCC(lstlegend[1].splCharFile, index + 3, 24, ws, "1");
                                        ws.Cells[index + 2, 27].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        // ws.Cells[index + 2, 27].Value = lstlegend[1].spl_char_desc;
                                    }
                                }
                                ws.Cells["AB" + (index - 1) + ":AD" + index + 2].Clear();
                                ws.Cells["AE" + (index - 1) + ":AG" + index + 2].Clear();
                                ws.Cells["AB" + (index - 1) + ":AD" + index + 2].Clear();
                            }
                            else if (lst[0].Customer_name.ToLower().Trim() == "volvo")
                            {
                                ws.Cells["Y" + (index - 1) + ":AA" + (index - 1)].Clear();
                                ws.Cells["AE" + (index - 1) + ":AG" + (index - 1)].Copy(ws.Cells["Y" + (index - 1) + ":AA" + (index - 1)]);
                                ws.Cells["Y" + (index) + ":AA" + (index + 1)].Clear();
                                ws.Cells["AE" + (index) + ":AG" + (index + 1)].Copy(ws.Cells["Y" + (index) + ":AA" + (index + 1)]);
                                ws.Cells["Y" + (index + 2) + ":AA" + (index + 3)].Clear();
                                ws.Cells["AE" + (index + 2) + ":AG" + (index + 3)].Copy(ws.Cells["Y" + (index + 2) + ":AA" + (index + 3)]);
                                ws.Cells["Y" + (index - 1) + ":AA" + (index - 1)].Merge = true;
                                ws.Cells["Y" + (index) + ":AA" + (index + 1)].Merge = true;
                                ws.Cells["Y" + (index + 2) + ":AA" + (index + 3)].Merge = true;
                                if (!string.IsNullOrWhiteSpace(lstlegend[0].splCharFile))
                                {
                                    if (lstlegend[0].spl_char_desc.Contains("Critical"))
                                    {
                                        addimagePCC(lstlegend[0].splCharFile, index + 1, 24, ws, "0");
                                        // ws.Cells[index, 27].Value = lstlegend[0].spl_char_desc;
                                    }
                                }
                                if (lstlegend.Count > 1)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstlegend[1].splCharFile))
                                    {
                                        if (lstlegend[1].spl_char_desc.Contains("Critical"))
                                        {
                                            addimagePCC(lstlegend[0].splCharFile, index + 1, 24, ws, "1");
                                        }

                                    }
                                }
                                if (lstlegend.Count > 2)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstlegend[2].splCharFile))
                                    {
                                        if (lstlegend[2].spl_char_desc.Contains("Significant"))
                                        {
                                            addimagePCC(lstlegend[2].splCharFile, index + 3, 24, ws, "2");
                                        }
                                    }
                                }
                                if (lstlegend.Count > 3)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstlegend[3].splCharFile))
                                    {
                                        if (lstlegend[3].spl_char_desc.Contains("Significant"))
                                        {
                                            addimagePCC(lstlegend[3].splCharFile, index + 3, 24, ws, "3");
                                        }
                                    }
                                }
                                if (lstlegend.Count > 4)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstlegend[4].splCharFile))
                                    {
                                        if (lstlegend[4].spl_char_desc.Contains("Significant"))
                                        {
                                            addimagePCC(lstlegend[4].splCharFile, index + 3, 24, ws, "4");
                                        }
                                    }
                                }
                                ws.Cells["AB" + (index - 1) + ":AD" + index + 2].Clear();
                                ws.Cells["AE" + (index - 1) + ":AG" + index + 2].Clear();
                                ws.Cells["AB" + (index - 1) + ":AD" + index + 2].Clear();
                            }
                            else if (lst[0].Customer_name.ToLower().Trim() == "apa")
                            {
                                ws.Cells["Y" + (index - 1) + ":AA" + (index - 1)].Clear();
                                ws.Cells["AB" + (index - 1) + ":AD" + (index - 1)].Copy(ws.Cells["Y" + (index - 1) + ":AA" + (index - 1)]);
                                ws.Cells["Y" + (index) + ":AA" + (index + 1)].Clear();
                                ws.Cells["Y" + (index + 2) + ":AA" + (index + 3)].Clear();
                                ws.Cells["Y" + (index) + ":AA" + (index + 3)].Merge = true;
                                ws.Cells["AB" + (index) + ":AD" + (index + 3)].Copy(ws.Cells["Y" + (index) + ":AA" + (index + 3)]);
                                if (!string.IsNullOrWhiteSpace(lstlegend[0].splCharFile))
                                {
                                    if (lstlegend[0].spl_char_desc.Contains("Key"))
                                    {
                                        addimagePCC(lstlegend[0].splCharFile, index, 24, ws, "0");
                                        ws.Cells[index, 27].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }

                                ws.Cells["AB" + (index - 1) + ":AD" + index + 2].Clear();
                                ws.Cells["AE" + (index - 1) + ":AG" + index + 2].Clear();
                                ws.Cells["AB" + (index - 1) + ":AD" + index + 2].Clear();

                            }


                        }
                        foreach (Class_Temp_RptControlPlan c in lst)
                        {
                            ws.Row(row).Height = xlrowht;
                            ws.Cells[row, 1].Value = sl_no.ToString();
                            ws.Cells[row, 2].Value = c.product_char;
                            if (c.splChar_slno > 0)
                            {
                                string img = GetFileName(c.splChar_slno.ToString());
                                string pth = (imgPath + img);
                                FileInfo imgFile = new FileInfo(pth);
                                if (imgFile.Exists)
                                {
                                    imgno += 1;
                                    Bitmap image = new Bitmap(pth);
                                    ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                    ExcelRangeBase cell = ws.Cells[row - 1, 2];
                                    excelPicture.SetPosition(cell.Start.Row, 8, cell.Start.Column, 5);
                                    excelPicture.SetSize(40, 40);
                                }
                            }
                            ws.Cells[row, 4].Value = c.spec1;
                            ws.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 4].Style.WrapText = true;
                            //ws.Cells[row, 4].Value = c.tol_min + "/" + c.tol_max;
                            ws.Cells[row, 5].Value = c.measurementTech2;
                            ws.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 5].Style.WrapText = true;
                            //ws.Cells[row, 6].Value = c.sampleFreq2 + " / " + c.sampleSize2;
                            if (c.sampleFreq2 == "" || c.sampleFreq2 == null)
                                ws.Cells[row, 6].Value = c.sampleSize2;
                            else if (c.sampleSize2 == "" || c.sampleSize2 == null)
                                ws.Cells[row, 6].Value = c.sampleFreq2;
                            else
                                ws.Cells[row, 6].Value = c.sampleFreq2 + " / " + c.sampleSize2;
                            ws.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 2].Style.WrapText = true;
                            ws.Cells[row, 6].Style.WrapText = true;
                            ws.Row(row).Height = xlrowht;
                            //// ---------- added to set the row height
                            //// Find the cell with the maximum content length
                            //ExcelRangeBase cellWithMaxContent = FindCellWithMaxContentPCC(row, ws,"PCC");


                            //// Calculate the required row height based on the cell content
                            //int requiredRowHeight = GetRequiredRowHeight(ws, cellWithMaxContent);

                            //// Set the row height to the calculated required height (if it's greater than the default height)
                            //int finalRowHeight = (int)Math.Max(ws.DefaultRowHeight, requiredRowHeight);
                            //ws.Row(cellWithMaxContent.Start.Row).Height = finalRowHeight + 30 ;
                            // ---------------
                            sl_no += 1; row = row + 1;
                        }

                        ws.Cells[5 + lst.Count + 8, 1].Value = "F/Q/009 Rev:0";
                        string[] cpnumber = lst[0].cp_number.Split('/');
                        ws.Cells[5 + lst.Count + 8, 24].Value = cpnumber[1] + cpnumber[2] + " / " + lst[0].process_no + "/ PCC - Rev No:0";
                        ws.Cells[5 + lst.Count + 8, 24].Style.WrapText = true;
                        int currentEndRow = ws.Dimension.End.Row;
                        ws.Cells["A1:AA" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:AA" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:AA" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:AA" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:AA" + currentEndRow];
                        ws.Protection.AllowEditObject = false;
                        xlPackage.SaveAs(excelFile);
                        string filePath = excelFile.ToString();
                        string worksheetName = "Sheet1";
                        int startColumn = 1;
                        int endColumn = 27;
                        int startRow = 6;
                        int endrow = 5 + lst.Count;
                        //AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        //var ret = ConvertXlstoPdf(fname, pdfname);
                        // return pdfname;
                    }

                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {

            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

    }



    [WebMethod(EnableSession = true)]
    public string GeneratePccBridge(int partsl, int oprnsl, string rwhght)
    {
        try
        {
            string sqlrowheightinsert = "";
            var userinfo = HttpContext.Current.Session["UserInfo"];
            int imgno = 0;
            string fname = "pcc_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));

            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/BridgeTemplate.xlsx"));
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl
                                + " and sp.show_in_legend = 1";
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";
            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                sqlrowheightinsert = "update  controlplan set PCC_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";
                db.Execute(sqlrowheightinsert);
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);

                string sqlcp = @"Select * from temp_rptControlPlan where methodDesc='PCC(ADB)'   and obsolete='N'
                              -- tr inner join SampleFrequency sf on sf.freq_slno =tr.freq_slno 
                                --  where sf.pcc=1";

                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);

                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where  methodDesc='PCC(ADB)'  and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0 and  methodDesc='PCC(ADB)'   and  Obsolete='Y'     ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }





                string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                string mach = db.ExecuteScalar<string>(sqlmachinecode, lst[0].machine_slno);
                if (lst.Count > 0)
                {
                    string imgPath = Server.MapPath("~/Documents/");
                    double xlrowht = 50;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                        ws.Cells["A3"].Value = "Part Name: " + lst[0].PartDescription;
                        ws.Cells["E3"].Value = "Part No : " + lst[0].mstPartNo;
                        ws.Cells["I3"].Value = "Rev: " + lst[0].cp_revno + " / " + lst[0].cp_revdt;
                        ws.Cells["K3"].Value = "Operation No./ Name : " + lst[0].process_no + " / " + lst[0].OperationDesc;
                        ws.Cells["M3"].Style.WrapText = true;
                        if (mach == null || mach == "" || mach == "-")
                            ws.Cells["M3"].Value = "Machine Description/ Code : " + lst[0].MachineDesc;
                        else
                            ws.Cells["M3"].Value = "Machine Description/ Code : " + lst[0].MachineDesc + "/ " + mach;
                        ws.Cells["A3"].Style.WrapText = true;
                        ws.Cells["E3"].Style.WrapText = true;
                        ws.Cells["K3"].Style.WrapText = true;
                        ws.Cells["I3"].Style.WrapText = true;
                        int sl_no = 1;
                        int row = 6;
                        ws.InsertRow(row, lst.Count);
                      
                       
                        foreach (Class_Temp_RptControlPlan c in lst)
                        {
                            ws.Row(row).Height = xlrowht;
                            ws.Cells[row, 1].Value = sl_no.ToString();
                            ws.Cells[row, 2].Value = c.product_char;
                            if (c.splChar_slno > 0)
                            {
                                string img = GetFileName(c.splChar_slno.ToString());
                                string pth = (imgPath + img);
                                FileInfo imgFile = new FileInfo(pth);
                                if (imgFile.Exists)
                                {
                                    imgno += 1;
                                    Bitmap image = new Bitmap(pth);
                                    ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                    ExcelRangeBase cell = ws.Cells[row - 1, 2];
                                    excelPicture.SetPosition(cell.Start.Row, 8, cell.Start.Column, 5);
                                    excelPicture.SetSize(40, 40);
                                }
                            }
                            ws.Cells[row, 4].Value = c.spec1;
                            ws.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 4].Style.WrapText = true;
                            //ws.Cells[row, 4].Value = c.tol_min + "/" + c.tol_max;
                            ws.Cells[row, 5].Value = c.measurementTech2;
                            ws.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 5].Style.WrapText = true;
                            //ws.Cells[row, 6].Value = c.sampleFreq2 + " / " + c.sampleSize2;
                            if (c.sampleFreq2 == "" || c.sampleFreq2 == null)
                                ws.Cells[row, 6].Value = c.sampleSize2;
                            else if (c.sampleSize2 == "" || c.sampleSize2 == null)
                                ws.Cells[row, 6].Value = c.sampleFreq2;
                            else
                                ws.Cells[row, 6].Value = c.sampleFreq2 + " / " + c.sampleSize2;
                            ws.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 2].Style.WrapText = true;
                            ws.Cells[row, 6].Style.WrapText = true;
                            ws.Row(row).Height = xlrowht;
                            //// ---------- added to set the row height
                            //// Find the cell with the maximum content length
                            //ExcelRangeBase cellWithMaxContent = FindCellWithMaxContentPCC(row, ws,"PCC");


                            //// Calculate the required row height based on the cell content
                            //int requiredRowHeight = GetRequiredRowHeight(ws, cellWithMaxContent);

                            //// Set the row height to the calculated required height (if it's greater than the default height)
                            //int finalRowHeight = (int)Math.Max(ws.DefaultRowHeight, requiredRowHeight);
                            //ws.Row(cellWithMaxContent.Start.Row).Height = finalRowHeight + 30 ;
                            // ---------------
                            sl_no += 1; row = row + 1;
                        }

                        ws.Cells[5 + lst.Count + 9, 1].Value = "F / ADB / QA / 01 Rev 02";
                        //string[] cpnumber = lst[0].cp_number.Split('/');
                        //ws.Cells[5 + lst.Count + 8, 24].Value = cpnumber[1] + cpnumber[2] + " / " + lst[0].process_no + "/ PCC - Rev No:0";
                        //ws.Cells[5 + lst.Count + 8, 24].Style.WrapText = true;
                        int currentEndRow = ws.Dimension.End.Row;
                        ws.Cells["A1:R" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:R" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:R" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:R" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:R" + currentEndRow];
                        ws.Protection.AllowEditObject = false;
                        xlPackage.SaveAs(excelFile);
                        string filePath = excelFile.ToString();
                        string worksheetName = "Sheet1";
                        int startColumn = 1;
                        int endColumn = 18;
                        int startRow = 6;
                        int endrow = 5 + lst.Count;
                        //AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        //var ret = ConvertXlstoPdf(fname, pdfname);
                        // return pdfname;
                    }

                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {

            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

    }




    [WebMethod(EnableSession = true)]
    public string GeneratePccIIR(int partsl, int oprnsl, string rwhght)
    {
        try
        {
            string sqlrowheightinsert = "";
            var userinfo = HttpContext.Current.Session["UserInfo"];
            int imgno = 0;
            string fname = "pcc_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));

            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/IIRTemplate.xlsx"));
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl
                                + " and sp.show_in_legend = 1";
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";
            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                sqlrowheightinsert = "update  controlplan set PCC_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";
                db.Execute(sqlrowheightinsert);
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);

                string sqlcp = @"Select * from temp_rptControlPlan where methodDesc='PCC(IIR)'   and obsolete='N'
                              -- tr inner join SampleFrequency sf on sf.freq_slno =tr.freq_slno 
                                --  where sf.pcc=1";





                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);



                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where methodDesc='PCC(IIR)'    and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0 and methodDesc='PCC(IIR)'    and  Obsolete='Y'     ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }



                string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                string mach = db.ExecuteScalar<string>(sqlmachinecode, lst[0].machine_slno);
                if (lst.Count > 0)
                {
                    string imgPath = Server.MapPath("~/Documents/");
                    double xlrowht = 50;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                        ws.Cells["A2"].Value = "Part Name: " + lst[0].PartDescription;
                        ws.Cells["A4"].Value = "Part No : " + lst[0].mstPartNo;
                        ws.Cells["A6"].Value = "Rev: " + lst[0].cp_revno+" / " + lst[0].cp_revdt;
                        

                        
                        int sl_no = 1;
                        int row = 9;
                        ws.InsertRow(row, lst.Count);
                        
                        foreach (Class_Temp_RptControlPlan c in lst)
                        {
                            ws.Row(row).Height = xlrowht;
                            ws.Cells[row, 1].Value = sl_no.ToString();
                            ws.Cells[row, 2].Value = c.product_char;
                            ws.Cells[row, 3].Value = c.spec1;
                            if (c.splChar_slno > 0)
                            {
                                string img = GetFileName(c.splChar_slno.ToString());
                                string pth = (imgPath + img);
                                FileInfo imgFile = new FileInfo(pth);
                                if (imgFile.Exists)
                                {
                                    imgno += 1;
                                    Bitmap image = new Bitmap(pth);
                                    ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                    ExcelRangeBase cell = ws.Cells[row - 1, 3];
                                    excelPicture.SetPosition(cell.Start.Row, 8, cell.Start.Column, 5);
                                    excelPicture.SetSize(40, 40);
                                }
                            }
                         
                            ws.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 4].Style.WrapText = true;
                            //ws.Cells[row, 4].Value = c.tol_min + "/" + c.tol_max;
                            ws.Cells[row, 6].Value = c.measurementTech2;
                            ws.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 5].Style.WrapText = true;
                            ws.Cells[row, 6].Style.WrapText = true;
                            ws.Cells[row, 2].Style.WrapText = true;
                            ws.Cells[row, 3].Style.WrapText = true;
                            //ws.Cells[row, 6].Value = c.sampleFreq2 + " / " + c.sampleSize2;
                            if (c.sampleFreq2 == "" || c.sampleFreq2 == null)
                                ws.Cells[row, 5].Value = c.sampleSize2;
                            else if (c.sampleSize2 == "" || c.sampleSize2 == null)
                                ws.Cells[row, 5].Value = c.sampleFreq2;
                            else
                                ws.Cells[row, 5].Value = c.sampleFreq2 + " / " + c.sampleSize2;
                            ws.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Row(row).Height = xlrowht;
                            //// ---------- added to set the row height
                            //// Find the cell with the maximum content length
                            //ExcelRangeBase cellWithMaxContent = FindCellWithMaxContentPCC(row, ws,"PCC");


                            //// Calculate the required row height based on the cell content
                            //int requiredRowHeight = GetRequiredRowHeight(ws, cellWithMaxContent);

                            //// Set the row height to the calculated required height (if it's greater than the default height)
                            //int finalRowHeight = (int)Math.Max(ws.DefaultRowHeight, requiredRowHeight);
                            //ws.Row(cellWithMaxContent.Start.Row).Height = finalRowHeight + 30 ;
                            // ---------------
                            sl_no += 1; row = row + 1;
                        }

                        ws.Cells[8 + lst.Count + 8, 1].Value = "F/Q/009 Rev:0";
                        //string[] cpnumber = lst[0].cp_number.Split('/');
                        //ws.Cells[8 + lst.Count + 8, 24].Value = cpnumber[1] + cpnumber[2] + " / " + lst[0].process_no + "/ PCC(IIR) - Rev No:02";
                        //ws.Cells[5 + lst.Count + 8, 24].Style.WrapText = true;
                        int currentEndRow = ws.Dimension.End.Row;
                        ws.Cells["A1:L" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:L" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:L" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:L" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:L" + currentEndRow];
                        ws.Protection.AllowEditObject = false;
                        xlPackage.SaveAs(excelFile);
                        string filePath = excelFile.ToString();
                        string worksheetName = "Sheet1";
                        int startColumn = 1;
                        int endColumn = 12;
                        int startRow = 9;
                        int endrow =8 + lst.Count;
                        //AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        //var ret = ConvertXlstoPdf(fname, pdfname);
                        // return pdfname;
                    }

                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {

            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

    }







    [WebMethod(EnableSession = true)]
    public string GenerateFoi(int partsl, int oprnsl, string rwhght)
    {
        try
        {
            string sqlrowheightinsert = "";
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string sqllegend = @"select distinct sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join[mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl + " and cp.operation_slno =" + oprnsl + " and sp.show_in_legend = 1";

            string fname = "foi_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/FOITemplate.xlsx"));


            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
            "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";

            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                sqlrowheightinsert = "update  controlplan set FOI_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";
                db.Execute(sqlrowheightinsert);
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);

                string sqlcp = @"Select * from temp_rptControlPlan where methodDesc='FOI'   and obsolete='N'
-- tr inner join SampleFrequency sf on sf.freq_slno =tr.freq_slno 
-- where sf.foi=1";

                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);

                
                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where  methodDesc='FOI'    and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0 and methodDesc='FOI'    and  Obsolete='Y'     ";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }





                string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                string mach = db.ExecuteScalar<string>(sqlmachinecode, lst[0].machine_slno);
                if (lst.Count > 0)
                {

                    double xlrowht = 50;
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                        ws.Cells["A4"].Value = "Part Name: " + lst[0].PartDescription;


                        rowvallength = (ws.Cells["A5"].Value).ToString().Length;

                        ws.Cells["D4"].Value = "Operation No./ Name: " + lst[0].process_no + " / " + lst[0].OperationDesc;
                        ws.Cells["D4"].Style.WrapText = true;
                        ws.Cells["A4"].Style.WrapText = true;
                        ws.Cells["A5"].Style.WrapText = true;
                        ws.Cells["D4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A5"].Value = "Part No : " + lst[0].mstPartNo;
                        rowvallengthpart = ws.Cells["A4"].Value.ToString();

                        ws.Cells["D5"].Value = "Rev Status : " + lst[0].partIssueNo + " - " + lst[0].partIssueDt.Replace('/', '.');
                        FOIrevstatuscolumn = (ws.Cells["D5"].Value).ToString().Length;
                        FOIOPcolumn = (ws.Cells["D4"].Value).ToString().Length;
                        if (mach == null || mach == "" || mach == "-")
                            ws.Cells["L3"].Value = lst[0].MachineDesc;
                        else
                            ws.Cells["L3"].Value = lst[0].MachineDesc + " / " + mach;
                        ws.Cells["L3"].Style.WrapText = true;
                        ws.Cells["D5"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        int sl_no = 1;
                        int row = 8;
                        ws.InsertRow(row, lst.Count - 1);
                        int index = (row + (lst.Count - 1) + 1);
                        int imgno = 0;
                        string imgPath = Server.MapPath("~/Documents/");
                        int legendcount = lstlegend.Count;
                        if (lstlegend.Count > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(lstlegend[0].splCharFile))
                            {
                                addimage(lstlegend[0].splCharFile, index, 1, ws, "0");
                                ws.Cells[index, 1].Value = " QCC - Quality Control Characteristics";
                            }
                            if (lstlegend.Count > 1)
                            {
                                if (!string.IsNullOrWhiteSpace(lstlegend[1].splCharFile))
                                {
                                    addimage(lstlegend[1].splCharFile, index, 7, ws, "1");
                                    //  ws.Cells[index, 7].Value = lstlegend[1].spl_char_desc + " - Safety Related Characteristics";
                                    ws.Cells[index, 7].Value = " SRC - Safety Related Characteristics";
                                }
                            }
                        }

                        ws.Cells[index + 3, 1].Value = "Inspected By: ";
                        //ws.Cells[index + 3, 8].Value = "Approved By: " + lst[0].approvedBy;
                        ws.Cells[index + 3, 8].Value = "Approved By: ";
                        string[] cp = lst[0].cp_number.Split('/');
                        ws.Cells[index + 4, 12].Value = cp[1] + cp[2] + "/" + lst[0].process_no + "/FOI - Rev No:00";

                        foreach (Class_Temp_RptControlPlan c in lst)
                        {
                            ws.Row(row).Height = xlrowht;
                            ws.Cells[row, 1].Value = sl_no.ToString();
                            ws.Cells[row, 2].Value = c.product_char;
                            if (c.splChar_slno > 0)
                            {
                                string img = GetFileName(c.splChar_slno.ToString());
                                string pth = (imgPath + img);
                                FileInfo imgFile = new FileInfo(pth);
                                if (imgFile.Exists)
                                {
                                    imgno += 1;
                                    Bitmap image = new Bitmap(pth);
                                    ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                    ExcelRangeBase cell = ws.Cells[row - 1, 2];
                                    excelPicture.SetPosition(cell.Start.Row, 18, cell.Start.Column, 10);
                                    excelPicture.SetSize(50, 50);
                                }
                            }
                            //ws.Cells[row, 4].Value = c.tol_min + "/" + c.tol_max;

                            ws.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 4].Value = c.spec1;
                            ws.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 2].Style.WrapText = true;
                            ws.Cells[row, 6].Style.WrapText = true;
                            ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ws.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            ws.Cells[row, 4].Style.WrapText = true;
                            ws.Cells[row, 6].Value = c.measurementTech;
                            ws.Cells[row, 7].Value = c.sampleSize;
                            ws.Row(row).Height = xlrowht;
                            // ---------- added to set the row height
                            // Find the cell with the maximum content length
                            //ExcelRangeBase cellWithMaxContent = FindCellWithMaxContentPCC(row, ws,"FOI");


                            //// Calculate the required row height based on the cell content
                            //int requiredRowHeight = GetRequiredRowHeight(ws, cellWithMaxContent);

                            //// Set the row height to the calculated required height (if it's greater than the default height)
                            //int finalRowHeight = (int)Math.Max(ws.DefaultRowHeight, requiredRowHeight);
                            //ws.Row(cellWithMaxContent.Start.Row).Height = finalRowHeight ;
                            //// ---------------
                            if (c.product_char.Contains("Appearance"))
                            {
                                if (c.product_char.Contains("Machining"))
                                    rowAPPMachining = row;
                                if (c.product_char.Contains("Casting"))
                                    rowAPPCasting = row;
                            }
                            sl_no += 1;
                            row = row + 1;

                        }
                        int currentEndRow = ws.Dimension.End.Row;
                        ws.Cells["A1:M" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:M" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:M" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        ws.Cells["A1:M" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:M" + currentEndRow];
                        ws.Protection.AllowEditObject = false;
                        xlPackage.SaveAs(excelFile);
                        string filePath = excelFile.ToString();
                        string worksheetName = "Sheet1";
                        int startColumn = 1;
                        int endColumn = 13;
                        int startRow = 8;
                        int endrow = 7 + lst.Count;
                        //AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        varcp = "FOI";

                        AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));
                        varcp = string.Empty;
                        rowAPPCasting = 0;
                        rowAPPMachining = 0;
                        rowvallength = 0;
                        rowvallengthpart = string.Empty;
                        FOIrevstatuscolumn = 0;
                        FOIOPcolumn = 0;
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        //var ret = ConvertXlstoPdf(fname, pdfname);
                        // return pdfname;
                    }
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }
        }
        catch (Exception ex)
        {

            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

    }
    public ExcelWorksheet Copy(ExcelWorkbook workbook, string existingWorksheetName, string newWorksheetName)
    {
        ExcelWorksheet worksheet = workbook.Worksheets.Copy(existingWorksheetName, newWorksheetName);
        return worksheet;
    }

    [WebMethod(EnableSession = true)]
    public string GeneratePmc(int partsl, int oprnsl, string charsl)
    {
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "pmc_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/PMC.xlsx"));

            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no AS INT) ) from controlplan p WHERE " +
                        "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.Obsolete='N' and  c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";

            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                //string sqlcp = @"Select * from temp_rptControlPlan  where methodDesc2='PMC' " +
                //" and (product_char <>'-' or product_char  <>'')" +
                //"-- tr inner join SampleFrequency sf on sf.freq_slno =tr.freq_slno " +
                //             "-- where sf.pmc=1" +
                //             "-- where tr.product_char='" + charsl + "'";

                // List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                string sqlcp = @"Select * from temp_rptControlPlan where Obsolete='N' and methodDesc2='PMC' " +
                       " and (product_char <>'-' or product_char  <>'')";
                List <Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where Obsolete='Y'  and methodDesc2='PMC' " +
                       " and (product_char <>'-' or product_char  <>'')  order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0  and Obsolete='Y'  and methodDesc2='PMC'";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }



               
                if (lst.Count > 0)
                {
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        int countofchars = 0;
                        double xlrowht = 50;
                        foreach (Class_Temp_RptControlPlan c1 in lst)
                        {

                            countofchars++;
                            //Copy(xlPackage.Workbook, xlPackage.Workbook.Worksheets[countofchars].Name, "Sheet_" + countofchars);
                            Copy(xlPackage.Workbook, "Template", "Sheet_" + countofchars);
                        }
                        xlPackage.Workbook.Worksheets.Delete("Template");
                        int cn = xlPackage.Workbook.Worksheets.Count;
                        countofchars = 1;
                        foreach (Class_Temp_RptControlPlan c1 in lst)
                        {

                            string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                            string mach = db.ExecuteScalar<string>(sqlmachinecode, c1.machine_slno);

                            string sqlSpclChar = @"select splCharFile from SpecialChars where splChar_slno= @0";
                            string splchar = db.ExecuteScalar<string>(sqlSpclChar, c1.splChar_slno);
                            string spl = string.Empty;
                            if (!string.IsNullOrWhiteSpace(splchar))
                            {
                                if (splchar.Contains("QCC") || splchar.Contains("-"))
                                {
                                    string[] name = splchar.Split('-');
                                    string[] secpart = name[1].Split('.');
                                    spl = secpart[0] + " - " + name[0];
                                }
                                else
                                {
                                    string[] secpart = splchar.Split('.');
                                    spl = secpart[0];
                                }
                            }
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Sheet_" + countofchars];
                            ws.Cells["A3"].Value = "Part No: " + c1.mstPartNo + " / " + c1.partIssueNo + " - " + c1.partIssueDt.Replace('/', '.');
                            ws.Cells["A3"].Style.WrapText = true;
                            ws.Cells["E3"].Value = "Part Name: " + c1.PartDescription;
                            ws.Cells["K3"].Value = "Characteristics: " + c1.product_char;
                            ws.Cells["A4"].Value = "Operation No/Name: " + c1.process_no + " / " + c1.OperationDesc;
                            ws.Cells["A4"].Style.WrapText = true;
                            if (mach == "" || mach == null || mach == "-")
                                ws.Cells["E4"].Value = "Machine Description/Code: " + c1.MachineDesc;
                            else
                                ws.Cells["E4"].Value = "Machine Description/Code: " + c1.MachineDesc + " / " + mach;
                            ws.Cells["E4"].Style.WrapText = true;
                            ws.Cells["T3"].Value = "Spec: " + c1.spec1;
                            ws.Cells["K4"].Value = "Measuring Equipment: " + c1.measurementTech2;
                            ws.Cells["K4"].Style.WrapText = true;
                            ws.Cells["W4"].Value = spl;

                            Color red = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);        // #FF0000
                            Color orange = Color.FromArgb(0xFF, 0xF4, 0xB0, 0x84);    // #F4B084
                            Color yellow = Color.FromArgb(0xFF, 0xFF, 0xF2, 0xCC);    // #FFF2CC
                            Color green = Color.FromArgb(0xFF, 0xC6, 0xE0, 0xB4);     // #C6E0B4

                            //countofchars += 1;
                            string tol_type = "bilateral";
                            decimal lsl = c1.min_spec;
                            decimal usl = c1.max_spec;


                            // for unilateral
                            if (c1.min_spec == 0 && c1.max_spec > 0)
                            {
                                lsl = 0;
                                usl = c1.max_spec;
                                tol_type = "uni-with-max";
                            }
                            int row = 6;
                            int colStart = 3, colEnd = 24;
                            int row_mean = 0, row_lsl = 0, row_usl = 0;

                            if (tol_type == "bilateral")
                            {
                                List<Class_PmcSeries> lstdata = new List<Class_PmcSeries>();

                                decimal fulltol = usl - lsl;
                                decimal tol = fulltol / 2;
                                decimal incr = tol * 0.1m;
                                decimal lowest = lsl - incr;
                                decimal highest = usl + incr;
                                decimal nominal = lsl + tol;

                                decimal red_data_1 = lowest;
                                decimal red_data_2 = highest;

                                decimal perc60 = fulltol * 60 / 100;
                                decimal perc30 = perc60 / 2;


                                decimal green_data_start = nominal - perc30;
                                decimal green_data_end = nominal + perc30;

                                decimal perc_45 = fulltol * 45 / 100;
                                decimal yellow_min_start = nominal - perc_45;
                                decimal yellow_min_end = green_data_start;

                                decimal yellow_max_start = green_data_end;
                                decimal yellow_max_end = nominal + perc_45;

                                decimal perc_50 = fulltol * 50 / 100;
                                decimal orange_min_start = nominal - perc_50;
                                decimal orange_min_end = yellow_min_start;

                                decimal orange_max_start = yellow_max_start;
                                decimal orange_max_end = nominal + perc_50;
                                
                                    for (decimal i = highest; i >= lowest; i -= incr)
                                    {
                                        Class_PmcSeries c = new Class_PmcSeries
                                        {
                                            fullset = i
                                        };
                                        lstdata.Add(c);
                                    }
                                    // insert rows to the excel
                                    ws.InsertRow(row, lstdata.Count);
                                

                                // loop through data and fill excel file
                                foreach (Class_PmcSeries c in lstdata)
                                {
                                    ws.Cells["B" + row].Value = c.fullset;
                                    if (c.fullset == usl)
                                    {
                                        ws.Cells["A" + row].Value = "USL";
                                        row_usl = row;
                                    }
                                    else if (c.fullset == lsl)
                                    {
                                        ws.Cells["A" + row].Value = "LSL";
                                        row_lsl = row;
                                    }
                                    else if (c.fullset == nominal)
                                    {
                                        ws.Cells["A" + row].Value = "MEAN";
                                        row_mean = row;
                                    }

                                    // shade with red color
                                    if (c.fullset == red_data_1 || c.fullset == red_data_2)
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(red);
                                    }
                                    // shade with green color
                                    else if (c.fullset >= green_data_start && c.fullset <= green_data_end)
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(green);
                                    }

                                    // yellow band
                                    else if ((c.fullset >= yellow_min_start && c.fullset < yellow_min_end) ||
                                             (c.fullset > yellow_max_start && c.fullset <= yellow_max_end))
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(yellow);
                                    }
                                    // orange band
                                    else
                                    //if ((c.fullset > orange_min_start && c.fullset <= orange_min_end) ||
                                    //         (c.fullset > orange_max_start && c.fullset <= orange_max_end))
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(orange);
                                    }
                                    row += 1;
                                }
                                // delete 2 blank rows after data filled
                                //ws.DeleteRow(row);
                                // ws.DeleteRow(row);
                                //ws.Cells[5 + lstdata.Count + 1, 1].Value = "PROCESS LOG CODE";

                                ws.Cells[6, 2, row, 2].Style.Numberformat.Format = "0.000";
                                ws.Cells[6, 1, row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                               
                                    ws.Cells[row_usl + 1, 1, row_mean - 1, 1].Merge = true;
                                    ws.Cells[row_mean + 1, 1, row_lsl - 1, 1].Merge = true;

                                    ws.Cells[5 + lstdata.Count + 14, 1].Value = "F/Q/010 Rev:0";
                                    string[] cpnumber = c1.cp_number.Split('/');
                                    ws.Cells[5 + lstdata.Count + 14, 21].Value = cpnumber[1] + cpnumber[2] + " / " + c1.process_no + "/ PMC - Rev No:0";
                                    ws.Cells[5 + lstdata.Count + 14, 21].Style.WrapText = true;
                                    ws.Cells[5 + lstdata.Count + 14, 21].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[5 + lstdata.Count + 14, 21].Style.WrapText = true;
                                    // ws.Row(5 + lstdata.Count + 16).Height = 40;
                            


                                int currentEndRow = ws.Dimension.End.Row;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                                ws.PrinterSettings.PrintArea = ws.Cells["A1:X" + (currentEndRow)];
                                countofchars += 1;
                            }
                            else if (tol_type == "uni-with-max")
                            {
                                List<Class_PmcSeries> lstdata = new List<Class_PmcSeries>();

                                decimal fulltol = c1.max_spec;
                                decimal tol = c1.max_spec;
                                decimal incr = tol * 0.1m;
                                decimal lowest = lsl;
                                decimal highest = usl + incr;

                                decimal red_data_1 = lowest;
                                decimal red_data_2 = highest;

                                decimal perc60 = fulltol * 60 / 100;
                                decimal perc30 = fulltol * 90 / 100;
                                decimal perc10 = fulltol;

                                decimal green_data_start = lsl;
                                decimal green_data_end = perc60;

                                decimal yellow_min_start = perc60 + incr;
                                decimal yellow_min_end = perc30;

                                decimal orange_min_start = perc30 + incr;
                                decimal orange_min_end = perc10;

                               
                                    for (decimal i = highest; i >= lowest; i -= incr)
                                    {

                                        Class_PmcSeries c = new Class_PmcSeries
                                        {
                                            fullset = i
                                        };
                                        lstdata.Add(c);
                                    }
                                    // insert rows to the excel
                                    ws.InsertRow(row, lstdata.Count);
                                
                                // loop through data and fill excel file
                                foreach (Class_PmcSeries c in lstdata)
                                {
                                    ws.Cells["B" + row].Value = c.fullset;
                                    if (c.fullset == usl)
                                    {
                                        ws.Cells["A" + row].Value = "USL";
                                        row_usl = row;
                                    }
                                    else if (c.fullset == lsl)
                                    {
                                        ws.Cells["A" + row].Value = "LSL";
                                        row_lsl = row;
                                    }

                                    // shade with red color
                                    if (c.fullset == red_data_1 || c.fullset == red_data_2)
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(red);
                                    }
                                    // shade with green color
                                    else if (c.fullset >= green_data_start && c.fullset <= green_data_end)
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(green);
                                    }

                                    // yellow band
                                    else if ((c.fullset >= yellow_min_start && c.fullset <= yellow_min_end))
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(yellow);
                                    }
                                    // orange band
                                    else
                                    //if ((c.fullset > orange_min_start && c.fullset <= orange_min_end) ||
                                    //         (c.fullset > orange_max_start && c.fullset <= orange_max_end))
                                    {
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[row, colStart, row, colEnd].Style.Fill.BackgroundColor.SetColor(orange);
                                    }
                                    row += 1;
                                }
                                // delete 2 blank rows after data filled
                                // ws.DeleteRow(row);
                                // ws.DeleteRow(row);
                                // ws.Cells[5 + lstdata.Count + 1, 1].Value = "PROCESS LOG CODE";

                                ws.Cells[6, 2, row, 2].Style.Numberformat.Format = "0.000";
                                ws.Cells[6, 1, row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                              
                                    ws.Cells[row_usl + 1, 1, row_lsl - 1, 1].Merge = true;

                                    ws.Cells[5 + lstdata.Count + 14, 1].Value = "F/Q/010 Rev: 0";
                                    string[] cpnumber = c1.cp_number.Split('/');
                                    ws.Cells[5 + lstdata.Count + 14, 21].Value = cpnumber[1] + cpnumber[2] + " / " + c1.process_no + "/ PMC - Rev No:0";
                                    ws.Cells[5 + lstdata.Count + 14, 21].Style.WrapText = true;
                                    ws.Cells[5 + lstdata.Count + 14, 21].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[5 + lstdata.Count + 14, 21].Style.WrapText = true;
                                    //  ws.Row(5 + lstdata.Count + 16).Height = 40;
                                

                                int currentEndRow = ws.Dimension.End.Row;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:X" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                                ws.PrinterSettings.PrintArea = ws.Cells["A1:X" + (currentEndRow)];
                                countofchars += 1;
                            }

                        }

                        xlPackage.Save();
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        // var ret = ConvertXlstoPdf(fname, pdfname);
                        // return pdfname;

                    }
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GenerateControlChart(int partsl, int oprnsl, string rwhght)
    {
        try
        {
            string sqlrowheightinsert = "";
            int noofsheets;
            int currentEndRow = 0;
            var userinfo = HttpContext.Current.Session["UserInfo"];
            string fname = "cc_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/ControlChartTemplate.xlsx"));

            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX( CAST(p.rev_no  AS INT) ) from controlplan p WHERE " +
                        "p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.obsolete='N' and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";

            string cp_slno = string.Empty;
            using (Database db = new Database("connString"))
            {
                sqlrowheightinsert = "update  controlplan set CC_rowHeight= " + Convert.ToInt32(rwhght) + " where part_slno=" + partsl + " and operation_slno= " + oprnsl + "";
                db.Execute(sqlrowheightinsert);
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                string sqlcp = @"Select * from temp_rptControlPlan  where methodDesc2='PMC'   and obsolete='N' " +
                       " and (product_char <>'-' or product_char  <>'')" +
                       "-- tr inner join SampleFrequency sf on sf.freq_slno =tr.freq_slno ";
                //   "-- where sf.pmc=1" +
                //      "-- where tr.product_char='" + charsl + "'";
                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);

                


                if (lst.Count <= 0)
                {
                    string sqlcpslno = @"Select TOP(1) cp_slno from temp_rptControlPlan where  methodDesc2='PMC'    and  Obsolete='Y'    order by cp_slno desc";
                    int cp = db.ExecuteScalar<int>(sqlcpslno);
                    sqlcp = "Select * from temp_rptControlPlan where  cp_slno=@0 and methodDesc2='PMC'    and  Obsolete='Y'";
                    lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp, cp);

                }





                if (lst.Count > 0)
                {
                    string pdfname = string.Empty;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        int countofchars = 0;
                        double xlrowht = 50;
                        foreach (Class_Temp_RptControlPlan c1 in lst)
                        {

                            countofchars++;
                            Copy(xlPackage.Workbook, xlPackage.Workbook.Worksheets[countofchars].Name, "Sheet_" + countofchars);

                        }
                        xlPackage.Workbook.Worksheets.Delete(1);
                        //xlPackage.Workbook.Worksheets.Delete("Controlchart");
                        int cn = xlPackage.Workbook.Worksheets.Count;
                        noofsheets = countofchars;
                        countofchars = 1;
                        foreach (Class_Temp_RptControlPlan c1 in lst)
                        {

                            string sqlmachinecode = @"select machinecode from machines where machine_slno=@0";
                            string mach = db.ExecuteScalar<string>(sqlmachinecode, c1.machine_slno);
                            string sqleval = @"select evalTech FROM [mei_controlplan].[dbo].[EvaluationTech] where evalTech_slno=@0";
                            string eval = db.ExecuteScalar<string>(sqleval, c1.evalTech_slno2);
                            string sqlSpclChar = @"select splCharFile from SpecialChars where splChar_slno= @0";
                            string splchar = db.ExecuteScalar<string>(sqlSpclChar, c1.splChar_slno);
                            string spl = string.Empty;
                            if (!string.IsNullOrWhiteSpace(splchar))
                            {
                                if (splchar.Contains("QCC") || splchar.Contains("-"))
                                {
                                    string[] name = splchar.Split('-');
                                    string[] secpart = name[1].Split('.');
                                    spl = secpart[0] + " - " + name[0];
                                }
                                else
                                {
                                    string[] secpart = splchar.Split('.');
                                    spl = secpart[0];
                                }
                            }
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Sheet_" + countofchars];
                            ws.Row(2).Height = 60;
                            ws.Cells["A2"].Value = "Part No : " + c1.mstPartNo + " / " + c1.partIssueNo + " - " + c1.partIssueDt.Replace('/', '.');
                            ws.Cells["A3"].Value = "Part Name: " + c1.PartDescription;
                            ws.Cells["H2"].Value = "Description: " + c1.product_char;
                            ws.Cells["H2"].Style.WrapText = true;
                            ws.Cells["H3"].Value = "Specification: " + c1.spec1;
                            ws.Cells["R2"].Value = "Process: " + c1.OperationDesc;
                            ws.Cells["R2"].Style.WrapText = true;
                            ws.Cells["R3"].Value = "Process No: " + c1.process_no;
                            ws.Cells["Z2"].Value = "Instrument: " + eval;
                            ws.Cells["Z2"].Style.WrapText = true;
                            ws.Cells["Z3"].Value = "Least Count:0.001 ";
                            if (mach == "" || mach == null || mach == "-")
                                ws.Cells["AI2"].Value = "Machine Description/Code: " + c1.MachineDesc;
                            else
                                ws.Cells["AI2"].Value = "Machine Description/Code: " + c1.MachineDesc + " / " + mach;
                            ws.Cells["AI2"].Style.WrapText = true;
                            ws.Cells["AI3"].Value = "Spl. Char:" + spl;

                            string tol_type = "bilateral";
                            decimal lsl = c1.min_spec;
                            decimal usl = c1.max_spec;


                            // for unilateral
                            if (c1.min_spec == 0 && c1.max_spec > 0)
                            {
                                lsl = 0;
                                usl = c1.max_spec;
                                tol_type = "uni-with-max";
                            }

                            int row = 6;
                            int colStart = 3, colEnd = 45;
                            int row_mean = 0, row_lsl = 0, row_usl = 0;

                            if (tol_type == "bilateral")
                            {
                                List<Class_PmcSeries> lstdata = new List<Class_PmcSeries>();

                                decimal fulltol = usl - lsl;
                                decimal tol = fulltol / 2;
                                decimal incr = tol * 0.1m;
                                decimal lowest = lsl - incr;
                                decimal highest = usl + incr;
                                decimal nominal = lsl + tol;
                                //ws.Cells["T4"].Value = c1.max_spec;
                                //ws.Cells["Z4"].Value = ws.Cells["Z4"].Value.ToString() + c1.min_spec;
                                //ws.Cells["H4"].Value = ws.Cells["H4"].Value.ToString() + highest;
                                //ws.Cells["N4"].Value = ws.Cells["N4"].Value.ToString() + lowest;
                                //ws.Cells["A4"].Value = c1.max_spec + c1.min_spec / 2;

                                decimal red_data_1 = lowest;
                                decimal red_data_2 = highest;

                                decimal perc60 = fulltol * 60 / 100;
                                decimal perc30 = perc60 / 2;


                                decimal green_data_start = nominal - perc30;
                                decimal green_data_end = nominal + perc30;

                                decimal perc_45 = fulltol * 45 / 100;
                                decimal yellow_min_start = nominal - perc_45;
                                decimal yellow_min_end = green_data_start;

                                decimal yellow_max_start = green_data_end;
                                decimal yellow_max_end = nominal + perc_45;

                                decimal perc_50 = fulltol * 50 / 100;
                                decimal orange_min_start = nominal - perc_50;
                                decimal orange_min_end = yellow_min_start;

                                decimal orange_max_start = yellow_max_start;
                                decimal orange_max_end = nominal + perc_50;

                                for (decimal i = highest; i >= lowest; i -= incr)
                                {
                                    Class_PmcSeries c = new Class_PmcSeries
                                    {
                                        fullset = i
                                    };
                                    if (i != highest && i != lowest)
                                        lstdata.Add(c);
                                }
                                // insert rows to the excel
                                // ws.InsertRow(row, lstdata.Count);

                                // loop through data and fill excel file
                                foreach (Class_PmcSeries c in lstdata)
                                {
                                    ws.Cells["C" + row].Value = c.fullset;
                                    if (c.fullset == usl)
                                    {
                                        //ws.Cells["A" + row].Value = "USL";
                                        row_usl = row;
                                    }
                                    else if (c.fullset == lsl)
                                    {
                                        // ws.Cells["A" + row].Value = "LSL";
                                        row_lsl = row;
                                    }
                                    else if (c.fullset == nominal)
                                    {
                                        // ws.Cells["A" + row].Value = "MEAN";
                                        row_mean = row;
                                    }

                                    row += 2;
                                }
                                // delete 2 blank rows after data filled
                                //ws.DeleteRow(row);
                                // ws.DeleteRow(row);
                                //ws.Cells[5 + lstdata.Count + 1, 1].Value = "PROCESS LOG CODE";

                                ws.Cells[6, 3, row, 3].Style.Numberformat.Format = "0.000";
                                ws.Cells[6, 1, row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //  ws.Cells[row_usl + 1, 1, row_mean - 1, 1].Merge = true;
                                // ws.Cells[row_mean + 1, 1, row_lsl - 1, 1].Merge = true;
                                // ws.Cells[row_usl + 1, 1, row_lsl - 1, 1].Value = "(x̅ CHART) Average Chart:\r\n\r\nUpper Control Limit:\r\nUCL=X̿ + A₂R̅\r\nLower Control Limit:\r\nLCL=X̿ - A₂R̅\r\n\r\nHere,\r\n\r\nX̿ = (x̅1+x̅2+…+xn)/n\r\nn= no of x̅\r\nA₂= Constant table value\r\nR̅= (R1+R2+..Rn)/n\r\nn= No of R";
                                ws.Cells["A6"].Value = " AVERAGE CHART \n (X bar CHART)";
                                //ws.Cells[5 + lstdata.Count + 35, 1].Value = "F/Q/010 Rev:0";
                                //string[] cpnumber = c1.cp_number.Split('/');
                                //ws.Cells[5 + lstdata.Count + 35, 21].Value = cpnumber[1] + cpnumber[2] + " / " + c1.process_no + "/ PMC - Rev No:0";
                                //ws.Cells[5 + lstdata.Count + 35, 21].Style.WrapText = true;
                                //ws.Cells[5 + lstdata.Count + 35, 21].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //ws.Cells[5 + lstdata.Count + 35, 21].Style.WrapText = true;
                                //// ws.Row(5 + lstdata.Count + 16).Height = 40;



                                currentEndRow = ws.Dimension.End.Row;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                                ws.PrinterSettings.PaperSize = ePaperSize.A3;

                                ws.PrinterSettings.PrintArea = ws.Cells["A1:AS" + (currentEndRow)];
                                ws.PrinterSettings.FitToPage = true;
                                ws.PrinterSettings.FitToWidth = 1;
                                countofchars += 1;
                            }
                            else if (tol_type == "uni-with-max")
                            {
                                List<Class_PmcSeries> lstdata = new List<Class_PmcSeries>();

                                decimal fulltol = c1.max_spec;
                                decimal tol = c1.max_spec;
                                decimal incr = tol * 0.1m;
                                decimal lowest = lsl;
                                decimal highest = usl + incr;
                                //ws.Cells["T4"].Value = c1.max_spec;
                                //ws.Cells["Z4"].Value = ws.Cells["Z4"].Value.ToString() + c1.min_spec;
                                //ws.Cells["H4"].Value = ws.Cells["H4"].Value.ToString() + highest;
                                //ws.Cells["N4"].Value = ws.Cells["N4"].Value.ToString() + lowest;
                                //ws.Cells["A4"].Value = c1.max_spec + c1.min_spec / 2;
                                decimal red_data_1 = lowest;
                                decimal red_data_2 = highest;

                                decimal perc60 = fulltol * 60 / 100;
                                decimal perc30 = fulltol * 90 / 100;
                                decimal perc10 = fulltol;

                                decimal green_data_start = lsl;
                                decimal green_data_end = perc60;

                                decimal yellow_min_start = perc60 + incr;
                                decimal yellow_min_end = perc30;

                                decimal orange_min_start = perc30 + incr;
                                decimal orange_min_end = perc10;


                                for (decimal i = highest; i >= lowest; i -= incr)
                                {
                                    Class_PmcSeries c = new Class_PmcSeries
                                    {
                                        fullset = i
                                    };
                                    if (i != highest && i != lowest)
                                        lstdata.Add(c);
                                }
                                // insert rows to the excel
                                //  ws.InsertRow(row, lstdata.Count);

                                // loop through data and fill excel file
                                foreach (Class_PmcSeries c in lstdata)
                                {
                                    ws.Cells["C" + row].Value = c.fullset;
                                    if (c.fullset == usl)
                                    {
                                        // ws.Cells["A" + row].Value = "USL";
                                        row_usl = row;
                                    }
                                    else if (c.fullset == lsl)
                                    {
                                        // ws.Cells["A" + row].Value = "LSL";
                                        row_lsl = row;
                                    }


                                    row += 2;
                                }
                                // delete 2 blank rows after data filled
                                // ws.DeleteRow(row);
                                // ws.DeleteRow(row);
                                // ws.Cells[5 + lstdata.Count + 1, 1].Value = "PROCESS LOG CODE";

                                ws.Cells[6, 3, row, 3].Style.Numberformat.Format = "0.000";
                                ws.Cells[6, 1, row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //ws.Cells[row_usl + 1, 1, row_lsl - 1, 1].Merge = true;
                                // ws.Cells[row_usl + 1, 1, row_lsl - 1, 1].Value = "(x̅ CHART) Average Chart:\r\n\r\nUpper Control Limit:\r\nUCL=X̿ + A₂R̅\r\nLower Control Limit:\r\nLCL=X̿ - A₂R̅\r\n\r\nHere,\r\n\r\nX̿ = (x̅1+x̅2+…+xn)/n\r\nn= no of x̅\r\nA₂= Constant table value\r\nR̅= (R1+R2+..Rn)/n\r\nn= No of R";
                                ws.Cells["A6"].Value = " AVERAGE CHART \n (X bar CHART)";


                                //ws.Cells[5 + lstdata.Count + 35, 1].Value = "F/Q/010 Rev: 0";
                                //string[] cpnumber = c1.cp_number.Split('/');
                                //ws.Cells[5 + lstdata.Count + 35, 21].Value = cpnumber[1] + cpnumber[2] + " / " + c1.process_no + "/ PMC - Rev No:0";
                                //ws.Cells[5 + lstdata.Count + 35, 21].Style.WrapText = true;
                                //ws.Cells[5 + lstdata.Count + 35, 21].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //ws.Cells[5 + lstdata.Count + 35, 21].Style.WrapText = true;
                                //  ws.Row(5 + lstdata.Count + 16).Height = 40;


                                currentEndRow = ws.Dimension.End.Row;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                                ws.Cells["A1:AS" + currentEndRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                                ws.PrinterSettings.PaperSize = ePaperSize.A3;
                                ws.PrinterSettings.PrintArea = ws.Cells["A1:AS" + (currentEndRow)];
                                ws.PrinterSettings.FitToPage = true;
                                ws.PrinterSettings.FitToWidth = 1;
                                countofchars += 1;
                            }

                        }

                        //xlPackage.Save();
                        xlPackage.SaveAs(excelFile);
                        string filePath = excelFile.ToString();
                        for (int i = 1; i <= noofsheets; i++)
                        {
                            string worksheetName = "Sheet_" + i;
                            int startColumn = 1;
                            int endColumn = 45;
                            int startRow = 6;
                            int endrow = currentEndRow;

                            AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo, Convert.ToInt32(rwhght));
                            // AutofitRowHeight(filePath, worksheetName, startRow, endrow, startColumn, endColumn, (List<Class_Employees>)userinfo);
                        }
                        pdfname = fname.Replace(".xlsx", ".pdf");
                        // var ret = ConvertXlstoPdf(fname, pdfname);
                        //return pdfname;


                    }
                    if (!string.IsNullOrWhiteSpace(pdfname))
                    {
                        var ret = ConvertXlstoPdf(fname, pdfname);
                    }
                    return pdfname;
                }
                else
                    return "";
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }

    }

    [WebMethod]
    public string CheckCPNumberExists(string cpnumber)
    {
        string sql = "SELECT COUNT(*) AS cnt FROM parts WHERE cp_number = @0";
        using (Database db = new Database("connString"))
        {
            int cnt = db.ExecuteScalar<int>(sql, cpnumber);
            string result = (cnt > 0) ? "exists" : "not exists";

            // Convert the result to a JSON object
            var response = new { result = result };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(response);
        }
    }

    [WebMethod(EnableSession = true)]
    public string GenerateLayoutInsp(int partsl, int oprnsl)
    {
        try
        {
            var userinfo = HttpContext.Current.Session["UserInfo"];
            int imgno = 0;
            string fname = "layout_" + partsl + "_" + oprnsl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filepath = "~/pdftemp/" + fname;
            FileInfo excelFile = new FileInfo(Server.MapPath(filepath));
            FileInfo templateFile = new FileInfo(Server.MapPath("~/App_Data/LayoutInspectionTemplate.xlsx"));
            string sql = @"SELECT  c.cp_slno FROM controlplan c where c.rev_no =(SELECT MAX(  CAST(p.rev_no  AS INT) ) from controlplan p WHERE " +
                       " p.Submitstatus='A' and p.Obsolete='N' AND c.part_slno=p.part_slno and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) and c.part_slno=" + partsl + " and c.operation_slno= " + oprnsl + "";
            string sqlrevdtls = @"SELECT rev_no, rev_date, rev_reason FROM controlplan c
                                  where c.part_slno =" + partsl + " and c.operation_slno= " + oprnsl + "";
            string sqllegend = @"select distinct Top 4 sp.splCharFile,spl_char_desc FROM[SpecialChars] sp inner join                             [mei_controlplan].[dbo].[customers] c on
                                c.cust_slno = sp.cust_slno inner join parts p on p.Customer_name = c.cust_name
                                inner join ControlPlan cp on cp.part_slno = p.part_slno
                                where cp.part_slno =" + partsl + " and cp.operation_slno =" + oprnsl + " and       sp.show_in_legend = 1";
            Logger.LogError(sql);
            string cp_slno = string.Empty;
            using (NPoco.Database db = new NPoco.Database("connString"))
            {
                cp_slno = db.ExecuteScalar<string>(sql);
                db.Execute(";exec SP_Temp_RptControlPlan @@cp_slno=@0,@@part_slno=@1, @@oper_slno=@2", cp_slno, partsl, oprnsl);
                List<Class_ControlPlan> lstrev = db.Fetch<Class_ControlPlan>(sqlrevdtls);
                string sqlcp = @"Select * from temp_rptControlPlan";
                List<Class_Temp_RptControlPlan> lst = db.Fetch<Class_Temp_RptControlPlan>(sqlcp);
                List<Class_SpecialChars> lstlegend = db.Fetch<Class_SpecialChars>(sqllegend);
                if (lst.Count > 0)
                {
                    double xlrowht = 90;
                    using (ExcelPackage xlPackage = new ExcelPackage(excelFile, templateFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                        ws.Cells[2, 1].Value = "Part Name: " + lst[0].PartDescription;
                        ws.Cells[2, 4].Value = "Revision Status: ";
                        ws.Cells[2, 7].Value = "Date: ";
                        ws.Cells[3, 1].Value = "Part No: " + lst[0].mstPartNo;
                        ws.Cells[3, 4].Value = "Customer: " + lst[0].Customer_name;
                        int row = 6;
                        ws.InsertRow(row, lst.Count);
                        string imgPath = Server.MapPath("~/Documents/");
                        int no = 1;
                        int index = (row + (lst.Count - 1) + 1);
                        int legendcount = lstlegend.Count;
                        if (lstlegend.Count > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(lstlegend[0].splCharFile))
                            {
                                addimage(lstlegend[0].splCharFile, index, 1, ws, "0");
                                ws.Cells[index, 1].Value = lstlegend[0].spl_char_desc;
                            }
                            if (lstlegend.Count > 1)
                            {
                                if (!string.IsNullOrWhiteSpace(lstlegend[1].splCharFile))
                                {
                                    addimage(lstlegend[1].splCharFile, index, 6, ws, "1");
                                    ws.Cells[index, 6].Value = lstlegend[1].spl_char_desc;
                                }
                            }
                        }
                        ws.Cells[index + 1, 1].Value = "Inspected By: " + lst[0].preparedBy;
                        ws.Cells[index + 1, 6].Value = "Approved By: " + lst[0].approvedBy;
                        foreach (Class_Temp_RptControlPlan c in lst)
                        {
                            ws.Cells[row, 1].Value = no.ToString();
                            ws.Cells[row, 2].Value = c.product_char;
                            ws.Cells[row, 4].Value = c.spec1;
                            //ws.Cells[row, 4].Value = c.proc_spec;
                            ws.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                            ws.Cells[row, 4].Style.WrapText = true;
                            ws.Cells[row, 5].Value = c.measurementTech;
                            ws.Row(row).Height = 50;
                            if (c.splChar_slno > 0)
                            {
                                string img = GetFileName(c.splChar_slno.ToString());
                                string pth = (imgPath + img);
                                FileInfo imgFile = new FileInfo(pth);
                                if (imgFile.Exists)
                                {
                                    imgno += 1;
                                    Bitmap image = new Bitmap(pth);
                                    ExcelPicture excelPicture = ws.Drawings.AddPicture("PictureName" + imgno, image);
                                    ExcelRangeBase cell = ws.Cells[row - 1, 2];
                                    excelPicture.SetPosition(cell.Start.Row, 10, cell.Start.Column, 13);
                                    excelPicture.SetSize(40, 40);
                                }
                            }
                            row += 1;
                            no++;
                        }
                        //ws.Cells["A16:K"+row].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        ws.Cells["A6:K" + row].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells["A6:K" + row].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells["A6:K" + row].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells["A6:K" + row].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        // set the print area                       
                        int newRowCount = ws.Dimension.End.Row + 1;
                        int currentEndRow = newRowCount;
                        ws.PrinterSettings.Orientation = eOrientation.Landscape;
                        ws.PrinterSettings.PrintArea = ws.Cells["A1:T" + currentEndRow];
                        ws.Protection.AllowEditObject = false;
                        xlPackage.SaveAs(excelFile);
                        string pdfname = fname.Replace(".xlsx", ".pdf");
                        var ret = ConvertXlstoPdf(fname, pdfname);
                        return pdfname;
                    }
                }
                return "";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(" Error in generating control plan: " + ex.ToString());
            return JsonConvert.SerializeObject(new { Message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLegendCount(string custsl)
    {
        string sql = "select count(*) as cnt from specialchars where cust_slno=@0 and show_in_legend=@1";
        using (Database db = new Database("connString"))
        {
            int cnt = db.ExecuteScalar<int>(sql, custsl, true);
            return cnt.ToString();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string CheckPartNoExists(string mstpartno)
    {
        string sql = "select count(*) as cnt from parts where mstpartno=@0 and del_status=@1";
        using (Database db = new Database("connString"))
        {
            int cnt = db.ExecuteScalar<int>(sql, mstpartno, "N");
            return cnt.ToString();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPartRevHistory(string partsl,string p)
    {
        using (Crud_part_revision_history crud = new Crud_part_revision_history())
        {
            using (Database db = new Database("connString"))
            {
                List<Class_parts> lstp = db.Query<Class_parts>("Select * from parts where mstpartno=@0   and part_slno =@1", p,Convert.ToInt32(partsl)).ToList();
                List<Class_part_revision_history> lst = crud.SelectAll().Where(x => x.part_slno == Convert.ToInt32(partsl)).ToList();

                //foreach (var part in lst)
                //{
                //    List<Class_part_revision_history> lst1 = crud.SelectAll().Where(x => x.part_slno == Convert.ToInt32(part.part_slno)).ToList();
                //    if (lst1.Count > 0)
                //    {
                //        foreach (var part1 in lst1)
                //        {
                //            var revHistory = new Class_part_revision_history
                //            {
                //                part_slno = Convert.ToInt32(part1.part_slno), // Example: you can adjust how to map part_slno
                //                change_nature = part1.change_nature,
                //                rev_no = part1.rev_no,
                //                rev_reasons = part1.rev_reasons,
                //                rev_date = part1.rev_date,
                //                revision_done_in = part1.revision_done_in,
                //                rev_slno = part1.rev_slno
                //            };

                //            // Add the mapped item to lst
                //            lst.Add(revHistory);
                //        }
                //    }
                //}
                lst = lst.OrderBy(x => x.rev_no).ToList();
                
                int revNo = 0;
                lst = lst.OrderBy(x => x.rev_slno).ToList();
                return JsonConvert.SerializeObject(lst);
            }
        }
    }

}
