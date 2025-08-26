using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;
using System.Web;
using System.Configuration;

[TableName("SOP_Mapping")]
[PrimaryKey("Map_slno")]

/// <summary>
/// properties defined for sop_header
/// </summary>
public class Class_sop_mapping : IDisposable
{
    public int Map_slno { get; set; }
    public int part_slno { get; set; }
    public int machine_slno { get; set; }
    public int operation_slno { get; set; }
    public string Group_Name { get; set; }
    public string Format_No { get; set; }
    public int Group_Id { get; set; }

    public string del_status { get; set; }
    [ResultColumn]
    public string mstPartNo { get; set; }
    [ResultColumn]
    public string OperationDesc { get; set; }
    [ResultColumn]
    public string MachineDesc { get; set; }
    public string Template { get; set; }
    public string Obsolete { get; set; }
    public string SubmitStatus { get; set; }
    public int rev_no { get; set; }
    public string rev_date { get; set; }
    public string rev_reason { get; set; }
    [ResultColumn]
    public string process_no { get; set; }
    
    public string nature_of_Change { get; set; }
    public string reason_For_Change { get; set; }

    public int? dcr_slno { get; set; }
    public string Qual_Char { get; set; }
    public int? PreparedBy { get; set; }
    public int? ApprovedBy { get; set; }
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
/// The CRUD class for sop_header
/// </summary>
public class Crud_sop_mapping : IDisposable
{
    public Crud_sop_mapping()
    {
        // 
    }

    /// <summary>
    /// inserts data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_mapping</param>
    public void Insert(Class_sop_mapping objName)
    {

        using (Database db = new Database("connString"))
        {
            db.Insert<Class_sop_mapping>(objName);
        }
    }

    /// <summary>
    /// updates data in table - sop_header
    /// </summary>
    /// <param name="objName">objName as Class_sop_mapping</param>
    public void Update(Class_sop_mapping objName)
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
            db.DeleteWhere<Class_sop_mapping>("sop_id=@0", id);
        }
    }

    /// <summary>
    /// selects all data from table - sop_header
    /// </summary>
    /// <param></param>
    /// <returns>Returns data as list object</returns>
    public List<Class_sop_mapping> SelectAll()
    {
        using (Database db = new Database("connString"))
        {
            return db.Fetch<Class_sop_mapping>();
        }
    }

    /// <summary>
    /// selects required data from table - sop_header
    /// </summary>
    /// <param name="id">id as Integer</param>
    /// <returns>Returns data as class object</returns>
    public Class_sop_mapping SelectOne(int id)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefaultById<Class_sop_mapping>(id);
        }
    }
    public List<Class_sop_mapping> GetSOPChildData(int gid)
    {
        Database db = new Database("connString");
        string qry = "Select mstPartNo,OperationDesc,MachineDesc,sm.part_slno,sm.operation_slno," +
            " Group_Id, " +
            " sm.machine_slno,Format_No,Group_name,sm.map_slno from sop_mapping sm inner join partsmapping pm on pm.part_slno=sm.part_slno  and pm.operation_slno=sm.operation_slno and pm.machine_slno=sm.machine_slno inner join parts p on sm.part_slno=p.part_slno  inner join operations op on op.operation_slno=sm.operation_slno inner join machines on machines.machine_slno=sm.machine_slno   where sm.del_status='N' and sm.rev_no=(SELECT MAX(rev_no) FROM SOP_Mapping WHERE Group_Id = @Group_Id)   and Group_Id=@Group_Id";
        List<Class_sop_mapping> lst= db.Fetch<Class_sop_mapping>(qry, new { Group_Id = gid }).ToList();
     
        return lst;


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
