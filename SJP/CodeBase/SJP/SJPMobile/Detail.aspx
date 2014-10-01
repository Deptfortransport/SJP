<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.Detail" %>
<%@ Register Namespace="SJP.UserPortal.SJPMobile.Controls" TagPrefix="uc1" assembly="sjp.userportal.sjpmobile" %> 
<%@ Register src="~/Controls/JourneyPageControl.ascx" tagname="JourneyPageControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/DetailsLegsControl.ascx" tagname="LegsControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
          
    <asp:UpdatePanel ID="journeyInputUpdate" runat="server" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>
    
            <uc1:JourneyPageControl ID="journeyPageControl" runat="server" />
            <uc1:LegsControl ID="legsDetails" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
