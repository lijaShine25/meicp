<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    

    <link href=" bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <script src="js/jquery-2.1.4.min.js"></script>

    <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css" rel="stylesheet" />
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.0.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>--%>

    <style>
        body {
            background: url(dist/img/wallpaper.jpg) no-repeat center center fixed;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
        }

        .panel-default {
            opacity: 0.9;
            margin-top: 30px;
        }

        .form-group.last {
            margin-bottom: 0px;
        }
    </style>

</head>
<body>
    <form class="form-horizontal" role="form" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <span class="glyphicon glyphicon-lock"></span> Control Plan Login
                    </div>
                    <div class="panel-body">
                        
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-3 control-label">
                                    User Id.</label>
                                <div class="col-sm-9">
                                    <input type="text" name="userid" class="form-control" placeholder="User ID" runat="server" id="txtuser" required />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-3 control-label">
                                    Password</label>
                                <div class="col-sm-9">
                                    <input type="password" name="password" class="form-control" placeholder="Password" runat="server" id="txtpassword" required />
                                </div>
                            </div>

                            <div class="form-group last">
                                <div class="col-sm-offset-3 col-sm-9">
                                    <asp:Button Text="Sign in" runat="server" ID="btnsubmit" ValidationGroup="Save"
                                        class="btn btn-success btn-sm" OnClick="btnsubmit_Click1" />
                                </div>
                            </div>
                        
                    </div>
                    <div class="panel-footer"><asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </form>
</body>
</html>
