-- =============================================
-- Script Template
-- =============================================

------------------------------------------------
-- General content, all added to the group 'HeaderFooter'
------------------------------------------------

DECLARE @Group varchar(100) = 'HeaderFooter'
DECLARE @Collection varchar(100) = 'General'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

------------------------------------------------------------------------------------------------------------------
-- Header - Olympics
------------------------------------------------------------------------------------------------------------------

-- Olympics Sub-section 1 - English
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Olympics.PrimaryContainer.Html',
'
<div id="lcg-lev0menuWrap" class="nav role-navigation">
	<h2 class="hidden">Main menu</h2>
	<ul class="lcg-topnav role-menu" id="lcg-lev0menu">
		<li class=" first "><a href="http://www.london2012.com/schedule-and-results">Schedule &amp; Results</a></li>
		<li class=""><a href="http://www.london2012.com/torch-relay">Torch Relay</a></li>
		<li class=""><a href="http://www.london2012.com/sports">Sports</a>
			<div class="flyOut hide">
				<ul class="sportsMenu g1 role-menu">
					<li><a href="http://www.london2012.com/archery"><span class="wn"><span class="sport-ico ar"> </span>Archery</span></a></li>
					<li><a href="http://www.london2012.com/athletics"><span class="wn"><span class="sport-ico at"> </span>Athletics</span></a></li>
					<li><a href="http://www.london2012.com/badminton"><span class="wn"><span class="sport-ico bd"> </span>Badminton</span></a></li>
					<li><a href="http://www.london2012.com/basketball"><span class="wn"><span class="sport-ico bk"> </span>Basketball</span></a></li>
					<li><a href="http://www.london2012.com/beach-volleyball"><span class="wn"><span class="sport-ico bv"> </span>Beach Volleyball</span></a></li>
					<li class=" last"><a href="http://www.london2012.com/boxing"><span class="wn"><span class="sport-ico bx"> </span>Boxing</span></a></li>
				</ul>
				<ul class="sportsMenu g2 role-menu">
					<li><a href="http://www.london2012.com/canoe-slalom"><span class="wn"><span class="sport-ico cs"> </span>Canoe Slalom</span></a></li>
					<li><a href="http://www.london2012.com/canoe-sprint"><span class="wn"><span class="sport-ico cf"> </span>Canoe Sprint</span></a></li>
					<li><a href="http://www.london2012.com/cycling-bmx"><span class="wn"><span class="sport-ico cb"> </span>Cycling - BMX</span></a></li>
					<li><a href="http://www.london2012.com/cycling-mountain-bike"><span class="wn"><span class="sport-ico cm"> </span>Cycling - Mountain Bike</span></a></li>
					<li><a href="http://www.london2012.com/cycling-road"><span class="wn"><span class="sport-ico cr"> </span>Cycling - Road</span></a></li>
					<li class=" last"><a href="http://www.london2012.com/cycling-track"><span class="wn"><span class="sport-ico ct"> </span>Cycling - Track</span></a></li>
				</ul>
				<ul class="sportsMenu g3 role-menu">
					<li><a href="http://www.london2012.com/diving"><span class="wn"><span class="sport-ico dv"> </span>Diving</span></a></li>
					<li><a href="http://www.london2012.com/equestrian"><span class="wn"><span class="sport-ico eq"> </span>Equestrian</span></a></li>
					<li><a href="http://www.london2012.com/fencing"><span class="wn"><span class="sport-ico fe"> </span>Fencing</span></a></li>
					<li><a href="http://www.london2012.com/football"><span class="wn"><span class="sport-ico fb"> </span>Football</span></a></li>
					<li><a href="http://www.london2012.com/gymnastics-artistic"><span class="wn"><span class="sport-ico ga"> </span>Gymnastics - Artistic</span></a></li>
					<li class=" last"><a href="http://www.london2012.com/gymnastics-rhythmic"><span class="wn"><span class="sport-ico gr"> </span>Gymnastics - Rhythmic</span></a></li>
				</ul>
				<ul class="sportsMenu g4 role-menu">
					<li><a href="http://www.london2012.com/handball"><span class="wn"><span class="sport-ico hb"> </span>Handball</span></a></li>
					<li><a href="http://www.london2012.com/hockey"><span class="wn"><span class="sport-ico ho"> </span>Hockey</span></a></li>
					<li><a href="http://www.london2012.com/judo"><span class="wn"><span class="sport-ico ju"> </span>Judo</span></a></li>
					<li><a href="http://www.london2012.com/modern-pentathlon"><span class="wn"><span class="sport-ico mp"> </span>Modern Pentathlon</span></a></li>
					<li><a href="http://www.london2012.com/rowing"><span class="wn"><span class="sport-ico ro"> </span>Rowing</span></a></li>
					<li class=" last"><a href="http://www.london2012.com/sailing"><span class="wn"><span class="sport-ico sa"> </span>Sailing</span></a></li>
				</ul>
				<ul class="sportsMenu g5 role-menu">
					<li><a href="http://www.london2012.com/shooting"><span class="wn"><span class="sport-ico sh"> </span>Shooting</span></a></li>
					<li><a href="http://www.london2012.com/swimming"><span class="wn"><span class="sport-ico sw"> </span>Swimming</span></a></li>
					<li><a href="http://www.london2012.com/synchronized-swimming"><span class="wn"><span class="sport-ico sy"> </span>Synchronised Swimming</span></a></li>
					<li><a href="http://www.london2012.com/table-tennis"><span class="wn"><span class="sport-ico tt"> </span>Table Tennis</span></a></li>
					<li><a href="http://www.london2012.com/taekwondo"><span class="wn"><span class="sport-ico tk"> </span>Taekwondo</span></a></li>
					<li class=" last"><a href="http://www.london2012.com/tennis"><span class="wn"><span class="sport-ico te"> </span>Tennis</span></a></li>
				</ul>
				<ul class="sportsMenu g6 last role-menu">
					<li><a href="http://www.london2012.com/gymnastic-trampoline"><span class="wn"><span class="sport-ico gt"> </span>Trampoline</span></a></li>
					<li><a href="http://www.london2012.com/triathlon"><span class="wn"><span class="sport-ico tr"> </span>Triathlon</span></a></li>
					<li><a href="http://www.london2012.com/volleyball"><span class="wn"><span class="sport-ico vo"> </span>Volleyball</span></a></li>
					<li><a href="http://www.london2012.com/water-polo"><span class="wn"><span class="sport-ico wp"> </span>Water Polo</span></a></li>
					<li><a href="http://www.london2012.com/weightlifting"><span class="wn"><span class="sport-ico wl"> </span>Weightlifting</span></a></li>
					<li class=" last"><a href="http://www.london2012.com/wrestling"><span class="wn"><span class="sport-ico wr"> </span>Wrestling</span></a></li>
				</ul>
			</div>
		</li>
		<li class=""><a href="http://www.london2012.com/athletes">Athletes</a></li>
		<li class=""><a href="http://www.london2012.com/countries">Countries</a></li>
		<li class=""><a href="http://www.london2012.com/join-in">Join in</a></li>
		<li class="current"><a href="http://www.london2012.com/spectators">Spectators</a></li>
		<li class=""><a href="http://www.london2012.com/news">News</a></li>
		<li class=" last "><a href="http://www.london2012.com/photos">Photos</a></li>
		<li class=" ldn-shop "><a href="http://shop.london2012.com/" class="external">Shop</a></li>
		<li class=" ldn-ticket "><a href="http://www.tickets.london2012.com/" class="external">Tickets</a></li>
		<li class=" ldn-para "><a href="http://www.london2012.com/paralympics">Paralympic Games</a></li>
	</ul>
