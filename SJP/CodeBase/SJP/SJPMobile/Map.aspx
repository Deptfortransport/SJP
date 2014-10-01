<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Map.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.Map" %>
<%@ Register Src="~/Controls/VenueMapControl.ascx" TagName="VenueMapControl" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMain" runat="server">
    <div id="modeInformation">
        <asp:HiddenField id="modeType" runat="server" value="" />
        <asp:HiddenField id="journeyPoints" runat="server" value="" />
        <asp:HiddenField id="mapLocationName" runat="server" value="" />
        <asp:HiddenField id="mapLocationCoordinate" runat="server" value="" />
    </div>
    <div class="clear"></div>
    <div class="mapContainer">
        <noscript>
            <asp:Label ID="lblMapInfoNonJS" runat="server" EnableViewState="false" />
        </noscript>
        <div class="myLocationMap">
            <asp:Button ID="currentLocationButton" runat="server" CssClass="hide locationCurrent" />
        </div>
        <div class="useLocationDiv">
            <asp:LinkButton ID="useLocationBtn" runat="server" EnableViewState="false" OnClick="useLocationBtn_Click" CssClass="useLocation" data-role="button" data-theme="a" ></asp:LinkButton>
        </div>
        <div class="viewJourneyDiv">
            <asp:Button ID="viewJourneyBtn" runat="server" CssClass="hide viewJourney" />
        </div>
        <div class="hide mapLoading">
            <asp:Image ID="loadingImage" runat="server" />
            <br />
            <asp:Label ID="loadingMessage" runat="server" />
        </div>
        <div id="mapDiv" class="map">
        </div>
        <div class="jshide">
            <div class="copyrightback">&copy; 2011 Microsoft Corporation</div>
            <div class="copyrightfront">&copy; 2011 Microsoft Corporation</div>
        </div>
    </div>
    <div class="clear"></div>
    <uc1:VenueMapControl ID="originVenueMapControl" runat="server" Visible="false"></uc1:VenueMapControl>
    <uc1:VenueMapControl ID="destinationVenueMapControl" runat="server" Visible="false"></uc1:VenueMapControl>
</asp:Content>
