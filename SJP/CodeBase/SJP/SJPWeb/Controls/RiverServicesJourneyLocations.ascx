<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiverServicesJourneyLocations.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.RiverServicesJourneyLocations" %>

<%@ Register src="RiverSerivceResults.ascx" tagname="RiverSerivceResults" tagprefix="uc1" %>

<div class="JourneyLocations" id="JourneyLocations">
    <asp:Panel ID="pnlRiverServices" runat="server">
        
        <asp:Label ID="usetheMap" class="venueMapLabel" runat="server"></asp:Label>
        <div class="venueMap">
            <div class="jssettings">
                <asp:HiddenField ID="mapBulletTarget" runat="server" />
            </div>
            <asp:Image ID="venueMap" runat="server" />
            <asp:PlaceHolder ID="mapBullets" runat="server" />
        </div>
        
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:HiddenField ID="waitCount" runat="server" />
                <asp:Timer ID="waitTimer" runat="server"  >
                </asp:Timer>
                
                <div class="serviceRoutes" id ="riverServiceRoutes" runat="server">
                    <asp:Label ID="routeSelection" CssClass="label" runat="server" EnableViewState="false" />
                    <br />
                    <asp:DropDownList ID="routeSelectionOptions" CssClass="options" runat="server" />

                    <div class="submit">
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="submit btnSmallPink floatleft" EnableViewState="false" />
                        <asp:Button ID="btnFindDepartureTimes" runat="server" OnClick="btnFindDepartureTimes_Click" CssClass="submit btnLargePink floatright" EnableViewState="false" />
                    </div>
                    <div class="clearboth"></div>
                </div>

                <div class="serviceResults" id="riverServiceResults" runat="server" visible="false">
                    
                    <div id="journeyProgress" runat="server" class="journeyProgress" >
                           <div class="progressBackgroundFilter"></div>
                            <div class="processMessage"> 
                                 <asp:Image ID="loading" runat="server" />
                                 <br /><br />
                                 <div>
                                    <asp:Label ID="loadingMessage" runat="server" />
                                 </div>
                            </div>
                          
                    </div>
                    <asp:Label ID="riverServiceResultsHeading" runat="server" Visible="false" />                    
                    <uc1:RiverSerivceResults ID="riverServiceResultOutward" runat="server" Visible="false" />
                    <uc1:RiverSerivceResults ID="riverServiceResultReturn" runat="server" Visible="false" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        
       
    </asp:Panel>
</div>

