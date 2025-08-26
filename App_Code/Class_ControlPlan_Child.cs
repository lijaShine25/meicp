using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Insight.Database;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("ControlPlan_Child")]
[PrimaryKey("cpc_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for ControlPlan_Child
/// </summary>
public class Class_ControlPlan_Child : IDisposable {
    public int cp_slno { get; set; }
    public int cpc_slno { get; set; }
    public int? evalTech_slno { get; set; }
    public int? freq_slno { get; set; }
    public int? method_slno { get; set; }
    public int? splChar_slno { get; set; }
    public String dimn_no { get; set; }
    public String gaugeCode { get; set; }
    public String process_char { get; set; }
    public String product_char { get; set; }
    public String reactionPlan { get; set; }
    public String res { get; set; }
    public String sampleSize { get; set; }
    public String tol_max { get; set; }
    public String tol_min { get; set; }
    public int? evalTech_slno2 { get; set; }
    public int? method_slno2 { get; set; }
    public int? freq_slno2 { get; set; }
    public String sampleSize2 { get; set; }
    public string spec1 { get; set; }
    public string spec2 { get; set; }

    [ResultColumn]
    public string evalTech { get; set; }
    [ResultColumn]
    public string methodDesc { get; set; }
    [ResultColumn]
    public string FreqDesc { get; set; }
    [ResultColumn]
    public string splfilename { get; set; }
    [ResultColumn]
    public string status1 { get; set; }
    [ResultColumn]
    public string evalTech2 { get; set; }
    [ResultColumn]
    public string methodDesc2 { get; set; }
    [ResultColumn]
    public string FreqDesc2 { get; set; }
    [ResultColumn]
    public string splfilename2 { get; set; }
    public decimal? min_spec { get; set; }
    public decimal? max_spec { get; set; }
    public decimal? tolerance { get; set; }

    public string PDI_type{ get; set; }

    #region "IDisposable Support"
    // To detect redundant calls
    private bool disposedValue;

    // IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            // TODO: set large fields to null.
        }
        this.disposedValue = true;
    }

    // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    //Protected Overrides Sub Finalize()
    //    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    //    Dispose(False)
    //    MyBase.Finalize()
    //End Sub

    // This code added by Visual Basic to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
/// <summary>
/// The CRUD class for ControlPlan_Child
/// </summary>
public class Crud_ControlPlan_Child : IControlPlan_Child, IDisposable
{
IControlPlan_Child iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_ControlPlan_Child()
{
   iObj = conn.As<IControlPlan_Child>();
}

   /// <summary>
   /// inserts data in table - ControlPlan_Child
   /// </summary>
   /// <param name="objName">objName as Class_ControlPlan_Child</param>
   public void usp_ControlPlan_ChildInsert(Class_ControlPlan_Child objName)
   {
       iObj.usp_ControlPlan_ChildInsert(objName);
   }

   /// <summary>
   /// updates data in table - ControlPlan_Child
   /// </summary>
   /// <param name="objName">objName as Class_ControlPlan_Child</param>
   public void usp_ControlPlan_ChildUpdate(Class_ControlPlan_Child objName)
   {
       iObj.usp_ControlPlan_ChildUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - ControlPlan_Child
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_ControlPlan_ChildDelete(Class_ControlPlan_Child objName)
   {
       iObj.usp_ControlPlan_ChildDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - ControlPlan_Child
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   ///  public IList<Class_ControlPlan_Child> usp_ControlPlan_ChildSelect(int cslno)
   public IList<Class_ControlPlan_Child> usp_ControlPlan_ChildSelect()
   {
      
       return iObj.usp_ControlPlan_ChildSelect().ToList();
   }

   public List<Class_ControlPlan_Child> GetChildData(int cslno) {

       
         List<Class_ControlPlan_Child> lstcd =  usp_ControlPlan_ChildSelect().ToList();
         List<Class_ControlPlan_Child> lst = lstcd.Where(x => x.cp_slno == cslno).ToList();
         foreach (Class_ControlPlan_Child c in lst)
         { 

             c.FreqDesc =GetFreq(c.freq_slno);
                 c.evalTech=GetEveltech(c.evalTech_slno);
                 c.methodDesc = GetControldesc(c.method_slno);
                 c.splfilename = Getsplfilename(c.splChar_slno);
            c.FreqDesc2 = GetFreq(c.freq_slno2);
            c.evalTech2 = GetEveltech(c.evalTech_slno2);
            c.methodDesc2 = GetControldesc(c.method_slno2);


        }

         return lst;
       

   }
   string  GetEveltech(int? evlslno)
   {
       if (evlslno > 0)
       {
           using (Database db = new Database("connString"))
           {
               //var et = db.SingleOrDefault<Class_EvaluationTech>(" where evalTech_slno=@0", evlslno);

               //if (et != null)
               //{
               //    return et.evalTech;
               //}
               //else
               //{
               //    return "";
               //}
               string evalTech = db.ExecuteScalar<string>("select evalTech from EvaluationTech where evalTech_slno=@0", evlslno);
               if (evalTech != null)
               {
                   return evalTech;
               }
               else
               {
                   return "";
               }

           }
       }
       else
       {
           return "";
       }
   }

   string  GetFreq(int? freqslno)
   {
       if (freqslno > 0)
       {
           using (Database db = new Database("connString"))
           {
               //return db.SingleOrDefault<Class_SampleFrequency>(" where freq_slno=@0", freqslno).FreqDesc;
               string FreqDesc = db.ExecuteScalar<string>("select FreqDesc from SampleFrequency where freq_slno=@0", freqslno);
               return FreqDesc;
           }

       }
       else
       {
           return "";
       }
   }

   string  GetControldesc(int? ctrl)
   {
       if (ctrl > 0)
       {
           using (Database db = new Database("connString"))
           {
               string methodDesc = db.ExecuteScalar<string>("select methodDesc from ControlMethods where method_slno=@0", ctrl);
               return methodDesc;
              // return db.SingleOrDefault<Class_ControlMethods>(" where method_slno=@0", ctrl).methodDesc;
           }
       }
       else
       {
           return "";
       }
   }
   string Getsplfilename(int? splslno) {
       if (splslno > 0)
       {
           using (Database db = new Database("connString"))
           {
               //return db.SingleOrDefault<Class_SpecialChars>(" where splChar_slno=@0", splslno).splCharFile;
               string splCharFile = db.ExecuteScalar<string>("select splCharFile from SpecialChars where splChar_slno=@0", splslno);
               return splCharFile;

           }
       }
       else
       {
           return "";
       }

   }
#region "IDisposable Support"
    // To detect redundant calls
    private bool disposedValue;

    // IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            // TODO: set large fields to null.
        }
        this.disposedValue = true;
    }

    // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    //Protected Overrides Sub Finalize()
    //    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    //    Dispose(False)
    //    MyBase.Finalize()
    //End Sub

    // This code added by Visual Basic to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

}
