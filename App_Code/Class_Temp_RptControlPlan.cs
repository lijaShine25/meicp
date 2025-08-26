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
using Org.BouncyCastle.Asn1.X509;
using Syncfusion.CompoundFile.XlsIO.Native;

[TableName("Temp_RptControlPlan")]
[PrimaryKey("Pk_Id", AutoIncrement = true)]

/// <summary>
/// properties defined for Temp_RptControlPlan
/// </summary>
public class Class_Temp_RptControlPlan : IDisposable {
    public int cp_slno { get; set; }
    public int operation_slno { get; set; }
    public int Pk_Id { get; set; }
    public int rev_no { get; set; }
    public int? splChar_slno { get; set; }
    public String approvedBy { get; set; }
    public String CFTeamName { get; set; }
    public String custApproval { get; set; }
    public String custApprovalDt { get; set; }
    public String Customer_name { get; set; }
    public String customerIssueDt { get; set; }
    public String customerIssueNo { get; set; }
    public String customerPartNo { get; set; }
    public String custQaApproval { get; set; }
    public String custQaApprovalDt { get; set; }
    public String Date { get; set; }
    public String dimn_no { get; set; }
    public String gaugeCode { get; set; }
    public String keyContact { get; set; }
    public String keyContactPhone { get; set; }
    public String MachineDesc { get; set; }
    public int machine_slno { get; set; }
    public String measurementTech { get; set; }
    public String methodDesc { get; set; }
    public String mstPartNo { get; set; }
    public String OperationDesc { get; set; }
    public String organization { get; set; }
    public String orgApprovalDt { get; set; }
    public String orgDate { get; set; }
    public String originalDt { get; set; }
    public String otherApproval { get; set; }
    public String otherApprovalDt { get; set; }
    public String PartDescription { get; set; }
    public String partIssueDt { get; set; }
    public String partIssueNo { get; set; }
    public String preparedBy { get; set; }
    public String process_char { get; set; }
    public String product_char { get; set; }
    public String reactionPlan { get; set; }
    public String res { get; set; }
    public String rev_date { get; set; }
    public String sampleFreq { get; set; }
    public string sampleSize { get; set; }
    public String tol_max { get; set; }
    public String tol_min { get; set; }
    public String user_revNo { get; set; }
    public String user_revDt { get; set; }
    public String process_no { get; set; }
    public String CftTeamSlNo { get; set; }
    public String cpType { get; set; }
    public string cp_number { get; set; }
    public string cp_revno { get; set; }
    public string cp_revdt { get; set; }
    public string supplier_code { get; set; }
    public string proc_spec { get; set; }
    public string ih_testing_ref { get; set; }
    public string spec1 { get; set; }
    public string spec2 { get; set; }
    public int? evalTech_slno2 { get; set; }
    public string sampleSize2 { get; set; }
    public int freq_slno2 { get; set; }
    public int? method_slno2 { get; set; }
    public string methodDesc2 { get; set; }
    public string sampleFreq2 { get; set; }
    public string measurementTech2 { get; set; }
    public decimal min_spec { get; set; }
    public decimal  max_spec { get; set; }
    public string PDI_type { get; set; }

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
