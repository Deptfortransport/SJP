<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.HeaderControl" %>

<% // NOTE: When copy/pasting new London2012 header templates for English and French, please ensure the 
   // various div "id" values are retained as provided in the template. VisualStudio automatically removes duplicate "id" 
   // values which the English and French versions share. Otherwise the css styles may not be applied correctly.  %>

<% // Header panel for English version of London2012 header content %>
<asp:Panel ID="pnlHeaderEn" runat="server" EnableViewState="false">

    <a name="top"></a>
	<div class="header">
		<div class="logoWrap floatleft">
			<div class="logo">
				<a href="http://m.london2012.com/" title="Official London 2012 web site" rel="external" accesskey="1"></a>
			</div>
		</div>
		<div class="menuWrap">
			<a id="menu" title="Main Menu" href="#nav" accesskey="2">Menu</a>
		</div>
	</div>
    <div id="navHolder"></div>
	<div class="clear"></div>

</asp:Panel>

<% // Header panel for French version of London2012 header content %>
<asp:Panel ID="pnlHeaderFr" runat="server" EnableViewState="false">

    <a name="top"></a>
	<div class="header">
		<div class="logoWrap floatleft">
			<div class="logo">
				<a href="http://m.london2012.com/" title="Official London 2012 web site" rel="external" accesskey="1">
                    <asp:Image ID="logoFr" runat="server" />
				</a>
			</div>
		</div>
		<div class="menuWrap">
			<a id="menu" title="Main Menu" href="#nav" accesskey="2">Menu</a>
		</div>
	</div>
    <div id="navHolder"></div>
	<div class="clear"></div>

</asp:Panel>