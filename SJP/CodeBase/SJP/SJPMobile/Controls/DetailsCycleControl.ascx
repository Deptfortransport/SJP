<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsCycleControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.DetailsCycleControl" %>

<div class="cycleLeg">
    <div class="summary">
        <div>
            <strong><asp:Label ID="lblDistanceTotalHead" runat="server"></asp:Label></strong><asp:Label ID="lblDistanceTotal" runat="server"></asp:Label>
            <strong><asp:Label ID="lblDurationTotalHead" runat="server"></asp:Label></strong><asp:Label ID="lblDurationTotal" runat="server"></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblOptions" runat="server"></asp:Label>
        </div>
    </div>
     <div class="detail">
        <asp:Repeater ID="legDetail" runat="server" OnItemDataBound="legDetail_DataBound"  >
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr class="headerRow">
                            <th class="distance" colspan="2">
                                <asp:Label ID="distanceHeading" runat="server"></asp:Label>
                            </th>
                            <th class="instruction">
                                <asp:Label ID="instructionHeading" runat="server"></asp:Label>
                            </th>
                            <th class="arrive">
                                <asp:Label ID="arriveHeading" runat="server"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                        <tr class="detailRow odd">
                            <td class="detailCol distance">
                                <asp:Label ID="distance"  runat="server"   />
                            </td>
                            <td class="detailCol infoimage" id="cyclePathImageCell">
                                <asp:Image ID="cyclePathImage" runat="server"  visible="false"/>
                                <br id="cyclePathImageSeparator" runat="server" visible="false" />
                                <asp:Image ID="cycleManoeuvreImage" runat="server" visible="false" />
                            </td>
                            <td class="detailCol instruction">
                                <asp:Label ID="instruction" runat="server"   />
                                <div class="showDetails">
                                    <div class="row" id="cyclePathName" runat="server" ></div>
                                    <div class="row" id="cycleInstruction" runat="server" ></div>
                                    <div class="row" id="cycleAttributeText" runat="server" ></div>
                                </div>
                                <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv debugInfoDivCycle">
                                    <asp:Label ID="lblDebugInfo" runat="server" EnableViewState="false" CssClass="debug" />
                                </div>
                            </td>
                            <td class="detailCol arrive">
                                 <asp:Label ID="arrive" runat="server"   />
                            </td>
                        </tr>

            </ItemTemplate>
            <AlternatingItemTemplate>
                         <tr class="detailRow">
                            <td class="detailCol distance">
                                <asp:Label ID="distance"  runat="server"   />
                            </td>
                            <td class="detailCol infoimage" id="cyclePathImageCell">
                                <asp:Image ID="cyclePathImage" runat="server" visible="false" />
                                <br id="cyclePathImageSeparator" runat="server" visible="false" />
                                <asp:Image ID="cycleManoeuvreImage" runat="server" visible="false" />
                            </td>
                            <td class="detailCol instruction">
                                <asp:Label ID="instruction" runat="server"  />
                                 <div class="showDetails">
                                    <div class="row" id="cyclePathName" runat="server" ></div>
                                    <div class="row" id="cycleInstruction" runat="server" ></div>
                                    <div class="row" id="cycleAttributeText" runat="server" ></div>
                                </div>
                                <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv debugInfoDivCycle">
                                    <asp:Label ID="lblDebugInfo" runat="server" EnableViewState="false" CssClass="debug" />
                                </div>
                            </td>
                            <td class="detailCol arrive">
                                 <asp:Label ID="arrive" runat="server"   />
                            </td>
                        </tr>

            </AlternatingItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
     </div>
     
</div>