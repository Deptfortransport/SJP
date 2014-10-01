<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JourneyInputControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.JourneyInputControl" %>
<%@ Register Namespace="SJP.UserPortal.SJPMobile.Controls" TagPrefix="cc1" assembly="sjp.userportal.sjpmobile" %>
<%@ Register src="~/Controls/LocationControl.ascx" tagname="LocationControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/EventDateControl.ascx" tagname="EventDateControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/AccessibleOptionsControl.ascx" tagname="AccessibleOptionsControl" tagprefix="uc1" %>

<div class="journeyInput">

    <div class="locationsDiv">
        <div id="fromLocation" class="locationRow">
            <uc1:LocationControl ID="locationFrom" runat="server" />
        </div>

        <div id="toLocation" class="locationRow">
            <uc1:LocationControl ID="locationTo" runat="server" />
        </div>
        
        <div id="locationParkDiv" runat="server" class="locationParkRow">
            <asp:UpdatePanel ID="updateContentPark" runat="server" RenderMode="Block" UpdateMode="Always">
                <ContentTemplate>
                    <div id="park">
                        <asp:LinkButton ID="parkselected" CssClass="cycleParkLink" data-transition="none" data-role="button" runat="server" aria-live="polite"></asp:LinkButton>
                    </div>
                    <asp:HiddenField ID="selectedCycleParkID" runat="server" />
                    <div data-role="page" id="cycleParkMapContainer" class="cycleParkMapPage" runat="server">
                        <div data-role="header" data-add-back-btn="true" data-position="fixed" class="headerBack">
                            <h2 id="cycleParkSelectorLabel" runat="server" EnableViewState="false"></h2>
                            <asp:LinkButton ID="closeparkmap" runat="server" CssClass="topNavLeft" data-role="none" data-icon="delete"></asp:LinkButton>
                        </div>
                        <div data-role="content" id="cycleParkMapPage" class="locationParkMapDiv" runat="server">
                            <div class="locationParkInfoDiv">
                                <asp:Label ID="cycleParkMapImageLabel" runat="server" EnableViewState="false"></asp:Label>
                            </div>
                            <div id="wrapper" class="wrapper">
                                <div id="contentMap" class="contentMap" runat="server">
                                    <asp:Image ID="cycleParkMapImage" runat="server" />
                                    <asp:PlaceHolder ID="cycleParkLinks" runat="server"></asp:PlaceHolder>
                                </div>
                            </div>
                        </div>
                        <div class="menuZoom"><a href="#" class="anchorzoomin" data-role="button" id="zoomin">Zoom in</a><a href="#" class="anchorzoomout" data-role="button" id="zoomout">Zoom out</a></div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>    
        </div>
    </div>

    <div id="travelFromToToggleDiv" runat="server" class="travelFromToVenueDiv" visible="false" enableviewstate="false" data-role="none">
        <asp:Button ID="travelFromToVenueToggle" CssClass="" runat="server" OnClick="TravelFromToToggle_Click" OnClientClick="return toggleLocation();" data-role="none" />
    </div>
    
    <div class="clear"></div>

    <div class="eventDateTime">
        <uc1:EventDateControl ID="eventControl" runat="server" />
    </div>

    <div class="clear"></div>
    
    <div id="accessibleOptionsDiv" runat="server" class="accessibilityOptionsRow">
        <uc1:AccessibleOptionsControl ID="accessibleOptionsControl" runat="server" />
    </div>

    <div id="journeyTypeDiv" runat="server" class="journeyTypeRow">

        <div data-role="collapsible" data-collapsed="true" data-inline="true" class="collapseMagenta cycling collapse jshide">
            
            <h2 id="journeyTypeHeading" runat="server" class="ui-collapsible-heading"></h2>
            
            <fieldset data-role="controlgroup">
    
                <div class="ui-collapsible-content ui-controlgroup-controls jsshow">
                    <asp:RadioButtonList ID="journeyTypeRdo" runat="server" CellPadding="0" CellSpacing="0" Width="100%"></asp:RadioButtonList>
                </div>

	        </fieldset>
	    </div>

    </div>

    <div class="clear"></div>

    <div class="submittab">
        <div class="jshide">
            <asp:LinkButton ID="planJourneyBtn" runat="server" OnClick="planJourneyBtn_Click" EnableViewState="false" CssClass="buttonMag" data-role="button" data-theme="a" ></asp:LinkButton>
        </div>
        <noscript>
            <asp:Button ID="planJourneyBtnNonJS" runat="server" OnClick="planJourneyBtn_Click" EnableViewState="false" CssClass="buttonMag buttonMagNonJS" ></asp:Button>
        </noscript>
    </div>

    <asp:HiddenField ID="previousLocationInputMode" runat="server" />
    <asp:HiddenField ID="journeyTypeSelected" runat="server" />
</div>
