<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JourneyPageControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.JourneyPageControl" %>

<div class="ui-grid-b journeyNav" id="navresults">
    <div class="ui-block-a">
        <asp:HyperLink ID="lnkPrevious" runat="server" CssClass="earlier jshide" EnableViewState="true" data-icon="arrow-l"  data-iconpos="notext" data-transition='none' data-role='button' rel="external" />
        <noscript>
            <asp:Button ID="btnPrevious" runat="server" OnClick="btnPreviousNext_Click" CssClass="earlierNonJS" EnableViewState="true" />
        </noscript>
    </div>       
    <div class="ui-block-b">
        <asp:Label ID="lblHeading" runat="server" EnableViewState="false"></asp:Label>
    </div>  
    <div class="ui-block-c jsshow">
        <asp:HyperLink ID="lnkNext" runat="server" CssClass="later jshide" EnableViewState="true" data-icon="arrow-r"  data-iconpos="notext" data-transition='none' data-role='button' rel="external" />
        <noscript>
            <asp:Button ID="btnNext" runat="server" OnClick="btnPreviousNext_Click" CssClass="laterNonJS" EnableViewState="true" />
        </noscript>
    </div>  
</div>

