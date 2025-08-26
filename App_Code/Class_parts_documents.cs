using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("parts_documents")]
[PrimaryKey("part_doc_slno")]

/// <summary>
/// properties defined for parts_documents
/// </summary>
public class Class_parts_documents : IDisposable {
    public int part_doc_slno { get; set; }
    public int part_slno { get; set; }
    public String doc_title { get; set; }
    public String doc_filename { get; set; }
    [ResultColumn]
    public string mstpartno { get; set; }
    [ResultColumn]
    public string PartDescription { get; set; }
    [ResultColumn]
    public string partIssueNo { get; set; }
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
/// The CRUD class for parts_documents
/// </summary>
public class Crud_parts_documents : IDisposable
{
   public Crud_parts_documents()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - parts_documents
   /// </summary>
   /// <param name="objName">objName as Class_parts_documents</param>
   public void Insert(Class_parts_documents objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_parts_documents>(objName);
       }
   }

   /// <summary>
   /// updates data in table - parts_documents
   /// </summary>
   /// <param name="objName">objName as Class_parts_documents</param>
   public void Update(Class_parts_documents objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Update(objName);
       }
   }

   /// <summary>
   /// deletes specified data from table - parts_documents
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void Delete(int id)
   {
       using (Database db = new Database("connString"))
       {
           db.DeleteWhere<Class_parts_documents>("part_doc_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - parts_documents
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_parts_documents> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_parts_documents>();
       }
   }

   /// <summary>
   /// selects required data from table - parts_documents
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_parts_documents SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_parts_documents>(id);
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
