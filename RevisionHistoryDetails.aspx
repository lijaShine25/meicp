<%@ Page MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true" CodeFile="RevisionHistoryDetails.aspx.cs" Inherits="RevisionHistoryDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Revision History
                </h1>
            </section>

            <section class="content">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box no-border no-header">
                            <div class="box-body">
                                <div class="col-md-6">
                                    <div class="form-group has-success">
                                        <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part# </label>
                                        <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" >
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />

                                    </div>
                                </div>
                                <div class="col-md-3 clearfix">
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="btnViewRptHistory" Text="View Report" runat="server"
                                            CssClass="btn btn-sm btn-warning" ValidationGroup="mandatories"
                                            OnClick="btnViewRptHistory_Click" />
                                        <asp:Button ID="btnExportXL" Text="Export to Excel" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnExportXL_Click" ValidationGroup="mandatories" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <asp:GridView runat="server" ID="grdData" CssClass="table table-bordered" 
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" EmptyDataText="No Records Found" OnPreRender="grdData_PreRender"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue" ShowFooter="false" CellSpacing="0" Width="100%">

                                    <Columns>

                                        <asp:BoundField DataField="mstPartNo" HeaderText="Part Number" />
                                        <asp:BoundField DataField="PartDescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="rev_no" HeaderText="Revision Number" />
                                        <asp:BoundField DataField="rev_date" HeaderText="Revision Date" />
                                        <asp:BoundField DataField="change_nature" HeaderText="Nature of Change" />
                                        <asp:BoundField DataField="rev_reasons" HeaderText="Revision Reason" />

                                    </Columns>
                                </asp:GridView>


                            </div>
                        </div>
                    </div>
                </div>

            </section>
        </div>
        <asp:HiddenField ID="hdnSlNo" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" Value="I" />

    </form>
    <script>
        $('.myselect').select2({
            theme: "classic"
        });
    </script>
</asp:Content>

