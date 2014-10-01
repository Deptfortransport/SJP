<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="Direction.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.Direction" %>
<%@ Register Namespace="SJP.UserPortal.SJPMobile.Controls" TagPrefix="uc1" assembly="sjp.userportal.sjpmobile" %> 
<%@ Register src="~/Controls/DetailsCycleControl.ascx" tagname="CycleLegControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    
    <div class="legCycle" id="legCycle" runat="server" visible="false">
         <uc1:CycleLegControl ID="cycleLeg" runat="server" />
    </div>

</asp:Content>