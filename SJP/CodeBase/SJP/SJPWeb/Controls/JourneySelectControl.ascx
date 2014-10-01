<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JourneySelectControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.JourneySelectControl" %>
<div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv">
    <div class="selectJourneyDrop floatleft">
        <asp:Label ID="lblJourneys" runat="server" CssClass="debug" EnableViewState="false"></asp:Label>
        <asp:DropDownList ID="journeysList" runat="server" CssClass="dropList" ></asp:DropDownList>
    </div>
    <div class="floatright">
        <asp:Button ID="btnShowJourney" runat="server" CssClass="btnSmallPink" EnableViewState="false" OnClick="btnShowJourney_Click" />
    </div>
    <div class="clearboth"></div>
</div>