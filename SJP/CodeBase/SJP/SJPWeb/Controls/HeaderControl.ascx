<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.HeaderControl" %>
<%@ Register TagPrefix="uc1" TagName="SkipToLinkControl" Src="SkipToLinkControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LanguageLinkControl" Src="LanguageLinkControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="StyleLinkControl" Src="StyleLinkControl.ascx" %>

<% // NOTE 1: When copy/pasting new London2012 header templates for English and French, please ensure the 
   // various div "id" values are retained as provided in the template. VisualStudio automatically removes duplicate "id" 
   // values which the English and French versions share. Otherwise the css styles may not be applied correctly.  

    // NOTE 2: Some header control loads html from SJPResourceManager (SJPContent) as there is only static html. 
    // However, if in a future release ASP Server controls are required in the header, then the static html should
    // be re-inserted here and removed from SJPContent
%>
   
   <% // Header panel for English version of London2012 header content %>
   <asp:Panel ID="pnlHeaderMastheadContainerEn" runat="server" EnableViewState="false">

            <div id="lcg-masthead" class="header">
			<!--googleoff: all-->
				<div id="lcg-banner">
					<a href="http://www.london2012.com/" title="Official London 2012 website" rel="home">Official London 2012 website</a>
				</div>
				<div id="lcg-motto">
					<div class="lcg-top-motto">Inspire a generation</div>
					<div class="lcg-middle-motto">
						<span class="games">Olympic Games</span>
						<span class="date">27 July - 12 August</span>
					</div>
					<div class="lcg-bottom-motto">Official London 2012 website</div>
				</div>
				<div id="environment-name-label" style="position:absolute;right:5px;top:3em"></div>
				<div id="lcg-topMenuBar">
					<div id="lcg-topMenuBarInner">
						<div id="lcg-topMenu">
							<h2 class="hidden">Accessibility menu </h2>
							<ul>
                                <li class="skipTo" id="liSkipToContentLinkEn" runat="server" enableviewstate="false">
                                    <uc1:SkipToLinkControl id="skipToLinkControlEn" runat="server"></uc1:SkipToLinkControl>
                                </li>
								<li id="liCookiesLinkEn" runat="server" enableviewstate="false"><a href="http://www.london2012.com/cookies-policy">Cookies policy</a></li>
								<li id="lcg-bsl"><a href="http://www.london2012.com/bsl"><abbr title="British Sign Language">BSL</abbr></a></li>
								<li><a href="http://www.london2012.com/easyread">Easy Read</a></li>
								<li id="lcg-accInfo"><a href="http://www.london2012.com/accessibility-statement" title="Accessibility information">Accessibility information</a></li>
								<li id="lcg-accLinks"><span class="hidden">Change Style Sheet</span>
									<ul id="lcg-fontsize">
                                        <li >[<a class="font-size" rel="standard" href="#" title="Switch to Normal text"><span id="acc-stdfonts"><span class="hidden">Normal text</span>A</span></a>]</li>
										<li class="medium">[<a class="font-size" rel="larger" href="#" title="Switch to Larger text"><span id="acc-largefonts"><span class="hidden">Large text</span>A</span></a>]</li>
										<li class="large">[<a class="font-size" rel="largest" href="#" title="Switch to Largest text"><span id="acc-largestfonts"><span class="hidden">Largest text</span>A</span></a>]</li>
									</ul>
									<ul id="lcg-styleswitch">                                    
                                        <li ><a class="skin-switch" rel="normal" href="#" title="Switch to Normal Style Sheet"><span id="acc-stdsize"><span class="hidden">Normal Style Sheet</span>A</span></a></li>
										<li class="dyslexia"><a class="skin-switch" rel="dyslexia" href="#" title="Switch to dyslexia style sheet"><span id="acc-dislexya"><span class="hidden">Dyslexia style sheet</span>A</span></a></li>
										<li class="high-visibility"><a class="skin-switch" rel="high-visibility" href="#" title="Switch to high visibility Style Sheet"><span id="acc-highvisi"><span class="hidden">High visibility Style Sheet</span>A</span></a></li>
									</ul>
								</li>
                                <li id="liLanguageLinkEn" runat="server" enableviewstate="false">
						            <uc1:LanguageLinkControl id="languageLinkControlEn" runat="server"></uc1:LanguageLinkControl>
                                </li>
								<li class="last"><a href="http://www.london2012.com/newsletter">Email updates</a></li>
							</ul>
						</div>
						<div id="lcg-topMenuRight">
						</div>
					</div>
				</div>
				<div class="clear"> </div>

				<div id="lcg-boxesWrap">
					<div class="box sjpCountdownBox">

					</div>
                    <div id="top-ad" class="top-box">
						<span class="hidden">advertisement</span>
						<script type="text/javascript">
							//<![CDATA[
						    try { googletag.cmd.push(function () { googletag.display('top-ad'); }); } catch (err) { }
							//]]>
						</script>
					</div>
				</div>
				<!--googleon: all-->
				<hr />
			</div>
			<!--googleoff: all-->
    </asp:Panel>

    <% // Header panel for English version of Paralympic London2012 header content %>
   <asp:Panel ID="pnlHeaderMastheadContainerEnPara" runat="server" EnableViewState="false">

            <div id="lcg-masthead" class="header">
			<!--googleoff: all-->
				<div id="lcg-banner">
					<a href="http://www.london2012.com/paralympics" title="Official London 2012 website" rel="home">Official London 2012 website</a>
				</div>
				<div id="lcg-motto">
					<div class="lcg-top-motto">Inspire a generation</div>
					<div class="lcg-middle-motto">
						<span class="games">Paralympic Games</span>
						<span class="date">29 Aug - 9 Sept</span>
					</div>
					<div class="lcg-bottom-motto">Official London 2012 website</div>
				</div>
				<div id="environment-name-label" style="position:absolute;right:5px;top:3em"></div>
				<div id="lcg-topMenuBar">
					<div id="lcg-topMenuBarInner">
						<div id="lcg-topMenu">
							<h2 class="hidden">Accessibility menu </h2>
							<ul>
                                <li class="skipTo" id="liSkipToContentLinkEnPara" runat="server" enableviewstate="false">
                                    <uc1:SkipToLinkControl id="skipToLinkControlEnPara" runat="server"></uc1:SkipToLinkControl>
                                </li>
								<li id="liCookiesLinkEnPara" runat="server" enableviewstate="false"><a href="http://www.london2012.com/paralympics/cookies-policy">Cookies policy</a></li>
								<li id="lcg-bsl"><a href="http://www.london2012.com/paralympics/bsl"><abbr title="British Sign Language">BSL</abbr></a></li>
								<li><a href="http://www.london2012.com/paralympics/easyread">Easy Read</a></li>
								<li id="lcg-accInfo"><a href="http://www.london2012.com/accessibility-statement" title="Accessibility information">Accessibility information</a></li>
								<li id="lcg-accLinks"><span class="hidden">Change Style Sheet</span>
									<ul id="lcg-fontsize">
                                        <li>[<a class="font-size" rel="standard" href="#" title="Switch to Normal text"><span id="acc-stdfonts"><span class="hidden">Normal text</span>A</span></a>]</li>
										<li class="medium">[<a class="font-size" rel="larger" href="#" title="Switch to Larger text"><span id="acc-largefonts"><span class="hidden">Large text</span>A</span></a>]</li>
										<li class="large">[<a class="font-size" rel="largest" href="#" title="Switch to Largest text"><span id="acc-largestfonts"><span class="hidden">Largest text</span>A</span></a>]</li>
									</ul>
									<ul id="lcg-styleswitch">                                    
                                        <li><a class="skin-switch" rel="normal" href="#" title="Switch to Normal Style Sheet"><span id="acc-stdsize"><span class="hidden">Normal Style Sheet</span>A</span></a></li>
										<li class="dyslexia"><a class="skin-switch" rel="dyslexia" href="#" title="Switch to dyslexia style sheet"><span id="acc-dislexya"><span class="hidden">Dyslexia style sheet</span>A</span></a></li>
										<li class="high-visibility"><a class="skin-switch" rel="high-visibility" href="#" title="Switch to high visibility Style Sheet"><span id="acc-highvisi"><span class="hidden">High visibility Style Sheet</span>A</span></a></li>
									</ul>
								</li>
                                <li id="liLanguageLinkEnPara" runat="server" enableviewstate="false">
						            <uc1:LanguageLinkControl id="languageLinkControlEnPara" runat="server"></uc1:LanguageLinkControl>
                                </li>
								<li class="last"><a href="http://www.london2012.com/newsletter">Email updates</a></li>
							</ul>
						</div>
						<div id="lcg-topMenuRight">
						</div>
					</div>
				</div>
				<div class="clear"> </div>

				<div id="lcg-boxesWrap">
					<div class="box sjpCountdownBox">

					</div>
                    <div id="top-ad" class="top-box">
						<span class="hidden">advertisement</span>
						<script type="text/javascript">
							//<![CDATA[
						    try { googletag.cmd.push(function () { googletag.display('top-ad'); }); } catch (err) { }
							//]]>
						</script>
					</div>
				</div>
				<!--googleon: all-->
				<hr />
			</div>
			<!--googleoff: all-->
    </asp:Panel>


    <% // Header panel for French version of London2012 header content %>
    <asp:Panel ID="pnlHeaderMastheadContainerFr" runat="server" EnableViewState="false">

            <div id="lcg-masthead" class="header">
                <!--googleoff: all-->
                <div id="lcg-banner">
                    <a href="http://fr.london2012.com/fr/" title="Site officiel de Londres 2012" rel="home">Site officiel de Londres 2012</a>
                </div>
                <div id="lcg-motto">
                    <div class="lcg-top-motto lcg-top-mottoFr">Inspirer une génération</div>
                    <div class="lcg-middle-motto">
                        <span class="games">Jeux Olympiques</span> 
                        <span class="date">27 juil. - 12 août</span>
                    </div>
                    <div class="lcg-bottom-motto">Site officiel de Londres 2012</div>
                </div>
                <div id="lcg-topMenuBar">
                    <div id="lcg-topMenuBarInner">
                        <div id="lcg-topMenu">
                            <h2 class="hidden">Menu accessibilité</h2>
                            <ul>
                                <li class="skipTo"><a href="#mainMenu">Accès direct au menu principal</a> </li>
                                <li class="skipTo" id="liSkipToContentLinkFr" runat="server" enableviewstate="false">
                                    <uc1:SkipToLinkControl id="skipToLinkControlFr" runat="server"></uc1:SkipToLinkControl>
                                </li>
                                <li id="liCookiesLinkFr" runat="server" enableviewstate="false"><a href="http://fr.london2012.com/fr/cookies-policy">Cookies</a> </li>
                                <li id="lcg-accLinks"><span class="hidden">Modifier la feuille de style</span> 
                                    <span class="cookie-tooltip hidden">basic_set-cookie-acc</span>
                                    <ul id="lcg-fontsize">
                                        <li>[<a class="font-size" rel="standard" href="#" title="Passer au texte normal"><span id="acc-stdfonts"><span class="hidden">Texte normal</span>A</span></a>]</li>
                                        <li class="medium">[<a class="font-size" rel="larger" href="#" title="Passer à un texte plus grand"><span id="acc-largefonts"><span class="hidden">Texte plus grand</span>A</span></a>]</li>
                                        <li class="large">[<a class="font-size" rel="largest" href="#" title="Passer au texte le plus grand"><span id="acc-largestfonts"><span class="hidden">Texte le plus grand</span>A</span></a>]</li>
                                    </ul>
                                    <ul id="lcg-styleswitch">
                                        <li><a class="skin-switch" rel="normal" href="#" title="Passer à la feuille de style normal"><span id="acc-stdsize"><span class="hidden">Feuille de style normal</span>A</span></a></li>
                                        <li class="dyslexia"><a class="skin-switch" rel="dyslexia" href="#" title="Passer à la feuille de style pour dyslexiques"><span id="acc-dislexya"><span class="hidden">Feuille de style pour dyslexiques</span>A</span></a></li>
                                        <li class="high-visibility"><a class="skin-switch" rel="high-visibility" href="#" title="Passer à la feuille de style pour déficients visuels"><span id="acc-highvisi"><span class="hidden">Feuille de style pour déficients visuels</span>A</span></a></li>
                                    </ul>
                                </li>
                                <li id="liLanguageLinkFr" runat="server" enableviewstate="false">
						            <uc1:LanguageLinkControl id="languageLinkControlFr" runat="server"></uc1:LanguageLinkControl>
                                </li>
                                <li class="last"><a href="https://secure.london2012.com/preferences/validate.cfm?s=1">Infos par e-mail</a> </li>
                            </ul>
                        </div>
                        <div id="lcg-topMenuRight">
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div id="lcg-boxesWrap">
                    <div class="box sjpCountdownBox">

					</div>
                    <div id="top-ad" class="top-box">
                        <span class="hidden">Publicité</span>
                        <script type="text/javascript">
                            try { googletag.cmd.push(function () { googletag.display('top-ad'); }); } catch (err) { }
                        </script>
                    </div>
                </div>
                <!--googleon: all-->
                <hr />
            </div>
            <!--googleoff: all-->
    </asp:Panel>

    <% // Header panel for French version of London2012 header content %>
    <asp:Panel ID="pnlHeaderMastheadContainerFrPara" runat="server" EnableViewState="false">

            <div id="lcg-masthead" class="header">
                <!--googleoff: all-->
                <div id="lcg-banner">
                    <a href="http://fr.london2012.com/fr/" title="Site officiel de Londres 2012" rel="home">Site officiel de Londres 2012</a>
                </div>
                <div id="lcg-motto">
                    <div class="lcg-top-motto lcg-top-mottoFr">Inspirer une génération</div>
                    <div class="lcg-middle-motto">
                        <span class="games">Jeux Paralympiques</span> 
                        <span class="date">29 août-9 sept</span>
                    </div>
                    <div class="lcg-bottom-motto">Site officiel de Londres 2012</div>
                </div>
                <div id="lcg-topMenuBar">
                    <div id="lcg-topMenuBarInner">
                        <div id="lcg-topMenu">
                            <h2 class="hidden">Menu accessibilité</h2>
                            <ul>
                                <li class="skipTo"><a href="#mainMenu">Accès direct au menu principal</a> </li>
                                <li class="skipTo" id="liSkipToContentLinkFrPara" runat="server" enableviewstate="false">
                                    <uc1:SkipToLinkControl id="skipToLinkControlFrPara" runat="server"></uc1:SkipToLinkControl>
                                </li>
                                <li id="liCookiesLinkFrPara" runat="server" enableviewstate="false"><a href="http://fr.london2012.com/fr/cookies-policy">Cookies</a> </li>
                                <li id="lcg-accLinks"><span class="hidden">Modifier la feuille de style</span> 
                                    <span class="cookie-tooltip hidden">basic_set-cookie-acc</span>
                                    <ul id="lcg-fontsize">
                                        <li>[<a class="font-size" rel="standard" href="#" title="Passer au texte normal"><span id="acc-stdfonts"><span class="hidden">Texte normal</span>A</span></a>]</li>
                                        <li class="medium">[<a class="font-size" rel="larger" href="#" title="Passer à un texte plus grand"><span id="acc-largefonts"><span class="hidden">Texte plus grand</span>A</span></a>]</li>
                                        <li class="large">[<a class="font-size" rel="largest" href="#" title="Passer au texte le plus grand"><span id="acc-largestfonts"><span class="hidden">Texte le plus grand</span>A</span></a>]</li>
                                    </ul>
                                    <ul id="lcg-styleswitch">
                                        <li><a class="skin-switch" rel="normal" href="#" title="Passer à la feuille de style normal"><span id="acc-stdsize"><span class="hidden">Feuille de style normal</span>A</span></a></li>
                                        <li class="dyslexia"><a class="skin-switch" rel="dyslexia" href="#" title="Passer à la feuille de style pour dyslexiques"><span id="acc-dislexya"><span class="hidden">Feuille de style pour dyslexiques</span>A</span></a></li>
                                        <li class="high-visibility"><a class="skin-switch" rel="high-visibility" href="#" title="Passer à la feuille de style pour déficients visuels"><span id="acc-highvisi"><span class="hidden">Feuille de style pour déficients visuels</span>A</span></a></li>
                                    </ul>
                                </li>
                                <li id="liLanguageLinkFrPara" runat="server" enableviewstate="false">
						            <uc1:LanguageLinkControl id="languageLinkControlFrPara" runat="server"></uc1:LanguageLinkControl>
                                </li>
                                <li class="last"><a href="https://secure.london2012.com/preferences/validate.cfm?s=1">Infos par e-mail</a> </li>
                            </ul>
                        </div>
                        <div id="lcg-topMenuRight">
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div id="lcg-boxesWrap">
                    <div class="box  sjpCountdownBox">
                        
                    </div>
                    <div id="top-ad" class="top-box">
                        <span class="hidden">Publicité</span>
                        <script type="text/javascript">
                            try { googletag.cmd.push(function () { googletag.display('top-ad'); }); } catch (err) { }
                        </script>
                    </div>
                </div>
                <!--googleon: all-->
                <hr />
            </div>
            <!--googleoff: all-->
    </asp:Panel>

    <% // Header panel for English version of London2012 header content %>
    <asp:Panel ID="pnlHeaderPrimaryContainerEn" runat="server" EnableViewState="false"></asp:Panel>

    <% // Header panel for French version of London2012 header content %>
    <asp:Panel ID="pnlHeaderPrimaryContainerFr" runat="server" EnableViewState="false"></asp:Panel>


    <% // Header panel for English version of London2012 header content %>
    <asp:Panel ID="pnlHeaderSecondaryContainerEn" runat="server" EnableViewState="false"></asp:Panel>

    <% // Header panel for French version of London2012 header content %>
    <asp:Panel ID="pnlHeaderSecondaryContainerFr" runat="server" EnableViewState="false"></asp:Panel>
