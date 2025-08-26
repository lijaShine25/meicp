<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="Rptcontrolplan.aspx.cs" Inherits="Rptcontrolplan" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <div class="content-wrapper">
                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                        <h1>Print Report
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
                        <div class="box box-info">
                        <div class="box-body">
                        <div class="form-group ">
                                            <asp:UpdatePanel ID="up1" runat="server">

                                                <ContentTemplate>
                                                    <table class="table table-bordered table-condensed">
                                                        <tr>
                                                            <td style="vertical-align: middle;">
                                                                <div class="form-group has-success">
                                                                    <label for="txtPartNo"><i class="fa fa-check-circle"></i>&nbsp;Part# </label>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlpart_slno" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpart_slno_OnSelectedIndexChanged">
                                                                    <asp:ListItem Text="Select..." />
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart_slno" runat="server" InitialValue="Select..."
                                                                    CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />

                                                            </td>
                                                            <td style="vertical-align: middle;">
                                                                <div class="form-group has-success">
                                                                    <label for="txtOperation"><i class="fa fa-check-circle"></i>&nbsp;Operation</label>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddloperation_slno" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRevNo_OnSelectedIndexChanged">
                                                                    <asp:ListItem Text="Select..." />
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Data Missing!" ControlToValidate="ddloperation_slno" runat="server" InitialValue="Select..."
                                                                    CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" />

                                                            </td>
                                                            <td style="vertical-align: middle;">
                                                                <label for="txtOperation"><i class="fa fa-check-circle"></i>&nbsp;Rev.No</label>                                                              
                                                            </td>
                                                            <td style="vertical-align: middle;">
                                                                <asp:DropDownList runat="server" ID="ddlRevNo" CssClass="form-control">
                                                                    <asp:ListItem Text="Latest" Value="0" />
                                                                </asp:DropDownList>
                                                            </td>
                                                           <%-- <td style="vertical-align: middle;">
                                                                <asp:RadioButton ID="rbControlPlanRpt" runat="server" Text="Control Plan Report" GroupName="cp" Checked="true" AutoPostBack="true" /></td>
                                                            <td style="vertical-align: middle;">
                                                                <asp:RadioButton ID="rbLineInspecRrt" runat="server" Text="Line Inspection Report" GroupName="cp" AutoPostBack="true" /></td>
                                            --%>            </tr>
                                                        <tr>
                                                            <td colspan="6">
                                                                <asp:Label Text="" runat="server" ID="lblMessage" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                       </div>
                                     </div>
                                   </div>
                                 </div>

                                  
                                    <div class="box-footer">
                                       <%-- <asp:Button ID="ViewRpt" Text="ViewReport" runat="server" CssClass="btn btn-primary" OnClick="btnViewReport_Click" ValidationGroup="mandatories" />--%>
                                   &nbsp
                                         <asp:Button ID="btnExportXL" Text="ExportToExcel" runat="server" CssClass="btn btn-primary"  ValidationGroup="mandatories" OnClick="btnExportXL_Click" />
                                  
                                         </div>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">



                                    <rsweb:ReportViewer ID="ReportViewer1" Visible="False" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="538px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="950px" CssClass="table-responsive">
                                        <LocalReport ReportPath="RptControlPlan2.rdlc">
                                            <DataSources>
                                                <%--<rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />--%>
                                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                                            </DataSources>
                                        </LocalReport>
                                    </rsweb:ReportViewer>

                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="mstl_controlplanDataSetTableAdapters.Temp_RptControlPlanTableAdapter"></asp:ObjectDataSource>

                                </div>
                            </div>
                        </div>
                    </section>

                </div>
                  <asp:HiddenField ID="hdnemplslno" runat="server" />
                <asp:HiddenField ID="hdnMode" runat="server" Value="I" />
                <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />
            </div>
        </form>
    </body>
    </html>
</asp:Content>
