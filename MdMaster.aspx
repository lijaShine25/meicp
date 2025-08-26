<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MdMaster.aspx.cs" Inherits="MdMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>MD Master
                </h1>
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
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">Add/ Edit</h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="txtequipno"><i class="fa fa-check-circle"></i>&nbsp;Equipment/ Gauge Number</label>
                                    <asp:TextBox runat="server" ID="txtequipno" CssClass="form-control" Text="" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="txtequipno" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="mdcat"><i class="fa fa-check-circle"></i>&nbsp;MD Category</label>
                                    <asp:TextBox runat="server" ID="txtmdcat" CssClass="form-control" Text=""  />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="txtmdcat" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                               
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-danger" OnClick="btnDelete_Click" Enabled="false" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-7">
                        <div class="box box-info">
                            <div class="box-header">
                                <h3 class="box-title">List of MDs</h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static" 
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="md_slno" HeaderText="Equipment/ Gauge No."
                                            DataNavigateUrlFormatString="~/MdMaster.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="equip_number" />
                                        <asp:BoundField DataField="md_category" HeaderText="Category" />
                                    </Columns>

                                </asp:GridView>
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

