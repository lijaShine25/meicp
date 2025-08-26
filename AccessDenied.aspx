<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AccessDenied.aspx.cs" Inherits="AccessDenied" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <aside class="right-side">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Access Denied
            </h1>

        </section>

        <!-- Main content -->
        <section class="content">

            <div class="error-page">
                <h2 class="headline">403</h2>
                <div class="error-content">
                    <h3><i class="fa fa-frown-o text-yellow"></i>
                        <asp:Label Text="" ID="txtPgTitle" runat="server" /></h3>
                    <p>
                        You do not have permission to view this screen.
                        <br />
                        You may select another screen from the menu.
                    </p>

                </div>
            </div>
            <!-- /.error-page -->

        </section>
        <!-- /.content -->
    </aside>
    <!-- /.right-side -->





</asp:Content>



