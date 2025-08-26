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

[TableName("Employees")]
[PrimaryKey("EmployeeSlNo", AutoIncrement = true)]

/// <summary>
/// properties defined for Employees
/// </summary>
public class Class_Employees : IDisposable {
    public string allmaster { get; set; }
    public string CanApprove { get; set; }
    public string CanPrepare { get; set; }
    public string isAdmin { get; set; }
    public int EmployeeSlNo { get; set; }
    public String del_status { get; set; }
    public String EmailId { get; set; }
    public String EmployeeName { get; set; }
    public String EmployeeRollNo { get; set; }
    public String login_id { get; set; }
    public String login_pwd { get; set; }
    public string key_contact { get; set; }
    [ResultColumn]
    public String status1 { get; set; }
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
/// The CRUD class for Employees
/// </summary>
public class Crud_Employees : IEmployees, IDisposable
{
IEmployees iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_Employees()
{
   iObj = conn.As<IEmployees>();
}

   /// <summary>
   /// inserts data in table - Employees
   /// </summary>
   /// <param name="objName">objName as Class_Employees</param>
   public void usp_EmployeesInsert(Class_Employees objName)
   {
       iObj.usp_EmployeesInsert(objName);
   }

   /// <summary>
   /// updates data in table - Employees
   /// </summary>
   /// <param name="objName">objName as Class_Employees</param>
   public void usp_EmployeesUpdate(Class_Employees objName)
   {
       iObj.usp_EmployeesUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - Employees
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_EmployeesDelete(Class_Employees objName)
   {
       iObj.usp_EmployeesDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - Employees
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_Employees> usp_EmployeesSelect()
   {
       return iObj.usp_EmployeesSelect().ToList();
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
