<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="Input.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.Input" %>
<%@ Register src="~/Controls/JourneyInputControl.ascx" tagname="JourneyInputControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/WaitControl.ascx" tagname="WaitControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    
    <asp:UpdatePanel ID="journeyInputUpdate" runat="server" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>

            <uc1:JourneyInputControl ID="journeyInputControl" runat="server" />

            <div class="waitContainer hide">
                <uc1:WaitControl ID="waitControl" runat="server" />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
