<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true" CodeBehind="PrintableJourneyOptions.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.PrintableJourneyOptions" %>
<%@ Register src="~/Controls/DetailsSummaryControl.ascx" tagname="JourneySummaryControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">

    <uc1:JourneySummaryControl ID="outwardSummaryControl" runat="server" Visible="false" />
            
    <uc1:JourneySummaryControl ID="returnSummaryControl" runat="server" Visible="false" />

</div>

</asp:Content>
