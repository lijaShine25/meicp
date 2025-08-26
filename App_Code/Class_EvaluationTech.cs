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

[TableName("EvaluationTech")]
[PrimaryKey("evalTech_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for EvaluationTech
/// </summary>
public class Class_EvaluationTech : IDisposable {
    public int evalTech_slno { get; set; }
    public String del_status { get; set; }
    public String evalTech { get; set; }
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
/// The CRUD class for EvaluationTech
/// </summary>
public class Crud_EvaluationTech : IEvaluationTech, IDisposable
{
IEvaluationTech iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_EvaluationTech()
{
   iObj = conn.As<IEvaluationTech>();
}

   /// <summary>
   /// inserts data in table - EvaluationTech
   /// </summary>
   /// <param name="objName">objName as Class_EvaluationTech</param>
   public void usp_EvaluationTechInsert(Class_EvaluationTech objName)
   {
       iObj.usp_EvaluationTechInsert(objName);
   }

   /// <summary>
   /// updates data in table - EvaluationTech
   /// </summary>
   /// <param name="objName">objName as Class_EvaluationTech</param>
   public void usp_EvaluationTechUpdate(Class_EvaluationTech objName)
   {
       iObj.usp_EvaluationTechUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - EvaluationTech
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_EvaluationTechDelete(Class_EvaluationTech objName)
   {
       iObj.usp_EvaluationTechDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - EvaluationTech
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_EvaluationTech> usp_EvaluationTechSelect()
   {

       return iObj.usp_EvaluationTechSelect().ToList();
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
