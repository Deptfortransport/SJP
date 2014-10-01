<%@ Page Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="JourneyPlannerInput.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.JourneyPlannerInput" %>

<%@ Register src="../Controls/LocationControl.ascx" tagname="LocationControl" tagprefix="uc1" %>
<%@ Register src="../Controls/EventDateControl.ascx" tagname="EventDateControl" tagprefix="uc1" %>
<%@ Register src="../Controls/JourneySelectControl.ascx" tagname="JourneySelectControl" tagprefix="uc1" %>
<%@ Register src="../Controls/JourneyOptionTabContainer.ascx" tagname="JourneyOptions" tagprefix="uc1" %>
<%@ Register Namespace="SJP.UserPortal.SJPWeb.Controls" TagPrefix="uc1" assembly="sjp.userportal.sjpweb" %> 
<%@ Register Namespace="SJP.Common.Web" TagPrefix="uc2" assembly="sjp.common.web" %> 


<asp:Content ID="ContentMessages" ContentPlaceHolderID="contentMessages" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
     
     <div class="mainSection">

        <asp:UpdatePanel ID="contentMessagesUpdater" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="pnlMessages" runat="server" EnableViewState="false" CssClass="contentMessages">

                    <asp:Repeater ID="rptrMessages" runat="server" EnableViewState="false" OnItemDataBound="rptrMessages_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" ></asp:Label>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <uc1:JourneySelectControl ID="journeySelectControl" runat="server" />

        <asp:UpdatePanel ID="locationDescUpdate" runat="server" UpdateMode="Always" RenderMode="Block">
            <ContentTemplate>

                <asp:Label ID="lblLocations" runat="server" EnableViewState="false" ></asp:Label>
                <br />

                <div id="travelBetweenVenuesDiv" runat="server" class="clearboth travelBetweenVenuesDiv" visible="false">
                    <uc2:LinkButton ID="travelBetweenVenues" CssClass="linkButton travelBetweenVenues" MouseOverClass="linkButtonMouseOver" OnClick="TravelBetweenVenues_Click" runat="server" />
                    <br />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>

    <div class="box">
        <div class="bH"></div>
        <div class="bC">
        
            <div class="journeyParams">

        <asp:UpdatePanel ID="fromLocationUpdate" runat="server" UpdateMode="Always" RenderMode="Block">
            <ContentTemplate>
                
                <div id="fromLocation" class="locationRow">
                    <asp:Label ID="fromLocationLabel" CssClass="locationLabel" runat="server" EnableViewState="false" />

                    <uc1:LocationControl ID="locationFrom"  runat="server" />
                    <a class="tooltip_information" href="#" onclick="return false;"  id="tooltip_information_from" runat="server" enableviewstate="false">
                        <asp:Image ID="locationFrom_Information" CssClass="information" runat="server" />
                    </a>
                </div>

                <div id="travelFromToToggleDiv" runat="server" class="clearboth travelFromToVenueDiv" visible="false">
                    <asp:ImageButton ID="travelFromToVenueToggle" CssClass="" runat="server" OnClick="TravelFromToToggle_Click" />
                </div>

                <asp:HiddenField ID="previousLocationInputMode" runat="server" />

            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="toLocationUpdate" runat="server" UpdateMode="Always" RenderMode="Block">
            <ContentTemplate>

                 <div id="toLocation" class="locationRow">
                    <asp:Label ID="toLocationLabel" CssClass="locationLabel" runat="server" EnableViewState="false" />
        
                     <uc1:LocationControl ID="locationTo" runat="server" />
                     <a class="tooltip_information" href="#" title=""  onclick="return false;" id="tooltip_information_to" runat="server" enableviewstate="false">
                        <asp:Image ID="locationTo_Information" CssClass="information" runat="server" />
                     </a>
                 </div>

            </ContentTemplate>
        </asp:UpdatePanel>

        <br />

        <asp:UpdatePanel ID="eventDateTimeUpdate" runat="server" UpdateMode="Conditional" RenderMode="Block">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="travelBetweenVenues" />
                <asp:AsyncPostBackTrigger ControlID="travelFromToVenueToggle" />
            </Triggers>
            <ContentTemplate>
                <div class="eventDateTime">
                    <uc1:EventDateControl ID="eventControl" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

            </div>    
     
    <uc1:JourneyOptions ID="journeyOptions" runat="server" />
      
    <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv">
        <asp:Label ID="lblDebugInfo" runat="server" EnableViewState="false" CssClass="debug" />
    </div>
     
      <%-- client side values for outward and return dates --%>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Block">
        <ContentTemplate>
             <div class="jssettings">
                <asp:HiddenField ID="calendarOutward" runat="server" />
                <asp:HiddenField ID="calederReturn" runat="server" />
                <asp:HiddenField ID="venueID" runat="server" />

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
        <div class="bF"></div>
    </div>

</asp:Content>
