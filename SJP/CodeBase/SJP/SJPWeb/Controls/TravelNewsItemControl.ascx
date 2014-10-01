<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelNewsItemControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.TravelNewsItemControl" %>
<div>
    <asp:Repeater ID="travelnewsItems" runat="server" OnItemDataBound="TravelNewsItems_DataBound">
        <ItemTemplate>
            <div class="tnItem">
                <asp:Label ID="lblNewsId" runat="server" EnableViewState="false" CssClass="displaynone"></asp:Label>
                <a ID="summaryLink" runat="server"></a>
                                           
                <div class="body">
                        <div class="affected">
                        <div class="affectedImage">
                            <asp:Image ID="affectedLocation" runat="server" Visible="false" EnableViewState="false" />
                        </div>
                                                   
                    </div>
                    <div class="summary">
                        <asp:Label ID="summaryText" CssClass="severity" runat="server" EnableViewState="false" />
                        <asp:Panel ID="affectedVenuesDiv" runat="server" Visible="false" CssClass="tnAffectedVenues">
                            <asp:Label ID="lblAffectedVenues" CssClass="tnBoldLabel" runat="server" EnableViewState="false" />
                            <asp:Label ID="affectedVenues" runat="server" EnableViewState="false" />
                        </asp:Panel>
                        <asp:Label ID="locationText" CssClass="location" runat="server" EnableViewState="false" />
                    </div>
                    <div id="detailDiv" class="detail" runat="server">
                                                    
                        <asp:Label ID="detailText" CssClass="detailText" runat="server" EnableViewState="false" />
                                                    
                        <asp:Label ID="travelAdvice" CssClass="travelAdvice" runat="server" Visible="false" EnableViewState="false" />

                        <div class="startdate">
                            <asp:Label ID="startDate" runat="server" EnableViewState="false" />
                        </div>
                    </div>
                    <div class="status">
                        <asp:Label ID="statusDate" runat="server" EnableViewState="false" />
                    </div>
                </div>
                <div class="incidenttype">
                    <div class="incidentImage">
                        <asp:Image ID="incidentType" runat="server" EnableViewState="false" />
                    </div>
                    <asp:Label ID="incidentText" runat="server" EnableViewState="false" />
                </div>
                <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv">
                    <asp:Label ID="lblDebugInfo" runat="server" EnableViewState="false" CssClass="debug" />
                </div>
                <div class="clearboth">
                </div>
            </div>
        </ItemTemplate>
        <AlternatingItemTemplate>
                <div class="tnItem">
                <asp:Label ID="lblNewsId" runat="server" EnableViewState="false" CssClass="displaynone"></asp:Label>
                <a ID="summaryLink" runat="server"></a>
                                            
                <div class="body">
                    <div class="affected">
                        <div class="affectedImage">
                            <asp:Image ID="affectedLocation" runat="server" Visible="false" EnableViewState="false" />
                        </div>
                                                    
                    </div>
                    <div class="summary">
                        <asp:Label ID="summaryText" CssClass="severity" runat="server" EnableViewState="false" />
                        <asp:Panel ID="affectedVenuesDiv" runat="server" Visible="false" CssClass="tnAffectedVenues">
                            <asp:Label ID="lblAffectedVenues" CssClass="tnBoldLabel" runat="server" EnableViewState="false" />
                            <asp:Label ID="affectedVenues" runat="server" EnableViewState="false" />
                        </asp:Panel>
                        <asp:Label ID="locationText" CssClass="location" runat="server" EnableViewState="false" />
                    </div>
                    <div id="detailDiv" class="detail" runat="server">
                                                    
                        <asp:Label ID="detailText" CssClass="detailText" runat="server" EnableViewState="false" />
                                                   
                        <asp:Label ID="travelAdvice" CssClass="travelAdvice" runat="server" Visible="false" EnableViewState="false" />

                        <div class="startdate">
                            <asp:Label ID="startDate" runat="server" EnableViewState="false" />
                        </div>
                    </div>
                    <div class="status">
                        <asp:Label ID="statusDate" runat="server" EnableViewState="false" />
                    </div>
                </div>
                <div class="incidenttype">
                    <div class="incidentImage">
                        <asp:Image ID="incidentType" runat="server" EnableViewState="false" />
                    </div>
                    <asp:Label ID="incidentText" runat="server" EnableViewState="false" />
                </div>
                <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv">
                    <asp:Label ID="lblDebugInfo" runat="server" EnableViewState="false" CssClass="debug" />
                </div>
                <div class="clearboth">
                </div>
            </div>
        </AlternatingItemTemplate>
    </asp:Repeater>
</div>