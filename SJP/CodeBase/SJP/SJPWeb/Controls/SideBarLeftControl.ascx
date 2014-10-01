<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SideBarLeftControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.SideBarLeftControl" %>

<% // NOTE: When copy/pasting new London2012 navigation templates for English and French, please ensure the 
   // various div "id" values are retained as provided in the template. VisualStudio automatically removes duplicate "id" 
   // values which the English and French versions share. Otherwise the css styles may not be applied correctly.  %>

<% // Navigation panel for English version of London2012 side navigation  %>
<asp:Panel ID="pnlNavEn" runat="server" EnableViewState="false">

    <div class="row"><!--googleoff: all-->
		<div class="menuLeft">
			<ul class="nav vert-menu  t2 ">
				<li class=" role-menuitem first  last ">
					<a href="http://www.london2012.com/paralympics/spectators/travel"><span>Travel</span></a>
					<ul class="sub-menu-left">
						<li class=" role-menuitem first current"><a id="sjpEnUrl" runat="server" href="~"><span>Plan your travel </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/book-your-travel"><span>Book your travel </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/accessible-travel"><span>Accessible travel </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/group-travel"><span>Group travel </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/games-travelcard"><span>Games Travelcard </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/walking"><span>Walking </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/travelling-from-outside-uk"><span>Travelling from outside the UK </span></a></li>
						<li class=" role-menuitem"><a href="http://www.london2012.com/paralympics/spectators/travel/cycling"><span>Cycling </span></a></li>
						<li class=" role-menuitem last "><a href="http://www.london2012.com/paralympics/spectators/travel/travelling-in-london"><span>Travelling in London </span></a></li> 
					</ul>
				</li>
			</ul>
		</div><!--googleon: all-->
	</div>

</asp:Panel>

<% // Navigation panel for French version of London2012 side navigation  %>
<asp:Panel ID="pnlNavFr" runat="server" EnableViewState="false">

    <div class="row"><!--googleoff: all-->
		<div class="menuLeft">
			<ul class="nav vert-menu  t2 ">
                <li class="role-menuitem first last">
                    <a href="http://fr.london2012.com/fr/spectators/travel"><span>Voyage</span> </a>
                    <ul class="sub-menu-left">
                        <li class=" role-menuitem first current"><a id="sjpFrUrl" runat="server" href="~"><span>Planifier votre trajet</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/book-your-travel"><span>R&#233;server votre voyage</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/accessible-travel"><span>Transport accessible</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/group-travel"><span>Voyager en groupe</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/games-travelcard"><span>Carte de transport des Jeux</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/walking"><span>&#224; pied</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/travelling-from-outside-uk"><span>Se rendre au Royaume-Uni de l'&#233;tranger</span> </a></li>
                        <li class=" role-menuitem"><a href="http://fr.london2012.com/fr/spectators/travel/cycling"><span>Cyclisme</span> </a></li>
                        <li class=" role-menuitem last"><a href="http://fr.london2012.com/fr/spectators/travel/travelling-in-london"><span>Se d&#233;placer dans Londres</span> </a></li>
                    </ul>
                </li>
            </ul>
		</div><!--googleon: all-->
	</div>

</asp:Panel>
