using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("sop_ProcessParameternew")]
[PrimaryKey("SlNo")]

/// <summary>
/// properties defined for sop_header
/// </summary>
public class Class_sop_ProcessParameter_new
{
    public int SlNo { get; set; }
    public int sop_id { get; set; }
    public int Map_slno { get; set; }
    //public string Program_No { get; set; }
    //public string Hydraulic_System_Pressure { get; set; }
    //public string Through_Coolant_Pressure { get; set; }
    //public string Component_Orientation { get; set; }
    public string Parameter { get; set; }
    public string Param_Value { get; set; }
    public string CheckingMethod { get; set; }
    public string Frequency { get; set; }
    public string Control_Method { get; set; }
    //public string Coolant_Concentration { get; set; }
    public int Group_Id { get; set; }
    public string Template { get; set; }
    [ResultColumn]
    public string  partIssueNo { get; set; }
    [ResultColumn]
    public string mstPartno { get; set; }
    [ResultColumn]
    public string partIssueDt { get; set; }
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
public class Crud_sop_ProcessParameter_new : IDisposable
{
    public Crud_sop_ProcessParameter_new()
    {
        // 
    }

    /// <summary>
    /// inserts data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_header</param>
    public void Insert(Class_sop_ProcessParameter_new objName)
    {

        using (Database db = new Database("connString"))
        {
            db.Insert<Class_sop_ProcessParameter_new>(objName);
        }
    }

    /// <summary>
    /// updates data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_header</param>
    public void Update(Class_sop_ProcessParameter_new objName)
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
            db.DeleteWhere<Class_sop_ProcessParameter_new>("SlNo=@0", id);
        }
    }

    /// <summary>
    /// selects all data from table - sop_header
    /// </summary>
    /// <param></param>
    /// <returns>Returns data as list object</returns>
    public List<Class_sop_ProcessParameter_new> SelectAll()
    {
        using (Database db = new Database("connString"))
        {
            return db.Fetch<Class_sop_ProcessParameter_new>();
        }
    }

    /// <summary>
    /// selects required data from table - sop_header
    /// </summary>
    /// <param name="id">id as Integer</param>
    /// <returns>Returns data as class object</returns>
    public Class_sop_ProcessParameter_new SelectOne(int id)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefaultById<Class_sop_ProcessParameter_new>(id);
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
