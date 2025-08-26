<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ControlPlan_Approval.aspx.cs" Inherits="ControlPlan_Approval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Control Plan - Approval
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
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="col-md-4">
                                    <div class="form-group has-success">
                                        <label for="ddlpart_slno"><i class="fa fa-check-circle"></i>&nbsp;Part</label>
                                        <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button Text="Filter Data" ID="btnFilter" CssClass=" btn btn-info" runat="server" OnClick="btnFilder_Click" />
                                <asp:Button Text="Clear" ID="btnClear" CssClass=" btn btn-info" runat="server" OnClick="btnClear_Click" />
                                <div class="pull-right">
                                    <span><i>*Select atleast one checkbox to submit</i></span>
                                     <asp:Button ID="btnSubmit" Text="Approve" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                                     <asp:Button ID="btnviewall" Text="ViewAll" runat="server" CssClass="btn btn-primary"  OnClick="btnviewall_Click" />
                                   
                                </div>
                            </div>
                            <%-- </div>
                    <div class="col-md-8">--%>
                            <div class="box box-info">
                                <div class="box-header">
                                    <h3 class="box-title">List of Mappings</h3>
                                </div>
                                <div class="box-body">
                                    <asp:GridView runat="server" ClientIDMode="Static"
                                        CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                        ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue" OnRowDataBound="grdData_OnRowDataBound">

                                        <Columns>
                                            <asp:BoundField DataField="mstPartNo" HeaderText="Part" />
                                            <asp:BoundField DataField="OperationDesc" HeaderText="Operation" />

                                            <asp:BoundField DataField="process_no" HeaderText="Process Number" />
                                            <asp:BoundField DataField="MachineDesc" HeaderText="Process Number" />
                                            <%--<asp:HyperLinkField DataNavigateUrlFields="map_slno" HeaderText="Machine"
                                            DataNavigateUrlFormatString="~/Mappings.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="MachineDesc" />--%>


                                            <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("map_slno") %>' runat="server" ID="MapSlNo" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol"  Visible="false" >
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("cp_slno") %>' runat="server" ID="CPSlNo" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cpdone" HeaderText="Is Control Plan Done" />
                                            <asp:BoundField DataField="Submitstatus" HeaderText="Submit Status" />
                                             <asp:BoundField DataField="CP_Submit_DateTime" HeaderText="Submit Date & Time" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"  />
                                            <asp:BoundField DataField="is_approved" HeaderText="Approval Status" />
                                             <asp:BoundField DataField="CP_Approved_DateTime" HeaderText="Approval Date & Time" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"  />
                                            <asp:TemplateField HeaderText="Select">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

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
        <asp:HiddenField ID="hdnMachineSlNo" runat="server" />
        <asp:HiddenField ID="hdnOperationSlNo" runat="server" />
        <asp:HiddenField ID="hdnProcessNo" runat="server" />

    </form>
    <script>
        $('.myselect').select2({
            theme: "classic"
        });

    </script>
</asp:Content>

