<%@ Page Language="C#" MasterPageFile="~/SJPWeb.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Map1.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.Map" %>
<%@ Register src="~/Controls/DetailsCycleControl.ascx" tagname="DetailsCycleControl" tagprefix="uc1" %>
<%@ Register Namespace="SJP.Common.Web" TagPrefix="uc2" assembly="sjp.common.web" %> 

    <asp:Content ID="contentMain" ContentPlaceHolderID="contentMain" runat="server">
        <div id="modeInformation">
            <asp:HiddenField id="journeyPoints" runat="server" value="" />
        </div>
        <div id="cycleJourney" runat="server">
            <table class="cycleLegTable">
                <tr>
                    <td colspan="2" width="980px">
                        <table width="980px">
                            <tr>
                                <td>
                                    <h1 id="cycleHeader" class="cycleHeader" runat="server"></h1>
                                </td>
                                <td>
                                    <div class="printerFriendly floatright">
                                        <uc2:LinkButton ID="btnPrinterFriendly" CssClass="linkButton floatright" MouseOverClass="linkButtonMouseOver" runat="server" />
                                        <noscript>
                                            <asp:hyperlink id="lnkPrinterFriendly" runat="server" target="_blank"></asp:hyperlink>
                                        </noscript>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblCycleSoftContent" runat="server" />
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
                    <td align="left" class="mapCycleInstructions">
                        <div class="divCycleInstructions">
                            <div class="divCycleInstructionsHeader">
                                <table width="300px">
                                    <tr>
                                        <td class="instructionHeading">
                                            <asp:Label ID="instructionHeading" runat="server" />
                                        </td>
                                        <td class="arriveHeading">
                                            <asp:Label ID="arriveHeading" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- cycle journey description -->
                            <uc1:DetailsCycleControl ID="cycleLeg" runat="server" />
                        </div>
                    </td>
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
            </table>
        </div>
        <div class="mapButtonDiv">
            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="submit btnSmallPink floatleft" EnableViewState="false" />
        </div>
    </asp:Content>
