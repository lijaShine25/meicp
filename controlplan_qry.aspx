<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="controlplan_qry.aspx.cs" Inherits="controlplan_qry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Control Plan Query
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
                                <asp:GridView runat="server" ClientIDMode="Static"
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue">
                                    <Columns>

                                        <asp:HyperLinkField DataNavigateUrlFields="cp_slno" HeaderText="Control Plan#"
                                            DataNavigateUrlFormatString="~/ControlPlan2.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="cp_slno" Visible="false" />


                                        <asp:BoundField DataField="rev_no" HeaderText="Revision#" Visible="false" />
                                        <%--  <asp:BoundField DataField="mstPartNo" HeaderText="Part" />--%>

                                        <%-- <asp:BoundField DataField="machine_slno" HeaderText="Machine Sl.NO" />
                                        <asp:BoundField DataField="operation_slno" HeaderText="Operation Sl. No." />--%>

                                        <%--<asp:TemplateField HeaderText="Part Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpart" runat="server" Text='<%# GetPartDescription(Eval("part_slno").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:BoundField DataField="mstpartno" HeaderText="Part No." />
                                         <asp:BoundField DataField="process_no" HeaderText="Operation No." />
                                        <asp:HyperLinkField DataNavigateUrlFields="cp_slno" HeaderText="Operation Desc"
                                            DataNavigateUrlFormatString="~/ControlPlan2.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="operationdesc" />

                                        <%--<asp:TemplateField HeaderText="Operation Desc">                                        
                                        <ItemTemplate>
                                        <asp:Label ID="lblop" runat="server" Text='<%# GetOpDescription(Eval("operation_slno").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>--%>


                                        <%--<asp:TemplateField HeaderText="Machine Desc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmch" runat="server" Text='<%# GetMcDescription(Eval("machine").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       
                                        <asp:BoundField DataField="machinedesc" HeaderText="Machine Desc." />
                                        <asp:BoundField DataField="ApproveStatus" HeaderText="Approve Status" />

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

