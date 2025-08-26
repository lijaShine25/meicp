<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SampleFreq.aspx.cs" Inherits="SampleFreq" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Sample Frequency
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
                            <div class="box-body">
                                <div class="form-group has-success hidden">
                                    <label for="txtsamplsize"><i class="fa fa-check-circle"></i>&nbsp;Sample Size</label>
                                    <asp:TextBox runat="server" ID="txtsamplsize" CssClass="form-control" Text="" TextMode="MultiLine" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="txtsamplsize" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />--%>
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtFreqDesc"><i class="fa fa-check-circle"></i>&nbsp;Frequency Description</label>
                                    <asp:TextBox runat="server" ID="txtFreqDesc" CssClass="form-control" Text=""  TextMode="MultiLine" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="txtFreqDesc" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group" hidden="hidden">
                                    <div class="checkbox">
                                        <asp:CheckBox CssClass="checkbox-inline" Text="Show in FOI" ID="chkfoi" runat="server" /><br />
                                        <asp:CheckBox CssClass="checkbox-inline" Text="Show in PCC" ID="chkpcc" runat="server" /><br />
                                        <asp:CheckBox CssClass="checkbox-inline" Text="Show in PMC" ID="chkpmc" runat="server" /><br />
                                        <asp:CheckBox CssClass="checkbox-inline" Text="Show in Matl.Test Report" ID="chkmtl" runat="server" /><br />
                                        <asp:CheckBox CssClass="checkbox-inline" Text="Show in Packing Report" ID="chkpacking" runat="server" />
                                        <asp:CheckBox CssClass="checkbox-inline" Text="Show in Dock Audit Report" ID="chkdock" runat="server" />
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label for="ddlActiveInactive">Active / In-Active</label>                                 
                                      <asp:DropDownList runat="server" ID="ddlActiveInactive" CssClass="form-control" Enabled="false">                                     
                                      <asp:ListItem Text="ACTIVE" Value="N" />
                                      <asp:ListItem Text="IN-ACTIVE" Value="Y" />                                   
                                      </asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="Rfd_AI" ErrorMessage="Data Missing!" ControlToValidate="ddlActiveInactive" runat="server" InitialValue="Select..."
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
                    <div class="col-md-8">
                        <div class="box box-info">
                            <div class="box-header">
                                <h3 class="box-title">List of Sample Frequency</h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static" 
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                         <%--<asp:BoundField DataField="sample_size" HeaderText="Sample Size" />--%>
                                        <asp:HyperLinkField DataNavigateUrlFields="freq_slno" HeaderText="Frequency Description"
                                            DataNavigateUrlFormatString="~/SampleFreq.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="FreqDesc" />
                                        <asp:BoundField DataField="del_status" HeaderText="Active / In-Active" Visible="false" />                                        
                                        <asp:BoundField DataField="status1" HeaderText="Active / In-Active" Visible="false" />                                        
                                       <%-- <asp:BoundField DataField="foi_txt" HeaderText="FOI" />
                                        <asp:BoundField DataField="pcc_txt" HeaderText="PCC" />
                                        <asp:BoundField DataField="pmc_txt" HeaderText="PMC" />
                                        <asp:BoundField DataField="mtl_txt" HeaderText="Material Test" />
                                        <asp:BoundField DataField="packing_txt" HeaderText="Packing" />--%>
                                   
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

