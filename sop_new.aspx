<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sop_new.aspx.cs" Inherits="sop_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <link href="Content/handsontable/handsontable.full.css" rel="stylesheet" />

    <%--   <link href="Content/handsontable/handsontable.full.min.css" rel="stylesheet" />--%>
    <script type="text/javascript" src="Scripts/handsontable/handsontable.full.js"></script>
    <script type="text/javascript" src="Scripts/hotdata.js"></script>
    <script type="text/javascript" src="Scripts/ajax.js"></script>
    <script type="text/javascript" src="Scripts/prop.js"></script>

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

        .separator {
            border-top: 1px solid #000; /* Change the color and thickness as needed */
            margin: 20px 0; /* Adjust the margin to create space above and below the line */
        }

        .center-header {
            text-align: center;
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
                                    <%--<div class="pull-left">
                                        <asp:Button Text="Initiate Revision" runat="server" CssClass="btn btn-warning" ID="btnirev" Enabled="false" />
                                        <asp:Button ID="btnApproved" Text="Approved" runat="server" CssClass="btn btn-success" Enabled="false" OnClick="btnApproved_Click" />
                                    </div>--%>
                                    <div class="pull-right">
                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnQuery" Text="Query" runat="server" CssClass="btn btn-info" PostBackUrl="~/sop_qry_new.aspx" />
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
                                            <label for="Group_Id"><i class="fa fa-check-circle"></i>&nbsp;Group </label>
                                            <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlgroup_slno_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Data Missing!" ControlToValidate="ddlGroup" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="Template"><i class="fa fa-check-circle"></i>&nbsp;Template </label>
                                            <asp:TextBox runat="server" Enabled="false" ID="txtTemplate" CssClass="form-control">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Data Missing!" ControlToValidate="txtTemplate" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group has-success">
                                            <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part# </label>
                                            <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlpart_slno_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>
                                    <div class="col-md-3 hidden">
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
                                            <asp:DropDownList runat="server" ID="ddloperation_slno" Enabled="false" CssClass="form-control">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="ddloperation_slno" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group has-success">
                                            <label for="txtMachineType"><i class="fa fa-check-circle"></i>&nbsp;Machine</label>
                                            <asp:DropDownList runat="server" ID="ddlmachine_slno" Enabled="false" CssClass="form-control">
                                                <asp:ListItem Text="Select..." Value="0" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Data Missing!" ControlToValidate="ddlmachine_slno" runat="server" InitialValue="0"
                                                CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="prevoprn">Previous Operation</label>
                                            <asp:TextBox runat="server" ID="txtprevoprn" CssClass="form-control" ReadOnly></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="nextoprn">Next Operation</label>
                                            <asp:TextBox runat="server" ID="txtnextoprn" CssClass="form-control" ReadOnly></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <asp:Button ID="btnCopy" runat="server" OnClick="btnCopy_Click" Text="Copy From Previous SOP" CssClass="btn btn-primary" />
                                        </div>
                                    </div>
                                </div>
                                <label onclick="toggleDiv()" style="cursor: pointer;">SOP</label>
                                <div id="divcontent" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <label for="objective">Objective</label>
                                                <asp:TextBox ID="txtobjective" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="fileUpload1">
                                                    Instruction No: 1&nbsp;&nbsp;&nbsp;                                            
                                                <span class="text-right"><a href="#" id="delFile1" runat="server" onclick="ConfirmDelete(1)" onserverclick="delFile1_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span>
                                                </label>
                                                <table class="table table-condensed">
                                                    <tr>
                                                        <td>
                                                            <input id="FileUpload1" type="file" name="file" runat="server" />
                                                            <a id="hrefFile1" runat="server">
                                                                <asp:Label ID="lblFile1" runat="server"></asp:Label>
                                                            </a>

                                                            <div class="form-group">
                                                                <asp:Label runat="server" for="txtcomment1">Comment </asp:Label>
                                                                <asp:TextBox runat="server" ID="txtcomment1" CssClass="form-control" Width="150" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="fileUpload2">
                                                    Instruction No: 2&nbsp;&nbsp;&nbsp;
                                                <span class="text-right"><a href="#" id="delFile2" runat="server" onclick="ConfirmDelete(2)" onserverclick="delFile2_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span></label>
                                                <table class="table table-condensed">
                                                    <tr>
                                                        <td>
                                                            <input id="FileUpload2" type="file" name="file" runat="server" />
                                                            <a id="hrefFile2" runat="server">
                                                                <asp:Label ID="lblFile2" runat="server"></asp:Label>
                                                            </a>

                                                            <div class="form-group">
                                                                <asp:Label runat="server" for="txtcomment2">Comment </asp:Label>
                                                                <asp:TextBox runat="server" ID="txtcomment2" CssClass="form-control" Width="150" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="fileUpload3">
                                                    Instruction No: 3&nbsp;&nbsp;&nbsp;                                            
                                                <span class="text-right"><a href="#" id="delFile3" runat="server" onclick="ConfirmDelete(3)" onserverclick="delFile3_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span>
                                                </label>
                                                <table class="table table-condensed">
                                                    <tr>
                                                        <td>
                                                            <input id="FileUpload3" type="file" name="file" runat="server" />
                                                            <a id="hrefFile3" runat="server">
                                                                <asp:Label ID="lblFile3" runat="server"></asp:Label>
                                                            </a>

                                                            <div class="form-group">
                                                                <asp:Label runat="server" for="txtcomment3">Comment </asp:Label>
                                                                <asp:TextBox runat="server" ID="txtcomment3" CssClass="form-control" Width="150" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="fileUpload4">
                                                    Instruction No: 4&nbsp;&nbsp;&nbsp;
                    <span class="text-right"><a href="#" id="delFile4" runat="server" onclick="ConfirmDelete(4)" onserverclick="delFile4_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span></label>
                                                <table class="table table-condensed">
                                                    <tr>
                                                        <td>
                                                            <input id="FileUpload4" type="file" name="file" runat="server" />
                                                            <a id="hrefFile4" runat="server">
                                                                <asp:Label ID="lblFile4" runat="server"></asp:Label>
                                                            </a>

                                                            <div class="form-group">
                                                                <label for="txtcomment4">Comment </label>
                                                                <asp:TextBox runat="server" ID="txtcomment4" CssClass="form-control" Width="150" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="fileUpload1">
                                                    Instruction No: 5&nbsp;&nbsp;&nbsp;                                            
                    <span class="text-right"><a href="#" id="delFile5" runat="server" onclick="ConfirmDelete(5)" onserverclick="delFile5_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span>
                                                </label>
                                                <table class="table table-condensed">
                                                    <tr>
                                                        <td>
                                                            <input id="FileUpload5" type="file" name="file" runat="server" />
                                                            <a id="hrefFile5" runat="server">
                                                                <asp:Label ID="lblFile5" runat="server"></asp:Label>
                                                            </a>

                                                            <div class="form-group">
                                                                <label for="txtcomment5">Comment </label>
                                                                <asp:TextBox runat="server" ID="txtcomment5" CssClass="form-control" Width="150" Rows="2" TextMode="MultiLine"> </asp:TextBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="fileUpload6">
                                                    Instruction No: 6&nbsp;&nbsp;&nbsp;
                    <span class="text-right"><a href="#" id="delFile6" runat="server" onclick="ConfirmDelete(6)" onserverclick="delFile6_Click"><i class="fa fa-trash-o"></i>&nbsp;(Del File)</a></span></label>
                                                <table class="table table-condensed">
                                                    <tr>
                                                        <td>
                                                            <input id="FileUpload6" type="file" name="file" runat="server" />
                                                            <a id="hrefFile6" runat="server">
                                                                <asp:Label ID="lblFile6" runat="server"></asp:Label>
                                                            </a>

                                                            <div class="form-group">
                                                                <label for="txtcomment6">Comment </label>
                                                                <asp:TextBox runat="server" ID="txtcomment6" CssClass="form-control" Width="150" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
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
                                    <div class="row">

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="oprninstr">Coolant Used</label>
                                                <asp:TextBox runat="server" ID="txtcoolant" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
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
                                <div class="separator"></div>
                                <label onclick="toggleDivprc()" style="cursor: pointer; color: dodgerblue">Process Parameters</label>
                                <div id="divcontentprcparam" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-12" runat="server" id="divprc">
                                            <div id="divtableprc" class="box-body" style="overflow-x: auto; overflow-y: auto; height: 200px; border: 1px solid #ccc;">
                                                <asp:GridView ID="GridViewprc" runat="server" ClientIDMode="Static" OnRowCommand="GridViewprc_RowCommand"
                                                    CssClass="table table-bordered table-responsive table-hover" UseAccessibleHeader="true"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Serial Number">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialNumber" runat="server" Style="text-align: center"  Text='<%# (Container.DataItemIndex + 1) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Parameter">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtParameter" CssClass="form-control" runat="server" Text='<%# Bind("Parameter") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Parameter Value">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtParamValue" CssClass="form-control" runat="server" Text='<%# Bind("Param_Value") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Checking Method">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCheckingMethod" CssClass="form-control" runat="server" Text='<%# Bind("CheckingMethod") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Checking Frequency">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFrequency" CssClass="form-control" runat="server" Text='<%# Bind("Frequency") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Control Method">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtControl_Method" CssClass="form-control" runat="server" Text='<%# Bind("Control_Method") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnAddNewRowPrc" runat="server" Text="Add New Row" CommandName="AddNewRow" CssClass="btn btn-primary" OnClick="btnAddNewRowPrc_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnDeleteprocess" runat="server" Text="Delete" CommandName="DeleteRow" CssClass="btn btn-danger"  CommandArgument='<%# Container.DataItemIndex %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="separator"></div>
                                <label onclick="toggleDivtooling()" style="cursor: pointer; color: dodgerblue;">Tooling Parameters</label>
                                <div id="divcontentooling" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-12" runat="server" id="div1">
                                            <div id="divtable2" class="box-body" style="overflow: auto; height: 200px; border: 1px solid #ccc;">
                                                <asp:GridView ID="GridView1" runat="server" ClientIDMode="Static" OnRowCommand="GridView1_RowCommand"
                                                    CssClass="table table-bordered table-responsive table-hover" UseAccessibleHeader="true"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="SerialNumber" HeaderText="Operation Sequence" />--%>
                                                        <asp:TemplateField HeaderText="Operation Sequence">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialNumber" runat="server" Style="text-align: center;" Text='<%# (Container.DataItemIndex + 1) %>' Width="50"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operation">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtOperation" CssClass="form-control" runat="server" Text='<%# Bind("Operation") %>' Width="200"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool Holder Name" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtHolderName" CssClass="form-control" runat="server" Text='<%# Bind("tool_holder_name") %>' Width="200"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool/Insert">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtToolInsert" CssClass="form-control" runat="server" Text='<%# Bind("tool") %>' Width="200"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cutting Speed (Rpm)">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCuttingSpeed" CssClass="form-control" runat="server" Text='<%# Bind("cutting_speed") %>' Width="200"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Feed (mm/rev)">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFeed" CssClass="form-control" runat="server" Text='<%# Bind("feed_rate") %>' Width="200"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool Life - Per Corner">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtToolLifePerCorner" CssClass="form-control" runat="server" Text='<%# Bind("per_corner") %>' Width="100"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool Life - No of Corner">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtToolLifeNoOfCorner" CssClass="form-control" runat="server" Text='<%# Bind("no_of_corners") %>' Width="100"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool Life - Total">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtToolLifeTotal" CssClass="form-control" runat="server" Text='<%# Bind("total_nos") %>' Width="100"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Control Method">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtControlMethod" CssClass="form-control" runat="server" Text='<%# Bind("control_method") %>' Width="200"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnAddNewRow" runat="server" Text="Add New Row" CommandName="AddNewRow" CssClass="btn btn-primary" OnClick="btnAddNewRow_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderStyle CssClass="center-header" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnDeletetoolings" runat="server" Text="Delete" CommandName="DeleteRow" CssClass="btn btn-danger"  CommandArgument='<%# Container.DataItemIndex %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
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
        <asp:HiddenField ID="hdnMapSlno" runat="server" />
         <asp:HiddenField ID="hdnTemplate" runat="server" />

        <asp:HiddenField ID="hdnPreviousSlNo_ForCopy" runat="server" Value="0" />

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
      <script src="plugins/select2/select2.min.js"></script>
    <script type="text/javascript">
        function confirmation() {
            return confirm("Are you sure you want to delete this item?");
        }
        $('.myselect').select2({
            theme: "classic"
        });
        function toggleDiv() {
            var div = document.getElementById("divcontent");
            if (div.style.display === "none") {
                div.style.display = "block";
            } else {
                div.style.display = "none";
            }
        }
        function toggleDivprc() {
            var div = document.getElementById("divcontentprcparam");
            if (div.style.display === "none") {
                div.style.display = "block";
            } else {
                div.style.display = "none";
            }
        }
        function toggleDivtooling() {
            var div = document.getElementById("divcontentooling");
            if (div.style.display === "none") {
                div.style.display = "block";
            } else {
                div.style.display = "none";
            }
        }


      <%--  $("document").ready(function () {

            previewFile1();
            previewFile2();
            function previewFile1() {
               console.log("Executing previewFile1");

                var fileInput = document.querySelector('#<%=FileUpload1.ClientID %>');
                var fileNameLabel = document.querySelector('#<%=lblFile1.ClientID %>');

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

             var fileInput2 = document.querySelector('#<%=FileUpload2.ClientID %>');
                var fileNameLabel2 = document.querySelector('#<%=lblFile2.ClientID %>');

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

                    preview2.src = "/Documents/SOP/" + fileNameLabel2.innerText;
                } else { preview2.src = "/dist/img/boxed-bg.jpg";}
 } }); --%>
    </script>
</asp:Content>

