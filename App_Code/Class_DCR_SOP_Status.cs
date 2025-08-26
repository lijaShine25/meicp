using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("DCR_SOP_Status")]
[PrimaryKey("slno")]

/// <summary>
/// properties defined for customers
/// </summary>
public class Class_DCR_SOP_Status : IDisposable {
    public int slno { get; set; }
    public int dcr_slno { get; set; }          
    public int part_slno { get; set; }
    public int Group_Id { get; set; }
    public int operation_slno { get; set; }
    public string status { get; set; }

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
public class Crud_DCR_SOP_Status : IDisposable
{
   public Crud_DCR_SOP_Status()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - customers
   /// </summary>
   /// <param name="objName">objName as Class_customers</param>
   public void Insert(Class_DCR_SOP_Status objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_DCR_SOP_Status>(objName);
       }
   }

   /// <summary>
   /// updates data in table - customers
   /// </summary>
   /// <param name="objName">objName as Class_customers</param>
   public void Update(Class_DCR_SOP_Status objName)
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
           db.DeleteWhere<Class_DCR_SOP_Status>("dcr_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - customers
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_DCR_SOP_Status> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_DCR_SOP_Status>();
       }
   }

   /// <summary>
   /// selects required data from table - customers
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_DCR_SOP_Status SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_DCR_SOP_Status>(id);
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
