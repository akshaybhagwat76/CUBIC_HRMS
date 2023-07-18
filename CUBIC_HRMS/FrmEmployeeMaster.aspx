<%@ Page Title="Employee Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmEmployeeMaster.aspx.cs" Inherits="CUBIC_HRMS.FrmEmployeeMaster" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/SubSite.css" rel="stylesheet" type="text/css" media="screen" runat="server" />


    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="js/dataTables.js"></script>
    <script src="js/dataTables.bootstrap4.js"></script>
    <script src="assets/demo/datatables-demo.js"></script>


    <script>

        function numericOnly(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;

            // Un-comment to discover a key that I have forgotten to take into account...
            //alert(keyCodeEntered);

            if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {
                return true;
            }
            // '+' sign...
            else if (keyCodeEntered == 43) {
                // Allow only 1 plus sign ('+')...
                if ((elementRef.value) && (elementRef.value.indexOf('+') >= 0))
                    return false;
                else
                    return true;
            }
            // '-' sign...
            //else if (keyCodeEntered == 45) {
            //    // Allow only 1 minus sign ('-')...
            //    if ((elementRef.value) && (elementRef.value.indexOf('-') >= 0))
            //        return false;
            //    else
            //        return true;
            //}
            // '.' decimal point...
            else if (keyCodeEntered == 46) {
                // Allow only 1 decimal point ('.')...
                if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
                    return false;
                else
                    return true;
            }

            return false;
        }


        function closeModal() {
            $("#ModalMessage").hide();
            $('.modal-backdrop').remove();
            $(document.body).removeClass('modal-open');
            document.body.style.overflow = "scroll";
        }

        function OpenModal(status, message) {
            $("#ModalConfirmation").modal('hide');
            var title = document.getElementById('MessageModalTitle');
            var content = document.getElementById('MessageModalContent');
            title.innerHTML = status;
            content.innerHTML = message;
            $('#ModalMessage').modal('show');
        }
    </script>

     <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container-fluid">

        <h1 class="h3 mb-2 text-gray-800"></h1>
        <p class="mb-1"></p>


        <div class="card shadow mb-4">
            <div class="card-header py-2">
                <h6 class="m-0 input-label-cubic-16" style="color: #4654DF"><i class="fa fa-user"></i>&nbsp;&nbsp; Employee Master </h6>
            </div>

            <div class="card-body">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="#813838" ShowModelStateErrors="True" BackColor="#FEE2E1" Font-Names="Segoe UI Symbol" EnableTheming="True" EnableClientScript="True" />


                <div class="row row-margin-btm-cubic">
                    <div class="form-group col-md-12" style="border: 0px solid">

                        <%-- MultiView --%>
                        <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
                            <asp:View ID="View1" runat="server">
                                <%-- Start DIV --%>
                                <div class="container form-group">

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-11">
                                            <div style="overflow: auto">
                                                <asp:Image ID="EmpPImage" runat="server" Height="132px" ImageAlign="Right" ImageUrl="~/Files/ProfilePicture.jpg" Width="149px" />
                                            </div>
                                        </div>
                                    </div>


                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblSelectEmpCode" runat="server" Text="Employee Code *" class="input-label-cubic-14"></asp:Label>
                                            <asp:DropDownList ID="ddlSelectEmpCode" runat="server" AutoPostBack="true" class="form-control drop-down-cubic-14" OnSelectedIndexChanged="ddlSelectEmpCode_SelectedIndexChanged"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-1">
                                        </div>

                                        <div class="form-group col-md-5" runat="server" id="trtxtEmpCode">
                                            <asp:Label ID="lblEmpCodetxt" runat="server" Text="Employee Code" class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtEmpCode" runat="server" class="form-control input-text-cubic-12" ReadOnly="true" BackColor="WhiteSmoke" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblDepartment" runat="server" Text="Department *" class="input-label-cubic-14"></asp:Label>
                                            <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="false" class="form-control drop-down-cubic-14" Style="margin-top: 5px" ValidateRequestMode="Enabled"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RequiredFieldValidator ID="vDepartment" runat="server" ControlToValidate="ddlDepartment" ErrorMessage="Please select Department" ForeColor="Red" BorderStyle="None">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>



                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblFirstName" runat="server" Text="First Name * " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtFirstName" runat="server" class="form-control input-text-cubic-12" placeholder=" Enter First Name"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-md-1">
                                            <asp:RequiredFieldValidator ID="vFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="Please enter a First Name" ForeColor="Red" BorderStyle="None">*</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblLastName" runat="server" Text="Last Name *" class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtLastName" runat="server" class="form-control input-text-cubic-14" placeholder=" Enter Last Name"></asp:TextBox>

                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RequiredFieldValidator ID="vLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Please enter a Last Name" ForeColor="Red" BorderStyle="None">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>


                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblNickName" runat="server" Text="Nick Name *" class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtNickName" runat="server" class="form-control input-text-cubic-12" placeholder=" Enter Nick Name"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RequiredFieldValidator ID="vNickName" runat="server" ControlToValidate="txtNickName" ErrorMessage="Please enter a Nick Name" ForeColor="Red" BorderStyle="None">*</asp:RequiredFieldValidator>
                                        </div>

                                        <div class="form-group col-md-4">
                                            <asp:Label ID="lblGender" runat="server" Text="Gender *" class="input-label-cubic-14"></asp:Label>
                                            <asp:RadioButtonList ID="rdGender" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Flow" class="form-control input-radio-cubic-20" Style="font-size: 14px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; border: 0">
                                                <asp:ListItem Selected="True" Style="margin-right: 10px;">Female</asp:ListItem>
                                                <asp:ListItem Style="margin-left: 10px;">Male</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblPosTitle" runat="server" Text="Position Title " class="input-label-cubic-14"></asp:Label>
                                            <asp:DropDownList ID="ddlPositionTitle" runat="server" AutoPostBack="false" class="form-control drop-down-cubic-14"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblDOB" runat="server" Text="DOB *" class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="dtDOB" ClientIDMode="Static" runat="server" TextMode="Date" class="form-control input-text-cubic-12"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RequiredFieldValidator ID="vDOB" Text="*" ErrorMessage="Please select DOB" ControlToValidate="dtDOB" runat="server" ForeColor="Red" />

                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblICNo" runat="server" Text="IC No. " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtICNo" runat="server" class="form-control input-text-cubic-12" placeholder=" Enter IC No."></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblEmpEmail" runat="server" Text="Email " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtEmpEmail" runat="server" class="form-control input-text-cubic-12"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmpEmail" ErrorMessage="Invalid Employee Email Format" ForeColor="Red" BorderStyle="None">*</asp:RegularExpressionValidator>

                                        </div>
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblEmpContact" runat="server" Text="Contact No" class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtEmpContact" runat="server" class="form-control input-text-cubic-12"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblCitizen" runat="server" Text="Citizen *" class="input-label-cubic-14"></asp:Label>
                                            <asp:DropDownList ID="ddlCitizen" runat="server" class="form-control drop-down-cubic-14"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblEmpCategory" runat="server" Text="Employee Category *" class="input-label-cubic-14"></asp:Label>

                                            <asp:DropDownList ID="ddlEmpCategory" runat="server" AutoPostBack="false" class="form-control drop-down-cubic-14" ValidateRequestMode="Enabled">
                                                <asp:ListItem>Exempt</asp:ListItem>
                                                <asp:ListItem>Non-Exempt</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblDirectSuperior" runat="server" Text="Direct Superior " class="input-label-cubic-14"></asp:Label>
                                            <asp:DropDownList ID="ddlSelectDSuperior" runat="server" AutoPostBack="true" class="form-control drop-down-cubic-14" OnSelectedIndexChanged="ddlSelectDSuperior_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-1">
                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblDirectSuperiorName" runat="server" Text="Direct Superior Name " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtDirectSuperiorName" runat="server" class="form-control input-text-cubic-12" ReadOnly="true" BackColor="WhiteSmoke"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblOnBehalf" runat="server" Text="Approve On Behalf " class="input-label-cubic-14"></asp:Label>

                                            <asp:DropDownList ID="ddlSelectOnBehalf" runat="server" AutoPostBack="true" class="form-control drop-down-cubic-14" OnSelectedIndexChanged="ddlSelectOnBehalf_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-1">
                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblOnBehalfName" runat="server" Text="Approve On Behalf Name" class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtOnBehalfName" runat="server" class="form-control input-text-cubic-12" ReadOnly="true" BackColor="WhiteSmoke"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblAccessCard" runat="server" Text="Access Card " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtAccessCard" runat="server" class="form-control input-text-cubic-12" placeholder=" Enter Access Card"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status " class="input-label-cubic-14"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" class="form-control drop-down-cubic-14">
                                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%-- ** DOCUMENT NEEDED --%>
                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-12">
                                            <div class="card-header py-2">
                                                <h6 class="m-0 input-label-cubic-14" style="color: #4654DF"><i class="fa fa-file"></i>&nbsp;&nbsp; Document Needed </h6>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblIncomeTaxNo" runat="server" Text="Income Tax No. " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtIncomeTaxNo" runat="server" class="form-control input-text-cubic-12" placeholder="Enter Income Tax No."></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-1">
                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblSocsoNo" runat="server" Text="Socso No. " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtSocsoNo" runat="server" class="form-control input-text-cubic-12" placeholder="Enter Socso No."></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblEPF" runat="server" Text="EPF No. " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtEPFNo" runat="server" class="form-control input-text-cubic-12" placeholder="Enter EPF No."></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-1">
                                        </div>
                                    </div>


                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblResume" runat="server" Text="Resume (Pdf Only)  " class="input-label-cubic-14"></asp:Label>
                                            <asp:FileUpload ID="FileUploadResumeDoc" runat="server" class="form-control" />
                                            <asp:LinkButton ID="LBtnResumeDoc" runat="server" OnClick="LBtnResumeDoc_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only PDF files are allowed for Passport" ValidationExpression="^.*\.(pdf|PDF)$" ControlToValidate="FileUploadResumeDoc" CssClass="text-red" ForeColor="Red">*</asp:RegularExpressionValidator>
                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblOfferLetter" runat="server" Text="Offer Letter Document (Pdf only) " class="input-label-cubic-14"></asp:Label>
                                            <asp:FileUpload ID="FileUploadOfferLetter" runat="server" class="form-control custom-file-input" />
                                            <asp:LinkButton ID="LBtnOLP" runat="server" OnClick="LBtnOLP_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RegularExpressionValidator ID="vFileUploadOfferLetter" runat="server" ErrorMessage="Only PDF files are allowed for Offer Letter" ValidationExpression="^.*\.(pdf|PDF)$" ControlToValidate="FileUploadOfferLetter" CssClass="text-red" ForeColor="Red">*</asp:RegularExpressionValidator>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblEmpImage" runat="server" Text="Profile Image (Jpg only) " class="input-label-cubic-14"></asp:Label>
                                            <asp:FileUpload ID="FileUploadAtt" runat="server" ClientIDMode="Static" class="form-control" />
                                            <asp:LinkButton ID="LBtnEmpImage" runat="server" OnClick="LBtnEmpImage_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RegularExpressionValidator ID="vFileUploadEmpImage" runat="server" ErrorMessage="Employee Image Only jpg files are allowed!" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG)$" ControlToValidate="FileUploadAtt" CssClass="text-red" ForeColor="Red">*</asp:RegularExpressionValidator>
                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblCompEmpInfo" runat="server" Text="Employee Information Document (Pdf only) " class="input-label-cubic-14"></asp:Label>
                                            <asp:FileUpload ID="FileUploadEmpInfoDoc" runat="server" class="form-control" />
                                            <asp:LinkButton ID="LBtnUploadEmpInfoDoc" runat="server" OnClick="LBtnUploadEmpInfoDoc_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </div>

                                        <div class="form-group col-md-1">
                                            <asp:RegularExpressionValidator ID="vUploadEmpInfoDoc" runat="server" ErrorMessage="Only PDF files are allowed for Employee Info Document" ValidationExpression="^.*\.(pdf|PDF)$" ControlToValidate="FileUploadEmpInfoDoc" CssClass="text-red" ForeColor="Red">*</asp:RegularExpressionValidator>
                                        </div>
                                    </div>

                                    <%-- ** FAMILY INFORMATION --%>
                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-12">
                                            <div class="card-header py-2">
                                                <h6 class="m-0 input-label-cubic-14" style="color: #4654DF"><i class="fa fa-users"></i>&nbsp;&nbsp; Family Information </h6>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row row-margin-btm-cubic">
                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblEContPerson1" runat="server" Text="Emergency Contact Person 1 " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtEContPerson1" runat="server" class="form-control input-text-cubic-12" placeholder=" Enter Emergency Contact Person 1"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-1">
                                        </div>

                                        <div class="form-group col-md-5">
                                            <asp:Label ID="lblECContactNo1" runat="server" Text="Contact No " class="input-label-cubic-14"></asp:Label>
                                            <asp:TextBox ID="txtECContactNo1" runat="server" class="form-control input-text-cubic-12" placeholder=" Enter Contact No"></asp:TextBox>
                                        </div>
                                    </div>


                                    <%-- ** BUTTON --%>
                                    <div class="row row-margin-btm-cubic">

                                        <div class="form-group col-md-6">
                                            &nbsp;
                                        </div>
                                        <div class="form-group col-md-5 d-flex justify-content-end">
                                            <asp:Button ID="btnClearAll1" runat="server" class="btn-clear-cubic" Style="margin-right: 10px;" Text="Clear All" Width="180" Height="35" OnClick="btnClearAll1_Click" />
                                            <asp:Button ID="btnNextPage1" runat="server" class="btn-next-cubic" Text="Next Page &gt;&gt;" Width="200" Height="35" OnClick="btnNextPage1_Click" />
                                        </div>
                                    </div>
                                    <%-- End DIV --%>
                                </div>
                            </asp:View>

