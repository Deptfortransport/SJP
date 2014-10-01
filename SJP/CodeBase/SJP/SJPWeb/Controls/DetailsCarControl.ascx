<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsCarControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.DetailsCarControl" %>

<div class="carLeg">
    <div class="header">
        <asp:Label ID="journeyHeader" runat="server" />
    </div>
    <div class="summary">
        <div>
            <strong><asp:Label ID="lblDistanceTotalHead" runat="server"></asp:Label></strong><asp:Label ID="lblDistanceTotal" runat="server"></asp:Label>
            <strong><asp:Label ID="lblDurationTotalHead" runat="server"></asp:Label></strong><asp:Label ID="lblDurationTotal" runat="server"></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblRoadsHead" runat="server"></asp:Label><asp:Label ID="lblRoads" runat="server"></asp:Label>
        </div>
    </div>
     <div class="detail">
        <asp:Repeater ID="legDetail" runat="server" OnItemDataBound="legDetail_DataBound" >
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr class="headerRow">
                            <th class="distance">
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
                            <td class="detailCol instruction">
                                <asp:Label ID="instruction" runat="server"  />
                                <asp:Image ID="highTrafficSymbol" runat="server" Visible ="false" />
                            </td>
                            <td class="detailCol arrive">
                                 <asp:Label ID="arrive" runat="server" />
                            </td>
                        </tr>
                        
            </ItemTemplate>
            <AlternatingItemTemplate>
                         <tr class="detailRow">
                            <td class="detailCol distance">
                                <asp:Label ID="distance"  runat="server"  />
                            </td>
                            <td class="detailCol instruction">
                                <asp:Label ID="instruction" runat="server" />
                                <asp:Image ID="highTrafficSymbol" runat="server" Visible ="false" />
                            </td>
                            <td class="detailCol arrive">
                                 <asp:Label ID="arrive" runat="server"  />
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