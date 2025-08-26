<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Parts.aspx.cs" Inherits="Parts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Parts
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
                        <div class="pull-left">
                            <asp:Button Text="Save & Revise Part Master" ID="btnRevise" CssClass="btn btn-warning" runat="server" OnClick="btnRevise_Click" Enabled="false" ValidationGroup="mandatories" />
                        </div>

                        <div class="pull-right">
                            <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" PostBackUrl="~/Parts_Query.aspx" />
                            <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="btn btn-danger" OnClick="btnDelete_Click" OnClientClick="return confirmation();" Enabled="false" Visible="false" />
                            <asp:Button ID="btnClear" Text="Clear" runat="server" OnClick="btnClear_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnrev" Text="Rev.Entry" runat="server" CssClass="btn btn-warning" PostBackUrl="~/PartRevEntry.aspx" />
                        </div>
                    </div>
                </div>
                <p></p>
                <div class="row">
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part Number</label>
                                    <asp:TextBox runat="server" ID="txtPartNo" CssClass="form-control" Text="" Style="text-transform: uppercase;" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="txtPartNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtPartDescription"><i class="fa fa-check-circle"></i>&nbsp;Part Description</label>
                                    <asp:TextBox runat="server" ID="txtPartDescription" CssClass="form-control" Text="" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="txtPartDescription" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtpartIssueNo"><i class="fa fa-check-circle"></i>&nbsp;Part Issue #</label>
                                    <asp:TextBox runat="server" ID="txtpartIssueNo" CssClass="form-control" Text="" Style="text-transform: uppercase;" Enabled="false" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Data Missing!" ControlToValidate="txtpartIssueNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtpartIssueDt"><i class="fa fa-check-circle"></i>&nbsp;Part Issue Date</label>
                                    <asp:TextBox runat="server" ID="txtpartIssueDt" CssClass="form-control" TextMode="MultiLine" Rows="2" Enabled="false" />

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Data Missing!" ControlToValidate="txtpartIssueDt" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />

                                </div>
                                <div class="form-group has-success">
                                    <label for="ddlcpType"><i class="fa fa-check-circle"></i>&nbsp;Control Plan Type</label>
                                    <asp:DropDownList runat="server" ID="ddlcpType" CssClass="form-control">
                                        <asp:ListItem Text="Select..." />
                                        <asp:ListItem Text="PROTOTYPE" Value="PROTOTYPE" />
                                        <asp:ListItem Text="PRE-LAUNCH" Value="PRE-LAUNCH" />
                                        <asp:ListItem Text="PRODUCTION" Value="PRODUCTION" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ErrorMessage="Data Missing!" ControlToValidate="ddlcpType" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtcpnumber"><i class="fa fa-check-circle"></i>&nbsp;Control Plan Number</label>
                                    <asp:TextBox runat="server" ID="txtcpnumber" CssClass="form-control text-uppercase" />
                                    <asp:RequiredFieldValidator ErrorMessage="Data Missing!" ControlToValidate="txtcpnumber" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="row no-header no-border">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtrevno">Control Plan Rev.No.</label>
                                            <asp:TextBox runat="server" ID="txtrevno" CssClass="form-control" Enabled="false"   onkeypress="return isNumberKey(event);" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtrevdt">Control Plan Rev.Date</label>
                                            <asp:TextBox runat="server" ID="txtrevdt" CssClass="form-control"  Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="change nature">Nature of Change</label>
                                    <asp:TextBox runat="server" ID="txtchangenature" CssClass="form-control" TextMode="MultiLine" Rows="2"  Enabled="false" />
                                </div>

                                <div class="form-group">
                                    <label for="rev.reason">Reason for Change</label>
                                    <asp:TextBox runat="server" ID="txtrevreason" CssClass="form-control" TextMode="MultiLine" Rows="2"   Enabled="false"/>
                                </div>

                                <div class="form-group has-success">
                                    <label for="ddlCftTeamSlNo"><i class="fa fa-check-circle"></i>&nbsp;CFT Team</label>
                                    <asp:DropDownList runat="server" ID="ddlCftTeamSlNo" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtCftMembers" CssClass="form-control" TextMode="MultiLine" disabled="true" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ErrorMessage="Data Missing!" ControlToValidate="ddlCftTeamSlNo" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="txtcustomerName"><i class="fa fa-check-circle"></i>&nbsp;Customer Name </label>
                                    <%--<asp:TextBox runat="server" ID="txtcustomerName" CssClass="form-control" Text="" Style="text-transform: uppercase;" />--%>
                                    <asp:DropDownList runat="server" ID="ddlcustomername" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ErrorMessage="Data Missing!" ControlToValidate="ddlcustomername" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>

                                <div class="form-group has-success">
                                    <label for="txtcustomerPartNo"><i class="fa fa-check-circle"></i>&nbsp;Customer Part Number</label>
                                    <asp:TextBox runat="server" ID="txtcustomerPartNo" CssClass="form-control" Text="" Style="text-transform: uppercase;" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="txtcustomerPartNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>

                                <div class="form-group has-success">
                                    <label for="txtcustomerIssueNo"><i class="fa fa-check-circle"></i>&nbsp;Customer Part Issue Number</label>
                                    <asp:TextBox runat="server" ID="txtcustomerIssueNo" CssClass="form-control" Text="" Style="text-transform: uppercase;" Enabled="false" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Data Missing!" ControlToValidate="txtcustomerIssueNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtcustomerIssueDt"><i class="fa fa-check-circle"></i>&nbsp;Customer Part Issue Date</label>
                                    <asp:TextBox runat="server" ID="txtcustomerIssueDt" CssClass="form-control" TextMode="MultiLine" Rows="2" Enabled="false" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Data Missing!" ControlToValidate="txtcustomerIssueDt" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group">
                                    <label for="supplier code">Supplier Code</label>
                                    <asp:TextBox runat="server" ID="txtsuppcode" CssClass="form-control" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtKeyContact"><i class="fa fa-check-circle"></i>&nbsp;Key Contact</label>
                                    <%--<asp:TextBox runat="server" ID="txtKeyContact" CssClass="frdate form-control" Style="text-transform: uppercase;"/>--%>
                                    <asp:DropDownList runat="server" ID="ddlkeycont" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="Data Missing!" ControlToValidate="ddlkeycont" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtkeyContactPhone"><i class="fa fa-check-circle"></i>&nbsp;Key Contact Phone</label>
                                    <asp:TextBox runat="server" ID="txtkeyContactPhone" CssClass="form-control" Text="" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage="Data Missing!" ControlToValidate="txtkeyContactPhone" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group has-success">
                                    <label for="txtoriginalDt"><i class="fa fa-check-circle"></i>&nbsp;Original Date</label>
                                    <asp:TextBox runat="server" ID="txtoriginalDt" CssClass="frdate form-control" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ErrorMessage="Data Missing!" ControlToValidate="txtoriginalDt" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group">
                                    <label for="txtprocspec">Process Specification</label>
                                    <asp:TextBox runat="server" ID="txtprocspec" CssClass="form-control" />
                                </div>
                                <did class="form-group">
                                    <label for="txtihref">IH Metallurgy Ref.</label>
                                    <asp:TextBox runat="server" ID="txtihref" CssClass="form-control" />
                                </did>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-primary">
                            <div class="box-body">

                                <div class="form-group has-success">
                                    <label for="txtoriginalDt"><i class="fa fa-check-circle"></i>&nbsp;Organization</label>
                                    <asp:TextBox runat="server" ID="txtorganization" CssClass="form-control" Text="MEIL - M. City" Style="text-transform: uppercase;" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ErrorMessage="Data Missing!" ControlToValidate="txtoriginalDt" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group">
                                    <label for="txtorgApprovalDt">Plant Approval / Date </label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtorgApprovalDt" CssClass="form-control" Text="" Style="text-transform: uppercase;" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtorgDate" CssClass="frdate form-control" />
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="form-group">
                                    <label for="txtorgDate">Customer Engg. Approval Reqd. & Date</label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtcustapproval" CssClass="form-control" Text="" Style="text-transform: uppercase;" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtcustapprovaldt" CssClass="frdate form-control" />
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="form-group">
                                    <label for="txtcustQaApproval">Customer QA Approval Reqd. & Date</label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtcustQaApproval" CssClass="form-control" Text="" Style="text-transform: uppercase;" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtcustQaApprovalDt" CssClass="frdate form-control" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="form-group">
                                    <label for="txtotherApproval">Other Approval Reqd. & Date</label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtotherApproval" CssClass="form-control" Text="" Style="text-transform: uppercase;" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtotherApprovalDt" CssClass="frdate form-control" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="form-group">
                                    <label for="txtpreparedby">Prepared By</label>
                                    <asp:DropDownList runat="server" ID="ddlPrepd" CssClass="form-control">
                                        <asp:ListItem Text="Select..." />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ErrorMessage="Data Missing!" ControlToValidate="ddlPrepd" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group">
                                    <label for="txtapprovedby">Approved By</label>
                                    <asp:DropDownList runat="server" ID="ddlAppd" CssClass="form-control">
                                        <asp:ListItem Text="Select..." />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ErrorMessage="Data Missing!" ControlToValidate="ddlAppd" runat="server" InitialValue="Select..."
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                </div>
                                <div class="form-group hidden">
                                    <label for="uploadfile1">Upload Files</label>
                                    <table class="table table-condensed">
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="uploadfile1" runat="server"></asp:FileUpload>

                                                <a id="hrefrcFile1" href="#" runat="server">
                                                    <asp:Label ID="lbluploadfile1" runat="server"></asp:Label></a>

                                            </td>

                                            <td>
                                                <a href="#" id="deluploadfile1" runat="server" onclick="ConfirmDelete()" onserverclick="delUploadFile1_Click"><i class="fa fa-trash-o"></i></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="uploadfile2" runat="server"></asp:FileUpload>
                                                <a id="hrefrcFile2" href="#" runat="server">
                                                    <asp:Label ID="lbluploadfile2" runat="server"></asp:Label></a>
                                            </td>

                                            <td>
                                                <a href="#" id="deluploadfile2" runat="server" onclick="ConfirmDelete()" onserverclick="delUploadFile2_Click"><i class="fa fa-trash-o"></i></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="uploadfile3" runat="server"></asp:FileUpload>
                                                <a id="hrefrcFile3" href="#" runat="server">
                                                    <asp:Label ID="lbluploadfile3" runat="server"></asp:Label></a>
                                            </td>
                                            <td>
                                                <a href="#" id="deluploadfile3" runat="server" onclick="ConfirmDelete()" onserverclick="delUploadFile3_Click"><i class="fa fa-trash-o"></i></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="uploadfile4" runat="server"></asp:FileUpload>
                                                <a id="hrefrcFile4" href="#" runat="server">
                                                    <asp:Label ID="lbluploadfile4" runat="server"></asp:Label></a>
                                            </td>
                                            <td>
                                                <a href="#" id="deluploadfile4" runat="server" onclick="ConfirmDelete()" onserverclick="delUploadFile4_Click"><i class="fa fa-trash-o"></i></a>
                                            </td>
                                        </tr>
                                    </table>
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

                                <div class="form-group">
                                    <label for="txtPartNo">Remarks</label>
                                    <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" CssClass="form-control" Text="" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <asp:HiddenField ID="hdnSlNo" runat="server" />
        <asp:HiddenField ID="hdnNewSl" runat="server" Value="0" />
        <asp:HiddenField ID="hdnrevclick" runat="server" Value="" />
        <asp:HiddenField ID="hdnMode" runat="server" Value="I" />
         <asp:HiddenField ID="hdnrevno" runat="server"  />
         <asp:HiddenField ID="hdndcrslno" runat="server"  />
       

    </form>
    <script type="text/javascript" src="plugins/jQueryUI/jquery-ui.min.js"></script>

    <script type="text/javascript">
        var data = [];
        $("document").ready(function () {
            <%--$('#<%=txtpartIssueDt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });

            $('#<%=txtcustomerIssueDt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });--%>


            $('#<%=txtoriginalDt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });

            $('#<%=txtorgDate.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,

            });

            $('#<%=  txtcustapprovaldt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });

            $('#<%=  txtcustQaApprovalDt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });

            $('#<%=  txtotherApprovalDt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });

            $('#<%=txtrevdt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                changeMonth: true,
                changeYear: true,
                autoclose: true,
            });

            $('#<%=ddlCftTeamSlNo.ClientID%>').change(function () {
                var sl = $('#<%=ddlCftTeamSlNo.ClientID%>').val();
                if (sl != "Select...") {
                    $.ajax({
                        type: 'POST',
                        url: 'data.asmx/GetCFTMembers',
                        data: "{'slno' : '" + sl + "'}",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            $('#<%=txtCftMembers.ClientID%>').val(data.d);
                        }
                    })
                }
                else {
                    $('#<%=txtCftMembers.ClientID%>').val("");
                }

            });



            // END-OF-DOCUMENT READY
            $('#<%=txtcpnumber.ClientID%>').on('change', function () {
                var cp = $('#<%=txtcpnumber.ClientID%>').val();
                var mode = $('#<%=hdnMode.ClientID%>').val();
                if (mode === "I") {
                    $.ajax({
                        url: 'data.asmx/CheckCPNumberExists',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        type: 'POST',
                        data: JSON.stringify({ cpnumber: cp }),
                        success: function (data) {
                            if (data.hasOwnProperty('d')) {
                                var result = JSON.parse(data.d).result;
                                if (result === "exists") {
                                    alert("CP Number already exists");
                                    $('#<%=btnSubmit.ClientID%>').attr('disabled', 'disabled');
                                } else {
                                    $('#<%=btnSubmit.ClientID%>').removeAttr('disabled');
                                }
                            } else {
                                console.error('Invalid response format');
                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert("Error: " + err.Message);
                        }
                    });
                }
            });

        });




        $('#<%=txtPartNo.ClientID%>').on('change', function () {
            var partno = $('#<%=txtPartNo.ClientID%>').val();
            var requestData = { 'mstpartno': partno };
            console.log(requestData);

            $.ajax({
                type: 'POST',
                url: 'data.asmx/CheckPartNoExists',
                data: JSON.stringify(requestData), // Convert data to JSON format
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    console.log(data); // Output the data to the console for inspection
                    var cnt = parseInt(data.d);
                    if (!isNaN(cnt)) {
                        if (cnt > 0) {
                            /*alert("Part No. already exists! Query to retrieve part and then click Save & Revise button.");*/
                            ShowMessage("Part No. already exists! Query to retrieve part and then click Save & Revise button.", "Info");
                            $('#<%=btnSubmit.ClientID%>').attr("disabled", "disabled");
                }
                else {
                    $('#<%=btnSubmit.ClientID%>').removeAttr('disabled');
                }

            } else {
                console.log("Error parsing count:", data.d);
            }
        },
        error: function (xhr, status, error) {
            // Display an alert with the error message
            alert("Error: " + error);
        }
    });
        });
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            // Allow only digits (0–9)
            if (charCode < 48 || charCode > 57)
                return false;
            return true;
        }

    </script>

</asp:Content>


