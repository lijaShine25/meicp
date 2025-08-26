<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PartDocuments.aspx.cs" Inherits="PartDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Part Documents
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
                    <div class="col-md-3">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group has-success">
                                    <label for="ddlpart"><i class="fa fa-check-circle"></i>&nbsp;Part</label>
                                    <asp:DropDownList runat="server" ID="ddlpart" CssClass="form-control myselect" AutoPostBack="true" OnSelectedIndexChanged="ddlparts_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Data Missing!" ControlToValidate="ddlpart" runat="server"
                                        CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories" InitialValue="0" />
                                </div>
                                <h4>Part Details</h4>
                                <table class="table table-bordered table-condensed text-sm">
                                    <tr>
                                        <th>Part Number</th>
                                        <td>
                                            <asp:Label ID="lblpartno" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Part Issue No.</th>
                                        <td>
                                            <asp:Label runat="server" ID="lblpartissno"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Part Issue Dt.</th>
                                        <td>
                                            <asp:Label runat="server" ID="lblpartissdt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Customer Name</th>
                                        <td>
                                            <asp:Label runat="server" ID="lblcustname"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Custome Part No.</th>
                                        <td>
                                            <asp:Label runat="server" ID="lblcustpartno"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                        </div>
                    </div>
                    <div class="col-md-9">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="doc title"><i class="fa fa-check-circle"></i>Doc.Title</label>
                                    <asp:TextBox runat="server" ID="txtdoctitle" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvtxtdoctitle" ErrorMessage="Data Missing!" ControlToValidate="txtdoctitle" runat="server"
CssClass="label label-danger" Display="Dynamic" ValidationGroup="mandatories"  />
                                </div>
                                <div class="form-group">
                                    <label for="upload file">File to Upload</label>
                                    <asp:FileUpload ID="uploadfile1" runat="server"></asp:FileUpload>
                                    <a id="hrefrcFile1" href="#"  runat="server" >
                                                    <asp:Label ID="lbluploadfile1" runat="server"></asp:Label></a> 
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-primary" ValidationGroup="mandatories" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-danger" OnClick="btnDelete_Click" Enabled="false" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-9">
                        <div class="box box-info">
                            <div class="box-header">
                                <h3 class="box-title">List of Part Documents</h3>
                                <div class="box-tools">
                                    <asp:Button runat="server" ID="btnclearFilter" CssClass="btn-sm btn-warning" Text="Clear Filter" OnClick="btnclearFilter_Click" />
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView runat="server" ClientIDMode="Static" 
                                    CssClass="table table-bordered table-responsive table-hover" ID="grdData" UseAccessibleHeader="true"
                                    ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-BorderColor="SkyBlue" OnRowDataBound="grdData_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="mstPartNo" HeaderText="Part Number" />
                                        <asp:BoundField DataField="PartDescription" HeaderText="Part Description" />
                                        <asp:BoundField DataField="partIssueNo" HeaderText="Part Issue No." />
                                         <asp:BoundField DataField="partIssueDt" HeaderText="Part Issue Date" />      
                                        <asp:HyperLinkField DataNavigateUrlFields="part_doc_slno" HeaderText="Doc.Title"
                                            DataNavigateUrlFormatString="~/PartDocuments.aspx?slno={0}"
                                            ItemStyle-Font-Underline="true" DataTextField="doc_title" />  
                                        <asp:TemplateField HeaderText="Doc.FileName">
                                            <ItemTemplate>
                                                <asp:Literal Text="" ID="ltlfilename" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Name" ItemStyle-CssClass="hidden" ControlStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Bind("doc_filename")%>' ID="lblfname1" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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


