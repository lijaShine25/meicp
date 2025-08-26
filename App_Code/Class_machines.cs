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

[TableName("machines")]
[PrimaryKey("machine_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for machines
/// </summary>
public class Class_machines : IDisposable {
    public int machine_slno { get; set; }
    public int operation_slno { get; set; }
    [ResultColumn]
    public string OperationDesc { get; set; }
    public string del_status { get; set; }
    public string MachineCode { get; set; }
    public string MachineDesc { get; set; }
     [ResultColumn]
    public string status1 { get; set; }

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
/// The CRUD class for machines
/// </summary>
public class Crud_machines : Imachines, IDisposable
{
Imachines iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_machines()
{
   iObj = conn.As<Imachines>();
}

   /// <summary>
   /// inserts data in table - machines
   /// </summary>
   /// <param name="objName">objName as Class_machines</param>
   public void usp_machinesInsert(Class_machines objName)
   {
       iObj.usp_machinesInsert(objName);
   }

   /// <summary>
   /// updates data in table - machines
   /// </summary>
   /// <param name="objName">objName as Class_machines</param>
   public void usp_machinesUpdate(Class_machines objName)
   {
       iObj.usp_machinesUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - machines
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_machinesDelete(Class_machines objName)
   {
       iObj.usp_machinesDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - machines
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_machines> usp_machinesSelect()
   {
       return iObj.usp_machinesSelect().ToList();
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
