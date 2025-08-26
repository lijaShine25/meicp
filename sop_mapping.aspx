<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sop_mapping.aspx.cs" Inherits="sop_mapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/handsontable/handsontable.full.min.js"></script>
    <script src="Scripts/hotdata.js"></script>
    <script src="Scripts/prop.js"></script>
    <script type="text/javascript" src="Scripts/ajax.js"></script>

    <style>
        #modalQaApproval .modal-dialog {
            width: 75%;
        }

        .datepicker {
            z-index: 1151 !important;
        }

        .auto-style1 {
            width: 157px;
        }
    </style>
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>SOP  Mapping
                </h1>
            </section>
            <section class="content">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="box no-border no-margin">
                            <div class="messagealert" id="alert_container">
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-9">
                        <div class="box">
                            <div class="box-header">
                                <div class="col-sm-8 col-md-offset-4">

                                    <div class="pull-right">
                                        <asp:Button Text="Revise" runat="server" CssClass="btn btn-warning" ID="btnirev" Enabled="true" />
                                        <asp:Button Text="Approve" runat="server" CssClass="btn btn-warning" ID="btnApprove" Enabled="true" OnClick="btnApprove_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-warning" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                                        <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="btn btn-danger" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnReport" Text="Report" runat="server" CssClass="btn btn-primary" OnClick="btnReport_Click" />
                                        <%--<asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-primary" OnClick="btnQuery_Click" />--%>
                                    </div>
                                </div>
                            </div>

                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtGroupName"><i class="fa fa-check-circle"></i>&nbsp;Group </label>
                                            <asp:TextBox runat="server" ID="txtGroupName" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtGroupName" ErrorMessage="Data Missing!" ControlToValidate="txtGroupName" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtTemplate"><i class="fa fa-check-circle"></i>&nbsp;Template </label>
                                            <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Select...">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 1" Text="Template 1">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 2" Text="Template 2">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 3" Text="Template 3">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 4" Text="Template 4">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 5" Text="Template 5">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 6" Text="Template 6">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Template 7" Text="Template 7">
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlTemplate" ErrorMessage="Data Missing!" ControlToValidate="ddlTemplate" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="0" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtFormatNo"><i class="fa fa-check-circle"></i>&nbsp;Document No.</label>
                                            <asp:TextBox runat="server" ID="txtFormatNo" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtFormatNo" ErrorMessage="Data Missing!" ControlToValidate="txtFormatNo" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtqualityChar"><i class="fa fa-check-circle"></i>&nbsp;Product Characteristics</label>
                                            <asp:TextBox runat="server" ID="txtqualityChar" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtqualityChar" ErrorMessage="Data Missing!" ControlToValidate="txtqualityChar" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="ddlPreparedBy"><i class="fa fa-check-circle"></i>&nbsp;Prepared By</label>
                                            <asp:DropDownList runat="server" ID="ddlPreparedBy" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Select.."></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlPrepared" ErrorMessage="Data Missing!" ControlToValidate="ddlPreparedBy" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="0" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="ddlApprovedBy"><i class="fa fa-check-circle"></i>&nbsp;Approved By</label>
                                            <asp:DropDownList runat="server" ID="ddlApprovedBy" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Select.."></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlApprovedBy" ErrorMessage="Data Missing!" ControlToValidate="ddlApprovedBy" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="0" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="ddlActive"><i class="fa fa-check-circle"></i>&nbsp;Active / Inactive</label>
                                            <asp:DropDownList runat="server" ID="ddlActive" CssClass="form-control">
                                                <asp:ListItem Value="Active" Text="Active" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="In Active" Text="In Active"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlActive" ErrorMessage="Data Missing!" ControlToValidate="ddlActive" runat="server"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Rev. No: </label>
                                            <asp:TextBox runat="server" ID="txtRevNo" CssClass="form-control" Enabled="false"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">


                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Rev. Date: </label>
                                            <asp:TextBox runat="server" ID="txtrevdt" CssClass="form-control" Enabled="false"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Rev Reason: </label>
                                            <asp:TextBox runat="server" ID="txtreasonforchange" CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="3"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Nature of Change: </label>
                                            <asp:TextBox runat="server" ID="txtchangenature" CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="3"></asp:TextBox>

                                        </div>
                                    </div>


                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-body">
                                                <div style="overflow: auto;">
                                                    <div id="tableMap" class="handsontable"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="box">
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static"
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:BoundField HeaderText="Group Id" DataField="Group_id" />
                                        <asp:HyperLinkField DataNavigateUrlFields="Group_Id" HeaderText="Group Name"
                                            DataNavigateUrlFormatString="~/sop_mapping.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="Group_Name" />


                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>




            <div class="modal fade" id="modalQaApproval" tabindex="-1" role="dialog" aria-labelledby="modalQaApproval"
                aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                &times;</button>
                            <h4 class="modal-title" id="H2">Initiate Revision</h4>
                            <br />
                        </div>

                        <div class="modal-body">
                            <div class="row">
                                <!-- Left Column -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtdcrnumber">DCR Number</label>
                                        <asp:TextBox ID="txtdcrnumber" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtdcrReqDate">DCR Request Date</label>
                                        <asp:TextBox runat="server" ID="txtdcrReqDate" CssClass="form-control" Enabled="false" />
                                    </div>
                                    <div class="form-group">
                                        <label for="tctdcrReqBy">DCR Requested By</label>
                                        <asp:TextBox runat="server" ID="tctdcrReqBy" CssClass="form-control" Enabled="false" />
                                    </div>
                                    <div class="form-group">
                                        <label for="txtchanges">Changes to be Made :</label>
                                        <asp:TextBox ID="txtchanges" runat="server" CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtExisting">Existing</label>
                                        <asp:TextBox runat="server" ID="txtExisting" CssClass="form-control" TextMode="MultiLine" Rows="4" Enabled="false" />
                                    </div>
                                    <div class="form-group">
                                        <label for="txtchangereq">Changes Required</label>
                                        <asp:TextBox runat="server" ID="txtchangereq" CssClass="form-control" TextMode="MultiLine" Rows="4" Enabled="false" />
                                    </div>
                                </div>
                                <!-- Right Column -->
                                <div class="col-md-6">

                                    <div class="form-group">
                                        <label for="txtreason">Reason for change</label>
                                        <asp:TextBox ID="txtreason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtreason">Nature of change</label>
                                        <asp:TextBox ID="txtnatureofchange" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtUserRevNo"><i class="fa fa-check-circle"></i>&nbsp;Rev.No.</label>
                                        <asp:TextBox runat="server" ID="txtUserRevNo" CssClass="form-control" ReadOnly="true" />
                                        <asp:RequiredFieldValidator ID="rfvtxtUserRevNo" ErrorMessage="Data Missing!" ControlToValidate="txtUserRevNo" runat="server"
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="revmandates" />
                                    </div>
                                    <div class="form-group">
                                        <label for="txtUserRevDt"><i class="fa fa-check-circle"></i>&nbsp;Rev.Date</label>
                                        <asp:TextBox runat="server" ID="txtUserRevDt" CssClass="form-control" ReadOnly="true" />
                                        <%--  <asp:RequiredFieldValidator ID="rfvtxtUserRevDt" ErrorMessage="Data Missing!" ControlToValidate="txtUserRevDt" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="revmandates" />--%>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtRevReason"><i class="fa fa-check-circle"></i>&nbsp;Rev.Reason</label>
                                        <asp:TextBox ID="txtrevreason" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtrevreason" ErrorMessage="Data Missing!" ControlToValidate="txtrevreason" runat="server"
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="revmandates" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="btnrevision" runat="server" Text="Revision" CssClass="btn btn-flat btn-info" OnClick="btnrevision_Click" ValidationGroup="revmandates" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                Close</button>
                        </div>
                    </div>
                </div>
            </div>





        </div>
        <asp:HiddenField ID="hdnEditMode" runat="server" Value="I" />
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />
        <asp:HiddenField ID="hdnpartsl" runat="server" />
        <asp:HiddenField ID="hdnopslno" runat="server" />
        <asp:HiddenField ID="hdnchild" runat="server" />
        <asp:HiddenField ID="hdngroupid" runat="server" />
        <asp:HiddenField ID="hdnapprovestatus" runat="server" Value="A" />
        <asp:HiddenField ID="hdnreqby" runat="server" />
        <asp:HiddenField ID="hdndcr_slno" runat="server" />
        <asp:HiddenField ID="hdnreas" runat="server" />
        <asp:HiddenField ID="hdnnature" runat="server" />
        <asp:HiddenField ID="hdnrevreason" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnrevnumber" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnrevdt" runat="server" Value=""></asp:HiddenField>

    </form>
    <script type="text/javascript">
        var $container = $("#tableMap");
        function confirmation() {
            return confirm("Are you sure you want to delete this item?");
        }
        $("document").ready(function () {

            var part = [];
            var op = [];
            var mch = []; var mchslno = [];
            var s = null;
            part = GetAjaxArray('', 'GetPartsMapping', 'mstPartNo');
            op = GetAjaxArray("{'partslno':" + s + "}", 'GetOperationsMapping', 'OperationDesc');

            console.log(part);

            $container.handsontable({
                startRows: 3,
                startCols: 6,
                minRows: 4,
                minCols: 6,
                height: 400,
                rowHeaders: true,
                minSpareRows: 1,
                minSpareCols: 7,
                enterMoves: { row: 0, col: 1 },
                autoWrapRow: true,

                contextMenu: false,
                outsideClickDeselects: false,
                removeRowPlugin: true,
                currentRowClassName: 'currentRow',
                colWidths: [180, 180, 180, 0.1, 0.1, 0.1],
                colHeaders: ["Part Number", "Operation", "Machine"],
                columns: [
                    {
                        data: "mstPartNo", type: 'dropdown', source: part, filter: true

                    },
                    {
                        data: "OperationDesc", type: 'dropdown', strict: true, filter: true, source: op
                    },

                    { data: "MachineDesc", type: 'text' },
                    { data: 'part_slno', type: 'text' },
                    { data: 'operation_slno', type: 'text' },
                    { data: 'machine_slno', type: 'text' }
                    /* { data: 'Format_No', type: 'text' }*/
                ]
                ,
                //hiddenColumns: {
                //    columns: [3, 4, 5], // Hide columns at indexes 3, 4, and 5
                //    indicators:false // Show indicators in the headers
                //},
                contextMenu: ['row_above', 'row_below', 'remove_row'],
                removeRowPlugin: true
            });


            var ht = $container.data('handsontable');

            ht.addHook('afterChange', function (changes, source) {
                if (source === 'loadData' || source === 'internal' || changes.length > 1) {
                    return;
                }
                var row = changes[0][0];
                var prop = changes[0][1];
                var value = changes[0][3];

                if (prop === 'mstPartNo') {
                    ht.setDataAtCell(row, 1, '');
                    var slno;

                    $.ajax({
                        url: "Data.asmx/Getpartslno",
                        dataType: 'json',
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        data: "{'partname' : '" + value + "'}",
                        success: function (res) {
                            var gdata = JSON.parse(res.d);
                            if (gdata.length > 0) {
                                slno = gdata[0].part_slno;
                                ht.setDataAtCell(row, 3, slno);
                            }
                        }
                    }).done(function () {
                        if (slno != null && slno != "") {
                            op = GetAjaxArray("{'partslno':" + slno + "}", 'GetOperationsMapping', 'OperationDesc');
                            if (op == null || op == "") {
                                alert("No operation mapped for selected part");

                            }
                        }
                        ht.updateSettings({
                            colWidths: [180, 180, 180, 0.1, 0.1, 0.1],
                            colHeaders: ["Part Number", "Operation", "Machine"],
                            columns: [
                                {
                                    data: "mstPartNo", type: 'dropdown', source: part, filter: true


                                },
                                {
                                    data: "OperationDesc", type: 'dropdown', strict: true, filter: true, source: op
                                },

                                { data: "MachineDesc", type: 'text' },
                                { data: 'part_slno', type: 'text' },
                                { data: 'operation_slno', type: 'text' },
                                { data: 'machine_slno', type: 'text' }
                                /*  { data: 'Format_No', type: 'text' }*/
                            ]
                        })

                        $('#<%=hdnpartsl.ClientID %>').val(slno);
                    });
                }

                else if (prop == 'OperationDesc') {



                    ht.setDataAtCell(row, 2, '');
                    var slno;
                    var pslno;
                    $.ajax({
                        url: "Data.asmx/Getoperationslno",
                        dataType: 'json',
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        data: "{'opname' : '" + value + "'}",
                        success: function (res) {
                            var gdata = JSON.parse(res.d);
                            if (gdata.length > 0) {
                                slno = gdata[0].operation_slno;
                                pslno = $('#<%=hdnpartsl.ClientID %>').val();
                                ht.setDataAtCell(row, 4, slno);
                                //console.log(slno + "-------" + pslno);
                            }
                        }
                    }).done(function () {
                        //alert(slno);
                        //alert(pslno);

                        if (slno != null && slno != "" && pslno != null && pslno != "") {
                            mch = GetAjaxArray("{'opslno':" + slno + ", 'pslno':" + pslno + "}", 'GetMachine', 'MachineDesc');
                            ht.setDataAtCell(row, 2, mch[0]);
                            console.log("machine-----" + mch);
                            mchslno = GetAjaxArray("{'opslno':" + slno + ", 'pslno':" + pslno + "}", 'GetMachine', 'machine_slno');
                            console.log("machine-----" + mchslno);
                            ht.setDataAtCell(row, 5, mchslno[0]);
                            if (mchslno == null || mchslno == "") {
                                alert("No Machines mapped for selected part & operation");

                            }
                        }
                        ht.updateSettings({
                            colWidths: [180, 180, 180, 0.1, 0.1, 0.1],
                            colHeaders: ["Part Number", "Operation", "Machine"],
                            columns: [
                                {
                                    data: "mstPartNo", type: 'dropdown', source: part, filter: true

                                },
                                {
                                    data: "OperationDesc", type: 'dropdown', strict: true, filter: true, source: op
                                },

                                { data: "MachineDesc", type: 'text' },
                                { data: 'part_slno', type: 'text' },
                                { data: 'operation_slno', type: 'text' },
                                { data: 'machine_slno', type: 'text' }
                                /* { data: 'Format_No', type: 'text' }*/
                            ]
                        })

                        $('#<%=hdnopslno.ClientID %>').val(slno);
                    });
                }
            });
            $('#<%=btnSave.ClientID%>, #<%=btnSubmit.ClientID%>').on('click', function (e) {

                var hotobj = $container.data('handsontable');
                var dt = hotobj.getSourceData();
                var ret = JSON.stringify(dt);
                var dd = removeAllBlankOrNull(dt);
                var gridData = hotobj.getData();
                var hotGridData = gridData;
                var hotArray = [];
                $.each(hotGridData, function (x, y) {
                    if (hotGridData[x][3] != null) {
                        var obj = new Object();
                        /*   obj.Format_No = hotGridData[x][3];*/
                        obj.part_slno = hotGridData[x][3];
                        obj.operation_slno = hotGridData[x][4];
                        obj.machine_slno = hotGridData[x][5];
                        obj.del_status = 'N';


                        hotArray.push(obj);
                    }
                });
                if (hotArray.length == 0) {
                    alert("Please add at least one row of data before saving.");
                    return;
                }
                else {
                    var hotresult = JSON.stringify(hotArray);
                    $('#<%=hdnchild.ClientID %>').val(hotresult);
                }
            });

            loadData();

            function loadData() {
                var slno = $('#<%=hdnSlNo.ClientID %>').val();
                if (slno > 0) {
                    var dArray = [[]];
                    var manhot = $container.data('handsontable');
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Data.asmx/GetSOPMappingChildData",
                        data: "{'slno' : " + slno + "}",
                        dataType: "json",
                        success: function (res) {
                            var data = JSON.parse(res.d);
                            console.log(res.d);

                            $(data).each(function (i, j) {
                                for (var i = 0; i < data.length; i++) {
                                    dArray[i] = new Array();
                                    dArray[i][0] = data[i].mstPartNo;
                                    dArray[i][1] = data[i].OperationDesc;
                                    dArray[i][2] = data[i].MachineDesc;
                                    /* dArray[i][3] = data[i].Format_No;*/
                                    dArray[i][3] = data[i].part_slno;
                                    dArray[i][4] = data[i].operation_slno;
                                    dArray[i][5] = data[i].machine_slno;


                                }
                            });
                        },
                        error: function (jqxhr, status, error) {
                            var msg = JSON.parse(jqxhr.responseText);
                            errormsg += JSON.stringify(msg);
                            alert(errormsg);
                        }
                    }).done(function () {
                        if (typeof dArray !== 'undefined' && dArray.length > 0) { manhot.populateFromArray(0, 0, dArray); }
                      //  $('#<%=btnSave.ClientID %>').prop('disabled', false);
                        $('#<%=btnDelete.ClientID %>').prop('disabled', false);
                    });
                }
            }






            $('#<%=btnirev.ClientID %>').on('click', function (e) {
                e.preventDefault();
                var slno = $('#<%=hdnSlNo.ClientID %>').val();
                var ss = $('#<%=hdnapprovestatus.ClientID %>').val();




                if (slno > 0 && ss == 'A') {


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Data.asmx/GetDCRSOPData",
                        data: "{'Group_Id' : " + slno + "}",
                        dataType: "json",
                        success: function (res) {
                            var data = JSON.parse(res.d);
                            console.log(data.length);

                            if (data.length > 0) {
                                $('#modalQaApproval').modal('show');;
                                var firstRecord = data[0];
                                $('#<%=txtdcrReqDate.ClientID %>').val(firstRecord.Request_Date);
                                $('#<%=tctdcrReqBy.ClientID %>').val(firstRecord.EmployeeName);
                                $('#<%=txtchanges.ClientID %>').val(firstRecord.changes);
                                $('#<%=txtExisting.ClientID %>').val(firstRecord.Existing);
                                $('#<%=txtreason.ClientID %>').val(firstRecord.Reason_For_Change);
                                $('#<%=txtchangereq.ClientID %>').val(firstRecord.Changes_Required);
                                $('#<%=txtnatureofchange.ClientID %>').val(firstRecord.nature_of_change);
                                $('#<%=hdnreqby.ClientID %>').val(firstRecord.Request_By);
                                $('#<%=txtdcrnumber.ClientID %>').val(firstRecord.dcr_number);
                                $('#<%=hdndcr_slno.ClientID %>').val(firstRecord.dcr_slno);
                                $('#<%=hdnreas.ClientID %>').val(firstRecord.Reason_For_Change);
                                $('#<%=hdnnature.ClientID %>').val(firstRecord.nature_of_change);
                                $('#<%=txtrevreason.ClientID %>').val(firstRecord.Reason_For_Change);
                                console.log("success");

                            }
                            else {
                                console.log("no data");
                                alert('Please raise a request through DCR');
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error(xhr.responseText);
                            console.log("error");
                        }
                    });


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Data.asmx/GetSOPMAPPINGRevNum",
                        data: "{'Group_Id' : " + slno + "}",
                        dataType: "json",
                        success: function (res) {
                            var revNumber = res.d;

                            $('#<%=txtUserRevNo.ClientID %>').val(revNumber);
                            var currentDate = new Date();
                            var formattedDate = (currentDate.getDate()).toString().padStart(2, '0') + '/' +
                                ((currentDate.getMonth() + 1)).toString().padStart(2, '0') + '/' +
                                currentDate.getFullYear();
                            $('#<%=txtUserRevDt.ClientID %>').val(formattedDate);

                            console.log("success");


                        },
                        error: function (xhr, status, error) {
                            console.error(xhr.responseText);
                            console.log("error");
                        }
                    });


                } else {

                    alert('Revision cannot be initiated. Either no DCR or the current SOP is not approved');
                }
            });



        });




    </script>
</asp:Content>

