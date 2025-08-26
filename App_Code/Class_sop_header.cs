using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("sop_header")]
[PrimaryKey("sop_id")]

/// <summary>
/// properties defined for sop_header
/// </summary>
public class Class_sop_header : IDisposable
{
    public int sop_id { get; set; }
    public int part_slno { get; set; }
    public int machine_slno { get; set; }
    public int operation_slno { get; set; }
    public int rev_no { get; set; }
    public String rev_reason { get; set; }
    public String rev_date { get; set; }
    public int? prev_oprn_slno { get; set; }
    public int? next_oprn_slno { get; set; }
    public String objective { get; set; }
    public String before_machine_filename { get; set; }
    public String after_machine_filename { get; set; }
    public String oprn_instruction { get; set; }
    public String checkpoints_list { get; set; }
    public String workholding_method { get; set; }
    public String firstoff_approval { get; set; }
    public String coolant_used { get; set; }
    public String reaction_plan { get; set; }
    public String notes { get; set; }
    public String cp_type { get; set; }
    public string submitstatus { get; set; }
    public bool is_approved { get; set; }
    public bool is_obsolete { get; set; }
    [ResultColumn]
    public String operationdesc { get; set; }
    [ResultColumn]
    public String machinedesc { get; set; }
    [ResultColumn]
    public String mstPartNo { get; set; }
    [ResultColumn]
    public String ApprovedStatus { get; set; }
    [ResultColumn]
    public String cp_number { get; set; }
    [ResultColumn]
    public String partIssueNo { get; set; }
    [ResultColumn]
    public String partIssueDt { get; set; }
    [ResultColumn]
    public String process_no { get; set; }
    [ResultColumn]
    public String prev_process_no { get; set; }
    [ResultColumn]
    public String next_process_no { get; set; }
    [ResultColumn]
    public String nextoperation { get; set; }
    [ResultColumn]
    public String prevoperation { get; set; }
    [ResultColumn]
    public String PartDescription { get; set; }

    [ResultColumn]
    public String Customer_name { get; set; }
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
public class Crud_sop_header : IDisposable
{
    public Crud_sop_header()
    {
        // 
    }

    /// <summary>
    /// inserts data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_header</param>
    public void Insert(Class_sop_header objName)
    {

        using (Database db = new Database("connString"))
        {
            db.Insert<Class_sop_header>(objName);
        }
    }

    /// <summary>
    /// updates data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_header</param>
    public void Update(Class_sop_header objName)
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
            db.DeleteWhere<Class_sop_header>("sop_id=@0", id);
        }
    }

    /// <summary>
    /// selects all data from table - sop_header
    /// </summary>
    /// <param></param>
    /// <returns>Returns data as list object</returns>
    public List<Class_sop_header> SelectAll()
    {
        using (Database db = new Database("connString"))
        {
            return db.Fetch<Class_sop_header>();
        }
    }

    /// <summary>
    /// selects required data from table - sop_header
    /// </summary>
    /// <param name="id">id as Integer</param>
    /// <returns>Returns data as class object</returns>
    public Class_sop_header SelectOne(int id)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefaultById<Class_sop_header>(id);
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
