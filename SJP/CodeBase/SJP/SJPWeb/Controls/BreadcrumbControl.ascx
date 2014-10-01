<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BreadcrumbControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.BreadcrumbControl" %>
<div id="breadcrumb-2012">
    <div id="breadcrumb-2012-container"><asp:Label ID="lblBreadcrumbTitle" runat="server" EnableViewState="false"></asp:Label>
        <ul id="breadcrumb-2012-list">

            <asp:PlaceHolder ID="breadcrumbLinks" runat="server" EnableViewState="false"></asp:PlaceHolder>

        </ul>
    </div>
</div>