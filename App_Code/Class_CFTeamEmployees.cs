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

[TableName("CFTeamEmployees")]
[PrimaryKey("cfte_SlNo", AutoIncrement = true)]

/// <summary>
/// properties defined for CFTeamEmployees
/// </summary>
public class Class_CFTeamEmployees : IDisposable {
    public int cfte_SlNo { get; set; }
    public int CFTeamSlNo { get; set; }
    public int EmployeeSlNo { get; set; }

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
/// The CRUD class for CFTeamEmployees
/// </summary>
public class Crud_CFTeamEmployees : ICFTeamEmployees, IDisposable
{
ICFTeamEmployees iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_CFTeamEmployees()
{
   iObj = conn.As<ICFTeamEmployees>();
}

   /// <summary>
   /// inserts data in table - CFTeamEmployees
   /// </summary>
   /// <param name="objName">objName as Class_CFTeamEmployees</param>
   public void usp_CFTeamEmployeesInsert(Class_CFTeamEmployees objName)
   {
       iObj.usp_CFTeamEmployeesInsert(objName);
   }

   /// <summary>
   /// updates data in table - CFTeamEmployees
   /// </summary>
   /// <param name="objName">objName as Class_CFTeamEmployees</param>
   public void usp_CFTeamEmployeesUpdate(Class_CFTeamEmployees objName)
   {
       iObj.usp_CFTeamEmployeesUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - CFTeamEmployees
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_CFTeamEmployeesDelete(Class_CFTeamEmployees objName)
   {
       iObj.usp_CFTeamEmployeesDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - CFTeamEmployees
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_CFTeamEmployees> usp_CFTeamEmployeesSelect()
   {
       return iObj.usp_CFTeamEmployeesSelect().ToList();
   }

   public string GetMembersList(int slno)
   {
       using (Database db = new Database("connString"))
       {
           string sql = "select distinct stuff(( " +
           " select ', ' + emp.employeeName from CFTeamEmployees tm  " +
           " inner join Employees emp on emp.EmployeeSlNo=tm.EmployeeSlNO " +
           " where cfteamSlNo=" + slno + " " +
           " for xml path ('')),1,1,'') as teamMembers ";

           string x = db.ExecuteScalar<string>(sql);
           return x;
       }
   }

   public string GetMemberMailList(string slno)
    {
        string sql = @"select distinct stuff(( 
            select ', ' + emp.emailid from CFTeamEmployees tm  
            inner join Employees emp on emp.EmployeeSlNo=tm.EmployeeSlNO 
            where cfteamSlNo=" + slno + 
            " for xml path ('')),1,1,'') as teamMembers ";

        using(Database db = new Database("connString"))
        {
            string x = db.ExecuteScalar<string>(sql);
            return x;
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
