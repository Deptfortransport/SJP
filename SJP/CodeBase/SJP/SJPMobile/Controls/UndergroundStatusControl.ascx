<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UndergroundStatusControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.UndergroundStatusControl" %>

<div id="undergroundStatusUnavailableDiv" runat="server" class="undergroundStatusUnavailable">
    <asp:Label ID="undergroundStatusUnavailableLbl" runat="server" CssClass="error" />
</div>

<div id="undergroundStatusDiv" runat="server" class="undergroundStatus clear">
<div id="results" class="undergroundNews jsshow">

    <div class="undergroundStatusLastUpdated">
        <asp:Label ID="undergroundStatusLastUpdatedLbl" runat="server" />
    </div>
    <asp:Repeater ID="undergroundStatusRptr" runat="server" OnItemDataBound="undergroundStatusRptr_DataBound">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <noscript>
                    <asp:Button ID="showDetailsBtnNonJS" runat="server" CssClass="showDetailsBtnNonJS" OnClick="showDetailsBtnNonJS_Click" />
                </noscript>
                <asp:LinkButton ID="showDetailsBtn" runat="server" rel="external">
                    <div class="statusRow">
                        <asp:HiddenField ID="statusLineId" runat="server" />                                              
                        <div id="statusColorDiv" runat="server" class="statusColour">
                            <asp:Label ID="statusLineLbl" runat="server" />
                        </div>
                        <div class="statusInfo">
                            <asp:Label ID="statusDescriptionLbl" runat="server" />
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
<asp:PlaceHolder ID="undergroundDetails" runat="server"></asp:PlaceHolder>
