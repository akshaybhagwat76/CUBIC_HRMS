<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmDiamond.aspx.cs" Inherits="CUBIC_HRMS.FrmDiamond" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/Diamond/css/style.css" rel="stylesheet" />





</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <h1 class="h3 mb-2 text-gray-800"></h1>
        <p class="mb-1"></p>


        <div class="card shadow mb-4">
            <div class="card-header py-2">
                <h6 class="m-0 input-label-cubic-16" style="color: #4654DF"><i class="fa fa-user"></i>&nbsp;&nbsp; Employee Master </h6>
            </div>

            <div class="card-body">
                <div class="row row-margin-btm-cubic">
                    <div class="form-group col-md-12" style="border: 0px solid">

                        <div class="design-main bg-black">
                            <div class="left-data">
                                <asp:Button class="btn-main" ID="btnNo1" runat="server" OnClick="btn_Number_Click" />
                                <asp:Button class="btn-main" ID="btnNo2" runat="server" OnClick="btn_Number_Click" />
                                <asp:Button class="btn-main borderTop" ID="btnNo3" runat="server" OnClick="btn_Number_Click" />
                            </div>
                            <div class="text-center daimond-main">
                                <div class="btn-data">
                                    <asp:Button class="btn-main" ID="btnNo4" runat="server" OnClick="btn_Number_Click" />
                                    <asp:Button class="btn-main" ID="btnNo5" runat="server" OnClick="btn_Number_Click" />
                                    <div class="one-btn">
                                        <asp:Button class="btn-main" ID="btnNo6" runat="server" OnClick="btn_Number_Click" />
                                    </div>
                                    <asp:Button class="btn-main" ID="btnNo7" runat="server" OnClick="btn_Number_Click" />
                                    <asp:Button class="btn-main" ID="btnNo8" runat="server" OnClick="btn_Number_Click" />

                                </div>

                                <img src="assets/Diamond/Images/daimond.png" alt="daimond" class="daimond-img" />
                                <div class="btn-bottom">
                                    <div class="btn-data2">
                                        <asp:Button class="btn-main" ID="btnNo9" runat="server" OnClick="btn_Number_Click" />
                                        <asp:Button class="btn-main" ID="btnNo10" runat="server" OnClick="btn_Number_Click" />
                                    </div>
                                    <div>
                                        <div class="one-btn design-btn">
                                            <asp:Button class="btn-main" ID="btnNo11" runat="server" OnClick="btn_Number_Click" />
                                        </div>
                                        <asp:Button class="btn-main" ID="btnNo12" runat="server" OnClick="btn_Number_Click" />

                                    </div>
                                </div>
                                <div class="right-data">
                                    <asp:Button class="btn-main" ID="btnNo13" runat="server" OnClick="btn_Number_Click" />
                                    <asp:Button class="btn-main" ID="btnNo14" runat="server" OnClick="btn_Number_Click" />
                                </div>
                            </div>
                            <div class="left-data">
                                <asp:Button class="btn-main" ID="btnNo15" runat="server" OnClick="btn_Number_Click" />
                                <asp:Button class="btn-main" ID="btnNo16" runat="server" OnClick="btn_Number_Click" />
                                <asp:Button class="btn-main borderTop" ID="btnNo17" runat="server" OnClick="btn_Number_Click" />
                            </div>
                            <div class="bottom-main">
                                <div class="bottom-data">

                                    <asp:Button class="btn-main" ID="btnNo18" runat="server" OnClick="btn_Number_Click" />
                                    <asp:Button class="btn-main" ID="btnNo19" runat="server" OnClick="btn_Number_Click" />
                                    <asp:Button class="btn-main mobile-screen" ID="btnNo20" runat="server" OnClick="btn_Number_Click" />
                                </div>
                                <div class="text-center hook-main">
                                    <img src="assets/Diamond/Images/hook.png" alt="hook" />
                                </div>
                            </div>
                            <div class="bottom-btn">
                                <asp:Button class="btn-main" ID="btnNo21" runat="server" OnClick="btn_Number_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
