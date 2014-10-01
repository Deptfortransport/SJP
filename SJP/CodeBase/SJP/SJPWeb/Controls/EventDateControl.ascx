<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventDateControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.EventDateControl" %>
<%@ Register src="~/Controls/Calendar.ascx" tagname="CalendarControl" tagprefix="uc1" %>

<div class="eventDate">
     <div class="jssettings">
        <asp:HiddenField ID="calendarStartDate" runat="server" />
        <asp:HiddenField ID="calendarEndDate" runat="server" />
        <asp:HiddenField ID="returnOnly" runat="server" />
    </div>
    <div id="outwardDateDiv" runat="server" class="outward">
        <div class="row">
            <asp:Label ID="outwardDateLabel" CssClass="dateLabel"  AssociatedControlID="outwardDate" runat="server" EnableViewState="false" />
            <div class="dateSelect">
                <asp:TextBox ID="outwardDate" CssClass="text dateEntry" runat="server" Columns="10" OnTextChanged="OutwardDate_Changed"></asp:TextBox>
            </div>
            <div class="timePicker">
                <asp:Label ID="arriveTimeLabel" CssClass="timeLabel"  AssociatedControlID="arriveTime" runat="server" EnableViewState ="false" />
                <asp:DropDownList ID="arriveTime" runat="server" CssClass="timePickerDrop"/>
            </div>
            <a class="tooltip_information" href="#" onclick="return false;"  id="tooltip_information_outward" runat="server" enableviewstate="false">
                <asp:Image ID="outward_Information" CssClass="information" runat="server" />
            </a>
        </div>
        <div class="clearboth"></div>
    </div>
    <div class="return">
        <div id="returnDateCheckBoxDiv" runat="server" class="row">
             <asp:CheckBox ID="isReturnJourney" CssClass="isReturn" runat="server" Checked="false" Text="Return Journey" />
        </div>
        <div class="row">
            <asp:Label ID="returnDateLabel" CssClass="dateLabel" AssociatedControlID="returnDate" runat="server" EnableViewState="false" />
            <div class="dateSelect">
                <asp:TextBox ID="returnDate" CssClass="text dateEntry" runat="server" Columns="10" OnTextChanged="ReturnDate_Changed"></asp:TextBox>
            </div>
            <div class="timePicker">
                <asp:Label ID="leaveTimeLabel" CssClass="timeLabel" AssociatedControlID="leaveTime" runat="server" EnableViewState ="false" />
                 <asp:DropDownList ID="leaveTime" runat="server" CssClass="timePickerDrop" />
            </div>
            <a class="tooltip_information" href="#" onclick="return false;"  id="tooltip_information_return" runat="server" enableviewstate="false">
                <asp:Image ID="return_Information" CssClass="information" runat="server" />
            </a>
         </div>
         <div class="clearboth"></div>
    </div>

    

</div>