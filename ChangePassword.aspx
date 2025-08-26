<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server" role="form">
    <aside class="right-side">
        <section class="content-header">
            <h1>Change Password</h1>
        </section>
        <section class="content">
            <div class="row">
                    <div class="col-xs-12">
                        <div class="box no-border no-margin">
                            <div class="messagealert" id="alert_container">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="box box-primary no-border">
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="txtNewPassword"><i class="fa fa-check-circle"></i>&nbsp;Enter Old Password</label>
                                    <asp:TextBox runat="server" ID="txtOldPassword" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Password cannot be blank!" ControlToValidate="txtOldPassword" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtNewPassword"><i class="fa fa-check-circle"></i>&nbsp;Enter New Password</label>
                                    <asp:TextBox runat="server" ID="txtNewPassword" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Password cannot be blank!" ControlToValidate="txtNewPassword" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group">
                                    <label for="txtNewPassword2"><i class="fa fa-check-circle"></i>&nbsp;Re-Enter New Password</label>
                                    <asp:TextBox runat="server" ID="txtNewPassword2" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Password cannot be blank!" ControlToValidate="txtNewPassword" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                    <asp:CompareValidator ID="CompareValidator1" ErrorMessage="Password do not match!!" ControlToValidate="txtNewPassword2" runat="server" ControlToCompare="txtNewPassword"
                                        CssClass="label label-warning" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnSubmit" Text="Reset Password" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>

                </div>
        </section>
    </aside>
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="I" />
        <asp:HiddenField ID="hdnMode" runat="server" Value="" />
    </form>

</asp:Content>

