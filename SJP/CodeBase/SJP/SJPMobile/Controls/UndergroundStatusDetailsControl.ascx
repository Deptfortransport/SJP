<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UndergroundStatusDetailsControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.UndergroundStatusDetailsControl" %>

<div data-role="page" class="undergroundDetail collapsed">
    <asp:HiddenField ID="undergroundDetailId" runat="server" />
    
    <div class="headerBack headerBackBlue jshide">
        <h2 ID="undergroundNewsTitle" runat="server"></h2>
        <asp:LinkButton ID="closeNewsItem" CssClass="topNavLeft" runat="server" data-icon="delete"></asp:LinkButton>
    </div>
    
    <h3 id="undergroundServiceHeadlineLbl" runat="server" class="statusColourHeadline"></h3>

    <div class="undergroundDetails">
        <div class="description">
            <asp:Label ID="statusDescriptionLbl" runat="server" />
        </div>
        <div class="detail">
            <asp:Label ID="statusDetailLbl" runat="server" />
        </div>
    </div>

</div>
