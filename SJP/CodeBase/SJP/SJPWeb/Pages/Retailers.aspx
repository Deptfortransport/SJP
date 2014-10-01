<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="Retailers.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.Retailers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">    
    
    <asp:Label ID="lblBookingSummary" runat="server" EnableViewState="false" />
        
    <asp:Panel ID="pnlRetailers" runat="server" EnableViewState="false" Visible="false">
        <asp:Repeater ID="rptRetailers" runat="server" EnableViewState="false">
            <ItemTemplate>
                <div class="sjpRetailer">
                    <div class="sjpRetailerIcon floatleft">
                        <asp:Image ID="imgRetailerIcon" runat="server" EnableViewState="false"/>
                    </div>
                    <div class="sjpRetailerButton floatright">
                        <asp:Button ID="btnRetailerHandoff" runat="server" CssClass="submit btnMediumPink displaynone" EnableViewState="false" CausesValidation="false" />
                        <noscript>
                            <asp:hyperlink id="lnkRetailerHandoff" runat="server" target="_blank" EnableViewState="false"></asp:hyperlink>
                        </noscript>
                    </div>
                </div>
                <div class="clearboth"></div>
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <asp:Label ID="lblDisclaimer1" runat="server" EnableViewState="false" />
        <asp:Label ID="lblDisclaimer2" runat="server" EnableViewState="false" />

        <div class="sjpSeparator" ></div>

    </asp:Panel>
    
    <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="submit btnSmallPink floatleft" EnableViewState="false" />
</div>

</asp:Content>
