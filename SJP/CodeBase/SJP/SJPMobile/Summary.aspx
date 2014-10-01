<%@ Page Title="" Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="SJP.UserPortal.SJPMobile.Summary" %>
<%@ Register src="~/Controls/JourneyInputControl.ascx" tagname="JourneyInputControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/DetailsSummaryControl.ascx" tagname="JourneySummaryControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/WaitControl.ascx" tagname="WaitControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">

    <asp:UpdatePanel ID="journeyInputUpdate" runat="server" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>

            <uc1:JourneyInputControl ID="journeyInputControl" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="journeySummaryUpdate" runat="server">
        <ContentTemplate>

            <asp:HiddenField ID="waitCount" runat="server" />
             
            <div id="waitControlDiv" runat="server" class="waitContainer">
                <uc1:WaitControl ID="waitControl" runat="server" />
            </div>

            <asp:Timer ID="waitTimer" runat="server" OnTick="WaitTimer_Tick"  >
            </asp:Timer>
            
            <uc1:JourneySummaryControl ID="outwardSummaryControl" runat="server" Visible="false" />
            
            <div class="submittab submittabreturn">
                <div class="jshide">
                    <asp:LinkButton ID="planReturnJourneyBtn" runat="server" OnClick="planReturnJourneyBtn_Click" CssClass="buttonMag" Visible="false"></asp:LinkButton>
                </div>
                <noscript>
                    <asp:Button ID="planReturnJourneyBtnNonJS" runat="server" OnClick="planReturnJourneyBtn_Click" CssClass="buttonMag buttonMagNonJS" Visible="false"></asp:Button>
                </noscript>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
