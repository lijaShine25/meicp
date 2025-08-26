using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("part_revision_history")]
[PrimaryKey("rev_slno", AutoIncrement =true)]

/// <summary>
/// properties defined for part_revision_history
/// </summary>
public class Class_part_revision_history : IDisposable {
    public int rev_slno { get; set; }
    public int part_slno { get; set; }
    public string rev_no { get; set; }
    public String rev_reasons { get; set; }
    public String rev_date { get; set; }
    public string change_nature { get; set; }
    [ResultColumn]
    public string mstpartno { get; set; }
    [ResultColumn]
    public string PartDescription { get; set; }
    public string revision_done_in { get; set; }

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
/// The CRUD class for part_revision_history
/// </summary>
public class Crud_part_revision_history : IDisposable
{
   public Crud_part_revision_history()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - part_revision_history
   /// </summary>
   /// <param name="objName">objName as Class_part_revision_history</param>
   public void Insert(Class_part_revision_history objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_part_revision_history>(objName);
       }
   }

   /// <summary>
   /// updates data in table - part_revision_history
   /// </summary>
   /// <param name="objName">objName as Class_part_revision_history</param>
   public void Update(Class_part_revision_history objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Update(objName);
       }
   }

   /// <summary>
   /// deletes specified data from table - part_revision_history
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void Delete(int id)
   {
       using (Database db = new Database("connString"))
       {
           db.DeleteWhere<Class_part_revision_history>("rev_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - part_revision_history
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_part_revision_history> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_part_revision_history>();
       }
   }

   /// <summary>
   /// selects required data from table - part_revision_history
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_part_revision_history SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_part_revision_history>(id);
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
