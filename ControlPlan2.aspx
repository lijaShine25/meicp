<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ControlPlan2.aspx.cs" Inherits="ControlPlan2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


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
                <h1>Control Plan
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
                            <asp:Button Text="Initiate Revision" runat="server" CssClass="btn btn-warning" ID="btnirev" Enabled="true" />
                            <asp:Button ID="btnApproved" Text="Approve" runat="server" CssClass="btn btn-success" Enabled="false" OnClick="btnApproved_Click" />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <a href="guidelinesnew.pdf" target="_blank">Guidelines</a>
                        </div>
                        <div class="pull-right">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSave_Click" />
                            <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" PostBackUrl="~/controlplan_qry.aspx" />
                            <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="btn btn-danger" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
                            <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                            <asp:Button ID="btnSubmit" Text="SUBMIT" runat="server" CssClass="btn btn-warning" Enabled="false" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>


                <p></p>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box no-border no-header">
                            <div class="box-body">
                                <div class="col-md-3">
                                    <div class="form-group has-success">
                                        <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part# </label>
                                        <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlpart_slno_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group has-success">
                                        <label for="txtCPType"><i class="fa fa-check-circle"></i>&nbsp;Control Plan Type </label>
                                        <asp:DropDownList runat="server" ID="ddlcpType" CssClass="form-control">
                                            <asp:ListItem Text="Select..." />
                                            <asp:ListItem Text="PROTOTYPE" Value="PROTOTYPE" />
                                            <asp:ListItem Text="PRE-LAUNCH" Value="PRE-LAUNCH" />
                                            <asp:ListItem Text="PRODUCTION" Value="PRODUCTION" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="ddlcpType" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group has-success">
                                        <label for="txtOperation"><i class="fa fa-check-circle"></i>&nbsp;Operation</label>
                                        <asp:DropDownList runat="server" ID="ddloperation_slno" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddloperation_slno_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="ddloperation_slno" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group has-success">
                                        <label for="txtMachineType"><i class="fa fa-check-circle"></i>&nbsp;Machine</label>
                                        <asp:DropDownList runat="server" ID="ddlmachine_slno" CssClass="form-control">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Data Missing!" ControlToValidate="ddlmachine_slno" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div style="overflow: auto;">
                                    <div id="cp" class="handsontable"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <asp:HiddenField ID="hdnEditMode" runat="server" Value="I" />
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />
        <asp:HiddenField ID="hdnchild" runat="server" />
        <asp:HiddenField ID="hdnemplslno" runat="server" />
        <asp:HiddenField ID="hdnsubmitstatus" runat="server" Value="N" />
        <asp:HiddenField ID="hdnCustSl" runat="server" Value="" />
        <asp:HiddenField ID="hdnrevno" runat="server" Value="" />
        <asp:HiddenField ID="hdnnature" runat="server" Value="" />
        <asp:HiddenField ID="hdnreas" runat="server" Value="" />
        <asp:HiddenField ID="hdnreqby" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdndcr_slno" runat="server" Value=""></asp:HiddenField>
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
                                    <asp:TextBox runat="server" ID="txtdcrReqDate" CssClass="form-control" Enabled="false"/>
                                </div>
                                <div class="form-group">
                                    <label for="tctdcrReqBy">DCR Requested By</label>
                                    <asp:TextBox runat="server" ID="tctdcrReqBy" CssClass="form-control" Enabled ="false" />
                                </div>
                                <div class="form-group">
                                    <label for="txtchanges">Changes to be Made :</label>
                                    <asp:TextBox ID="txtchanges" runat="server" CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txtExisting">Existing</label>
                                    <asp:TextBox runat="server" ID="txtExisting" CssClass="form-control" TextMode="MultiLine" Rows="4" Enabled="false"/>
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
                                    <asp:TextBox ID="txtreason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"  ReadOnly="true" ></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txtreason">Nature of change</label>
                                    <asp:TextBox ID="txtnatureofchange" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"   ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txtUserRevNo"><i class="fa fa-check-circle"></i>&nbsp;Rev.No.</label>
                                    <asp:TextBox runat="server" ID="txtUserRevNo" CssClass="form-control"  ReadOnly="true"/>
                                    <asp:RequiredFieldValidator ID="rfvtxtUserRevNo" ErrorMessage="Data Missing!" ControlToValidate="txtUserRevNo" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="revmandates" />
                                </div>
                                <div class="form-group">
                                    <label for="txtUserRevDt"><i class="fa fa-check-circle"></i>&nbsp;Rev.Date</label>
                                    <asp:TextBox runat="server" ID="txtUserRevDt" CssClass="form-control"   ReadOnly="true"/>
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
        <asp:HiddenField ID="hdnCpType" runat="server" Value="" />
        <asp:HiddenField ID="hdnProcessNo" runat="server" Value="" />
        <asp:HiddenField ID="hdnMachine" runat="server" Value="" />
        <asp:HiddenField ID="hdnuser" runat="server" Value="" />
        <asp:HiddenField ID="hdnAppd" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnPrep" runat="server" Value=""></asp:HiddenField>

        <asp:HiddenField ID="hdnuserrevno" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnuserrevdate" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnrevreason" runat="server" Value=""></asp:HiddenField>

         <asp:HiddenField ID="hdnchangenature" runat="server" Value=""></asp:HiddenField>


        <asp:HiddenField ID="hdnrevnumber" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnrevdt" runat="server" Value=""></asp:HiddenField>

        <asp:HiddenField ID="hdnadmin" runat="server" Value="N" />
    </form>


    <%--<script type="text/javascript" src="Scripts/ajax.js"></script>--%>
    <%--<link href="Content/handsontable/handsontable.full.min.css" rel="stylesheet" />
