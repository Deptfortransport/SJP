<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JourneyPlannerWidget.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.JourneyPlannerWidget" %>

<div class="row">
    <div class="box journeyPlannerWidget">
        <div class="bH">
            <h3>
                <asp:Label ID="widgetHeading" runat="server"></asp:Label>
            </h3>
        </div>
        <div class="bC">
            <div class="jpPromoImageDiv">
                <asp:Image ID="jpPromoImage" class="promoImage" runat="server" />
            </div>
            <div class="jpPromoContentDiv">
                <asp:Label ID="jpPromoContent" class="promoContent" runat="server" />
            </div>
            <br />
        </div>
        <div class="bF"> 
            <div class="more">
                <asp:HyperLink ID="jpLink" CssClass="submit" runat="server">
                    <asp:Label ID="jpLinkText" runat="server" />
                </asp:HyperLink>
            </div>
        </div>
     </div> 
</div>
