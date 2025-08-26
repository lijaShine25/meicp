using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("customers")]
[PrimaryKey("cust_slno")]

/// <summary>
/// properties defined for customers
/// </summary>
public class Class_customers : IDisposable {
    public int cust_slno { get; set; }
    public String cust_name { get; set; }
    public bool del_status { get; set; }

    [ResultColumn]
    public string Active { get; set; }

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
/// The CRUD class for customers
/// </summary>
public class Crud_customers : IDisposable
{
   public Crud_customers()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - customers
   /// </summary>
   /// <param name="objName">objName as Class_customers</param>
   public void Insert(Class_customers objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_customers>(objName);
       }
   }

   /// <summary>
   /// updates data in table - customers
   /// </summary>
   /// <param name="objName">objName as Class_customers</param>
   public void Update(Class_customers objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Update(objName);
       }
   }

   /// <summary>
   /// deletes specified data from table - customers
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void Delete(int id)
   {
       using (Database db = new Database("connString"))
       {
           db.DeleteWhere<Class_customers>("cust_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - customers
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_customers> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_customers>();
       }
   }

   /// <summary>
   /// selects required data from table - customers
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_customers SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_customers>(id);
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
