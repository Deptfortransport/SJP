<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParkAndRideJourneyLocations.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.ParkAndRideJourneyLocations" %>
<div class="JourneyLocations" id="JourneyLocations">
    <asp:Panel ID="pnlParkAndRideCarParks" runat="server">
        <div id="venueMapDiv" runat="server" class="venueMap" Enableviewstate="false">>
            <div class="jssettings">
                <asp:HiddenField ID="mapBulletTarget" runat="server" />
            </div>
            <asp:Image ID="venueMap" runat="server" />
            <asp:PlaceHolder ID="mapBullets" runat="server" />
        </div>
        <div class="preferredParks">
            <asp:Label ID="preferredParksHeading" CssClass="label" runat="server" EnableViewState="false" />
            <asp:DropDownList ID="preferredParksOptions" CssClass="options" runat="server" />
        </div>
        <div id="parkAndRideBooking" runat="server" class="parkAndRideBooking" Enableviewstate="false">
             <div class="bookingUrl floatright" >
                <asp:HyperLink ID="parkAndRideBookingURL" runat="server" Enableviewstate="false"></asp:HyperLink>
                <asp:Image ID="openInNewWindow" runat="server" Enableviewstate="false"/>
             </div>
             <div class="clearboth"></div>
        </div>
        <div class="parkAndRideTimeSlot">
            <asp:Label ID="parkAndRideTimeSlotHeading" CssClass="label" runat="server" EnableViewState="false" />
            <asp:DropDownList ID="drpTimeSlotOptions" CssClass="options" runat="server" />
        </div>
        <div class="parkAndRideNote" >
            <asp:label id="parkAndRideNote" runat="server" EnableViewState="false" />
        </div>
        <div class="bookingNote" >
            <asp:label id="parkAndRideBookingNote" runat="server" EnableViewState="false" />
        </div>
        <div class="timeSlotNote" >
            <asp:label id="parkAndRideTimeSlotNote" runat="server" EnableViewState="false" />
        </div>
    </asp:Panel>
</div>