<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GNATMapsWidget.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.GNATMapsWidget" %>

<div class="row">
    <div class="box gnatMapsWidget">
        <div class="bH">
            <h3>
                <asp:HyperLink ID="widgetHeadingLink" runat="server"  Target="_blank"></asp:HyperLink>
            </h3>
        </div>
        <div class="bC">
            <asp:HyperLink ID="gnatImageLink" runat="server"  Target="_blank">
                <asp:Image ID="gnatImage" CssClass="venueMap" runat="server" />
            </asp:HyperLink>
            <br /><br />
        </div>
        <div class="bF"> 
            <div class="more">
                <asp:HyperLink ID="pdfLink" CssClass="submit" runat="server" Target="_blank">
                    <asp:Label ID="pdfDownloadButton" runat="server" />
                </asp:HyperLink>
            </div>
        </div>
    </div>
</div>