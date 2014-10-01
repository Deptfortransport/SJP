<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VenueMapControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.VenueMapControl" %>

<asp:Panel ID="venueMapPage" runat="server" data-role="page">
    <div class="headerBack pageBack">
        <h2 ID="venueMapPageHeading" runat="server" EnableViewState="false"></h2>
        <asp:LinkButton ID="closevenuemap" runat="server" CssClass="topNavLeft" data-role="none" data-icon="delete"></asp:LinkButton>
        <asp:HyperLink ID="venueMapPdf" runat="server" CssClass="topNavRight" data-rel="external" data-transition="none"  />
	</div>
	<div data-role="content" class="ui-content">
        <div class="venueMapTitleDiv">
            <asp:Label ID="venueMapLbl" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="venueMapDiv">
            <div id="wrapper" class="wrapper">
                <div class="venueMapImgDiv">
                    <asp:Image ID="venueMap" runat="server" />
                </div>
            </div>
        </div>
        <div class="menuZoom"><a href="#" data-role="button" class="anchorzoomin" id="zoomin">Zoom in</a><a href="#" data-role="button" class="anchorzoomout" id="zoomout">Zoom out</a></div>
    </div>
</asp:Panel>