<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <!-- Right side column. Contains the navbar and content of the page -->
        <aside class="right-side">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Settings
                </h1>
            </section>

            <!-- Main content -->
            <section class="content">

                <div class="row">
                    <div class="col-md-4">
                        <div class="box">
                            <div class="box-header">
                                <h4 class="box-title">Email Settings</h4>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="txtFromMail">From Email Id</label>
                                    <asp:TextBox runat="server" ID="txtFromMail" CssClass="form-control" />
                                </div>
                                <div class="form-group">
                                    <label for="txtMailPwd">Password</label>
                                    <asp:TextBox runat="server" ID="txtMailPwd" CssClass="form-control" TextMode="Password" />
                                    <asp:Label Text="Not Displayed due to Security Reasons!!" ID="lblPwd" runat="server" class="label label-info xs" />
                                    <asp:Label Text="Password is blank!!" ID="lblPwdBlank" runat="server" class="label label-danger xs" Visible="false" />
                                </div>
                                <div class="form-group">
                                    <label for="txtSmtp">SMTP Address</label>

                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="fa fa-laptop"></i>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtSmtp" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="txtPort">Port #</label>
                                    <asp:TextBox runat="server" ID="txtPort" CssClass="form-control" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPort" Display="Dynamic"
                                        ErrorMessage="Should be valid integer between 2 to 4 digits" CssClass="label label-warning" ValidationExpression="\d{2,4}"></asp:RegularExpressionValidator>

                                </div>
                                <div class="form-group">
                                    <label for="ddlAdsl">Authentication Required</label>
                                    <asp:DropDownList runat="server" ID="ddlAdsl" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y" />
                                        <asp:ListItem Text="NO" Value="N" Selected="True" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button Text="Submit" ID="btnSubmit" CssClass="btn btn-primary" runat="server" OnClick="btnSubmit_Click" />
                                <asp:Button Text="Load Default" runat="server" CssClass="btn btn-warning" ID="btnDefault" OnClick="btnDefault_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">Test Mail Settings<br />
                                <small>Save Data before sending test mail</small></h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="txtToMail">Send Test Mail to...</label>
                                    <asp:TextBox runat="server" ID="txtToMail" CssClass="form-control" />
                                </div>

                            </div>
                            <div class="box-footer">
                                <asp:Button Text="Send Test Mail" runat="server" ID="btnTestMail" CssClass="btn btn-success" OnClick="btnTestMail_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-info">
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="ddlMailTrigger">Enable Mail Triggers</label>
                                    <asp:DropDownList runat="server" ID="ddlMailTrigger" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y" />
                                        <asp:ListItem Text="NO" Value="N" />
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="ddlReminders">Enable Reminders & Escalations</label>
                                    <asp:DropDownList runat="server" ID="ddlReminders" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y" />
                                        <asp:ListItem Text="NO" Value="N" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.row -->

            </section>
            <!-- /.content -->
        </aside>
        <asp:HiddenField ID="hdnPwd" Value="" runat="server" />
        <!-- /.right-side -->
    </form>
</asp:Content>

