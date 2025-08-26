<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Containment.aspx.cs" Inherits="Containment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Containment
                    <small class="pull-right"><a href="Containment_Landing.aspx" class="btn btn-sm bg-orange" title="Go Back.."><i class="fa fa-backward"></i></a></small>
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="box small">
                            <table class="table table-condensed">
                                <tr>
                                    <th>Complaint #</th>
                                    <th>Reported Dt.</th>
                                    <th>Initiator</th>
                                    <th>Group</th>
                                    <th>Type</th>
                                    <th>Plant</th>
                                    <th>Customer</th>
                                    <th>Cust.Part #</th>
                                    <th>Rejn.Qty.</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label Text="" ID="lblComplNo" runat="server" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblReptDt" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblInitiator" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblComplGroup" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblComplType" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblPlant" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblCustomer" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblCustPartNo" /></td>
                                    <td>
                                        <asp:Label Text="" runat="server" ID="lblRejnQty" /></td>
                                    <td>
                                        <a class="btn btn-default" data-toggle="modal" data-target="#modalProblem"><i class="fa fa-info"></i>&nbsp;Problem Decription</a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="box no-border no-margin">
                            <div class="messagealert" id="alert_container">
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="pull-right">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories"  />
                            <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" ValidationGroup="mandatories"  />
                            <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-danger" OnClientClick="return confirmation();" />
                            <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary"  />
                            <asp:Button ID="btnSubmit" Text="SUBMIT" runat="server" CssClass="btn btn-warning"  />    
                        </div>
                    </div>
                </div>
                <p></p>
                <div class="row">
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="txtMfgBatchNo">Manufacturing Batch #</label>
                                    <asp:TextBox runat="server" ID="txtMfgBatchNo" CssClass="form-control" />
                                </div>
                                <div class="form-group">
                                    <label for="txtMfgDate">Manufacturing Date</label>
                                    <asp:TextBox runat="server" ID="txtMfgDate" CssClass="form-control myDate" />
                                </div>
                                <div class="form-group">
                                    <label for="txtShift">Shift & Production Time</label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtShift" /></td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtProdnTime" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="nav-tabs-custom">
                                    <ul class="nav nav-tabs">
                                        <li class="active"><a href="#tab_1" data-toggle="tab">Suspected Lot Details</a></li>
                                        <li><a href="#tab_2" data-toggle="tab">Sorting / Rework Method</a></li>
                                        <li><a href="#tab_3" data-toggle="tab">Sorting / Rework Details</a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane active" id="tab_1">
                                            <table class="table table-condensed">
                                                <tr>
                                                    <th>Location</th>
                                                    <th>Batch #</th>
                                                    <th>Quantity</th>
                                                </tr>
                                                <tr>
                                                    <td>In-House</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtInHouse1" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtInHouseQty" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Transit</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtTransit1" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtTransitQty" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Warehouse</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtWarehouse1" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtWareHouseQty" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Control Line</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtControl1" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtControlQty" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Field</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtField1" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFieldQty" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <!-- /.tab-pane -->
                                        <div class="tab-pane" id="tab_2">
                                            <table class="table table-bordered">
                                                <tr>
                                                    <td>Sorting / Rework Procedure / Steps</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtSorting" CssClass="form-control" Width="200px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Verification results of Interim Solution</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtVerification" CssClass="form-control" Width="200px" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <!-- /.tab-pane -->
                                        <div class="tab-pane" id="tab_3">
                                            <table class="table table-condensed">
                                                <tr>
                                                    <th>Location</th>
                                                    <th>Batch #</th>
                                                    <th>Quantity</th>
                                                </tr>
                                                <tr>
                                                    <td>In-House</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtInHouse2" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtInHouseQty2" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Transit</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtTransit2" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtTransitQty2" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Warehouse</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtWarehouse2" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtWareHouseQty2" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Customer</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtControl2" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtControlQty2" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Field</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtField2" CssClass="form-control" /></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFieldQty2" CssClass="form-control" Width="100px" /></td>
                                                </tr>
                                            </table>

                                        </div>
                                        <!-- /.tab-pane -->
                                    </div>
                                    <!-- /.tab-content -->
                                    <div class="form-group">
                                        <label for="txtNotes">Notes</label>
                                        <asp:TextBox runat="server" ID="txtNotes" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                                <!-- nav-tabs-custom -->

                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <asp:HiddenField ID="hdnEditMode" runat="server" Value="I" />
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />


        <div class="modal fade" id="modalProblem" tabindex="-1" role="dialog" aria-labelledby="modalProblem"
            aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;</button>
                        <h4 class="modal-title" id="H2">Problem Description</h4>
                        <br />
                    </div>
                    <div class="modal-body" style="height: 300px;">
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtProblemDesc" CssClass="form-control" textmode="MultiLine" Rows="12" disabled  />
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

