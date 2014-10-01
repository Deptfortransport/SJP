<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelNewsInfoWidget.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.TravelNewsInfoWidget" %>

<div class="row">
    <div class="box travelNewsInfoWidget">
        <div class="bH">
            <h3>
                <asp:Label ID="widgetHeading" runat="server"></asp:Label>
            </h3>
        </div>
        <div class="bC">
            <p>
                <asp:Label ID="tnInfoContent" class="promoContent" runat="server" />
            </p>
            <br />
        </div>
        <div class="bF"> 
        </div>
     </div> 
</div>
