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

[TableName("parts")]
[PrimaryKey("part_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for parts
/// </summary>
public class Class_parts : IDisposable {
    public int CftTeamSlNo { get; set; }
    [ResultColumn]
    public int part_slno { get; set; }
    public String custApproval { get; set; }
    public String custApprovalDt { get; set; }
    public String customerIssueDt { get; set; }
    public String customerIssueNo { get; set; }
    public String customerPartNo { get; set; }
    public String custQaApproval { get; set; }
    public String custQaApprovalDt { get; set; }
    public String keyContact { get; set; }
    public String keyContactPhone { get; set; }
    public String mstPartNo { get; set; }
    public String organization { get; set; }
    public String orgApprovalDt { get; set; }
    public String orgDate { get; set; }
    public String originalDt { get; set; }
    public String otherApproval { get; set; }
    public String otherApprovalDt { get; set; }
    public String PartDescription { get; set; }
    public String partIssueDt { get; set; }
    public String partIssueNo { get; set; }
    public String uploadfile1 { get; set; }
    public String uploadfile2 { get; set; }
    public String uploadfile3 { get; set; }
    public String uploadfile4 { get; set; }
    public string cpType { get; set; }
    public string Obsolete { get; set; }
    public int preparedBy { get; set; }
    public int approvedBy { get; set; }
    public string rev_reason { get; set; }
    public string change_nature { get; set; }

    [ResultColumn]
    public string status1 { get; set; }

    public String Remarks { get; set; }

    [ResultColumn]
    public String CFTeamName { get; set; }
    public String del_status { get; set; }
    public String Customer_name { get; set; }
    [ResultColumn]
    public string part_doc { get; set; }

    public string cp_number { get; set; }
    public string cp_revno { get; set; }
    public string cp_revdt { get; set; }
    public string supplier_code { get; set; }
    public string proc_spec { get; set; }
    public string ih_testing_ref { get; set; }


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
/// The CRUD class for parts
/// </summary>
public class Crud_parts : Iparts, IDisposable
{
Iparts iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_parts()
{
   iObj = conn.As<Iparts>();
}

   /// <summary>
   /// inserts data in table - parts
   /// </summary>
   /// <param name="objName">objName as Class_parts</param>
public IList<Class_parts> usp_partsInsert(Class_parts objName)
{
    List<Class_parts> lst = iObj.usp_partsInsert(objName).ToList();
    return lst;
}


   /// <summary>
   /// updates data in table - parts
   /// </summary>
   /// <param name="objName">objName as Class_parts</param>
   public void usp_partsUpdate(Class_parts objName)
   {
       iObj.usp_partsUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - parts
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_partsDelete(Class_parts objName)
   {
       iObj.usp_partsDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - parts
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_parts> usp_partsSelect()
   {
       return iObj.usp_partsSelect().ToList();
   }
    public IList<Class_parts> usp_PartsMappingSelect()
    {
        return iObj.usp_PartsMappingSelect().ToList();
    }
    public IList<Class_parts> usp_partsSelect_mapping()
    {
        return iObj.usp_partsSelect_mapping().ToList();
    }


    public Class_parts SelectOne(int id)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefaultById<Class_parts>(id);
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
