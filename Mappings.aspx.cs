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

public partial class Mappings : System.Web.UI.Page
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

        if (userInfo[0].isAdmin != "Y" && userInfo[0].CanPrepare != "Y" && userInfo[0].CanApprove != "Y" && userInfo[0].allmaster != "Y")
        {
            Response.Redirect("~/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            //if (userInfo[0].is_ET_User != "Y")
            //{
            //    Response.Redirect("~/AccessDenied.aspx");
            //}

            LoadOperations();
            // LoadMachines();
            LoadParts();
            LoadGridData();

            if (Request.QueryString.HasKeys())
            {
                hdnSlNo.Value = Request.QueryString["slno"];
                hdnMode.Value = "E";
                GetDetails();
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
            }
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData();
        LoadGridData();
        //ClearData();
        ddloperation_slno.SelectedIndex = 0;
        ddlmachine_slno.SelectedIndex = 0;
        ClearData();
    }
    void LoadOperations()
    {
        Crud_operations crud = new Crud_operations();

        List<Class_operations> lst = crud.usp_operationsSelect().ToList().Where(x => x.del_status == "ACTIVE").ToList();
        ddloperation_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddloperation_slno.Items.Add(new ListItem(lst[cnt].OperationDesc, Convert.ToString(lst[cnt].operation_slno)));
        }

        ddloperation_slno.Items.Insert(0, "Select...");

    }
    void LoadMachines()
    {
        Crud_machines crud = new Crud_machines();
        List<Class_machines> lst1 = crud.usp_machinesSelect().ToList();
        List<Class_machines> lst = lst1.Where(x => x.operation_slno == Convert.ToInt16(ddloperation_slno.SelectedValue) && x.del_status == "ACTIVE").ToList();

        ddlmachine_slno.Items.Clear();

        for (int cnt = 0; cnt < lst.Count; cnt++)
        {
            ddlmachine_slno.Items.Add(new ListItem(lst[cnt].MachineCode + "-" + lst[cnt].MachineDesc, Convert.ToString(lst[cnt].machine_slno)));
        }

        ddlmachine_slno.Items.Insert(0, "Select...");

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteData();
        LoadGridData();
        ClearData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }

    /// <summary>
    /// saves the business locations data
    /// </summary>
    /// 

    void GetDetails()
    {
        using (Crud_PartsMapping crud = new Crud_PartsMapping())
        {
            Class_PartsMapping cls = crud.usp_PartsMappingSelect().Where(x => x.map_slno == Convert.ToInt32(hdnSlNo.Value)).FirstOrDefault();

            List<Class_PartsMapping> lst1 = crud.usp_PartsMappingSelect().ToList();
            List<Class_PartsMapping> lst = lst1.Where(x => x.map_slno == Convert.ToInt16(hdnSlNo.Value)).ToList();
            txtProcessNo.Text = lst[0].process_no;

            txtMapSlNo.Text = cls.map_slno.ToString();
            LoadParts();
            ddlpart_slno.SelectedValue = Convert.ToString(cls.part_slno);
            LoadOperations();
            ddloperation_slno.SelectedValue = Convert.ToString(cls.operation_slno);

            LoadMachines();
            ddlmachine_slno.SelectedValue = Convert.ToString(cls.machine_slno);

            btnDelete.Enabled = false;
            // hdnsubmitstatus.Value = c.Submitstatus;

            hdnOperationSlNo.Value = ddloperation_slno.SelectedValue;
            //hdnProcessNo.Value = txtProcessNo.Text;
hdnProcessNo.Value = lst[0].process_no;
            hdnMachineSlNo.Value = ddlmachine_slno.SelectedValue;
            ddlpart_slno.Enabled = false;

        }

    }
    bool CheckProcessExists()
    {
        bool exists = false;
        using (Crud_PartsMapping crud = new Crud_PartsMapping())
        {
            if (hdnMode.Value == "I")
            {
                List<Class_PartsMapping> lst = crud.usp_PartsMappingSelect()
                    .Where(x => x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)
                    && x.process_no == txtProcessNo.Text).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
            else if (hdnMode.Value == "E")
            {
                List<Class_PartsMapping> lst = crud.usp_PartsMappingSelect()
                    .Where(x => x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)
                    && x.process_no == txtProcessNo.Text).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
        }
        // added " && x.process_no == txtProcessNo.Text" in the filter condition on 26-10-2023
        return exists;
    }
    bool CheckOPExists()
    {
        bool exists = false;
        using (Crud_PartsMapping crud = new Crud_PartsMapping())
        {
            if (hdnMode.Value == "I")
            {
                List<Class_PartsMapping> lst = crud.usp_PartsMappingSelect()
                    .Where(x => x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)
                    && x.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue)).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
            else if (hdnMode.Value == "E")
            {
                List<Class_PartsMapping> lst = crud.usp_PartsMappingSelect()
                    .Where(x => x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)
                    && x.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue)).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
        }
        // added " && x.process_no == txtProcessNo.Text" in the filter condition on 26-10-2023
        return exists;
    }
    bool CheckExists()
    {
        bool exists = false;
        using (Crud_PartsMapping crud = new Crud_PartsMapping())
        {
            if (hdnMode.Value == "I")
            {
                List<Class_PartsMapping> lst = crud.usp_PartsMappingSelect()
                    .Where(x => x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)
                    && x.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue)
                    && x.process_no == txtProcessNo.Text).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
            else if (hdnMode.Value == "E")
            {
                List<Class_PartsMapping> lst = crud.usp_PartsMappingSelect()
                    .Where(x => x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue)
                    && x.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue)
                    && x.process_no == txtProcessNo.Text).ToList();
                if (lst.Count > 0) exists = true; else exists = false;
            }
        }
        // added " && x.process_no == txtProcessNo.Text" in the filter condition on 26-10-2023
        return exists;
    }
    void SaveData()
  {
      using (Class_PartsMapping cls = new Class_PartsMapping())
      {
          cls.part_slno = int.Parse(ddlpart_slno.SelectedValue);
          cls.operation_slno = int.Parse(ddloperation_slno.SelectedValue);
          cls.machine_slno = int.Parse(ddlmachine_slno.SelectedValue);
          cls.process_no = txtProcessNo.Text;
          using (Crud_PartsMapping crud = new Crud_PartsMapping())
          {
              using (Database db = new Database("connString"))
              {
                  if (hdnMode.Value == "I")
                  {
                      bool chkOPExists = CheckOPExists();
                      bool chkProcessExists = CheckProcessExists();
                      bool chkallExists = CheckExists();
                      if (chkOPExists == true)
                      {
                          if (chkallExists == true)
                          {
                              ShowMessage("Data already exists!", MessageType.Info);
                              //return;
                          }
                          else
                          {
                              ShowMessage("Operation already exists for the Part!", MessageType.Info);
                          }
                          return;
                      }

                      if (chkProcessExists == true)
                      {
                          if (chkallExists == true)
                          {
                              ShowMessage("Data already exists!", MessageType.Info);
                          }
                          else
                          {
                              ShowMessage("Process No. already exists for the Part!", MessageType.Info);
                          }
                          return;
                      }
                      if (chkallExists == true)
                      {
                          ShowMessage("Process No. already exists for the Part!", MessageType.Info);
                          return;
                      }



                      crud.usp_PartsMappingInsert(cls);
                      ShowMessage("Record Inserted Successfully", MessageType.Success);


                  }
                  else
                  {
                      //string qry1 = "select count (*) from PartsMapping where part_slno=@0 and operation_slno=@1  and process_no=@2 and map_slno <> @3";
                      string qry1 = "select count (*) from PartsMapping where part_slno=@0 and operation_slno=@1 and  process_no=@2 and map_slno <> @3";
                      int a = db.ExecuteScalar<int>(qry1, Convert.ToInt16(ddlpart_slno.SelectedValue), Convert.ToInt16(ddloperation_slno.SelectedValue), txtProcessNo.Text, hdnSlNo.Value);
                      if (a > 0)
                      {
                          ShowMessage("Mapping Record Already Exist", MessageType.Info);
                      }
                      else
                      {
                          cls.map_slno = Convert.ToInt16(hdnSlNo.Value);
                          crud.usp_PartsMappingUpdate(cls);

                          string sqlOp_slno = "update controlplan set operation_slno=" + ddloperation_slno.SelectedValue + ", process_no='" + txtProcessNo.Text + "', machine_slno=" + ddlmachine_slno.SelectedValue +
                                            " where part_slno=" + ddlpart_slno.SelectedValue + " and machine_slno=" + hdnMachineSlNo.Value + " and operation_slno=" + hdnOperationSlNo.Value + " and process_no='" + hdnProcessNo.Value + "'"
                                           + " and rev_no=(SELECT MAX(rev_no) from Controlplan where part_slno=" + ddlpart_slno.SelectedValue + " and machine_slno=" + hdnMachineSlNo.Value + " and operation_slno=" + hdnOperationSlNo.Value + " and process_no='" + hdnProcessNo.Value + "' ) ";

                          if (hdnOperationSlNo.Value != ddloperation_slno.SelectedValue || hdnMachineSlNo.Value != ddlmachine_slno.SelectedValue || hdnProcessNo.Value != txtProcessNo.Text)
                          {

                              db.Execute(sqlOp_slno);

                          }
                    
                              ShowMessage("Record Updated Successfully", MessageType.Success);
                      }
                  }
              }
          }
      }
  }
    void SaveData1()
    {
        using (Class_PartsMapping cls = new Class_PartsMapping())
        {
            cls.part_slno = int.Parse(ddlpart_slno.SelectedValue);
            cls.operation_slno = int.Parse(ddloperation_slno.SelectedValue);
            cls.machine_slno = int.Parse(ddlmachine_slno.SelectedValue);
            cls.process_no = txtProcessNo.Text;

            // check if data already exists
            bool chkOPExists = CheckOPExists();
            bool chkProcessExists = CheckProcessExists();
            bool chkallExists = CheckExists();
            if (chkOPExists == true)
            {
                if (chkallExists == true)
                { 
                    ShowMessage("Data already exists!", MessageType.Info);
                }
                else 
                {
                    ShowMessage("Operation already exists for the Part!", MessageType.Info);
                }
                return;
            }

            if (chkProcessExists == true)
            {
                if (chkallExists == true)
                {
                    ShowMessage("Data already exists!", MessageType.Info);
                }
                else
                {
                    ShowMessage("Process No. already exists for the Part!", MessageType.Info);
                }
                return;
            }
            if (chkallExists == true)
            {
                ShowMessage("Process No. already exists for the Part!", MessageType.Info);
                return;
            }
            using (Crud_PartsMapping crud = new Crud_PartsMapping())
            {

                string qry = "select count (*) from PartsMapping where part_slno=@0 and operation_slno=@1 and process_no=@2";

                using (Database db = new Database("connString"))
                {

                    int x = db.ExecuteScalar<int>(qry, Convert.ToInt16(ddlpart_slno.SelectedValue), Convert.ToInt16(ddloperation_slno.SelectedValue), txtProcessNo.Text);
                    if (x > 0)
                    {
                        ShowMessage("Mapping Record Already Exist", MessageType.Info);
                    }

                    else
                    {
                        if (hdnMode.Value == "I")
                        {
                            crud.usp_PartsMappingInsert(cls);
                            ShowMessage("Record Inserted Successfully", MessageType.Success);
                        }
                        else if (hdnMode.Value == "E")
                        {

                            string qry1 = "select count (*) from PartsMapping where part_slno=@0 and operation_slno=@1  and process_no=@3";
                            int a = db.ExecuteScalar<int>(qry1, Convert.ToInt16(ddlpart_slno.SelectedValue), Convert.ToInt16(ddloperation_slno.SelectedValue), txtProcessNo.Text);
                            if (a > 0)
                            {
                                ShowMessage("Mapping Record Already Exist", MessageType.Info);
                            }
                            else
                            {
                                cls.map_slno = Convert.ToInt16(hdnSlNo.Value);
                                crud.usp_PartsMappingUpdate(cls);

                                string sqlOp_slno = "update controlplan set operation_slno=" + ddloperation_slno.SelectedValue + ", process_no=" + txtProcessNo.Text + ", machine_slno=" + ddlmachine_slno.SelectedValue +
                                                  " where part_slno=" + ddlpart_slno.SelectedValue + " and machine_slno=" + hdnMachineSlNo.Value + " and operation_slno=" + hdnOperationSlNo.Value + " and process_no=" + hdnProcessNo.Value + " "
                                                 + " and rev_no=(SELECT MAX(rev_no) from Controlplan where part_slno=" + ddlpart_slno.SelectedValue + " and machine_slno=" + hdnMachineSlNo.Value + " and operation_slno=" + hdnOperationSlNo.Value + " and process_no=" + hdnProcessNo.Value + " ) ";

                                if (hdnOperationSlNo.Value != ddloperation_slno.SelectedValue || hdnMachineSlNo.Value != ddlmachine_slno.SelectedValue || hdnProcessNo.Value != txtProcessNo.Text)
                                {

                                    db.Execute(sqlOp_slno);

                                }
                                ShowMessage("Record Updated Successfully", MessageType.Success);
                            }


                        }

                    }
                }
            }

        }
    }

    protected void ddloperation_slno_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        //commented by selva on 19062016
        LoadMachines();
    }

    protected void btnFilder_Click(object sender, EventArgs e)
    {
        LoadGridData();
    }


    /// <summary>
    /// deletes the business location data
    /// </summary>

    void DeleteData()
    {
        using (Class_PartsMapping cls = new Class_PartsMapping())
        {

            cls.map_slno = Convert.ToInt16(txtMapSlNo.Text);
            using (Crud_PartsMapping crud = new Crud_PartsMapping())
            {
                Crud_ControlPlan crudcp = new Crud_ControlPlan();
                List<Class_ControlPlan> lst = crudcp.usp_ControlPlanSelect().Where(x => x.operation_slno == Convert.ToInt32(ddloperation_slno.SelectedValue) && x.machine_slno == Convert.ToInt32(ddlmachine_slno.SelectedValue) && x.part_slno == Convert.ToInt32(ddlpart_slno.SelectedValue) && x.process_no == txtProcessNo.Text.Trim()).ToList();

                if (lst.Count > 0)
                {
                    ShowMessage("Can not delete the data if Referred elsewhere..!", MessageType.Info);
                }
                else
                {
                    crud.usp_PartsMappingDelete(cls);
                    ShowMessage("Record Deleted Successfully", MessageType.Success);
                }

                //crud.usp_PartsMappingDelete(cls);
                //ShowMessage("Record Deleted Successfully", MessageType.Success);

                //Uncommented lines  from 282 to 293  and commented lines 295 and  296 on 26-10-2023
            }
        }
    }

    /// <summary>
    /// clears all the data from the screen
    /// </summary>
    void ClearData()
    {
        ddlpart_slno.SelectedIndex = 0;
        ddloperation_slno.SelectedIndex = 0;
        ddlmachine_slno.SelectedIndex = 0;
        txtProcessNo.Text = "";
        hdnMode.Value = "I";
        hdnSlNo.Value = "";
        hdnMachineSlNo.Value = "";
        hdnOperationSlNo.Value = "";
        hdnProcessNo.Value = "";
        LoadGridData();
        btnDelete.Enabled = false;
    }

    /// <summary>
    /// loads the data in grdData control
    /// </summary>
    void LoadGridData()
    {
        Crud_PartsMapping crud = new Crud_PartsMapping();
        List<Class_PartsMapping> lst = null;

        List<Class_PartsMapping> lst1 = crud.usp_PartsMappingSelect().ToList();
        if (ddlpart_slno.SelectedIndex > 0)
        {
            List<Class_PartsMapping> lst2 = lst1.Where(x => x.part_slno == Convert.ToInt16(ddlpart_slno.SelectedValue)).ToList();
            lst = lst2;
            if (ddloperation_slno.SelectedIndex > 0)
            {
                List<Class_PartsMapping> lst3 = lst1.Where(x => x.part_slno == Convert.ToInt16(ddlpart_slno.SelectedValue) &&
                                    x.operation_slno == Convert.ToInt16(ddloperation_slno.SelectedValue)).ToList();
                lst = lst3;
            }
        }
        else if (ddlpart_slno.SelectedIndex == 0 && ddloperation_slno.SelectedIndex > 0)
        {
            List<Class_PartsMapping> lst3 = lst1.Where(x => x.operation_slno == Convert.ToInt16(ddloperation_slno.SelectedValue)).ToList();
            lst = lst3;
        }
        else
        {
            lst = lst1;
        }
        grdData.DataSource = lst;
        grdData.DataBind();
        grdData.HeaderRow.TableSection = TableRowSection.TableHeader;
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



}