<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="TravelNews.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.TravelNews" %>
<%@ Register src="~/Controls/UndergroundStatusControl.ascx" tagname="UndergroundStatusControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/TravelNewsControl.ascx" tagname="TravelNewsControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/VenueSelectControl.ascx" tagname="VenueSelectControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/WaitControl.ascx" tagname="WaitControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    
    <div class="tnFilter">
        <div class="collapseMagenta collapse jsshow" data-role="collapsible" data-collapsed="true" data-inline="true">
            
            <h2 ID="newsModeHeading" runat="server" class="ui-collapsible-heading" enableviewstate="false"></h2>
            
            <fieldset data-role="controlgroup">
                <div class="ui-collapsible-content ui-controlgroup-controls jsshow">
                    <legend id="newsModeOptionsLegend" runat="server" class="screenReaderOnly" enableviewstate="false" />
                
                    <asp:radiobuttonlist ID="newsModes" runat="server" CssClass="tnFilterOptions" RepeatLayout="Flow" ></asp:radiobuttonlist>
                    <noscript>
                        <asp:Button ID="tnFilterBtnNonJS" runat="server" CssClass="tnFilterBtnNonJS" OnClick="tnModeDrp_SelectedIndexChanged" EnableViewState="false" />
                    </noscript>
                </div>
            </fieldset>
        </div>
        <asp:Label ID="selectedVenue" runat="server" CssClass="tnFilterVenue"></asp:Label>
        <uc1:VenueSelectControl ID="venueSelectControl" runat="server"></uc1:VenueSelectControl>
    </div>

    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            
            <div id="refreshLinkDiv" runat="server" class="refreshLinkDiv">
                <asp:HyperLink id="refreshLink" runat="server" EnableViewState="false" class="refreshLink"></asp:HyperLink>
            </div>

            <uc1:TravelNewsControl ID="travelNewsControl" runat="server" />
            <uc1:UndergroundStatusControl ID="undergroundStatusControl" runat="server" />

            <div class="waitContainer hide">
                <uc1:WaitControl ID="waitControl" runat="server" />
            </div>

            <div>
                <asp:HiddenField ID="venueNaptans" runat="server" />
            </div>

            <div class="providedBy">
                <asp:Label ID="providedByLbl" runat="server" ></asp:Label>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<%--AutoPostBack="true" OnSelectedIndexChanged="tnModeDrp_SelectedIndexChanged"--%>