<%-- ************************************ VIEW 2 --%>
                            <asp:View ID="View2" runat="server">
                                <br />


                                <table class="w3-table-all">

                                    <tr>
                                        <th class="auto-style24">
                                            <h3 style="margin-top: 21px; margin-bottom: 21px; font-family: Arial, Helvetica, sans-serif">Address Information</h3>
                                        </th>
                                        <th style="width: 30%;"></th>
                                        <th style="width: 0%;"></th>
                                        <th style="width: 25%;">&nbsp;</th>
                                        <th style="width: 10%;"></th>
                                        <th style="width: 10%;"></th>
                                    </tr>

                                    <tr>
                                        <th class="auto-style24">
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px">Required</h5>
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th>&nbsp;</th>
                                        <th>&nbsp;</th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblAddress1" runat="server" Text="Address 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAddress1" runat="server" class="w3-input w3-border" placeholder="Enter Address 1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddress1" runat="server" ControlToValidate="txtAddress1"  ErrorMessage="Please enter a Address 1"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblAddress2" runat="server" Text="Address 2 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAddress2" runat="server" class="w3-input w3-border" placeholder="Enter Address 2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddress2" runat="server" ControlToValidate="txtAddress2"  ErrorMessage="Please enter a Address 2"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblAddress3" runat="server" Text="Address 3 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAddress3" runat="server" class="w3-input w3-border" placeholder="Enter Address 3"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddress3" runat="server" ControlToValidate="txtAddress3"  ErrorMessage="Please enter a Address 3"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblAddress4" runat="server" Text="Address 4 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAddress4" runat="server" class="w3-input w3-border" placeholder="Enter Address 4"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddress4" runat="server" ControlToValidate="txtAddress4"  ErrorMessage="Please enter a Address 4"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblPostCode" runat="server" Text="Post Code "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtPostCode" runat="server" class="w3-input w3-border" placeholder=" Enter Post Code"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vPostCode" runat="server" ControlToValidate="txtPostCode"  ErrorMessage="Please enter a Post Code"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblCity" runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtCity" runat="server" class="w3-input w3-border" placeholder=" Enter City"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vCity" runat="server" ControlToValidate="txtCity"  ErrorMessage="Please enter a City"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblState" runat="server" Text="State "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtState" runat="server" class="w3-input w3-border" placeholder=" Enter State"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vState" runat="server" ControlToValidate="txtPostCode"  ErrorMessage="Please enter a State"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblCountry" runat="server" Text="Country "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtCountry" runat="server" class="w3-input w3-border" placeholder=" Enter Country"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vCountry" runat="server" ControlToValidate="txtCity"  ErrorMessage="Please enter a Country"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblAddContact1" runat="server" Text="Contact No 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAddContactNo1" runat="server" class="w3-input w3-border" placeholder=" Enter Contact  No 1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddContact1" runat="server" ControlToValidate="txtPostCode"  ErrorMessage="Please enter a Contact No 1"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--  <asp:Label ID="lblAddEmailC1" runat="server" Text="Email "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--  <asp:TextBox ID="txtAddEmailC1" runat="server" class="w3-input w3-border" placeholder=" Enter Email"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddEmailC1" runat="server" ControlToValidate="txtAddEmailC1"  ErrorMessage="Please enter a Email"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <%--   <asp:Label ID="lblAddContactNo2" runat="server" Text="Contact No 2"  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--  <asp:TextBox ID="txtAddContactNo2" runat="server" class="w3-input w3-border" placeholder="Enter Contact No 2 "></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vAddContactNo2" runat="server" ControlToValidate="txtAddress4"  ErrorMessage="Please enter a Contact No 2"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style24"></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td class="cssTableFieldInput">
                                            <asp:CheckBox ID="chkSameAsAbove" AutoPostBack="true" runat="server" Text="Address Same as Above" OnCheckedChanged="chkSameAsAbove_CheckedChanged" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>



                                    <tr>
                                        <th class="cssTableFieldInput">
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px; font-weight: 700">Mailing Address Information</h5>
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblMAddress1" runat="server" Text="Address 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMAddress1" runat="server" class="w3-input w3-border" placeholder="Enter Address 1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMAddress1" runat="server" ControlToValidate="txtMAddress1"  ErrorMessage="Please enter a Address 1"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblMAddress2" runat="server" Text="Address 2 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMAddress2" runat="server" class="w3-input w3-border" placeholder="Enter Address 2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMAddress2" runat="server" ControlToValidate="txtMAddress2"  ErrorMessage="Please enter a Address 2"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblMAddress3" runat="server" Text="Address 3 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMAddress3" runat="server" class="w3-input w3-border" placeholder="Enter Address 3"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMAddress3" runat="server" ControlToValidate="txtMAddress3"  ErrorMessage="Please enter a Address 3"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblMAddress4" runat="server" Text="Address 4 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMAddress4" runat="server" class="w3-input w3-border" placeholder="Enter Address 4"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMAddress4" runat="server" ControlToValidate="txtMAddress4"  ErrorMessage="Please enter a Address 4"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblMPostCode" runat="server" Text="Post Code "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMPostCode" runat="server" class="w3-input w3-border" placeholder=" Enter Post Code"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMPostCode" runat="server" ControlToValidate="txtMPostCode"  ErrorMessage="Please enter a Post Code"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblMCity" runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMCity" runat="server" class="w3-input w3-border" placeholder=" Enter City"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMCity" runat="server" ControlToValidate="txtCity"  ErrorMessage="Please enter a City"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblMState" runat="server" Text="State "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMState" runat="server" class="w3-input w3-border" placeholder="Enter State"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMState" runat="server" ControlToValidate="txtPostCode"  ErrorMessage="Please enter a State"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblMCountry" runat="server" Text="Country "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMCountry" runat="server" class="w3-input w3-border" placeholder=" Enter Country"></asp:TextBox>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMCountry" runat="server" ControlToValidate="txtMCountry"  ErrorMessage="Please enter a Country"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style25" style="padding: 15px">
                                            <asp:Label ID="lblMContactNo1" runat="server" Text="Contact No 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtMContactNo1" runat="server" class="w3-input w3-border" placeholder=" Enter Contact  No 1"></asp:TextBox>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--<asp:RequiredFieldValidator ID="vMContactNo1" runat="server" ControlToValidate="txtPostCode"  ErrorMessage="Please enter a Contact No 1"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--<asp:Label ID="lblMEmail" runat="server" Text="Email "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%-- <asp:TextBox ID="txtMEmail" runat="server" class="w3-input w3-border" placeholder=" Enter Email"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            <%--<asp:RequiredFieldValidator ID="vMEmail" runat="server" ControlToValidate="txtMEmail"  ErrorMessage="Please enter a Email"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <th class="auto-style24">
                                            <asp:Button ID="btnBackPage1" runat="server" BorderStyle="Solid" Height="35" OnClick="btnBackPage1_Click" Style="float: left; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="&lt;&lt; Back Page" Width="200" />
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th>
                                            <asp:Button ID="btnClearAll2" runat="server" Style="float: right; background-color: #FFFFFF; color: #FAAB1A; border-radius: 8px; font-size: small" Text="Clear All" BorderStyle="Solid" Width="180" Height="35" CssClass="auto-style1" BorderColor="#FAAB1A" OnClick="btnClearAll2_Click" />
                                        </th>
                                        <th>
                                            <asp:Button ID="btnNextPage2" runat="server" BorderStyle="Solid" class="btnNextPage2" OnClick="btnNextPage2_Click" Height="35" Style="float: left; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="Next Page &gt;&gt;" Width="200" />
                                        </th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </table>

                            </asp:View>

                            <asp:View ID="View3" runat="server">

                                <br />

                                <table class="w3-table-all">

                                    <tr>
                                        <th class="auto-style27">
                                            <h3 style="margin-top: 21px; margin-bottom: 21px; font-family: Arial, Helvetica, sans-serif">Payroll Information</h3>
                                        </th>
                                        <th class="auto-style28"></th>
                                        <th class="auto-style29"></th>
                                        <th class="auto-style27"></th>
                                        <th class="auto-style30"></th>
                                        <th class="auto-style30"></th>
                                    </tr>

                                    <tr>
                                        <th>
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px">Required</h5>
                                        </th>
                                        <th></th>
                                        <th class="auto-style26"></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblPaymentType" runat="server" Text="Payment Type "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlPaymentType" runat="server" AutoPostBack="false" class="w3-input w3-border" ValidateRequestMode="Enabled">
                                                <asp:ListItem>Cash</asp:ListItem>
                                                <asp:ListItem>Bank</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="auto-style26"></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <%--<tr>
     <td  class="cssTableFieldInput" style= padding:15px >  
          &nbsp;</td>
      <td  class="cssTableFieldInput"  >  
               &nbsp;</td>
          <td class="auto-style26"></td>
           <td></td>
          <td></td>
          <td></td>
      </tr>--%>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtBankName" runat="server" class="w3-input w3-border"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="lblBankCode" runat="server" Text="Bank Code " Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBankCode" runat="server" AutoPostBack="true" class="w3-input w3-border" ValidateRequestMode="Enabled" OnSelectedIndexChanged="ddlBankCode_SelectedIndexChanged" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblAccountNo" runat="server" Text="Account No. "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAccountNo" runat="server" class="w3-input w3-border" placeholder="Enter Account No."></asp:TextBox>
                                        </td>
                                        <td class="auto-style26">
                                            <%--<asp:RequiredFieldValidator ID="vAccountNo" runat="server" ControlToValidate="txtAccountNo"  ErrorMessage="Please enter a Account No."  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblEmpStatus" runat="server" Text="Employment Type"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlEmpType" runat="server" AutoPostBack="false" class="w3-input w3-border" ValidateRequestMode="Enabled">
                                                <asp:ListItem>Full Time</asp:ListItem>
                                                <asp:ListItem>Part Time</asp:ListItem>
                                                <asp:ListItem>Contract</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--   <asp:Label ID="lblResident" runat="server" Text="Is Resident?"  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%-- <asp:DropDownList ID="ddlResident" runat="server" AutoPostBack="false"   class="w3-input w3-border" ValidateRequestMode="Enabled"  >
                    <asp:ListItem>Y</asp:ListItem>
                   <asp:ListItem>N</asp:ListItem>
               </asp:DropDownList>--%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSalaryStatus" runat="server" Text="Salary Type"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlSalaryType" runat="server" AutoPostBack="false" class="w3-input w3-border" ValidateRequestMode="Enabled">
                                                <asp:ListItem>Monthly</asp:ListItem>
                                                <asp:ListItem>Daily</asp:ListItem>
                                                <asp:ListItem>Hourly</asp:ListItem>
                                                <asp:ListItem>Bi Weekly</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblShiftPreset" runat="server" Visible="false" Text="Shift Preset"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlShiftPreset" runat="server" Visible="false" AutoPostBack="false" class="w3-input w3-border" ValidateRequestMode="Enabled">
                                                <asp:ListItem>O1</asp:ListItem>
                                                <asp:ListItem>O2</asp:ListItem>
                                                <asp:ListItem>O3</asp:ListItem>
                                                <asp:ListItem>S</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSalaryWedges" runat="server" Text="Salary Wedges"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtSalaryWedges" runat="server" class="w3-input w3-border" placeholder="Enter Salary Wedges" value="0" onkeypress="return numericOnly(this);"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSalaryCurrency" runat="server" Text="Currency *"> </asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlSalaryCurrency" runat="server" class="w3-input w3-border"></asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblAllowanceWedges" runat="server" Text="Meal Allowance"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtAllowanceWedges" runat="server" class="w3-input w3-border" placeholder="Enter Allowance Wedges" value="0" onkeypress="return numericOnly(this);"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblAllowanceCurrency" runat="server" Text="Currency *"> </asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlAllowanceCurrency" runat="server" class="w3-input w3-border"></asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblWorkingHour" runat="server" Text="Working Hour "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtWorkingHour" runat="server" class="w3-input w3-border" placeholder="Enter Working Hour" value="9"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="vWorkingHour" runat="server" ControlToValidate="txtWorkingHour" ErrorMessage="Please enter valid integer in working hour" ForeColor="Red" Text="*" ValidationExpression="((\d+)((\.\d{1,2})?))$" />
                                            <%--<asp:RequiredFieldValidator ID="vAllowCurrency" runat="server" ControlToValidate="txtAllowCurrency"  ErrorMessage="Please enter a Allowance Currency"  ForeColor="Red" BorderStyle="None"  >*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblWorkingDay" runat="server" Text="Working Day"></asp:Label></td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="txtWorkingDay" runat="server" class="w3-input w3-border" placeholder="Enter Working Day" value="26"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblDate1stPayReview" runat="server" Text="Date of first pay"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="DTDatePayReview" ClientIDMode="Static" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="vDate1stPayReview" runat="server" ControlToValidate="DTDatePayReview" ErrorMessage="Please enter a Date of first pay" ForeColor="Red" BorderStyle="None" Text="*" />
                                        </td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--  <asp:Label ID="lblAttTracking" runat="server" Text="Attendance Tracking *" > </asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%-- <asp:RadioButtonList ID="rbAttTracking" runat="server"  RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Selected="True">Yes</asp:ListItem>
                              <asp:ListItem>No</asp:ListItem>
                          </asp:RadioButtonList>--%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblDateJoin" runat="server" Text="Date of Join "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="dtDateJoin" ClientIDMode="Static" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="vDateJoin" runat="server" ControlToValidate="dtDateJoin" ErrorMessage="Please enter a Date Join" ForeColor="Red" Text="*" />
                                        </td>

                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblEClaimOT" runat="server" Text="Entitle Claim OT *">
                                            </asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:RadioButtonList ID="rbEClaimOT" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblDateResign" runat="server" Text="Date of Resign "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:RadioButtonList ID="rbIsResign" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbIsResign_SelectedIndexChanged" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem Selected="True">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td class="cssTableFieldInput">
                                            <asp:TextBox ID="dtDateResign" ClientIDMode="Static" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:RequiredFieldValidator ID="vDateResign" runat="server" ControlToValidate="dtDateResign" ErrorMessage="Please enter a Date Resign" ForeColor="Red" Text="*" Visible="False" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblResignType" runat="server" Text="Resign Type"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlResignType" runat="server" AutoPostBack="false" class="w3-input w3-border" ValidateRequestMode="Enabled">
                                                <asp:ListItem>-</asp:ListItem>
                                                <asp:ListItem>Resign</asp:ListItem>
                                                <asp:ListItem>Terminate</asp:ListItem>
                                                <asp:ListItem>Quit</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <th>
                                            <asp:Button ID="btnBackPage2" runat="server" BorderStyle="Solid" CssClass="auto-style1" Height="35" OnClick="btnBackPage2_Click" Style="float: left; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="&lt;&lt; Back Page" Width="200" />
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th>
                                            <asp:Button ID="btnClearAll3" runat="server" Style="float: right; background-color: #FFFFFF; color: #FAAB1A; border-radius: 8px; font-size: small" Text="Clear All" BorderStyle="Solid" Width="180" Height="35" CssClass="auto-style1" OnClick="btnClearAll3_Click" BorderColor="#FAAB1A" />
                                        </th>
                                        <th>
                                            <asp:Button ID="btnSummary" runat="server" BorderStyle="Solid" class="btnSummary" CssClass="auto-style1" Height="35" OnClick="btnSummary_Click" Style="float: right; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="Summary &gt;&gt;" Width="200" />
                                        </th>
                                        <th>&nbsp;</th>
                                    </tr>


                                </table>
                            </asp:View>

                            <asp:View ID="View4" runat="server">

                                <br />

                                <table class="w3-table-all">

                                    <tr>
                                        <th style="width: 25%;">
                                            <h3 style="margin-top: 21px; margin-bottom: 21px; font-family: Arial, Helvetica, sans-serif">Summary Information</h3>
                                        </th>
                                        <th style="width: 30%;"></th>
                                        <th style="width: 0%;"></th>
                                        <th style="width: 25%;">&nbsp;</th>
                                        <th style="width: 10%;"></th>
                                        <th style="width: 10%;"></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblConEmpCode" runat="server" Text="Employee Code" Visible="False"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:DropDownList ID="ddlConEmpCode" runat="server" AutoPostBack="True" class="w3-input w3-border" OnSelectedIndexChanged="ddlConEmpCode_SelectedIndexChanged" ValidateRequestMode="Enabled" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSBusinessUni" runat="server" Text="Business Unit "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput" colspan="3">
                                            <asp:Label ID="lblSVBusinessUnit" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="auto-style32"></td>
                                        <td class="auto-style32"></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSDepartment" runat="server" Text="Department "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVDepartment" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpCode" runat="server" Text="Employee Code "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmpCode" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblsProfileImage" runat="server" Text="Profile Image "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:LinkButton ID="LBtnSProImage" runat="server" OnClick="LBtnSProImage_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSFirstName" runat="server" Text="First Name "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVFirstName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSLastName" runat="server" Text="Last Name "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVLastName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSNickName" runat="server" Text="Nick Name "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVNickName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSGender" runat="server" Text="Gender "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVGender" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSICNo" runat="server" Text="IC. No "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVICNo" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSDOB" runat="server" Text="Date of Birth "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVDOB" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSCitizen" runat="server" Text="Citizen "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVCitizen" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--     <asp:Label ID="lblSIsLocal" runat="server" Text="Local/Foreign (IS Local ?) "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--  <asp:Label ID="lblSVIsLocal" runat="server" Text=""></asp:Label>  --%>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpEmail" runat="server" Text="Email "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmpEmail" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpContact" runat="server" Text="Contact "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmpContactNo" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpCategory" runat="server" Text="Employee Category "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmpCategory" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpGrade" runat="server" Text="Employee Grade "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmpGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSPositionTitle" runat="server" Text="Position Title "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVPositionTitle" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSDirectSup" runat="server" Text="Direct Superior "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVDirectSup" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSIncomeNo" runat="server" Text="Income Tax No. "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVIncomeTaxNo" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEPFNo" runat="server" Text="EPF No. "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEPFNO" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSSocsoNo" runat="server" Text="Socso No. "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSValueSocso" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--<asp:Label ID="lblSVisaExpDate" runat="server" Text="Visa Expiry Date "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--  <asp:Label ID="lblSValueVisaExpDate" runat="server" Text=""></asp:Label>  --%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <%--
    <tr>
     <td  class="cssTableFieldInput" style= padding:15px >  
          <asp:Label ID="lblSWorkPType" runat="server" Text="Work Permit Type "  ></asp:Label>
      </td>
      <td  class="cssTableFieldInput">
           <asp:Label ID="lblSVWorkPType" runat="server" Text=""></asp:Label>  
      </td>
       <td></td>
     <td  class="cssTableFieldInput" style= padding:15px >  
          <asp:Label ID="lblSWPExpDate" runat="server" Text="Work Permit Expiry Date "  ></asp:Label>
      </td>
      <td  class="cssTableFieldInput">
            <asp:Label ID="lblSVWPExpDate" runat="server" Text=""></asp:Label>  
      </td>
      <td></td>
    </tr>--%>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSOnBehalf" runat="server" Text="Approve On Behalf "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVOnBehalf" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--     <asp:Label ID="lblSAccessCard" runat="server" Text="Access Card "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--                   <asp:Label ID="lblSVAccessCard" runat="server" Text=""></asp:Label>--%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSResume" runat="server" Text="Resume Document "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:LinkButton ID="LBtnSVResume" runat="server" OnClick="LBtnSVResume_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSOfferLetterDoc" runat="server" Text="Offer Letter Document "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:LinkButton ID="LBtnSVOfferLetterDoc" runat="server" OnClick="LBtnSVOfferLetterDoc_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpInfo" runat="server" Text="Employee Information "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--  <asp:Label ID="lblSVPassDoc" runat="server" Text=""></asp:Label>  --%>
                                            <asp:LinkButton ID="LBtnSVEmpInfo" runat="server" OnClick="LBtnSVEmpInfo_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSStatus" runat="server" Text="Status "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVStatus" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>



                                    <tr>
                                        <th>
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px; font-weight: 700">Family Information</h5>
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmeContPer1" runat="server" Text="Emergency Contact Person 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmeContPer1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmeContNo1" runat="server" Text="Contact No "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmeContNo1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <th>
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px; font-weight: 700">Address Information</h5>
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAddress1" runat="server" Text="Address 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAddress1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAddress2" runat="server" Text="Address 2 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAddress2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAddress3" runat="server" Text="Address 3 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAddress3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAddress4" runat="server" Text="Address 4 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAddress4" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSPostCode" runat="server" Text="Post Code "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVPostCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSCity" runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVCity" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSState" runat="server" Text="State "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVState" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSCountry" runat="server" Text="Country "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVCountry" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSContactNo1" runat="server" Text="Contact No 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVContactNo1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--       <asp:Label ID="lblSEmail" runat="server" Text="Email "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--    <asp:Label ID="lblSVEmail" runat="server" Text=""></asp:Label>  --%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--          <asp:Label ID="lblSContactNo2" runat="server" Text="Contact No 2 "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--     <asp:Label ID="lblSVContactNo2" runat="server" Text=""></asp:Label>  --%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <th>
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px; font-weight: 700">Mailing Address Information</h5>
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMAddress1" runat="server" Text="Address 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMAddress1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMAddress2" runat="server" Text="Address 2 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMAddress2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMAddress3" runat="server" Text="Address 3 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMAddress3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMAddress4" runat="server" Text="Address 4 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMAddress4" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMPostCode" runat="server" Text="Post Code "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMPostCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMCity" runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMCity" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMState" runat="server" Text="State "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMState" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMCountry" runat="server" Text="Country "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMCountry" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSMContact1" runat="server" Text="Contact No 1 "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVMContact1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%--   <asp:Label ID="lblSMEmail" runat="server" Text="Email "  ></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%--    <asp:Label ID="lblSVMEmail" runat="server" Text=""></asp:Label>  --%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <th>
                                            <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px; font-weight: 700">Payroll Information</h5>
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSPaymentType" runat="server" Text="Payment Type "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVPaymentType" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <%-- <asp:Label ID="lblSShiftPreset" runat="server" Text="Shift Preset "></asp:Label>--%>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <%-- <asp:Label ID="lblSVSHiftPreset" runat="server" Text=""></asp:Label>--%>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSBankName" runat="server" Text="Bank Name"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVBankName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAccountNo" runat="server" Text="Account No. "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAccountNo" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSEmpStatus" runat="server" Text="Employeement Type"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVEmpType" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSSalStatus" runat="server" Text="Salary Type"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVSalType" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>



                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSWorkingDay" runat="server" Text="Working Day"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVWorkingDay" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSWorkingHour" runat="server" Text="Working Hour "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVWorkingHour" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>




                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSSalaryWedges" runat="server" Text="Salary Wedges "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVSalaryWedges" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSSalCurrency" runat="server" Text="Salary Currency "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVSalCurrency" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAllWages" runat="server" Text="Allowance Wages "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAllWages" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSAllCurrency" runat="server" Text="Allowance Currency "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVAllCurrency" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>


                                    <%-- <tr>
    <td  class="cssTableFieldInput" style= padding:15px >  
          <asp:Label ID="lblSIsResident" runat="server" Text="Is Resident? * "  ></asp:Label>
      </td>
      <td  class="cssTableFieldInput">  
           <asp:Label ID="lblSVIsResident" runat="server" Text=""></asp:Label>  
      </td>
      <td></td>
      <td></td>
      <td></td>
      <td></td>
    </tr>--%>


                                    <%-- <tr>
     <td  class="cssTableFieldInput" style= padding:15px >  
          <asp:Label ID="lblSEClaimOT" runat="server" Text="Entitle Claim OT "  ></asp:Label>
      </td>
      <td  class="cssTableFieldInput">  
           <asp:Label ID="lblSVEClaimOT" runat="server" Text=""></asp:Label>  
      </td>
       <td></td>
     <td  class="cssTableFieldInput" style= padding:15px >  
          <asp:Label ID="lblSAttTracking" runat="server" Text="Attendance Tracking "  ></asp:Label>
      </td>
       <td  class="cssTableFieldInput">
            <asp:Label ID="lblSVAttTracking" runat="server" Text=""></asp:Label>  
      </td>
      <td></td>
    </tr>--%>


                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSDateFirstJoin" runat="server" Text="Date of First Join"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVDateFirstJoin" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSDate1stPayRev" runat="server" Text="Date of first pay"></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVDate1stPayRev" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSIsDesign" runat="server" Text=" Is he/she Resign? "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVIsDesign" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSResignType" runat="server" Text="Resign Type "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVResignType" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="cssTableFieldInput" style="padding: 15px">
                                            <asp:Label ID="lblSDateResign" runat="server" Text="Date of Resign "></asp:Label>
                                        </td>
                                        <td class="cssTableFieldInput">
                                            <asp:Label ID="lblSVDateResign" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <th>
                                            <asp:Button ID="btnBackPage3" runat="server" BorderStyle="Solid" CssClass="auto-style1" Height="35" OnClick="btnBackPage3_Click" Style="float: left; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="&lt;&lt; Back Page" Width="200" />
                                        </th>
                                        <th></th>
                                        <th></th>
                                        <th>
                                            <%--<asp:Button ID="btnClose" runat="server" BorderStyle="Solid" class="btnSubmit" CssClass="auto-style1" Height="35" OnClick="btnSubmit_Click" style="float: right;background-color:#FAAB1A;color:#FFFFFF;border-radius:8px;font-size:small" Text="Close" Visible="False" Width="200" />--%>
                                        </th>
                                        <th>
                                            <asp:Button ID="btnSubmit" runat="server" BorderStyle="Solid" class="btnSubmit" OnClick="btnSubmit_Click" CssClass="auto-style1" Height="35" Style="float: right; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="Submit" Width="200" />
                                        </th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>

                    </div>


                </div>
            </div>
        </div>
    </div>


    <br />
</asp:Content>
