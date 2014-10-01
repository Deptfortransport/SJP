<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="RetailerHandoff.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.RetailerHandoff" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">

    <div class="sjpSeparator" ></div>

    <div id="debugDiv" runat="server" visible="false">
        <div>
            <asp:TextBox id="txtbxHandoffXML" runat="server" TextMode="MultiLine" Width="450px" Height="400px" CausesValidation="false"></asp:TextBox>
        </div>
        <br />
        <asp:RadioButtonList ID="retailersRadioList" runat="server"></asp:RadioButtonList><br />
        <asp:Label ID="lblHandoffURL" runat="server"></asp:Label>
        <asp:TextBox ID="txtbxHandoffURL" runat="server" Width="350px"></asp:TextBox><br />
        <div class="floatright">
            <asp:Button ID="btnRetailerHandoff" runat="server" CssClass="submit btnMediumPink" EnableViewState="false" CausesValidation="false" OnClick="btnRetailerHandoff_Click" />
        </div>
    </div>

</div>

</asp:Content>