</div>
<div class="clear"><hr /> </div><!--googleon: all-->
'

-- Olympics Sub-section 1 - French
EXEC AddContent @Group, @CultFr, @Collection, 'Header.Olympics.PrimaryContainer.Html',
'
<div class="nav role-navigation" id="lcg-lev0menuWrap">
    <h2 class="hidden"><a href="#" id="mainMenu" name="mainMenu">Menu principal</a></h2>
    <ul id="lcg-lev0menu" class="lcg-topnav role-menu sf-js-enabled sf-shadow">
        <li class="first"><a id="schedule" href="http://fr.london2012.com/fr/schedule-and-results/">Calendrier et résultats</a></li>
        <li class=""><a id="medals" href="http://fr.london2012.com/fr/medals/">Médailles</a></li>
        <li class=""><a id="sports" href="http://fr.london2012.com/fr/sports/" class="sf-with-ul">Sports<span class="sf-sub-indicator"></span></a>
            <div class="flyOut hide">
                <div class="skipTo">
                    <a href="#athletes">Passer la liste des sports</a></div>
                <ul class="sportsMenu g1 role-menu">
                    <li><a href="http://fr.london2012.com/fr/athletics"><span class="wn"><span class="sport-ico at"> </span>Athlétisme</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/rowing"><span class="wn"><span class="sport-ico ro"> </span>Aviron</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/badminton"><span class="wn"><span class="sport-ico bd"> </span>Badminton</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/basketball"><span class="wn"><span class="sport-ico bk"> </span>Basketball</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/boxing"><span class="wn"><span class="sport-ico bx"> </span>Boxe</span></a></li>
                    <li class=" last"><a href="http://fr.london2012.com/fr/canoe-sprint"><span class="wn"> <span class="sport-ico cf"></span>Canoë-kayak, course en ligne</span></a></li></ul>
                <ul class="sportsMenu g2 role-menu">
                    <li><a href="http://fr.london2012.com/fr/canoe-slalom"><span class="wn"><span class="sport-ico cs"> </span>Canoë slalom</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/cycling-bmx"><span class="wn"><span class="sport-ico cb"> </span>Cyclisme - BMX</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/cycling-mountain-bike"><span class="wn"><span class="sport-ico cm"> </span>Cyclisme - Mountain bike</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/cycling-track"><span class="wn"><span class="sport-ico ct"> </span>Cyclisme - Piste</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/cycling-road"><span class="wn"><span class="sport-ico cr"> </span>Cyclisme - Route</span></a></li>
                    <li class=" last"><a href="http://fr.london2012.com/fr/fencing"><span class="wn"><span class="sport-ico fe"></span>Escrime</span></a></li></ul>
                <ul class="sportsMenu g3 role-menu">
                    <li><a href="http://fr.london2012.com/fr/football"><span class="wn"><span class="sport-ico fb"> </span>Football</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/gymnastic-trampoline"><span class="wn"><span class="sport-ico gt"></span>Gymnastique &ndash; Trampoline</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/gymnastics-artistic"><span class="wn"><span class="sport-ico ga"></span>Gymnastique artistique</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/gymnastics-rhythmic"><span class="wn"><span class="sport-ico gr"></span>Gymnastique rythmique</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/weightlifting"><span class="wn"><span class="sport-ico wl"> </span>Haltérophilie</span></a></li>
                    <li class=" last"><a href="http://fr.london2012.com/fr/handball"><span class="wn"><span class="sport-ico hb"></span>Handball</span></a></li></ul>
                <ul class="sportsMenu g4 role-menu">
                    <li><a href="http://fr.london2012.com/fr/hockey"><span class="wn"><span class="sport-ico ho"> </span>Hockey</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/judo"><span class="wn"><span class="sport-ico ju"> </span>Judo</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/wrestling"><span class="wn"><span class="sport-ico wr"> </span>Lutte</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/swimming"><span class="wn"><span class="sport-ico sw"> </span>Natation</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/synchronized-swimming"><span class="wn"><span class="sport-ico sy"></span>Natation synchronisée</span></a></li>
                    <li class=" last"><a href="http://fr.london2012.com/fr/modern-pentathlon"><span class="wn"> <span class="sport-ico mp"></span>Pentathlon moderne</span></a></li></ul>
                <ul class="sportsMenu g5 role-menu">
                    <li><a href="http://fr.london2012.com/fr/diving"><span class="wn"><span class="sport-ico dv"> </span>Plongeon</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/equestrian"><span class="wn"><span class="sport-ico eq"> </span>Sports équestres</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/taekwondo"><span class="wn"><span class="sport-ico tk"> </span>Taekwondo</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/tennis"><span class="wn"><span class="sport-ico te"> </span>Tennis</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/table-tennis"><span class="wn"><span class="sport-ico tt"> </span>Tennis de table</span></a></li>
                    <li class=" last"><a href="http://fr.london2012.com/fr/shooting"><span class="wn"><span class="sport-ico sh"></span>Tir</span></a></li></ul>
                <ul class="sportsMenu g6 last role-menu">
                    <li><a href="http://fr.london2012.com/fr/archery"><span class="wn"><span class="sport-ico ar"> </span>Tir à l’arc</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/triathlon"><span class="wn"><span class="sport-ico tr"> </span>Triathlon</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/sailing"><span class="wn"><span class="sport-ico sa"> </span>Voile</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/volleyball"><span class="wn"><span class="sport-ico vo"> </span>Volleyball</span></a></li>
                    <li><a href="http://fr.london2012.com/fr/beach-volleyball"><span class="wn"><span class="sport-ico bv"></span>Volleyball de plage</span></a></li>
                    <li class=" last"><a href="http://fr.london2012.com/fr/water-polo"><span class="wn"> <span class="sport-ico wp"></span>Water-polo</span></a></li></ul>
            </div>
        </li>
        <li class=""><a id="athletes" href="http://fr.london2012.com/fr/athletes/">Athlètes</a></li>
        <li class=""><a id="join-in" href="http://fr.london2012.com/fr/join-in/">Participer</a></li>
        <li class=""><a id="spectators" href="http://fr.london2012.com/fr/spectators/venues/">Spectateurs</a></li>
        <li class="last"><a id="news" href="http://fr.london2012.com/fr/news/">Actus</a></li>
        <li class="ldn-shop"><a id="theShop" class="external" href="http://shop.london2012.com?cm_mmc=LOCOG-_-website-_-navigation-_-homepage">Boutique</a></li>
        <li class="ldn-ticket"><a id="tickets" class="external" href="http://www.tickets.london2012.com/">Billets</a></li>
        <li class="ldn-para"><a id="paralympics" href="http://www.london2012.com/paralympics/">Jeux Paralympiques</a></li></ul>
</div>
<div class="clear"><hr /> </div><!--googleon: all-->
'

