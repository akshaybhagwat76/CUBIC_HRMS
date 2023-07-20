<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmDashboard.aspx.cs" Inherits="CUBIC_HRMS.FrmDashboard" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/SubSite.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <link href="assets/fullcalendar/fullcalendar.min.css" rel="stylesheet" />

    <script src="assets/fullcalendar/jquery.min.js"></script>

    <style>
        th.fc-day-header.fc-widget-header {
            background-color: #ABBADC;
            color: white;
        }
    </style>

    <div class="container-fluid">
        <div class="card shadow mb-4">
            <%-- <div class="card-header py-2">
                <h6 class="m-0 input-label-cubic-16" style="color: #4654DF"><i class="fa fa-user"></i>&nbsp;&nbsp; Employee Master </h6>
            </div>--%>

            <div class="card-body">

                <div id="calendar" class="has-toolbar"></div>
            </div>
        </div>
    </div>

    <script type="text/javascript">  
        $(document).ready(function () {
            var _EventList = [];

            $.ajax({
                type: "POST",
                url: "FarchData.asmx/CalenderData",
                data: "{'myUserName':'MyUserNameIsRaj'}",
                contentType: "application/json",
                datatype: "json",
                success: function (responseFromServer) {

                    var _Data = JSON.parse(responseFromServer.d);
                    for (var i = 0; i < _Data.length; i++) {
                        var _obj = { title: _Data[i].LEAVE_NAME, start: _Data[i].LEAVED_DATE_FROM, end: _Data[i].LEAVED_DATE_TO, color: _Data[i].COLOR, id: _Data[i].LEAVEH_NO };
                        _EventList.push(_obj);
                    }

                    $('#calendar').fullCalendar(
                        {
                            header:
                            {
                                left: 'prev,next today',
                                center: 'title',
                                right: 'month,agendaWeek,agendaDay'
                            },
                            defaultDate: new Date(),
                            editable: true,
                            eventLimit: true, // allow "more" link when too many events
                            events: _EventList,
                            eventRender: function (event, element) {
                                element.bind('click', function (e) {

                                    var eventId = event.id;
                                    if (eventId) {
                                        var url = 'FrmEmpty?ID=' + eventId;
                                        window.location.href = url;
                                    }
                                });
                            },

                            //for day double click
                            dayRender: function (date, cell) {
                                cell.bind('click', function () {

                                    var d = new Date(date);
                                    var strDate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
                                    var url = 'FrmLeaveRequest?ID=HLinkLeaveRequest&date=' + strDate;
                                    window.location.href = url;
                                })
                            }
                        });
                }
            });
        });
    </script>



    <!-- END PAGE BASE CONTENT -->



</asp:Content>
