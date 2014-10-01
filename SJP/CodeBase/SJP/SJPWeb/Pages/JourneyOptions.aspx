<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="JourneyOptions.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.JourneyOptions" %>
<%@ Register src="~/Controls/DetailsSummaryControl.ascx" tagname="JourneySummaryControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/JourneySelectControl.ascx" tagname="JourneySelectControl" tagprefix="uc1" %>
<%@ Register Namespace="SJP.UserPortal.SJPWeb.Controls" TagPrefix="uc1" assembly="sjp.userportal.sjpweb" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">

    <div class="jssettings">
       <asp:HiddenField ID="handoff" runat="server" />
       <asp:HiddenField ID="retailer" runat="server" />
       <asp:HiddenField ID="journeyKeys" runat="server" />
       <asp:HiddenField ID="journeyHash" runat="server" />
       <asp:HiddenField ID="printerFriendlyUrl" runat="server" />
    </div>
    
    <asp:UpdatePanel ID="journeyOptionsUpdater" runat="server">
       
        <ContentTemplate>
            <asp:HiddenField ID="waitCount" runat="server" />
             
            <div id="journeyProgress" runat="server" class="journeyProgress" >
                    <div class="sjpSeparator" ></div>
                    <div class="progressBackgroundFilter"></div>
                    <div class="processMessage"> 
                         <asp:Image ID="loading" runat="server" />
                         <br /><br />
                         <div>
                            <asp:Label ID="loadingMessage" runat="server" />
                            <noscript>
                                <br />
                                <asp:Label ID="longWaitMessage" runat="server" />
                                <asp:HyperLink ID="longWaitMessageLink" runat="server" />
                            </noscript>
                            
                         </div>
                    </div>
                    <div class="sjpSeparator" ></div>
            </div>
            <asp:Timer ID="waitTimer" runat="server" OnTick="WaitTimer_Tick"  >
            </asp:Timer>
            
            <div id="messageSeprator" runat="server" visible="false">
                <div class="sjpSeparator" ></div>
            </div>

            <div id="journeyInfoDiv1" runat="server" visible="false" class="journeyInfo1">
                <asp:Label ID="lblJourneyInfo1" runat="server" />
            </div>

            <div id="journeyInfoDiv2" runat="server" visible="false" class="journeyInfo2">
                <asp:Label ID="lblJourneyInfo2" runat="server" />
                <asp:HyperLink ID="lnkJourneyFAQs" runat="server" />
            </div>

            <uc1:JourneySelectControl ID="journeySelectControl" runat="server" />

            <uc1:JourneySummaryControl ID="outwardSummaryControl" runat="server" Visible="false" />
            
            <uc1:JourneySummaryControl ID="returnSummaryControl" runat="server" Visible="false" />
            
            <div>
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="submit btnSmallPink floatleft" EnableViewState="false" />
                <asp:Image ID="openInNewWindow" style="display:none;" CssClass="retailerInNewWindow" runat="server" />
                <asp:Button ID="btnTickets" runat="server" OnClick="btnTickets_Click" Visible="false" CssClass="submit btnMediumPink floatright" EnableViewState="false" />
                <a class="tooltip_information bookTicketsInfo floatright" href="#" onclick="return false;" id="tooltip_information" runat="server" enableviewstate="false" visible="false">
                    <asp:Image ID="bookTicketsInfo" CssClass="information" runat="server" Visible="false" />
                </a>
            </div>

            <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv">
                <asp:Label ID="lblDebugInfo" runat="server" EnableViewState="false" CssClass="debug" />
            </div>
           
        </ContentTemplate>
        
    </asp:UpdatePanel>

</div>     
    
</asp:Content>