-- Olympics Sub-section 2 - English
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Olympics.SecondaryContainer.Html',
'
<div id="lcg-lev2menuWrap" class="role-navigation">
	<h2 class="hidden">Secondary Menu</h2>
	<ul id="lcg-lev2menu" class="role-menu">
		<li class="current">
			<a href="http://www.london2012.com/spectators/travel">Travel</a> 
			<div class="flyOut hide">
				<ul> 
					<li class="current"><a  class="current" href="http://travel.london2012.com/SJPWeb/Pages/JourneyPlannerInput.aspx"><span><span> Plan your travel</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/book-your-travel"><span><span> Book your travel</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/accessible-travel"><span><span> Accessible travel</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/grup-travel"><span><span> Group travel</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/games-travelcard"><span><span> Games Travelcard</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/walking"><span><span> Walking</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/travelling-from-outside-uk"><span><span> Travelling from outside the UK</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/cycling"><span><span> sport_cycling</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/travelling-in-london"><span><span> Travelling in London</span></span></a></li>
				</ul>
			</div>
		</li>
		<li><a href="http://www.london2012.com/spectators/venues">Venues</a></li>
		<li>
			<a href="http://www.london2012.com/spectators/visiting">Visiting</a> 
			<div class="flyOut hide">
				<ul> 
					<li><a href="http://www.london2012.com/spectators/visiting/london-and-uk"><span><span> London and the UK</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/visiting/accommodation"><span><span> Accommodation</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/visiting/safety-and-secutity"><span><span> Safety and security</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/visiting/faith-communities"><span><span> Faith communities</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/visiting/food-and-drink"><span><span> Food and drink</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/travel/travelling-in-london"><span><span> Travelling in London</span></span></a></li>
				</ul>
			</div>
		</li>
		<li>
			<a href="http://www.london2012.com/spectators/ceremonies">Ceremonies</a> 
			<div class="flyOut hide">
				<ul> 
					<li><a href="http://www.london2012.com/spectators/ceremonies/opening-ceremony"><span><span> Opening Ceremonies</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/ceremonies/closing-ceremony"><span><span> Closing Ceremonies</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/ceremonies/team-welcome-ceremony"><span><span> Team Welcome Ceremonies</span></span></a></li>
					<li><a href="http://www.london2012.com/spectators/ceremonies/olympic-victory-ceremonies"><span><span> Victory Ceremonies</span></span></a></li>
				</ul>
			</div>
		</li>
		<li class="accInfo">
			<a href="http://www.london2012.com/spectators/accessibility"><span class="wrapImgAcc"> </span>Accessibility</a> 
			<div class="flyOut hide">
				<ul> 
					<li><a href="http://www.london2012.com/spectators/travel/accessible-travel"><span><span> Accessible travel</span></span></a></li>
					<li><a href="http://www.london2012.com/accessibility-statement"><span><span> Web accessibility statement</span></span></a></li>
				</ul>
			</div>
		</li>
		<li>
			<a href="http://www.london2012.com/spectators/tickets">Tickets</a> 
			<div class="flyOut hide">
				<ul> 
					<li><a href="http://www.london2012.com/spectators/tickets/ticket-checker"><span><span> Ticketing website checker</span></span></a></li>
				</ul>
			</div>
		</li>
		<li class=" last "><a href="http://www.london2012.com/spectators/games-maker">Games Makers</a></li>
	</ul>
</div>
<div class="clear"><hr /> </div>
<!--googleon: all-->
'

-- Olympics Sub-section 2 - French
EXEC AddContent @Group, @CultFr, @Collection, 'Header.Olympics.SecondaryContainer.Html',
''

------------------------------------------------------------------------------------------------------------------
-- Header - Paralympics
------------------------------------------------------------------------------------------------------------------

-- Paralympics Sub-section 1 - English
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Paralympics.PrimaryContainer.Html',
'
    <div class="nav role-navigation" id="lcg-lev0menuWrap">
      <h2 class="hidden">
        <a href="#" id="mainMenu" name="mainMenu">Main menu</a>
      </h2>
      <ul id="lcg-lev0menu" class="lcg-topnav role-menu sf-js-enabled sf-shadow">
        <li class=" first">
          <a id="schedule" href="http://www.london2012.com/paralympics/schedule-and-results" name="schedule">Schedule &amp; Results</a>
        </li>
        <li class="">
          <a id="torch-relay" href="http://www.london2012.com/paralympics/torch-relay" name="torch-relay">Torch Relay</a>
        </li>
        <li class="">
          <a id="sports" href="http://www.london2012.com/paralympics/sports" class="sf-with-ul" name="sports">Sports<span class="sf-sub-indicator"></span></a>
          <div class="flyOut hide">
            <div class="skipTo">
              <a href="#athletes">Skip sports list</a>
            </div>
            <div class="classif">
              <a href="http://www.london2012.com/paralympics/sports/classification.html">Classification explained</a>
              <hr>
            </div>
            <ul class="sportsMenu g1 role-menu">
              <li>
                <a href="http://www.london2012.com/paralympics/archery"><span class="wn">Archery</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/athletics"><span class="wn">Athletics</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/boccia"><span class="wn">Boccia</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/cycling-road"><span class="wn">Cycling Road</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/cycling-track"><span class="wn">Cycling Track</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/equestrian"><span class="wn">Equestrian</span></a>
              </li>
              <li class=" last">
                <a href="http://www.london2012.com/paralympics/football-5-a-side"><span class="wn">Football 5-a-side</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g2 role-menu">
              <li>
                <a href="http://www.london2012.com/paralympics/football-7-a-side"><span class="wn">Football 7-a-side</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/goalball"><span class="wn">Goalball</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/judo"><span class="wn">Judo</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/powerlifting"><span class="wn">Powerlifting</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/rowing"><span class="wn">Rowing</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/sailing"><span class="wn">Sailing</span></a>
              </li>
              <li class=" last">
                <a href="http://www.london2012.com/paralympics/shooting"><span class="wn">Shooting</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g3 role-menu last">
              <li>
                <a href="http://www.london2012.com/paralympics/swimming"><span class="wn">Swimming</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/table-tennis"><span class="wn">Table Tennis</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/sitting-volleyball"><span class="wn">Sitting Volleyball</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/wheelchair-basketball"><span class="wn">Wheelchair Basketball</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/wheelchair-fencing"><span class="wn">Wheelchair Fencing</span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/wheelchair-rugby"><span class="wn">Wheelchair Rugby</span></a>
              </li>
              <li class=" last">
                <a href="http://www.london2012.com/paralympics/wheelchair-tennis"><span class="wn">Wheelchair Tennis</span></a>
              </li>
            </ul>
          </div>
        </li>
        <li class="">
          <a id="athletes" href="http://www.london2012.com/paralympics/athletes" name="athletes">Athletes</a>
        </li>
        <li class="">
          <a id="countries" href="http://www.london2012.com/paralympics/countries" name="countries">Countries</a>
        </li>
        <li class="">
          <a id="join-in" href="http://www.london2012.com/paralympics/join-in" name="join-in">Join in</a>
        </li>
        <li class="current">
          <a id="spectators" href="http://www.london2012.com/paralympics/spectators" name="spectators">Spectators</a>
        </li>
        <li class="">
          <a id="news" href="http://www.london2012.com/paralympics/news" name="news">News</a>
        </li>
        <li class=" last">
          <a id="photos" href="http://www.london2012.com/paralympics/photos" name="photos">Photos</a>
        </li>
        <li class=" ldn-shop">
          <a id="theShop" class="external" href="http://shop.london2012.com?cm_mmc=LOCOG-_-website-_-navigation-_-homepage" name="theShop">Shop</a>
        </li>
        <li class=" ldn-ticket">
          <a id="tickets" class="external" href="http://www.tickets.london2012.com/" name="tickets">Tickets</a>
        </li>
        <li class=" ldn-oly">
          <a id="olympics" href="http://www.london2012.com/" name="olympics">Olympic Games</a>
        </li>
      </ul>
    </div>
	<div class="clear"><hr /> </div>
