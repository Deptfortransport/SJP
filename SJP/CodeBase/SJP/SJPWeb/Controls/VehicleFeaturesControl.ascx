<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehicleFeaturesControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.VehicleFeaturesControl" %>

<asp:Repeater ID="rptrVehicleFeatures" runat="server" EnableViewState="false" OnItemDataBound="rptrVehicleFeatures_ItemDataBound">
    <HeaderTemplate>
        <div class="clearboth">
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Image ID="imgVehicleFeature" runat="server" EnableViewState = "false" />
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>