<%@ Page Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="PrintableMap1.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.PrintableMap1" %>
<%@ Register src="~/Controls/DetailsCycleControl.ascx" tagname="DetailsCycleControl" tagprefix="uc1" %>

<asp:Content ID="contentMain" ContentPlaceHolderID="contentMain" runat="server">
    <div id="modeInformation">
        <asp:HiddenField id="journeyPoints" runat="server" value="" />
        <asp:HiddenField ID="printerFriendlyFlag" runat="server" value="true" />
    </div>
    <div id="cycleJourney" runat="server">
        <table class="cycleLegTable">
            <tr>
                <td>
                    <div class="header">
                        <div class="journeyDirection">
                                <asp:Label ID="journeyDirection" runat="server" />
                        </div>
                        <div class="journeyHeading">
                            <asp:Label ID="journeyHeader" CssClass="journeyHeader" runat="server" />
                            <br />
                            <asp:Label ID="journeyDateTime" CssClass="journeyDateTime" runat="server" />
                        </div>
                        <div class="clearboth"></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <!-- cycle journey map -->
                    <div class="mapContainer">
                        <noscript>
                            <asp:Label ID="lblMapInfoNonJS" runat="server" EnableViewState="false" />
                        </noscript>
                        <div id="mapDiv" class="map">
                            <span>&nbsp;</span>
                        </div>
                        <div class="copyrightdiv">
                            <div class="copyrightback">&copy; 2011 Microsoft Corporation</div>
                            <div class="copyrightfront">&copy; 2011 Microsoft Corporation</div>
                        </div>
                    </div>
                    <div class="mapLegend">
                        <table>
                            <tr>
                                <td>
                                    <!-- origin -->
                                    <img src="../VersionGTW/Images/maps/Blue-flag_no-shadow.png" alt=" " />
                                </td>
                                <td>
                                    <asp:Label ID="startLocation" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="../VersionGTW/Images/maps/Pink-flag_no-shadow.png" alt=" " />
                                </td>
                                <td>
                                    <asp:Label ID="endLocation" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height:20px;"></td>
            </tr>
            <tr>
                <td align="left" class="mapCycleInstructions">
                    <div class="divCycleInstructions">
                        <!-- cycle journey description -->
                        <uc1:DetailsCycleControl ID="cycleLeg" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
