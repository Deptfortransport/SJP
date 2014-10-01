<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="Map2.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.Map2" %>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMain" runat="server">

    <noscript>
        <asp:Label ID="lblMapInfoNonJS" runat="server" EnableViewState="false" />
    </noscript>
    <div class="clear"></div>
    <div class="mapContainer">
        
        <asp:HiddenField ID="mapStartLocationCoordinate" runat="server" Value="" />
        <asp:HiddenField ID="mapEndLocationCoordinate" runat="server" Value="" />
        <asp:HiddenField ID="mapTravelMode" runat="server" Value=""  Visible="false"/>

        <div id="directionsPanel" class="directions"></div>
        <div id="mapCanvas" class="map"></div>
    </div>
    <br />

</asp:Content>
