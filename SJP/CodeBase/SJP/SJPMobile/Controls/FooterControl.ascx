<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.FooterControl" %>

<% // NOTE: When copy/pasting new London2012 footer templates for English and French, please ensure the 
   // various div "id" values are retained as provided in the template. VisualStudio automatically removes duplicate "id" 
   // values which the English and French versions share. Otherwise the css styles may not be applied correctly.  %>

<% // Footer panel for English version of London2012 footer  %>
<asp:Panel ID="pnlFooterEn" runat="server" EnableViewState="false">

    <div class="clear"></div>
	<div class="footer">
        <div id="backDiv" runat="server" enableviewstate="false" class="backFooterDiv">
		  <div class="navWrap">
            <div class="colNav colNavFirst">
                <ul>
                    <li class="navItem">
                        <asp:LinkButton ID="backBtn" runat="server" CssClass="jshide" OnClick="backBtn_Click" EnableViewState="false" data-role="none"  rel="external"></asp:LinkButton>
                        <noscript>
                            <asp:Button ID="backBtnNonJS" runat="server" CssClass="backBtnNonJS" OnClick="backBtn_Click" EnableViewState="false" />
                        </noscript>
                    </li>
                </ul>
            </div>
          </div>
        </div>
		<div id="bottomNav" class="bottomNav">
		  <h2 class="hide">Main menu</h2>
		  <a name="nav"></a>
		  <div id="nav" class="navWrap">
			<div class="colNav colNavFirst">
			  <ul>
				<li class="navItem">
				  <a id="mainMenuEnUrl" runat="server" href="~" rel="external" accesskey="3">Travel planner home</a>
				</li>
                <li id="liLanguageLinkEn" runat="server" class="navItem language">
				    Language
                    <asp:LinkButton id="lnkbtnLanguageFr" runat="server" CssClass="languageLink" AccessKey="4"></asp:LinkButton>
				</li>
			  </ul>
			</div>
			<div class="colNav colNavSecond">
			  <ul>
				<li class="navItem">
				    <a id="linktop" href="#top" rel="external" accesskey="5">Back to top</a>
				</li>
			  </ul>
			</div>
			<div class="clear"></div>
            <div class="colNav colNav2 colNavFirst colNavFirst2">
			  <ul>
                <li class="navItem ldn-oly">
                    <a href="http://m.london2012.com/">Olympic Games</a>
                </li>
              </ul>
			</div>
			<div class="colNav colNav2 colNavSecond colNavSecond2">
			  <ul>
                <li class="navItem ldn-para">
                    <a href="http://m.london2012.com/paralympics">Paralympic Games</a>
                </li>
              </ul>
            </div>
			<div class="clear"></div>
			<div class="fullsite">
			  <a href="http://travel.london2012.com/SJPWeb/?dnr=true" rel="external" accesskey="6">Full site</a>
		    </div>
		  </div>
		  <div class="bottomUrls">
			<a href="http://m.london2012.com/our-terms" rel="external" accesskey="7">Terms</a>
            <span class="sep"> | </span>
            <a href="http://m.london2012.com/policy">Privacy Policy</a>
            <asp:Label ID="lblSeperatorCookieEn" runat="server" EnableViewState="false" Text=" | " />
			<a href="http://m.london2012.com/cookies-policy" id="lnkCookiePolicyEn" runat="server" enableviewstate="false" rel="external" accesskey="8">Cookies policy</a>
		  </div>
	      <div class="copyright">&#169; London 2012</div>
		</div>
	</div>
    
</asp:Panel>

<% // Footer panel for French version of London2012 footer  %>
<asp:Panel ID="pnlFooterFr" runat="server" EnableViewState="false">

    <div class="clear"></div>
	<div class="footer">
		<div id="bottomNav" class="bottomNav">
		  <h2 class="hide">Main menu</h2>
		  <a name="nav"></a>
		  <div id="nav" class="navWrap">
			<div class="colNav colNavFirst">
			  <ul>
				<li class="navItem">
                  <a id="mainMenuFrUrl" runat="server" href="~" rel="external" accesskey="3">Accueil de l'outil de planification de trajet</a>
				</li>
				<li id="liLanguageLinkFr" runat="server" class="navItem language">
					Langue 
                    <asp:LinkButton id="lnkbtnLanguageEn" runat="server" CssClass="languageLink" AccessKey="4"></asp:LinkButton>
				</li>
			  </ul>
			</div>
            <div class="colNav colNavSecond">
              <ul>
				<li class="navItem">
				    <a id="linktop" href="#top" rel="external" accesskey="5">Revenir au début de page</a>
				</li>
			  </ul>
			</div>
            <div class="clear"></div>
            <div class="colNav colNav2 colNavFirst colNavFirst2">
			  <ul>
                <li class="navItem ldn-oly">
                    <a href="http://m.london2012.com/">Olympic Games</a>
                </li>
              </ul>
			</div>
			<div class="colNav colNav2 colNavSecond colNavSecond2">
			  <ul>
                <li class="navItem ldn-para">
                    <a href="http://m.london2012.com/paralympics">Paralympic Games</a>
                </li>
              </ul>
            </div>
			<div class="clear"></div>
			<div class="fullsite">
			  <a href="http://travel.london2012.com/SJPWeb/?l=fr&dnr=true" rel="external" accesskey="6">Site intégral</a>
		    </div>
		  </div>
		  <div class="bottomUrls">
			<a href="http://m.london2012.com/our-terms" rel="external" accesskey="7">Termes</a>
            <span class="sep"> | </span>
            <a href="http://m.london2012.com/policy">Politique de confidentialité</a>
			<asp:Label ID="lblSeperatorCookieFr" runat="server" EnableViewState="false" Text=" | " />
			<a href="http://m.london2012.com/cookies-policy" id="lnkCookiePolicyFr" runat="server" enableviewstate="false" rel="external" accesskey="8">Politique sur les cookies</a>
		   </div>
	       <div class="copyright">&#169; London 2012</div>
		</div>
	</div>

</asp:Panel>
<div class="clear"></div>
