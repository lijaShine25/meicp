<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="db_backup.aspx.cs" Inherits="db_backup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        #divFile p { 
            font:13px tahoma, arial; 
        }
        #divFile h3 { 
            font:16px arial, tahoma; 
            font-weight:bold;
        }
    </style>
    <form id="form1" runat="server" role="form">
        <aside class="right-side">
            <section class="content-header">
                <h1>Database Actions
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-md-6">
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">Download DB Backup File</h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                <label for="txtDbName">Database Name</label>
                                    <asp:TextBox runat="server" ID="txtDbName" CssClass="form-control" Text="" readonly="true" />
                                </div>
                                <asp:Button Text="CLICK DOWNLOAD DATABASE BACKUP" ID="btnBackup" CssClass="btn btn-warning" runat="server" OnClick="btnBackup_Click" />
                            </div>
                            <div class="box-footer">
                                <span class="label label-danger">When prompted click on Save. Do not attempt to Open the file !!</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">Run SQL Scripts</h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button Text="Execute File" runat="server" CssClass="btn btn-danger" ID="btnRunSql" OnClick="btnRunSql_Click" />
                                <asp:Label Text="" ID="lblStatus1" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">
                                    Upload Files
                                </h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="ddlFolder">
                                        <asp:DropDownList runat="server" ID="ddlFolder" CssClass="form-control">
                                            <asp:ListItem Text="Root Folder" Selected="True" />
                                            <asp:ListItem Text="App_Code Folder" />
                                        </asp:DropDownList>
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label for="fileupload2">Select Files to Upload</label>
                                    <div id="divFile"><p>
                                    <asp:FileUpload ID="FileUpload2" runat="server" AllowMultiple="true"  /></p></div>
                                </div>
                            </div>
                            <div class="box-footer">
                                
                                <asp:Button Text="Upload File(s)" runat="server" CssClass="btn btn-info" ID="btnUpload" OnClick="btnUpload_Click" />
                                <asp:Label Text="" ID="lblUpload" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </aside>
    </form>
    <script>
        $('#btnUpload').click(function () {
            if (fileUpload.value.length == 0) {    // CHECK IF FILE(S) SELECTED.
                alert('No files selected.');
                return false;
            }
        });
</script>
</asp:Content>

