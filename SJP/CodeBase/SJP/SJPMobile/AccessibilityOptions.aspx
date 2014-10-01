<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="AccessibilityOptions.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.AccessibilityOptions" %>
<%@ Register src="~/Controls/AccessibleStopsControl.ascx" tagname="AccessibleStopsControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    
    <asp:UpdatePanel ID="accessibleStops" runat="server" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>

            <uc1:AccessibleStopsControl ID="accessibleStopsControl" runat="server" />
             
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
