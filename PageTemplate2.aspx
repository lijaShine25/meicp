<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PageTemplate2.aspx.cs" Inherits="PageTemplate2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Page Title (or Form Name)
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="box no-border no-margin">
                            <div class="messagealert" id="alert_container">
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-8">
                        <div class="pull-right">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories"  />
                            <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" ValidationGroup="mandatories"  />
                            <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="btn btn-danger" OnClientClick="return confirmation();" />
                            <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary"  />
                            <asp:Button ID="btnSubmit" Text="SUBMIT" runat="server" CssClass="btn btn-warning"  />    
                            <a class="btn btn-success" data-toggle="modal" data-target="#modalQaApproval"><i class="fa fa-check-circle"></i>&nbsp;QA Approval</a>
                        </div>
                    </div>
                </div>
                <p></p>
                <div class="row">
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="TextBox1"><i class="fa fa-check"></i>&nbsp;Label</label>
                                    <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control"  />
                                </div>
                                <div class="form-group has-success">
                                    <label for="DropdownList1"><i class="fa fa-check"></i>&nbsp;Label</label>
                                    <asp:DropDownList runat="server" ID="DropdownList1" CssClass="form-control">
                                        <asp:ListItem Text="Select..." />
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="form-group">
                                    <label for="TextBox2">Label</label>
                                    <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control"  />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <asp:HiddenField ID="hdnEditMode" runat="server" Value="I" />
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />

        <div class="modal fade" id="modalQaApproval" tabindex="-1" role="dialog" aria-labelledby="modalQaApproval"
            aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;</button>
                        <h4 class="modal-title" id="H2">Modal Form</h4>
                        <br />
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            
                        </div>
                        
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                            Close</button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function () {

            $('.myDate').datepicker({
                dateFormat: 'dd-MM-yyyy',
                showOtherMonths: true,
                showStatus: true,
                changeMonth: true,
                changeYear: true,
                showWeek: true,
                firstDay: 1,
                defaultDate: "+1w",
                numberOfMonths: 1
            });


        });

    </script>

</asp:Content>

