<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VenueMapsWidget.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.VenueMapsWidget" %>

<div class="row">
    <div class="box venueMapsWidget">
        <div class="bH">
            <h3>
                <asp:HyperLink ID="widgetHeadingLink" runat="server"></asp:HyperLink>
            </h3>
        </div>
        <div class="bC">
            <asp:HyperLink ID="venueImageLink" runat="server">
                <asp:Image ID="venueImage" CssClass="venueMap" runat="server" />
            </asp:HyperLink>
            <br />
            <div class="clearboth"></div>
        </div>
        <div class="bF"> 
            <div class="more">
                <asp:HyperLink ID="pdfLink" CssClass="submit" runat="server">
                    <asp:Label ID="pdfDownloadButton" runat="server" />
                </asp:HyperLink>
            </div>
        </div>
    </div>
</div>
