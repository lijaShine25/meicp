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

[TableName("ControlMethods")]
[PrimaryKey("method_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for ControlMethods
/// </summary>
public class Class_ControlMethods : IDisposable {
    public int method_slno { get; set; }
    public String del_status { get; set; }
    public String methodDesc { get; set; }
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
/// The CRUD class for ControlMethods
/// </summary>
public class Crud_ControlMethods : IControlMethods, IDisposable
{
IControlMethods iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_ControlMethods()
{
   iObj = conn.As<IControlMethods>();
}

   /// <summary>
   /// inserts data in table - ControlMethods
   /// </summary>
   /// <param name="objName">objName as Class_ControlMethods</param>
   public void usp_ControlMethodsInsert(Class_ControlMethods objName)
   {
       iObj.usp_ControlMethodsInsert(objName);
   }

   /// <summary>
   /// updates data in table - ControlMethods
   /// </summary>
   /// <param name="objName">objName as Class_ControlMethods</param>
   public void usp_ControlMethodsUpdate(Class_ControlMethods objName)
   {
       iObj.usp_ControlMethodsUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - ControlMethods
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_ControlMethodsDelete(Class_ControlMethods objName)
   {
       iObj.usp_ControlMethodsDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - ControlMethods
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_ControlMethods> usp_ControlMethodsSelect()
   {
       return iObj.usp_ControlMethodsSelect().ToList();
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
