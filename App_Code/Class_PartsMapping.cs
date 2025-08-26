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

[TableName("PartsMapping")]
[PrimaryKey("map_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for PartsMapping
/// </summary>
public class Class_PartsMapping : IDisposable {
    public int machine_slno { get; set; }
    public int map_slno { get; set; }
    public int operation_slno { get; set; }
    public int part_slno { get; set; }

    public string OperationDesc { get; set; }
    public string MachineDesc { get; set; }
    public string PartDescription { get; set; }
    public string mstPartNo { get; set; }
    public string Obsolete { get; set; }
    public string process_no { get; set; }
    [ResultColumn]
    public int cp_slno { get; set; }
    [ResultColumn]
   public string cpdone { get; set; }
    [ResultColumn]
    public string  Submitstatus { get; set; }
    [ResultColumn]
    public string  is_approved { get; set; }
    [ResultColumn]
    public int  approvedby { get; set; }
    [ResultColumn]
    public DateTime? CP_Submit_DateTime { get; set; }
    [ResultColumn]
    public DateTime? CP_Approved_DateTime { get; set; }

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
/// The CRUD class for PartsMapping
/// </summary>
public class Crud_PartsMapping : IPartsMapping, IDisposable
{
IPartsMapping iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_PartsMapping()
{
   iObj = conn.As<IPartsMapping>();
}

   /// <summary>
   /// inserts data in table - PartsMapping
   /// </summary>
   /// <param name="objName">objName as Class_PartsMapping</param>
   public void usp_PartsMappingInsert(Class_PartsMapping objName)
   {
       iObj.usp_PartsMappingInsert(objName);
   }

   /// <summary>
   /// updates data in table - PartsMapping
   /// </summary>
   /// <param name="objName">objName as Class_PartsMapping</param>
   public void usp_PartsMappingUpdate(Class_PartsMapping objName)
   {
       iObj.usp_PartsMappingUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - PartsMapping
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_PartsMappingDelete(Class_PartsMapping objName)
   {
       iObj.usp_PartsMappingDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - PartsMapping
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_PartsMapping> usp_PartsMappingSelect()
   {
       return iObj.usp_PartsMappingSelect().ToList();
   }
    public IList<Class_PartsMapping> usp_PartsMappingSelect_CPApproval()
    {
        return iObj.usp_PartsMappingSelect_CPApproval().ToList();
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
