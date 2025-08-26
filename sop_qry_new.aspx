<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sop_qry_new.aspx.cs" Inherits="sop_qry_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>SOP Query
                <small class="pull-right">



                    <a href="ControlPlan2.aspx" class="btn btn-sm bg-orange" title="Go Back.."><i class="fa fa-backward"></i></a>

                </small>
                </h1>
            </section>

            <section class="content">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>

                <div class="row">
                    <div class="col-md-12">
                        <div class="box no-border no-header">
                            <div class="box-body">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtPartNo">Part# </label>
                                        <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlpart_slno_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                            CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />--%>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtOperation">Operation</label>
                                        <asp:DropDownList runat="server" ID="ddloperation_slno" CssClass="form-control myselect">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="ddlGroup">Group</label>
                                        <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control myselect">
                                            <asp:ListItem Text="Select..." />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <br />
                                    <asp:Button ID="btnViewRptHistory" Text="View Report" runat="server"
                                        CssClass="btn btn-sm btn-warning" ValidationGroup="mandatories"
                                        OnClick="btnViewRptHistory_Click" />
                                      <asp:Button ID="btnRefresh" Text="Refresh" runat="server"
      CssClass="btn btn-sm btn-success" ValidationGroup="mandatories"
      OnClick="btnRefresh_Click" />
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
                                    <Columns>

                                        <asp:HyperLinkField DataNavigateUrlFields="sop_id" HeaderText="SOP ID#"
                                            DataNavigateUrlFormatString="~/sop_new.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="sop_id" Visible="false" />



                                        <asp:BoundField DataField="mstpartno" HeaderText="Part No." />
                                        <asp:TemplateField HeaderText="Operation Desc">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlOperationDesc" runat="server"
                                                    Text='<%# Eval("operationdesc") %>'
                                                    NavigateUrl='<%# "~/sop_new.aspx?slno=" + Eval("sop_id") + "&template=" + Eval("Template") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:HyperLinkField DataNavigateUrlFields="sop_id" HeaderText="Operation Desc"
                                            DataNavigateUrlFormatString="~/sop_new.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="operationdesc" />--%>
                                        <asp:BoundField DataField="Template" HeaderText="Template" />
                                        <asp:BoundField DataField="Group_Name" HeaderText="Group" />
                                        <asp:BoundField DataField="machinedesc" HeaderText="Machine Desc." />
                                        <asp:BoundField DataField="objective" HeaderText="Objective" />
                                        <asp:BoundField DataField="oprn_instruction" HeaderText="Oprn. Instruction" />
                                        <asp:BoundField DataField="coolant_used" HeaderText="Coolant" />
                                        <asp:BoundField DataField="ApprovedStatus" HeaderText="Approved Status" />
                                         <%--<asp:BoundField DataField="PreparedBy" HeaderText="Prepared By" />
                                         <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" />--%>
                                         <asp:BoundField DataField="Active" HeaderText="Active / Inactive " />

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

