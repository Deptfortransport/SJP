<%@ Page Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">
    
    <asp:Label ID="lblErrorMessage1" runat="server" EnableViewState="false"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblErrorMessage2" runat="server" EnableViewState="false"></asp:Label>

</div>
    
</asp:Content>
