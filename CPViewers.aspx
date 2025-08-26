<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CPViewers.aspx.cs" Inherits="CPViewers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Control Plan Viewers
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
                                    <label for="txtEmployeeRollNo"><i class="fa fa-check-circle"></i>&nbsp;Employee Roll/ Id.#</label>
                                    <asp:TextBox runat="server" ID="txtEmployeeRollNo" CssClass="form-control" Text="" style="text-transform:uppercase;" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="txtEmployeeRollNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtEmployeeName"><i class="fa fa-check-circle"></i>&nbsp;Employee Name</label>
                                    <asp:TextBox runat="server" ID="txtEmployeeName" CssClass="form-control" Text="" style="text-transform:uppercase;" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="txtEmployeeName" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                
                                <div class="form-group">
                                    <label for="txtLoginId">Login Id.</label>
                                    <asp:TextBox runat="server" ID="txtLoginId" CssClass="form-control" autoComplete="off" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtLoginId"
                                        ErrorMessage="Enter Login Id !" Display="Dynamic" CssClass="label label-danger" ValidationGroup="Save" />
                                </div>
                                <div class="form-group">
                                    <label for="txtPassword">Password</label>
                                    <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password" />
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
                                <h3 class="box-title">List of Employees</h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static" 
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:BoundField DataField="EmployeeRollNo" HeaderText="Roll #/ Id.#" />
                                        <asp:HyperLinkField DataNavigateUrlFields="EmployeeSlNo" HeaderText="Employee Name"
                                            DataNavigateUrlFormatString="~/Employees.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="EmployeeName" />
                                         <asp:BoundField DataField="del_status" HeaderText="Active / In-Active" />                                        
                                     <asp:BoundField DataField="status1" HeaderText="Active / In-Active" Visible="false" />                                        
                                   
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="visibility:hidden;">
                    <div class="form-group has-success">
                                    <label for="txtEmailId"><i class="fa fa-check-circle"></i>&nbsp;E-Mail Id.</label>
                                    <asp:TextBox runat="server" ID="txtEmailId" CssClass="form-control" Text="" style="text-transform:uppercase;" />
                                    
                                </div>

                                <div class="form-group has-success">
                                    <label for="ddlCanPrepare"><i class="fa fa-check-circle"></i>&nbsp;Can Prepare</label>
                                    <asp:DropDownList runat="server" ID="ddlCanPrepare" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="N" Selected="True"></asp:ListItem>                                    
                                    </asp:DropDownList>
                                   
                                </div>
                                <div class="form-group has-success">
                                    <label for="ddlCanApprove"><i class="fa fa-check-circle"></i>&nbsp;Can Approve</label>
                                    <asp:DropDownList runat="server" ID="ddlCanApprove" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="N" Selected="True"></asp:ListItem>                                    
                                    </asp:DropDownList>
                                    
                                </div>
                                <div class="form-group has-success">
                                    <label for="ddlisAdmin"><i class="fa fa-check-circle"></i>&nbsp;Is Admin</label>
                                    <asp:DropDownList runat="server" ID="ddlisAdmin" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="N" Selected="True"></asp:ListItem>                                    
                                    </asp:DropDownList>
                                    
                                </div>
                                <div class="form-group has-success">
                                    <label for="ddlallmaster"><i class="fa fa-check-circle"></i>&nbsp;Access to all Master</label>
                                    <asp:DropDownList runat="server" ID="ddlallmaster" CssClass="form-control">
                                        <asp:ListItem Text="YES" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="N" Selected="True"></asp:ListItem>                                    
                                    </asp:DropDownList>
                                    
                                </div>
                </div>
            </section>
        </div>
        <asp:HiddenField ID="hdnSlNo" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" Value="I" />
        <asp:HiddenField ID="hdnPassword" runat="server" Value="" />

    </form>

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%=txtEmployeeRollNo.ClientID%>').blur(function () {
                var x = $('#<%=txtEmployeeRollNo.ClientID%>').val();
                $('#<%=txtLoginId.ClientID%>').val(x);
                $('#<%=txtPassword.ClientID%>').val(x);
                $('#<%=hdnPassword.ClientID%>').val(x);
            });
        });
        </script>

</asp:Content>

