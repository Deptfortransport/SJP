<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccessibleFeaturesControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.AccessibleFeaturesControl" %>

<asp:Repeater ID="rptrAccessibleFeatures" runat="server" EnableViewState="false" OnItemDataBound="rptrAccessibleFeatures_ItemDataBound">
    <HeaderTemplate>
        <div class="clearboth">
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Image ID="imgAccessibleFeature" runat="server" EnableViewState = "false" />
    </ItemTemplate>
    <FooterTemplate>
        </div>
        <br />
    </FooterTemplate>
</asp:Repeater>
