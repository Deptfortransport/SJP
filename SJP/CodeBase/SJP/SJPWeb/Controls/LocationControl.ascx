<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.LocationControl" %>
<%@ Register Namespace="SJP.UserPortal.SJPWeb.Controls" TagPrefix="cc1" assembly="sjp.userportal.sjpweb" %> 
<%@ Register Namespace="SJP.Common.Web" TagPrefix="cc2" assembly="sjp.common.web" %> 

<div class="locationControl">
    <div class="jssettings">
        <asp:HiddenField ID="scriptId" runat="server" />
        <asp:HiddenField ID="scriptPath" runat="server" />
        <asp:HiddenField ID="jsEnabled" Value="false" runat="server" />
    </div>
    <div id="ambiguityRow" runat="server" class="ambiguityrow" visible="false">
        <asp:Label ID="ambiguityText" CssClass="ambiguityinfo" runat="server" />
    </div>
    <asp:Label ID="locationInput_Discription"  runat="server" style="display:none;" />
    <asp:TextBox ID="locationInput" CssClass="locationBox watermark" runat="server" ToolTip="postcode, location, station"></asp:TextBox>
    <asp:HiddenField ID="locationInput_hdnValue"  runat="server" Value="" />
    <asp:HiddenField ID="locationInput_hdnType"  runat="server" Value="" />
    <cc2:GroupDropDownList ID="locationDropdown" CssClass="locationDrop" runat="server" Visible="false">
    </cc2:GroupDropDownList>
    <asp:Button ID="reset" CssClass="locationreset linkButton" runat="server" OnClick="ResetLocation" Visible="false" />
</div>
