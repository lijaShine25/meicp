using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("sop_toolings")]
[PrimaryKey("sop_tool_slno")]

/// <summary>
/// properties defined for sop_toolings
/// </summary>
public class Class_sop_toolings : IDisposable {
    public int sop_tool_slno { get; set; }
    public int sop_id { get; set; }
    public String tool_holder_name { get; set; }
    public String tool { get; set; }
    public String cutting_speed { get; set; }
    public int? per_corner { get; set; }
    public int? no_of_corners { get; set; }
    public int? total_nos { get; set; }
    public String control_method { get; set; }
    public String feed_rate { get; set; }
   

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
/// The CRUD class for sop_toolings
/// </summary>
public class Crud_sop_toolings : IDisposable
{
   public Crud_sop_toolings()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - sop_toolings
   /// </summary>
   /// <param name="objName">objName as Class_sop_toolings</param>
   public void Insert(Class_sop_toolings objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_sop_toolings>(objName);
       }
   }

   /// <summary>
   /// updates data in table - sop_toolings
   /// </summary>
   /// <param name="objName">objName as Class_sop_toolings</param>
   public void Update(Class_sop_toolings objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Update(objName);
       }
   }

   /// <summary>
   /// deletes specified data from table - sop_toolings
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void Delete(int id)
   {
       using (Database db = new Database("connString"))
       {
           db.DeleteWhere<Class_sop_toolings>("sop_tool_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - sop_toolings
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_sop_toolings> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_sop_toolings>();
       }
   }

   /// <summary>
   /// selects required data from table - sop_toolings
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_sop_toolings SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_sop_toolings>(id);
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
