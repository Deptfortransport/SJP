<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calendar.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.Calendar" %>
<%@ Import Namespace="SJP.UserPortal.SJPWeb" %>
<%@ Register Src="~/Controls/CalendarMonth.ascx" TagName="CalendarMonth" TagPrefix="uc1" %>
<div id="sjp-calendar-tabs" class="tabbed-content three-tabs">
    <asp:HiddenField ID="calendar_SelectedDate" runat="server" />
    <div class="tabbed-headers-container hide-high-vis">
        <ul class="tabbed-headers" id="monthHeader" style="display: block;">
            <li id="monthHeader1" class="current" runat="server">
                <asp:Button ID="month1Button" runat="server" OnCommand="monthTab_Command" CssClass="monthTab" />
            </li>
            <li id="monthHeader2" runat="server">
                <asp:Button ID="month2Button" runat="server"  OnCommand="monthTab_Command" CssClass="monthTab" />
            </li>
            <li id="monthHeader3" runat="server">
                <asp:Button ID="month3Button" runat="server"  OnCommand="monthTab_Command" CssClass="monthTab" />
            </li>
        </ul>
    </div>
    <div class="content current">
        <div id="BlogCalendar" class="calendarView">
            <uc1:CalendarMonth runat="server" ID="calendarMonth1" Year="2012" Month="6"  />
            <uc1:CalendarMonth runat="server" ID="calendarMonth2" Year="2012" Month="7"  />
            <uc1:CalendarMonth runat="server" ID="calendarMonth3" Year="2012" Month="8"  />
        </div>
    </div>
</div>
 