'

-- Paralympics Sub-section 1 - French
EXEC AddContent @Group, @CultFr, @Collection, 'Header.Paralympics.PrimaryContainer.Html',
'
    <div class="nav role-navigation" id="lcg-lev0menuWrap">
      <h2 class="hidden">
        <a href="#" id="mainMenu" name="mainMenu">Menu principal</a>
      </h2>
      <ul id="lcg-lev0menu" class="lcg-topnav role-menu sf-js-enabled sf-shadow">
        <li class="first">
          <a id="schedule" href="http://fr.london2012.com/fr/schedule-and-results/" name="schedule">Calendrier et résultats</a>
        </li>
        <li class="">
          <a id="medals" href="http://fr.london2012.com/fr/medals/" name="medals">Médailles</a>
        </li>
        <li class="">
          <a id="sports" href="http://fr.london2012.com/fr/sports/" class="sf-with-ul" name="sports">Sports</a>
          <div class="flyOut hide">
            <div class="skipTo">
              <a href="#athletes">Passer la liste des sports</a>
            </div>
            <ul class="sportsMenu g1 role-menu">
              <li>
                <a href="http://fr.london2012.com/fr/athletics"><span class="wn">Athlétisme</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/rowing"><span class="wn">Aviron</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/badminton"><span class="wn">Badminton</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/basketball"><span class="wn">Basketball</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/boxing"><span class="wn">Boxe</span></a>
              </li>
              <li class=" last">
                <a href="http://fr.london2012.com/fr/canoe-sprint"><span class="wn">Canoë-kayak, course en ligne</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g2 role-menu">
              <li>
                <a href="http://fr.london2012.com/fr/canoe-slalom"><span class="wn">Canoë slalom</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/cycling-bmx"><span class="wn">Cyclisme - BMX</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/cycling-mountain-bike"><span class="wn">Cyclisme - Mountain bike</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/cycling-track"><span class="wn">Cyclisme - Piste</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/cycling-road"><span class="wn">Cyclisme - Route</span></a>
              </li>
              <li class=" last">
                <a href="http://fr.london2012.com/fr/fencing"><span class="wn">Escrime</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g3 role-menu">
              <li>
                <a href="http://fr.london2012.com/fr/football"><span class="wn">Football</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/gymnastic-trampoline"><span class="wn">Gymnastique  Trampoline</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/gymnastics-artistic"><span class="wn">Gymnastique artistique</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/gymnastics-rhythmic"><span class="wn">Gymnastique rythmique</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/weightlifting"><span class="wn">Haltérophilie</span></a>
              </li>
              <li class=" last">
                <a href="http://fr.london2012.com/fr/handball"><span class="wn">Handball</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g4 role-menu">
              <li>
                <a href="http://fr.london2012.com/fr/hockey"><span class="wn">Hockey</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/judo"><span class="wn">Judo</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/wrestling"><span class="wn">Lutte</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/swimming"><span class="wn">Natation</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/synchronized-swimming"><span class="wn">Natation synchronisée</span></a>
              </li>
              <li class=" last">
                <a href="http://fr.london2012.com/fr/modern-pentathlon"><span class="wn">Pentathlon moderne</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g5 role-menu">
              <li>
                <a href="http://fr.london2012.com/fr/diving"><span class="wn">Plongeon</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/equestrian"><span class="wn">Sports équestres</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/taekwondo"><span class="wn">Taekwondo</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/tennis"><span class="wn">Tennis</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/table-tennis"><span class="wn">Tennis de table</span></a>
              </li>
              <li class=" last">
                <a href="http://fr.london2012.com/fr/shooting"><span class="wn">Tir</span></a>
              </li>
            </ul>
            <ul class="sportsMenu g6 last role-menu">
              <li>
                <a href="http://fr.london2012.com/fr/archery"><span class="wn">Tir à l’arc</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/triathlon"><span class="wn">Triathlon</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/sailing"><span class="wn">Voile</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/volleyball"><span class="wn">Volleyball</span></a>
              </li>
              <li>
                <a href="http://fr.london2012.com/fr/beach-volleyball"><span class="wn">Volleyball de plage</span></a>
              </li>
              <li class=" last">
                <a href="http://fr.london2012.com/fr/water-polo"><span class="wn">Water-polo</span></a>
              </li>
            </ul>
          </div>
        </li>
        <li class="">
          <a id="athletes" href="http://fr.london2012.com/fr/athletes/" name="athletes">Athlètes</a>
        </li>
        <li class="">
          <a id="join-in" href="http://fr.london2012.com/fr/join-in/" name="join-in">Participer</a>
        </li>
        <li class="">
          <a id="spectators" href="http://fr.london2012.com/fr/spectators/venues/" name="spectators">Spectateurs</a>
        </li>
        <li class="last">
          <a id="news" href="http://fr.london2012.com/fr/news/" name="news">Actus</a>
        </li>
        <li class="ldn-shop">
          <a id="theShop" class="external" href="http://shop.london2012.com?cm_mmc=LOCOG-_-website-_-navigation-_-homepage" name="theShop">Boutique</a>
        </li>
        <li class="ldn-ticket">
          <a id="tickets" class="external" href="http://www.tickets.london2012.com/" name="tickets">Billets</a>
        </li>
        <li class="ldn-oly">
          <a id="olympics" href="http://www.london2012.com/" name="olympics">Jeux Olympiques</a>
        </li>
      </ul>
    </div>
    <div class="clear"><hr /> </div>
'

