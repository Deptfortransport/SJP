<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.FooterControl" %>

<% // NOTE 1: When copy/pasting new London2012 footer templates for English and French, please ensure the 
   // various div "id" values are retained as provided in the template. VisualStudio automatically removes duplicate "id" 
   // values which the English and French versions share. Otherwise the css styles may not be applied correctly.  
   
   // NOTE 2: Current footer control loads html from SJPResourceManager (SJPContent) as there is only static html. 
   // However, if in a future release ASP Server controls are required in the footer, then the static html should
   // be re-inserted here and removed from SJPContent
%>

<% // Footer panel for English version of London2012 footer  %>
<asp:Panel ID="pnlFooterEn" runat="server" EnableViewState="false"></asp:Panel>

<% // Footer panel for French version of London2012 footer  %>
<asp:Panel ID="pnlFooterFr" runat="server" EnableViewState="false"></asp:Panel>

<div class="clear"></div>
