using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using Newtonsoft.Json;
using Elmah;
using System.Linq;

[TableName("Settings")]

public class Class_Settings
{
    public int smtp_port { get; set; }
    public String enable_adsl { get; set; }
    public String from_mailid { get; set; }
    public String from_mailpwd { get; set; }
    public String smtp_address { get; set; }
    public string enable_trigger { get; set; }
    public string enable_reminders { get; set; }

}


public class CrudSettings : IDisposable
{
    public string SelectSettings()
    {
        using (Database db = new Database("connString"))
        {
            var optData = db.Query<Class_Settings>("Select * from settings").ToList();
            return JsonConvert.SerializeObject(optData);
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