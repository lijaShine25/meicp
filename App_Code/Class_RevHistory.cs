using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class_RevHistory
/// </summary>
public class Class_RevHistory
{ //For Revision History
    public int rev_no { get; set; }
    public String rev_reason { get; set; }
    public String mstPartNo { get; set; }
    public String PartDescription { get; set; }
    public String OperationDesc { get; set; }
    public String MachineDesc { get; set; }
    public String rev_date { get; set; }
    public String user_revNo { get; set; }
    public String user_revDt { get; set; } 
}