<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenericPromoWidget.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.GenericPromoWidget" %>

<div class="row">
    <div class="box genericPromo">
        <div class="bH">
            <h3 class="bHover">
                <asp:HyperLink ID="promoHeadingLink" runat="server" />
            </h3>
        </div>
        <div class="bC">
            <asp:HyperLink ID="promoImageLink" runat="server">
                <asp:Image ID="promoImage" runat="server" />
            </asp:HyperLink>
            <asp:Literal ID="promoContent" runat="server" />
            <br />
        </div>
        <div class="bF"> 
            <div class="more">
			    <asp:HyperLink ID="promoButtonLink" runat="server">
                    <asp:Label ID="promoButton" runat="server" />
                </asp:HyperLink>
			</div>
		</div>
    </div>
</div>
