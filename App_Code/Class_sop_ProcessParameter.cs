using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("sop_ProcessParameter")]
[PrimaryKey("SlNo")]

/// <summary>
/// properties defined for sop_header
/// </summary>
public class Class_sop_ProcessParameter
{
    public int SlNo { get; set; }
    public int sop_id { get; set; }
    public string ProcessParameter { get; set; }
    public string Specification { get; set; }
    public string CheckingMethod { get; set; }
    public string Frequency { get; set; }
    public string ToolOfControl { get; set; }

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
/// The CRUD class for sop_header
/// </summary>
public class Crud_sop_ProcessParameter : IDisposable
{
    public Crud_sop_ProcessParameter()
    {
        // 
    }

    /// <summary>
    /// inserts data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_header</param>
    public void Insert(Class_sop_ProcessParameter objName)
    {

        using (Database db = new Database("connString"))
        {
            db.Insert<Class_sop_ProcessParameter>(objName);
        }
    }

    /// <summary>
    /// updates data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_header</param>
    public void Update(Class_sop_ProcessParameter objName)
    {
        using (Database db = new Database("connString"))
        {
            db.Update(objName);
        }
    }

    /// <summary>
    /// deletes specified data from table - sop_header
    /// </summary>
    /// <param name="id">id as Integer</param>
    public void Delete(int id)
    {
        using (Database db = new Database("connString"))
        {
            db.DeleteWhere<Class_sop_ProcessParameter>("SlNo=@0", id);
        }
    }

    /// <summary>
    /// selects all data from table - sop_header
    /// </summary>
    /// <param></param>
    /// <returns>Returns data as list object</returns>
    public List<Class_sop_ProcessParameter> SelectAll()
    {
        using (Database db = new Database("connString"))
        {
            return db.Fetch<Class_sop_ProcessParameter>();
        }
    }

    /// <summary>
    /// selects required data from table - sop_header
    /// </summary>
    /// <param name="id">id as Integer</param>
    /// <returns>Returns data as class object</returns>
    public Class_sop_ProcessParameter SelectOne(int id)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefaultById<Class_sop_ProcessParameter>(id);
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
