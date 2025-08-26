<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sop.aspx.cs" Inherits="sop" %>

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
                <h1>SOP
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
                                    <div class="pull-left">
                                        <asp:Button Text="Initiate Revision" runat="server" CssClass="btn btn-warning" ID="btnirev" Enabled="false" />
                                        <asp:Button ID="btnApproved" Text="Approved" runat="server" CssClass="btn btn-success" Enabled="false" OnClick="btnApproved_Click" />
                                    </div>
                                    <div class="pull-right">
                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" PostBackUrl="~/sop_qry.aspx" />
                                        <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="btn btn-danger" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnSubmit" Text="SUBMIT" runat="server" CssClass="btn btn-warning" Enabled="false" OnClick="btnSubmit_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part# </label>
                                            <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlpart_slno_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtCPType"><i class="fa fa-check-circle"></i>&nbsp;Control Plan Type </label>
                                            <asp:DropDownList runat="server" ID="ddlcpType" CssClass="form-control">
                                                <asp:ListItem Text="Select..." Value="0" />
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
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="ddloperation_slno" runat="server" InitialValue="Select..."
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtMachineType"><i class="fa fa-check-circle"></i>&nbsp;Machine</label>
                                            <asp:DropDownList runat="server" ID="ddlmachine_slno" CssClass="form-control">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Data Missing!" ControlToValidate="ddlmachine_slno" runat="server" InitialValue="Select..."
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="prevoprn">Previous Operation</label>
                                            <asp:TextBox runat="server" ID="txtprevoprn" CssClass="form-control" ReadOnly></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="nextoprn">Next Operation</label>
                                            <asp:TextBox runat="server" ID="txtnextoprn" CssClass="form-control" ReadOnly></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="objective">Objective</label>
                                            <asp:TextBox ID="txtobjective" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-5">
                                        <div class="form-group">
                                            <label for="fileUpload1">
                                                Before Machining&nbsp;&nbsp;&nbsp;                                            
                                                <span class="text-right"><a href="#" id="delFile1" runat="server" onclick="ConfirmDelete(1)" onserverclick="delFile1_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span>
                                            </label>
                                            <table class="table table-condensed">
                                                <tr>
                                                    <td>
                                                        <input id="FileUpload1" type="file" name="file" runat="server" onchange="previewFile1()" />
                                                        <a id="hrefFile1" href="#" runat="server">
                                                            <asp:Label ID="lblFile1" runat="server"></asp:Label>
                                                        </a>
                                                        <asp:Image ID="imgFile1" runat="server" Height="225px" ImageUrl="~/dist/img/boxed-bg.jpg" Width="225px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="col-md-5 col-md-offset-2">
                                        <div class="form-group">
                                            <label for="fileUpload2">
                                                After Machining&nbsp;&nbsp;&nbsp;
                                                <span class="text-right"><a href="#" id="delFile2" runat="server" onclick="ConfirmDelete(2)" onserverclick="delFile2_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span></label>
                                            <table class="table table-condensed">
                                                <tr>
                                                    <td>
                                                        <input id="FileUpload2" type="file" name="file1" runat="server" onchange="previewFile2()" />
                                                        <a id="hrefFile2" href="#" runat="server">
                                                            <asp:Label ID="lblFile2" runat="server"></asp:Label>
                                                        </a>
                                                        <asp:Image ID="imgFile2" runat="server" Height="225px" ImageUrl="~/dist/img/boxed-bg.jpg" Width="225px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="oprninstr">Operation Instruction</label>
                                            <asp:TextBox runat="server" ID="txtoprninstruction" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="oprninstr">List of Checkpoints</label>
                                            <asp:TextBox runat="server" ID="txtochkpoints" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="oprninstr">Work Holding Method</label>
                                            <asp:TextBox runat="server" ID="txtworkholding" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="oprninstr">First Off Approval</label>
                                            <asp:TextBox runat="server" ID="txtfirstoff" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <asp:Button runat="server" ID="btncp" CssClass="btn btn-facebook" Text="Pull From Control Plan" OnClick="btnCP_Click" />
                                <div class="row">
                                    <div class="col-md-6" runat="server" id="divgridprocess">
                                        <label for="table1">Table-1 Process Parameter</label>
                                        <div id="divtable1" class="box-body" style="overflow-x: auto; overflow-y: auto; width: 500px; height: 200px; border: 1px solid #ccc;">
                                            <asp:GridView ID="grdProcessParam" runat="server" ClientIDMode="Static"
                                                CssClass="table table-bordered table-responsive table-hover" UseAccessibleHeader="true"
                                                ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                                EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                                <Columns>
                                                    <asp:BoundField DataField="process_char" HeaderText="Parameter" />
                                                    <asp:BoundField DataField="spec1" HeaderText="Specification" />
                                                    <asp:BoundField DataField="evalTech" HeaderText="Checking Method" />
                                                    <asp:BoundField DataField="FreqDesc" HeaderText="Check Freq." />
                                                    <asp:BoundField DataField="methodDesc" HeaderText="Tool Of Control" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="col-md-6" runat="server" id="divHandsonProcess" visible="false">
                                        <label for="tableprocessparam">Table-1 Process Parameter</label>
                                        <div id="tableprocParam" class="handsontable"></div>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="table2">Table-2 Tooling Details</label>
                                        <div id="table2" class="handsontable"></div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="table3">Table-3 Quality Characteristics</label>

                                        <div id="divtable2" class="box-body" style="overflow-x: auto; overflow-y: auto; width: 500px; height: 200px; border: 1px solid #ccc;">
                                            <asp:GridView ID="grdQualityChar" runat="server" ClientIDMode="Static"
                                                CssClass="table table-bordered table-responsive table-hover" UseAccessibleHeader="true"
                                                ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                                EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                                <Columns>
                                                    <asp:BoundField DataField="Product_char" HeaderText="Control Item" />
                                                    <asp:BoundField DataField="spec1" HeaderText="Specification(in mm.)" />
                                                    <asp:BoundField DataField="splfilename" HeaderText="Special Character" />
                                                    <asp:BoundField DataField="evalTech" HeaderText="Measurement Technique" />
                                                    <asp:BoundField DataField="FreqDesc" HeaderText="Sample Size & Frequency" />
                                                    <asp:BoundField DataField="MethodDesc" HeaderText="Control Method" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="oprninstr">Coolant Used</label>
                                            <asp:TextBox runat="server" ID="txtcoolant" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="oprninstr">Reaction Plan</label>
                                            <asp:TextBox runat="server" ID="txtreacionplan" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="oprninstr">Notes</label>
                                            <asp:TextBox runat="server" ID="txtnotes" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
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
        <asp:HiddenField ID="hdnCpType" runat="server" Value="" />
        <asp:HiddenField ID="hdnProcessNo" runat="server" Value="" />
        <asp:HiddenField ID="hdnMachine" runat="server" Value="" />
        <asp:HiddenField ID="hdnuser" runat="server" Value="" />
        <asp:HiddenField ID="hdnAppd" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnPrep" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdnprevoprnslno" runat="server" />
        <asp:HiddenField ID="hdnnextoprnslno" runat="server" />
        <asp:HiddenField ID="hdnchildPrc" runat="server" />
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
                        <div class="box-default">
                            <div class="form-group">
                                <label for="txtUserRevNo">Rev.No.</label>
                                <asp:TextBox runat="server" ID="txtUserRevNo" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="txtUserRevDt">Rev.Date</label>
                                <asp:TextBox runat="server" ID="txtUserRevDt" CssClass="form-control myDate" />
                            </div>
                            <div class="form-group">
                                <label for="txtRevReason">Rev.Reason</label>
                                <asp:TextBox ID="txtrevreason" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnrevision" runat="server" Text="Revision" CssClass="btn btn-flat btn-info" OnClick="btnrevision_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                            Close</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/handsontable/handsontable.full.min.js"></script>
    <script src="Scripts/hotdata.js"></script>
    <script src="Scripts/prop.js"></script>
    <script type="text/javascript" src="Scripts/ajax.js"></script>

    <%--       <link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />
   <script type="text/javascript" src="Scripts/handsontable/handsontable.full.min.js"></script>
   <script src="Scripts/hotdata.js"></script>
   <script src="Scripts/prop.js"></script>
   <script type="text/javascript" src="Scripts/ajax.js"></script>--%>
    <script>
        //var $container2 = $("#table2");
        $("document").ready(function () {

            var $container2 = $("#table2");
            var $containerprocess = $("#tableprocParam");
            $container2.handsontable({
                startRows: 5, //number of rows to be displayed at initialization
                startCols: 8, // number of columns to be displayed
                minRows: 4,
                minCols: 8,
                //data: data,
                height: 200,
                rowHeaders: false, //show row headers
                minSpareRows: 5, //minimum last row
                minSpareCols: 0,
                enterMoves: { row: 0, col: 1 },
                autoWrapRow: true,
                contextMenu: false,
                outsideClickDeselects: false, //deselect if clicked outside
                removeRowPlugin: true, //remove row other than from context menu
                currentRowClassName: 'currentRow',  //css class for selected row
                colWidths: [150, 150,
                    100, 120, 200,
                    200, 200, 150

                ],

                colHeaders: ["Tool Holder Name", "Tool", "Cutting Speed", "Feed Rate", "Tool Life - Per Corner (Nos.).", "Tool Life - No. of Corner", "Tool Life - Total Nos.", "Control Method"], //names of columnheaders
                columns: [
                    { type: 'text', data: 'tool_holder_name' },
                    { type: 'text', data: 'tool' },
                    { type: 'text', data: 'cutting_speed' },
                    { type: 'text', data: 'feed_rate' },
                    { type: 'text', data: 'per_corner' },
                    { type: 'text', data: 'no_of_corners' },
                    { type: 'text', data: 'total_nos', },
                    { type: 'text', data: 'control_method' },
                ],

                removeRowPlugin: true //remove row other than from context menu
            });

            $containerprocess.handsontable({
                startRows: 5, //number of rows to be displayed at initialization
                startCols: 6, // number of columns to be displayed
                minRows: 4,
                minCols: 6,
                //data: data,
                height: 200,
                rowHeaders: false, //show row headers
                minSpareRows: 5, //minimum last row
                minSpareCols: 0,
                enterMoves: { row: 0, col: 1 },
                autoWrapRow: true,
                contextMenu: false,
                outsideClickDeselects: false, //deselect if clicked outside
                removeRowPlugin: true, //remove row other than from context menu
                currentRowClassName: 'currentRow',  //css class for selected row
                colWidths: [50, 150,
                    100, 120, 200,
                    150

                ],

                colHeaders: ["Sl.No.", "Parameter", "Spec.", "Checking Method", "Checking Frequency", "Tool Of Control"], //names of columnheaders            
                columns: [
                    { type: 'text'},
                    { type: 'text', data: 'ProcessParameter' },
                    { type: 'text', data: 'Specification' },
                    { type: 'text', data: 'CheckingMethod' },
                    { type: 'text', data: 'Frequency' },
                    { type: 'text', data: 'ToolOfControl' },

                ],

                removeRowPlugin: true //remove row other than from context menu
            });
           <%-- $('#<%=btnSave.ClientID%>').on('click', function (e) {
                var $container2 = $("#table2");
                var hotobj = $container2.data('handsontable');
                //waitforbuttontodisable();
                var gridData = hotobj.getData();
                var hotGridData = gridData;//{};
                alert(hotGridData);
                var hotArray = [];
                $.each(hotGridData, function (x, y) {
                    if (hotGridData[x][0] != null) {
                        var obj = new Object();
                        obj.tool_holder_name = hotGridData[x][0];
                        obj.tool = hotGridData[x][1];
                        obj.cutting_speed = hotGridData[x][2];
                        obj.per_corner = hotGridData[x][3];
                        obj.no_of_corners = hotGridData[x][4];
                        obj.total_nos = hotGridData[x][5];
                        obj.control_method = hotGridData[x][6];
                        hotArray.push(obj);
                   }
                });
                var hotresult = JSON.stringify(hotArray);
                console.log(hotresult);
                $('#<%=hdnchild.ClientID %>').val(hotresult);
            });--%>
            var $containerhot = $("#table2");
            var $containerhotProcess = $("#tableprocParam");
            $('#<%=btnSave.ClientID %>').click(function (e) {
                hot = $containerhot.data('handsontable');
                hotprc = $containerhotProcess.data('handsontable');
                alert(hotprc);
                var r1 = getHotData(hot);
                var rprc = getHotDataPrc(hotprc);
                console.log(r1);
                console.log(rprc);
                $('#<%=hdnchild.ClientID %>').val(r1);
                $('#<%=hdnchildPrc.ClientID %>').val(rprc);
                
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
                    obj.tool_holder_name = hotGridData[x][0];
                    obj.tool = hotGridData[x][1];
                    obj.cutting_speed = hotGridData[x][2];
                    obj.feed_rate = hotGridData[x][3];
                    obj.per_corner = hotGridData[x][4];
                    obj.no_of_corners = hotGridData[x][5];
                    obj.total_nos = hotGridData[x][6];
                    obj.control_method = hotGridData[x][7];
                    hotArray.push(obj);
                });
                var hotresult = JSON.stringify(hotArray);
                return hotresult;
            }

            function getHotDataPrc(myhotprc) {
                var gridData = myhotprc.getData();
                var hotGridData = {};
                var hotArray = [];
                $.each(gridData, function (rowKey, object) {
                    if (!myhotprc.isEmptyRow(rowKey)) hotGridData[rowKey] = object;
                });

                $.each(hotGridData, function (x, y) {
                    var obj = new Object();
                    obj.sop_id = $('#<%=hdnSlNo.ClientID %>').val();
                    obj.ProcessParameter = hotGridData[x][1];
                    obj.Specification = hotGridData[x][2];
                    obj.CheckingMethod = hotGridData[x][3];
                    obj.Frequency = hotGridData[x][4];
                    obj.ToolOfControl = hotGridData[x][5];
                    hotArray.push(obj);
                });
                var hotresult = JSON.stringify(hotArray);
                return hotresult;
            }

            previewFile1();
            previewFile2();
            LoadTableData();
            LoadTableDataPrc();
            function LoadTableData() {
                var dArray = [[]];

                var $container1 = $("#table2");
                var ht = $container1.data('handsontable');
                var slno = $('#<%=hdnSlNo.ClientID%>').val();

                $.ajax({
                    url: "Data.asmx/LoadSOPToolingsData",
                    dataType: 'json',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    data: "{'slno' : '" + slno + "'}",
                    success: function (res) {
                        var gdata = JSON.parse(res.d);
                        if (gdata.length > 0) {
                            for (var i = 0; i < gdata.length; i++) {
                                dArray[i] = new Array();

                                dArray[i][0] = gdata[i].tool_holder_name;
                                dArray[i][1] = gdata[i].tool;
                                dArray[i][2] = gdata[i].cutting_speed;
                                dArray[i][3] = gdata[i].feed_rate;
                                dArray[i][4] = gdata[i].per_corner;
                                dArray[i][5] = gdata[i].no_of_corners;
                                dArray[i][6] = gdata[i].total_nos;
                                dArray[i][7] = gdata[i].control_method;

                            }

                        }
                    },
                    error: function (jqxhr, status, error) {
                        alert(error);
                    }
                }).done(function () {
                    console.log(dArray);
                    ht.populateFromArray(0, 0, dArray);
                    hot = $container1.data('handsontable');
                    var r1 = getHotData(hot);
                    console.log(r1);
                    $('#<%=hdnchild.ClientID %>').val(r1);
                    console.log($('#<%=hdnchild.ClientID %>').val());
                });
            }

            function LoadTableDataPrc() {
                var dArray = [[]];

                var $containerprc = $("#tableprocParam");
                var ht = $containerprc.data('handsontable');
                var slno = $('#<%=hdnSlNo.ClientID%>').val();

                $.ajax({
                    url: "Data.asmx/LoadSOPPrcParamData",
                    dataType: 'json',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    data: "{'slno' : '" + slno + "'}",
                    success: function (res) {
                        var gdata = JSON.parse(res.d);
                        if (gdata.length > 0) {
                            for (var i = 0; i < gdata.length; i++) {
                                dArray[i] = new Array();
                                dArray[i][0] = i + 1;
                                dArray[i][1] = gdata[i].ProcessParameter;
                                dArray[i][2] = gdata[i].Specification;
                                dArray[i][3] = gdata[i].CheckingMethod;
                                dArray[i][4] = gdata[i].Frequency;
                                dArray[i][5] = gdata[i].ToolOfControl;
                            }

                        }
                    },
                    error: function (jqxhr, status, error) {
                        alert(error);
                    }
                }).done(function () {
                    console.log(dArray);
                    ht.populateFromArray(0, 0, dArray);
                    hot = $containerprc.data('handsontable');
                    var r1 = getHotData(hot);
                    console.log(r1);
                    $('#<%=hdnchildPrc.ClientID %>').val(r1);
                      console.log($('#<%=hdnchildPrc.ClientID %>').val());
                  });
            }

            $('#<%=btnirev.ClientID %>').on('click', function (e) {
                e.preventDefault();
                var slno = $('#<%=hdnSlNo.ClientID %>').val();
                var ss = $('#<%=hdnsubmitstatus.ClientID %>').val();
                if (slno > 0 && ss == 'A') {

                    $('#modalQaApproval').modal('show');;
                } else {

                    alert('Revision cannot be initiated. Either no record is chosen or the current control plan is not approved');
                }
            });



        });


        function previewFile1() {
            console.log("Executing previewFile1");
            var preview = document.querySelector('#<%=imgFile1.ClientID %>');
            var fileInput = document.querySelector('#<%=FileUpload1.ClientID %>');
            var fileNameLabel = document.querySelector('#<%=lblFile1.ClientID %>');
           // var fileNameLabel = $('#<%=lblFile1.ClientID %>').val();
            if (fileInput.files.length > 0) {
                var file = fileInput.files[0];

                var reader = new FileReader();
                reader.onloadend = function () {
                    preview.src = reader.result;
                };

                if (file) {
                    reader.readAsDataURL(file);
                } else {
                    preview.src = "";
                }
            }
            else if (fileNameLabel.innerText != "") {
                // If no file is selected, set the preview based on the filename from the label
                preview.src = "/Documents/SOP/" + fileNameLabel.innerText;
            }
            else { preview.src = "/dist/img/boxed-bg.jpg"; }
        }

        function previewFile2() {
            console.log("Executing previewFile2");
            var preview2 = document.querySelector('#<%=imgFile2.ClientID %>');
            var fileInput2 = document.querySelector('#<%=FileUpload2.ClientID %>');
            var fileNameLabel2 = document.querySelector('#<%=lblFile2.ClientID %>');
           // var fileNameLabel2=  $('#<%=lblFile2.ClientID %>').val();
            if (fileInput2.files.length > 0) {
                var file2 = fileInput2.files[0];

                var reader2 = new FileReader();
                reader2.onloadend = function () {
                    preview2.src = reader2.result;
                };

                if (file2) {
                    reader2.readAsDataURL(file2);
                } else {
                    preview2.src = "";
                }
            } else if (fileNameLabel2.innerText != "") {
                // If no file is selected, set the preview based on the filename from the label
                preview2.src = "/Documents/SOP/" + fileNameLabel2.innerText;
            } else { preview2.src = "/dist/img/boxed-bg.jpg"; }
        }


        //function ConfirmDelete(fileId) {
        //    var confirmation = confirm("Are you sure you want to delete this file?");
        //    if (confirmation) {
        //        // User clicked 'OK', perform the delete action

        //        var lblFile = document.getElementById('lblFile' + fileId);
        //        var imgFile = document.getElementById('imgFile' + fileId);
        //        var fileUpload = document.getElementById('FileUpload' + fileId);

        //        // Assuming you want to clear the file-related controls
        //       /// lblFile.innerHTML = '';
        //       // imgFile.src = '';
        //       // fileUpload.value = '';
        //        // Call server-side function to delete the file
        //        deleteFileFromServer(fileId);


        //        alert("File deleted!");
        //    } else {
        //        // User clicked 'Cancel', do nothing or handle accordingly
        //        alert("Deletion canceled.");
        //    }
        //}

        //function deleteFileFromServer(fileId) {
        //    // Make an AJAX call to the server to delete the file
        //    $.ajax({
        //        type: 'POST',
        //        url: 'sop.aspx/DeleteFile',
        //        data: '{ fid: ' + fileId + ' }',
        //        contentType: 'application/json; charset=utf-8',
        //        dataType: 'json',
        //        success: function (response) {
        //            // Handle the response from the server (if needed)
        //        },
        //        error: function (error) {
        //            // Handle the error (if any)
        //            console.error('Error deleting file:', error);
        //        }
        //    });
        //}

    </script>
</asp:Content>

