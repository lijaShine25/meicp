using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("DCR_SOP")]
[PrimaryKey("dcr_slno")]

/// <summary>
/// properties defined for DCR_SOP
/// </summary>
public class Class_DCR_SOP : IDisposable {
    public int dcr_slno { get; set; }
    public String dcr_number { get; set; }
    public int Group_Id { get; set; }
    public String change_area { get; set; }
    public String Process_Number { get; set; }
    public String Process_Name { get; set; }
    public String Prc_Characteristics { get; set; }
    public String Prd_Characteristics { get; set; }
    public String Specification { get; set; }
    public String Measurement_Tech { get; set; }
    public String Sample_Size { get; set; }
    public String Sample_Frequency { get; set; }
    public String Control_Method { get; set; }
    public String Others { get; set; }
    public String Existing { get; set; }
    public String Changes_Required { get; set; }
    public String Reason_For_Change { get; set; }
    public String nature_of_change { get; set; }
    public int? Request_By { get; set; }
    public String Request_Date { get; set; }
    public String del_status { get; set; }
    public String Submit_Status { get; set; }
    public DateTime? DCR_Submit_DateTime { get; set; }
    public DateTime? DCR_Approved_DateTime { get; set; }
    [ResultColumn]
    public string Group_Name { get; set; }
    [ResultColumn]
    public string employeename { get; set; }

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

public class Crud_dcr_sop : IDisposable
{
    public Crud_dcr_sop()
    {
        // 
    }

    /// <summary>
    /// inserts data in table - customers
    /// </summary>
    /// <param name="objName">objName as Class_customers</param>
    public void Insert(Class_DCR_SOP objName)
    {
        using (Database db = new Database("connString"))
        {
            db.Insert<Class_DCR_SOP>(objName);
        }
    }

    /// <summary>
    /// updates data in table - customers
    /// </summary>
    /// <param name="objName">objName as Class_customers</param>
    public void Update(Class_DCR_SOP objName)
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
            db.DeleteWhere<Class_DCR_SOP>("cust_slno=@0", id);
        }
    }

    /// <summary>
    /// selects all data from table - customers
    /// </summary>
    /// <param></param>
    /// <returns>Returns data as list object</returns>
    public List<Class_DCR_SOP> SelectAll()
    {
        using (Database db = new Database("connString"))
        {
            return db.Fetch<Class_DCR_SOP>();
        }
    }

    /// <summary>
    /// selects required data from table - customers
    /// </summary>
    /// <param name="id">id as Integer</param>
    /// <returns>Returns data as class object</returns>
    public Class_DCR_SOP SelectOne(int id)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefaultById<Class_DCR_SOP>(id);
        }
    }
    public List<Class_DCR_SOP> PendingDCRSOPApproval(int appdby)
    {
        string sql = @"select distinct d.dcr_slno,d.Goup_Id,d.DCR_Submit_DateTime,d.Request_By,
d.Request_Date,d.change_area, e.employeename,CASE 
        WHEN Submit_Status = 'Y' THEN 'Submitted'
        WHEN Submit_Status = 'A' THEN 'Approved'
         WHEN Submit_Status = 'O' THEN 'Obsolete'
 WHEN Submit_Status = 'N' THEN 'Not Submitted'
    END AS   Submit_Status
from dcr_sop d inner join sop_mapping cp on cp.Goup_Id=d.Goup_Id
inner join employees e on e.employeeslno=d.Request_By
where d.Submit_status=@0 and d.del_status=@1  and cp.Obsolete='N' and p.approvedBy=@2";
        using (Database db = new Database("connString"))
        {
            List<Class_DCR_SOP> lst = db.Fetch<Class_DCR_SOP>(sql, "Y", "N", appdby);
            return lst;
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
