<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarMonth.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.CalendarMonth" %>
<div id="calendarMonth" class="calendarMonth" runat="server">
    <asp:HiddenField ID="calendarMonth_Month" runat="server" />
    <asp:HiddenField ID="calendarMonth_Year" runat="server" />
        
    <asp:Repeater ID="calendarMonthView" runat="server" OnItemDataBound="calendarMonthView_DataBound" >
        <HeaderTemplate>
             <table class="BlogCalendarTable" cellspacing="5" cellpadding="5" border="0">
                <thead>
                    <tr>
                        <th><%# GetDayResourceString(DayOfWeek.Sunday) %></th>
                        <th><%# GetDayResourceString(DayOfWeek.Monday) %></th>
                        <th><%# GetDayResourceString(DayOfWeek.Tuesday) %></th>
                        <th><%# GetDayResourceString(DayOfWeek.Wednesday) %></th>
                        <th><%# GetDayResourceString(DayOfWeek.Thursday) %></th>
                        <th><%# GetDayResourceString(DayOfWeek.Friday) %></th>
                        <th><%# GetDayResourceString(DayOfWeek.Saturday) %></th>
		            </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <asp:Repeater ID="calendarMonthRow" runat="server"  OnItemDataBound="calendarMonthRow_DataBound">
                        <ItemTemplate>
                             <td id="dayCell" runat="server" class="day">
                               
                                    <asp:Button ID="Day" runat="server" OnCommand="Day_Command" CommandArgument="<%# Container.DataItem %>"
                                        Text='<%# Container.DataItem %>' />
                               
                                
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                   
                </tr>
        </ItemTemplate>
        <FooterTemplate>
                </tbody>
                </table>
               
        </FooterTemplate>
    </asp:Repeater>
   
</div>
