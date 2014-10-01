<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventDateControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.EventDateControl" %>

<div class="eventDate">
    <div class="jssettings">
        <asp:HiddenField ID="calendarStartDate" runat="server" />
        <asp:HiddenField ID="calendarEndDate" runat="server" />
        <asp:HiddenField ID="isToVenueFlag" runat="server" />
        <asp:HiddenField ID="isArriveByFlag" runat="server" />
        <asp:HiddenField ID="isNowFlag" runat="server" />
    </div>
    <div id="outwardDateDiv" runat="server" class="outward">
        <div class="row">
            <div class="screenReaderOnly">
                <asp:Label ID="eventDateLabel" CssClass="eventDateLabel" AssociatedControlID="outwardDate" runat="server" EnableViewState="false" />                
            </div>
            <div class="dateSelect">
                <div class="screenReaderOnly">
                    <asp:Label ID="outwardDateLabel" CssClass="dateLabel"  AssociatedControlID="outwardDate" runat="server" EnableViewState="false" />
                </div>
                <div class="setdatebox">
                    <asp:TextBox ID="outwardDate" CssClass="text dateEntry jsinput" runat="server" Columns="10" OnTextChanged="OutwardDate_Changed"></asp:TextBox>
                </div>
                <div data-role="page" id="datepage">
                    <div class="headerBack">
                        <h2 ID="dateSelectorLabel" runat="server" EnableViewState="false"></h2>
                        <asp:HyperLink ID="closedate" runat="server" CssClass="topNavLeft" data-role="none" data-icon="delete" />
                    </div>
                    <asp:ListView runat="server" ID="outwardDateMonths" ItemPlaceholderID="monthPlaceholder">
                        <LayoutTemplate>
                            <div data-role="content">	
                                <fieldset data-role="controlgroup" >
                                    <div data-role="collapsible-set">
                                        <asp:PlaceHolder runat="server" ID="monthPlaceholder" /> 
                                    </div>
                                </fieldset>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div data-role="collapsible" data-collapsed="<%# GetCollapsed(Eval("monthName")) %>" class="collapseDate" id="<%# Eval("monthName") %>">
                                <h3><%# Eval("monthName") %> <%# Eval("year") %></h3>
                                <div class="collapseDateDays <%# (GetCollapsed(Eval("monthName")) == "true") ? "collapse" : "" %>">
                                    <ul class="week"><li>Mon</li><li>Tue</li><li>Wed</li><li>Thur</li><li>Fri</li><li>Sat</li><li>Sun</li></ul>

                                    <asp:ListView runat="server" ID="outwardDateWeeks" GroupPlaceholderID="weekPlaceholder" ItemPlaceholderID="dayPlaceholder" DataSource='<%# Eval("dates") %>' GroupItemCount="7">
                                        <LayoutTemplate>
                                            <asp:PlaceHolder runat="server" ID="weekPlaceholder" /> 
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <div class="<%# (Eval("disabled") != "") ? "day dayDisabled" : "day" %>" >
                                                <input type="radio" name="data" id="<%# Eval("date") %>/<%# Eval("month") %>/<%# Eval("year") %><%# (Eval("disabled") != "") ? "d" : "" %>" value="<%# Eval("date") %>/<%# Eval("month") %>/<%# Eval("year") %>"<%# (Eval("disabled") != "") ? Eval("disabled") : Eval("selected") %>/>
                                                <label for="<%# Eval("date") %>/<%# Eval("month") %>/<%# Eval("year") %><%# (Eval("disabled") != "") ? "d" : "" %>">
                                                    <%# Eval("date") %>
                                                </label>
                                            </div>
                                        </ItemTemplate>
                                        <GroupTemplate>
                                            <asp:PlaceHolder ID="dayPlaceholder" runat="server" />
                                        </GroupTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div class="timePicker">
                <div class="screenReaderOnly">
                    <asp:Label ID="outwardTimeLabel" CssClass="timeLabel"  AssociatedControlID="outwardTime" runat="server" EnableViewState ="false" />
                </div>
                <div class="settimebox">
                    <asp:TextBox ID="outwardTime" runat="server" CssClass="timeInput jsinput" />
                </div>
                <div id="timepage" data-role="page" >
                    <div class="headerBack">
        	    	    <h2 ID="timeSelectorLabel" runat="server" EnableViewState="false"></h2>
                        <asp:HyperLink ID="closetime" runat="server" CssClass="topNavLeft" data-role="none" data-icon="delete" />
    	            </div>
                    <div data-role="content">
                        <asp:RadioButtonList ID="timeRadioList" runat="server" data-role="controlgroup">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div id="nowSelectDiv" runat="server" class="nowSelect" enableviewstate="false">
                <asp:LinkButton ID="nowLink" runat="server" CssClass="jshide" OnClick="nowLink_Click"></asp:LinkButton>
                <noscript>
                    <asp:Button ID="nowLinkNonJS" runat="server" CssClass="nowLinkNonJS" OnClick="nowLink_Click" />
                </noscript>
            </div>
        </div>
    </div>
</div>
