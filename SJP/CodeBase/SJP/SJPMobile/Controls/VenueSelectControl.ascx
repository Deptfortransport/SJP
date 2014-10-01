<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VenueSelectControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.VenueSelectControl" %>
<%@ Register Namespace="SJP.Common.Web" TagPrefix="cc2" assembly="sjp.common.web" %>


<div data-role="page" id="venuespage">
    <div class="headerBack">
        <h2 ID="venuesSelectorLabel" runat="server" EnableViewState="false"></h2>
        <asp:LinkButton ID="closevenues" runat="server" CssClass="topNavLeft" data-role="none" data-icon="delete"></asp:LinkButton>
	</div>
	<div data-role="content" class="ui-content">
        <div id="allVenuesDiv" runat="server" class="allVenuesDiv">
            <asp:LinkButton ID="allVenuesButton" runat="server" data-role="button" Visible="false"></asp:LinkButton>
        </div>
        <cc2:GroupRadioButtonList ID="locationSelector" CssClass="locationDrop" runat="server">
        </cc2:GroupRadioButtonList>
    </div>
</div>