-- Paralympics Sub-section 2 - English
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Paralympics.SecondaryContainer.Html',
'
    <div class="role-navigation" id="lcg-lev2menuWrap">
      <h2 class="hidden">
        Secondary Menu
      </h2>
      <ul class="role-menu sf-js-enabled sf-shadow" id="lcg-lev2menu">
        <li class="current">
          <a href="http://www.london2012.com/paralympics/spectators/travel/" class="sf-with-ul">Travel<span class="sf-sub-indicator"></span></a>
          <div class="flyOut hide">
            <ul>
              <li class="current">
                <a href="http://travel.london2012.com/SJPWeb/Pages/JourneyPlannerInput.aspx"><span><span>Plan your travel</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/book-your-travel/"><span><span>Book your travel</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/accessible-travel/" data-found="true"><span><span>Accessible travel</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/group-travel/"><span><span>Group travel</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/games-travelcard/"><span><span>Games Travelcard</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/walking/"><span><span>Walking</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/travelling-from-outside-uk/"><span><span>Travelling from outside the UK</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/cycling/"><span><span>Cycling</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/travelling-in-london/"><span><span>Travelling in London</span></span></a>
              </li>
            </ul>
          </div>
        </li>
        <li data-rel="/paralympics/spectators">
          <a alias="/venue" href="http://www.london2012.com/paralympics/spectators/venues/">Venues</a>
        </li>
        <li>
          <a href="http://www.london2012.com/paralympics/spectators/visiting/" class="sf-with-ul">Visiting<span class="sf-sub-indicator"></span></a>
          <div class="flyOut hide">
            <ul>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/visiting/london-and-uk/"><span><span>London and the UK</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/visiting/accommodation/"><span><span>Accommodation</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/visiting/safety-and-security/"><span><span>Safety and security</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/visiting/families/"><span><span>Families</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/visiting/faith-communities/"><span><span>Faith communities</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/visiting/food-and-drink/"><span><span>Food and drink</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/travel/travelling-in-london/"><span><span>Travelling in London</span></span></a>
              </li>
            </ul>
          </div>
        </li>
        <li>
          <a href="http://www.london2012.com/paralympics/spectators/ceremonies/" class="sf-with-ul">Ceremonies<span class="sf-sub-indicator"></span></a>
          <div class="flyOut hide">
            <ul>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/ceremonies/opening-ceremony/"><span><span>Opening Ceremony</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/ceremonies/closing-ceremony/"><span><span>Closing Ceremony</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/ceremonies/team-welcome-ceremony/"><span><span>Team Welcome Ceremonies</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/ceremonies/olympic-victory-ceremonies/"><span><span>Victory Ceremonies</span></span></a>
              </li>
            </ul>
          </div>
        </li>
        <li class="accInfo">
          <a href="http://www.london2012.com/paralympics/spectators/accessibility/" class="sf-with-ul"><span class="wrapImgAcc"> </span>Accessibility<span class="sf-sub-indicator"></span></a>
          <div class="flyOut hide">
            <ul>
              <li class="current">
                <a href="http://www.london2012.com/paralympics/spectators/travel/accessible-travel/" data-found="true"><span><span>Accessible travel</span></span></a>
              </li>
              <li>
                <a href="http://www.london2012.com/paralympics/accessibility-statement/"><span><span>Web accessibility statement</span></span></a>
              </li>
            </ul>
          </div>
        </li>
        <li>
          <a href="http://www.london2012.com/paralympics/spectators/tickets/" class="sf-with-ul">Tickets<span class="sf-sub-indicator"></span></a>
          <div class="flyOut hide">
            <ul>
              <li>
                <a href="http://www.london2012.com/paralympics/spectators/tickets/ticket-checker/"><span><span>Ticketing website checker</span></span></a>
              </li>
            </ul>
          </div>
        </li>
        <li class="last">
          <a href="http://www.london2012.com/paralympics/spectators/games-maker/">Games Makers</a>
        </li>
      </ul>
    </div>
	<div class="clear"><hr /> </div>
'

-- Paralympics Sub-section 2 - French
EXEC AddContent @Group, @CultFr, @Collection, 'Header.Paralympics.SecondaryContainer.Html',
''


------------------------------------------------------------------------------------------------------------------
-- Footer - Olympics
------------------------------------------------------------------------------------------------------------------

-- English
EXEC AddContent @Group, @CultEn, @Collection, 'Footer.Olympics.Html',
'
<div class="footer" id="lcg-footer">
	<!--googleoff: all-->
	<h2 class="hidden">Footer menu</h2>
    <div id="footerTop">
        <div id="colsWrap">
            <div class="colLinks" id="quickLinksList">
                <span class="bottomLinks footAbout"><a href="http://www.london2012.com/about-us/index.html">
                    <span class="ico"></span>About us</a> </span><span class="bottomLinks">Quick Links</span>
                <div class="nav">
                    <ul class="wide">
                        <li><a href="http://www.london2012.com/contact-us">Contact us</a> </li>
                        <li><a href="http://www.london2012.com/media-centre">Media centre</a> </li>
                        <li><a href="http://www.london2012.com/business">For businesses</a> </li>
                        <li><a href="http://www.london2012.com/local-residents">For local residents</a></li>
                        <li><a href="http://www.london2012.com/about-us/nocs">For NOCs</a> </li>
                        <li><a href="http://www.london2012.com/about-us/publications">Publications</a> </li>
                        <li><a class="external" href="http://m.london2012.com">View mobile site</a> </li>
                        <li><a href="http://www.london2012.com/mobileapps/index.html">Mobile apps</a> </li>
                    </ul>
                </div>
            </div>
            <div class="colLinks" id="sportsList">
                <span class="bottomLinks">Sports</span>
                <div class="nav">
                    <ul>
                        <li><a href="http://www.london2012.com/archery/index.html">Archery</a> </li>
                        <li><a href="http://www.london2012.com/athletics/index.html">Athletics</a> </li>
                        <li><a href="http://www.london2012.com/badminton/index.html">Badminton</a> </li>
                        <li><a href="http://www.london2012.com/basketball/index.html">Basketball</a> </li>
                        <li><a href="http://www.london2012.com/beach-volleyball/index.html">Beach Volleyball</a></li>
                        <li><a href="http://www.london2012.com/boxing/index.html">Boxing</a> </li>
                        <li><a href="http://www.london2012.com/canoe-slalom/index.html">Canoe Slalom</a></li>
                        <li><a href="http://www.london2012.com/canoe-sprint/index.html">Canoe Sprint</a></li>
                        <li><a href="http://www.london2012.com/cycling-bmx/index.html">Cycling - BMX</a></li>
                        <li><a href="http://www.london2012.com/cycling-mountain-bike/index.html">Cycling - Mountain Bike</a> </li>
                        <li><a href="http://www.london2012.com/cycling-road/index.html">Cycling - Road</a></li>
                        <li><a href="http://www.london2012.com/cycling-track/index.html">Cycling - Track</a></li>
                    </ul>
                    <ul>
                        <li><a href="http://www.london2012.com/diving/index.html">Diving</a> </li>
                        <li><a href="http://www.london2012.com/equestrian/index.html">Equestrian</a> </li>
                        <li><a href="http://www.london2012.com/fencing/index.html">Fencing</a> </li>
                        <li><a href="http://www.london2012.com/football/index.html">Football</a> </li>
                        <li><a href="http://www.london2012.com/gymnastics-artistic/index.html">Gymnastics - Artistic</a> </li>
                        <li><a href="http://www.london2012.com/gymnastics-rhythmic/index.html">Gymnastics - Rhythmic</a> </li>
                        <li><a href="http://www.london2012.com/handball/index.html">Handball</a> </li>
                        <li><a href="http://www.london2012.com/hockey/index.html">Hockey</a> </li>
                        <li><a href="http://www.london2012.com/judo/index.html">Judo</a> </li>
                        <li><a href="http://www.london2012.com/modern-pentathlon/index.html">Modern Pentathlon</a></li>
                        <li><a href="http://www.london2012.com/rowing/index.html">Rowing</a> </li>
                        <li><a href="http://www.london2012.com/sailing/index.html">Sailing</a> </li>
                    </ul>
                    <ul class="last">
                        <li><a href="http://www.london2012.com/shooting/index.html">Shooting</a> </li>
                        <li><a href="http://www.london2012.com/swimming/index.html">Swimming</a> </li>
                        <li><a href="http://www.london2012.com/synchronized-swimming/index.html">Synchronised Swimming</a> </li>
                        <li><a href="http://www.london2012.com/table-tennis/index.html">Table Tennis</a></li>
                        <li><a href="http://www.london2012.com/taekwondo/index.html">Taekwondo</a> </li>
                        <li><a href="http://www.london2012.com/tennis/index.html">Tennis</a> </li>
                        <li><a href="http://www.london2012.com/gymnastic-trampoline/index.html">Trampoline</a></li>
                        <li><a href="http://www.london2012.com/triathlon/index.html">Triathlon</a> </li>
                        <li><a href="http://www.london2012.com/volleyball/index.html">Volleyball</a> </li>
                        <li><a href="http://www.london2012.com/water-polo/index.html">Water Polo</a> </li>
                        <li><a href="http://www.london2012.com/weightlifting/index.html">Weightlifting</a></li>
                        <li><a href="http://www.london2012.com/wrestling/index.html">Wrestling</a> </li>
                    </ul>
                </div>
            </div>
            <div class="colLinks" id="otherLondonSites">
                <span class="bottomLinks">Other London 2012 sites</span>
                <div class="nav">
                    <ul>
                        <li><a class="external" href="http://www.tickets.london2012.com">Tickets</a> </li>
                        <li><a class="external" href="http://shop.london2012.com/on/demandware.store/Sites-ldn-Site/default/Default-Start?cm_mmc=LOCOG-_-website-_-carousel-_-homepage">Shop</a> </li>
                        <li><a class="external" href="http://getset.london2012.com/en/home">Get Set</a></li>
                        <li><a class="external" href="http://youngleaders.london2012.com/young-leaders/">Young Leaders</a> </li>
                        <li><a class="external" href="http://festival.london2012.com/">London 2012 Festival</a></li>
                        <li><a class="external" href="https://mascot-games.london2012.com/">Mascots</a></li>
                        <li><a class="external" href="http://www.londonpreparesseries.com/">London Prepares series</a> </li>
                        <li><a class="external" href="https://volunteer.london2012.com/ESIREG/jsp/_login.jsp">Games Maker zone</a> </li>
                        <li class="ldn-para"><a href="http://www.london2012.com/paralympics">Paralympic Games</a></li>
                    </ul>
                </div>
            </div>
            <div class="colLinks" id="usingThisSite">
                <span class="bottomLinks">Using this site</span>
                <div class="nav">
                    <ul>
                        <li><a href="http://www.london2012.com/sitemap">Site Map</a> </li>
                        <li><a class="ldn-popup" data-attr="top=200,left=300,width=500,height=600" href="http://ask.london2012.com">Ask a Question</a> </li>
                        <li><a href="http://www.london2012.com/accessibility-statement">Web accessibility statement</a></li>
                        <li><a href="http://www.london2012.com/stay-safe-online">Stay safe online</a> </li>
                        <li><a href="http://www.london2012.com/freedom-of-information">Freedom of Information</a></li>
                        <li><a href="http://www.london2012.com/using-this-site">Using this site</a> </li>
                        <li><a href="http://www.london2012.com/privacy-policy">Privacy Policy</a> </li>
                        <li><a href="http://www.london2012.com/cookies-policy">Cookies policy</a> </li>
                        <li><a href="http://www.london2012.com/copyright">Copyright</a> </li>
                        <li><a href="http://www.london2012.com/terms-of-use">Terms of use</a> </li>
                        <li><a href="http://www.london2012.com/spectators/tickets/ticket-checker/index.html">Ticketing website checker</a> </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="footerBottom">
		<div id="footerBottomL">
			<p class="footerPayOff">Official site of the London 2012 Olympic and Paralympic Games</p>
			<div class="footerSocial">
				<span>Follow Us On:</span>
				<ul>
					<li><a class="external" href="http://twitter.com/London2012">Twitter</a></li>
					<li><a class="external" href="http://www.facebook.com/London2012">Facebook</a></li>
					<li><a class="external" href="http://www.youtube.com/london2012">YouTube</a></li>
				</ul>
			</div>
		</div>
		<div id="footerBottomR">
						
		</div>
	</div>
	<!--googleon: all-->		
