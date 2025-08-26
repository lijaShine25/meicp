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

[TableName("CFTeams")]
[PrimaryKey("CFTeamSlNo", AutoIncrement = true)]

/// <summary>
/// properties defined for CFTeams
/// </summary>
public class Class_CFTeams : IDisposable {
    public int CFTeamSlNo { get; set; }
    public String CFTeamName { get; set; }
    public String del_status { get; set; }

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
/// The CRUD class for CFTeams
/// </summary>
public class Crud_CFTeams : ICFTeams, IDisposable
{
ICFTeams iObj;
ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["connString"];

public Crud_CFTeams()
{
   iObj = conn.As<ICFTeams>();
}

   /// <summary>
   /// inserts data in table - CFTeams
   /// </summary>
   /// <param name="objName">objName as Class_CFTeams</param>
   public void usp_CFTeamsInsert(Class_CFTeams objName)
   {
       iObj.usp_CFTeamsInsert(objName);
   }

   /// <summary>
   /// updates data in table - CFTeams
   /// </summary>
   /// <param name="objName">objName as Class_CFTeams</param>
   public void usp_CFTeamsUpdate(Class_CFTeams objName)
   {
       iObj.usp_CFTeamsUpdate(objName);
   }

   /// <summary>
   /// deletes specified data from table - CFTeams
   /// </summary>
   /// <param name="id">id as Integer</param>
   public void usp_CFTeamsDelete(Class_CFTeams objName)
   {
       iObj.usp_CFTeamsDelete(objName);
   }

   /// <summary>
   /// selects all the data from table - CFTeams
   /// </summary>
   /// <param></param>
   /// <returns>Returns data as list object</returns>
   public IList<Class_CFTeams> usp_CFTeamsSelect()
   {
       return iObj.usp_CFTeamsSelect().ToList();
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
