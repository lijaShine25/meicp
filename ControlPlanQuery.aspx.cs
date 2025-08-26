using Newtonsoft.Json;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ControlPlanQuery : System.Web.UI.Page
{
      public enum MessageType { Success, Error, Info, Warning };
      string id;
      int cp_id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        LoadItemsData();
        if (!Page.IsPostBack)
        {
            //if (userInfo[0].is_ET_User != "Y")
            //{
            //    Response.Redirect("~/AccessDenied.aspx");
            //}

          
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {

        id = grdData.SelectedRow.Cells[0].Text;
        cp_id = Int32.Parse(id);
    }

    protected void LoadItemsData()
    {

        Crud_ControlPlan ci = new Crud_ControlPlan();
        List<Class_ControlPlan> lst1 = ci.usp_ControlPlanSelect().ToList();

     //   List<Class_Employees> lst = lst1.Where(x => x.cp_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
        grdData.DataSource = lst1;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    public string GetPartDesc(string slno)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_parts>(" where part_slno=@0", slno).PartDescription;

        }
    }
    public string GetOpDesc(string slno)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_operations>(" where operation_slno=@0", slno).OperationDesc;

        }
    }
    public string GetMcDesc(string slno)
    {
        using (Database db = new Database("connString"))
        {
            return db.SingleOrDefault<Class_machines>(" where machine_slno=@0", slno).MachineDesc;

        }
    }
}