<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CycleJourneyLocations.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.CycleJourneyLocations" %>
<div class="JourneyLocations" id="JourneyLocations">
    <asp:Panel ID="pnlCycleParks" CssClass="cycleParks" runat="server">
        <asp:Label ID="usetheMap" class="venueMapLabel" runat="server" EnableViewState="false"></asp:Label>
        <div class="venueMap">
            <div class="jssettings">
                <asp:HiddenField ID="mapBulletTarget" runat="server" />
            </div>
            <asp:Image ID="venueMap" runat="server" />
            <asp:PlaceHolder ID="cycleParkBullets" runat="server" />
        </div>
        <div class="preferredParks">
            <h2 ID="preferredParksHeading" runat="server"></h2>
            <asp:Label ID="preferredParksInfo" CssClass="labelInfo" runat="server" />
            <asp:DropDownList ID="preferredParksOptions" CssClass="options" runat="server" />
        </div>
        <div class="typeOfRoute">
            <asp:Label ID="typeOfRouteHeading" CssClass="label" runat="server" />
            <asp:DropDownList ID="typeOfRouteOptions" CssClass="options" runat="server" />
        </div>
    </asp:Panel>
</div>