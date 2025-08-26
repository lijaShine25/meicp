using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Elmah;
using NPoco;
using System.Text;
public partial class SmartCopy : System.Web.UI.Page
{

    public enum MessageType { Success, Error, Info, Warning };
    public bool overRideExistingData = false;
    List<Class_Employees> userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanPrepare != "Y" )
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        
        //if (userInfo[0].is_ET_User != "Y")
        //{
        //    Response.Redirect("~/AccessDenied.aspx");
        //}

        if (!Page.IsPostBack)
        {
            LoadSourcePN();
        }
    }

    protected void ddlSourcePart_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDestinationPN();
        LoadGridOperations();
    }

    void LoadSourcePN()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst = null;
        if (hdnMode.Value == "I")
        {
            lst = crud.usp_partsSelect().ToList().Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList(); ;
        }
        else if (hdnMode.Value == "E")
        {
            lst = crud.usp_partsSelect().ToList();
        }          

        ddlSourcePart.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlSourcePart.Items.Add(new ListItem(Convert.ToString(lst[cnt].mstPartNo), Convert.ToString(lst[cnt].part_slno)));
        }
        ddlSourcePart.Items.Insert(0, "Select...");
    }

    void LoadDestinationPN()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst = null;
        if (hdnMode.Value == "I")
        {
            lst = crud.usp_partsSelect().ToList().Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList(); ;
        }
        else if (hdnMode.Value == "E")
        {
            lst = crud.usp_partsSelect().ToList();
        }        
       
        ddlDestnPart.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlDestnPart.Items.Add(new ListItem(Convert.ToString(lst[cnt].mstPartNo), Convert.ToString(lst[cnt].part_slno)));
        }
        ListItem li = ddlDestnPart.Items.FindByValue(ddlSourcePart.SelectedValue);
        if (li != null){
            ddlDestnPart.Items.Remove(li);
        }

        ddlDestnPart.Items.Insert(0, "Select...");
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }

    void ClearData()
    {
        ddlDestnPart.SelectedIndex = 0;
        ddlSourcePart.SelectedIndex = 0;
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
    }

    void LoadGridOperations()
    {
        //Crud_PartsMapping crud = new Crud_PartsMapping();
        //List<Class_operations> lst = crud.usp_operationsSelect().ToList();
        using (Database db = new Database("connString"))
        {

            var x = db.Query<Class_operations>("select * FROM operations o,partsmapping p where p.operation_slno=o.operation_slno AND p.part_slno=@0 AND o.del_status = 'N' ", Convert.ToInt32(ddlSourcePart.SelectedValue)).ToList();
            if (x.Count > 0)
            {
                grdSourceOperations.DataSource = x;
                grdSourceOperations.DataBind();
            }
            else
            {
                grdSourceOperations.DataSource = null;
                grdSourceOperations.DataBind();
            }
        }

        CheckAvailableOperationsInGrid();


    }

    void CheckAvailableOperationsInGrid()
    {
        foreach (GridViewRow r in grdSourceOperations.Rows)
        {
            CheckBox chk = (CheckBox)r.FindControl("lblChkSel");
            Label lbl = (Label)r.FindControl("lblOperationSlNo");
            chk.Enabled = GetOperationAvailability(Convert.ToInt32(ddlSourcePart.SelectedValue), Convert.ToInt32(lbl.Text));
           

        }

    }

    bool GetOperationAvailability(int pslno, int opslno)
    {

        string qry = " SELECT c.* FROM controlplan c where c.rev_no =(SELECT MAX(p.rev_no ) from controlplan p WHERE c.part_slno=p.part_slno " +
" and c.operation_slno=p.operation_slno AND c.machine_slno=p.machine_slno ) AND c.part_slno =" + pslno +" AND c.operation_slno =" + opslno +" AND c.submitstatus='A' ";

        using (Database db = new Database("connString"))
        {
            var x = db.Query<Class_ControlPlan>(qry).ToList();

            if (x.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }


    bool CheckexistingOperationinTarget(int partslno, int opslno)
    {

        string qry = "select count(*) from controlplan where  part_slno=" + partslno + " and operation_slno=" + opslno;

        using (Database db = new Database("connString"))
        {
            int x = db.ExecuteScalar<int>(qry);

            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    protected void btnSubmit_Click(object sender,EventArgs e) {
        save();
    }
    bool AtleastOneSelected()
    {
        bool chkd = false;
        foreach (GridViewRow row in grdSourceOperations.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("lblChkSel");
            if (chk.Checked)
            {
                chkd = true;
                break;
            }
        }
        return chkd;
    }



    void save()
    {
        if (AtleastOneSelected())
        {
            StringBuilder sb = new StringBuilder();
   
            using (Crud_ControlPlan cp = new Crud_ControlPlan())
            {
                // loop through each item in gridview
                foreach (GridViewRow r in grdSourceOperations.Rows)
                {
                    CheckBox chk = (CheckBox)r.FindControl("lblChkSel");
                    Label lbl = (Label)r.FindControl("lblOperationSlNo");
                    if (chk.Checked) // check for the operation selection
                    {
                        if (CheckexistingOperationinTarget(Convert.ToInt32(ddlDestnPart.SelectedValue), Convert.ToInt32(lbl.Text)) == false) // verify that operation is present for that part no
                        {
                            //if not smartcopy it
                            cp.usp_ControlPlanSmartCopy(Convert.ToInt32(ddlSourcePart.SelectedValue), Convert.ToInt32(lbl.Text), Convert.ToInt32(ddlDestnPart.SelectedValue),ddlDestnPart.SelectedItem.Text);
                        }
                        else
                        { // else
                            if (overRideExistingData == true) //this is a boolean variable in the public declaration if it is true then goahead delete the existing
                            {
                                //here delete the existing control plan values for particular part and operation irresepctive of their status
                                DeleteControlplan(Convert.ToInt32(ddlDestnPart.SelectedValue), Convert.ToInt32(lbl.Text));
                                // and smartcopy it
                                cp.usp_ControlPlanSmartCopy(Convert.ToInt32(ddlSourcePart.SelectedValue), Convert.ToInt32(lbl.Text), Convert.ToInt32(ddlDestnPart.SelectedValue),ddlDestnPart.Text);
                            }
                            else
                            {
                                //if overrideexistingdata is set to false which is the default behaviour then gather those part numbers to a string builder for reference
                                sb.Append(lbl.Text);

                            }

                        }
                    }
                }
            }
            if (sb.Length > 0)
            {
                //if not inserted part numbers and operations are present then show them using the standard alert message
                Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + sb.ToString() + " operations are already present');", true);
            }
            else
            {
                // success alert message
                Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('updated');", true);
            }
        }
        else
        {
            ShowMessage("Select atleast one Operation for Copy", MessageType.Info);
        }
    }

    /// <summary>
    /// displays user messages in the top of the screen
    /// </summary>
    /// <param name="Message">Message content</param>
    /// <param name="type">Message type as Success, Error, Info, Warning</param>
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }
     
    void DeleteControlplan(int pslno, int opslno) {

        using (Database db = new Database("connString"))
        {
            List<Class_ControlPlan> lstcp = db.FetchWhere<Class_ControlPlan>(x => x.part_slno == pslno & x.operation_slno == opslno);

            if (lstcp.Count > 0) {

                foreach (Class_ControlPlan c in lstcp) {

                    db.DeleteWhere<Class_ControlPlan_Child>(" where cp_slno=@0", c.cp_slno);
                    db.Delete<Class_ControlPlan>(c.cp_slno );
                } 
            }


        }
    }
    
    protected void grdSourceOperations_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {


        }
    }

    void clear() {
        grdSourceOperations.DataSource = null;
        grdSourceOperations.DataBind();
       
    }

    //protected void btnClear_Click(object sender, EventArgs e) {

    //    Response.Redirect("~/SmartCopy.aspx",false );
    //}
}