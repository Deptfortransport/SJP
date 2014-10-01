<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="JourneyLocations.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.JourneyLocations" %>
<%@ Register Src="~/Controls/CycleJourneyLocations.ascx" TagName="CycleJourneyLocations" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ParkAndRideJourneyLocations.ascx" TagName="ParkAndRideJourneyLocations" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/RiverServicesJourneyLocations.ascx" TagName="RiverServicesJourneyLocations" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHeading" runat="server">

    <div id="sectionSubHeader" class="sectionSubHeader floatleft">
        <h2 id="sjpHeading" runat="server" enableviewstate="false"></h2>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">

    <div id="messageSeprator" runat="server" visible="false">
        <div class="sjpSeparator" ></div>
    </div>

    <uc1:CycleJourneyLocations id="cycleJourneyLocations" runat="server" Visible="false" />
    <uc1:ParkAndRideJourneyLocations id="parkAndRideJourneyLocations" runat="server" Visible="false" />
    <uc1:RiverServicesJourneyLocations id="riverServicesJourneyLocations" runat="server" Visible="false" />
    
    <asp:UpdatePanel ID="commandUpdatePanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <ContentTemplate>
            <div class="journeyLocationsSubmit">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="submit btnSmallPink floatleft" EnableViewState="false" />
                <asp:Button ID="btnPlanJourney" runat="server" OnClick="btnPlanJourney_Click" CssClass="submit btnLargePink floatright" Visible="false" EnableViewState="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>
    
</asp:Content>
