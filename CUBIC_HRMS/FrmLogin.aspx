<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLogin.aspx.cs" Inherits="CUBIC_HRMS.FrmLogin" %>

<%--//// css vs min.css 
after done css, can use cssminifier.com to do the convertion   
Where bootstrap.css is the development version and bootstrap.min.css is production.
The main purpose of using bootstrap.min.css is to reduce the size of the file(style) that boosts the website speed.
 if the css need run from server, then we need att runat="server, else the css will not able to link"
all JS and CSS is crazy important
 --%>


<!-- Custom styles for this template -->
<style>
    #txtUsername::placeholder {
        color: dimgrey;
    }

    #txtPassword::placeholder {
        color: dimgrey;
    }

    body {
        /*background: linear-gradient(to right, #6d99f1, white);*/ /* Gradient from light purple to white */
        background: linear-gradient(to right, #354152, #354152); /* Gradient from light purple to white */
        color:#7e8ba3;
        display: flex;
        flex-direction: column;
        min-height: 100vh;
    }

    /*.container {
        background-color: white;*/ /* White background for the container to contrast with the page */
        /*border-radius: 15px;*/ /* Rounded corners for the container */
        /*box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.05);*/ /* Shadow for a slight 3D effect */
        /*flex-grow: 1;
    }*/

    .register {
        box-shadow: 0 0 250px #000;
    }

    .form-border {
        border: none; /* Remove border */
        background: #f8f9fa; /* Light background for the form */
        padding: 20px; /* Padding around the form */
        border-radius: 10px; /* Rounded corners for the form */
        box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.5); /* Shadow for a slight 3D effect */
    }

    .footer-admin {
        background-color: #343a40; /* Dark background for the footer */
        color: white; /* White text for the footer */
    }

    .footer-admin a {
        color: #adb5bd; /* Light grey color for the links in the footer */
    }
</style>

<script>
    document.title = "HRMS Login Page";
</script>


<!doctype html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">


    <link href="css/sb-admin-2-Pro.css" rel="stylesheet" />
    <link rel="icon" type="image/x-icon" href="assets/img/favicon.png" />
    <script data-search-pseudo-elements defer src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/js/all.min.js" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/feather-icons/4.29.0/feather.min.js" crossorigin="anonymous"></script>


    <title>Login - HRMS</title>
</head>

<body>
    <div class="container py-5 h-100">

        <%--<div class="register">--%>
            <div class="row d-flex align-items-center justify-content-center h-100">
                <div class="col-md-7 col-lg-5 col-xl-5 ">
                    <div class="d-flex align-items-center justify-content-center mb-4">
                        <img src="Image/CubicLogo.png" class="img-fluid mr-2" alt="Phone image" width="120" height="120">
                    </div>

                    <div class="d-flex align-items-center justify-content-center mb-4">
                        <h2 style="font-size: 30px; color: #7e8ba3">HRMS - Sign In</h2>
                    </div>

                    <div class="form-border">
                        <form method="post" class="form" runat="server">
                            <!-- Email input -->
                            <div class="form-outline mb-4">
                                <label class="form-label" for="txtUsername">User Name</label>
                                <asp:TextBox ID="txtUsername" runat="server" class="form-control form-control-lg" Font-Size="14px"></asp:TextBox>
                            </div>

                            <!-- Password input -->
                            <div class="form-outline mb-4">
                                <label class="form-label" for="txtPassword">Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" placeholder="••••••••••••" Font-Size="14px" TextMode="Password" class="form-control form-control-lg"></asp:TextBox>
                            </div>

                            <div class="d-flex justify-content-start mb-4">
                                <!-- Checkbox -->
                                <div class="form-check ">
                                    <asp:CheckBox ID="ChkRememberMe" runat="server" Checked="true" />
                                    <label class="form-check-label" for="ChkRememberMe">Remember me </label>

                                </div>
                                <a href="#!" hidden>Forgot password?</a>
                            </div>

                            <!-- Submit button -->
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnLogin_Click" type="submit" Text="Sign In" class="btn btn-primary btn-lg btn-block w-100" Style="background-color: #38d39f; border-color: #38d39f" />
                        </form>
                    </div>
                </div>
            </div>
        <%--</div>--%>
    </div>

    <footer class="footer-admin mt-auto footer-dark">
        <div class="container-xl px-4">
            <div class="row">
                <div class="col-md-6 small" style="color: grey;">Copyright © CubicSoftware Solution Sdn Bhd. All Right Reserved. 2019 - <%: DateTime.Now.Year %> . v2.0.2</div>
                <div class="col-md-6 text-md-end small" style="color: grey;">
                    <a href="#!">Privacy Policy</a>
                    ·
                               
                    <a href="#!">Terms &amp; Conditions</a>
                </div>
            </div>
        </div>
    </footer>

</body>
</html>

