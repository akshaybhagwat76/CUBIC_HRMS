<%@ Page Title="Leave REquest" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmLeaveRequest.aspx.cs" Inherits="CUBIC_HRMS.FrmLeaveRequest" %>


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


    <asp:ScriptManager ID="ScriptManagerMain" runat="server"></asp:ScriptManager>

    <div class="container-fluid">

        <h1 class="h3 mb-2 text-gray-800"></h1>
        <p class="mb-1"></p>

        <div class="card-header py-2">
            <h6 class="m-0 input-label-cubic-16" style="color: #4654DF"><i class="fa fa-user"></i>&nbsp;&nbsp; Apply Leave</h6>
        </div>

        <div class="card shadow mb-4">

            <div class="card-body">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="#813838" ShowModelStateErrors="True" BackColor="#FEE2E1" Font-Names="Segoe UI Symbol" EnableTheming="True" EnableClientScript="True" />

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="w3-table-all">

                            <tr>
                                <td style="border: 0pt solid black; width: 21%;">
                                    <h5 style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 15px">&nbsp;</h5>
                                </td>
                                <td style="border: 0pt solid black; width: 20%;"></td>
                                <td style="border: 0pt solid black; width: 3%;"></td>
                                <td style="border: 0pt solid black; width: 30%;"></td>
                                <td style="border: 0pt solid black; width: 20%;"></td>
                                <td style="border: 0pt solid black; width: 5%;"></td>
                            </tr>

                            <tr runat="server" id="tr_Transactionddl">
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblTransactionDdl" runat="server" Text="Transaction No*" Style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:DropDownList ID="ddlTransactionNo" runat="server" AutoPostBack="true" class="w3-input w3-border" ValidateRequestMode="Enabled" Style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif" OnSelectedIndexChanged="ddlTransactionNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblTransactionNo" runat="server" Text="Transaction No *"></asp:Label></td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:TextBox ID="txtLoadTransNo" runat="server" class="w3-input w3-border" ReadOnly="true" BackColor="#E6E6FF" Text="0"></asp:TextBox></td>
                                <td></td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblLeaveType" runat="server" Text="Leave Type *"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:DropDownList ID="ddlLeaveType" runat="server" AutoPostBack="true" class="w3-input w3-border" ValidateRequestMode="Enabled" OnSelectedIndexChanged="ddlLeaveType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="vLeaveType" runat="server" ControlToValidate="ddlLeaveType" ErrorMessage="Please select a Leave Type" ForeColor="Red" InitialValue="- Select -" BorderStyle="None">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblLeaveCode" runat="server" Text="Leave Code"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:TextBox ID="txtLeaveCodeKeep" runat="server" class="w3-input w3-border" ReadOnly="true" BackColor="#E6E6FF" Text="0"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblFrom" runat="server" Text="From *"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px; background-color: transparent">
                                    <asp:TextBox ID="dtFrom" ClientIDMode="Static" runat="server" TextMode="Date" class="form-control" AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="vFrom" Text="*" ErrorMessage="Please select Date From" ControlToValidate="dtFrom" runat="server" ForeColor="Red" />
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblTo" runat="server" Text="To *"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 6px">
                                    <asp:TextBox ID="dtTo" ClientIDMode="Static" runat="server" TextMode="Date" class="form-control" AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="vTo" Text="*" ErrorMessage="Please select Date To" ControlToValidate="dtTo" runat="server" ForeColor="Red" />
                                </td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblHalfDay" runat="server" Text="AM/PM *"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:DropDownList ID="ddlHalfDay" runat="server" AutoPostBack="true" class="w3-input w3-border" ValidateRequestMode="Enabled" OnSelectedIndexChanged="ddlHalfDay_SelectedIndexChanged">
                                        <asp:ListItem>- Select -</asp:ListItem>
                                        <asp:ListItem>Full Day</asp:ListItem>
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="vHalfDay" runat="server" ControlToValidate="ddlHalfDay" ErrorMessage="Please select Half Day or Full Day" ForeColor="Red" InitialValue="- Select -" BorderStyle="None">*</asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAutoNumber" runat="server" Visible="False"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblLeaveBalance" runat="server" Text="Leave Balance (Day)  " Font-Bold="True"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:TextBox ID="txtLeaveBalance" runat="server" class="w3-input w3-border" ReadOnly="true" BackColor="#E6E6FF" Text="0"></asp:TextBox></td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <%--       <asp:Label ID="lblDays0" runat="server" Text="Days ">--%>
                                    <%-- </asp:Label>--%>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblDayApplied" runat="server" Text="You are applying (Day)  " Font-Bold="True"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:TextBox ID="txtDayApplied" runat="server" class="w3-input w3-border" ReadOnly="true" BackColor="#E6E6FF" Text="0"></asp:TextBox>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <%--<asp:CompareValidator runat="server" ControlToValidate="txtDayApplied" Operator="GreaterThanEqual" ValueToCompare="0" Type="Integer" ErrorMessage="Applied date cannot be less than zero" Display="Dynamic" Text="*"/>--%>
                                    <asp:RangeValidator ID="vDayApplied" runat="server" ErrorMessage="Applied date cannot be less than zero" ControlToValidate="txtDayApplied" MaximumValue="100" MinimumValue="0.5" Type="Double" ForeColor="Red">*</asp:RangeValidator>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblOtherDet" runat="server" Text="Other Details">
                                    </asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:TextBox ID="txtOtherDet" runat="server" class="w3-input w3-border" placeholder="Enter Other Details" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td></td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtAutoNumberIncreament" runat="server" Visible="False"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>



                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status  "></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:TextBox ID="txtStatus" runat="server" class="w3-input w3-border" ReadOnly="true" BackColor="#E6E6FF" Text="Pending" BorderStyle="Ridge"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>

                            <tr>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:Label ID="lblAttFile" runat="server" Text="Attach File (Only pdf)"></asp:Label>
                                </td>
                                <td class="cssTableFieldInput" style="padding: 8px">
                                    <asp:FileUpload ID="FileUpload" runat="server" ClientIDMode="Static" />
                                    <br />
                                    <asp:LinkButton ID="LBtnFileAtt" runat="server" OnClick="LBtnFileAtt_Click" OnClientClick="window.document.forms[0].target='_blank';" />
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only PDF files are allowed!" ValidationExpression="^.*\.(pdf|PDF)$" ControlToValidate="FileUpload" CssClass="text-red" ForeColor="Red">*</asp:RegularExpressionValidator>
                                </td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtTransactionNo" runat="server" Visible="False"></asp:TextBox>
                                </td>
                                <td>
                                    <%--<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />--%>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        </div>
               
                    </ContentTemplate>
                </asp:UpdatePanel>


                <table class="w3-table-all">
                    <tr>
                        <th></th>
                        <th></th>
                        <th class="auto-style11"></th>
                        <th class="auto-style14">&nbsp;</th>
                        <th>
                            <asp:Button ID="btnSendRequest" runat="server" Style="float: right; background-color: #FAAB1A; color: #FFFFFF; border-radius: 8px; font-size: small" Text="Send Request" BorderStyle="Solid" Width="160" Height="35" OnClick="btnSendRequest_Click" />
                            <asp:Button ID="btnCancel" runat="server" Style="float: right; background-color: #FFFFFF; color: #FAAB1A; border-radius: 8px; font-size: small" Text="Cancel" BorderStyle="Solid" Width="160" Height="35" BorderColor="#FAAB1A" OnClick="btnCancel_Click" Visible="False" />
                        </th>
                        <th>&nbsp;</th>
                    </tr>

                </table>


                <div class="WordWrap">
                    <hr class="cssContentHeaderLine" />
                    <asp:GridView ID="grdSummary" runat="server" AllowSorting="True" EmptyDataText="No records found"
                        Font-Names="'Segoe UI', Tahoma, Geneva, Verdana, sans-serif"
                        AutoGenerateColumns="False"
                        AllowPaging="True"
                        OnPageIndexChanging="grdSummary_PageIndexChanging"
                        PageSize="20"
                        CellPadding="4"
                        ForeColor="#333333"
                        font-family="Verdana"
                        Font-Size="10pt"
                        GridLines="None" Width="90%" AlternatingRowStyle-HorizontalAlign="NotSet" ShowFooter="True" Style="margin-left: auto; margin-right: auto;">
                        <AlternatingRowStyle BackColor="White" />

                        <Columns>

                            <%--   <asp:BoundField HeaderText="No" DataField="FLEXI_NO" HeaderStyle-CssClass="text-center">
         <ItemStyle HorizontalAlign="Center"></ItemStyle>
         </asp:BoundField>--%>

                            <asp:BoundField HeaderText="Transaction No" DataField="LEAVEH_NO" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Emp Code" DataField="LEAVEH_EMP_CODE" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Leave Code" DataField="LEAVEH_CODE" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Leave Date From" DataField="LEAVED_DATE_FROM" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Leave Date To" DataField="LEAVED_DATE_TO" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Half Day/Full Day" DataField="LEAVED_FULLDAYORNOT" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Duration" DataField="LEAVED_TOTAL_APPLY_DAY" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Leave Balance" DataField="LEAVED_LEAVE_DAY" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Status" DataField="LEAVED_STATUS" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Approved By" DataField="LEAVE_APPROVED_BY" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Approved Date" DataField="LEAVE_APPROVED_DATE" HeaderStyle-CssClass="text-center" ItemStyle-Wrap="true">
                                <HeaderStyle CssClass="text-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                        </Columns>

                        <EditRowStyle BackColor="#7C6F57" BorderStyle="None" />
                        <FooterStyle BackColor="#344955" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#344955" Font-Bold="True" ForeColor="White" Font-Size="15px" />
                        <PagerStyle BackColor="#FAAB1A" ForeColor="White" HorizontalAlign="Center" Font-Size="13px" />
                        <RowStyle CssClass="rowHover" BackColor="#E3EAEB"></RowStyle>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                        <EmptyDataRowStyle Width="550px" ForeColor="Red" Font-Bold="true" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
        </div>
    </div>
</asp:Content>
