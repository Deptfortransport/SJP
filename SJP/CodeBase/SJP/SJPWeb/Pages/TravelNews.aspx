<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true"
    CodeBehind="TravelNews.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.TravelNews" %>

<%@ Register src="~/Controls/Calendar.ascx" tagname="CalendarControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/TravelNewsItemControl.ascx" tagname="TravelNewsItemControl" tagprefix="uc1" %>
<%@ Register Namespace="SJP.UserPortal.SJPWeb.Controls" TagPrefix="cc1" Assembly="sjp.userportal.sjpweb" %>
<%@ Register Namespace="SJP.Common.Web" TagPrefix="cc2" assembly="sjp.common.web" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="contentHeading" runat="server">
    <div id="sectionSubHeader" class="sectionSubHeader floatleft">
        <h2 id="sjpHeading" runat="server" enableviewstate="false"></h2>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">

    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="tnOptions">
                <div class="jssettings">
                    <asp:HiddenField ID="calendarStartDate" runat="server" />
                    <asp:HiddenField ID="calendarEndDate" runat="server" />
                </div>
                <div class="regionSelector floatleft">
                    <cc1:ImageMapControl ID="ukImageMap" runat="server" OnRegionClicked="RegionChange" />
                </div>
                <div class="tnfilter floatright">
                    <div id="refreshLinkDiv" runat="server" class="refreshLinkDiv">
                        <asp:HyperLink id="refreshLink" runat="server" EnableViewState="false"></asp:HyperLink>
                    </div>
                    <div class="header">
                        <asp:Label ID="tnFilterHeading" runat="server"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="label">
                            <asp:Label ID="lblVenue" runat="server" AssociatedControlID="useVenues"></asp:Label><asp:CheckBox ID="useVenues" runat="server" />
                        </div>
                        <div class="options">
                            <cc2:GroupDropDownList ID="venueDropdown" runat="server" ></cc2:GroupDropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="label">
                            <asp:Label ID="lblRegion" runat="server"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:DropDownList ID="regionDrop" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RegionDropChange" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="label">
                            <asp:Label ID="lblInclude" runat="server"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:CheckBox ID="publicTransportNews" runat="server"  CssClass="transportType" />
                            <asp:CheckBox ID="roadNews" runat="server" CssClass="transportType" />
                        </div>
                    </div>
                    <div class="row tnphrase">
                        <div class="label">
                            <asp:Label ID="lblTNPhrase" runat="server"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:TextBox ID="tnPhrase" runat="server" />
                        </div>
                    </div>
                    <div class="row tndate">
                        <div class="label">
                            <asp:Label ID="lblTNDate" runat="server"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:TextBox ID="tnDate" cssClass="dateEntry" Columns="12" runat="server" />
                        </div>
                    </div>
                    <div class="row submit">
                        <asp:Button ID="filterNews" CssClass="submit btnMediumPink floatright" EnableViewState="false"  runat="server" Text="Apply filter"
                            OnClick="FilterNews_Click" />
                    </div>
                </div>
                <div class="clearboth"></div>
            </div>
            <div class="sjpSeparator"></div>

            <br />
            <div id="newsProgress" runat="server" class="newsProgress hide" >
                    <div class="sjpSeparator" ></div>
                    <div class="progressBackgroundFilter"></div>
                    <div class="processMessage"> 
                         <asp:Image ID="loading" runat="server" />
                         <br /><br />
                         <div>
                            <asp:Label ID="loadingMessage" runat="server" />
                            <noscript>
                                <br />
                                <asp:Label ID="longWaitMessage" runat="server" />
                                <asp:HyperLink ID="longWaitMessageLink" runat="server" />
                            </noscript>
                            
                         </div>
                    </div>
                    <div class="sjpSeparator" ></div>
            </div>
            <div class="clearboth"></div>

            <div class="travelnews">
                <asp:Label ID="lblNoIncidents" runat="server"></asp:Label>
                <asp:Label ID="lblFilterNews" CssClass="warning bold" runat="server"></asp:Label>
                <asp:Label ID="lblDebug" runat="server" CssClass="debug" Visible="false" EnableViewState="false"></asp:Label>
                <div id="tnOlympicTravelNews" class="impact" runat="server" visible="false">
                    <div id="tnOlympicHeading" class="impactHeading active" runat="server">
                        <div class="arrow_btn">
                            <noscript>
                                <asp:ImageButton ID="olympicSeverityButton" runat="server" OnCommand="ImageButton_Command" CommandName="ShowOlympicTN" />
                            </noscript>
                        </div>
                        <div class="text">
                            <h3> 
                                <asp:Label ID="olympicImpactHeading" CssClass="severity" runat="server" />
                            </h3>
                        </div>
                        <div class="clearboth">
                        </div>
                    </div>
                    <div id="olympicImpactContainer" class="imContainer expanded" runat="server">
                        <uc1:TravelNewsItemControl ID="olympicImpactTravelNewsItems" runat="server" />
                    </div>
                </div>
                <div id="tnOtherTravelNews" class="impact" runat="server" visible="false">
                    <div id="tnOtherHeading" class="impactHeading active" runat="server">
                        <div class="arrow_btn">
                            <noscript>
                                <asp:ImageButton ID="otherSeverityButton" runat="server" OnCommand="ImageButton_Command" CommandName="ShowOtherTN" />
                            </noscript>
                        </div>
                        <div class="text">
                            <h3> 
                                <asp:Label ID="otherImpactHeading" CssClass="severity" runat="server" />
                            </h3>
                        </div>
                        <div class="clearboth">
                        </div>
                    </div>
                    <div id="otherImpactContainer" class="imContainer expanded" runat="server">
                        <asp:Repeater ID="travelNewsRepeater" runat="server" OnItemDataBound="TravelNews_DataBound">
                            <ItemTemplate>
                                <div class="severity">
                                    <asp:HiddenField ID="tnSeverity" runat="server" />
                                    <div id="tnHeading" class="severityHeading" runat="server">
                                        <div class="arrow_btn">
                                            <noscript>
                                                <asp:ImageButton ID="severityButton" runat="server" OnCommand="ImageButton_Command" CommandName="ShowSeverity" />
                                            </noscript>
                                        </div>
                                        <div class="text">
                                            <h3> 
                                                <asp:Label ID="severityHeading" CssClass="severity" runat="server" />
                                            </h3>
                                        </div>
                                        <div class="clearboth">
                                        </div>
                                    </div>
                                    <div id="tnContainer" class="tnContainer collapsed" runat="server">
                                        <uc1:TravelNewsItemControl ID="travelnewsItems" runat="server" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <%-- client side calendar resources --%>
     <div class="jssettings">
        <asp:HiddenField ID="calendar_ButtonText" runat="server" />
        <asp:HiddenField ID="calendar_DayNames" runat="server" />
        <asp:HiddenField ID="calendar_NextText" runat="server" />
        <asp:HiddenField ID="calendar_PrevText" runat="server" />
        <asp:HiddenField ID="calendar_MonthNames" runat="server" />
     </div>

</div>

</asp:Content>
