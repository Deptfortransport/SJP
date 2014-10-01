<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsLegControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.DetailsLegControl" %>
<%@ Register src="~/Controls/VehicleFeaturesControl.ascx" tagname="VehicleFeaturesControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/AccessibleFeaturesControl.ascx" tagname="AccessibleFeaturesControl" tagprefix="uc1" %>

<asp:Panel ID="pnlMapLocationRow" runat="server" EnableViewState="true" Visible="false">
    <div class="rowMap">
        <div class="locationMapLinkDiv" id="locationMapLinkDiv" runat="server">
            <asp:HyperLink ID="locationMapLink" runat="server" CssClass="buttonMap" rel="external" />
        </div>
    </div>
</asp:Panel>

<% // Interchange row displayed when CJP has removed a short walk leg between a station change  %>
<asp:Panel ID="pnlInterchangeRow" runat="server" EnableViewState="true" Visible="false">
<div class="leg">
    <div class="row">

        <div class="row1">
            
            <div class="columnEmpty">
            </div>

            <div class="columnTime">
                <div class="timeArrive">
                    <asp:Label ID="interchangeArrive" runat="server" EnableViewState="true" />
                    <asp:Label ID="interchangeArriveTime" runat="server" EnableViewState="true" />
                </div>
                <br />
                <div class="timeDepart">
                </div>
            </div>

            <div class="columnNode">
                <div class="nodeImage">
                    <asp:Image ID="interchangeNodeImage" runat="server" EnableViewState="true" />
                </div>
            </div>

            <div class="columnLocation">
                <asp:Label ID="interchangeLocation" runat="server" EnableViewState="true" />
            </div>

        </div>

    </div>
</div>
</asp:Panel>
<% // End Interchange row %>

 <div class="leg">
    <div class="row">

        <div class="row1">
            
            <div class="columnEmpty">
            </div>

            <div class="columnTime">
                <div class="timeArrive">
                    <asp:Label ID="legArrive" runat="server" EnableViewState="true" />
                    <asp:Label ID="legArriveTime" runat="server" EnableViewState="true" />
                </div>
                <br />
                <div class="timeDepart">
                    <asp:Label ID="legDepart" runat="server" EnableViewState="true" />
                    <asp:Label ID="legDepartTime" runat="server" EnableViewState="true" />
                </div>
            </div>

            <div class="columnNode">
                <div class="nodeImage">
                    <asp:Image ID="legNodeImage" runat="server" EnableViewState="true" />
                </div>
            </div>

            <div class="columnLocation">
                <asp:Label ID="legLocation" runat="server" EnableViewState="true" />
                
                <uc1:AccessibleFeaturesControl ID="legLocationAccessibleFeatures" runat="server"></uc1:AccessibleFeaturesControl>
                
            </div>

        </div>

        <div class="row2">
            
            <div class="columnMode">
                <asp:Image ID="legMode" runat="server" EnableViewState="true" />
                <br />
                <asp:Label ID="legModeDetail" runat="server" EnableViewState="true" />
            </div>

            <div class="columnEmpty"> 
            </div>
            
            <div class="columnLine">
                <div class="lineImage">
                    <asp:Image ID="legLineImage" runat="server" EnableViewState="true" />
                </div>
            </div>

            <div class="columnDetail">
                <div>
                    <div>
                        <div class="arrow_btn" id="arrow_btn" runat="server" visible="false">
                            <asp:ImageButton ID="showDetail" runat="server" CommandName="Select" Visible="false" />
                        </div>
                        <asp:button ID="legInstructionBefore" CssClass="linkButton detailLinkButton" runat="server" Visible="false" />
                        <asp:Label ID="legInstruction" runat="server" EnableViewState="true" />
                        <asp:Label ID="legInstructionTime" runat="server" EnableViewState="true" />
                        <asp:HyperLink ID="gpxLink" runat="server" Target="_blank" Visible="false" />
                        <a class="tooltip_information gpxInfo" href="#" onclick="return false;" id="gpxTooltipInfo" runat="server" enableviewstate="true" visible="false">
                            <asp:Image ID="gpxInfoImage" CssClass="information" runat="server" Visible="false" />
                        </a>
                    </div>
                    <div class="jssettings">
                        <asp:HiddenField ID="legInstructionBeforeId" runat="server" />
                    </div>
                </div>

                <div class="vehicleFeatures" id="vehicleFeaturesDiv" runat="server">
                    <uc1:VehicleFeaturesControl ID="legVehicleFeatures" runat="server"></uc1:VehicleFeaturesControl>
                    <uc1:AccessibleFeaturesControl ID="legAccessibleFeatures" runat="server"></uc1:AccessibleFeaturesControl>
                </div>

                <div class="jsLegLinks jshide" id="travelNotesLinkDiv" runat="server">
                    <asp:image CssClass="linkType" ID="travelNotesLinkSymbol" runat="server" />
                    <a class="jsLegLink travelNotesLink" id="travelNotesLink" href="#" runat="server" visible="false" />
                </div>
                <div id="divNotes" runat="server" class="detailNotes hide jsshow">
                </div>

                <div class="bookTicketDiv" id="bookTicketDiv" runat="server">
                    <asp:Label ID="bookTicketPhoneText" runat="server" Enableviewstate="false"></asp:Label>
                    <asp:HyperLink ID="travelcardLink" CssClass="travelcardLink" runat="server" >
                        <asp:Label CssClass="linkText" ID="travelcardLinkText" runat="server" Enableviewstate="false"></asp:Label>
                        <br />
                    </asp:HyperLink>
                </div>

                <div class="accessibleInfoDiv" id="accessibleInfoDiv" runat="server">
                    <asp:Label ID="accessibleInfoText" runat="server" EnableViewState="true" />                    
                </div>
                
                <div class="directionsMapLinkDiv" id="directionsMapLinkDiv" runat="server">
                    <asp:LinkButton ID="directionsMapLink" runat="server" OnClick="directionsMapLink_Click" CssClass="linkMap" data-role="none" />
                </div>
            </div>
            
        </div>
    </div>
        
</div>

<% // End row displayed for the last journey leg to show end location %>
<asp:Panel ID="pnlEndLocationRow" runat="server" EnableViewState="true" Visible="false">
<div class="leg">
    <div class="row">

        <div class="row1">
            
            <div class="columnEmpty">
            </div>

            <div class="columnTime">
                <div class="timeArrive">
                    <asp:Label ID="endArrive" runat="server" EnableViewState="true" />
                    <asp:Label ID="endArriveTime" runat="server" EnableViewState="true" />
                </div>
                <br />
                <div class="timeDepart">
                </div>
            </div>

            <div class="columnNode">
                <div class="nodeImage">
                    <asp:Image ID="endNodeImage" runat="server" EnableViewState="true" />
                </div>
            </div>

            <div class="columnLocation">
                <asp:Label ID="endLocation" runat="server" EnableViewState="true" />
                
                <uc1:AccessibleFeaturesControl ID="endLocationAccessibleFeatures" runat="server"></uc1:AccessibleFeaturesControl>

            </div>

        </div>

    </div>
</div>
</asp:Panel>

<asp:Panel ID="pnlMapEndLocationRow" runat="server" EnableViewState="true" Visible="false">
    <div class="rowMap rowEndMap">
        <div class="locationMapLinkDiv endLocationMapLinkDiv" id="endLocationMapLinkDiv" runat="server">
            <asp:HyperLink ID="endLocationMapLink" runat="server" CssClass="buttonMap" rel="external" />
        </div>
    </div>
</asp:Panel>
<% // End End location row %>
