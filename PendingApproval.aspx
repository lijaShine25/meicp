<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingApproval.aspx.cs" Inherits="PendingApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <section class="content-header">
                <h1>Pending for Approval</h1>
            </section>
            <section class="content">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static"
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="cp_slno" HeaderText="Control Plan Number"
                                            DataNavigateUrlFormatString="~/ControlPlan2.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="cp_number" />
                                        <asp:BoundField DataField="mstPartNo" HeaderText="Part No." />
                                        <asp:BoundField DataField="partdescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="OperationDesc" HeaderText="Part Issue Dt." />
                                        <asp:BoundField DataField="cp_revno" HeaderText="Rev.No." />
                                        <asp:BoundField DataField="cp_revdt" HeaderText="Rev.Dt." />
                                        <asp:BoundField DataField="employeename" HeaderText="Prepared By" />
                                        <asp:BoundField DataField="CP_Submit_DateTime" HeaderText="Submit Date & Time" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section class="content-header">
                <h1>DCR - Pending for Approval</h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static"
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdDCR" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="dcr_slno" HeaderText="DCR Number"
                                            DataNavigateUrlFormatString="~/dcr.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="dcr_number" />
                                        <asp:BoundField DataField="mstPartNo" HeaderText="Part No." />
                                        <asp:BoundField DataField="partdescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="OperationDesc" HeaderText="Part Issue Dt." />
                                        <asp:BoundField DataField="employeename" HeaderText="Request By" />
                                        <asp:BoundField DataField="DCR_Submit_DateTime" HeaderText="Submit Date & Time" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </form>

</asp:Content>