</div>
'

-- French
EXEC AddContent @Group, @CultFr, @Collection, 'Footer.Olympics.Html',
'
<div id="lcg-footer" class="footer">
    <h2 class="hidden">Menu pied de page</h2>
    <div id="footerTop">
        <div id="colsWrap">
            <div class="colLinks" id="quickLinksList">
                <span class="bottomLinks footAbout"><a href="http://fr.london2012.com/fr/about-us/index.html">
                    <span class="ico"> </span>À propos de nous</a></span>
            </div>
            <div class="colLinks" id="sportsList">
                <span class="bottomLinks">Sports</span>
                <div class="nav">
                    <ul>
                        <li><a href="http://fr.london2012.com/fr/athletics/index.html">Athlétisme</a> </li>
                        <li><a href="http://fr.london2012.com/fr/rowing/index.html">Aviron</a> </li>
                        <li><a href="http://fr.london2012.com/fr/badminton/index.html">Badminton</a> </li>
                        <li><a href="http://fr.london2012.com/fr/basketball/index.html">Basketball</a> </li>
                        <li><a href="http://fr.london2012.com/fr/boxing/index.html">Boxe</a> </li>
                        <li><a href="http://fr.london2012.com/fr/canoe-sprint/index.html">Canoë-kayak, course en ligne</a> </li>
                        <li><a href="http://fr.london2012.com/fr/canoe-slalom/index.html">Canoë slalom</a> </li>
                        <li><a href="http://fr.london2012.com/fr/cycling-bmx/index.html">Cyclisme - BMX</a> </li>
                        <li><a href="http://fr.london2012.com/fr/cycling-mountain-bike/index.html">Cyclisme - Mountain bike</a>
                        </li>
                        <li><a href="http://fr.london2012.com/fr/cycling-track/index.html">Cyclisme - Piste</a> </li>
                        <li><a href="http://fr.london2012.com/fr/cycling-road/index.html">Cyclisme - Route</a> </li>
                        <li><a href="http://fr.london2012.com/fr/fencing/index.html">Escrime</a> </li>
                    </ul>
                    <ul>
                        <li><a href="http://fr.london2012.com/fr/football/index.html">Football</a> </li>
                        <li><a href="http://fr.london2012.com/fr/gymnastic-trampoline/index.html">Gymnastique - Trampoline</a> </li>
                        <li><a href="http://fr.london2012.com/fr/gymnastics-artistic/index.html">Gymnastique artistique</a> </li>
                        <li><a href="http://fr.london2012.com/fr/gymnastics-rhytmic/index.html">Gymnastique rythmique</a> </li>
                        <li><a href="http://fr.london2012.com/fr/weightlifting/index.html">Haltérophilie</a> </li>
                        <li><a href="http://fr.london2012.com/fr/handball/index.html">Handball</a> </li>
                        <li><a href="http://fr.london2012.com/fr/hockey/index.html">Hockey</a> </li>
                        <li><a href="http://fr.london2012.com/fr/judo/index.html">Judo</a> </li>
                        <li><a href="http://fr.london2012.com/fr/wrestling/index.html">Lutte</a> </li>
                        <li><a href="http://fr.london2012.com/fr/swimming/index.html">Natation</a> </li>
                        <li><a href="http://fr.london2012.com/fr/synchronized-swimming/index.html">Natation synchronisée</a> </li>
                        <li><a href="http://fr.london2012.com/fr/modern-pentathlon/index.html">Pentathlon moderne</a> </li>
                    </ul>
                    <ul class="last">
                        <li><a href="http://fr.london2012.com/fr/diving/index.html">Plongeon</a> </li>
                        <li><a href="http://fr.london2012.com/fr/equestrian/index.html">Sports équestres</a> </li>
                        <li><a href="http://fr.london2012.com/fr/taekwondo/index.html">Taekwondo</a> </li>
                        <li><a href="http://fr.london2012.com/fr/tennis/index.html">Tennis</a> </li>
                        <li><a href="http://fr.london2012.com/fr/table-tennis/index.html">Tennis de table</a> </li>
                        <li><a href="http://fr.london2012.com/fr/shooting/index.html">Tir</a> </li>
                        <li><a href="http://fr.london2012.com/fr/archery/index.html">Tir à l’arc</a> </li>
                        <li><a href="http://fr.london2012.com/fr/triathlon/index.html">Triathlon</a> </li>
                        <li><a href="http://fr.london2012.com/fr/sailing/index.html">Voile</a> </li>
                        <li><a href="http://fr.london2012.com/fr/volleyball/index.html">Volleyball</a> </li>
                        <li><a href="http://fr.london2012.com/fr/beach-volleyball/index.html">Volleyball de Plage</a> </li>
                        <li><a href="http://fr.london2012.com/fr/water-polo/index.html">Water-polo</a> </li>
                    </ul>
                </div>
            </div>
            <div class="colLinks" id="otherLondonSites">
                <span class="bottomLinks">Autres sites de Londres 2012</span>
                <div class="nav">
                    <ul>
                        <li><a class="external" href="http://www.tickets.london2012.com">Billets</a> </li>
                        <li><a class="external" href="http://shop.london2012.com/on/demandware.store/Sites-ldn-Site/default/Default-Start?cm_mmc=LOCOG-_-website-_-carousel-_-homepage">
                            Boutique</a> </li>
                        <li><a class="external" href="http://getset.london2012.com/en/home">Get Set</a>
                        </li>
                        <li><a class="external" href="http://youngleaders.london2012.com/young-leaders/">Jeunes
                            responsables</a> </li>
                        <li><a class="external" href="http://festival.london2012.com/">Festival de Londres 2012</a>
                        </li>
                        <li><a class="external" href="https://mascot-games.london2012.com/">Mascottes</a>
                        </li>
                        <li><a class="external" href="http://www.londonpreparesseries.com/">Séries London Prepares</a>
                        </li>
                        <li><a class="external" href="https://volunteer.london2012.com/ESIREG/jsp/_login.jsp">
                            Espace Games Maker</a> </li>
                    </ul>
                </div>
            </div>
            <div class="colLinks" id="usingThisSite">
                <span class="bottomLinks">Utilisation du site</span>
                <div class="nav">
                    <ul>
                        <li><a href="http://fr.london2012.com/fr/privacy-policy/">Politique de confidentialité</a> </li>
                        <li><a href="http://fr.london2012.com/fr/cookies-policy/">Cookies</a> </li>
                        <li><a href="http://fr.london2012.com/fr/terms-of-use/">Conditions d''utilisation</a> </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="footerBottom">
        <div id="footerBottomL">
            <p class="footerPayOff">Site officiel de Londres 2012</p>
            <div class="footerSocial">
                <span>Suivez-nous sur:</span>
                <ul>
                    <li><a href="http://twitter.com/London2012" class="external">Twitter</a> </li>
                    <li><a href="http://www.facebook.com/London2012" class="external">Facebook</a> </li>
                    <li><a href="http://www.youtube.com/london2012" class="external">YouTube</a> </li>
                </ul>
            </div>
        </div>
        <div id="footerBottomR">
                
        </div>
    </div>
