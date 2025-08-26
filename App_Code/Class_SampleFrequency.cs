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

[TableName("SampleFrequency")]
[PrimaryKey("freq_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for SampleFrequency
/// </summary>
public class Class_SampleFrequency : IDisposable {
    public int freq_slno { get; set; }
    public String del_status { get; set; }
    public String FreqDesc { get; set; }
    public bool foi { get; set; }
    public bool pcc { get; set; }
    public bool pmc { get; set; }
    public bool material_test_report { get; set; }
    public string sample_size { get; set; }
    public bool packing { get; set; }
    public bool dockaudit { get; set; }

     [ResultColumn]
    public string status1 { get; set; }
    [ResultColumn]
    public string foi_txt { get; set; }
    [ResultColumn]
    public string pcc_txt { get; set; }
    [ResultColumn]
    public string pmc_txt { get; set; }
    [ResultColumn]
    public string mtl_txt { get; set; }
    [ResultColumn]
    public string packing_txt { get; set; }
    [ResultColumn]
    public string docaudit_txt { get; set; }

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
/// The CRUD class for SampleFrequency
/// </summary>
public class Crud_SampleFrequency : ISampleFrequency, IDisposable
{
ISampleFrequency iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_SampleFrequency()
{
   iObj = conn.As<ISampleFrequency>();
}

   /// <summary>
   /// inserts data in table - SampleFrequency
   /// </summary>
   /// <param name="objName">objName as Class_SampleFrequency</param>
   public void usp_SampleFrequencyInsert(Class_SampleFrequency objName)
   {
       iObj.usp_SampleFrequencyInsert(objName);
   }

   /// <summary>
   /// updates data in table - SampleFrequency
   /// </summary>
   /// <param name="objName">objName as Class_SampleFrequency</param>
   public void usp_SampleFrequencyUpdate(Class_SampleFrequency objName)
   {
       iObj.usp_SampleFrequencyUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - SampleFrequency
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_SampleFrequencyDelete(Class_SampleFrequency objName)
   {
       iObj.usp_SampleFrequencyDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - SampleFrequency
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_SampleFrequency> usp_SampleFrequencySelect()
   {
       return iObj.usp_SampleFrequencySelect().ToList();
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
