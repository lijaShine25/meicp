using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using NPoco;
using Elmah;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

public partial class db_backup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            System.Data.SqlClient.SqlConnectionStringBuilder connBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();

            connBuilder.ConnectionString = connectionString;

            string server = connBuilder.DataSource;           //-> this gives you the Server name.
            txtDbName.Text = connBuilder.InitialCatalog;
        }
    }

    protected void btnBackup_Click(object sender, EventArgs e)
    {
        string dbName = txtDbName.Text;
        string date = DateTime.Now.ToShortDateString();
        date = date.Replace("/", "-");
        date = date.Replace(" ", ":");

        string path = Server.MapPath("DBBackup//");
        string absFn = path + dbName + "_" + date + ".bak";


        using (NPoco.Database  bkup = new NPoco.Database ("connString"))
        {
            string query = "backup database " + dbName + " To Disk='" + absFn + "'";
            bkup.Execute(query);
        }

        string fn = dbName + "_" + date + ".bak";

        Response.ContentType = "backup/bak";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fn);
        Response.TransmitFile(absFn);
        Response.End();

    }

    protected void btnRunSql_Click(object sender, EventArgs e)
    {
        RunSqlCommands_SMO();
    }

    void RunSqlCommands_SMO()
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        if (FileUpload1.HasFile)
        {
            string path = Server.MapPath("App_Code//");
            FileInfo fn = new FileInfo(FileUpload1.FileName);
            string absFn = path + fn.Name;

            FileUpload1.PostedFile.SaveAs(absFn);

            string sqlConnectionString = connectionString; //connection string
            FileInfo file = new FileInfo(absFn); //*.sql file path
            string script = file.OpenText().ReadToEnd();
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Server server = new Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(script);
            lblStatus1.Text = "Completed Successfully!!";
        }
    }


    void RunSqlCommands()
    {
        String filename = "";

        if (FileUpload1.HasFile)
        {
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/App_Data/" + FileUpload1.PostedFile.FileName));
            filename = Server.MapPath("~/App_Data/" + FileUpload1.PostedFile.FileName);
                
            var fileContent = File.ReadAllText(filename);
            var sqlqueries = fileContent.Split(new[] { " GO " }, StringSplitOptions.RemoveEmptyEntries);

            using (NPoco.Database db = new NPoco.Database("connString"))
            {
                foreach (var query in sqlqueries)
                {
                    db.Execute(query);
                }
                lblStatus1.Text = "Completed Successfully!!";
            }
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload2.HasFile)     // CHECK IF ANY FILE HAS BEEN SELECTED.
        {
            HttpFileCollection hfc = Request.Files;
            // lblFileList.Text = "Select <b>" + hfc.Count + "</b> file(s)";

            for (int i = 0; i <= hfc.Count - 1; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (hpf.ContentLength > 0)
                {
                    string path = Request.PhysicalApplicationPath;
                    if (ddlFolder.SelectedIndex == 1)
                    {
                        path = Server.MapPath("App_Code//");
                    }
                    FileInfo fn = new FileInfo(hpf.FileName);
                    hpf.SaveAs(path + fn.Name);
                }
            }
            lblUpload.Text = "Files copied to Server Successfully !!";
        }
        else
        {
            lblUpload.Text = "No Files selected !!!";
        }
    }
}