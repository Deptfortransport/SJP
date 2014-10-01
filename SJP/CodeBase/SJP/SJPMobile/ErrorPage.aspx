<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.ErrorPage" %>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMain" runat="server">

    <h2 id="titleMessage" runat="server" enableviewstate="false"></h2>

    <div class="message">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    </div>

</asp:Content>
