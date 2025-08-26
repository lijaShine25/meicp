using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using Elmah;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using NPoco;

public partial class PartRevEntry : System.Web.UI.Page
{
    public enum MessageType { Success, Error, Info, Warning };
    List<Class_Employees> userInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        userInfo = (List<Class_Employees>)Session["UserInfo"];
        if (userInfo == null)
        {
            Response.Redirect("~/LoginPage.aspx");
        }

        if (userInfo[0].isAdmin != "Y" )
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadParts();
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

    void LoadParts()
    {
        Crud_parts crud = new Crud_parts();
        List<Class_parts> lst1 = crud.usp_partsSelect().ToList();
        List<Class_parts> lst = lst1.Where(x => x.del_status == "ACTIVE" && x.Obsolete == "N").ToList();
        ddlpart_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlpart_slno.Items.Add(new ListItem(lst[cnt].mstPartNo, Convert.ToString(lst[cnt].part_slno)));
        }
        ddlpart_slno.Items.Insert(0, "Select...");
    }

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    // delete all the entries for the selected part
    //    using(Database db = new Database("connString"))
    //    {
    //        //string sqldel = "delete from part_revision_history where part_slno=@0";
    //        //db.Execute(sqldel, ddlpart_slno.SelectedValue);

    //        // insert the data 
    //        List<Class_part_revision_history> lst = JsonConvert.DeserializeObject<List<Class_part_revision_history>>(hdnchild.Value);
    //        if (lst.Count > 0)
    //        {
    //            foreach(Class_part_revision_history c in lst)
    //            {   if ((c.rev_no == null || c.rev_no == "") || string.IsNullOrEmpty(c.rev_date))
    //                { }
    //                else
    //                {
    //                    // db.Insert<Class_part_revision_history>(c);
    //                    db.Update(c);
    //                }
    //            }
    //            ShowMessage("Data saved!", MessageType.Success);
    //        }
    //        else
    //        {
    //            ShowMessage("No data to save!!!", MessageType.Info);
    //        }
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // delete all the entries for the selected part
        using (Database db = new Database("connString"))
        {
            string sqldel = "delete from part_revision_history where part_slno=@0";
            db.Execute(sqldel, ddlpart_slno.SelectedValue);

            // insert the data 
            List<Class_part_revision_history> lst = JsonConvert.DeserializeObject<List<Class_part_revision_history>>(hdnchild.Value);
            if (lst.Count > 0)
            {
                foreach (Class_part_revision_history c in lst)
                {
                    //if ((c.rev_no == null || c.rev_no == "") || string.IsNullOrEmpty(c.rev_date))
                    //{ }
                    //else
                    //{
                        db.Insert<Class_part_revision_history>(c);
                        //db.Update(c);
                    //}
                }
                ShowMessage("Data saved!", MessageType.Success);
            }
            else
            {
                ShowMessage("No data to save!!!", MessageType.Info);
            }
        }
    }
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    using (Database db = new Database("connString"))
    //    {
    //        // Deserialize the Handsontable data
    //        List<Class_part_revision_history> lst = JsonConvert.DeserializeObject<List<Class_part_revision_history>>(hdnchild.Value);

    //        if (lst != null && lst.Count > 0)
    //        {
    //            foreach (Class_part_revision_history c in lst)
    //            {
    //                // Skip if required fields are missing
    //                if (string.IsNullOrEmpty(c.rev_no) || string.IsNullOrEmpty(c.rev_date))
    //                    continue;

    //                // Treat new records (no rev_slno or 0) as inserts, others as updates
    //                if (c.rev_slno == null || c.rev_slno == 0)
    //                {

    //                    db.Insert(c);
    //                }
    //                else
    //                {
    //                    db.Update(c);
    //                }
    //            }

    //            ShowMessage("Data saved!", MessageType.Success);
    //        }
    //        else
    //        {
    //            ShowMessage("No data to save!!!", MessageType.Info);
    //        }
    //    }
    //}


}