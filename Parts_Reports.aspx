<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Parts_Reports.aspx.cs" Inherits="Parts_Reports" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" runat="server"  role="form">
         <aside class="right-side">
       <%-- <section class="content-header">
                <h1>Parts Master Report</h1>
            </section>      --%>    
            <section class="content-header">
                <h1>Parts Master Report

                <small class="pull-right"> 
                      <asp:Button ID="btnExportXL" Text="Export to Excel" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnExportXL_Click" />
                </small>
                </h1>
            </section>
            <section class="content">

                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <div style="overflow: auto;">
                                <asp:GridView runat="server" ID="grdData"  CssClass="table table-striped table-bordered"  ClientIDMode="Static" 
                                     ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"   OnPreRender="grdData_PreRender" 
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue" ShowFooter="false" CellSpacing="0" Width="100%" >

                                    <Columns>                                        
                                        <asp:BoundField DataField="mstPartNo" HeaderText="Part Number" />                               
                                        <asp:BoundField DataField="PartDescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="partIssueNo" HeaderText="Part Issue No" />
                                        <asp:BoundField DataField="partIssueDt" HeaderText="Part Issue Date" />
                                       <%-- <asp:BoundField DataField="cpType" HeaderText="Control Plan Type" />--%>
                                        <asp:BoundField DataField="Customer_name" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="customerPartNo" HeaderText="Customer Part No" />
                                        <asp:BoundField DataField="customerIssueNo" HeaderText="Customer Issue No" />
                                        <asp:BoundField DataField="customerIssueDt" HeaderText="Customer Issue Date" />
                                      <%--  <asp:BoundField DataField="keyContact" HeaderText="Key Contact" />
                                        <asp:BoundField DataField="keyContactPhone" HeaderText="Key Contact Phone" />
                                        <asp:BoundField DataField="originalDt" HeaderText="Original Date" />
                                        <asp:BoundField DataField="organization" HeaderText="Origanization" />                                       
                                         <asp:BoundField DataField="orgApprovalDt" HeaderText="Origanization Approval" />
                                        <asp:BoundField DataField="orgDate" HeaderText="Origanization Approval Date" />
                                         <asp:BoundField DataField="custApproval" HeaderText="Customer Approval " />
                                         <asp:BoundField DataField="custApprovalDt" HeaderText="Customer Approval Date" />
                                         <asp:BoundField DataField="custQaApproval" HeaderText="Customer QA Approval " />
                                         <asp:BoundField DataField="custQaApprovalDt" HeaderText="Customer QA Approval Date" />
                                       <asp:BoundField DataField="otherApproval" HeaderText="Other Approval " />
                                         <asp:BoundField DataField="otherApprovalDt" HeaderText="Other Approval Date" />--%>
                                       <asp:BoundField DataField="del_status" HeaderText="Active/In-Active" />   
                                       <asp:BoundField DataField="Remarks" HeaderText="Remarks" />                                                                        

                                    </Columns>
                                </asp:GridView>
                              </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
      
      
 </aside>
    </form>
</asp:Content>

