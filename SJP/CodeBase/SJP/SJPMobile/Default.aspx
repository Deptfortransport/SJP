<%@ Page Language="C#" MasterPageFile="~/SJPMobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SJP.UserPortal.SJPMobile._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" Runat="Server">
    
    <div class="jshide">
        <asp:LinkButton ID="publicTransportModeBtn" runat="server" OnClick="publicTransportModeBtn_Click" CssClass="buttonMag nomargingBot" EnableViewState="false" data-role="button" data-theme="a" ></asp:LinkButton>
    </div>
    <div class="jshide">
        <asp:LinkButton ID="cycleModeBtn" runat="server" OnClick="cycleModeBtn_Click" CssClass="buttonMag" EnableViewState="false" data-role="button" data-theme="a" ></asp:LinkButton>
    </div>
    <div class="jshide">
        <asp:LinkButton ID="travelNewsBtn" runat="server" OnClick="travelNewsBtn_Click" CssClass="buttonBlue" EnableViewState="false" data-role="button" data-theme="a" ></asp:LinkButton>
    </div>

    <noscript>
        <div>
            <asp:Button id="publicTransportModeBtnNonJS" runat="server" OnClick="publicTransportModeBtn_Click" CssClass="buttonMag buttonMagNonJS" EnableViewState="false" />
        </div>
        <div>
            <asp:Button id="cycleModeBtnNonJS" runat="server" OnClick="cycleModeBtn_Click" CssClass="buttonMag buttonMagNonJS" EnableViewState="false" />
        </div>
        <div>
            <asp:Button id="travelNewsBtnNonJS" runat="server" OnClick="travelNewsBtn_Click" CssClass="buttonBlue buttonBlueNonJS" EnableViewState="false" />
        </div>
    </noscript>
</asp:Content>