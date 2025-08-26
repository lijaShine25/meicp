<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ControlPlanQuery.aspx.cs" Inherits="ControlPlanQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%-- <link href="Scripts/handsontable/jquery.handsontable.full.css" rel="stylesheet" />--%>
   <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>List of Items
                    <span class="pull-right">
                    <a href="ControlPlan2.aspx"><i class="fa fa-arrow-circle-left" title="Back to Main Page"></i></a>
                </span>
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box no-border">
                            <div class="box-body">


                                <asp:GridView runat="server" ID="grdData" CssClass="dataTable" ClientIDMode="Static"
                                    UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged = "OnSelectedIndexChanged" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="cp_slno"
                                            DataNavigateUrlFormatString="~/ControlPlan.aspx?cp_slno={0}&Editmode=E"
                                            ItemStyle-Font-Underline="true" DataTextField="cp_slno" />
                                        <asp:boundfield headertext="Control Plan Type" datafield="cpType" />
                                        <asp:BoundField HeaderText="Parts" DataField="part_slno" />
                                                                                                                     
                                        

                                      <%--  <asp:BoundField HeaderText="Uom Description" DataField="uom_description" />
                                        <asp:BoundField HeaderText="Item specification" DataField="item_spec" />
                                        <asp:BoundField HeaderText="Loaction" DataField="location" />
                                        <asp:BoundField HeaderText="Min Stock" DataField="min_stock" />
                                        <asp:BoundField HeaderText="Reorder Level" DataField="reorder_level" />
                                        <asp:BoundField HeaderText="Opening Stock" DataField="opening_stock" />                                         
                                        <asp:BoundField HeaderText="Current Stock" DataField="current_stock" />--%>
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

