<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="dcr.aspx.cs" Inherits="dcr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        #modalQaApproval .modal-dialog {
            width: 75%;
        }

        .datepicker {
            z-index: 1151 !important;
        }
    </style>
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Document Change Request
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
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-header">
                                <div class="col-sm-8 col-md-offset-4">
                                    <div class="pull-right">

                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" PostBackUrl="~/dcr_qry.aspx" />
                                        <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="btn btn-danger" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnSubmit" Text="SUBMIT" runat="server" CssClass="btn btn-warning" Enabled="false" OnClick="btnSubmit_Click" />
                                        <asp:Button ID="btnApprove" Text="APPROVE" runat="server" CssClass="btn btn-warning" Enabled="false" OnClick="btnApprove_Click" />
                                    </div>
                                </div>
                            </div>

                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="Request_Date"><i class="fa fa-check-circle"></i>&nbsp;Request Date</label>
                                            <asp:TextBox runat="server" ID="txtRequestDate" CssClass="form-control" ReadOnly></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtRequestDate" ErrorMessage="Data Missing!" ControlToValidate="txtRequestDate" runat="server" InitialValue="Select..."
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="dcr_number"><i class="fa fa-check-circle"></i>&nbsp;DCR Number</label>
                                            <asp:TextBox runat="server" ID="txtdcr_number" CssClass="form-control" ReadOnly></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtdcr_number" ErrorMessage="Data Missing!" ControlToValidate="txtdcr_number" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="Request_By"><i class="fa fa-check-circle"></i>&nbsp;Request By</label>
                                            <asp:DropDownList runat="server" ID="ddlEmployees" CssClass="form-control myselect">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvtxtRequestBy" ErrorMessage="Data Missing!" ControlToValidate="ddlEmployees" runat="server" InitialValue="Select..."
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="mstPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part Number</label>
                                            <asp:DropDownList runat="server" ID="ddlmstPartNo" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlmstPartNo_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvmstPartNo" ErrorMessage="Data Missing!" ControlToValidate="ddlmstPartNo" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="PartDescription"><i class="fa fa-check-circle"></i>&nbsp;Part Name</label>
                                            <asp:TextBox runat="server" ID="txtPartName" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvddlPartDescription" ErrorMessage="Data Missing!" ControlToValidate="txtPartName" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="operation_slno"><i class="fa fa-check-circle"></i>&nbsp;Operation</label>
                                            <%-- <asp:DropDownList runat="server" ID="ddloperationslno" CssClass="form-control myselect" >
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>--%>
                                            <asp:ListBox runat="server" ID="ddloperationslno" CssClass="form-control myselect2" SelectionMode="Multiple">
                                                <asp:ListItem Text="Select..." Value="" />

                                            </asp:ListBox>
                                            <asp:RequiredFieldValidator ID="rfvddloperationslno" ErrorMessage="Data Missing!" ControlToValidate="ddloperationslno" runat="server" InitialValue=""
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="change_area"><i class="fa fa-check-circle"></i>&nbsp;Changes Required Area</label>
                                            <asp:DropDownList runat="server" ID="ddlchangearea" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="ddlchangearea_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Select..." Value="0" />
                                                <%--      <asp:ListItem Text="SOP" Value="SOP" />--%>
                                                <asp:ListItem Text="CP" Value="CP" />
                                                <asp:ListItem Text="Drawing Revision" Value="Drawing Revision" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlchangearea" ErrorMessage="Data Missing!" ControlToValidate="ddlchangearea" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkOpNo" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Process Number</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkProcess" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Process Name</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkprdchar" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Product Characteristics</label>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkprcchar" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Process Characteristics</label>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkSpecificaiton" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Specificaiton</label>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkMeasurement_Tech" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Measurement Technique</label>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkSample_size" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Sample Size</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkSampleFreq" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Sample Frequency</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkControlMethod" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Control method</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkOthers" CssClass="form-check-input" runat="server" />
                                            <label class="form-check-label" for="radCP">Others</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group has-success">
                                            <label class="form-check-label" for="radCP"><i class="fa fa-check-circle"></i>&nbsp;Existing</label>
                                            <asp:TextBox ID="txtExisting" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtExisting" ErrorMessage="Data Missing!" ControlToValidate="txtExisting" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories">
                                            </asp:RequiredFieldValidator>
                                        </div>

                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-group has-success">
                                            <label class="form-check-label" for="radCP"><i class="fa fa-check-circle"></i>&nbsp;Changes Required</label>
                                            <asp:TextBox ID="txtChanges" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtChange" ErrorMessage="Data Missing!" ControlToValidate="txtChanges" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="form-group has-success">
                                            <label class="form-check-label" for="radCP"><i class="fa fa-check-circle"></i>&nbsp;Reason For Change</label>
                                            <asp:TextBox ID="txtReason" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtReason" ErrorMessage="Data Missing!" ControlToValidate="txtReason" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="form-group has-success">
                                            <label class="form-check-label"><i class="fa fa-check-circle"></i>&nbsp;Nature of Change</label>
                                            <asp:TextBox ID="txtnatureOfChange" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtnatureOfChange" ErrorMessage="Data Missing!" ControlToValidate="txtnatureOfChange" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>


                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </section>
        </div>
        <asp:HiddenField runat="server" ID="hdnEditMode" Value="I" />
        <asp:HiddenField runat="server" ID="hdnSlNo" />
        <asp:HiddenField runat="server" ID="hdnsubmitstatus" />
            <asp:HiddenField runat="server" ID="hdnsubmitdate" />
        <asp:HiddenField runat="server" ID="hdnops" />
        <asp:HiddenField runat="server" ID="hdnopstxt" />
    </form>
    <link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/handsontable/handsontable.full.min.js"></script>
    <script src="Scripts/hotdata.js"></script>
    <script src="Scripts/prop.js"></script>
    <script type="text/javascript" src="Scripts/ajax.js"></script>

    <script>
        $('.myselect').select2({
            theme: "classic"
        });
        $(document).ready(function () {

            var initialValues = $('#<%= hdnops.ClientID %>').val().split(',');
            $('.myselect2').val(initialValues).trigger('change');
            updateHiddenField();


            $('.myselect2').select2({
                theme: "classic"
            }).on('change', function () {
                // Get selected values as an array
                var selectedValues = $(this).val();
                var selectedTexts = $(this).find('option:selected').map(function () {
                    return $(this).text();
                }).get();
                // Join the selected values into a comma-separated string
                var selectedValuesString = selectedValues ? selectedValues.join(',') : '';
                var selectedTextString = selectedTexts.join(',');

                // Update the hidden field with the selected texts
                $('#<%=hdnopstxt.ClientID %>').val(selectedTextString);
                // Update the hidden field with the selected values
                $('#<%=hdnops.ClientID %>').val(selectedValuesString);
                console.log($('#<%=hdnops.ClientID %>').val());
            });




            function updateHiddenField() {
                // Get selected texts
                var selectedTexts = $('.myselect2').find('option:selected').map(function () {
                    return $(this).text();
                }).get();

                // Join the selected texts into a comma-separated string
                var selectedtextString = selectedTexts.join(',');

                // Update the hidden field with the selected texts
                $('#<%= hdnopstxt.ClientID %>').val(selectedtextString);
            }
        });
    </script>
</asp:Content>

