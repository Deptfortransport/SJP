<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WaitControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.WaitControl" %>

<div id="journeyProgress" runat="server" class="journeyProgress" >
    <div class="processMessage" aria-live="polite"> 
        <div class="loadingMessageDiv">
            <asp:Label ID="loadingMessage" runat="server" />
            <noscript>
                <br />
                <asp:Label ID="longWaitMessage" runat="server" />
                <asp:HyperLink ID="longWaitMessageLink" runat="server" />
            </noscript>   
        </div>
        <div class="loadingImageDiv">
            <asp:Image ID="loadingImage" runat="server" />
        </div>
    </div>
</div>