<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JourneyOptionTabContainer.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.JourneyOptionTabContainer" %>

<%@ Register src="~/Controls/PublicJourneyOptionsTab.ascx" tagname="PublicJourneyTab" tagprefix="uc1" %>
<%@ Register src="~/Controls/RiverServicesOptionsTab.ascx" tagname="RiverServicesTab" tagprefix="uc1" %>
<%@ Register src="~/Controls/ParkAndRideOptionsTab.ascx" tagname="ParkAndRideOptionsTab" tagprefix="uc1" %>
<%@ Register src="~/Controls/BlueBadgeOptionsTab.ascx" tagname="BlueBadgeOptionsTab" tagprefix="uc1" %>
<%@ Register src="~/Controls/CycleOptionsTab.ascx" tagname="CycleOptionsTab" tagprefix="uc1" %>

<div class="sjp_jo_tab">
    <div class="instruction">
        <asp:Label ID="lblJourneyOptions" runat="server" EnableViewState="false" />
        <br />
    </div>
    <asp:HiddenField ID="hdnActiveTab" runat="server" />
    <div class="sjp_tab_headers">
        <ul>
            <li id="pjLink" class="pjTab" runat="server">
            <asp:Button ID="publicJourney" runat="server"  Text="Public Transport" OnClick ="TabClicked"  />
            </li>
            <li id="rsLink" class="rsTab" runat="server">
                <asp:Button ID="riverServices" runat="server" Text="River Services" OnClick ="TabClicked" />
            </li>
            <li id="prLink" class="prTab" runat="server">
                <asp:Button ID="parkAndRide" runat="server" Text="Park & Ride" OnClick="TabClicked"  />
            </li>
            <li id="bbLink" class="bbTab" runat="server">
                <asp:Button ID="blueBadge" runat="server" Text="Blue Badge" OnClick="TabClicked" />
            </li>
            <li id="cyLink" class="cyTab" runat="server">
                <asp:Button ID="cycle" runat="server" Text="Cycle" OnClick="TabClicked" />
            </li>
        </ul>
        <div class="clearboth"></div>
        <div class="sjp_tab_highlight">
            <div id="divHighlight" runat="server" enableviewstate="false" class=""></div>
        </div>
    </div>
    <div class="sjp_tab_body">
        <div class="content_wrapper">
            <div id="pjTab" runat="server" class="tab">
                <uc1:PublicJourneyTab id="publicJourneyTab" runat="server"  />
            </div>
            <div id="rsTab" runat="server" class="tab">
                <uc1:RiverServicesTab ID="riverServicesTab" runat="server"  />
            </div>
            <div id="prTab" runat="server" class="tab">
                <uc1:ParkAndRideOptionsTab ID="parkandrideTab" runat="server"  />
            </div>
            <div id="bbTab" runat="server" class="tab">
                <uc1:BlueBadgeOptionsTab ID="blueBadgeTab" runat="server"  />
            </div>
            <div id="cyTab" runat="server" class="tab">
                <uc1:CycleOptionsTab ID="cycleTab" runat="server"  />
            </div>
        </div>
        <div class="clearboth"></div>
    </div>
    
</div>



   