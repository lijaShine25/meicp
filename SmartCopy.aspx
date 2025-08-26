<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SmartCopy.aspx.cs" Inherits="SmartCopy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }

        .fileStrike {
            text-decoration: line-through;
        }

        .fileUnStrike {
            text-decoration: none;
        }
    </style>
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Copy
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-md-8">
                        <div class="box no-border no-header">
                            <div class="pull-right">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click"  />
                                <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <p></p>
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
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">Source Part Number</h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="txtOperationDesc"><i class="fa fa-check-circle"></i>&nbsp;Select Source Part Number</label>
                                    <asp:DropDownList runat="server" ID="ddlSourcePart" CssClass="form-control" OnSelectedIndexChanged="ddlSourcePart_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Select..." />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlSourcePart" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="Select..." />
                                </div>
                                <div class="form-group">
                                    <label for="grdSourceOperations">Select Operations to Copy</label>
                                    <asp:GridView runat="server" ClientIDMode="Static"
                                        CssClass="table table-bordered table-responsive table-hover" ID="grdSourceOperations" UseAccessibleHeader="true"
                                        ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue" OnRowDataBound="grdSourceOperations_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox  Text=""  ID="lblChkSel" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("operation_slno") %>'   ID="lblOperationSlNo" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="operationdesc" HeaderText="Operation Description" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">Destination Part Number</h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="txtOperationDesc"><i class="fa fa-check-circle"></i>&nbsp;Select Destination Part Number</label>
                                    <asp:DropDownList runat="server" ID="ddlDestnPart" CssClass="form-control">
                                        <asp:ListItem Text="Select..." />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="ddlDestnPart" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="Select..." />
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </section>
        </div>
        <asp:HiddenField ID="hdnSlNo" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" Value="I" />

    </form>
</asp:Content>

