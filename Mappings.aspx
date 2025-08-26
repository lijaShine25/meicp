<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Mappings.aspx.cs" Inherits="Mappings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <form id="Form1" role="form" runat="server" autocomplete="off">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Mappings
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
                                    <label for="ddlpart_slno"><i class="fa fa-check-circle"></i>&nbsp;Part</label>
                                     <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" >
                                                <asp:ListItem Text="Select..." />
                                            </asp:DropDownList>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>

                                <div class="form-group has-success">
                                    <label for="ddloperation_slno"><i class="fa fa-check-circle"></i>&nbsp;Operation</label>
                                     <asp:DropDownList runat="server" ID="ddloperation_slno" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddloperation_slno_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Select..." />
                                            </asp:DropDownList>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddloperation_slno" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>

                                <div class="form-group has-success">
                                    <label for="txtProcessNo"><i class="fa fa-check-circle"></i>&nbsp;Process Number</label>
                                    <asp:TextBox runat="server" ID="txtProcessNo" CssClass="form-control" Text="" style="text-transform:uppercase;"  onkeypress="return isNumberKey(event)" />
                                    <asp:RequiredFieldValidator ID="rfPN" ErrorMessage="Data Missing!" ControlToValidate="txtProcessNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="ddlmachine_slno"><i class="fa fa-check-circle"></i>&nbsp;Machine</label>
                                    <asp:DropDownList runat="server" ID="ddlmachine_slno" CssClass="form-control">
                                    </asp:DropDownList>
                                    <span class="bg-light-blue text-muted">Select Operation to Display Machines</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="ddlmachine_slno" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />

                                </div>

                                <div>
                                <asp:TextBox runat="server" ID="txtMapSlNo" CssClass="form-control" Text="" Style="text-transform: uppercase; visibility:hidden" />
                                     </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-danger" OnClick="btnDelete_Click" Enabled="false" />
                            <div class="pull-right">
                                <asp:Button Text="Filter Data" ID="btnFilter" CssClass=" btn btn-info" runat="server" OnClick="btnFilder_Click" />
                            </div>
                            </div>
                            
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="box box-info">
                            <div class="box-header">
                                <h3 class="box-title">List of Mappings</h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static" 
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">

                                    <Columns>
                                        <asp:BoundField DataField="mstPartNo" HeaderText="Part" />
                                        <asp:BoundField DataField="OperationDesc" HeaderText="Operation" />
                                        
                                       <asp:BoundField DataField="process_no" HeaderText="Process Number" />
                                        
                                        <asp:HyperLinkField DataNavigateUrlFields="map_slno" HeaderText="Machine"
                                            DataNavigateUrlFormatString="~/Mappings.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="MachineDesc" />


                                        <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("map_slno") %>' runat="server" ID="MapSlNo" />
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

 function isNumberKey(evt) {
     var charCode = (evt.which) ? evt.which : event.keyCode;
     if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
         return false;
     return true;
 }

    </script>
</asp:Content>

