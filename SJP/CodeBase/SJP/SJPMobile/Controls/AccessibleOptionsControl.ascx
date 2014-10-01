<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccessibleOptionsControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.AccessibleOptionsControl" %>

<div id="accessibilityOptions" runat="server" class="accessibilityOptions">
    <noscript>
        <asp:Button id="accessibilityOptionsBtn" runat="server" EnableViewState="false" OnClick="AdditionalMobilityNeeds_Click" CssClass="accessibilityOptionsBtnNonJS" />
    </noscript>
    <div id="mobilityOptions" runat="server" data-role="collapsible" data-collapsed="true" data-inline="true" class="collapseMagenta collapse jshide">

        <h2 id="mobiltyOptionsHeading" runat="server" class="ui-collapsible-heading" enableviewstate="false" />
        
        <fieldset data-role="controlgroup">
            
            <div class="ui-collapsible-content ui-controlgroup-controls jsshow">
                <asp:RadioButton ID="stepFree" runat="server" GroupName="mobilityOptions" />
                <div class="clearboth"></div>
                <asp:RadioButton ID="stepFreeAndAssistance" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption"/>
                <div class="clearboth"></div>
                <asp:RadioButton ID="assistance" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption"/>
                <div class="clearboth"></div>
                <asp:RadioButton ID="excludeUnderground" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption" />
                <div class="clearboth"></div>
                <asp:RadioButton ID="noMobilityNeeds" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption" />
                <div class="clearboth"></div>
            </div>

        </fieldset>
    </div>  
    <asp:HiddenField ID="assistanceOptionSelected" runat="server" />
</div>
