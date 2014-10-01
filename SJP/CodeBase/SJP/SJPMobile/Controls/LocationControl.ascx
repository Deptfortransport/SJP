<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.LocationControl" %>
<%@ Register Namespace="SJP.UserPortal.SJPMobile.Controls" TagPrefix="cc1" assembly="sjp.userportal.sjpmobile" %> 
<%@ Register Namespace="SJP.Common.Web" TagPrefix="cc2" assembly="sjp.common.web" %>
<%@ Register src="~/Controls/VenueSelectControl.ascx" tagname="VenueSelectControl" tagprefix="uc1" %>

<div class="locationControl venues">
    <div class="jssettings">
        <asp:HiddenField ID="scriptId" runat="server" />
        <asp:HiddenField ID="scriptPath" runat="server" />
        <asp:HiddenField ID="jsEnabled" Value="false" runat="server" />
    </div>
    
    <asp:HiddenField ID="locationInput_hdnValue"  runat="server" Value="" />
    <asp:HiddenField ID="locationInput_hdnType"  runat="server" Value="" />

    <div id="locationDirectionDiv" runat="server" class="screenReaderOnly" >
        <asp:Label ID="locationDirectionLbl" CssClass="locationLabel" runat="server" EnableViewState="false" />
    </div>

    <div id="ambiguityRow" runat="server" class="ambiguityrow" visible="false">
        <asp:DropDownList ID="ambiguityDropdown" runat="server" CssClass="ambiguityDrop" data-role="none"></asp:DropDownList>
    </div>
    
    <div id="locationDescriptionDiv" runat="server" class="screenReaderOnly">
        <asp:Label ID="locationInput_Discription"  runat="server" EnableViewState="false" />
    </div>
    <div id="currentLocationDiv" runat="server" class="mylocation">
        <asp:Button ID="currentLocationButton" runat="server" CssClass="hide locationCurrent" />
    </div>
       
    <asp:TextBox ID="locationInput" CssClass="locationInput watermark jshide" runat="server" autocorrect="off" autocapitalize="off" aria-live="polite"></asp:TextBox>
    <noscript>
        <asp:TextBox ID="locationInputNonJS" CssClass="locationInputNonJS watermark" runat="server" autocorrect="off" autocapitalize="off" Visible="false"></asp:TextBox>        
        <cc2:GroupDropDownList ID="venueDropdownNonJS" CssClass="venueDropdownNonJS" runat="server" Visible="false"></cc2:GroupDropDownList>
    </noscript>
    
    <asp:HyperLink ID="venueInputPageLnk" runat="server" CssClass="venuesLink jshide" data-transition='none' data-role='button' data-icon='none'></asp:HyperLink>
    <asp:LinkButton ID="resetButton" runat="server" CssClass="resetLink jshide" OnClick="ResetLocation" data-role='button' data-icon='none'/>
    <noscript>
        <asp:Button ID="resetButtonNonJS" runat="server" CssClass="resetLocationNonJS" OnClick="ResetLocation" />
    </noscript>
</div>

<uc1:VenueSelectControl ID="venueSelectControl" runat="server"></uc1:VenueSelectControl>