</div>
'

------------------------------------------------------------------------------------------------------------------
-- Footer - Paralympics
------------------------------------------------------------------------------------------------------------------

-- English
EXEC AddContent @Group, @CultEn, @Collection, 'Footer.Paralympics.Html',
'
    <div id="lcg-footer" class="footer">
      <h2 class="hidden">
        Footer menu
      </h2>
      <div id="footerTop">
        <div id="colsWrap">
          <div class="colLinks" id="quickLinksList">
            <span class="bottomLinks footAbout"><span class="ico"> </span>
				<a href="http://www.london2012.com/paralympics/about-us/index.html">About us</a></span><span class="bottomLinks">Quick Links</span>
            <div class="nav">
              <ul class="wide">
                <li>
                  <a href="http://www.london2012.com/paralympics/contact-us">Contact us</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/media-centre">Media centre</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/business">For businesses</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/local-residents">For local residents</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/about-us/npcs">For NPCs</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/about-us/publications">Publications</a>
                </li>
                <li>
                  <a class="external" href="http://m.london2012.com/paralympics">View mobile site</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/mobileapps/index.html">Mobile apps</a>
                </li>
              </ul>
            </div>
          </div>
          <div class="colLinks" id="sportsList">
            <span class="bottomLinks">Sports</span>
            <div class="nav">
              <ul>
                <li>
                  <a href="http://www.london2012.com/paralympics/archery/index.html">Archery</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/athletics/index.html">Athletics</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/boccia/index.html">Boccia</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/cycling-road/index.html">Cycling Road</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/cycling-track/index.html">Cycling Track</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/equestrian/index.html">Equestrian</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/football-5-a-side/index.html">Football 5-a-side</a>
                </li>
              </ul>
              <ul>
                <li>
                  <a href="http://www.london2012.com/paralympics/football-7-a-side/index.html">Football 7-a-side</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/goalball/index.html">Goalball</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/judo/index.html">Judo</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/powerlifting/index.html">Powerlifting</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/rowing/index.html">Rowing</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/sailing/index.html">Sailing</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/shooting/index.html">Shooting</a>
                </li>
              </ul>
              <ul class="last">
                <li>
                  <a href="http://www.london2012.com/paralympics/swimming/index.html">Swimming</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/table-tennis/index.html">Table Tennis</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/sitting-volleyball/index.html">Sitting Volleyball</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/wheelchair-basketball/index.html">Wheelchair Basketball</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/wheelchair-fencing/index.html">Wheelchair Fencing</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/wheelchair-rugby/index.html">Wheelchair Rugby</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/wheelchair-tennis/index.html">Wheelchair Tennis</a>
                </li>
              </ul>
            </div>
          </div>
          <div class="colLinks" id="otherLondonSites">
            <span class="bottomLinks">Other London 2012 sites</span>
            <div class="nav">
              <ul>
                <li>
                  <a class="external" href="http://www.tickets.london2012.com">Tickets</a>
                </li>
                <li>
                  <a class="external" href="http://shop.london2012.com/on/demandware.store/Sites-ldn-Site/default/Default-Start?cm_mmc=LOCOG-_-website-_-carousel-_-homepage">Shop</a>
                </li>
                <li>
                  <a class="external" href="http://getset.london2012.com/en/home">Get Set</a>
                </li>
                <li>
                  <a class="external" href="http://youngleaders.london2012.com/young-leaders/">Young Leaders</a>
                </li>
                <li>
                  <a class="external" href="http://festival.london2012.com/">London 2012 Festival</a>
                </li>
                <li>
                  <a class="external" href="https://mascot-games.london2012.com/">Mascots</a>
                </li>
                <li>
                  <a class="external" href="http://www.londonpreparesseries.com/">London Prepares series</a>
                </li>
                <li>
                  <a class="external" href="https://volunteer.london2012.com/ESIREG/jsp/_login.jsp">Games Maker zone</a>
                </li>
                <li class="ldn-oly">
                  <a href="http://www.london2012.com">Olympic Games</a>
                </li>
              </ul>
            </div>
          </div>
          <div class="colLinks" id="usingThisSite">
            <span class="bottomLinks">Using this site</span>
            <div class="nav">
              <ul>
                <li>
                  <a href="http://www.london2012.com/paralympics/sitemap">Site Map</a>
                </li>
                <li>
                  <a class="external" href="http://ask.london2012.com">Ask a Question</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/accessibility-statement">Web accessibility statement</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/stay-safe-online">Stay safe online</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/freedom-of-information">Freedom of Information</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/using-this-site">Using this site</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/privacy-policy">Privacy Policy</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/cookies-policy">Cookies policy</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/copyright">Copyright</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/terms-of-use">Terms of use</a>
                </li>
                <li>
                  <a href="http://www.london2012.com/paralympics/spectators/tickets/ticket-checker/index.html">Ticketing website checker</a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      <div id="footerBottom">
        <div id="footerBottomL">
          <p class="footerPayOff">
            Official site of the London 2012 Olympic and Paralympic Games
          </p>
          <div class="footerSocial">
            <span>Follow Us On:</span>
            <ul>
              <li>
                <a href="http://twitter.com/London2012" class="external">Twitter</a>
              </li>
              <li>
                <a href="http://www.facebook.com/London2012" class="external">Facebook</a>
              </li>
              <li>
                <a href="http://www.youtube.com/london2012" class="external">YouTube</a>
              </li>
            </ul>
          </div>
        </div>
        <div id="footerBottomR">
          
        </div>
      </div>
    </div>
