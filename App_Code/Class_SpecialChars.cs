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

[TableName("SpecialChars")]
[PrimaryKey("splChar_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for SpecialChars
/// </summary>
public class Class_SpecialChars : IDisposable {
    public int splChar_slno { get; set; }
    public String del_status { get; set; }
    public String splCharFile { get; set; }
    public int cust_slno { get;set; }
    public string spl_char_desc { get; set; }
    public bool show_in_legend { get; set; }

    [ResultColumn]
    public string cust_name { get; set; }   
    
    [ResultColumn]
    public string status1 { get; set; }

    [ResultColumn]
    public string imageFile { get; set; }

    [ResultColumn]
    public string show_in_legend2 { get; set; }

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
/// The CRUD class for SpecialChars
/// </summary>
public class Crud_SpecialChars : ISpecialChars, IDisposable
{
ISpecialChars iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_SpecialChars()
{
   iObj = conn.As<ISpecialChars>();
}

   /// <summary>
   /// inserts data in table - SpecialChars
   /// </summary>
   /// <param name="objName">objName as Class_SpecialChars</param>
   public void usp_SpecialCharsInsert(Class_SpecialChars objName)
   {
       iObj.usp_SpecialCharsInsert(objName);
   }

   /// <summary>
   /// updates data in table - SpecialChars
   /// </summary>
   /// <param name="objName">objName as Class_SpecialChars</param>
   public void usp_SpecialCharsUpdate(Class_SpecialChars objName)
   {
       iObj.usp_SpecialCharsUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - SpecialChars
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_SpecialCharsDelete(Class_SpecialChars objName)
   {
       iObj.usp_SpecialCharsDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - SpecialChars
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_SpecialChars> usp_SpecialCharsSelect()
   {
       return iObj.usp_SpecialCharsSelect().ToList();
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
