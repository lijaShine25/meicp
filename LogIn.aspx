<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="LogIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>Control Plan | MEI</title>
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport'>
    <!-- bootstrap 3.0.2 -->
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- font Awesome -->
    <link href="css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Theme style -->
    <link href="css/AdminLTE.css" rel="stylesheet" type="text/css" />

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
          <script src="js/html5shiv.js"></script>
          <script src="js/respond.min.js"></script>
        <![endif]-->

</head>
<body>
    <div class="form-box" id="login-box">
        <div class="header">Sign In</div>
        <form runat="server">
            <div class="body bg-gray">
                <div class="form-group">
                    <input type="text" name="userid" class="form-control" placeholder="User ID" runat="server" id="txtuser" />
                     <asp:RequiredFieldValidator runat="server" id="reqMachID" controltovalidate="txtuser" 
                                    errormessage="Please enter User Name!" Display="Dynamic" CssClass="label label-danger" ValidationGroup="Save"/>
                </div>
                <div class="form-group">
                    <input type="password" name="password" class="form-control" placeholder="Password" runat="server" id="txtpassword" />
                     <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="txtpassword" 
                                    errormessage="Please enter PassWord!" Display="Dynamic" CssClass="label label-danger" ValidationGroup="Save"/>
                </div>

            </div>
            <div class="footer">
                <asp:Button Text="Sign me in" runat="server" ID="btnsubmit"  ValidationGroup="Save"
                    class="btn btn-primary btn-block" onclick="btnsubmit_Click1" />
                <p>
                    <asp:Label ID="lblmsg" runat="server"></asp:Label></p>

                <%--                     
                    <p><a href="#">I forgot my password</a></p>
                    
                    <a href="register.html" class="text-center">Register a new membership</a>
                --%>
            </div>
        </form>

        <%--            <div class="margin text-center">
                <span>Sign in using social networks</span>
                <br/>
                <button class="btn bg-light-blue btn-circle"><i class="fa fa-facebook"></i></button>
                <button class="btn bg-aqua btn-circle"><i class="fa fa-twitter"></i></button>
                <button class="btn bg-red btn-circle"><i class="fa fa-google-plus"></i></button>

            </div>
        --%>
    </div>


    <!-- jQuery 2.0.2 -->
    <script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>
    <!-- Bootstrap -->
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>
</body>
</html>
