using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("mdMaster")]
[PrimaryKey("md_slno")]

/// <summary>
/// properties defined for mdMaster
/// </summary>
public class Class_mdMaster : IDisposable {
    public long md_slno { get; set; }
    public String equip_number { get; set; }
    public String md_category { get; set; }

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
/// The CRUD class for mdMaster
/// </summary>
public class Crud_mdMaster : IDisposable
{
   public Crud_mdMaster()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - mdMaster
   /// </summary>
   /// <param name="objName">objName as Class_mdMaster</param>
   public void Insert(Class_mdMaster objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_mdMaster>(objName);
       }
   }

   /// <summary>
   /// updates data in table - mdMaster
   /// </summary>
   /// <param name="objName">objName as Class_mdMaster</param>
   public void Update(Class_mdMaster objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Update(objName);
       }
   }

   /// <summary>
   /// deletes specified data from table - mdMaster
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void Delete(int id)
   {
       using (Database db = new Database("connString"))
       {
           db.DeleteWhere<Class_mdMaster>("md_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - mdMaster
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_mdMaster> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_mdMaster>();
       }
   }

   /// <summary>
   /// selects required data from table - mdMaster
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_mdMaster SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_mdMaster>(id);
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
