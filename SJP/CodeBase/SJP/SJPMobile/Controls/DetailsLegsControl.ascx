<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsLegsControl.ascx.cs"
    Inherits="SJP.UserPortal.SJPMobile.Controls.DetailsLegsControl" %>
<%@ Register Src="~/Controls/DetailsLegControl.ascx" TagName="LegControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/VenueMapControl.ascx" TagName="VenueMapControl" TagPrefix="uc1" %>
<div class="legsDetail">
    <asp:Repeater ID="legsDetailView" runat="server" OnItemDataBound="LegsDetailView_DataBound">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <uc1:LegControl ID="legControl" runat="server" />
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
</div>
<uc1:VenueMapControl ID="destinationVenueMapControl" runat="server" Visible="false">
</uc1:VenueMapControl>
<uc1:VenueMapControl ID="originVenueMapControl" runat="server" Visible="false"></uc1:VenueMapControl>
<div id="additionalInfoPage" data-role="page" style="display: none;">
    <div id="additionaInfoDialog">
        <div class="headerBack" data-role="header" data-add-back-btn="true" data-position="fixed">
            <h2 id="additionalInfoHeader" runat="server" enableviewstate="false">
            </h2>
            <asp:LinkButton ID="closeinfodialog" runat="server" CssClass="additionalNotesClose"
                data-role="button" data-icon="delete"></asp:LinkButton>
        </div>
        <div id="additionalInfoNotes" data-role="content" class="ui-content">
        </div>
    </div>
</div>
