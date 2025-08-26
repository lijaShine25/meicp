<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Machines.aspx.cs" Inherits="Machines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Machines
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
                                    <label for="txtMachineCode"><i class="fa fa-check-circle"></i>&nbsp;Machine Code</label>
                                    <asp:TextBox runat="server" ID="txtMachineCode" CssClass="form-control" Text=""  />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="txtMachineCode" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtMachineDesc"><i class="fa fa-check-circle"></i>&nbsp;Machine Description</label>
                                    <asp:TextBox runat="server" ID="txtMachineDesc" CssClass="form-control" Text=""  />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="txtMachineDesc" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                               
                                 <div class="form-group has-success">
                                    <label for="ddloperation_slno"><i class="fa fa-check-circle"></i>&nbsp;Operation</label>
                                    <asp:DropDownList runat="server" ID="ddloperation_slno" CssClass="form-control myselect">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="ddloperation_slno" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
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
                    <div class="col-md-7">
                        <div class="box box-info">
                            <div class="box-header">
                                <h3 class="box-title">List of Machines</h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static" 
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:BoundField DataField="machinecode" HeaderText="Machine Code" />
                                        <asp:HyperLinkField DataNavigateUrlFields="machine_slno" HeaderText="Machine Description"
                                            DataNavigateUrlFormatString="~/Machines.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="MachineDesc" />
                                        <asp:BoundField DataField="OperationDesc" HeaderText="Operation" />
                                         <asp:BoundField DataField="del_status" HeaderText="Active / In-Active" />      
                                          <asp:BoundField DataField="status1" HeaderText="Active / In-Active" Visible="false" />                                        
                                                                     
                                   
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
     <script>
     $('.myselect').select2({
         theme: "classic"
     });
     </script>
</asp:Content>