<script src="Scripts/handsontable/handsontable.full.min.js"></script>--%>
    <link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/handsontable/handsontable.full.min.js"></script>
    <script src="Scripts/hotdata.js"></script>
    <script src="Scripts/prop.js"></script>
    <script type="text/javascript" src="Scripts/ajax.js"></script>


    <script type="text/javascript">

        var $container = $("#cp");
        $("document").ready(function () {
            var data = [];
            $('.left-side').toggleClass("collapse-left");
            $(".right-side").toggleClass("strech");
            $('#<%=txtUserRevDt.ClientID %>').datepicker({
                format: 'dd/mm/yyyy',
                showOtherMonths: true,
                showStatus: true,
                changeMonth: true,
                changeYear: true,
                showWeek: true,
                autoclose: true,
                firstDay: 1,
                defaultDate: "+1w",
                numberOfMonths: 1
            });



            //  var data = [];

            var pseudoHeaderRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                Handsontable.TextCell.renderer.apply(this, arguments);
                // Handsontable.renderers.TextRenderer.apply(this, arguments);

                var style = td.style;
                style.textAlign = 'center';
                style.fontStyle = 'normal';
                style.fontSize = '8';
                style.color = '#000';
                style.background = '#eee';
                style.fontWeight = 'normal';

                if (row == 0) {
                    switch (col) {
                        case 0: td.innerHTML = "Characteristics"; break;
                        case 3: td.innerHTML = "Spl.Char.Class"; break;
                        case 4: td.innerHTML = "Methods"; break;
                        case 10: td.innerHTML = "RES"; break;
                        case 11: td.innerHTML = "Control Methods"; break;
                        case 12: td.innerHTML = "Reaction Plan"; break;

                    }
                } else if (row == 1) {
                    switch (col) {
                        case 0: td.innerHTML = "Dim.No."; break;
                        case 1: td.innerHTML = "Product"; break;
                        case 2: td.innerHTML = "Process"; break;
                        case 4: td.innerHTML = "Product / Process Specification Tolerance"; break;
                        case 6: td.innerHTML = "Evaluation Measurement Technique"; break;
                        case 7: td.innerHTML = "Gauge/ Fixture Code"; break;
                        case 8: td.innerHTML = "Sample"; break;
                    }
                } else if (row == 2) {
                    switch (col) {
                        case 4: td.innerHTML = "Min"; break;
                        case 5: td.innerHTML = "Max"; break;
                        case 8: td.innerHTML = "Size"; break;
                        case 9: td.innerHTML = "Freq"; break;
                    }
                }
                return td;
            }
            var custsl = $('#<%=hdnCustSl.ClientID%>').val();
            var splchar = [];
            if (custsl == 0) {
                splchar = GetAjaxArray("", 'GetSpecialChars', 'splCharFile');
            }
            else {
                splchar = GetAjaxArray("{'custsl':'" + custsl + "'}", 'GetSpecialChars2', 'splCharFile');
            }

            //alert(splchar);
            var freq = GetAjaxArray('', 'GetFrequencies', 'FreqDesc');
            var cm = GetAjaxArray('', 'GetControlmethods', 'methodDesc');
            var gage = GetAjaxArray('', 'GetGauges', 'md_id');
            var evalarray = GetAjaxArray('', 'GetEvalTech', 'evalTech');

            $container.handsontable({
                startRows: 5, //number of rows to be displayed at initialization
                startCols: 18, // number of columns to be displayed
                minRows: 4,
                minCols: 5,
                //data: data,
                height: 400,
                rowHeaders: false, //show row headers
                minSpareRows: 5, //minimum last row
                minSpareCols: 0,
                enterMoves: { row: 0, col: 1 },
                autoWrapRow: true,

                contextMenu: false,
                outsideClickDeselects: false, //deselect if clicked outside
                removeRowPlugin: true, //remove row other than from context menu
                currentRowClassName: 'currentRow',  //css class for selected row
                //colWidths: [50, 150, 150,
                //    100, 200,
                //    150, 120, 150,
                //    150, 120, 150,
                //    200
                //],
                colHeaders: ["No.", "Product Char.", "Process Char.",
                    "Spl.Char.<br/>Class", "Product/ Process<br/>Specification/ Tolerance",
                    "Evaluation Meas.<br/>Technique", "Sample Size<br/>(Nos.)", "Sample Freq.", "Control Method<br/>(First Off Inspection)",
                    "Evaluation Meas.<br/>Technique", "Sample Size<br/>(Nos.)", "Sample Freq.", "Control Method<br/>(In-Process)",
                    "Reaction Plan", "Min. Spec", "Max. Spec", "Tolerance", "PDI Type"], //names of columnheaders
                columns: [
                    { data: "dimn_no" },
                    { data: "product_char" },
                    { data: "process_char" },
                    { data: "splChar_slno", type: 'dropdown', source: splchar, allowEmpty: true, strict: true, filter: true },
                    { data: "spec1" },
                    { data: "evalTech", type: 'dropdown', source: evalarray, strict: true, filter: true },
                    { data: "sampleSize" },
                    { data: "FreqDesc", type: 'dropdown', source: freq, strict: true, filter: true },
                    { data: "methodDesc", type: 'dropdown', source: cm, strict: true, filter: true },
                    { data: "evalTech2", type: 'dropdown', source: evalarray, strict: true, filter: true },
                    { data: "sampleSize2" },
                    { data: "FreqDesc2", type: 'dropdown', source: freq, strict: true, filter: true },
                    { data: "methodDesc2", type: 'dropdown', source: cm, strict: true, filter: true },
                    { data: "reactionPlan" },
                    { data: "min_spec", type: 'numeric' },
                    { data: "max_spec", type: 'numeric' },
                    { data: "tolerance", type: 'numeric' },
                    { data: "PDI_type", type: 'dropdown', source: ['Variable', 'Attribute'] }


                ], //column mapping to the respecitve data fields
                afterSelectionEnd: function (r, c, r1, c1) {
                    row = r;
                    col = c;
                }, // this function is called when a row is selected it can have multiple rows and columns for multiselection here we have used the single row selection only
                //afterChange: afterChange, //calling of afterchange function when an afterChange event occurs
                contextMenu: ['row_above', 'row_below', 'remove_row'],
                //mergeCells: [
                //    { row: 0, col: 0, rowspan: 1, colspan: 3 },   // characteristcs
                //    { row: 0, col: 3, rowspan: 3, colspan: 1 },  //spl char class
                //    { row: 0, col: 4, rowspan: 1, colspan: 6 },  //methods
                //    { row: 1, col: 4, rowspan: 1, colspan: 2 },  //product/process spec.tolerance
                //    { row: 1, col: 5, rowspan: 1, colspan: 1 },  //evaluation
                //    { row: 1, col: 6, rowspan: 1, colspan: 1 },  //gauge fixture code
                //    { row: 1, col: 8, rowspan: 1, colspan: 2 },  //sample
                //    { row: 0, col: 10, rowspan: 3, colspan: 1 },  //Res
                //    { row: 0, col: 11, rowspan: 3, colspan: 1 },  //Control Method
                //    { row: 0, col: 12, rowspan: 3, colspan: 1 },  //Reaction Plan
                //],
                //cells: function (row, col, prop) {
                //    var cellProperties = {};
                //    if (row === 0 || row === 1 || row === 2) {
                //        return {
                //            renderer: pseudoHeaderRenderer, readOnly: true
                //        }
                //    }
                //},
                removeRowPlugin: true //remove row other than from context menu
            });

            //function customRenderer(instance, td) {
            //    Handsontable.renderers.TextRenderer.apply(this, arguments);
            //    return td;
            //}


            function removeAllBlankOrNull(JsonObj) {
                $.each(JsonObj, function (key, value) {
                    if (value === "" || value === null) {
                        delete JsonObj[key];
                    }
                    else if (typeof (value) === "object") {
                        JsonObj[key] = removeAllBlankOrNull(value);
                    }
                });
                return JsonObj;
            }



            //Submit Button Onclick

            var hotIsValid = true;
            //Submit Button Onclick

            $('#<%=btnSave.ClientID %>').click(function (e) {

                hot = $container.data('handsontable');

                //  var chk = valproduct() what is this ???

                //if (chk == 1) {
                var r1 = getHotData(hot);
                console.log(r1);

                if (hotIsValid === false) {
                    e.preventDefault();
                    alert('some data are invalid please check');
                    return false;
                }

                //  alert(r1);


                $('#<%=hdnchild.ClientID %>').val(r1);
                // }
                //else {
                //  return false;
                //}

            });


            function getHotData(myhot) {
                var gridData = myhot.getData();
                var hotGridData = {};
                var hotArray = [];
                $.each(gridData, function (rowKey, object) {
                    if (!myhot.isEmptyRow(rowKey)) hotGridData[rowKey] = object;
                });

                $.each(hotGridData, function (x, y) {
                    var obj = new Object();
                    var freqval = 0, cmval = 0, evalval = 0;
                    var freqval2 = 0, cmval2 = 0, evalval2 = 0;

                    if (hotGridData[x][5] != "" && hotGridData[x][5] != null && hotGridData[x][5] != "-") {
                        console.log("evaltech");
                        evalval = GetAjaxValue(hotGridData[x][5], 'GetEveltechslno', 'evltech');
                    }

                    if (hotGridData[x][7] != "" && hotGridData[x][7] != null && hotGridData[x][7] != "-") {
                        console.log("freqslno");
                        freqval = GetAjaxValue(hotGridData[x][7], 'GetFreqslno', 'freq');
                    }

                    if (hotGridData[x][8] != "" && hotGridData[x][8] != null && hotGridData[x][8] != "-") {
                        console.log("controlslno");
                        cmval = GetAjaxValue(hotGridData[x][8], 'GetControlslno', 'ctrl');
                    }

                    if (hotGridData[x][9] != "" && hotGridData[x][9] != null && hotGridData[x][9] != "-") {
                        console.log("evaltech2");
                        evalval2 = GetAjaxValue(hotGridData[x][9], 'GetEveltechslno', 'evltech');
                    }

                    if (hotGridData[x][11] != "" && hotGridData[x][11] != null && hotGridData[x][11] != "-") {
                        console.log("freq2");
                        freqval2 = GetAjaxValue(hotGridData[x][11], 'GetFreqslno', 'freq');
                    }

                    if (hotGridData[x][12] != "" && hotGridData[x][12] != null && hotGridData[x][12] != "-") {
                        console.log('controlslno2');
                        cmval2 = GetAjaxValue(hotGridData[x][12], 'GetControlslno', 'ctrl');
                    }


                    hotIsValid = true;
                    //if (freqval === -1 || cmval === -1 || evalval === -1) {
                    //    hotIsValid = false;
                    //    return false;
                    //}
                    obj.dimn_no = hotGridData[x][0]; // hotGridData[x].dimn_no;
                    obj.product_char = hotGridData[x][1]; //hotGridData[x].product_char;
                    obj.process_char = hotGridData[x][2]; //hotGridData[x].process_char;
                    obj.splfilename = hotGridData[x][3];
                    obj.splChar_slno = 0; //hotGridData[x].splChar_slno;
                    obj.spec1 = hotGridData[x][4]; //hotGridData[x].tol_min;
                    /*obj.spec2 = hotGridData[x][5];*/
                    obj.evalTech = hotGridData[x][5];
                    obj.sampleSize = hotGridData[x][6]; //hotGridData[x].sampleSize;
                    obj.FreqDesc = hotGridData[x][7]; //hotGridData[x].sampleFreq;
                    obj.methodDesc = hotGridData[x][8];
                    obj.method_slno = cmval; //hotGridData[x].method_slno;
                    obj.evalTech2 = hotGridData[x][9];
                    obj.sampleSize2 = hotGridData[x][10]; //hotGridData[x].sampleSize;
                    obj.FreqDesc2 = hotGridData[x][11]; //hotGridData[x].sampleFreq;
                    obj.methodDesc2 = hotGridData[x][12];
                    obj.method_slno2 = cmval; //hotGridData[x].method_slno;
                    obj.reactionPlan = hotGridData[x][13]; //hotGridData[x].reactionPlan;
                    if (hotGridData[x][14] == null)
                        obj.min_spec = 0;
                    else
                        obj.min_spec = hotGridData[x][14];
                    if (hotGridData[x][15] == null)
                        obj.max_spec = 0;
                    else
                        obj.max_spec = hotGridData[x][15]
                    if (hotGridData[x][16] == null)
                        obj.tolerance = 0;
                    else
                        obj.tolerance = hotGridData[x][16];
                    obj.evalTech_slno = evalval;
                    obj.freq_slno = freqval;
                    obj.PDI_type = hotGridData[x][17];
                    hotArray.push(obj);
                });
                var hotresult = JSON.stringify(hotArray);
                return hotresult;
            }

            loadData();

            function loadData() {
                var slno = $('#<%=hdnSlNo.ClientID %>').val();
                if (slno > 0) {
                    var manArray = [];
                    var dArray = [[]];


                    var manhot = $container.data('handsontable');

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Data.asmx/GetControlplanChildData",
                        data: "{'cp_slno' : " + slno + "}",
                        dataType: "json",
                        success: function (res) {
                            var data = JSON.parse(res.d);
                            console.log(res.d);
                            $(data).each(function (i, j) {

                                //  manArray.push(getHotobject(j));
                                for (var i = 0; i < data.length; i++) {
                                    dArray[i] = new Array();

                                    //'dArray[i][0] = data[i].MachineSlNo + ":" + data[i].MachineDesc;
                                    dArray[i][0] = data[i].dimn_no;
                                    dArray[i][1] = data[i].product_char;
                                    dArray[i][2] = data[i].process_char;
                                    dArray[i][3] = data[i].splfilename;
                                    dArray[i][4] = data[i].spec1;
                                    /*dArray[i][5] = data[i].spec2;*/
                                    dArray[i][5] = data[i].evalTech;
                                    dArray[i][6] = data[i].sampleSize;
                                    dArray[i][7] = data[i].FreqDesc;
                                    dArray[i][8] = data[i].methodDesc;
                                    dArray[i][9] = data[i].evalTech2;
                                    dArray[i][10] = data[i].sampleSize2;
                                    dArray[i][11] = data[i].FreqDesc2;
                                    dArray[i][12] = data[i].methodDesc2;
                                    dArray[i][13] = data[i].reactionPlan;
                                    dArray[i][14] = data[i].min_spec;
                                    dArray[i][15] = data[i].max_spec;
                                    dArray[i][16] = data[i].tolerance;
                                    dArray[i][17] = data[i].PDI_type;

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
                        $('#<%=btnSave.ClientID %>').prop('disabled', false);
                       <%-- $('#<%=btnDelete.ClientID %>').prop('disabled', false);--%>//commented this on 18 02 2025
                        if ($('#<%=hdnsubmitstatus.ClientID %>').val() == 'Y' || $('#<%=hdnsubmitstatus.ClientID %>').val() == 'A') {
                            $('#<%=btnSave.ClientID %>').prop('disabled', true);
                            $('#<%=btnDelete.ClientID %>').prop('disabled', true);  //commented this on 18 02 2025

                            //        manhot.updateSettings({
                            //            mergeCells: [
                            //{ row: 0, col: 0, rowspan: 1, colspan: 3 },   // characteristcs
                            //{ row: 0, col: 3, rowspan: 3, colspan: 1 },  //spl char class
                            //{ row: 0, col: 4, rowspan: 1, colspan: 6 },  //methods
                            //{ row: 1, col: 4, rowspan: 1, colspan: 2 },  //product/process spec.tolerance
                            //{ row: 1, col: 5, rowspan: 1, colspan: 1 },  //evaluation
                            //{ row: 1, col: 6, rowspan: 1, colspan: 1 },  //gauge fixture code
                            //{ row: 1, col: 8, rowspan: 1, colspan: 2 },  //sample
                            //{ row: 0, col: 10, rowspan: 3, colspan: 1 },  //Res
                            //{ row: 0, col: 11, rowspan: 3, colspan: 1 },  //Control Method
                            //{ row: 0, col: 12, rowspan: 3, colspan: 1 },  //Reaction Plan
                            //            ],
                            //            cells: function (row, col, prop) {
                            //                var cellProperties = {};
                            //                if (row === 0 || row === 1 || row === 2) {
                            //                    return {
                            //                        renderer: pseudoHeaderRenderer, readOnly: true
                            //                    }
                            //                }
                            //                else if (row >= 3) {
                            //                    cellProperties.readOnly = false;

                            //                    return cellProperties;
                            //                }
                            //            }
                            //        });
                            //   alert('Data submitted');


                        }

                       <%-- if ($('#<%=hdnadmin.ClientID %>').val() == 'Y') {
                            ($('#<%=hdnAppd.ClientID %>').val() != 'Y')
                            {
                                $('#<%=btnDelete.ClientID %>').prop('disabled', false);
                                $('#<%=btnSave.ClientID %>').prop('disabled', false);
                                $('#<%=btnSubmit.ClientID %>').prop('disabled', false);
                            }
                        }
                        if ($('#<%=hdnuser.ClientID %>').val() == 'Y') {
                            $('#<%=btnSave.ClientID %>').prop('disabled', false);
                        }--%>

                    });
                }
            }

            function getHotobject(myobj) {
                var obj = new Object();
                obj.dimn_no = myobj.dimn_no;
                obj.product_char = myobj.product_char;
                obj.process_char = myobj.process_char;
                obj.splfilename = myobj.splfilename;
                obj.spec1 = myobj.spec1;
                /*obj.spec2 = myobj.spec2;*/
                obj.evalTech = myobj.evalTech;
                obj.sampleSize = myobj.sampleSize;
                obj.FreqDesc = myobj.FreqDesc;
                obj.methodDesc = myobj.methodDesc;
                obj.evalTech2 = myobj.evalTech2;
                obj.sampleSize2 = myobj.sampleSize2;
                obj.FreqDesc2 = myobj.FreqDesc2;
                obj.evalTech2 = myobj.evalTech2;
                obj.methodDesc2 = myobj.methodDesc2;
                obj.reactionPlan = myobj.reactionPlan;
                obj.min_spec = myobj.min_spec;
                obj.max_spec = myobj.max_spec;
                obj.tolerance = myobj.tolerance;
                obj.PDI_type = myobj.PDI_type;

                return obj;
            }

            $('#<%=btnirev.ClientID %>').on('click', function (e) {
                e.preventDefault();
                var slno = $('#<%=hdnSlNo.ClientID %>').val();
            var ss = $('#<%=hdnsubmitstatus.ClientID %>').val();
            if (slno > 0 && ss == 'A') {
                var partslno = $('#<%=ddlpart_slno.ClientID %>').val();
                    var oprnslno = $('#<%=ddloperation_slno.ClientID %>').val();
                    var changearea = "CP";

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Data.asmx/GetDCRData",
                        data: "{'partslno' : " + partslno + ", 'oprnslno' : " + oprnslno + ", 'changearea' : '" + changearea + "'}",
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
                        url: "Data.asmx/GetRevNum",
                        data: "{'partslno' : " + partslno + ", 'oprnslno' : " + oprnslno + ", 'changearea' : '" + changearea + "'}",
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

                alert('Revision cannot be initiated. Either no record is chosen or the current control plan is not approved');
            }
        });


        });
        // end of document ready

        var ht = $container.data('handsontable');

        function afterChange(changes, src) {

            console.log(changes, src)


            if (changes != null || changes != undefined) {
                min = ht.getDataAtRowProp(changes[0][0], "min_spec");
                max = ht.getDataAtRowProp(changes[0][0], "max_spec");
            }
        }



        //ht.addHook('afterChange', afterChange)

        //ht.addHook('afterChange', function (change, source) {
        //   if (change != null || change != undefined) {
        //       console.log(change, source);
        //       var min;
        //       var max;
        //       if (change[0][1] == "min_spec" || change[0][1] == "max_spec") {
        //           min = ht.getDataAtRowProp(change[0][0], "min_spec");
        //          max = ht.getDataAtRowProp(change[0][0], "max_spec");
        //            if (min >= 0 && max >= 0) {                       
        //              if (min > max) {
        //                   // ht.setDataAtRowProp(change[0][0], "qty_used", "Error");
        //                    alert("Min_spec  should be less than Max_spec");
        //                    return;
        //               }

        //            }
        //        }
        //    }
        //  ht.render()
        //});



        $('.myselect').select2({
            theme: "classic"
        });

        function GetDCRData() {
            var partslno = $('#<%=ddlpart_slno.ClientID %>').val();
            var oprnslno = $('#<%=ddloperation_slno.ClientID %>').val();
            var changearea = "CP";

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Data.asmx/GetDCRData",
                data: "{'partslno' : " + partslno + "'oprnslno' : " + oprnslno + "'changearea' : " + changearea + "}",
                dataType: "json",
                success: function (res) {
                    var data = JSON.parse(res.d);
                    console.log(data);
                    $('#txtdcrReqDate').val(data[0].Request_Date);
                    $('#txtdcrReqBy').val(data[0].Request_By);
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                }
            });



        }

    </script>

</asp:Content>