'

-- French
EXEC AddContent @Group, @CultFr, @Collection, 'Footer.Paralympics.Html',
'
	<div id="lcg-footer" class="footer">
      <h2 class="hidden">
        Menu pied de page
      </h2>
      <div id="footerTop">
        <div id="colsWrap">
          <div class="colLinks" id="quickLinksList">
            <span class="bottomLinks footAbout"><span class="ico"> </span>
				<a href="http://fr.london2012.com/fr/about-us/index.html">À propos de nous</a></span><span class="bottomLinks">Liens rapides</span>
            <div class="nav">
              <ul class="wide">
                <li>
                  <a href="http://fr.london2012.com/fr/contact-us">Nous contacter</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/media-centre">Centre de presse</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/business">Pour les entreprises</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/local-residents">Pour les riverains</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/about-us/npcs">Pour les CNP</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/about-us/publications">Publications</a>
                </li>
                <li>
                  <a class="external" href="http://m.london2012.com/paralympics">Voir le site mobile</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/mobileapps/index.html">Applications mobiles</a>
                </li>
              </ul>
            </div>
          </div>
          <div class="colLinks" id="sportsList">
            <span class="bottomLinks">Sports</span>
            <div class="nav">
              <ul>
                <li>
                  <a href="http://fr.london2012.com/fr/archery/index.html">Tir à l''Arc</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/athletics/index.html">Athlétisme</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/boccia/index.html">Boccia</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/cycling-road/index.html">Cyclisme sur route</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/cycling-track/index.html">Cyclisme sur piste</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/equestrian/index.html">Sports Équestres</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/football-5-a-side/index.html">Football à cinq</a>
                </li>
              </ul>
              <ul>
                <li>
                  <a href="http://fr.london2012.com/fr/football-7-a-side/index.html">Football à sept</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/goalball/index.html">Goalball</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/judo/index.html">Judo</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/powerlifting/index.html">Powerlifting</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/rowing/index.html">Aviron</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/sailing/index.html">Voile</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/shooting/index.html">Tir</a>
                </li>
              </ul>
              <ul class="last">
                <li>
                  <a href="http://fr.london2012.com/fr/swimming/index.html">Natation</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/table-tennis/index.html">Tennis de Table</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/sitting-volleyball/index.html">Volleyball assis</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/wheelchair-basketball/index.html">Basketball en fauteuil</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/wheelchair-fencing/index.html">Escrime en fauteuil</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/wheelchair-rugby/index.html">Rugby en fauteuil</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/wheelchair-tennis/index.html">Tennis en fauteuil</a>
                </li>
              </ul>
            </div>
          </div>
          <div class="colLinks" id="otherLondonSites">
            <span class="bottomLinks">Autres sites de Londres 2012 (en anglais)</span>
            <div class="nav">
              <ul>
                <li>
                  <a class="external" href="http://www.tickets.london2012.com">Billets</a>
                </li>
                <li>
                  <a class="external" href="http://shop.london2012.com/on/demandware.store/Sites-ldn-Site/default/Default-Start?cm_mmc=LOCOG-_-website-_-carousel-_-homepage">Boutique</a>
                </li>
                <li>
                  <a class="external" href="http://getset.london2012.com/en/home">Get Set</a>
                </li>
                <li>
                  <a class="external" href="http://youngleaders.london2012.com/young-leaders/">Jeunes responsables</a>
                </li>
                <li>
                  <a class="external" href="http://festival.london2012.com/">Festival de Londres 2012</a>
                </li>
                <li>
                  <a class="external" href="https://mascot-games.london2012.com/">Mascottes</a>
                </li>
                <li>
                  <a class="external" href="http://www.londonpreparesseries.com/">Séries London Prepares</a>
                </li>
                <li>
                  <a class="external" href="https://volunteer.london2012.com/ESIREG/jsp/_login.jsp">Espace Games Maker</a>
                </li>
                <li class="ldn-oly">
                  <a href="http://fr.london2012.com/">Jeux Olympiques</a>
                </li>
              </ul>
            </div>
          </div>
          <div class="colLinks" id="usingThisSite">
            <span class="bottomLinks">Utilisation du site</span>
            <div class="nav">
              <ul>
                <li>
                  <a href="http://fr.london2012.com/fr/sitemap">Plan du site</a>
                </li>
                <li>
                  <a class="external" href="http://ask.london2012.com">Poser une question</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/accessibility-statement">Conditions d''accès</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/stay-safe-online">Surfer en toute sécurité</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/freedom-of-information">Liberté d''information</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/using-this-site">Utilisation du site</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/privacy-policy">Politique de confidentialité</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/cookies-policy">Cookies</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/copyright">Copyright</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/terms-of-use">Conditions d''utilisation</a>
                </li>
                <li>
                  <a href="http://fr.london2012.com/fr/spectators/tickets/ticket-checker/index.html">Vérificateur du site de billetterie</a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      <div id="footerBottom">
        <div id="footerBottomL">
          <p class="footerPayOff">
            Site officiel de Londres 2012
          </p>
          <div class="footerSocial">
            <span>Suivez-nous sur:</span>
            <ul>
              <li>
                <a href="http://twitter.com/London2012" class="external">Twitter</a>
              </li>
              <li>
                <a href="http://www.facebook.com/London2012" class="external">Facebook</a>
              </li>
              <li>
                <a href="http://www.youtube.com/london2012" class="external">YouTube</a>
              </li>
            </ul>
          </div>
        </div>
        <div id="footerBottomR">
          
        </div>
      </div>
    </div>
'


GO