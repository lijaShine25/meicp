<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="Form1" role="form" runat="server">
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Welcome to Control Plan Software
                </h1>
            </section>
            <section class="content">
                <div class="row">
                    <div class="col-md-12">
                        <span id="pending" runat="server" class="text-capitalize text-blue">
                           <i class=" fa fa-bell fade"></i>
                            You have pending for approvals. <br />
                            <a href="PendingApproval.aspx">Click here to open</a>
                        </span>
                    </div>
                </div>
            </section>
        </div>
       
    </form>
</asp:Content>

