<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PartRevEntry.aspx.cs" Inherits="PartRevEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />--%>
    <%--<script type="text/javascript" src="Scripts/handsontable/handsontable.full.min.js"></script>--%>

    <link href="Content/handsontable/handsontable.full.min.css" rel="stylesheet" />
    <script src="Scripts/handsontable/handsontable.full.min.js"></script>
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Part Rev. Entry
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
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="ddlpart_slno"><i class="fa fa-check-circle"></i>&nbsp;Part</label>
                                            <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect">
                                                <asp:ListItem Text="Select..." />
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box no-border no-header no-footer no-margin">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div id="cp" class="handsontable"></div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>

        <asp:HiddenField ID="hdnchild" runat="server" />
    </form>
    <script>
        $('.myselect').select2({
            theme: "classic"
        });


        var $container = $("#cp");
        var data = [];

        $container.handsontable({
            startRows: 2, //number of rows to be displayed at initialization
            startCols: 6, // number of columns to be displayed
            //minRows: 4,
            //minCols: 5,
            //data: data,
            height: 350,
            rowHeaders: false, //show row headers
            minSpareRows: 1, //minimum last row
            minSpareCols: 0,
            enterMoves: { row: 0, col: 1 },
            autoWrapRow: true,
            contextMenu: false,
            outsideClickDeselects: false, //deselect if clicked outside
            removeRowPlugin: true, //remove row other than from context menu
            currentRowClassName: 'currentRow',  //css class for selected row
            colWidths: [70, 100, 250, 400, 250, 0.1],
            colHeaders: ["Rev.No.", "Rev.Dt.", "Nature Of Change", "Reason for Change", "Revision Done In"], //names of columnheaders
            columns: [
                { data: "rev_no" },
                { data: "rev_date" },
                { data: "change_nature" },
                { data: "rev_reasons" },
                { data: "revision_done_in" }
              //  { data: "rev_slno" },
            ], //column mapping to the respecitve data fields
            afterSelectionEnd: function (r, c, r1, c1) {
                row = r;
                col = c;
            }, // this function is called when a row is selected it can have multiple rows and columns for multiselection here we have used the single row selection only
            // afterChange: afterchange, //calling of afterchange function when an afterChange event occurs
            //contextMenu: ['row_above', 'row_below', 'remove_row'],
            // removeRowPlugin: true //remove row other than from context menu
        });

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

        var hotIsValid = true;
        //Submit Button Onclick

        $('#<%=btnSubmit.ClientID %>').click(function (e) {
            hot = $container.data('handsontable');
            var r1 = getHotData(hot);
            console.log(r1);
            if (hotIsValid === false) {
                e.preventDefault();
                alert('some data are invalid please check');
                return false;
            }
            $('#<%=hdnchild.ClientID %>').val(r1);
        });


        function getHotData(myhot) {
            var gridData = myhot.getData();
            var hotGridData = {};
            var hotArray = [];
            $.each(gridData, function (rowKey, object) {
                if (!myhot.isEmptyRow(rowKey)) hotGridData[rowKey] = object;
            });
            var partno = $('#<%=ddlpart_slno.ClientID%> :selected').val();

            $.each(hotGridData, function (x, y) {
                var obj = new Object();

                hotIsValid = true;

                obj.part_slno = partno;
                obj.rev_no = hotGridData[x][0];
                obj.rev_date = hotGridData[x][1];
                obj.change_nature = hotGridData[x][2];
                obj.rev_reasons = hotGridData[x][3];
                obj.revision_done_in = hotGridData[x][4];
                //obj.rev_slno = hotGridData[x][5];
               // var revSlno = hotGridData[x][5];
                //if (revSlno !== null && revSlno !== "" && revSlno !== 0) {
                //    obj.rev_slno = parseInt(revSlno);
                //}
                hotArray.push(obj);
            });
            var hotresult = JSON.stringify(hotArray);
            return hotresult;
        }

        function loadData() {
            var slno = $('#<%=ddlpart_slno.ClientID %> :selected').val();
            var p = $('#<%=ddlpart_slno.ClientID %> :selected').text();
            if (slno > 0) {
                var manArray = [];
                var dArray = [[]];
                var manhot = $container.data('handsontable');
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Data.asmx/GetPartRevHistory",
                    data: "{'partsl' : '" + slno + "','p' : '" + p + "'}",
                    dataType: "json",
                    success: function (res) {
                        var data = JSON.parse(res.d);
                        console.log(res.d);
                        console.log("DATA:----" + data);

                        //if (data.length === 0) {
                        //    // Editable (new entry mode)
                        //    manhot.updateSettings({
                        //        columns: [
                        //            { data: "rev_no", editor: 'text' },
                        //            { data: "rev_date", editor: 'text' },
                        //            { data: "change_nature"},
                        //            { data: "rev_reasons"},
                        //            { data: "revision_done_in"},
                        //            { data: "rev_slno"}
                        //        ]
                        //    });
                        //} else {
                        //    // Read-only (history mode)
                        //    manhot.updateSettings({
                        //        columns: [
                        //            { data: "rev_no", editor: false },
                        //            { data: "rev_date", editor: false },
                        //            { data: "change_nature"},
                        //            { data: "rev_reasons" },
                        //            { data: "revision_done_in" },
                        //            { data: "rev_slno" }
                        //        ]
                        //    });
                        //}







                        $(data).each(function (i, j) {
                            for (var i = 0; i < data.length; i++) {
                                dArray[i] = new Array();
                                dArray[i][0] = data[i].rev_no;
                                dArray[i][1] = data[i].rev_date;
                                dArray[i][2] = data[i].change_nature;
                                dArray[i][3] = data[i].rev_reasons;
                                dArray[i][4] = data[i].revision_done_in;
                              //  dArray[i][5] = data[i].rev_slno;
                            }
                            console.log('darray:', dArray);
                        });

                    },
                    error: function (jqxhr, status, error) {
                        var msg = JSON.parse(jqxhr.responseText);
                        errormsg += JSON.stringify(msg);
                        alert(errormsg);
                    }
                }).done(function () {
                    if (typeof dArray !== 'undefined' && dArray.length > 0) { manhot.populateFromArray(0, 0, dArray); }
                });
            }
        }


        $('#<%=ddlpart_slno.ClientID%>').on('change', function () {
            var manhot = $container.data('handsontable');
            manhot.clear();
            loadData();
        });
    </script>
</asp:Content>

