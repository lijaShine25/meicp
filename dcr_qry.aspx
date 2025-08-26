<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="dcr_qry.aspx.cs" Inherits="dcr_qry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>DCR Query
                <small class="pull-right">
                    <a href="dcr.aspx" class="btn btn-sm bg-orange" title="Go Back.."><i class="fa fa-backward"></i></a>
                </small>
                </h1>
            </section>
            <style>
    .reason-column {
        width: 180px; /* Set your desired width */
        /*white-space: nowrap;*/ /* Prevent text from wrapping */
        overflow: hidden; /* Hide overflow text */
        text-overflow: ellipsis; /* Add ellipsis for overflowed text */
    }
</style>
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
                                        <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlpart_slno_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtOperation">Operation</label>
                                        <asp:DropDownList runat="server" ID="ddloperation_slno" CssClass="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <br />
                                    <asp:Button ID="btnViewRptHistory" Text="View Report" runat="server"
                                        CssClass="btn btn-sm btn-warning" ValidationGroup="mandatories"
                                        OnClick="btnViewRptHistory_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <div style="overflow-x: auto; width: 100%;height:500px">
                                <asp:GridView runat="server" ClientIDMode="Static"
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="dcr_slno" HeaderText="DCR Number"
                                            DataNavigateUrlFormatString="~/dcr.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="dcr_number"  />
                                        <asp:BoundField DataField="partDescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="mstpartno" HeaderText="Part No." />
                                          <asp:BoundField DataField="OperationDesc" HeaderText="Operation Desc" />
                                       <%-- <asp:HyperLinkField DataNavigateUrlFields="dcr_slno" HeaderText="Operation Desc"
                                            DataNavigateUrlFormatString="~/dcr.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="OperationDesc" />--%>
                                    <asp:BoundField DataField="Existing" HeaderText="Existing" />
                                         <asp:BoundField DataField="Changes_Required" HeaderText="Changes Required" />
                                      <asp:BoundField DataField="Reason_For_Change" HeaderText="Reason For Change" ItemStyle-CssClass="reason-column"  />
                                        
                                        <asp:BoundField DataField="change_area" HeaderText="Change Area" />
                                         <asp:BoundField DataField="employeename" HeaderText="Request By" />
                                        <asp:BoundField DataField="Submit_Status" HeaderText="Submit Status" />
                                         <asp:BoundField DataField="DCR_Submit_DateTime" HeaderText="Submit Date & Time" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
                                         <asp:BoundField DataField="DCR_Approved_DateTime" HeaderText="Approved Date & Time" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
                                    </Columns>
                                </asp:GridView>
                                       </div>
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

