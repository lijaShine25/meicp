using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("DCR")]
[PrimaryKey("dcr_slno")]

/// <summary>
/// properties defined for customers
/// </summary>
public class Class_DCR: IDisposable {
  
        public int dcr_slno { get; set; }
    public string dcr_number { get; set; }
    public string partDescription { get; set; }
        public int part_slno { get; set; }
        public string mstPartNo { get; set; }
        public int? operation_slno { get; set; }
        public string change_area { get; set; }
        public string Process_Number { get; set; }
        public string Process_Name { get; set; }
        public string Prc_Characteristics { get; set; }
        public string Prd_Characteristics { get; set; }
        public string Specification { get; set; }
        public string Measurement_Tech { get; set; }
        public string Sample_Size { get; set; }
        public string Sample_Frequency { get; set; }
        public string Control_Method { get; set; }
        public string Others { get; set; }
        public string Existing { get; set; }
        public string Changes_Required { get; set; }
        public string Reason_For_Change { get; set; }
        public int Request_By { get; set; }
        public string Request_Date { get; set; }
        public string del_status { get; set; }
        public string Submit_Status { get; set; }
    public string operations { get; set; }
    public DateTime? DCR_Submit_DateTime { get; set; }
    public DateTime? DCR_Approved_DateTime { get; set; }

    [ResultColumn]
    public string OperationDesc { get; set; }
    [ResultColumn]
    public string EmployeeName { get; set; }
    [ResultColumn]
    public string changes { get; set; }

    [ResultColumn]
    public string process_no { get; set; }
    [ResultColumn]
    public string MachineDesc { get; set; }
    public string nature_of_change { get; set; }

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
public class Crud_dcr : IDisposable
{
   public Crud_dcr()
   {
       // 
   }

   /// <summary>
   /// inserts data in table - customers
   /// </summary>
   /// <param name="objName">objName as Class_customers</param>
   public void Insert(Class_DCR objName)
   {
       using (Database db = new Database("connString"))
       {
           db.Insert<Class_DCR>(objName);
       }
   }

   /// <summary>
   /// updates data in table - customers
   /// </summary>
   /// <param name="objName">objName as Class_customers</param>
   public void Update(Class_DCR objName)
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
           db.DeleteWhere<Class_DCR>("cust_slno=@0", id);
       }
   }

   /// <summary>
   /// selects all data from table - customers
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public List<Class_DCR> SelectAll()
   {
       using (Database db = new Database("connString"))
       {
           return db.Fetch<Class_DCR>();
       }
   }

   /// <summary>
   /// selects required data from table - customers
   /// </summary>
   /// <param name="id">id as Integer</param>
   /// <returns>Returns data as class object</returns>
   public Class_DCR SelectOne(int id)
   {
       using (Database db = new Database("connString"))
       {
           return db.SingleOrDefaultById<Class_DCR>(id);
       }
   }
    public List<Class_DCR> PendingDCRApproval(int appdby)
    {
        string sql = @"select distinct d.dcr_slno, p.mstPartNo,d.dcr_number, p.PartDescription,d.DCR_Submit_DateTime,d.Request_By,
STUFF((
        SELECT ', ' + opn.OperationDesc
        FROM operations opn
        WHERE opn.operation_slno IN (
            SELECT Value 
            FROM dbo.SplitString(d.operations, ',')
        )
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS OperationDesc,d.Request_Date,d.change_area, e.employeename,CASE 
        WHEN Submit_Status = 'Y' THEN 'Submitted'
        WHEN Submit_Status = 'A' THEN 'Approved'
         WHEN Submit_Status = 'O' THEN 'Obsolete'
 WHEN Submit_Status = 'N' THEN 'Not Submitted'
    END AS   Submit_Status
from dcr d inner join controlplan cp on cp.part_slno=d.part_slno and cp.operation_slno IN (SELECT Value FROM dbo.SplitString(d.operations, ','))
inner join parts p on p.part_slno=d.part_slno
inner join employees e on e.employeeslno=d.Request_By
left outer join machines  m on m.machine_slno=cp.machine_slno
where d.Submit_status=@0 and d.del_status=@1  and cp.Obsolete='N' and p.approvedBy=@2";
        using (Database db = new Database("connString"))
        {
            List<Class_DCR> lst = db.Fetch<Class_DCR>(sql, "Y", "N", appdby);
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
