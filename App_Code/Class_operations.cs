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

[TableName("operations")]
[PrimaryKey("operation_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for operations
/// </summary>
public class Class_operations : IDisposable {
    public int operation_slno { get; set; }
    public String del_status { get; set; }
    public String OperationDesc { get; set; }
    [ResultColumn]
    public String status1 { get; set; }

    public bool PMC { get; set; }
    public bool DOCK { get; set; }
    public bool FOI { get; set; }
    public bool PCC { get; set; }
    public bool SOP { get; set; }
    public bool PDI { get; set; }

    public bool PDI_2PerSkid { get; set; }

    public bool PDI_EveryHour { get; set; }



    //public String is_superuser { get; set; }

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
/// The CRUD class for operations
/// </summary>
public class Crud_operations : Ioperations, IDisposable
{
Ioperations iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_operations()
{
   iObj = conn.As<Ioperations>();
}

   /// <summary>
   /// inserts data in table - operations
   /// </summary>
   /// <param name="objName">objName as Class_operations</param>
   public void usp_operationsInsert(Class_operations objName)
   {
       iObj.usp_operationsInsert(objName);
   }

   /// <summary>
   /// updates data in table - operations
   /// </summary>
   /// <param name="objName">objName as Class_operations</param>
   public void usp_operationsUpdate(Class_operations objName)
   {
       iObj.usp_operationsUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - operations
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_operationsDelete(Class_operations objName)
   {
       iObj.usp_operationsDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - operations
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_operations> usp_operationsSelect()
   {
       return iObj.usp_operationsSelect().ToList();
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
