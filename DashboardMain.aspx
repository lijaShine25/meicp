<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DashboardMain.aspx.cs" Inherits="DashboardMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Welcome to Control Plan Software
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-md-12 text-center">
                        <h1>
                            <br />
                            <span id="pending" runat="server" class="text-capitalize text-danger">
                                <i class="fa fa-bell fa-beat-fade"></i>
                                You have Approvals Pending !!<br />
                                <br />
                                <a href="PendingApproval.aspx">Click here to open</a>
                            </span>
                        </h1>
                    </div>
                </div>

                <!-- Control Plan and SOP Tables -->
                <div class="row">
                    <div class="col-md-6">
                        <!-- Control Plan Table -->
                        <h3>Control Plan Summary</h3>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Description</th>
                                    <th>Count</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td><strong>TOTAL CONTROL PLAN</strong></td>
                                    <td>
                                        <asp:Label ID="lblTotalControlPlan" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><strong>ACTIVE - CONTROL PLAN</strong></td>
                                    <td>
                                        <asp:Label ID="lblActiveControlPlan" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><strong>INACTIVE - CONTROL PLAN</strong></td>
                                    <td>
                                        <asp:Label ID="lblInactiveControlPlan" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><strong>TOTAL APPROVED - CONTROL PLAN</strong></td>
                                    <td>
                                        <asp:Label ID="lblTotalApprovedControlPlan" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><strong>NOT SUBMITTED - CONTROL PLAN</strong></td>
                                    <td>
                                        <asp:Label ID="lblNotSubmittedControlPlan" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><strong>SUBMITTED - CONTROL PLAN</strong></td>
                                    <td>
                                        <asp:Label ID="lblSubmittedControlPlan" runat="server"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div class="col-md-6">
                        <!-- SOP Table -->
                        <h3>SOP Summary</h3>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Description</th>
                                    <th>Count</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td><strong>TOTAL SOP</strong></td>
                                    <td>
                                        <asp:Label ID="lblTotalSop" runat="server" Text="0"></asp:Label></td>
                                    <!-- Placeholder for TOTAL SOP count -->
                                </tr>
                                <tr>
                                    <td><strong>ACTIVE - SOP</strong></td>
                                    <td>
                                        <asp:Label ID="lblActiveSop" runat="server" Text="0"></asp:Label></td>
                                    <!-- Placeholder for ACTIVE SOP count -->
                                </tr>
                                <tr>
                                    <td><strong>INACTIVE - SOP</strong></td>
                                    <td>
                                        <asp:Label ID="lblInactiveSop" runat="server" Text="0"></asp:Label></td>
                                    <!-- Placeholder for INACTIVE SOP count -->
                                </tr>
                                <tr>
                                    <td><strong>TOTAL APPROVED - SOP</strong></td>
                                    <td>
                                        <asp:Label ID="lblTotalApprovedSop" runat="server" Text="0"></asp:Label></td>
                                    <!-- Placeholder for TOTAL APPROVED SOP count -->
                                </tr>
                                <tr>
                                    <td><strong>NOT SUBMITTED - SOP</strong></td>
                                    <td>
                                        <asp:Label ID="lblNotSubmittedSop" runat="server" Text="0"></asp:Label></td>
                                    <!-- Placeholder for NOT SUBMITTED SOP count -->
                                </tr>
                                <tr>
                                    <td><strong>SUBMITTED - SOP</strong></td>
                                    <td>
                                        <asp:Label ID="lblSubmittedSop" runat="server" Text="0"></asp:Label></td>
                                    <!-- Placeholder for SUBMITTED SOP count -->
                                </tr>
                            </tbody>
                        </table>

                    </div>
                </div>
            </section>
        </div>
    </form>
</asp:Content>
