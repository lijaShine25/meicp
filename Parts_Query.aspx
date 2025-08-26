<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Parts_Query.aspx.cs" Inherits="Parts_Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Parts List
                <small class="pull-right">
                    <asp:Button ID="btnViewRptHistory" Text="View Report" runat="server"
                        CssClass="btn btn-sm btn-warning" ValidationGroup="mandatories"
                        onclick="btnViewRptHistory_Click" />
                    <a href="Parts.aspx" class="btn btn-sm bg-orange" title="Go Back.."><i class="fa fa-backward"></i></a>
                </small>
                </h1>
            </section>
            <section class="content">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part# </label>
                                        <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" >
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="status">Acitve/ InActive</label>
                                        <asp:DropDownList runat="server" CssClass="form-control" id="ddlstatus" >
                                            <asp:listitem text="All" value="" />
                                            <asp:listitem text="Active" value="N" selected="true" />
                                            <asp:listitem text="InActive" value="Y" />
                                        </asp:DropDownList>
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
                                <asp:GridView runat="server" ClientIDMode="Static"
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="part_slno" HeaderText="Part Number"
                                            DataNavigateUrlFormatString="~/Parts.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="mstPartNo" />
                                        <asp:BoundField DataField="PartDescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="partIssueNo" HeaderText="Part Issue No." />
                                        <asp:BoundField DataField="partIssueDt" HeaderText="Part Issue Dt." />
                                        <asp:BoundField DataField="customerPartNo" HeaderText="Customer Part No." />
                                        <asp:BoundField DataField="customerIssueNo" HeaderText="Customer Issue No." />
                                        <asp:BoundField DataField="customerIssueDt" HeaderText="Customer Issue Dt." />
                                        <asp:BoundField DataField="keyContact" HeaderText="Key Contact" />
                                        <asp:BoundField DataField="CFTeamName" HeaderText="CF Team Name" />
                                        <asp:BoundField DataField="del_status" HeaderText="Active / In-Active" />
                                        <asp:BoundField DataField="status1" HeaderText="Active / In-Active" Visible="false" />
                                    </columns>
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

