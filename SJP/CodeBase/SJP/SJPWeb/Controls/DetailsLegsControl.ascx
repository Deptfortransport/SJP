<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsLegsControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.DetailsLegsControl" %>
<%@ Register src="~/Controls/DetailsLegControl.ascx" tagname="LegControl" tagprefix="uc1" %>

<div class="legsDetail" >
    <asp:Repeater ID="legsDetailView" runat="server" OnItemDataBound="LegsDetailView_DataBound">
         <HeaderTemplate>
            <asp:Label ID="lblRoutingDetails" runat="server" Visible="false"></asp:Label>
         </HeaderTemplate>
         <ItemTemplate>
            <uc1:LegControl ID="legControl" runat="server" />
         </ItemTemplate>
         <FooterTemplate>
         </FooterTemplate>
    </asp:Repeater>
</div>