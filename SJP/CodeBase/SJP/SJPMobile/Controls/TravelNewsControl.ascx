<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelNewsControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.TravelNewsControl" %>

<div id="travelNewsUnavailableDiv" runat="server" class="travelNewsUnavailable">
    <asp:Label ID="travelNewsUnavailableLbl" runat="server" CssClass="error" />
</div>

<div id="travelNewsDiv" runat="server" class="travelNews clear">
<div id="results" class="news jsshow">

    <div class="travelNewsLastUpdated">
        <asp:Label ID="travelNewsLastUpdatedLbl" runat="server"/>
    </div>
    <asp:Repeater ID="travelNewsRptr" runat="server" OnItemDataBound="travelNewsRptr_DataBound">
        <HeaderTemplate>
            <ul data-role="listview" id="alltravelnews">
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <noscript>
                    <asp:Button ID="showDetailsBtnNonJS" runat="server" CssClass="showDetailsBtnNonJS" OnClick="showDetailsBtnNonJS_Click" />
                </noscript>
                <asp:LinkButton ID="showDetailsBtn" runat="server" data-role="dialog" data-transition="none">
                    <div class="ui-grid-a">
                        <asp:HiddenField ID="newsId" runat="server" />
                        <div class="newsTransportMode floatleft">
                            <asp:Image ID="newsTransportModeImg" runat="server" />
                        </div>
                        <div class="ui-block-b">
                            <strong><asp:Label ID="newsHeadlineLbl" runat="server" /></strong>
                            <br /><asp:Label ID="newsUpdatedLbl" runat="server" />
                        </div>
                    </div>
                </asp:LinkButton>
                <br />
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
</div>
<asp:PlaceHolder ID="newsDetails" runat="server"></asp:PlaceHolder>
