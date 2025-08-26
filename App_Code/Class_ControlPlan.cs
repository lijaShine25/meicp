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

[TableName("ControlPlan")]
[PrimaryKey("cp_slno", AutoIncrement = true)]

/// <summary>
/// properties defined for ControlPlan
/// </summary>
public class Class_ControlPlan : IDisposable {
    public int cp_slno { get; set; }
    public int machine_slno { get; set; }
    public int operation_slno { get; set; }
    public int part_slno { get; set; }


    public int rev_no { get; set; }
    public String cpType { get; set; }
    public String rev_reason { get; set; }

    //public String natureOfChange { get; set; }


    


    public string Submitstatus { get; set; }
    public string Obsolete { get; set; }
    public string rev_date { get; set; }

    public string user_revNo { get; set; }
    public string user_revDt { get; set; }
    public bool is_approved { get; set; }
   
    [ResultColumn]
    public string operationdesc { get; set; }
    [ResultColumn]
    public string approvestatus { get; set; }

     [ResultColumn]
    public String status1 { get; set; }

    public String partno { get; set; }

    public String process_no { get; set; }

    [ResultColumn]
    public string mstPartNo { get; set; }
 
    [ResultColumn]
    public string user_revno { get; set; }
    [ResultColumn]
    public string machine { get; set; }
    [ResultColumn]
    public string partdescription { get; set; }
    [ResultColumn]
    public string cp_revno { get; set; }
    [ResultColumn]
    public string cp_revdt { get; set; }
    [ResultColumn]
    public string cp_number { get; set; }
    [ResultColumn]
    public string machinedesc { get; set; }
    [ResultColumn]
    public string employeename { get; set; }
    public int? dcr_slno { get; set; }

    public DateTime? CP_Submit_DateTime { get; set; }
    public DateTime? CP_Approved_DateTime { get; set; }


    public string nature_of_Change { get; set; }
    public string reason_For_Change { get; set; }



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
/// The CRUD class for ControlPlan
/// </summary>
public class Crud_ControlPlan : IControlPlan, IDisposable
{
IControlPlan iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_ControlPlan()
{
   iObj = conn.As<IControlPlan>();
}

   /// <summary>
   /// inserts data in table - ControlPlan
   /// </summary>
   /// <param name="objName">objName as Class_ControlPlan</param>
   public void usp_ControlPlanInsert(Class_ControlPlan objName)
   {
       iObj.usp_ControlPlanInsert(objName);
   }

   /// <summary>
   /// updates data in table - ControlPlan
   /// </summary>
   /// <param name="objName">objName as Class_ControlPlan</param>
   public void usp_ControlPlanUpdate(Class_ControlPlan objName)
   {
       iObj.usp_ControlPlanUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - ControlPlan
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_ControlPlanDelete(Class_ControlPlan objName)
   {
       iObj.usp_ControlPlanDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - ControlPlan
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_ControlPlan> usp_ControlPlanSelect()
   {
       return iObj.usp_ControlPlanSelect().ToList();
   }

   //public void usp_InitiateControlPlanRevision(Class_ControlPlan objName, Class_ControlPlan objName1)
   //{
   //    iObj.usp_InitiateControlPlanRevision(objName, objName1);
   //}
  
   public void usp_InitiateControlPlanRevision(int cpslno, string revreason, string revno, string revdate,int dcr=0)
   {
       iObj.usp_InitiateControlPlanRevision(cpslno, revreason, revno, revdate,dcr);
   }

   public void usp_ControlPlanSmartCopy(int pslno, int opslno, int newpartslno, string newpartno) {
       iObj.usp_ControlPlanSmartCopy(pslno, opslno, newpartslno, newpartno);
   }

    public List<Class_ControlPlan>PendingApproval(int appdby)
    {
        string sql = @"select c.cp_slno,p.mstPartNo, p.PartDescription,c.CP_Submit_DateTime,
o.OperationDesc,p.cp_number, p.cp_revno, p.cp_revdt, e.employeename
from ControlPlan c
inner join parts p on p.part_slno=c.part_slno
inner join operations o on o.operation_slno=c.operation_slno
inner join employees e on e.employeeslno=p.preparedBy
where c.Submitstatus=@0 and c.Obsolete=@1 and p.approvedBy=@2";
        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan> lst = db.Fetch<Class_ControlPlan>(sql, "Y", "N", appdby);
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
