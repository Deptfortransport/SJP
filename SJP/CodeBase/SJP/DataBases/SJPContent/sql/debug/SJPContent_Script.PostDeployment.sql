/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- =============================================
-- Script Template
-- =============================================


GRANT EXECUTE ON [dbo].[GetContent]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetChangeTable]	TO [SJP_User]

GO

-- =============================================
-- Script Template
-- =============================================


USE SJPContent
Go


EXEC AddChangeNotificationTable 'Content'
EXEC AddChangeNotificationTable 'ContentGroup'
EXEC AddChangeNotificationTable 'ContentOverride'

GO

-- =============================================
-- Script Template
-- =============================================

USE SJPContent
Go

-- If declaring a new Group, also add definition in the code file ResourceManager.SJPResourceManager.cs

EXEC AddGroup 'General'
EXEC AddGroup 'Sitemap'
EXEC AddGroup 'HeaderFooter'
EXEC AddGroup 'JourneyOutput'
EXEC AddGroup 'Analytics'
EXEC AddGroup 'Mobile'

GO

-- =============================================
-- Content script to add resource data
-- =============================================

------------------------------------------------
-- General content, all added to the group 'General'
------------------------------------------------

DECLARE @Group varchar(100) = 'General'
DECLARE @Collection varchar(100) = 'General'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

EXEC AddContent @Group, @CultEn, @Collection, 'Heading.Text', 'Spectator journey planner'
	EXEC AddContent @Group, @CultFr, @Collection, 'Heading.Text', 'Outil de planification de trajet des spectateurs'

-- Header controls
EXEC AddContent @Group, @CultEn, @Collection, 'Header.SkipTo.Link.Text', 'Skip to content'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.SkipTo.Link.Text', 'Aller au sommaire'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Login.Link.Text', 'Login / Sign up'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Login.Link.Text', 'Connexion / S''inscrire'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Language.Link.Text', 'Français'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Language.Link.Text', 'English'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleDyslexia.Text', 'A'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleDyslexia.ToolTip', 'Switch to Dyslexia Style Sheet'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleDyslexia.Hidden.Text', 'Dyslexic Style Sheet'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleHighVis.Text', 'A'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleHighVis.ToolTip', 'Switch to High Visibility Style Sheet'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleHighVis.Hidden.Text', 'High Visibility Style Sheet'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleNormal.Text', 'A'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleNormal.ToolTip', 'Switch to Normal Style Sheet'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.AccessibleNormal.Hidden.Text', 'Normal Style Sheet'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontLarge.Text', '[A]'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontLarge.ToolTip', 'Switch to Largest text'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontLarge.Hidden.Text', 'Largest text'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontMedium.Text', '[A]'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontMedium.ToolTip', 'Switch to Larger text'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontMedium.Hidden.Text', 'Larger text'

EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontSmall.Text', '[A]'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontSmall.ToolTip', 'Switch to Normal text'
EXEC AddContent @Group, @CultEn, @Collection, 'Header.Style.Link.FontSmall.Hidden.Text', 'Normal text'

	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleDyslexia.Text', 'A'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleDyslexia.ToolTip', 'Passer à la feuille de style pour dyslexiques'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleDyslexia.Hidden.Text', 'Feuille de style pour les dyslexiques'

	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleHighVis.Text', 'A'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleHighVis.ToolTip', 'Passer à la feuille de style pour déficients visuels'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleHighVis.Hidden.Text', 'Feuille de style pour les déficients visuels'

	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleNormal.Text', 'A'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleNormal.ToolTip', 'Passer à la feuille de style Normal'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.AccessibleNormal.Hidden.Text', 'Feuille de style Normal'

	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontLarge.Text', '[A]'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontLarge.ToolTip', 'Passer au texte le plus grand'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontLarge.Hidden.Text', 'Texte le plus grand'

	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontMedium.Text', '[A]'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontMedium.ToolTip', 'Passer à un texte plus grand'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontMedium.Hidden.Text', 'Texte plus grand'

	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontSmall.Text', '[A]'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontSmall.ToolTip', 'Passer au texte normal'
	EXEC AddContent @Group, @CultFr, @Collection, 'Header.Style.Link.FontSmall.Hidden.Text', 'Texte normal'

EXEC AddContent @Group, @CultEn, @Collection, 'BreadcrumbControl.lblBreadcrumbTitle.Text', 'You are in:'
	EXEC AddContent @Group, @CultFr, @Collection, 'BreadcrumbControl.lblBreadcrumbTitle.Text', 'Vous êtes dans:'

-- Side bar control
EXEC AddContent @Group, @CultEn, @Collection, 'SideBarLeftControl.Logo.ImageUrl','logos/logo-para.png'
EXEC AddContent @Group, @CultEn, @Collection, 'SideBarLeftControl.Logo.AlternateText','LOCOG logo'

-- Journey Input Page
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.Heading.Text', 'Plan my journey'
EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.Heading.Text', 'Planifier mon trajet'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.locationLabel.Text', 'Enter your start location into the ''From'' box to generate a list of matches and then select from the drop-down menu or enter your postcode.<br /><br /><strong>Please enter the time you would like to get to your venue, taking into account <a href="http://www.london2012.com/Paralympics/spectators/venues">recommended arrival times.</a> Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.</strong>'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.locationLabel.travelBetweenVenues.Text', 'Select the venues you are travelling between and choose your date(s) of travel.<br /><br /><strong>Please enter the time you would like to get to your venue, taking into account <a href="http://www.london2012.com/Paralympics/spectators/venues">recommended arrival times.</a> Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.</strong>'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.locationLabel.travelFromVenue.Text', 'Enter your end location into the ''To'' box to generate a list of matches and then select from the drop-down menu or enter your postcode.<br /><br /><strong>Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.</strong>'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.ValidationError.Text', 'There is a problem with your travel choices'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.NoAccessibleVenueError.Text','Sorry no journeys that meet your accessibility options are available on this date'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.travelBetweenVenues.Text','Are you travelling between two venues?  Plan your journey here.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.travelBetweenVenues.StartAgain.Text','If you are not travelling between two venues, plan your journey here.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.travelFromVenue.Text','Just need to plan a journey away from a venue?  Plan your journey here.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.travelFromVenue.StartAgain.Text','If you are not just travelling away from a venue, plan your journey here.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.fromLocationLabel.Text','From'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.toLocationLabel.Text','To'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.LocationFromInformation.ImageUrl','presentation/information.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.LocationFromInformation.AlternateText','Select location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.LocationFromInformation.ToolTip','Type your location here. You can use locations such as towns, areas, stations or postcodes. Remember to select a location from the drop-down list. Choose your nearest accessible station for the best results when planning an accessible journey.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.LocationToInformation.ImageUrl','presentation/information.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.LocationToInformation.AlternateText','Select venue'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.LocationToInformation.ToolTip','Select a venue from the drop-down list.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ImageUrl','arrows/Up_down.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.AlternateText','Switch locations'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.ToVenue','Plan a journey from a venue to another destination'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.FromVenue','Plan a journey to a venue from your chosen origin'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.VenueToVenue','Switch your venues'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerInput.MallMessage.Text', 'If you are travelling to The Mall then you must select the correct entrance (North/South), as shown on your ticket. You will not be able to cross the route on the day of your event.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.Heading.Text', 'Planifier mon trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.locationLabel.Text', 'Entrez votre point de départ dans la case ''Départ'' pour générer une liste de correspondances, puis sélectionnez dans le menu déroulant ou entrez le code postal.<br /><br /><strong>Veuillez indiquer l''horaire auquel vous souhaitez arriver sur le site, en tenant compte <a href="http://fr.london2012.com/fr/spectators/venues">des horaires d''arrivée recommandés.</a> N''oubliez pas - il y aura une forte affluence  dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.locationLabel.travelBetweenVenues.Text', 'Sélectionnez les sites entre lesquels vous allez circuler ainsi que la/les date(s) de vos déplacements.<br /><br /><strong>Veuillez indiquer l''horaire auquel vous souhaitez arriver sur le site, en tenant compte <a href="http://fr.london2012.com/fr/spectators/venues">des horaires d''arrivée recommandés.</a> N''oubliez pas - il y aura une forte affluence  dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.locationLabel.travelFromVenue.Text', 'Entrez votre lieu d''arrivée dans la boite ''Vers'' pour générer une liste de choix à sélectionner dans un menu déroulant ou entrez votre code postal.<br /><br /><strong>Votre plan de trajet comprendra un temps supplémentaire pour tenir compte des contrôles de sécurité de type aéroport sur les sites et de retards imprévus sur le réseau de transports.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.ValidationError.Text', 'Il ya un problème avec votre choix de voyage'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.NoAccessibleVenueError.Text','Désolé, pas de voyages qui répondent à vos options d''accessibilité sont disponibles à cette date'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.travelBetweenVenues.Text','Vous vous rendez d''un site à l''autre? Planifiez votre trajet ici.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.travelBetweenVenues.StartAgain.Text','Si vous n''envisagez pas de circuler entre deux sites, planifiez votre trajet ici.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.fromLocationLabel.Text','Départ'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.toLocationLabel.Text','Arrivée'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.LocationFromInformation.AlternateText','Sélectionnez Démarrer Lieu'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.LocationFromInformation.ToolTip','Tapez votre point ici. Vous pouvez utiliser des noms de villes, régions, gares ou des codes postaux. Pensez à sélectionner un lieu dans le menu déroulant. Choisissez votre station/gare accessible la plus proche pour une planification optimale de votre voyage.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.LocationToInformation.AlternateText','Sélectionnez Lieu'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.LocationToInformation.ToolTip','Sélectionnez un site dans le menu déroulant.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.ToVenue','Planifier un trajet d''un site vers une autre destination'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.FromVenue','Planifier un trajet vers un site depuis votre lieu de d''origine'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.VenueToVenue','Échangez les sites'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerInput.MallMessage.Text', 'Si vous voyagez en direction du Mall, vous devez sélectionner l''entrée correspondante (Nord/Sud), telle qu''indiquée sur votre billet. Vous ne pourrez pas traverser la route le jour de l''évènement.'

-- EventDate Control
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.outwardDateLabel.Text','Date'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.returnDateLabel.Text','Date'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.OutwardInformation.ImageUrl','presentation/information.png'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.OutwardInformation.AlternateText','Outward date and time'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.OutwardInformation.ToolTip','Please enter the time you would like to get to your venue, taking into account <a href="http://www.london2012.com/Paralympics/spectators/venues"><strong>recommended arrival times</strong>.</a> Remember the transport network will be busy during the Games and you may wish to allow for this.'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.ReturnInformation.ImageUrl','presentation/information.png'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.ReturnInformation.AlternateText','Return date and time'
EXEC AddContent @Group, @CultEn, @Collection,'EventDateControl.ReturnInformation.ToolTip','Please enter the time you wish to leave your venue in to the journey planner. Remember the transport network will be busy during the Games and you may wish to allow for this.'

	EXEC AddContent @Group, @CultFr, @Collection,'EventDateControl.outwardDateLabel.Text','Date'
	EXEC AddContent @Group, @CultFr, @Collection,'EventDateControl.returnDateLabel.Text','Date'
	EXEC AddContent @Group, @CultFr, @Collection,'EventDateControl.OutwardInformation.ToolTip','Veuillez indiquer l''horaire auquel vous souhaitez arriver sur le site, en tenant compte <a href="http://fr.london2012.com/fr/spectators/venues"><strong>des horaires d''arrivée recommandés</strong>.</a> Souvenez-vous que les transports en commun seront très sollicités pendant les Jeux, préparez votre emploi du temps en conséquence.'
	EXEC AddContent @Group, @CultFr, @Collection,'EventDateControl.ReturnInformation.ToolTip','Veuillez indiquer l''horaire auquel vous souhaitez quitter le site. Souvenez-vous que les transports en commun seront très sollicités pendant les Jeux et planifiez vos horaires en conséquence.'

EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.arriveTimeLabel.Text','Arrive at'
EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.leaveTimeLabel.Text','Leave at'
EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.isReturnJourney.Text','Return journey'
EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.outwardToggle.Text','Different outward date?'
EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.returnToggle.Text','Different return date?'

	EXEC AddContent @Group, @CultFr, @Collection,'EventCalendar.arriveTimeLabel.Text','Arriver à'
	EXEC AddContent @Group, @CultFr, @Collection,'EventCalendar.leaveTimeLabel.Text','Partir à'
	EXEC AddContent @Group, @CultFr, @Collection,'EventCalendar.isReturnJourney.Text','Voyage de retour'

-- Calendar Control
EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.monthHeaderLink.CssClass','current'
EXEC AddContent @Group, @CultEn, @Collection,'EventCalendar.monthHeader.Button.CssClass','monthTab'

-- Calendar Month Control
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Sunday','S'
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Monday','M'
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Tuesday','T'
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Wednesday','W'
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Thursday','T'
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Friday','F'
EXEC AddContent @Group, @CultEn, @Collection,'CalendarMonth.Saturday','S'

-- JourneyOptionsTabContainer
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.JourneyOptions.Text','Use the tabs below to choose your preferred travel options (either public transport, river services, park-and-ride, Blue Badge or cycling) and then plan your journey.'
	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.JourneyOptions.Text','Utilisez les onglets ci-dessous pour choisir votre option de transport préférée (transports en commun, navettes fluviales, parcs relais, Blue Badge ou vélo) puis planifiez votre trajet.'
	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.publiJourney.Text','Public transport'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.riverServices.Text','River services'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.parkAndRide.Text','Park-and-ride'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.blueBadge.Text','Blue Badge'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.cycle.Text','Cycle'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.publiJourney.Text','Transport en commun'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.riverServices.Text','Navettes fluviales'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.parkAndRide.Text','Parc relais'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.blueBadge.Text','Blue Badge'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.cycle.Text','Vélo'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.publiJourney.ToolTip','Public transport'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.riverServices.ToolTip','River services'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.parkAndRide.ToolTip','Park-and-ride'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.blueBadge.ToolTip','Blue Badge'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.cycle.ToolTip','Cycle'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.publiJourney.ToolTip','Transport en commun'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.riverServices.ToolTip','Navettes fluviales'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.parkAndRide.ToolTip','Parc relais'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.blueBadge.ToolTip','Blue Badge'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.cycle.ToolTip','Vélo'

-- loading image on Journey tab container page
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.Loading.Imageurl', 'presentation/hourglass_small.gif'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.Loading.AlternateText', 'Loading...'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.loadingMessage.Text', 'Please wait...'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.Loading.AlternateText', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.loadingMessage.Text', 'S''il vous plaît attendre...'

-- JourneyOptionsTabContainer -- BlueBadgeOptions
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Disabled.ImageUrl','presentation/JO_bluebadge_unavailable.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Disabled.AlternateText','Blue Badge options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Disabled.ToolTip','Blue Badge options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.ImageUrl','presentation/JO_bluebadge_selected.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.AlternateText','Blue Badge options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.ToolTip','Blue Badge options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.PlanBlueBadge.Text','Next &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.PlanBlueBadge.ToolTip','Next'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.AlternateText','Options Blue Badge'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.ToolTip','Options Blue Badge'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.PlanBlueBadge.Text','Suivant &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.PlanBlueBadge.ToolTip','Suivant'

-- JourneyOptionsTabContainer -- CycleOptions
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.Disabled.ImageUrl','presentation/JO_cycle_unavailable.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.Disabled.AlternateText','Cycle options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.Disabled.ToolTip','Cycle options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.ImageUrl','presentation/JO_cycle_selected.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.AlternateText','Cycle options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.ToolTip','Cycle options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.PlanCycle.Text','Next &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.PlanCycle.ToolTip','Next'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.AlternateText','Options vélo'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.ToolTip','Options vélo'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.PlanCycle.Text','Suivant &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.PlanCycle.ToolTip','Suivant'

-- JourneyOptionsTabContainer -- ParkAndRideOptions
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Disabled.ImageUrl','presentation/JO_parkride_unavailable.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Disabled.AlternateText','Park and ride options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Disabled.ToolTip','Park and ride options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.ImageUrl','presentation/JO_parkride_selected.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.AlternateText','Park and ride options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.ToolTip','Park and ride options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.PlanParkAndRide.Text','Next &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.PlanParkAndRide.ToolTip','Next'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.AlternateText','Options parc relais'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.ToolTip','Options parc relais'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.PlanParkAndRide.Text','Suivant &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.PlanParkAndRide.ToolTip','Suivant'

-- JourneyOptionsTabContainer -- PublicJourneyOptions
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Disabled.ImageUrl','presentation/JO_pt_unavailable.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Disabled.AlternateText','Public transport options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Disabled.ToolTip','Public transport options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ImageUrl','presentation/JO_pt_selected.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AlternateText','Public transport options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ToolTip','Public transport options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.ImageUrl','arrows/right_arrow.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.AlternateText','Further accessibility options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.ToolTip','Click to view accessibility options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.MobilityNeedsLabel.Text','Further accessibility options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.Text','Read more about accessible travel during the Games.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.URL','http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AlternateText','Options transport en commun'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ToolTip','Options transport en commun'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.AlternateText','Autres options d''accessibilité'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.ToolTip','Cliquer pour voir les options d''accessibilité'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.MobilityNeedsLabel.Text','Autres options d''accessibilité'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.Text','En savoir plus sur l’accessibilité des transports pendant les Jeux.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.URL','http://www.london2012.com/fr/spectators/travel/accessible-travel/'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Information.ImageUrl','presentation/information.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Information.AlternateText','Info'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Text','I need a journey that is suitable for a wheelchair user'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Information.ToolTip','This option includes London Buses, recommended National Rail stations as well as step-free London Underground, DLR stations and piers in London. Some London Underground stations are step-free from entrance to platform and some stations will have a step and a gap between the train and the platform. Buses in London are low-level with ramps designed for all customers to get on and off easily. Assistance to board National Rail stations should always be booked in advance.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Text','I need a journey that is level and suitable for a wheelchair user and I will also require staff assistance' 
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Information.ToolTip','Choose this option if you require step-free journeys and staff assistance. This option includes any stations, stops and piers that are step-free and where staff assistance is available. London Buses are not included in this option. Please note you should always book assistance at National Rail stations in advance and only stations where assistance can be guaranteed at Games-time are included. Some London Underground stations will have a step and a gap between the train and the platform and stations are staffed during operating hours with staff available to assist passengers to the platform.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Text','I need a journey with staff assistance at stations, stops and piers'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Information.ToolTip','Choose this option if you require staff assistance on your journey, but not a step-free journey.  This option includes any stations, stops and piers where staff assistance is provided.  London Buses are not included in this option.  Please note you should always book assistance at National Rail stations in advance and only stations where assistance can be guaranteed at Games-time are included. London Underground stations are staffed during operating hours with staff available to assist passengers to the platform.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Text','I do not want to use London Underground'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Information.ToolTip','Choose this option if you wish to avoid using the London Underground.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Text','I do not have any accessibility requirements'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Information.ToolTip','Choose this option if you do not require a step-free journey or assistance. This is the default option.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.FewerInterchanges.Text','Plan my accessible journey with the fewest changes'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Short.Text','Suitable for wheelchair user'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Short.Text','Wheelchair user with staff assistance'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Short.Text','Journey with staff assistance'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Short.Text','Journey without London Underground'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Short.Text','No accessibility requirements'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.FewerInterchanges.Short.Text','Fewest changes'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.PlanPublicJourney.Text','Plan my journey &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.PlanPublicJourney.ToolTip','Plan my journey'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Text','J''ai besoin d''un trajet sans escalier, qui est adapté aux utilisateurs de fauteuil roulant'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Information.ToolTip','Cette option comprend d’une part les bus londoniens et les gares recommandées du National Rail, d’autre part les stations du métro londonien et du DLR, et quais des navettes fluviales à Londres, qui sont de plain pied ou équipés d’ascenseurs. Dans certaines stations du métro londonien vous pourrez vous déplacer de l’entrée jusqu’au quai sans avoir à monter ou descendre de marches, mais dans d’autres il existe une différence de niveau et un  espace entre le métro et le quai.  Les bus londoniens sont à plancher bas et équipés d’une rampe amovible pour permettre à tous les voyageurs de monter et descendre facilement. Si vous avez besoin d’assistance dans les gares du National Rail vous devez la réserver à l’avance.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Text','J''ai besoin d''un trajet sans escalier, qui est adapté aux utilisateurs de fauteuil roulant et comprenant un service d''assistance'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Information.ToolTip','Choisissez cette option si votre trajet ne doit pas comporter de marches et si vous avez besoin d’assistance. Cette option comprend  les gares, stations, arrêts et quais de navettes fluviales qui sont de plain pied ou équipés d’ascenseurs et offrent un service d’assistance. Les bus londoniens ne sont pas inclus dans cette option. Veuillez noter que si vous avez besoin d’assistance dans une gare du National Rail, vous devez la réserver à l’avance, et que cela concerne uniquement les gares où une assistance peut être garantie pendant les Jeux. Dans certaines stations du métro londonien il existe une différence de niveau et un  espace entre le métro et le quai ; des agents sont présents dans les stations et sur les quais aux heures d’ouverture, qui peuvent aider les passagers dans leur déplacement vers les quais.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Text','J''ai besoin d''un trajet incluant une assistance aux gares, arrêts et embarcadères'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Information.ToolTip','Choisissez cette option si vous avez besoin d’assistance au cours de votre trajet, mais pas d’un trajet sans marches. Cette option comprend toutes les gares, stations de métro, arrêts et quais des navettes fluviales qui offrent un service d’assistance. Les bus londoniens ne sont pas inclus dans cette option. Veuillez noter que si vous avez besoin d’assistance dans une gare du National Rail, vous devez la réserver à l’avance, et que cela concerne uniquement les gares où l’assistance peut être garantie pendant les Jeux. Des agents  sont présents dans les stations du métro londonien aux heures d’ouverture, qui peuvent aider les passagers dans leur déplacement vers les quais.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Text','Je ne veux pas utiliser le métro de Londres'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Information.ToolTip','Choisissez cette option si vous souhaitez éviter de prendre le métro.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Text','Je n''ai pas toutes les exigences d''accessibilité'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Information.ToolTip','Choisissez cette option si vous n’avez pas besoin d’un trajet qui ne comporte pas de marches ou que vous n’avez pas besoin d’assistance. Ceci est l’option par défaut.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.FewerInterchanges.Text','Planifier mon trajet accessible avec le moins de correspondances possible'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Short.Text','Convient aux personnes en fauteuil roulant'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Short.Text','Utilisateur de fauteuil roulant avec assistance du personnel'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Short.Text','Trajet avec assistance du personnel'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Short.Text','Trajet sans utilisation du métro'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Short.Text','Pas de besoin spécifique en accessibilité'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.FewerInterchanges.Short.Text','Le moins de correspondances'
	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.PlanPublicJourney.Text','Planifier mon trajet &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.PlanPublicJourney.ToolTip','Planifier mon trajet'

-- JourneyOptionsTabContainer -- RiverServicesOptions
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Disabled.ImageUrl','presentation/JO_river_unavailable.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Disabled.AlternateText','River services options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Disabled.ToolTip','River services options - Unavailable'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.ImageUrl','presentation/JO_river_selected.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.AlternateText','River services options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.ToolTip','River services options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.PlanRiverServices.Text','Next &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.PlanRiverServices.ToolTip','Next'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.AlternateText','Options navette fluviale'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.ToolTip','Options navette fluviale'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.PlanRiverServices.Text','Suivant &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.PlanRiverServices.ToolTip','Suivant'

-- Location Control
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.ambiguityText.Text','Which "{0}" did you mean?'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.ambiguityText.noLocationFound.Text','The location you have entered is not recognised.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.ambiguityText.noLocationUKFound.Text','The travel planner enables you to plan a journey from anywhere in Great Britain to Olympic venues. Please try a Great Britain postcode.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.ambiguityText.noLocationLocalityFound.Text','The travel planner enables you to plan a journey from anywhere in Great Britain to Olympic venues. Please try a Great Britain postcode.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.ambiguityText.invalidPostcode.Text','You have entered an invalid postcode. Please note that postcodes for P.O. boxes are not recognised by the journey planner.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.ambiguityText.chooseVenueLocation.Text','Please choose a venue'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.locationDropdown.DefaultItem.Text','-- Select Location -- '
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.VenueDropdown.DefaultItem.Text',' -- Select venue -- '
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Tooltip','Postcode, Location, Station'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Tooltip.Venue','Choose a venue'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Tooltip.All','Postcode, location, station, or choose a venue'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Discription.Text','Select a location from the autocomplete dropdown that appears as you type in.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Discription.Text.Venue','Select a venue by clicking the venues button.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Discription.Text.All','Select a location from the autocomplete dropdown that appears as you type in or select a venue by clicking the venues button.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Reset.Text','Please enter another location.'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Reset.Ambiguous.Text','Enter another location'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.LocationInput.Reset.Clear.Text','Clear'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.CurrentLocation.Text','My location'
EXEC AddContent @Group, @CultEn, @Collection,'LocationControl.CurrentLocation.Tooltip','My location'

	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.ambiguityText.noLocationFound.Text','Le lieu que vous avez saisi n''est pas reconnu.'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.ambiguityText.noLocationUKFoundText.Text','Le planificateur de trajet vous permet d''organiser un voyage depuis n''importe où en Grande-Bretagne vers les sites olympiques. Veuillez entrer un code postal en Grande-Bretagne.'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.ambiguityText.noLocationLocalityFoundText.Text','Le planificateur de trajet vous permet d''organiser un voyage depuis n''importe où en Grande-Bretagne vers les sites olympiques. Veuillez entrer un code postal en Grande-Bretagne.'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.ambiguityText.invalidPostcode.Text','Vous avez saisi un code postal non valide. Les BP ne sont pas reconnues par l''outil de planification de parcours.'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.ambiguityText.chooseVenueLocation.Text','Choisir un site'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.LocationInput.Tooltip.Venue','Choisir un site'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.LocationInput.Tooltip.All','Code postal, adresse, station ou choisir un site'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.LocationInput.Reset.Text','Veuillez saisir un autre lieu.'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.CurrentLocation.Text','Ma localisation'
	EXEC AddContent @Group, @CultFr, @Collection,'LocationControl.CurrentLocation.Tooltip','Ma localisation'

-- Validate and Run
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DateTimeIsBeforeEvent', 'The spectator journey planner enables journey planning only between 18 July and 14 September 2012. Please select a date in this period.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DateTimeIsAfterEvent',	'The spectator journey planner enables journey planning only between 18 July and 14 September 2012. Please select a date in this period.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DateNotValid', 'The date you have entered is not valid. Please enter a valid date between 18 July and 14 September 2012.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.OutwardDateIsAfterReturnDate', 'The return date or time you have entered is earlier than your departure date or time. <strong>Please enter a later return date or time.</strong>'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DateTimeIsInThePast', 'The spectator journey planner enables journey planning only between 18 July and 14 September 2012. Please select a date in this period.'

	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DateTimeIsBeforeEvent', 'L''outil de planification de parcours spectateur permet seulement de planifier des parcours entre le 18 juillet et le 14 septembre 2012. Veuillez sélectionner une date comprise dans cette période.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DateTimeIsAfterEvent',	'L''outil de planification de parcours spectateur permet seulement de planifier des parcours entre le 18 juillet et le 14 septembre 2012. Veuillez sélectionner une date comprise dans cette période.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DateNotValid', 'La date que vous avez saisie n''est pas valide. Veuillez saisir une date comprise entre le 18 juillet et le 14 septembre 2012.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.OutwardDateIsAfterReturnDate', 'La date ou l''heure de retour que vous avez saisie est antérieure à votre date ou heure de départ. <strong>Veuillez saisir une date ou heure de retour ultérieure.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DateTimeIsInThePast', 'L''outil de planification de parcours spectateur permet seulement de planifier des parcours entre le 18 juillet et le 14 septembre 2012. Veuillez sélectionner une date comprise dans cette période.'

EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.InvalidOrigin', 'The location you have entered is not recognised. <strong>Please enter another location.</strong>'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.InvalidDestination', 'The location you have entered is not recognised. <strong>Please enter another location.</strong>'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.AtleastOneLocationShouleBeVenue', 'The location entered in the ''To'' box must be a <strong>London 2012 venue</strong>. Please select a venue from the <strong>drop-down</strong> list.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.OriginAndDestinationOverlaps', 'The venues you have selected are in the same location. Your best transport option is likely to be a walk between the venues. Please use the map in the right hand menu to help plan your walk.<br /><br />Within some venues, such as the Olympic Park, disabled spectators will be able to make use of Games Mobility. This free service will be easy to find inside the venue and will loan out manual wheelchairs and mobility scooters.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.OriginAndDestinationAreSame', 'The venues you have selected are the same. Please enter a different ''From'' or ''To'' London 2012 Olympic venue.'

	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.InvalidOrigin', 'Le lieu que vous avez saisi n''est pas reconnu. <strong>Veuillez saisir un autre lieu.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.InvalidDestination', 'Le lieu que vous avez saisi n''est pas reconnu. <strong>Veuillez saisir un autre lieu.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.AtleastOneLocationShouleBeVenue', 'Le lieu saisi dans la case ''Arrivée'' doit correspondre à un site de<strong>Londres 2012.</strong> Sélectionnez un site dans le <strong>menu déroulantt</strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.OriginAndDestinationAreSame', 'Les sites que vous avez sélectionnés sont identiques. Veuillez saisir un autre point de "Départ"/ "Arrivée" parmi les sites olympiques de Londres 2012.'

EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.CyclePlannerUnavailableKey', 'Sorry, cycle journey planning is currently unavailable.  Please try again shortly.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.LocationHasNoPoint', 'Cycle planning is not available within the area you have selected. Please choose a different travel from and/or to location.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DistanceBetweenLocationsTooGreat', 'The spectator journey planner can only plan cycle journeys of up to 20km. The journey you requested was further than this; please enter a closer departure point.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DistanceToVenueLocationTooGreat', 'Cycle journeys to {0} can only be planned where the distance is {1}km or less; the journey you requested was longer than this.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DistanceFromVenueLocationTooGreat', 'Cycle journeys from {0} can only be planned where the distance is {1}km or less; the journey you requested was longer than this.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.LocationInInvalidCycleArea', 'At the moment we are able to plan cycle journeys in a limited number of areas. Please amend your choices.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.LocationPointsNotInSameCycleArea', 'To plan a cycle journey the travel from and to locations must either be in the same area or in adjoining areas with a direct connection between them. Please amend your choices.'

	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.CyclePlannerUnavailableKey', 'Désolé, l''outil de planification de parcours est actuellement indisponible. Veuillez réessayer dans quelques minutes.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DistanceToVenueLocationTooGreat', 'L''outil de planification de {0} ne peut planifier que des parcours de {1}km. Le trajet demandé est trop long; veuillez saisir un point de départ plus proche.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DistanceFromVenueLocationTooGreat', 'Les trajets à vélo depuis {0} peuvent uniquement être planifiés lorsque la distance est de {1}km ou moins ; le trajet requis est trop long.'
	
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.ErrorMessage.General1', 'Sorry, an error occurred attempting to plan your journey. This may be due to a technical problem. Please check your travel options and retry planning your journey.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.ErrorMessage.General2', 'However, if you continue to experience difficulties, you may have found an error or omission. Please let us know by using the Contact us facility below, so that we can correct it for future users.'


-- Retailers and RetailerHandoff Page
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Heading.Text', 'Book your travel tickets'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.BookingSummary.Text', 'These are the bookable parts of your journey - select the parts you wish to book now to progress to the booking websites. <br /><br />Spectators with a ticket for a Games event in London will receive a one-day Games Travelcard for the day of that event. This will entitle you to travel within zones 1–9 on the London public transport network throughout the day of your event. Spectators at Eton Dorney, the Lee Valley White Water Centre and Hadleigh Farm will also receive a Games Travelcard for use on public transport in London and to travel by National Rail between London and the recommended stations for those venues. No additional fare will be payable. <a href="http://www.london2012.com/paralympics/spectators/travel/games-travelcard/">Read more information on the Games Travelcard</a>.'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Disclaimer1.Text', 'London2012 is not responsible for any financial transactions on retailer websites.'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Disclaimer2.Text', 'We advise you to read the retailer Terms and conditions and Privacy policy before making your booking.'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Back.Text', '&lt; Back'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Back.ToolTip', 'Back'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Error.NoRetailers.Text', 'No retailers were found for your journey.'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Message.JourneyHandedOff.Text', 'Your journey has been passed on to the tickets retailer site (opened in a new window).'
EXEC AddContent @Group, @CultEn, @Collection,'RetailerHandoff.Heading.Text', 'Book your travel tickets'
EXEC AddContent @Group, @CultEn, @Collection,'RetailerHandOff.JavascriptDisabled.Text', 'If the results do not appear within 30 seconds, please click this link.'
EXEC AddContent @Group, @CultEn, @Collection,'RetailerHandOff.Error.NoRetailers.Text', 'No retailers were found for your journey.'
EXEC AddContent @Group, @CultEn, @Collection,'RetailerHandOff.Error.HandoffXml.Text', 'Sorry, there was a problem passing your journey details to the journey booking website.'

	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Heading.Text', 'Réservez votre voyage'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.BookingSummary.Text', 'Voici les parties à réserver de votre voyage - sélectionnez les parties que vous souhaitez réserver maintenant en allant sur les sites de réservation.<br /><br />Les spectateurs munis d''un billet pour assister à une épreuve des Jeux se déroulant dans Londres recevront une carte de transport Games Travelcard valable pour la journée de cette épreuve. La Games Travelcard vous permettra de voyager gratuitement dans les zones 1 à 9 du réseau des transports en commun londoniens pendant toute la journée de votre épreuve. Les spectateurs se rendant à Eton Dorney, Lee Valley White Water Centre et Hadleigh Farm recevront également une carte de transport Games Travelcard utilisable dans les transports en commun londoniens et pour prendre le train entre Londres et les gares recommandées pour ces sites. Aucun supplément ne vous sera demandé. <a href="http://www.london2012.com/fr/spectators/travel/games-travelcard/">Pour plus d''informations sur la Games Travelcard</a>.'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Disclaimer1.Text', 'Londres 2012 n''est pas responsable des transactions financières sur les sites détaillant.'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Disclaimer2.Text', 'Nous vous conseillons de lire les conditions générales détaillant les conditions et la politique de confidentialité avant de faire votre réservation.'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Back.Text', '&lt; Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Back.ToolTip', 'Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Error.NoRetailers.Text', 'Pas de détaillants ont été trouvés pour votre voyage.'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Message.JourneyHandedOff.Text', 'Votre voyage a été transmis au détaillant de billets sur le site (ouvert dans une nouvelle fenêtre) .'
	EXEC AddContent @Group, @CultFr, @Collection,'RetailerHandOff.JavascriptDisabled.Text', 'Si les résultats n''apparaissent pas dans les 30 prochaines secondes, merci de cliquer sur ce lien.'
	EXEC AddContent @Group, @CultFr, @Collection,'RetailerHandOff.Error.NoRetailers.Text', 'Pas de détaillants ont été trouvés pour votre voyage.'
	EXEC AddContent @Group, @CultFr, @Collection,'RetailerHandoff.Heading.Text', 'Réservez votre voyage'
	EXEC AddContent @Group, @CultFr, @Collection,'RetailerHandOff.Error.HandoffXml.Text', 'Désolé, une erreur s''est produite lors de l''envoi des informations relatives à votre trajet vers le site de réservation'

-- Retailer resources (for Retailers table)
--		Live retailers
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.Image.Path', 'presentation/Retailer_coach.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.Image.AlternateText', 'Coach ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.HandoffButton.Text', 'Buy &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.HandoffButton.ToolTip', 'Buy (opens in new window)'

	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.HandoffButton.Text', 'Acheter &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.HandoffButton.ToolTip', 'Acheter (ouvre une nouvelle fenêtre)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.Image.Path', 'presentation/Retailer_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.Image.AlternateText', 'Ferry ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.HandoffButton.Text', 'Buy &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.HandoffButton.ToolTip', 'Buy (opens in new window)'

	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Ferry.CityCruises.HandoffButton.Text', 'Acheter &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Ferry.CityCruises.HandoffButton.ToolTip', 'Acheter (ouvre une nouvelle fenêtre)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.Image.Path', 'presentation/Retailer_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.Image.AlternateText', 'Ferry ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.HandoffButton.Text', 'Buy &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.HandoffButton.ToolTip', 'Buy (opens in new window)'

	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.HandoffButton.Text', 'Acheter &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.HandoffButton.ToolTip', 'Acheter (ouvre une nouvelle fenêtre)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.Image.Path', 'presentation/Retailer_rail.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.Image.AlternateText', 'Rail ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.HandoffButton.Text', 'Buy &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.HandoffButton.ToolTip', 'Buy (opens in new window)'

	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Rail.NationalRail.HandoffButton.Text', 'Acheter &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'Retailers.Retailer.Rail.NationalRail.HandoffButton.ToolTip', 'Acheter (ouvre une nouvelle fenêtre)'

--		Test retailers
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Coach.Image.Path', 'presentation/Retailer_coach.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Coach.Image.AlternateText', 'Test ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Coach.HandoffButton.Text', 'Show XML &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Coach.HandoffButton.ToolTip', 'Show retailer handoff XML (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Ferry.Image.Path', 'presentation/Retailer_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Ferry.Image.AlternateText', 'Test ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Ferry.HandoffButton.Text', 'Show XML &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Ferry.HandoffButton.ToolTip', 'Show retailer handoff XML (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Rail.Image.Path', 'presentation/Retailer_rail.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Rail.Image.AlternateText', 'Test ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Rail.HandoffButton.Text', 'Show XML &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Test.Rail.HandoffButton.ToolTip', 'Show retailer handoff XML (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.Test.Image.Path', 'presentation/Retailer_coach.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.Test.Image.AlternateText', 'Coach ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.Test.HandoffButton.Text', 'First Group (TEST) &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Coach.DirectManagedTransport.Test.HandoffButton.ToolTip', 'First Group (TEST) (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.Test.Image.Path', 'presentation/Retailer_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.Test.Image.AlternateText', 'Ferry ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.Test.HandoffButton.Text', 'City Cruises (TEST) &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.CityCruises.Test.HandoffButton.ToolTip', 'City Cruises (TEST) (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.Test.Image.Path', 'presentation/Retailer_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.Test.Image.AlternateText', 'Ferry ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.Test.HandoffButton.Text', 'Thames Clipper (TEST) &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Ferry.ThamesClipper.Test.HandoffButton.ToolTip', 'Thames Clipper (TEST) (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.Test.Image.Path', 'presentation/Retailer_rail.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.Test.Image.AlternateText', 'Rail ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.Test.HandoffButton.Text', 'National Rail (TEST) &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.NationalRail.Test.HandoffButton.ToolTip', 'National Rail (TEST) (opens in new window)'

EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.WebTIS.Test.Image.Path', 'presentation/Retailer_rail.png'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.WebTIS.Test.Image.AlternateText', 'Rail ticket retailer'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.WebTIS.Test.HandoffButton.Text', 'WebTIS (TEST) &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'Retailers.Retailer.Rail.WebTIS.Test.HandoffButton.ToolTip', 'WebTIS (TEST) (opens in new window)'

-- Location Control
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Air','Air'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Bus','Bus'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Car','Car'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.CheckIn','CheckIn'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.CheckOut','CheckOut'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Coach','Coach'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Cycle','Cycle'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Drt','Drt'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Ferry','Ferry'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Metro','Metro'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Rail','Train'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.RailReplacementBus','Rail replacement / Bus link'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Taxi','Taxi'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Tram','Tram / Light Rail'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Transfer','Transfer'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Underground','Underground'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Walk','Walk'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.TransitRail','Train'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.TransitShuttleBus','Shuttle Bus'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Queue','Venue queue'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.WalkInterchange','Interchange'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.CableCar','Cable Car'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.EuroTunnel','Euro Tunnel'

	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Air','Avion'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Bus','Bus'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Car','Voiture'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.CheckIn','CheckIn'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.CheckOut','CheckOut'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Coach','Car'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Cycle','Vélo'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Drt','Drt'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Ferry','Ferry'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Metro','Metro'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Rail','Train'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.RailReplacementBus','Train annulé / Bus de remplacement'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Taxi','Taxi'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Tram','Tramway / Métro'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Transfer','Correspondance'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Underground','Métro'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Walk','Marche'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.TransitRail','Navette'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.TransitShuttleBus','Navette'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.Queue','File d''attente pour le site'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.WalkInterchange','Correspondance'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.CableCar','Téléphérique'
	EXEC AddContent @Group, @CultFr, @Collection,'TransportMode.EuroTunnel','Tunnel sous la Manche'

-- Transport Mode Image Urls
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Air.ImageUrl','presentation/jp_air.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Bus.ImageUrl','presentation/jp_bus.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Car.ImageUrl','presentation/jp_car.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.CheckIn.ImageUrl','presentation/jp_checkin.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.CheckOut.ImageUrl','presentation/jp_checkout.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Coach.ImageUrl','presentation/jp_coach.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Cycle.ImageUrl','presentation/jp_cycle.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Drt.ImageUrl','presentation/jp_drt.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Ferry.ImageUrl','presentation/jp_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Metro.ImageUrl','presentation/jp_metro.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Rail.ImageUrl','presentation/jp_rail.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.RailReplacementBus.ImageUrl','presentation/jp_rail_replacement_bus.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Taxi.ImageUrl','presentation/jp_taxi.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Tram.ImageUrl','presentation/jp_tram.png'
--EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Transfer.ImageUrl','presentation/jp_transfer.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Underground.ImageUrl','presentation/jp_underground.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Walk.ImageUrl','presentation/jp_walk.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.TransitRail.ImageUrl','presentation/jp_rail.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.TransitShuttleBus.ImageUrl','presentation/jp_bus.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.Queue.ImageUrl','presentation/jp_queue.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.WalkInterchange.ImageUrl','presentation/jp_walk_interchange.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.CableCar.ImageUrl','presentation/jp_cablecar.png'
EXEC AddContent @Group, @CultEn, @Collection,'TransportMode.EuroTunnel.ImageUrl','presentation/jp_eurotunnel.png'

-- Journey Options Page
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Heading.Text', 'Journey options'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.JourneyInfo1.Text', 'Your journey options are listed below. Click a route option to reveal the journey details. Alternatively, you can select the most suitable journey and then click on <strong>''Book travel''</strong> to progress to the bookable parts of your journey. Please note that not all journeys can be booked in advance and remember that some parts of your journey may be covered by your <a href="http://www.london2012.com/paralympics/spectators/travel/games-travelcard/">Games Travelcard</a>.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.JourneyInfo2.Text', 'My journey takes a lot longer than I would have expected.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.JourneyInfoFAQLink.Text', 'Read the FAQs to find out why.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.JourneyInfoFAQLink.ToolTip', 'Read the FAQs to find out why my journey takes a lot longer than I would have expected'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.JourneyInfoFAQLink.Url', 'http://www.london2012.com/paralympics/spectators/travel/travel-faqs/index.html#Z'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.Heading.Text', 'Options de trajet'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.JourneyInfo1.Text', 'Vos options de trajet sont listées ci-dessous. Cliquez sur un itinéraire pour afficher tous les détails. Vous pouvez aussi sélectionner le trajet le plus adapté, puis cliquer sur <strong>''Effectuer une réservation''</strong>pour passer aux parties à réserver de votre voyage. Veuillez noter qu''il n''est pas possible de réserver tous les trajets à l''avance et rappelez-vous que certaines parties de votre voyage peuvent être couvertes par votre carte de transport <a href="http://www.london2012.com/fr/spectators/travel/games-travelcard/">Games Travelcard</a>.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.JourneyInfo2.Text', 'Mon trajet ne suit pas l''itinéraire que j''avais prévu.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.JourneyInfoFAQLink.Text', 'Pourquoi?'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.JourneyInfoFAQLink.ToolTip', 'Mon trajet ne suit pas l''itinéraire que j''avais prévu. Pourquoi?'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.JourneyInfoFAQLink.Url', 'http://www.london2012.com/fr/spectators/travel/travel-faqs/index.html#Z'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.PrinterFriendly.Text', 'Printer friendly'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.PrinterFriendly.ToolTip', 'Open a new window with a printer friendly version of the journey'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Back.Text', '&lt; Back'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Back.ToolTip', 'Back'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Tickets.Text','Book travel &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Tickets.ToolTip','Book travel'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.BookTicketsInfo.AlternateText','Book travel info'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.BookTicketsInfo.ToolTip','Select ''Book travel'' once you have selected your journey. You may need to book with different travel operators, in which case you will be taken to a summary page. Some journeys will be covered by your Games Travelcard and do not need to be booked.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.BookTicketsInfo.ImageUrl','presentation/information.png'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.PrinterFriendly.Text', 'Version imprimable'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.PrinterFriendly.ToolTip', 'Ouvrir une nouvelle fenêtre avec une version imprimable de votre trajet'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.Back.Text', '&lt; Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.Back.ToolTip', 'Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.Tickets.Text','Réserver un trajet &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.Tickets.ToolTip','Réserver un trajet'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.BookTicketsInfo.ToolTip','Sélectionnez ''Effectuer une réservation'' dès que vous avez sélectionné votre trajet. Vous devrez peut-être réserver auprès de différentes sociétés de transport, auquel cas vous serez orienté vers une page de synthèse. Certains trajets seront couverts par votre carte de transport Games Travelcard et ne nécessiteront donc pas de réservation.'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Loading.Imageurl', 'presentation/hourglass_medium.gif'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.Loading.AlternateText', 'Loading...'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.loadingMessage.Text', 'Please wait while we prepare your journey plan'
	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.Loading.AlternateText', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.loadingMessage.Text', 'Veuillez patienter pendant que nous élaborons votre trajet'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.LongWaitMessage.Text', 'If the results do not appear within 30 seconds, '
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.LongWaitMessageLink.Text', 'please click this link.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.LongWaitMessageLink.ToolTip', 'If the results do not appear within 30 seconds, please click this link.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOptions.LongWaitMessage.Text', 'Si les résultats n''apparaissent pas dans les 30 prochaines secondes, '
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOptions.LongWaitMessageLink.Text', 'merci de cliquer sur ce lien.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOptions.LongWaitMessageLink.ToolTip', 'Si les résultats n''apparaissent pas dans les 30 prochaines secondes, merci de cliquer sur ce lien'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.NoResultsFound.Error', 'An error occured attempting to obtain journey options using the details you have entered.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptions.NoResultsFound.UserInfo', 'Please click ''Back'' to change journey options and replan the journey.'
	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.NoResultsFound.Error', 'Une erreur s''est produite de tenter d''obtenir les options chemin en utilisant les coordonnées que vous avez entré.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptions.NoResultsFound.UserInfo', 'Veuillez cliquer sur "Retour" pour changer vos options de trajet et replanifier votre trajet.'
	
-- Error Page
EXEC AddContent @Group, @CultEn, @Collection,'Error.Heading.Text', 'Sorry, an error has occurred.'
EXEC AddContent @Group, @CultEn, @Collection,'Error.Message1.Text', 'This may be due to a technical problem which we will do our best to resolve.'
EXEC AddContent @Group, @CultEn, @Collection,'Error.Message2.Text', 'Please <a class="error" href="{0}">select this link</a> to return to the spectator journey planner and try again.'

	EXEC AddContent @Group, @CultFr, @Collection,'Error.Heading.Text', 'Désolé, une erreur s''est produite.'
	EXEC AddContent @Group, @CultFr, @Collection,'Error.Message1.Text', ' Il s''agit apparemment d''un problème technique et nous mettons tout en œuvre pour résoudre ce désagrément.'
	EXEC AddContent @Group, @CultFr, @Collection,'Error.Message2.Text', ' Veuillez <a class="error" href="{0}"> cliquer sur ce lien</a> pour retourner à l''outil de planification de parcours spectateur et réessayer.'

-- Sorry Page
EXEC AddContent @Group, @CultEn, @Collection,'Sorry.Heading.Text', 'Sorry, the spectator journey planner is unexpectedly busy at this time. You may wish to return later to plan your journey.'
EXEC AddContent @Group, @CultEn, @Collection,'Sorry.Message1.Text', ''
EXEC AddContent @Group, @CultEn, @Collection,'Sorry.Message2.Text', ''
EXEC AddContent @Group, @CultEn, @Collection,'Sorry.Message3.Text', ''

	EXEC AddContent @Group, @CultFr, @Collection,'Sorry.Heading.Text', 'Désolé, l''accès à l''outil de planification de parcours spectateur est en ce moment très encombré. Réessayez plus tard pour planifier votre trajet.'

-- PageNotFound Page
EXEC AddContent @Group, @CultEn, @Collection,'PageNotFound.Heading.Text', 'Sorry, the page you have requested has not been found.'
EXEC AddContent @Group, @CultEn, @Collection,'PageNotFound.Message1.Text', '<br />Please try the following options:'
EXEC AddContent @Group, @CultEn, @Collection,'PageNotFound.Message2.Text', '<a class="info" href="{0}">Go to the London 2012 homepage</a> or <br /><a class="info" href="{1}">use our site map</a>'

	EXEC AddContent @Group, @CultFr, @Collection,'PageNotFound.Heading.Text', 'Désolé, la page que vous avez demandée est introuvable. '
	EXEC AddContent @Group, @CultFr, @Collection,'PageNotFound.Message1.Text', '<br />Veuillez essayer les options suivantes :'
	EXEC AddContent @Group, @CultFr, @Collection,'PageNotFound.Message2.Text', '<a class="info" href="{0}">Allez sur la page d''accueil de Londres 2012</a> ou <br /><a class="info" href="{1}">utilisez notre carte du site</a>'

-- Session Timeout
EXEC AddContent @Group, @CultEn, @Collection,'SessionTimeout.Message.Text', 'Your session has expired. Please check your travel choices and plan your journey again.'
	EXEC AddContent @Group, @CultFr, @Collection,'SessionTimeout.Message.Text', 'Votre session a expiré. Veuillez vérifier vos choix de transport et planifier à nouveau votre trajet.'

-- Landing Page
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.CheckTravelOptions.Text', 'Please check your travel options before planning your journey.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidLocations.Text', 'At least one location entered must be a <strong>London 2012 venue</strong>. Please select a venue from the <strong>drop-down</strong> list.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidDestination.Text', 'The location entered in the ''To'' box must be a <strong>London 2012 venue</strong>. Please select a venue from the <strong>drop-down</strong> list.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidOrigin.Text', 'The location entered in the ''From'' box must be a <strong>London 2012 venue</strong>. Please select a venue from the <strong>drop-down</strong> list.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidDate.Text', 'The date or time you have entered is not valid.  Please enter a valid date and time between 18 July and 14 September 2012.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidDateInPast.Text', 'The date or time you have entered is not valid. Please enter a valid date and time between 18 July and 14 September 2012.'

	EXEC AddContent @Group, @CultFr, @Collection,'Landing.Message.InvalidLocations.Text', 'Au moins un des lieux entrés doit être un <strong>site de Londres 2012</strong>. Veuillez sélectionner un site dans le <strong>menu déroulant</strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'Landing.Message.InvalidDestination.Text', 'Le lieu saisi dans la case ''Arrivée'' doit correspondre à un site de <strong>Londres 2012</strong>. Sélectionnez un site dans le <strong>menu déroulantt</strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'Landing.Message.InvalidDate.Text', 'La date ou l''heure que vous avez saisie n''est pas valide. Veuillez saisir une date et heure comprise entre le 18 juillet et le 14 septembre 2012.'
	EXEC AddContent @Group, @CultFr, @Collection,'Landing.Message.InvalidDateInPast.Text', 'La date ou l''heure que vous avez saisie n''est pas valide. Veuillez saisir une date et heure comprise entre le 18 juillet et le 14 septembre 2012.'

-- JourneyLocations Page
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.Heading.ParkAndRide.Text', 'Park and Ride car parks'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.Heading.BlueBadge.Text', 'Blue Badge car parks'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.Heading.Cycle.Text', 'Cycle parks'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.Heading.RiverServices.Text', 'Choose a river service'
	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.Heading.ParkAndRide.Text', 'Parc relais'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.Heading.BlueBadge.Text', 'Parkinks Blue Badge'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.Heading.Cycle.Text', 'Parc à vélo'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.Heading.RiverServices.Text', 'Choisir une navette fluviale'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.Back.Text', '&lt; Back'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.Back.ToolTip', 'Back'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.PlanJourney.Text', 'Plan my journey &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.PlanJourney.ToolTip', 'Plan my journey'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.Back.Text', '&lt; Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.Back.ToolTip', 'Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.PlanJourney.Text', 'Planifier mon trajet &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.PlanJourney.ToolTip', 'Planifier mon trajet'

-- CycleJourneyLocations Control
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.UseTheMap.Text','Use the map to help you choose the cycle parking location that suits you best.'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.PreferredParksHeading.Text', 'Choose preferred cycle park'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.PreferredParksInfo.Text','Choose your cycle parking location and the type of route before selecting <strong>''Plan my journey''</strong>. The venue is a short distance from the cycle parking location. <a href="{0}" class="viewMapLink">View a map of {1}</a>.'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.PreferredParksInfo.NoMapLink.Text','Choose your cycle parking location and the type of route before selecting <strong>''Plan my journey''</strong>. The venue is a short distance from the cycle parking location.'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.TypeOfRouteHeading.Text', 'Choose your type of route'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.CycleParkNoneFound.Text', 'No cycle parking is available on your chosen date or time for {0}. Cycle parking is typically open a few hours either side of event start and finish times.<br /><br /><strong>Please ensure you choose a date or time when a Games event is occurring at this venue.</strong>'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyLocations.CycleReturnDateDifferent.Text', 'The return date you have entered is different to the date of your outward journey. Cycle parks are only available for parking for a single day. Please select new date(s).'

	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.UseTheMap.Text','Utilisez la carte pour vous aider à localiser le parc à vélos qui vous convient le mieux.'
	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.PreferredParksHeading.Text', '	Choisissez le parc cycle préféré'
	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.PreferredParksInfo.Text','Choisissez le parc à vélos et le type d''itinéraire avant de sélectionner <strong>''Planifier mon voyage''</strong>. Le site se trouve à proximité du parc à vélos. <a href="{0}" class="viewMapLink">Voir une carte de {1}</a>.'
	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.PreferredParksInfo.NoMapLink.Text','Choisissez le parc à vélos et le type d''itinéraire avant de sélectionner <strong>''Planifier mon voyage''</strong>. Le site se trouve à proximité du parc à vélos.'
	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.TypeOfRouteHeading.Text', 'Choisissez votre type d''itinéraire'
	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.CycleParkNoneFound.Text', 'Aucun stationnement pour vélos n''est disponible à la date ou l''heure choisie pour {0}. En général, le stationnement pour vélos se limite à quelques heures, correspondant à la durée de l''événement sportif.<br /><br /><strong> Veuillez choisir une autre date ou heure.</strong>'
	EXEC AddContent @Group, @CultFr, @Collection,'CycleJourneyLocations.CycleReturnDateDifferent.Text', 'La date de retour que vous avez entré est différente de la date de votre voyage aller. Parcs du cycle ne sont disponibles que pour le stationnement pour une seule journée. S''il vous plaît choisir la date de nouvelles (s).'

-- RiverServices Journey Locations Control
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.UseTheMap.Text', 'Use the map to help you choose your best route to {0}'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.RouteSelection.To.Text', 'Choose your route and select <strong>''Find departures times''</strong> to see a list of services operating to the venue.'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.RouteSelection.From.Text', 'Choose your route and select <strong>''Find departures times''</strong> to see a list of services operating from the venue.'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.BtnFindDepartureTimes.Text', 'Find departure times'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.BtnFindDepartureTimes.ToolTip', 'Find departure times'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.RouteSelectionOptions.Option.Text', '{0} to {1}'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.RiverServiceNoneFound.Text', 'There are no river services for your venue on the requested date of travel. Please plan your journey using other public transport or select a new date.'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.RiverServiceResultsHeading.Text', 'Choose the journey that is most convenient for you and select <strong>''Plan my journey''</strong> to view that river service as part of your journey. The venue will be a short distance from the arrival pier. Extra time has been allowed to get to the venue from the arrival pier and to enter the venue. You may wish to allow even more time in your journey plan.'

	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.UseTheMap.Text', 'Utilisez la carte pour vous aider à choisir le meilleur itinéraire vers {0}'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.RouteSelection.To.Text', 'Choisissez votre itinéraire et sélectionnez <strong>''Trouver les horaires de départ''</strong> pour consulter une liste des services opérant vers le site.'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.RouteSelection.From.Text', 'Choisissez votre itinéraire et sélectionnez <strong>''Trouver les horaires de départ''</strong> pour consulter une liste des services opérant vers le site.'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.BtnFindDepartureTimes.Text', 'Trouver un horaire de départ'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.BtnFindDepartureTimes.ToolTip', 'Trouver un horaire de départ'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.RiverServiceNoneFound.Text','Il n''y a aucun service de navettes fluviales pour ce site à la date demandée. Veuillez prévoir l''utilisation d''un autre moyen de transport en commun ou choisir une autre date.'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.RiverServiceResultsHeading.Text', 'Choisissez votre trajet préféré et sélectionnez <strong>''Planifier mon voyage''</strong> pour inclure la navette fluviale dans votre trajet. Le site se trouve près de l''embarcadère d''arrivée. Un délai supplémentaire a été ajouté pour tenir compte du trajet entre l''embarcadère et l''entrée du site. Si vous le souhaitez, vous pouvez rajouter du temps à votre plan de trajet.'

-- loading image on River Services Journey Locations page
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.Loading.Imageurl', 'presentation/hourglass_medium.gif'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.Loading.AlternateText', 'Loading...'
EXEC AddContent @Group, @CultEn, @Collection,'RiverServicesJourneyLocations.loadingMessage.Text', 'Please wait...'

	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.Loading.AlternateText', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection,'RiverServicesJourneyLocations.loadingMessage.Text', 'Veuillez patienter...'
	
-- ParkAndRideJourneyLocations Control
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.PreferredParksHeading.Text', 'What is your booked car park location?'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideTimeSlotHeading.Text', 'What is your booked time slot?'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideTimeSlotNote.Text', 'The time slot will be used to plan the journey to the selected car park'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideBookingURL.Text', 'Book a Park &amp; Ride space'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideBookingURL.URL', 'http://www.firstgroupgamestravel.com/park-and-ride/'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideNote.Text', 'Only Park &amp; Ride car parks open on your selected travel date are shown.'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideBookingNote.Text', 'Park &amp; Ride spaces must be booked in advance'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideNoneFound.Text', 'There are no park-and-ride facilities for your venue on the requested date of travel. Please select a new date.'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideClosedArrivingAt.Text', 'No Park &amp; Ride car parks were found which are open for your outward travel arrival time'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideClosedLeavingFrom.Text', 'No Park &amp; Ride car parks were found which are open for your return travel leaving time'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.ParkAndRideReturnDateDifferent.Text', 'The return date you have entered is different to the date of your outward journey. Park-and-ride car parks are only available for parking for a single day. Please select new date(s).'

	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.PreferredParksHeading.Text', 'Où se trouve votre place de parking réservée?'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideTimeSlotHeading.Text', 'Quel sont vos horaires de réservation?'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideTimeSlotNote.Text', 'Les horaires seront utilisés pour planifier votre trajet vers le parking sélectionné'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideBookingURL.Text', 'Réservez votre place de parc relais'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideNote.Text', 'Seuls les parcs relais ouverts lors de la date de votre voyage sont indiqués.'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideBookingNote.Text', 'Les places dans les parcs relais doivent être réservées à l''avance'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideNoneFound.Text', 'Il n''y a pas de service de parcs relais prévu pour ce site à la date demandée. Veuillez choisir une autre date.'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideClosedArrivingAt.Text', 'Aucun parc relais n''est ouvert à l''heure d''arrivée de votre voyage de retour'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideClosedLeavingFrom.Text', 'Aucun parc relais n''est ouvert à l''heure de départ de votre voyage de retour'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.ParkAndRideReturnDateDifferent.Text', 'La date de retour que vous avez saisie est différente de la date du trajet aller. Le stationnement dans les parcs relais est limité à une journée. Veuillez choisir une autre date de voyage.'

EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeBookingURL.Text', 'Book a Blue Badge space'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeBookingURL.URL', 'http://www.firstgroupgamestravel.com/blue-badge-parking/'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeNote.Text', 'Only Blue Badge car parks open on your selected travel date are shown.'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeBookingNote.Text', 'Blue Badge spaces must be booked in advance'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeNoneFound.Text', 'There are no Blue Badge car park facilities for your venue on the requested date of travel. Please select a new date.'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeClosedArrivingAt.Text', 'No Blue Badge car parks were found which are open for your outward travel arrival time'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeClosedLeavingFrom.Text', 'No Blue Badge car parks were found which are open for your return travel leaving time'
EXEC AddContent @Group, @CultEn, @Collection,'ParkAndRideJourneyLocations.BlueBadgeReturnDateDifferent.Text', 'The return date you have entered is different to the date of your outward journey. Blue Badge car parks are only available for parking for a single day. Please select new date(s).'

	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeBookingURL.Text', 'Réserver une place Blue Badge'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeNote.Text', 'Seuls les parkings Blue Badge ouverts lors de la date de votre voyage sont indiqués.'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeBookingNote.Text', 'Les places Blue Badge doivent être réservées à l''avance'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeNoneFound.Text', 'Il n''y a aucun service de stationnement Blue Badge prévu pour ce site à la date demandée. Veuillez choisir une autre date.'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeClosedArrivingAt.Text', 'Aucun parc relais n''est ouvert à l''heure d''arrivée de votre voyage de retour'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeClosedLeavingFrom.Text', 'Aucun parc relais n''est ouvert à l''heure de départ de votre voyage de retour'
	EXEC AddContent @Group, @CultFr, @Collection,'ParkAndRideJourneyLocations.BlueBadgeReturnDateDifferent.Text', 'La date de retour que vous avez saisie est différente de la date du trajet aller. Le stationnement Blue Badge est limité à une journée. Veuillez choisir une autre date de voyage.'

-- Open In New Window
EXEC AddContent @Group, @CultEn, @Collection,'OpenInNewWindow.URL', 'presentation/link_external_pink.png'
EXEC AddContent @Group, @CultEn, @Collection,'OpenInNewWindow.Blue.URL', 'presentation/link_external_blue.gif'
EXEC AddContent @Group, @CultEn, @Collection,'OpenInNewWindow.AlternateText', 'opens in another window'
EXEC AddContent @Group, @CultEn, @Collection,'OpenInNewWindow.Text', 'opens in another window'
	
	EXEC AddContent @Group, @CultFr, @Collection,'OpenInNewWindow.AlternateText', 'ouvre dans une autre fenêtre'
	EXEC AddContent @Group, @CultFr, @Collection,'OpenInNewWindow.Text', 'ouvre dans une autre fenêtre'

-- Maps
-- Maps - Brands Hatch
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100BND.CyclePark.Url', 'maps/CycleParksMaps/8100BRH_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100BND.CyclePark.AlternateText', 'Map of cycle parks for Brands Hatch'

-- Maps - Olympic park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OPK.CyclePark.Url', 'maps/CycleParksMaps/8100OPK_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OPK.CyclePark.AlternateText', 'Map of cycle parks for Olympic Park'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OPK.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100OPK_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OPK.ParkAndRide.AlternateText', 'Map of car parks for Olympic Park'

-- Maps - Victoria Park Live (Olympic park)
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100VPL.CyclePark.Url', 'maps/CycleParksMaps/8100VPL_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100VPL.CyclePark.AlternateText', 'Map of cycle parks for Victoria Park Live Site'

-- Maps - Greenwich park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.CyclePark.Url', 'maps/CycleParksMaps/8100GRP_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.CyclePark.AlternateText', 'Map of cycle parks for Greenwich Park'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100GRP_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.ParkAndRide.AlternateText', 'Map of car parks for Greenwich Park'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.RiverServices.Url', 'maps/RiverMaps/8100GRP_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.RiverServices.AlternateText', 'Map of river piers for Greenwich Park'

-- Maps - Earls Court
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.CyclePark.Url', 'maps/CycleParksMaps/8100EAR_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.CyclePark.AlternateText', 'Map of cycle parks for Earls Court'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100EAR_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.ParkAndRide.AlternateText', 'Map of car parks for Earls Court'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.RiverServices.Url', 'maps/RiverMaps/8100EAR_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.RiverServices.AlternateText', 'Map of river piers for Earls Court'

-- Maps - Eton Dorney
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100ETD.CyclePark.Url', 'maps/CycleParksMaps/8100ETD_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100ETD.CyclePark.AlternateText', 'Map of cycle parks for Eton Dorney'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100ETD.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100ETD_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100ETD.ParkAndRide.AlternateText', 'Map of car parks for Eton Dorney'

-- Maps - ExCeL
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EXL.CyclePark.Url', 'maps/CycleParksMaps/8100EXL_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EXL.CyclePark.AlternateText', 'Map of cycle parks for ExCeL'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EXL.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100EXL_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EXL.ParkAndRide.AlternateText', 'Map of car parks for ExCeL'

-- Maps - Hadleigh Farm
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAD.CyclePark.Url', 'maps/CycleParksMaps/8100HAD_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAD.CyclePark.AlternateText', 'Map of cycle parks for Hadleigh Farm'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAD.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100HAD_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAD.ParkAndRide.AlternateText', 'Map of car parks for Hadleigh Farm'

-- Maps - Hampden Park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAM.CyclePark.Url', 'maps/CycleParksMaps/8100HAM_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAM.CyclePark.AlternateText', 'Map of cycle parks for Hampden Park'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAM.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100HAM_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAM.ParkAndRide.AlternateText', 'Map of car parks for Hampden Park'

-- Maps - Horse Guards Parade
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.CyclePark.Url', 'maps/CycleParksMaps/8100HGP_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.CyclePark.AlternateText', 'Map of cycle parks for Horse Guards Parade'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100HGP_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.ParkAndRide.AlternateText', 'Map of car parks for Horse Guards Parade'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.RiverServices.Url', 'maps/RiverMaps/8100HGP_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.RiverServices.AlternateText', 'Map of river piers for Horse Guards Parade'

-- Maps - Hyde Park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.CyclePark.Url', 'maps/CycleParksMaps/8100HYD_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.CyclePark.AlternateText', 'Map of cycle parks for Hyde Park'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100HYD_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.ParkAndRide.AlternateText', 'Map of car parks for Hyde Park'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.RiverServices.Url', 'maps/RiverMaps/8100HYD_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.RiverServices.AlternateText', 'Map of river piers for Hyde Park'

-- Maps - Hyde Park Live
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HPL.CyclePark.Url', 'maps/CycleParksMaps/8100HYD_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HPL.CyclePark.AlternateText', 'Map of cycle parks for Hyde Park Live'


-- Maps - Lords Cricket Ground
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LCG.CyclePark.Url', 'maps/CycleParksMaps/8100LCG_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LCG.CyclePark.AlternateText', 'Map of cycle parks for Lords Cricket Ground'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LCG.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100LCG_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LCG.ParkAndRide.AlternateText', 'Map of car parks for Lords Cricket Ground'

-- Maps - Lee Valley White Water Centre
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LVC.CyclePark.Url', 'maps/CycleParksMaps/8100LVC_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LVC.CyclePark.AlternateText', 'Map of cycle parks for Lee Valley White Water Centre'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LVC.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100LVC_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LVC.ParkAndRide.AlternateText', 'Map of car parks for Lee Valley White Water Centre'

-- Maps - Millennium Stadium
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MIL.CyclePark.Url', 'maps/CycleParksMaps/8100MIL_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MIL.CyclePark.AlternateText', 'Map of cycle parks for Millennium Stadium'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MIL.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100MIL_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MIL.ParkAndRide.AlternateText', 'Map of car parks for Millennium Stadium'

-- Maps - The Mall
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.CyclePark.Url', 'maps/CycleParksMaps/8100MAL_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.CyclePark.AlternateText', 'Map of cycle parks for The Mall'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100MAL_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.ParkAndRide.AlternateText', 'Map of car parks for The Mall'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.RiverServices.Url', 'maps/RiverMaps/8100MAL_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.RiverServices.AlternateText', 'Map of river piers for The Mall'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.CyclePark.Url', 'maps/CycleParksMaps/8100MLL_CycleParksMapNorth.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.CyclePark.AlternateText', 'Map of cycle parks for The Mall - North'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100MAL_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.ParkAndRide.AlternateText', 'Map of car parks for The Mall'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.RiverServices.Url', 'maps/RiverMaps/8100MAL_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.RiverServices.AlternateText', 'Map of river piers for The Mall'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.CyclePark.Url', 'maps/CycleParksMaps/8100MLL_CycleParksMapSouth.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.CyclePark.AlternateText', 'Map of cycle parks for The Mall - South'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100MAL_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.ParkAndRide.AlternateText', 'Map of car parks for The Mall'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.RiverServices.Url', 'maps/RiverMaps/8100MAL_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.RiverServices.AlternateText', 'Map of river piers for The Mall'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLE.CyclePark.Url', 'maps/CycleParksMaps/8100MAL_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLE.CyclePark.AlternateText', 'Map of cycle parks for The Mall - East'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLE.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100MAL_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLE.ParkAndRide.AlternateText', 'Map of car parks for The Mall'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLE.RiverServices.Url', 'maps/RiverMaps/8100MAL_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLE.RiverServices.AlternateText', 'Map of river piers for The Mall'

-- Maps - North Greenwich Arena
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.CyclePark.Url', 'maps/CycleParksMaps/8100NGA_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.CyclePark.AlternateText', 'Map of cycle parks for North Greenwich Arena'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100NGA_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.ParkAndRide.AlternateText', 'Map of car parks for North Greenwich Arena'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.RiverServices.Url', 'maps/RiverMaps/8100NGA_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.RiverServices.AlternateText', 'Map of river piers for North Greenwich Arena'

-- Maps - Old Trafford
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OLD.CyclePark.Url', 'maps/CycleParksMaps/8100OLD_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OLD.CyclePark.AlternateText', 'Map of cycle parks for Old Trafford'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OLD.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100OLD_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OLD.ParkAndRide.AlternateText', 'Map of car parks for Old Trafford'

-- Maps - The Royal Artillery Barracks
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.CyclePark.Url', 'maps/CycleParksMaps/8100RAB_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.CyclePark.AlternateText', 'Map of cycle parks for The Royal Artillery Barracks'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100RAB_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.ParkAndRide.AlternateText', 'Map of car parks for The Royal Artillery Barracks'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.RiverServices.Url', 'maps/RiverMaps/8100RAB_RiverMap.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.RiverServices.AlternateText', 'Map of river piers for The Royal Artillery Barracks'

-- Maps - Woolich Live (near The Royal Artillery Barracks)
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WOL.CyclePark.Url', 'maps/CycleParksMaps/8100RAB_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WOL.CyclePark.AlternateText', 'Map of cycle parks for The Royal Artillery Barracks'

-- Maps - Weymouth and Portland - The Nothe
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WAP.CyclePark.Url', 'maps/CycleParksMaps/8100WAP_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WAP.CyclePark.AlternateText', 'Map of cycle parks for Weymouth and Portland - The Nothe'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WAP.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100WAP_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WAP.ParkAndRide.AlternateText', 'Map of car parks for Weymouth and Portland - The Nothe'

-- Maps - Weymouth Live
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WLB.CyclePark.Url', 'maps/CycleParksMaps/8100WAP_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WLB.CyclePark.AlternateText', 'Map of cycle parks for Weymouth Live - on the beach'

-- Maps - Wembley Arena
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEA.CyclePark.Url', 'maps/CycleParksMaps/8100WEA_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEA.CyclePark.AlternateText', 'Map of cycle parks for Wembley Arena'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEA.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100WEA_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEA.ParkAndRide.AlternateText', 'Map of car parks for Wembley Arena'

-- Maps - Wembley Stadium
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEM.CyclePark.Url', 'maps/CycleParksMaps/8100WEM_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEM.CyclePark.AlternateText', 'Map of cycle parks for Wembley Stadium'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEM.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100WEM_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEM.ParkAndRide.AlternateText', 'Map of car parks for Wembley Stadium'

-- Maps - Wimbledon
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WIM.CyclePark.Url', 'maps/CycleParksMaps/8100WIM_CycleParksMap.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WIM.CyclePark.AlternateText', 'Map of cycle parks for Wimbledon'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WIM.ParkAndRide.Url', 'maps/ParkAndRideMaps/8100WIM_ParkAndRideMap.jpg'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WIM.ParkAndRide.AlternateText', 'Map of car parks for Wimbledon'

-- Mapping
EXEC AddContent @Group, @CultEn, @Collection,'Map.Error.NoJourney', 'Sorry, an error has occurred.'

	EXEC AddContent @Group, @CultFr, @Collection,'Map.Error.NoJourney', 'Désolé, une erreur s''est produite.'

-- Olympic Venue Contents
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.SelectVenue.Information','Please select the venue to travel first'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.SelectVenue.Information','Veuillez sélectionner un site de voyager en première'

-- Public Journey options
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Available.Information','To plan a journey to your selected venue, select <strong>''Plan my journey''</strong>. If you have additional mobility requirements, please select these first from the options below.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.PublicJourneyOptions.Available.Information','Pour planifier le trajet vers le site choisi, sélectionnez <strong>''Planifier mon trajet''</strong>. Si vous avez d''autres exigences de mobilité, veuillez les sélectionner en premier lieu à partir des options ci-après.'

-- Cycle options
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.Disable.Information','Cycle journey planning for <strong>London 2012 venues</strong> will be available soon. To find out more about cycling to Games events, please click on the link below.<br /><a href="http://www.london2012.com/paralympics/visiting/">Find out about cycling to the Games</a>.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.NotAvailable.Information','There are currently no dedicated cycle parking facilities close to {0}. You can plan a journey by public transport. Select the <strong>public transport</strong> travel option to do this.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.NotAvailable.DistanceGreaterThanLimit.Information','Your starting point is further from {0} than we would recommend for the average cycle journey. We recommend that you shorten your journey if possible or plan to travel by public transport. Select the <strong>public transport</strong> travel option to do this.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.Available.To.Information','Cycling is a healthy, fun and sustainable way to get to {0}. To plan your cycle journey, select <strong>''next''</strong>.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.CycleOptions.Available.From.Information','Cycling is a healthy, fun and sustainable way to get from {0}. To plan your cycle journey, select <strong>''next''</strong>.'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.Disable.Information','Des itinéraires routiers à vélo pour les <strong>sites de Londres 2012</strong> seront disponibles prochainement. Pour en savoir plus sur comment se rendre à vélo aux épreuves des Jeux, cliquez sur le lien ci-dessous.<br /><a href="http://www.london2012.com/visiting/getting-to-the-games/transport-options/cycling.php">Find out about cycling to the Games</a>.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.NotAvailable.Information','Il n’existe pas actuellement de parc à  vélo à proximité de {0}.  Vous pouvez cependant planifier un trajet par les transports en commun. Pour cela, sélectionnez l’option <strong>transports en commun</strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.NotAvailable.DistanceGreaterThanLimit.Information','Votre point de départ est plus éloigné de {0} que ce que nous recommanderions pour un parcours moyen à vélo. Nous vous recommandons de raccourcir votre trajet, si possible, ou d''envisager de prendre les transports en commun. Pour cela, sélectionnez l''option <strong>transport</strong> en commun.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.Available.To.Information','Le vélo est un moyen de locomotion sportif, plaisant et écologique pour se rendre à {0}. Pour planifier votre trajet à vélo, sélectionnez <strong>''suivant''</strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.CycleOptions.Available.From.Information','Le vélo est un mode de transport sain, amusant et écologique pour se rendre à {0}. Pour planifier votre trajet à vélo, sélectionnez <strong>''suivant''</strong>.'

-- Park And Ride options
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Disable.Information','Park-and-ride car parks are available for the majority of London 2012 venues. Most park-and-ride sites are close to venues and transfers will be short. However, in some instances, transfer can take up to one hour. Road planning will be available from March 2012 onwards. To book your parking space now, please follow the link below.<br /><strong><a href="http://www.firstgroupgamestravel.com/park-and-ride/" target="_blank">Book your 2012 Games park-and-ride space {0}</a></strong>'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.NotAvailable.Information','There are no park-and-ride car parks that serve {0}. We recommend that you travel by public transport. Select the <strong>public transport</strong> travel option to do this.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Available.Information','Park-and-ride car parks are available for {0}. Parking spaces must be booked in advance. <strong><a href="http://www.firstgroupgamestravel.com/park-and-ride/" target="_blank">Book your 2012 Games park-and-ride space {1}</a></strong>. If you have already booked and been allocated a space, select <strong>''next''</strong> to plan your journey.'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Disable.Information','Des services de parcs relais sont disponibles pour la majorité des sites de Londres 2012. La plupart des parcs relais sont à proximité des sites et les transferts seront de courte durée. Toutefois, dans certains cas, le transfert peut prendre jusqu''à une heure. Pour réserver votre place de stationnement, veuillez suivre le lien ci-dessous.<br /><strong><a href="http://www.firstgroupgamestravel.com/park-and-ride/" target="_blank">Réservez votre place de parc relais des Jeux de 2012 {0}</a></strong>'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.NotAvailable.Information','Aucun parc relais ne dessert {0}. Nous vous recommandons d''utiliser les transports en commun. Pour cela, sélectionnez l''option de <strong>transport</strong> en commun.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.ParkAndRideOptions.Available.Information','Des services de parcs relais sont disponibles pour {0}. Ces places de stationnement doivent être réservées à l''avance. <strong><a href="http://www.firstgroupgamestravel.com/park-and-ride/" target="_blank">Réservez votre place de parc relais des Jeux de 2012 {1}</a></strong>. Si vous avez déjà réservé et qu''une place vous a été attribuée, sélectionnez <strong>''suivant''</strong> pour planifier votre trajet.'

-- Blue Badge options
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Disable.Information','Blue Badge car parks are available for all London 2012 venues. Road planning will be available from March 2012 onwards. To book your parking space now, please follow the link below.<br /><strong><a href="http://www.firstgroupgamestravel.com/blue-badge-parking/" target="_blank">Book your Blue Badge space {0}</a></strong>'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.NotAvailable.Information','We are sorry but there are no Blue Badge car parks that serve {0}. We recommend that you plan to travel by public transport instead. Select the <strong>public transport</strong> travel option to do this.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Available.Information','Blue Badge car parks are available for {0}. Blue Badge spaces must be booked in advance. <strong><a href="http://www.firstgroupgamestravel.com/blue-badge-parking/" target="_blank">Book your Blue Badge space {1}</a></strong>. If you have already booked and been allocated a space, select <strong>''next''</strong> to plan your journey.'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Disable.Information','Des stationnements Blue Badge sont disponibles pour tous les sites de Londres 2012. Pour réserver votre place de stationnement, veuillez suivre le lien ci-dessous.<br /><strong><a href="http://www.firstgroupgamestravel.com/blue-badge-parking/" target="_blank">Réservez votre place Blue Badge {0}</a></strong>'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.NotAvailable.Information','Nous sommes désolés, mais  aucun parc de stationnement Blue Badge ne dessert {0}. Nous vous recommandons d''utiliser plutôt les transports en commun. Pour cela, sélectionnez l''option <strong>transport</strong> en commun.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.BlueBadgeOptions.Available.Information','Des stationnements Blue Badge sont disponibles pour {0}. Les places de stationnement Blue Badge doivent être réservées à l''avance. <strong><a href="http://www.firstgroupgamestravel.com/blue-badge-parking/" target="_blank">Réservez votre place Blue Badge {1}</a></strong>. Si vous avez déjà réservé et qu''une place vous a été attribuée, sélectionnez <strong>''suivant''</strong> pour planifier votre trajet.'

-- River Services options
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Disable.Information','We are sorry but this option is currently unavailable.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.NotAvailable.From.Information','We do not recommend travelling from {0} using river services as they are not suitable for this venue.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.NotAvailable.To.Information','We do not recommend travelling to {0} using river services as they are not suitable for this venue.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.From.Information','You may wish to consider taking a river service from {0}. <strong><a href="http://www.london2012.com/paralympics/spectators/travel/book-your-travel/" target="_blank">Click here to find out more details {1}</a></strong>.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.To.Information','You may wish to consider taking a river service to {0}. <strong><a href="http://www.london2012.com/paralympics/spectators/travel/book-your-travel/" target="_blank">Click here to find out more details {1}</a></strong>.'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Available.Information','{0} is served by river services. If you would like to plan a journey using a river service, select <strong>''next''</strong>. River services will be included in your journey plan.'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Disable.Information','Nous sommes désolés mais cette option est actuellement indisponible.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.NotAvailable.From.Information','Il n''est pas recommandé de se rendre à {0} en utilisant la navette fluviale car ce transport n''est pas adapté au site.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.NotAvailable.To.Information','Nous déconseillons l''utilisation de navettes fluviales pour se rendre à {0} car elles ne sont pas prévues pour ce site.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.From.Information','Vous pouvez prendre une navette fluviale depuis {0}. <strong><a href="http://www.london2012.com/fr/spectators/travel/book-your-travel/" target="_blank">Cliquez ici pour en savoir plus {1}</a></strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.To.Information','Vous pouvez prendre une navette fluviale pour vous rendre à {0}. <strong><a href="http://www.london2012.com/fr/spectators/travel/book-your-travel/" target="_blank">Cliquez ici pour en savoir plus {1}</a></strong>.'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOptionTabContainer.RiverServicesOptions.Available.Information','{0} est desservi par un service de navettes fluviales. Si vous prévoyez d''utiliser un service de navettes fluviales pour votre trajet, sélectionnez <strong>''suivant''</strong>. Les services de navettes fluviales seront inclus dans votre plan de trajet.'

-- DataServices -- CycleRouteType
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CycleRouteType.Fastest', 'Fastest'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CycleRouteType.Quietest', 'Quietest'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CycleRouteType.Recreational', 'Recreational'

	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.CycleRouteType.Fastest', 'Trajet rapide'
	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.CycleRouteType.Quietest', 'Trajet calme'
	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.CycleRouteType.Recreational', 'Trajet détente'

-- DataServices -- CountryDrop
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CountryDrop.Default', 'Select country'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CountryDrop.England', 'England'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CountryDrop.Wales', 'Wales'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.CountryDrop.Scotland', 'Scotland'

	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.CountryDrop.Default', 'Choisir une région'

-- DataServices -- Travel News Regions dropdown
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.All', 'All'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.South West', 'South West'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.South East', 'South East'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.London', 'London'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.East Anglia', 'East Anglia'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.East Midlands', 'East Midlands'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.West Midlands', 'West Midlands'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.Yorkshire and Humber', 'Yorkshire and Humber'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.North West', 'North West'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.North East', 'North East'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.Scotland', 'Scotland'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsRegionDrop.Wales', 'Wales'

-- DataServices -- TravelNewsMode
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsViewMode.All', 'All travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsViewMode.LondonUnderground', 'London Underground lines'
EXEC AddContent @Group, @CultEn, @Collection, 'DataServices.NewsViewMode.Venue', 'Venue'

	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.NewsViewMode.All', 'Toutes les informations sur la circulation'
	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.NewsViewMode.LondonUnderground', 'Lignes de métro du London Underground'
	EXEC AddContent @Group, @CultFr, @Collection, 'DataServices.NewsViewMode.Venue', 'Site'

-- TravelNews Page
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.Heading.Text', 'Live travel news'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.Heading.Text', 'Bulletin trafic en temps réel'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.TNFilterHeading.Text',''
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LblRegion.Text','Region'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LblInclude.Text','Include'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.PublicTransportNews.Text','Public transport'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.RoadNews.Text','Road'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LblTNPhrase.Text','Search with specific words or phrases'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LblTNDate.Text','Happening on'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LblRegion.Text','Région'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LblInclude.Text','Inclure'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.PublicTransportNews.Text','Transport en commun'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.RoadNews.Text','Route'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LblTNPhrase.Text','Rechercher par mot ou phrase spécifique'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LblTNDate.Text','Date et horaire'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblUnavailable.Text','Sorry, Live travel news is currently unavailable.'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblNoIncidents.Text','There are currently no incidents reported for the selected options.'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblNoIncidents.Venue.Text','There are currently no incidents reported for the selected venue {0}.'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblNoIncidents.Venues.Text','There are currently no incidents reported for the selected venues {0}.'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblNoIncidents.AllVenues.Text','There are currently no incidents reported for venues.'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblNoIncidents.InvalidVenue.Text','There are currently no incidents reported for the selected venue {0}'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.lblNoIncidents.Venue.Text','Il n''y a actuellement aucun incident signalé sur le site sélectionné : {0}.'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.lblNoIncidents.InvalidVenue.Text','Il n''y a actuellement aucun incident signalé sur le site sélectionné : {0}'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblFilterNews.Text','Select ''Apply filter'' to display incidents'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblVenue.Text','Venues'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.lblAffectedVenues.Text','Venues impacted:'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.OlympicTravelNewsHeader.Text','News impacting games travel'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.OtherTravelNewsHeader.Text','Other travel news'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.lblVenue.Text','Sites'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.OlympicTravelNewsHeader.Text','Évènements ayant une incidence sur le trafic des Jeux'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AutoRefreshLink.Start.Text','Auto-refresh this page'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AutoRefreshLink.Start.ToolTip','Auto-refresh this page'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AutoRefreshLink.Stop.Text','Stop auto-refresh'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AutoRefreshLink.Stop.ToolTip','Stop auto-refresh'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.FilterNews.Text','Apply filter'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.FilterNews.ToolTip','Apply filter'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.FilterNews.Text','Appliquer un filtre'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.FilterNews.ToolTip','Appliquer un filtre'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.TNDatePicker.Image.Url','presentation/DatePicker.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.TNDatePicker.ToolTip','Date picker'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LastUpdated.Text','Last updated'
	
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LastUpdated.Text','Dernière mise à jour'

EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.VenueDropdown.DefaultItem.Text','All Venues'

EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.Loading.Imageurl', 'presentation/hourglass_medium.gif'
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.Loading.AlternateText', 'Loading...'
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.loadingMessage.Text', 'Please wait while we retrieve travel news'
	
	EXEC AddContent @Group, @CultFr, @Collection,'TravelNews.Loading.AlternateText', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection,'TravelNews.loadingMessage.Text', 'Please wait while we retrieve travel news'

EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.LongWaitMessage.Text', 'If the travel news does not appear within 30 seconds, '
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.LongWaitMessageLink.Text', 'please click this link.'
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.LongWaitMessageLink.ToolTip', 'If the travel news does not appear within 30 seconds, please click this link.'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LongWaitMessage.Text', 'If the travel news does not appear within 30 seconds, '
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LongWaitMessageLink.Text', 'please click this link.'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LongWaitMessageLink.ToolTip', 'If the travel news does not appear within 30 seconds, please click this link.'

EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.SeverityButton.collapsed.ImageUrl', 'arrows/right_arrow.png'
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.SeverityButton.expanded.ImageUrl', 'arrows/down_arrow.png'
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.SeverityButton.AlternateText', 'Show'
EXEC AddContent @Group, @CultEn, @Collection,'TravelNews.SeverityButton.ToolTip', 'Show'


-- Travel News Severity heading texts
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.Critical.Text', 'Critical'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.Serious.Text', 'Serious'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.VerySevere.Text', 'Very severe'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.Severe.Text', 'Severe'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.Medium.Text', 'Medium'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.Slight.Text', 'Slight'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SeverityHeading.VerySlight.Text', 'Very slight'

-- Travel News Interactive Map
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.East Anglia.HighlightImage', 'maps/Map_Highlighted_EastAnglia.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.East Anglia.SelectedImage', 'maps/Map_Selected_EastAnglia.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.East Midlands.HighlightImage', 'maps/Map_Highlighted_EastMidlands.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.East Midlands.SelectedImage', 'maps/Map_Selected_EastMidlands.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.ImageUrl', 'maps/Map_UKRegionMap.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.London.HighlightImage', 'maps/Map_Highlighted_London.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.London.SelectedImage', 'maps/Map_Selected_London.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.North East.HighlightImage', 'maps/Map_Highlighted_NorthEast.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.North East.SelectedImage', 'maps/Map_Selected_NorthEast.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.North West.HighlightImage', 'maps/Map_Highlighted_NorthWest.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.North West.SelectedImage', 'maps/Map_Selected_NorthWest.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.Scotland.HighlightImage', 'maps/Map_Highlighted_Scotland.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.Scotland.SelectedImage', 'maps/Map_Selected_Scotland.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.South East.HighlightImage', 'maps/Map_Highlighted_SouthEast.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.South East.SelectedImage', 'maps/Map_Selected_SouthEast.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.South West.HighlightImage', 'maps/Map_Highlighted_SouthWest.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.South West.SelectedImage', 'maps/Map_Selected_SouthWest.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.Wales.HighlightImage', 'maps/Map_Highlighted_Wales.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.Wales.SelectedImage', 'maps/Map_Selected_Wales.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.West Midlands.HighlightImage', 'maps/Map_Highlighted_WestMidlands.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.West Midlands.SelectedImage', 'maps/Map_Selected_WestMidlands.png'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.Yorkshire and Humber.HighlightImage', 'maps/Map_Highlighted_Yorkshire.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'UKRegionImageMap.Yorkshire and Humber.SelectedImage', 'maps/Map_Selected_Yorkshire.png'

-- Travel News - Location Text
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.locationText.Text','Location: {0}'

-- Travel News - Start Date
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.StartDate.Text','Started at {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.StartDate.Format','dd MMM yyyy HH:mm'

-- Travel News - Status Date
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.StatusDate.Updated.Text','Updated at {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.StatusDate.Cleared.Text','Cleared at {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.StatusDate.Format','dd MMM yyyy HH:mm'

-- Travel News - Operator
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.Operator.Text','Transport operator: '

-- Travel News - Severity
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.Severity.Text','Severity: '

-- Travel News - Venues affected
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.VenuesAffected.Text','Venues impacted: '

-- Travel News - Detail
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.Detail.Text','Details: '

-- Travel News - Spectator advice
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.SpectatorAdvice.Text','Spectator advice: '

-- Travel News - Incident Type
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.Text','Incident type: '

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.Road.Planned.Url','presentation/TN_roadworks.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.Road.UnPlanned.Url','presentation/TN_road_incident.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.PublicTransport.Planned.Url','presentation/TN_pt_engineering.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.PublicTransport.UnPlanned.Url','presentation/TN_pt_incident.png'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.Road.Planned.AlternateText','Roadworks'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.Road.UnPlanned.AlternateText','Road incident'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.PublicTransport.Planned.AlternateText','Engineering'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.IncidentType.PublicTransport.UnPlanned.AlternateText','Incident'

-- Travel News- Affected location
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Railway(Regional).Url', 'presentation/jp_rail.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Railway(Suburban).Url', 'presentation/jp_rail.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Metro.Url', 'presentation/jp_metro.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Underground.Url', 'presentation/jp_underground.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Railway(Intercity).Url', 'presentation/jp_rail.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Ferry.Url', 'presentation/jp_ferry.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.PublicTransport.Url', 'presentation/jo_pt_selected.png'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Railway(Regional).AlternateText', 'Railway (Regional)'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Railway(Suburban).AlternateText', 'Railway (Suburban)'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Metro.AlternateText', 'Metro'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Underground.AlternateText', 'Underground'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Railway(Intercity).AlternateText', 'Railway (Intercity)'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Ferry.AlternateText', 'Ferry'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.PublicTransport.AlternateText', 'Public Transport'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.AffectedLocation.PublicTransport.AlternateText', 'Transport en commun'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Road.M.Url', 'presentation/TN_motorway.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Road.A.Url', 'presentation/TN_A_Road.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Road.A(M).Url', 'presentation/TN_A_Road.png'

EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Road.M.AlternateText', 'Motorway'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Road.A.AlternateText', 'A road'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.AffectedLocation.Road.A(M).AlternateText', 'A(M) road'

-- Underground news
EXEC AddContent @Group, @CultEn, @Collection, 'UndergroundNews.LastUpdated.Text','Last updated'
EXEC AddContent @Group, @CultEn, @Collection, 'UndergroundNews.Unavailable.Text','We are unable to bring you live London Underground line status at the moment. Please come back later.'
	
	EXEC AddContent @Group, @CultFr, @Collection, 'UndergroundNews.LastUpdated.Text','Dernière mise à jour'
	EXEC AddContent @Group, @CultFr, @Collection, 'UndergroundNews.Unavailable.Text','Nous ne pouvons vous fournir l''état du trafic de la ligne London Underground pour le moment. Veuillez revenir ultérieurement.'

-- Journey planner widget
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerWidget.WidgetHeading.Text','Plan your journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerWidget.JPPromoImage.ImageUrl','placeholders/getting-to-the-games.jpg'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerWidget.JPPromoImage.AlternateText','Use the London 2012 Spectator Journey Planner to plan your travel to venues from anywhere in Great britain'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerWidget.JPPromoContent.Text','Use the London 2012 Spectator Journey Planner to plan your travel to venues from anywhere in Great britain'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerWidget.JPLink.Text','Plan now'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyPlannerWidget.JPLink.ToolTip','Plan your journey'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerWidget.JPPromoImage.AlternateText','Utilisez l''outil de planification de trajet des spectateurs de Londres 2012 pour planifier votre voyage depuis n''importe quel lieu en Grande-Bretagne'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerWidget.JPPromoContent.Text','Utilisez l''outil de planification de trajet des spectateurs de Londres 2012 pour planifier votre voyage depuis n''importe quel lieu en Grande-Bretagne'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerWidget.WidgetHeading.Text','Planifier votre trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerWidget.JPLink.Text','Voir le trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyPlannerWidget.JPLink.ToolTip','Planifier votre trajet'

-- Travel news widget
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.WidgetHeading.Text','Live travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.MoreLink.Text','More travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.MoreLink.ToolTip','More travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.HeadlineLink.ToolTip','Click for details'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.PrevButton.ImageUrl','arrows/white-arrow-left.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.PrevButton.AlternateText','Prev'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.NextButton.ImageUrl','arrows/white-arrow-right.png'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.NextButton.AlternateText','Next'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.NoIncidents.Text','There are no travel news incidents affecting venues, select more to view all travel news.'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsWidget.NoIncidents.ForJourney.Text','There are no travel news incidents affecting venues in your journey, select more to view all travel news.'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsWidget.WidgetHeading.Text','Bulletin trafic en temps réel'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsWidget.MoreLink.Text','Plus d''informations sur le trafic'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsWidget.MoreLink.ToolTip','Plus d''informations sur le trafic'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsWidget.NextButton.AlternateText','Suivant'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsWidget.NoIncidents.Text','Aucun incident dans les transports ne perturbe actuellement les sites, sélectionner plus pour voir toutes les actualités de transport.'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsWidget.NoIncidents.ForJourney.Text','Aucun incident dans les transports ne perturbe actuellement les sites dans votre trajet, sélectionner plus pour voir toutes les actualités de transport.'
	
-- Travel news info widget
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsInfoWidget.WidgetHeading.Text','Travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsInfoWidget.Content.Text','Travel news provided by <a href="http://www.transportdirect.info" title="Transport Direct">Transport Direct</a>'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsInfoWidget.WidgetHeading.Text','Actualité des transports'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsInfoWidget.Content.Text','Bulletin trafic fourni par <a href="http://www.transportdirect.info" title="Transport Direct">Transport Direct</a>'

EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100BXH.Url','maps/box-hill.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100BND.Url','maps/brands-hatch-venue-jpg.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100COV.Url','maps/city-of-coventry-stadium.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EAR.Url','maps/earls-court.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100ETD.Url','maps/eton-dorney.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EXL.Url','maps/excel.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EXLN1.Url','maps/excel.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EXLN2.Url','maps/excel.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EXLS1.Url','maps/excel.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EXLS2.Url','maps/excel.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100EXLS3.Url','maps/excel.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100GRP.Url','maps/greenwich-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HAD.Url','maps/hadleigh-farm.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HAM.Url','maps/hampden-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HAP.Url','maps/hampton-court-palace.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HGP.Url','maps/horse-guards-parade.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HYD.Url','maps/hyde-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100LVC.Url','maps/lee-valley-white-water-centre.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100LCG.Url','maps/lord-s-cricket-ground.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100MIL.Url','maps/millennium-stadium.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100NGA.Url','maps/north-greenwich-arena.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100OLD.Url','maps/old-trafford.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100OPK.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100AQC.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100STA.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100VEL.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100BBA.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HOC.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100AWP.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HBA.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100BMX.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100ETM.Url','maps/olympic-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100SJP.Url','maps/st-james-park.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100MAL.Url','maps/the-mall.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100MLN.Url','maps/the-mall.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100MLS.Url','maps/the-mall.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100MLE.Url','maps/the-mall.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100RAB.Url','maps/the-royal-artillery-barracks.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100WEA.Url','maps/wembley-arena.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100WEM.Url','maps/wembley-stadium.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100WAP.Url','maps/weymouth-and-portland.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100WIM.Url','maps/wimbledon.jpg'

EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100HPL.Url','maps/hyde-park-live.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100PFL.Url','maps/potters-field-live.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100TSL.Url','maps/trafalgar-square-live.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100VPL.Url','maps/victoria-park-live.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.VenueMapImage.8100WLB.Url','maps/weymouth-and-portland.jpg'

EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.WidgetHeading.Text','Map of {0}'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.PDFLinkText.Text','Download PDF <span class="meta-nav">&gt;</span>'
EXEC AddContent @Group, @CultEn, @Collection,'VenueMapsWidget.PDFLinkText.AlternateText','Download PDF'

	EXEC AddContent @Group, @CultFr, @Collection,'VenueMapsWidget.WidgetHeading.Text','Plan de {0}'
	EXEC AddContent @Group, @CultFr, @Collection,'VenueMapsWidget.PDFLinkText.Text','Télécharger la version PDF <span class="meta-nav">&gt;</span>'
	EXEC AddContent @Group, @CultFr, @Collection,'VenueMapsWidget.PDFLinkText.AlternateText','Télécharger la version PDF'

-- CycleJourneyGPXDownload page
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyGPXDownload.Error.Text','There was a problem generating the GPX file for your journey. Please close your browser and try again or contact us if the problem continues to occur.'
EXEC AddContent @Group, @CultEn, @Collection,'CycleJourneyGPXDownload.FileName','{0} To {1}.gpx'

-- ToolTip Widget
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.PrevButton.ImageUrl','arrows/white-arrow-left.png'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.NextButton.ImageUrl','arrows/white-arrow-right.png'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.PrevButton.AlternateText','Prev'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.NextButton.AlternateText','Next'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.TopTipsHeading.Text','Top tips'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.navigationPrev.Text','Prev'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.navigationNext.Text','Next'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips','1,2,3,4,5'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.NextButton.AlternateText','Suivant'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.TopTipsHeading.Text','Infos pratiques'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.navigationNext.Text','Suivant'

-- NOTE : for indiviual pages put the page Id as show in example below. The tip will be shown in the same order as in the property
--EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyPlannerInput.Toptips','5,2'
-- Page specific Top tips
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyPlannerInput.Toptips','1,2,3,4,5'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyLocations.RiverServices.Toptips','6,7,8,9'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyLocations.Cycle.Toptips','10,11,12,13,14'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyLocations.ParkAndRide.Toptips','15,16,17,18,19'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyLocations.BlueBadge.Toptips','20,21,22,23,24'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.JourneyOptions.Toptips','25,26,27,28,29,30'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Retailers.Toptips','31,32,33,34'

-- Top tips with Ids
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.1','When planning an accessible journey you may wish to choose your nearest accessible station for the best results.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.2','The spectator journey planner allows you to plan a journey from anywhere in Great Britain to a London 2012 venue – choose your origin and venue and select your mode of travel.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.3','Allow more time than normal for travel, as London’s transport system will be much busier than usual.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.4','You can plan and book your preferred mode of travel: public transport, park-and-ride, cycling, accessible transport and river services.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.5','Use the spectator journey planner FAQs to answer your questions.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.1','Pour une planification optimale de votre voyage accessible, choisissez votre station/gare accessible la plus proche.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.2','L''outil de planification de parcours spectateur vous permet de planifier un trajet vers un site de Londres 2012 depuis n''importe quel endroit en Grande Bretagne – choisissez votre point de départ et site d''arrivée ainsi que votre moyen de transport.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.3','Prévoyez plus de temps que d''habitude, car le réseau de transport londonien sera bien plus fréquenté qu''en temps normal.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.4','Vous pouvez planifier et réserver votre moyen de transport préféré : services de transport en commun, parc relais, vélo, transport accessible et navettes fluviales.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.5','Utilisez les FAQ de l''outil de planification de parcours spectateur pour trouver une réponse à vos questions.'

EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.6','Choose the river service that is most suitable for you.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.7','Your journey plan will include your chosen river service.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.8','Remember to book the river service journey if you are happy with your selection – boat capacity is limited, so you should book early.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.9','The <a class="externalLink" target="_blank" href="http://www.london2012.com/paralympics/spectators/travel/book-your-travel/">river services</a> page has more information.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.6','Choisissez le service de navettes fluviales qui vous convient le mieux.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.7','Votre plan de trajet comprendra le service de navettes fluviales choisi.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.8','Pensez à réserver le service de navettes fluviales si votre sélection vous convient – les bateaux ayant une capacité limitée, il est préférable de réserver à l''avance.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.9','Vous trouverez plus d''informations sur la page des <a class="externalLink" target="_blank" href="http://www.london2012.com/fr/spectators/travel/book-your-travel/">services de navettes fluviales</a>'

EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.10','Choose the cycle parking location that is most suitable for you – we will provide a journey plan to that location.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.11','Remember to come back and check your cycle plan – road and cycle path closures may lead to changes.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.12','We will add more features to help you plan your cycle journey.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.13','Bicycle locks are not supplied, so please remember to bring one with you.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.14','If your event finishes at dusk or later, remember your bike lights and wear bright or reflective clothing.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.10','Choisissez le lieu de stationnement pour vélos qui vous convient le mieux – nous vous fournirons un plan pour vous rendre à cet endroit.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.11','Pensez à vérifier et revoir si besoin votre trajet en vélo – des changements peuvent survenir en raison de fermetures de routes et pistes cyclables.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.12','Nous ajouterons d''autres fonctionnalités pour vous aider à planifier votre trajet en vélo.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.13','Pensez à vous munir d''un cadenas de vélo car ceux-ci ne sont pas fournis.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.14','Si l''événement auquel vous assistez se termine à la tombée du jour, pensez à allumer les feux du vélo et à porter des vêtements de couleur vive ou réfléchissants.'

EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.15','Select your allocated park-and-ride site to plan your road and shuttle bus journey to the venue.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.16','Use our maps to plan your car journey to your park-and-ride site. Where your onward travel is by foot, metro and/or train, make sure you have the details.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.17','Do not use satellite navigation (satnav) to reach park-and-ride sites. Be aware that traffic conditions may change closer to the venue so follow signs and the information provided with your tickets and parking permit.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.18','At the end of a session, make sure you get on the correct shuttle bus from the venue to your allocated park-and-ride site.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.19','Come prepared for all types of weather, as sites will be large and cover is only likely to be provided at shuttle bus stops.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.15','Sélectionnez le parc relais qui vous a été attribué pour planifier votre trajet en bus à destination du site.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.16','Utilisez nos cartes pour planifier votre trajet en voiture à destination du parc relais. Si votre trajet aller s''effectue à pied, en métro et/ou en train, assurez-vous d''avoir tous les renseignements utiles.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.17','N''utilisez pas de système de navigation par satellite pour vous rendre au parc relais. Tenez compte des conditions de circulation qui peuvent changer à l''approche du site et suivez les panneaux de signalisation et les informations accompagnant vos billets et votre permis de stationnement.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.18','En sortant du site après la fin d''une session, assurez-vous de prendre la navette de bus à destination du parc relais qui vous a été attribué.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.19','Prévoyez des vêtements pour tout type de temps car les sites sont étendus et les seuls endroits couverts risquent de se limiter aux abris bus.'

EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.20','A limited number of parking spaces are provided close to the venue for disabled spectators who are UK Blue Badge holders or members of an equivalent national scheme.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.21','Choose the Blue Badge site you have been allocated to plan your journey to the venue by road.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.22','You can only book spaces once you have received confirmation of whether you have been allocated tickets. Blue Badge parking is free of charge. Because spaces are limited, we recommend you book as early as possible.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.23','You will need a valid booking permit and Blue Badge (or equivalent) to enter an accessible parking site.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.24','Do not use satellite navigation (satnav) to reach Blue Badge sites. Be aware that traffic conditions may change closer to the venue so follow signs and the information provided with your tickets and parking permit.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.20','Un nombre limité de places de parking sera prévu à proximité de chaque site de compétition pour les spectateurs handicapés détenteurs d''un Blue Badge britannique ou membres d''une organisation nationale similaire.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.21','Choisissez le stationnement Blue Badge qui vous a été attribué pour planifier votre trajet par route à destination du site.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.22','Vous ne pouvez réserver des places qu''après avoir reçu confirmation que des billets vous ont été attribués. Le stationnement Blue Badge est gratuit. Le nombre de places étant limité, nous vous conseillons de réserver le plus tôt possible.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.23','Vous aurez besoin d''un permis de stationner valide et d''un Blue Badge (ou équivalent) pour accéder au site de stationnement accessible. '
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.24','N''utilisez pas de système de navigation par satellite pour vous rendre au lieu de stationnement Blue Badge. Tenez compte des conditions de circulation qui peuvent changer à l''approche du site et suivez les panneaux de signalisation et les informations accompagnant vos billets et votre permis de stationnement.'

EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.25','You can choose to progress to book parts of your journey by selecting either ''Book travel'' at the bottom of the page or ''Book your ticket'' in the journey details.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.26','You should allow time for airport-style security at venues.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.27','Book your transport early – this allows you to choose from the best available options for you.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.28','For events in or around London, you will receive a one-day Games Travelcard with your event ticket covering Tube, bus, DLR and rail journeys.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.29','Make sure you have your London 2012 event ticket with you when travelling on the 2012 Games rail services – including on your return journey!'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.30','London 2012 is not responsible for any financial transactions on retailer websites. We advise you to read the retailer Terms and conditions and Privacy policy before making your booking.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.25','Vous avez le choix entre réserver des parties de votre voyage en sélectionnant ''Effectuer une réservation'' au bas de la page ou ''Réserver votre billet'' dans les informations détaillées du voyage.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.26','Il est conseillé de prévoir un temps supplémentaire pour les contrôles de sécurité de type aéroport sur les sites.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.27','Réservez votre transport à l''avance afin de pouvoir choisir l''option qui vous convient le mieux parmi celles proposées.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.28','Pour les épreuves se déroulant dans et aux environs de Londres, vous recevrez avec votre billet une carte de transport Games Travelcard valable une journée et couvrant vos trajets en métro, bus, DLR et train.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.29','Pensez à avoir votre billet d''entrée Londres 2012 sur vous quand vous utilisez les services ferroviaires pendant les Jeux 2012, y compris lors du trajet retour!'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.30','Londres 2012 n''est pas responsable des transactions financières effectuées sur les sites Internet des prestataires. Nous vous conseillons de bien lire les conditions générales de vente et la politique de confidentialité de ces prestataires avant de faire une réservation.'

EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.31','There are parts of your journey you can book now – select the parts you wish to book and progress to the transport operator booking websites.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.32','Book your transport early – this allows you to choose from the best available options for you.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.33','For events in or around London, you will receive a one-day Games Travelcard with your event ticket covering Tube, bus, DLR and rail journeys.'
EXEC AddContent @Group, @CultEn, @Collection,'TopTipsWidget.Toptips.34','You should allow extra time for airport-style security at venues.'

	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.31','Vous pouvez réserver certaines parties de votre voyage dès maintenant – sélectionnez les parties que vous souhaitez réserver, puis rendez-vous sur les sites de réservation en ligne de ces sociétés de transport.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.32','Réservez votre transport à l''avance afin de pouvoir choisir l''option qui vous convient le mieux parmi celles proposées.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.33','Pour les épreuves se déroulant dans et aux environs de Londres, vous recevrez avec votre billet une carte de transport Games Travelcard valable une journée et couvrant vos trajets en métro, bus, DLR et train.'
	EXEC AddContent @Group, @CultFr, @Collection,'TopTipsWidget.Toptips.34','Il est conseillé de prévoir un temps supplémentaire pour les contrôles de sécurité de type aéroport sur les sites.'

--------------------------
-- Generic Promo Widgets
--------------------------
-- Walking
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoHeadingLink.Text','Walking to the Games'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoHeadingLink.ToolTip','Walking to the Games'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoHeadingLink.Url','http://www.london2012.com/paralympics/spectators/travel/walking/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoImageLink.Url','http://www.london2012.com/paralympics/spectators/travel/walking/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoImage.ImageUrl','placeholders/walking-to-the-games.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoImage.AlternateText','Walking to the Games'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoContent.Text','<p>Read more about walking to Games venues.</p>'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoButtonLink.Url','http://www.london2012.com/paralympics/spectators/travel/walking/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoButtonLink.ToolTip','Find out more'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.Walking.PromoButton.Text','Find out more <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoHeadingLink.Text','Se rendre aux Jeux à pied'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoHeadingLink.ToolTip','Se rendre aux Jeux à pied'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoHeadingLink.Url','http://www.london2012.com/fr/spectators/travel/walking/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoImageLink.Url','http://www.london2012.com/fr/spectators/travel/walking/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoImage.AlternateText','Se rendre aux Jeux à pied'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoContent.Text','<p>En savoir plus sur les moyens de se rendre à pied sur les sites des Jeux.</p>'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoButtonLink.Url','http://www.london2012.com/fr/spectators/travel/walking/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoButtonLink.ToolTip','En savoir plus'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.Walking.PromoButton.Text','En savoir plus <span class="meta-nav">&gt;</span>'
	
-- GamesTravelCard
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoHeadingLink.Text','Games Travelcard'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoHeadingLink.ToolTip','Games Travelcard'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoHeadingLink.Url','http://www.london2012.com/paralympics/spectators/travel/games-travelcard/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoImageLink.Url','http://www.london2012.com/paralympics/spectators/travel/games-travelcard/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoImage.ImageUrl','placeholders/games-travelcard.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoImage.AlternateText','Games Travelcard'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoContent.Text','<p>Read more about the Games Travelcard and the area covered.</p>'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoButtonLink.Url','http://www.london2012.com/paralympics/spectators/travel/games-travelcard/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoButtonLink.ToolTip','Find out more'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GamesTravelCard.PromoButton.Text','Find out more <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoHeadingLink.Text','Carte de transport Games Travelcard'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoHeadingLink.ToolTip','Carte de transport Games Travelcard'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoHeadingLink.Url','http://www.london2012.com/fr/spectators/travel/games-travelcard/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoImageLink.Url','http://www.london2012.com/fr/spectators/travel/games-travelcard/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoImage.AlternateText','Carte de transport Games Travelcard'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoContent.Text','<p>En savoir plus sur la carte de transport Games Travelcard et les zones qu''elle couvrent</p>'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoButtonLink.Url','http://www.london2012.com/fr/spectators/travel/games-travelcard/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoButtonLink.ToolTip','En savoir plus'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GamesTravelCard.PromoButton.Text','En savoir plus <span class="meta-nav">&gt;</span>'

-- AccessibleTravel
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoHeadingLink.Text','Accessible travel information'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoHeadingLink.ToolTip','Accessible travel information'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoHeadingLink.Url','http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoImageLink.Url','http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoImage.ImageUrl','placeholders/accessible-travel-info.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoImage.AlternateText','Accessible travel information'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoContent.Text','<p>Read more about accessible travel to the Games.</p>'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoButtonLink.Url','http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoButtonLink.ToolTip','Find out more'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.AccessibleTravel.PromoButton.Text','Find out more <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.AccessibleTravel.PromoHeadingLink.Url','http://www.london2012.com/fr/spectators/travel/accessible-travel/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.AccessibleTravel.PromoImageLink.Url','http://www.london2012.com/fr/spectators/travel/accessible-travel/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.AccessibleTravel.PromoButtonLink.Url','http://www.london2012.com/fr/spectators/travel/accessible-travel/'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.AccessibleTravel.PromoButtonLink.ToolTip','En savoir plus'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.AccessibleTravel.PromoButton.Text','En savoir plus <span class="meta-nav">&gt;</span>'

-- GettingtotheGames
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoHeadingLink.Text','Getting to the Games'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoHeadingLink.ToolTip','Getting to the Games'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoHeadingLink.Url','http://www.london2012.com/travel/Paralympics/travel'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoImageLink.Url','http://www.london2012.com/travel/Paralympics/travel'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoImage.ImageUrl','placeholders/getting-to-the-games-new.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoImage.AlternateText','Getting to the Games'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoContent.Text','<p>Find out more about travelling to the Games.</p>'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoButtonLink.Url','http://www.london2012.com/travel/Paralympics/travel'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoButtonLink.ToolTip','Read More'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GettingtotheGames.PromoButton.Text','Read More <span class="meta-nav">&gt;</span>'

-- FAQ
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoHeadingLink.Text','Spectator journey planner FAQs'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoHeadingLink.ToolTip','Spectator journey planner FAQs'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoHeadingLink.Url','http://www.london2012.com/paralympics/spectators/travel/travel-faqs/index.html'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoImageLink.Url','http://www.london2012.com/paralympics/spectators/travel/travel-faqs/index.html'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoImage.ImageUrl','placeholders/faqs-promo.jpg'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoImage.AlternateText','Spectator journey planner FAQs'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoContent.Text','<p>Browse our FAQs about the spectator journey planner.</p>'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoButtonLink.Url','http://www.london2012.com/paralympics/spectators/travel/travel-faqs/index.html'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoButtonLink.ToolTip','Browse'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.FAQ.PromoButton.Text','Browse <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoHeadingLink.Text','FAQ de l''outil de planification de trajet des spectateurs'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoHeadingLink.ToolTip','FAQ de l''outil de planification de trajet des spectateurs'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoHeadingLink.Url','http://www.london2012.com/fr/spectators/travel/travel-faqs/index.html'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoImageLink.Url','http://www.london2012.com/fr/spectators/travel/travel-faqs/index.html'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoImage.AlternateText','FAQ de l''outil de planification de trajet des spectateurs'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoContent.Text','<p>Renseignez-vous sur l''outil de planification de trajet des spectateurs dans notre FAQ.</p>'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoButtonLink.Url','http://www.london2012.com/fr/spectators/travel/travel-faqs/index.html'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoButtonLink.ToolTip','Rechercher'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.FAQ.PromoButton.Text','Rechercher <span class="meta-nav">&gt;</span>'

-- GBGNAT Map Widget
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoHeadingLink.Text','Accessible stations in Great Britain'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoHeadingLink.ToolTip','Accessible stations in Great Britain'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoHeadingLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/83/map-of-national-rail-stations-with-staff-assistance-in-the-uk_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoImageLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/83/map-of-national-rail-stations-with-staff-assistance-in-the-uk_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoImage.ImageUrl','placeholders/Map of National Rail stations with staff assistance in the UK.png'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoImage.AlternateText','Download the UK map showing where stations are step-free with assistance available at the station and where there is assistance available at the station, but not necessarily step-free facilities.'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoContent.Text',''
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoButtonLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/83/map-of-national-rail-stations-with-staff-assistance-in-the-uk_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoButtonLink.ToolTip','Download PDF'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.GBGNATMap.PromoButton.Text','Download PDF <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GBGNATMap.PromoHeadingLink.Text','Stations accessibles en Grande-Bretagne'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GBGNATMap.PromoHeadingLink.ToolTip','Stations accessibles en Grande-Bretagne'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GBGNATMap.PromoButtonLink.ToolTip','Télécharger la version PDF'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.GBGNATMap.PromoButton.Text','Télécharger la version PDF <span class="meta-nav">&gt;</span>'

-- SEGNAT Map Widget
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoHeadingLink.Text','Accessible stations in South-East'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoHeadingLink.ToolTip','Accessible stations in South-East'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoHeadingLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/84/map-of-national-rail-stations-with-staff-assistance-in-south-east_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoImageLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/84/map-of-national-rail-stations-with-staff-assistance-in-south-east_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoImage.ImageUrl','placeholders/Map of National Rail stations with staff assistance in south-east.png'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoImage.AlternateText','Download the south-east map showing where stations are step-free with assistance available at the station and where there is assistance available at the station, but not necessarily step-free facilities.'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoContent.Text',''
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoButtonLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/84/map-of-national-rail-stations-with-staff-assistance-in-south-east_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoButtonLink.ToolTip','Download PDF'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.SEGNATMap.PromoButton.Text','Download PDF <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.SEGNATMap.PromoHeadingLink.Text','Stations accessibles dans le sud-est de l''Angleterre'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.SEGNATMap.PromoHeadingLink.ToolTip','Stations accessibles dans le sud-est de l''Angleterre'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.SEGNATMap.PromoButtonLink.ToolTip','Télécharger la version PDF'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.SEGNATMap.PromoButton.Text','Télécharger la version PDF <span class="meta-nav">&gt;</span>'

-- London GNAT Map Widget
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoHeadingLink.Text','Accessible stations in London'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoHeadingLink.ToolTip','Accessible stations in London'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoHeadingLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/82/map-of-accessible-stations-in-london_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoImageLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/82/map-of-accessible-stations-in-london_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoImage.ImageUrl','placeholders/Map of accessible stations in London.png'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoImage.AlternateText','Download the London map showing where stations are step-free with assistance available at the station and where there is assistance available at the station, but not necessarily step-free facilities.'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoContent.Text',''
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoButtonLink.Url','http://www.london2012.com/mm/Document/spectators/Travel/01/24/89/82/map-of-accessible-stations-in-london_Neutral.pdf'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoButtonLink.ToolTip','Download PDF'
EXEC AddContent @Group, @CultEn, @Collection,'GenericPromoWidget.LondonGNATMap.PromoButton.Text','Download PDF <span class="meta-nav">&gt;</span>'

	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.LondonGNATMap.PromoHeadingLink.Text','Stations accessibles dans Londres'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.LondonGNATMap.PromoHeadingLink.ToolTip','Stations accessibles dans Londres'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.LondonGNATMap.PromoButtonLink.ToolTip','Télécharger la version PDF'
	EXEC AddContent @Group, @CultFr, @Collection,'GenericPromoWidget.LondonGNATMap.PromoButton.Text','Télécharger la version PDF <span class="meta-nav">&gt;</span>'

-----------------------------------------------------------------------------------------
-- Cycle Parks bullet styles
-- this will position the bullet links on the map
-----------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.EARCY01.Style','position:absolute;left:341px;top:200px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.ETDCY01.Style','position:absolute;left:257px;top:235px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.ETDCY02.Style','position:absolute;left:70px;top:138px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.EXLCY01.Style','position:absolute;left:76px;top:154px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.GRPCYC01.Style','position:absolute;left:6px;top:147px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.GRPCYC02.Style','position:absolute;left:141px;top:10px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.GRPCYC03_BH1.Style','position:absolute;left:311px;top:372px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.GRPCYC03_BH6.Style','position:absolute;left:311px;top:372px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.HADCY01.Style','position:absolute;left:186px;top:140px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.HADCY02.Style','position:absolute;left:133px;top:262px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.LCGCY01.Style','position:absolute;left:249px;top:254px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.LVCCY01.Style','position:absolute;left:259px;top:95px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.NGACY01.Style','position:absolute;left:258px;top:311px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.OPKCYC01.Style','position:absolute;left:78px;top:267px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.OPKCYC02.Style','position:absolute;left:132px;top:32px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.OPKCYC03.Style','position:absolute;left:331px;top:304px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.RABCY01.Style','position:absolute;left:166px;top:345px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.WAPCY02.Style','position:absolute;left:97px;top:173px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.WEACY01.Style','position:absolute;left:361px;top:177px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.WEMCY01.Style','position:absolute;left:361px;top:177px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.CyclePark.WIMCY01.Style','position:absolute;left:215px;top:135px;'


-----------------------------------------------------------------------------------------
-- River Services bullet styles
-- this will position the bullet links on the map
-----------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.RiverServices.9300BFR_9300GNW.Style','position:absolute;left:95px;top:64px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.RiverServices.9300BFR_9300GNW.ImageUrl','maps/rivermaps/9300BFR_9300GNW.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.RiverServices.9300CAW_9300GNW.Style','position:absolute;left:317px;top:98px;'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.RiverServices.9300CAW_9300GNW.ImageUrl','maps/rivermaps/9300CAW_9300GNW.png'

-------------------------------------------------------------------------------------------
-- Further Accessibility Options Page
-------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.AdminAreaList.DefaultItem.Text','Select county'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.DistrictList.DefaultItem.Text','Select London borough'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATList.DefaultItem.Text','Select your stop'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATList.NoStopsFound.Text','No stops found'
EXEC AddContent @Group, @CultEn, @Collection,'AccessiblilityOptions.ValidationError.Text','Please select a valid nearest station'
EXEC AddContent @Group, @CultEn, @Collection,'AccessiblilityOptions.NoStopSelected.Text','Please select a valid nearest station'

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.AdminAreaList.DefaultItem.Text','Choisir un comté'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.GNATList.DefaultItem.Text','Choisir un arrêt'

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Heading.Text', 'Select your accessible stop'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblMapInfo.Origin.Text', 'See maps to help you find a suitable origin'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblMapInfo.Destination.Text', 'See maps to help you find a suitable destination'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblRequestJourney.RequireSpecialAssistance.Text','You requested a journey with staff assistance at stations, stops and piers.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblRequestJourney.RequireStepFreeAccess.Text','You requested a journey that is level and suitable for a wheelchair user.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblRequestJourney.RequireStepFreeAccessAndAssistance.Text','You requested a journey that is level and suitable for a wheelchair user and also require staff assistance.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblFrom.Text','From'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblTo.Text','To'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblDateTime.Text','On Date'

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Heading.Text', 'Choisir un arrêt accessible'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblMapInfo.Origin.Text', 'Consulter une carte pour trouver un point de départ approprié'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblMapInfo.Destination.Text', 'See maps to help you find a suitable destination'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblRequestJourney.RequireSpecialAssistance.Text','Vous avez demandé un trajet incluant une assistance aux gares, arrêts et embarcadères.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblRequestJourney.RequireStepFreeAccess.Text','Vous avez demandé un trajet sans escalier, qui est adapté aux utilisateurs de fauteuil roulant.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblRequestJourney.RequireStepFreeAccessAndAssistance.Text','Vous avez demandé un trajet sans escalier, qui est adapté aux utilisateurs de fauteuil roulant et comprenant un service d''assistance.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblFrom.Text','Départ'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblTo.Text','Arrivée'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblDateTime.Text','Le'

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Accessibility.RequireSpecialAssistance.Origin.Text',               'The spectator journey planner is not able to find a journey meeting your requirements from your chosen origin. Please select an area from the drop down list below to search for stations near to your origin that meet your accessibility requirements.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Accessibility.RequireSpecialAssistance.Destination.Text',          'The spectator journey planner is not able to find a journey meeting your requirements to your chosen destination. Please select an area from the drop down list below to search for stations near to your destination that meet your accessibility requirements.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccess.Origin.Text',                  'The spectator journey planner is not able to find a journey meeting your requirements from your chosen origin. Please select an area from the drop down list below to search for stations near to your origin that meet your accessibility requirements.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccess.Destination.Text',             'The spectator journey planner is not able to find a journey meeting your requirements to your chosen destination. Please select an area from the drop down list below to search for stations near to your destination that meet your accessibility requirements.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Origin.Text',     'The spectator journey planner is not able to find a journey meeting your requirements from your chosen origin. Please select an area from the drop down list below to search for stations near to your origin that meet your accessibility requirements.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Destination.Text','The spectator journey planner is not able to find a journey meeting your requirements to your chosen destination. Please select an area from the drop down list below to search for stations near to your destination that meet your accessibility requirements.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.AccessibilityPlanJourneyInfo.Text',''

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Accessibility.RequireSpecialAssistance.Origin.Text',               'L''outil de planification de trajet des spectateurs ne trouve aucun trajet correspondant au point de départ sélectionné. Merci de choisir une zone dans le menu déroulant ci-dessous pour trouver les stations à proximité de votre lieu d''origine remplissant les conditions d''accessibilité requises.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Accessibility.RequireSpecialAssistance.Destination.Text',          'L''outil de planification de trajet des spectateurs ne trouve aucun trajet correspondant au point de départ sélectionné. Merci de choisir une zone dans le menu déroulant ci-dessous pour trouver les stations à proximité de votre lieu d''origine remplissant les conditions d''accessibilité requises.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccess.Origin.Text',                  'L''outil de planification de trajet des spectateurs ne trouve aucun trajet correspondant au point de départ sélectionné. Merci de choisir une zone dans le menu déroulant ci-dessous pour trouver les stations à proximité de votre lieu d''origine remplissant les conditions d''accessibilité requises.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccess.Destination.Text',             'L''outil de planification de trajet des spectateurs ne trouve aucun trajet correspondant au point de départ sélectionné. Merci de choisir une zone dans le menu déroulant ci-dessous pour trouver les stations à proximité de votre lieu d''origine remplissant les conditions d''accessibilité requises.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Origin.Text',     'L''outil de planification de trajet des spectateurs ne trouve aucun trajet correspondant au point de départ sélectionné. Merci de choisir une zone dans le menu déroulant ci-dessous pour trouver les stations à proximité de votre lieu d''origine remplissant les conditions d''accessibilité requises.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Destination.Text','L''outil de planification de trajet des spectateurs ne trouve aucun trajet correspondant au point de départ sélectionné. Merci de choisir une zone dans le menu déroulant ci-dessous pour trouver les stations à proximité de votre lieu d''origine remplissant les conditions d''accessibilité requises.'
	

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATStopTypeList.Rail.Text','Rail'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATStopTypeList.Ferry.Text','Ferry'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATStopTypeList.Underground.Text','Underground'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATStopTypeList.DLR.Text','DLR'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATStopTypeList.Coach.Text','Coach'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.GNATStopTypeList.Tram.Text','Tram'

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.StopTypeSelect.Origin.Text','Select the type of stop you would prefer to start your journey from'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.StopTypeSelect.Destination.Text','Select the type of stop you would prefer to end your journey at'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.StopSelectInfo.Origin.Text','Select your start country and location to view a list of your nearest accessible stations.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.StopSelectInfo.Destination.Text','Select your end country and location to view a list of your nearest accessible stations.'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblCountry.Text','Select your country'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblAdminArea.Text','Select your county'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.LblDistrict.Text','Select your borough (if known)'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.JourneyFrom.Text','Select your nearest station'

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.StopTypeSelect.Origin.Text','Sélectionner le type de moyen de transport par lequel commencer le trajet'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.StopTypeSelect.Destination.Text','Sélectionner le type de moyen de transport par lequel commencer le trajet'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.StopSelectInfo.Origin.Text','Sélectionner un pays de départ et une adresse pour voir une liste des stations accessibles les plus proches.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.StopSelectInfo.Destination.Text','Sélectionner un pays de départ et une adresse pour voir une liste des stations accessibles les plus proches.'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblCountry.Text','Choisir un pays'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblAdminArea.Text','Choisir une région'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.LblDistrict.Text','Select your borough (if known)'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.JourneyFrom.Text','Choisir la station la plus proche'

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Back.Text', '&lt; Back'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Back.ToolTip', 'Back'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.PlanJourney.Text', 'Plan my journey &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.PlanJourney.ToolTip', 'Plan my journey'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnAdminAreaListGo.Text', 'Go &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnCountryListGo.Text', 'Go &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnDistrictListGo.Text', 'Go &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnStopTypeListGo.Text', 'Go &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnGNATListGo.Text', 'Go &gt;'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnAdminAreaListGo.ToolTip', 'Go'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnCountryListGo.ToolTip', 'Go'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnDistrictListGo.ToolTip', 'Go'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnStopTypeListGo.ToolTip', 'Go'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.BtnGNATListGo.ToolTip', 'Go'

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.PlanJourney.Text', 'Planifier mon trajet &gt;'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.PlanJourney.ToolTip', 'Planifier mon trajet'

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Update.Text', 'Update stations'
EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.Update.ToolTip', 'Update stations'

EXEC AddContent @Group, @CultEn, @Collection,'AccessibilityOptions.NoGNATStopFound.Text','There are no stations in this area that meet your accessibility requirements. Please choose an area nearby to search for other stations that meet your accessibility requirements.'

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Back.Text', '&lt; Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.Back.ToolTip', 'Retour'

	EXEC AddContent @Group, @CultFr, @Collection,'AccessibilityOptions.NoGNATStopFound.Text','Il n''y a pas de station dans cette zone qui réponde à vos conditions d''accessibilité. Veuillez choisir une zone proche pour sélectionner une station qui répond à vos conditions d''accessibilité.'


-----------------------------------------------------------------------------------------------
-- Calendar Jquery calendar resource content to make it french/english
-----------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'Calendar.ButtonText', 'Calendar of the London 2012 Games Period'
EXEC AddContent @Group, @CultEn, @Collection,'Calendar.NextText', 'Next'
EXEC AddContent @Group, @CultEn, @Collection,'Calendar.PrevText', 'Previous'
EXEC AddContent @Group, @CultEn, @Collection,'Calendar.DayNames', 'Su, Mo, Tu, We, Th, Fr, Sa'
EXEC AddContent @Group, @CultEn, @Collection,'Calendar.MonthNames', 'January, February, March, April, May, June, July, August, September, October, November, December'


	EXEC AddContent @Group, @CultFr, @Collection,'Calendar.ButtonText', 'Calendrier des Jeux de Londres 2012'
	EXEC AddContent @Group, @CultFr, @Collection,'Calendar.NextText', 'Suivant'

-----------------------------------------------------------------------------------------------
-- Cycle map page
-----------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'CycleMap.Header', 'Cycle journey map'
EXEC AddContent @Group, @CultEn, @Collection,'CycleMap.SoftContent', '' -- not specified yet
GO

-- =============================================
-- Content script to add Sitemap resource data
-- =============================================

------------------------------------------------
-- Sitemap content, all added to the group 'Sitemap'
------------------------------------------------

DECLARE @Group varchar(100) = 'Sitemap'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

-- Common
EXEC AddContent @Group, @CultEn, 'Pages', 'Pages.Title', 'Plan your travel | London 2012'
EXEC AddContent @Group, @CultEn, 'Pages', 'Pages.Title.Seperator', ' | '

-- Example format for sitemap resource content:
-- group, language, resourceKey(as from sitemap), resourceKey+XXX, value

-- London2012 breadcrumbs
EXEC AddContent @Group, @CultEn, 'London2012.Homepage', 'London2012.Homepage.Breadcrumb.Title', 'London 2012 homepage'
EXEC AddContent @Group, @CultEn, 'London2012.Visiting', 'London2012.Visiting.Breadcrumb.Title', 'Visiting in 2012'
EXEC AddContent @Group, @CultEn, 'London2012.Getting', 'London2012.Getting.Breadcrumb.Title', 'Getting to the Games'
EXEC AddContent @Group, @CultEn, 'London2012.Planning', 'London2012.Planning.Breadcrumb.Title', 'Plan your travel'

	EXEC AddContent @Group, @CultFr, 'London2012.Homepage', 'London2012.Homepage.Breadcrumb.Title', 'Page d''accueil'
	EXEC AddContent @Group, @CultFr, 'London2012.Visiting', 'London2012.Visiting.Breadcrumb.Title', 'Visiter en 2012'
	EXEC AddContent @Group, @CultFr, 'London2012.Getting', 'London2012.Getting.Breadcrumb.Title', 'Se rendre aux Jeux'
	EXEC AddContent @Group, @CultFr, 'London2012.Planning', 'London2012.Planning.Breadcrumb.Title', 'Planifiez votre voyage'

-- Default page
EXEC AddContent @Group, @CultEn, 'Pages.Default', 'Pages.Default.Title', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Default', 'Pages.Default.Breadcrumb.Title', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Default', 'Pages.Default.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Default', 'Pages.Default.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Default', 'Pages.Default.Canonical', 'http://www.london2012.com/travel/'

-- JourneyPlannerInput page
EXEC AddContent @Group, @CultEn, 'Pages.JourneyPlannerInput', 'Pages.JourneyPlannerInput.Title', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyPlannerInput', 'Pages.JourneyPlannerInput.Breadcrumb.Title', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyPlannerInput', 'Pages.JourneyPlannerInput.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyPlannerInput', 'Pages.JourneyPlannerInput.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyPlannerInput', 'Pages.JourneyPlannerInput.Canonical', 'http://www.london2012.com/travel/'

-- AccessibilityOptions page
EXEC AddContent @Group, @CultEn, 'Pages.AccessibilityOptions', 'Pages.AccessibilityOptions.Title', 'Select your stop'
EXEC AddContent @Group, @CultEn, 'Pages.AccessibilityOptions', 'Pages.AccessibilityOptions.Breadcrumb.Title', 'Select your stop'
EXEC AddContent @Group, @CultEn, 'Pages.AccessibilityOptions', 'Pages.AccessibilityOptions.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.AccessibilityOptions', 'Pages.AccessibilityOptions.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.AccessibilityOptions', 'Pages.AccessibilityOptions.Canonical', 'http://www.london2012.com/travel/'

-- JourneyLocations page
EXEC AddContent @Group, @CultEn, 'Pages.JourneyLocations', 'Pages.JourneyLocations.Title', 'Journey locations'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyLocations', 'Pages.JourneyLocations.Breadcrumb.Title', 'Journey locations'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyLocations', 'Pages.JourneyLocations.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyLocations', 'Pages.JourneyLocations.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyLocations', 'Pages.JourneyLocations.Canonical', 'http://www.london2012.com/travel/'

-- JourneyOptions page
EXEC AddContent @Group, @CultEn, 'Pages.JourneyOptions', 'Pages.JourneyOptions.Title', 'Journey options'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyOptions', 'Pages.JourneyOptions.Breadcrumb.Title', 'Journey options'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyOptions', 'Pages.JourneyOptions.Meta.Description', 'Spectator journey options'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyOptions', 'Pages.JourneyOptions.Meta.Keywords', 'London 2012, Spectator journey planner, Journey options'
EXEC AddContent @Group, @CultEn, 'Pages.JourneyOptions', 'Pages.JourneyOptions.Canonical', 'http://www.london2012.com/travel/'

-- PrintableJourneyOptions page
EXEC AddContent @Group, @CultEn, 'Pages.PrintableJourneyOptions', 'Pages.PrintableJourneyOptions.Title', 'Printable journey options'
EXEC AddContent @Group, @CultEn, 'Pages.PrintableJourneyOptions', 'Pages.PrintableJourneyOptions.Breadcrumb.Title', 'Printable journey options'
EXEC AddContent @Group, @CultEn, 'Pages.PrintableJourneyOptions', 'Pages.PrintableJourneyOptions.Meta.Description', 'Spectator journey options'
EXEC AddContent @Group, @CultEn, 'Pages.PrintableJourneyOptions', 'Pages.PrintableJourneyOptions.Meta.Keywords', 'London 2012, Spectator journey planner, Journey options'
EXEC AddContent @Group, @CultEn, 'Pages.PrintableJourneyOptions', 'Pages.PrintableJourneyOptions.Canonical', 'http://www.london2012.com/travel/'

-- Retailers page
EXEC AddContent @Group, @CultEn, 'Pages.Retailers', 'Pages.Retailers.Title', 'Retailers'
EXEC AddContent @Group, @CultEn, 'Pages.Retailers', 'Pages.Retailers.Breadcrumb.Title', 'Retailers'
EXEC AddContent @Group, @CultEn, 'Pages.Retailers', 'Pages.Retailers.Meta.Description', 'Spectator journey retailers'
EXEC AddContent @Group, @CultEn, 'Pages.Retailers', 'Pages.Retailers.Meta.Keywords', 'London 2012, Spectator journey planner, Retailers'
EXEC AddContent @Group, @CultEn, 'Pages.Retailers', 'Pages.Retailers.Canonical', 'http://www.london2012.com/travel/'

-- RetailerHandoff page
EXEC AddContent @Group, @CultEn, 'Pages.RetailerHandoff', 'Pages.RetailerHandoff.Title', 'Retailer handoff'
EXEC AddContent @Group, @CultEn, 'Pages.RetailerHandoff', 'Pages.RetailerHandoff.Breadcrumb.Title', 'Retailer handoff'
EXEC AddContent @Group, @CultEn, 'Pages.RetailerHandoff', 'Pages.RetailerHandoff.Meta.Description', 'Spectator journey retailer handoff'
EXEC AddContent @Group, @CultEn, 'Pages.RetailerHandoff', 'Pages.RetailerHandoff.Meta.Keywords', 'London 2012, Spectator journey planner, Retailer handoff'
EXEC AddContent @Group, @CultEn, 'Pages.RetailerHandoff', 'Pages.RetailerHandoff.Canonical', 'http://www.london2012.com/travel/'

-- MapBing page
EXEC AddContent @Group, @CultEn, 'Pages.MapBing', 'Pages.MapBing.Title', 'Map'
EXEC AddContent @Group, @CultEn, 'Pages.MapBing', 'Pages.MapBing.Breadcrumb.Title', 'Map'
EXEC AddContent @Group, @CultEn, 'Pages.MapBing', 'Pages.MapBing.Meta.Description', 'Spectator journey planner map'
EXEC AddContent @Group, @CultEn, 'Pages.MapBing', 'Pages.MapBing.Meta.Keywords', 'London 2012, Spectator journey planner, Map'
EXEC AddContent @Group, @CultEn, 'Pages.MapBing', 'Pages.MapBing.Canonical', 'http://www.london2012.com/travel/'

-- MapGoogle page
EXEC AddContent @Group, @CultEn, 'Pages.MapGoogle', 'Pages.MapGoogle.Title', 'Map'
EXEC AddContent @Group, @CultEn, 'Pages.MapGoogle', 'Pages.MapGoogle.Breadcrumb.Title', 'Map'
EXEC AddContent @Group, @CultEn, 'Pages.MapGoogle', 'Pages.MapGoogle.Meta.Description', 'Spectator journey planner map'
EXEC AddContent @Group, @CultEn, 'Pages.MapGoogle', 'Pages.MapGoogle.Meta.Keywords', 'London 2012, Spectator journey planner, Map'
EXEC AddContent @Group, @CultEn, 'Pages.MapGoogle', 'Pages.MapGoogle.Canonical', 'http://www.london2012.com/travel/'

-- TravelNews page
EXEC AddContent @Group, @CultEn, 'Pages.TravelNews', 'Pages.TravelNews.Title', 'Live travel news'
EXEC AddContent @Group, @CultEn, 'Pages.TravelNews', 'Pages.TravelNews.Breadcrumb.Title', 'Live travel news'
EXEC AddContent @Group, @CultEn, 'Pages.TravelNews', 'Pages.TravelNews.Meta.Description', 'Spectator journey planner Live travel news'
EXEC AddContent @Group, @CultEn, 'Pages.TravelNews', 'Pages.TravelNews.Meta.Keywords', 'London 2012, Spectator journey planner, Live travel news'
EXEC AddContent @Group, @CultEn, 'Pages.TravelNews', 'Pages.TravelNews.Canonical', 'http://www.london2012.com/travel/'

-- Error page
EXEC AddContent @Group, @CultEn, 'Pages.Error', 'Pages.Error.Title', 'Error'
EXEC AddContent @Group, @CultEn, 'Pages.Error', 'Pages.Error.Breadcrumb.Title', 'Error'
EXEC AddContent @Group, @CultEn, 'Pages.Error', 'Pages.Error.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Error', 'Pages.Error.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Error', 'Pages.Error.Canonical', 'http://www.london2012.com/travel/'

-- Sorry page
EXEC AddContent @Group, @CultEn, 'Pages.Sorry', 'Pages.Sorry.Title', 'Sorry'
EXEC AddContent @Group, @CultEn, 'Pages.Sorry', 'Pages.Sorry.Breadcrumb.Title', 'Sorry'
EXEC AddContent @Group, @CultEn, 'Pages.Sorry', 'Pages.Sorry.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Sorry', 'Pages.Sorry.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.Sorry', 'Pages.Sorry.Canonical', 'http://www.london2012.com/travel/'

-- PageNotFound page
EXEC AddContent @Group, @CultEn, 'Pages.PageNotFound', 'Pages.PageNotFound.Title', 'Page not found'
EXEC AddContent @Group, @CultEn, 'Pages.PageNotFound', 'Pages.PageNotFound.Breadcrumb.Title', 'Page not found'
EXEC AddContent @Group, @CultEn, 'Pages.PageNotFound', 'Pages.PageNotFound.Meta.Description', 'Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.PageNotFound', 'Pages.PageNotFound.Meta.Keywords', 'London 2012, Spectator journey planner'
EXEC AddContent @Group, @CultEn, 'Pages.PageNotFound', 'Pages.PageNotFound.Canonical', 'http://www.london2012.com/travel/'


-- MobileDefault page
EXEC AddContent @Group, @CultEn, 'Pages.MobileDefault', 'Pages.MobileDefault.Title', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDefault', 'Pages.MobileDefault.Breadcrumb.Title', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDefault', 'Pages.MobileDefault.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDefault', 'Pages.MobileDefault.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDefault', 'Pages.MobileDefault.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileDefault', 'Pages.MobileDefault.Title', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDefault', 'Pages.MobileDefault.Breadcrumb.Title', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDefault', 'Pages.MobileDefault.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDefault', 'Pages.MobileDefault.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileInput page
EXEC AddContent @Group, @CultEn, 'Pages.MobileInput', 'Pages.MobileInput.Title', 'Input'
EXEC AddContent @Group, @CultEn, 'Pages.MobileInput', 'Pages.MobileInput.Breadcrumb.Title', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileInput', 'Pages.MobileInput.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileInput', 'Pages.MobileInput.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileInput', 'Pages.MobileInput.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileInput', 'Pages.MobileInput.Breadcrumb.Title', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileInput', 'Pages.MobileInput.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileInput', 'Pages.MobileInput.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileSummary page
EXEC AddContent @Group, @CultEn, 'Pages.MobileSummary', 'Pages.MobileSummary.Title', 'Summary'
EXEC AddContent @Group, @CultEn, 'Pages.MobileSummary', 'Pages.MobileSummary.Breadcrumb.Title', 'Journey summary'
EXEC AddContent @Group, @CultEn, 'Pages.MobileSummary', 'Pages.MobileSummary.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileSummary', 'Pages.MobileSummary.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileSummary', 'Pages.MobileSummary.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileSummary', 'Pages.MobileSummary.Breadcrumb.Title', 'Résumé du trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileSummary', 'Pages.MobileSummary.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileSummary', 'Pages.MobileSummary.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileDetail page
EXEC AddContent @Group, @CultEn, 'Pages.MobileDetail', 'Pages.MobileDetail.Title', 'Details'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDetail', 'Pages.MobileDetail.Breadcrumb.Title', 'Journey detail'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDetail', 'Pages.MobileDetail.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDetail', 'Pages.MobileDetail.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDetail', 'Pages.MobileDetail.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileDetail', 'Pages.MobileDetail.Breadcrumb.Title', 'Détails du trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDetail', 'Pages.MobileDetail.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDetail', 'Pages.MobileDetail.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileDirection page
EXEC AddContent @Group, @CultEn, 'Pages.MobileDirection', 'Pages.MobileDirection.Title', 'Directions'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDirection', 'Pages.MobileDirection.Breadcrumb.Title', 'Journey direction'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDirection', 'Pages.MobileDirection.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDirection', 'Pages.MobileDirection.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileDirection', 'Pages.MobileDirection.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileDirection', 'Pages.MobileDirection.Breadcrumb.Title', 'Destination du trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDirection', 'Pages.MobileDirection.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileDirection', 'Pages.MobileDirection.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileMap page
EXEC AddContent @Group, @CultEn, 'Pages.MobileMap', 'Pages.MobileMap.Title', 'Map'
EXEC AddContent @Group, @CultEn, 'Pages.MobileMap', 'Pages.MobileMap.Breadcrumb.Title', 'Journey map'
EXEC AddContent @Group, @CultEn, 'Pages.MobileMap', 'Pages.MobileMap.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileMap', 'Pages.MobileMap.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileMap', 'Pages.MobileMap.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileMap', 'Pages.MobileMap.Title', 'Carte'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileMap', 'Pages.MobileMap.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileMap', 'Pages.MobileMap.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileTravelNews page
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Title', 'Travel news'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Breadcrumb.Title', 'Travel news'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Canonical', 'http://www.london2012.com/travel/'

	EXEC AddContent @Group, @CultFr, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Title', 'Actualité des transports'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Breadcrumb.Title', 'Actualité des transports'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Meta.Description', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, 'Pages.MobileTravelNews', 'Pages.MobileTravelNews.Meta.Keywords', 'London 2012, Outil de planification de trajet'

-- MobileTravelNewsDetail page
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNewsDetail', 'Pages.MobileTravelNewsDetail.Title', 'Travel news detail'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNewsDetail', 'Pages.MobileTravelNewsDetail.Breadcrumb.Title', 'Travel news detail'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNewsDetail', 'Pages.MobileTravelNewsDetail.Meta.Description', 'Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNewsDetail', 'Pages.MobileTravelNewsDetail.Meta.Keywords', 'London 2012, Travel planner'
EXEC AddContent @Group, @CultEn, 'Pages.MobileTravelNewsDetail', 'Pages.MobileTravelNewsDetail.Canonical', 'http://www.london2012.com/travel/'

GO

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

-- =============================================
-- Content script to add resource data
-- =============================================

------------------------------------------------
-- Journey Output content, all added to the group 'JourneyOutput'
------------------------------------------------

DECLARE @Group varchar(100) = 'JourneyOutput'
DECLARE @Collection varchar(100) = 'Journey'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

------------------------------------------------------------------------------------------------------------------
-- Messages
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.JourneyWebTooFarAhead', 'The spectator journey planner enables journey planning only between 18 July and 14 September 2012. Please select a date in this period.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.JourneyWebNoResults', 'Sorry, the journey planner is currently unable to obtain public transport journey options using the details you have provided. You may wish to try again using a different date or time.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.JourneyWebTooFarAhead', 'L''outil de planification de parcours spectateur permet seulement de planifier des parcours entre le 18 juillet et le 14 septembre 2012. Veuillez sélectionner une date comprise dans cette période.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.JourneyWebNoResults', 'Désolé, l''outil de planification de parcours ne peut pas fournir d''options de trajet par transport en commun à partir des informations que vous avez données. Essayez à nouveau en utilisant une date ou heure différente.'

-- Public Transport
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPInternalError', 'Sorry, the journey planner is currently unable to obtain journey options using the details you have provided. You may wish to try again using a different date or time.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPInternalError', 'Désolé, l''outil de planification de parcours ne peut pas fournir d''options de trajet à partir des informations que vous avez données. Essayez à nouveau en utilisant une date ou heure différente.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPNoResults', 'Sorry, the journey planner is currently unable to obtain public transport journey options using the details you have provided. You may wish to try again using a different date or time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again using a different date or time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again using a different date or time.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPNoResults.Car', 'The journey planner is unable to plan your car journey using the details you have entered. This may be because your start point is on a road with access restrictions - please check and try again.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.Car', 'The journey planner is unable to plan a complete set of car journeys using the details you have entered.  This may be because your start point is on a road with access restrictions - please check and try again.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.Car', 'The journey planner is unable to plan a complete set of car journeys using the details you have entered.  This may be because your start point is on a road with access restrictions - please check and try again.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPNoResults.WheelchairAssistance', 'Sorry, the journey planner is currently unable to obtain public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station that has assistance available.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.WheelchairAssistance', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station that has assistance available.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.WheelchairAssistance', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station.that has assistance available.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPNoResults.Wheelchair', 'Sorry, the journey planner is currently unable to obtain public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.Wheelchair', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.Wheelchair', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPNoResults.Assistance', 'Sorry, the journey planner is currently unable to obtain public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station that has assistance available.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.Assistance', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station that has assistance available.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.Assistance', 'Sorry, the journey planner is currently unable to obtain a complete set of public transport journey options using the details you have provided. You may wish to try again starting your journey from an accessible station.that has assistance available.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPNoResults', 'Désolé, l''outil de planification de parcours ne peut pas fournir d''options de trajet par transport en commun à partir des informations que vous avez données. Essayez à nouveau en utilisant une date ou heure différente.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound', 'Désolé, l''outil de planification de parcours ne peut pas fournir d''options complètes de trajet par transport en commun à partir des informations que vous avez données. Essayez à nouveau en utilisant une date ou heure différente.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound', 'Désolé, l''outil de planification de parcours ne peut pas fournir d''options complètes de trajet par transport en commun à partir des informations que vous avez données. Essayez à nouveau en utilisant une date ou heure différente.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPNoResults.Car', 'L''outil de planification de parcours ne peut pas planifier votre trajet en voiture à partir des informations que vous avez saisies. Le problème vient peut-être du fait que votre point de départ est situé sur une route avec restrictions d''accès - veuillez vérifier et réessayer.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.Car', 'L''outil de planification de parcours ne peut pas planifier de trajets complets en voiture à partir des informations que vous avez saisies. Le problème vient peut-être du fait que votre point de départ est situé sur une route avec restrictions d''accès - veuillez vérifier et réessayer.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.Car', 'L''outil de planification de parcours ne peut pas planifier de trajets complets en voiture à partir des informations que vous avez saisies. Le problème vient peut-être du fait que votre point de départ est situé sur une route avec restrictions d''accès - veuillez vérifier et réessayer.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPNoResults.WheelchairAssistance', 'Désolé, l''outil de planification de trajet ne peut actuellement pas obtenir les options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible avec assistance.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.WheelchairAssistance', 'Désolé, l''outil de planification ne peut actuellement pas obtenir l''ensemble des options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible avec assistance.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.WheelchairAssistance', 'Désolé, l''outil de planification ne peut actuellement pas obtenir l''ensemble des options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible avec assistance.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPNoResults.Wheelchair', 'Désolé, l''outil de planification de trajet ne peut actuellement pas obtenir les options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.Wheelchair', 'Désolé, l''outil de planification ne peut actuellement pas obtenir l''ensemble des options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.Wheelchair', 'Désolé, l''outil de planification ne peut actuellement pas obtenir l''ensemble des options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPNoResults.Assistance', 'Désolé, l''outil de planification de trajet ne peut actuellement pas obtenir les options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible avec assistance.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.OutwardFound.Assistance', 'Désolé, l''outil de planification ne peut actuellement pas obtenir l''ensemble des options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible avec assistance.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPPartialResults.ReturnFound.Assistance', 'Désolé, l''outil de planification ne peut actuellement pas obtenir l''ensemble des options de transports en commun que vous avez demandées. Vous pouvez redéfinir votre lieu de départ depuis une station accessible avec assistance.'

-- Cycle
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CyclePlannerInternalError', 'The journey planner is unable to plan a cycle journey using the details you have entered. This may be because your start point is on a road with access restrictions - please check and try again.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CyclePlannerPartialReturn', 'The journey planner is unable to plan a complete set of cycle journeys. This may be because your start point is on a road with access restrictions - please check and try again.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CyclePlannerNoResults', 'The journey planner is unable to plan a cycle journey using the details you have entered. This may be because your start point is on a road with access restrictions - please check and try again.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CyclePlannerInternalError', 'L''outil de planification de parcours ne peut pas planifier un trajet en vélo à partir des informations que vous avez saisies. Le problème vient peut-être du fait que votre point de départ est situé sur une route avec restrictions d''accès - veuillez vérifier et réessayer.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CyclePlannerPartialReturn', 'L''outil de planification de parcours ne peut pas planifier de trajets complets en vélo.  Le problème vient peut-être du fait que votre point de départ est situé sur une route avec restrictions d''accès - veuillez vérifier et réessayer.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CyclePlannerNoResults', 'L''outil de planification de parcours ne peut pas planifier un trajet en vélo à partir des informations que vous avez saisies. Le problème vient peut-être du fait que votre point de départ est situé sur une route avec restrictions d''accès - veuillez vérifier et réessayer.'

-- Stop event
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPStopEventInternalError', 'The journey planner could not find any river services for your selected route on the requested date of travel. Please select a new date or plan your journey using other public transport.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPStopEventPartialReturn', 'The journey planner could not find any river services for your selected route on the requested date of travel. Please select a new date or plan your journey using other public transport.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPStopEventNoResults', 'The journey planner could not find any river services for your selected route on the requested date of travel. Please select a new date or plan your journey using other public transport.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPStopEventNoEarlierResults', 'Sorry, there are no earlier services on {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPStopEventNoLaterResults', 'Sorry, there are no later services on {0}'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPStopEventInternalError', 'L''outil de planification de parcours n''a trouvé aucun service de navettes fluviales pour l''itinéraire sélectionné à la date demandée. Veuillez choisir une autre date ou prévoir l''utilisation d''un autre moyen de transport en commun.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPStopEventPartialReturn', 'L''outil de planification de parcours n''a trouvé aucun service de navettes fluviales pour l''itinéraire sélectionné à la date demandée. Veuillez choisir une autre date ou prévoir l''utilisation d''un autre moyen de transport en commun.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPStopEventNoResults', 'L''outil de planification de parcours n''a trouvé aucun service de navettes fluviales pour l''itinéraire sélectionné à la date demandée. Veuillez choisir une autre date ou prévoir l''utilisation d''un autre moyen de transport en commun.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPStopEventNoEarlierResults', 'Désolé, il n''y a pas de services plus tôt le {0}'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CJPStopEventNoLaterResults', 'Désolé, il n''y a pas de services plus tard le {0}'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CJPOvernightJourneyRejected','All journeys returned were deemed to have awkward overnight interchanges.  Please try a different arrival / departure time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CycleParkIsClosed.Leave','{0} closes at {1}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CycleParkIsClosed.Arrive','{0} opens at {1}'

------------------------------------------------------------------------------------------------------------------
-- Journey Header
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.JourneyDirection.Outward.Text','Outward'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.JourneyDirection.Return.Text','Return'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.JourneyHeader.Outward.Text','{0} to {1}'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.JourneyHeader.Return.Text','{0} to {1}'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.JourneyHeader.ArriveBy.Text','Arriving by {0}'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.JourneyHeader.LeavingAt.Text','Leaving at {0}'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.JourneyDirection.Outward.Text','Aller'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.JourneyDirection.Return.Text','Retour'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.JourneyHeader.Outward.Text','{0} et {1}'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.JourneyHeader.Return.Text','{0} et {1}'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.JourneyHeader.ArriveBy.Text','	Arrivant par {0}'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.JourneyHeader.LeavingAt.Text','Laissant à {0}'

------------------------------------------------------------------------------------------------------------------
-- Journey Summary
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.HeaderRow.Transport.Text','Transport'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.HeaderRow.Changes.Text','Changes'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.HeaderRow.Leave.Text','Leave'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.HeaderRow.Arrive.Text','Arrive'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.HeaderRow.JourneyTime.Text','Journey Time'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.HeaderRow.Select.Text',''

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.HeaderRow.Transport.Text','Transport'	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.HeaderRow.Changes.Text','Correspondances'	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.HeaderRow.Leave.Text','Départ'	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.HeaderRow.Arrive.Text','Arrivée'	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.HeaderRow.JourneyTime.Text','Durée du trajet'	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.HeaderRow.Select.Text',''

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.DetailsRow.BtnClose.Text','Close'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.DetailsRow.BtnClose.ToolTip','Close'	

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.DetailsRow.BtnClose.Text','Fermer'	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.DetailsRow.BtnClose.ToolTip','Fermer'	

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SummaryRow.JourneysCount.Text','Journey options' -- Used for SJPMobile
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SummaryRow.JourneyCount.Text','Journey options' -- Used for SJPMobile
	
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.SummaryRow.JourneysCount.Text','Options du trajet' -- Used for SJPMobile
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.SummaryRow.JourneyCount.Text','Options du trajet' -- Used for SJPMobile

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SelectJourney.Show.Text','View' -- Used for SJPMobile
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.SelectJourney.Show.Text','Afficher' -- Used for SJPMobile

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SelectJourney.Close.ImageUrl','arrows/right_arrow.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SelectJourney.Open.ImageUrl','arrows/down_arrow.png'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SelectJourney.AlternateText','Select this journey'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.SelectJourney.ToolTip','Click to view this journey'	

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.Detail.Close.ImageUrl','arrows/right_arrow.png'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.Detail.Open.ImageUrl','arrows/down_arrow.png'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.Detail.Close.AlternateText','Show details'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.Detail.Open.AlternateText','Hide details'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.Detail.Close.ToolTip','Click to show details'	
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.Detail.Open.ToolTip','Click to hide details'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.EarlierJourney.Outward.Text','Need an earlier journey?'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.LaterJourney.Outward.Text','Need a later journey?'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.EarlierJourney.Return.Text','Need an earlier journey?'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.LaterJourney.Return.Text','Need a later journey?'

EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.EarlierService.Outward.Text','Get earlier river services'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.LaterService.Outward.Text','Get later river services'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.EarlierService.Return.Text','Get earlier river services'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyOutput.LaterService.Return.Text','Get later river services'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.EarlierJourney.Outward.Text','Besoin d''arriver plus tôt?'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.LaterJourney.Outward.Text','Besoin de partir plus tard?'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.EarlierJourney.Return.Text','Besoin d''arriver plus tôt?'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.LaterJourney.Return.Text','Besoin de partir plus tard?'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.EarlierService.Outward.Text','Trouvez une navette fluviale antérieure'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.LaterService.Outward.Text','Trouver une navette fluviale ultérieure'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.EarlierService.Return.Text','Trouvez une navette fluviale antérieure'
	EXEC AddContent @Group, @CultFr, @Collection,'JourneyOutput.LaterService.Return.Text','Trouver une navette fluviale ultérieure'

------------------------------------------------------------------------------------------------------------------
-- Journey Text
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Take', 'Take '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TakeThe', 'Take the '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Or', 'or '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Towards', 'towards '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.WalkTo', 'Walk about {0} to {1}{2}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.WalkTo.Allowance', ''
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.WalkInterchangeTo', 'Interchange to {0}, please allow about {1}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TransferTo', 'Transfer to '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TaxiTo', 'Take a taxi to '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.To', 'to '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Then', 'then '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Drive', 'Drive'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DriveExpand', 'See full car route instructions'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Cycle', 'Cycle'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.Cycle', 'Vélo'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.CycleExpand', 'See full cycle route instructions'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Leave', 'Leave'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Depart', 'Depart'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Arrive', 'Arrive'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.CheckIn', 'Check-in'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.Leave', 'Partir'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.Depart', 'Départ'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.Arrive', 'Arriver'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DurationMax', 'max: {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DurationTypical', 'typical: {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Hours', 'hrs'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Hour', 'hr'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Minutes', 'mins'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Minute', 'min'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Seconds', 'seconds'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.FrequencyDuration', 'Runs every {0}'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.Km', 'km'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TransferMode.From', 'Transfer from {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TransferMode.To', 'Transfer to {0}'

-- Check constraints
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.CheckConstraint.securityCheck', 'security check queue'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.CheckConstraint.baggageSecurityCheck', 'security check queue'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.CheckConstraint.egress', 'exit'

-- Check constraints (Queue not shown)
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Outward', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Outward', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Return', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Return', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Outward', 'N''oubliez pas - il y aura une forte affluence  dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Outward', '<a href="http://fr.london2012.com/fr/spectators/venues">Avez-vous vérifié l''heure d''arrivée recommandée sur votre site ? </a> N''oubliez pas - il y aura une forte affluence dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Return', 'N''oubliez pas - il y aura une forte affluence  dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Return', '<a href="http://fr.london2012.com/fr/spectators/venues">Avez-vous vérifié l''heure d''arrivée recommandée sur votre site ? </a> N''oubliez pas - il y aura une forte affluence dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'

-- Check constraints - Accessible (Queue not shown )
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Outward', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel. Inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Outward', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Once inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Return', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel. Inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Return', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Once inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/Paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Outward', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel. Inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/fr/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Outward', '<a href="http://fr.london2012.com/fr/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Once inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/fr/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Return', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel. Inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/fr/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Return', '<a href="http://fr.london2012.com/fr/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Once inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://www.london2012.com/fr/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'

-- Check constraints (Queue shown)
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Outward', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Additional time</a> in your journey has been allowed for the time needed to exit your venue. However, you may wish to allow for even more time in your journey plan. To do this, please re-plan your journey with a later departure time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Outward', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Additional time</a> in your journey has been allowed for airport-style security and unforeseen delays in the transport network. However, you may wish to allow for additional time in your journey plan. To do this, please re-plan your journey with an earlier arrival time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Return', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Additional time</a> in your journey has been allowed for the time needed to exit your venue. However, you may wish to allow for even more time in your journey plan. To do this, please re-plan your journey with a later departure time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Return', '<a href="http://www.london2012.com/Paralympics/spectators/venues">Additional time</a> in your journey has been allowed for airport-style security and unforeseen delays in the transport network. However, you may wish to allow for additional time in your journey plan. To do this, please re-plan your journey with an earlier arrival time.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Outward', '<a href="http://fr.london2012.com/fr/spectators/venues">Additional time</a> in your journey has been allowed for the time needed to exit your venue. However, you may wish to allow for even more time in your journey plan. To do this, please re-plan your journey with a later departure time.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Outward', '<a href="http://fr.london2012.com/fr/spectators/venues">Additional time</a> in your journey has been allowed for airport-style security and unforeseen delays in the transport network. However, you may wish to allow for additional time in your journey plan. To do this, please re-plan your journey with an earlier arrival time.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Return', '<a href="http://fr.london2012.com/fr/spectators/venues">Additional time</a> in your journey has been allowed for the time needed to exit your venue. However, you may wish to allow for even more time in your journey plan. To do this, please re-plan your journey with a later departure time.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Return', '<a href="http://fr.london2012.com/fr/spectators/venues">Additional time</a> in your journey has been allowed for airport-style security and unforeseen delays in the transport network. However, you may wish to allow for additional time in your journey plan. To do this, please re-plan your journey with an earlier arrival time.'

-- Check constraints - Mobile (Queue not shown)
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Outward.Mobile', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Outward.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues/">Have you checked the recommended arrival time for your venue?</a> Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Return.Mobile', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Return.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues/">Have you checked the recommended arrival time for your venue?</a> Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Outward.Mobile', 'N''oubliez pas - il y aura une forte affluence  dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Outward.Mobile', '<a href="http://m.london2012.com/spectators/venues/">Avez-vous vérifié l''heure d''arrivée recommandée sur votre site ? </a> N''oubliez pas - il y aura une forte affluence dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Return.Mobile', 'N''oubliez pas - il y aura une forte affluence  dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Return.Mobile', '<a href="http://m.london2012.com/spectators/venues/">Avez-vous vérifié l''heure d''arrivée recommandée sur votre site ? </a> N''oubliez pas - il y aura une forte affluence dans les transports en commun pendant les Jeux. Prévoyez des délais supplémentaires pour vos trajets.'

-- Check constraints - Mobile Accessible (Queue not shown )
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Outward.Mobile', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel. Inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://m.london2012.com/paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Outward.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Once inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://m.london2012.com/paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Return.Mobile', 'Remember – the transport network will be extremely busy during the Games so you may want to allow additional time for travel. Inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://m.london2012.com/paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Return.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues">Have you checked the recommended arrival time for your venue?</a> Once inside a venue, disabled spectators will be able to make use of Games Mobility for help with getting around, <a href="http://m.london2012.com/paralympics/spectators/travel/accessible-travel/">read more about accessibility in the venue</a>.'

-- Check constraints - Mobile (Queue shown)
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Outward.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues">Additional time</a> in your journey has been allowed for the time needed to exit your venue. However, you may wish to allow for even more time in your journey plan. To do this, please re-plan your journey with a later departure time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Outward.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues">Additional time</a> in your journey has been allowed for airport-style security and unforeseen delays in the transport network. However, you may wish to allow for additional time in your journey plan. To do this, please re-plan your journey with an earlier arrival time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Return.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues">Additional time</a> in your journey has been allowed for the time needed to exit your venue. However, you may wish to allow for even more time in your journey plan. To do this, please re-plan your journey with a later departure time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Return.Mobile', '<a href="http://m.london2012.com/paralympics/spectators/venues">Additional time</a> in your journey has been allowed for airport-style security and unforeseen delays in the transport network. However, you may wish to allow for additional time in your journey plan. To do this, please re-plan your journey with an earlier arrival time.'


-- GPX Download link 
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.GPXDownLoad','Download a GPX file of your route'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.GPXInfo','The GPX link allows you to download your cycle route to your computer. You can then load this file onto other applications or devices, such as GPS receivers.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOptions.GPXInfoImage.AlternateText','The GPX link allows you to download your cycle route to your computer. You can then load this file onto other applications or devices, such as GPS receivers.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOptions.GPXInfoImage.ImageUrl','presentation/information.png'
	
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.GPXDownLoad','Télécharger une version GPX de votre itinéraire'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.GPXInfo','Le lien GPX vous permet de télécharger votre parcours à vélo sur votre ordinateur. Vous pouvez ensuite charger ce fichier sur d''autres applications ou appareils, tels que des récepteurs GPS.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOptions.GPXInfoImage.AlternateText','Le lien GPX vous permet de télécharger votre parcours à vélo sur votre ordinateur. Vous pouvez ensuite charger ce fichier sur d''autres applications ou appareils, tels que des récepteurs GPS.'

-- Javascript links for retailer handoff and additional travel notes
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TravelNotesLink','Additional travel notes'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.BookTicketLink','Book {0} travel'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.BookTicketLink.Accessible','Book {0} travel and assistance requests'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.BookTicketLink.CoachAndRail','Book combined coach and rail travel'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.BookTicketInfo','Find out about booking this {0} journey '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Url.BookTicketInfo', 'http://www.london2012.com/spectators/travel/book-your-travel/'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.TravelcardLink','This part of your journey is covered by the Games Travelcard'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Url.TravelcardLink', 'http://www.london2012.com/paralympics/spectators/travel/games-travelcard/'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Url.TravelcardLink.Mobile', 'http://m.london2012.com/paralympics/spectators/travel/games-travelcard/'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.BookTicketLink','Réserver votre voyage en {0}'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Url.BookTicketInfo', 'http://www.london2012.com/fr/spectators/travel/book-your-travel/'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.TravelcardLink','Cette section du trajet est couverte par la carte de transport Games Travelcard'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Url.TravelcardLink', 'http://www.london2012.com/fr/spectators/travel/games-travelcard/'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.BookTicketPhone','To book this {0} journey please phone {1}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.BookTicketPhone.CoachAndRail','To book this combined coach and rail travel please phone {0}'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.BookTicketPhone','Pour réserver ce trajet, appelez le {1}'

-- Accessible journey
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AccessibleInfo','Assistance requests must be booked in advance'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.AccessibleLink','Find out about accessibility at {0}'

-- Maps link
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.VenueMapLink','Venue map'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.VenueMapLinkToolTip','Venue map for {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DirectionsMapLink.Cycle','View map of cycle route'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DirectionsMapLinkToolTip.Cycle','View map of cycle route'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DirectionsMapLink.Walk','View map of walking route'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Text.DirectionsMapLinkToolTip.Walk','View map of walking route'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.VenueMapLink','Plan du site'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.VenueMapLinkToolTip','Voir le plan de {0}'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.DirectionsMapLink.Cycle','Voir un plan des trajets à vélo'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Text.DirectionsMapLinkToolTip.Cycle','Voir un plan des trajets à vélo'

------------------------------------------------------------------------------------------------------------------
-- Journey Images
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Start.ImageURL', 'presentation/Journey_Start.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Start.Medium.ImageURL', 'presentation/Journey_Start_Medium.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.End.ImageURL', 'presentation/Journey_End.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.End.Medium.ImageURL', 'presentation/Journey_End_Medium.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Node.ImageURL', 'presentation/Journey_Node.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Node.Medium.ImageURL', 'presentation/Journey_Node_Medium.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Node.Long.ImageURL', 'presentation/Journey_Node_Long.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Node.ExtraLong.ImageURL', 'presentation/Journey_Node_ExtraLong.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.LineSolid.ImageURL', 'presentation/Journey_Line_Solid.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.LineDotted.ImageURL', 'presentation/Journey_Line_Dot.png'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Start.AltText', 'Start'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.End.AltText', 'End'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.Node.AltText', 'Change'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.LineSolid.AltText', 'Leg'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Image.LineDotted.AltText', 'Leg'

-- Javascript links for retailer handoff and additional travel notes
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.TravelNotesLink.ImageUrl', 'presentation/information-red.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.BookTicketLink.ImageUrl', 'presentation/Ticket_pink.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.BookTicketInfo.ImageUrl', 'presentation/Ticket_blue.png'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.TravelNotesLink.AlternateText', 'Additional travel notes'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.BookTicketLink.AlternateText', 'Book your {0} journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.BookTicketLink.Accessible.AlternateText', 'Book your {0} journey and assistance requests'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.BookTicketInfo.AlternateText', 'Find out about booking this {0} journey'

------------------------------------------------------------------------------------------------------------------
-- Vehicle Features Icons
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.0', 'presentation/vehiclefeatures/img_0_Buffet_Service.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.1', 'presentation/vehiclefeatures/img_1_Restaurant_in_First_Class_only_and_Buffet_Service.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.2', 'presentation/vehiclefeatures/img_2_Hot_Buffet.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.3', 'presentation/vehiclefeatures/img_3_Meal_Included_for_First_Class_Passengers.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.4', 'presentation/vehiclefeatures/img_4_Restaurant_and_Buffet_Service.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.5', 'presentation/vehiclefeatures/img_5_Trolley_Service.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.10', 'presentation/vehiclefeatures/img_10_Reservations_Compulsory.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.11', 'presentation/vehiclefeatures/img_11_Reservations_Recommended.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.12', 'presentation/vehiclefeatures/img_12_Reservations_Available.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.13', 'presentation/vehiclefeatures/img_13_Reservations _for_Cycles.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.21', 'presentation/vehiclefeatures/img_21_First_and_Standard_Class.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.22', 'presentation/vehiclefeatures/img_22_First_Class_Only.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.30', 'presentation/vehiclefeatures/img_30_First_and_Standard_Class_Sleepers.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.31', 'presentation/vehiclefeatures/img_31_First_Class_Sleepers_Only.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.ImageURL.32', 'presentation/vehiclefeatures/img_32_Standard_Class_Sleepers_Only.gif'

EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.0', 'Buffet service'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.1', 'Restaurant in first class only and Buffet service'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.2', 'Hot Buffet'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.3', 'Meal included for first class passengers'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.4', 'Restaurant and Buffet service'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.5', 'Trolley service'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.10', 'Reservations compulsory'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.11', 'Reservations recommended'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.12', 'Reservations available'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.13', 'Reservations for cycles'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.21', 'First and standard class'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.22', 'First class only'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.30', 'First and standard class sleepers'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.31', 'First class sleepers only'
EXEC AddContent @Group, @CultEn, @Collection, 'RailVehicleFeaturesIcon.AltTextToolTip.32', 'Standard class sleepers only'

--------------------------------------------------------------------------------------------------------------------
-- Accessible Features Icons
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceAssistanceBoarding', 'presentation/accessiblefeatures/Accessible_assistance.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceAssistanceWheelchair', 'presentation/accessiblefeatures/Accessible_wheelchair_assistance_2.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceAssistanceWheelchairBooked', 'presentation/accessiblefeatures/Accessible_wheelchair_assistance_2.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceAssistancePorterage', 'presentation/accessiblefeatures/Accessible_assistance.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceAssistanceOther', 'presentation/accessiblefeatures/Accessible_assistance.png'

EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceLowFloor', 'presentation/accessiblefeatures/Accessible_low_floor_vehicle.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.ServiceWheelchairBookingRequired', 'presentation/accessiblefeatures/Accessible_walk_2.png'

--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.EscalatorFreeAccess', 'presentation/accessiblefeatures/Accessible_EscalatorFreeAccess.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.LiftFreeAccess', 'presentation/accessiblefeatures/Accessible_LiftFreeAccess.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.MobilityImpairedAccess', 'presentation/accessiblefeatures/Accessible_walk_2.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.StepFreeAccess', 'presentation/accessiblefeatures/Accessible_StepFreeAccess.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.WheelchairAccess', 'presentation/accessiblefeatures/Accessible_walk_2.png'

EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessLiftUp', 'presentation/accessiblefeatures/Accessible_lift_up.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessLiftDown', 'presentation/accessiblefeatures/Accessible_lift_down.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessStairsUp', 'presentation/accessiblefeatures/Accessible_stairs_up.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessStairsDown', 'presentation/accessiblefeatures/Accessible_stairs_down.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessSeriesOfStairs', 'presentation/accessiblefeatures/Accessible_stairs_up.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessEscalatorUp', 'presentation/accessiblefeatures/Accessible_escalator_up.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessEscalatorDown', 'presentation/accessiblefeatures/Accessible_escalator_down.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessTravelator', 'presentation/accessiblefeatures/Accessible_AccessTravelator.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessRampUp', 'presentation/accessiblefeatures/Accessible_ramp_up.png'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessRampDown', 'presentation/accessiblefeatures/Accessible_ramp_down.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessShuttle', 'presentation/accessiblefeatures/Accessible_AccessShuttle.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessBarrier', 'presentation/accessiblefeatures/Accessible_AccessBarrier.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessNarrowEntrance', 'presentation/accessiblefeatures/Accessible_AccessNarrowEntrance.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessConfinedSpace', 'presentation/accessiblefeatures/Accessible_AccessConfinedSpace.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessQueueManagement', 'presentation/accessiblefeatures/Accessible_AccessQueueManagement.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessNone', 'presentation/accessiblefeatures/Accessible_AccessNone.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessUnknown', 'presentation/accessiblefeatures/Accessible_AccessUnknown.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessOther', 'presentation/accessiblefeatures/Accessible_AccessOther.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessOpenSpace', 'presentation/accessiblefeatures/Accessible_AccessOpenSpace.gif'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessStreet', 'presentation/accessiblefeatures/Accessible_walk.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessPavement', 'presentation/accessiblefeatures/Accessible_walk.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessFootpath', 'presentation/accessiblefeatures/Accessible_walk.png'
--EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.ImageURL.AccessPassage', 'presentation/accessiblefeatures/Accessible_passage.png'

EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceAssistanceBoarding', 'Assistance with boarding and alighting'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceAssistanceWheelchair', 'Assistance for wheelchair users'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceAssistanceWheelchairBooked', 'Assistance for wheelchair users'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceAssistancePorterage', 'Assistance with luggage'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceAssistanceOther', 'Assistance available'

EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceLowFloor', 'Service has a low floor'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.ServiceWheelchairBookingRequired', 'Booking required for wheelchair users'

EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.EscalatorFreeAccess', 'Escalator free access'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.LiftFreeAccess', 'Lift free access'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.MobilityImpairedAccess', 'Access for wheelchair users' -- same as WheelchairAccess
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.StepFreeAccess', 'Step free access'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.WheelchairAccess', 'Access for wheelchair users'

EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessLiftUp', 'Lift up'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessLiftDown', 'Lift down'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessStairsUp', 'Stairs up'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessStairsDown', 'Stairs down'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessSeriesOfStairs', 'Series of stairs'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessEscalatorUp', 'Escalator up'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessEscalatorDown', 'Escalator down'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessTravelator', 'Travelator'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessRampUp', 'Ramp up'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessRampDown', 'Ramp down'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessShuttle', 'Shuttle'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessBarrier', 'Barrier'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessNarrowEntrance', 'Narrow entrance'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessConfinedSpace', 'Confined space'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessQueueManagement', 'Queue management'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessNone', 'No access'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessUnknown', 'Unknown access'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessOther', 'Other access'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessOpenSpace', 'Open space'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessStreet', 'Street'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessPavement', 'Pavement'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessFootpath', 'Footpath'
EXEC AddContent @Group, @CultEn, @Collection, 'AccessibleFeaturesIcon.AltTextToolTip.AccessPassage', 'Passage'

------------------------------------------------------------------------------------------------------------------
-- Venue Map Images
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100OPK.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100AQC.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100STA.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100VEL.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100BBA.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HOC.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100AWP.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HBA.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100BMX.Url','maps/VenueMaps/travel-to-olympic-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETM.Url','maps/VenueMaps/travel-to-olympic-park-o.png'

EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EXL.Url','maps/VenueMaps/travel-to-excel-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EXLN1.Url','maps/VenueMaps/travel-to-excel-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EXLN2.Url','maps/VenueMaps/travel-to-excel-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EXLS1.Url','maps/VenueMaps/travel-to-excel-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EXLS2.Url','maps/VenueMaps/travel-to-excel-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EXLS3.Url','maps/VenueMaps/travel-to-excel-o.png'

EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100BXH.Url','maps/VenueMaps/travel-to-box-hill-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100NGA.Url','maps/VenueMaps/travel-to-north-greenwich-arena-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100RAB.Url','maps/VenueMaps/travel-to-royal-artillery-barracks-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100EAR.Url','maps/VenueMaps/travel-to-earls-court-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100LCG.Url','maps/VenueMaps/travel-to-lords-cricket-ground-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WIM.Url','maps/VenueMaps/travel-to-wimbeldon-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HGP.Url','maps/VenueMaps/travel-to-horse-guards-parade-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HYD.Url','maps/VenueMaps/travel-to-hyde-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WEA.Url','maps/VenueMaps/travel-to-wembley-arena-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100LVC.Url','maps/VenueMaps/travel-to-lee-valley-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HAD.Url','maps/VenueMaps/travel-to-hadleigh-farm-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HAP.Url','maps/VenueMaps/travel-to-hampton-court-palace-o.png'

EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WEM.Url','maps/VenueMaps/travel-to-wembley-stadium-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MIL.Url','maps/VenueMaps/travel-to-millennium-stadium-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HAM.Url','maps/VenueMaps/travel-to-hampden-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100OLD.Url','maps/VenueMaps/travel-to-old-trafford-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100COV.Url','maps/VenueMaps/travel-to-city-of-coventry-stadium-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100SJP.Url','maps/VenueMaps/travel-to-st-james-park-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100BND.Url','maps/VenueMaps/travel-to-brands-hatch-o.png'

EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100HPL.Url','maps/VenueMaps/travel-to-hyde-park-live.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100PFL.Url','maps/VenueMaps/travel-to-potters-field-live.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100TSL.Url','maps/VenueMaps/travel-to-trafalgar-square-live.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100VPL.Url','maps/VenueMaps/travel-to-victoria-park-live.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WOL.Url','maps/VenueMaps/travel-to-royal-artillery-barracks-o.png'

-- Venues with date specific maps:
-- Eton Dorney - default
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url','maps/VenueMaps/travel-to-eton-dorney-o.png'
-- Eton Dorney - 13 Aug to 14 Sept
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120813','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120814','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120815','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120816','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120817','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120818','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120819','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120820','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120821','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120822','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120823','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120824','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120825','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120826','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120827','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120828','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120829','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120830','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120831','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120901','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120902','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120903','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120904','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120905','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120906','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120907','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120908','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120909','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120910','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120911','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120912','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120913','maps/VenueMaps/travel-to-eton-dorney-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100ETD.Url.20120914','maps/VenueMaps/travel-to-eton-dorney-p.png'

-- Greenwich Park - default
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url','maps/VenueMaps/travel-to-greenwich-o.png'
-- Greenwich Park - 13 Aug to 14 Sep
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120813','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120814','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120815','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120816','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120817','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120818','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120819','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120820','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120821','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120822','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120823','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120824','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120825','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120826','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120827','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120828','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120829','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120830','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120831','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120901','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120902','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120903','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120904','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120905','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120906','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120907','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120908','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120909','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120910','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120911','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120912','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120913','maps/VenueMaps/travel-to-greenwich-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100GRP.Url.20120914','maps/VenueMaps/travel-to-greenwich-p.png'

-- The Mall
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url','maps/VenueMaps/travel-to-the-mall-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120813','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120814','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120815','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120816','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120817','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120818','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120819','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120820','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120821','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120822','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120823','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120824','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120825','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120826','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120827','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120828','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120829','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120830','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120831','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120901','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120902','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120903','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120904','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120905','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120906','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120907','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120908','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120909','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120910','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120911','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120912','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120913','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MAL.Url.20120914','maps/VenueMaps/travel-to-the-mall-p.png'

-- The Mall - North
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url','maps/VenueMaps/travel-to-the-mall-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120813','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120814','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120815','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120816','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120817','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120818','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120819','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120820','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120821','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120822','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120823','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120824','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120825','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120826','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120827','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120828','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120829','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120830','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120831','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120901','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120902','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120903','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120904','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120905','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120906','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120907','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120908','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120909','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120910','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120911','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120912','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120913','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLN.Url.20120914','maps/VenueMaps/travel-to-the-mall-p.png'

-- The Mall - South
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url','maps/VenueMaps/travel-to-the-mall-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120813','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120814','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120815','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120816','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120817','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120818','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120819','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120820','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120821','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120822','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120823','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120824','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120825','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120826','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120827','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120828','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120829','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120830','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120831','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120901','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120902','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120903','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120904','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120905','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120906','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120907','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120908','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120909','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120910','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120911','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120912','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120913','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLS.Url.20120914','maps/VenueMaps/travel-to-the-mall-p.png'

-- The Mall - East
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url','maps/VenueMaps/travel-to-the-mall-o.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120813','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120814','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120815','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120816','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120817','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120818','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120819','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120820','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120821','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120822','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120823','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120824','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120825','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120826','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120827','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120828','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120829','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120830','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120831','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120901','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120902','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120903','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120904','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120905','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120906','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120907','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120908','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120909','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120910','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120911','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120912','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120913','maps/VenueMaps/travel-to-the-mall-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100MLE.Url.20120914','maps/VenueMaps/travel-to-the-mall-p.png'

-- Weymouth and Portland - default
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url','maps/VenueMaps/travel-to-weymouth-and-portland-o.png'
-- Weymouth and Portland - 13 Aug to 14 Sept
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120813','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120814','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120815','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120816','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120817','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120818','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120819','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120820','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120821','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120822','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120823','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120824','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120825','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120826','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120827','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120828','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120829','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120830','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120831','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120901','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120902','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120903','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120904','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120905','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120906','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120907','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120908','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120909','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120910','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120911','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120912','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120913','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WAP.Url.20120914','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'

-- Weymouth Beach Live - default
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url','maps/VenueMaps/travel-to-weymouth-and-portland-o.png'
-- Weymouth Beach Live - 13 Aug to 14 Sept
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120813','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120814','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120815','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120816','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120817','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120818','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120819','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120820','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120821','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120822','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120823','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120824','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120825','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120826','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120827','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120828','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120829','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120830','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120831','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120901','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120902','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120903','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120904','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120905','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120906','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120907','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120908','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120909','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120910','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120911','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120912','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120913','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'
EXEC AddContent @Group, @CultEn, @Collection, 'Venue.VenueMapImage.8100WLB.Url.20120914','maps/VenueMaps/travel-to-weymouth-and-portland-p.png'

------------------------------------------------------------------------------------------------------------------
-- Details Car Control
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.TotalDistanceHeading.Text','Distance'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.TotalDurationHeading.Text','Duration'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.MajorRoadsHeading.Text','Main roads'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.HighTrafficSymbol.Url','presentation/roadexclamation.gif'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.HighTrafficSymbol.AlternateText','High traffic levels likely on this road'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.HighTrafficSymbol.ToolTip','High traffic levels likely on this road'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.DistanceHeading.Text','Distance (km)'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.InstructionHeading.Text','Instruction'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCarControl.ArriveHeading.Text','Arrive'

	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCarControl.TotalDistanceHeading.Text','Distance'
	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCarControl.TotalDurationHeading.Text','Durée'
	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCarControl.MajorRoadsHeading.Text','Routes principales'
	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCarControl.ArriveHeading.Text','Arriver'

------------------------------------------------------------------------------------------------------------------
-- Details Cycle Control
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.TotalDistanceHeading.Text','Distance'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.TotalDurationHeading.Text','Duration'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.DistanceHeading.Text','Distance (km)'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.InstructionHeading.Text','Instruction'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.ArriveHeading.Text','Arrive'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.Journey.Text','journey'
EXEC AddContent @Group, @CultEn, @Collection,'DetailsCycleControl.AverageCyclingSpeed.Text','Average cycling speed is {0}kph'

	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCycleControl.TotalDistanceHeading.Text','Distance'
	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCycleControl.TotalDurationHeading.Text','Durée'
	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCycleControl.ArriveHeading.Text','Arriver'
	EXEC AddContent @Group, @CultFr, @Collection,'DetailsCycleControl.AverageCyclingSpeed.Text','La vitesse moyenne à vélo est de {0}kph'

--------------------------------------------------------------------------------------------------------------------------------
-- Car and Cycle journey details - Instructions text
--------------------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.RouteJoins','Route joins {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.RouteLeavesCycleInfrastructure','Route leaves cycle specific infrastructure'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.RouteUses','Route uses'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.PleaseNote','Please note the route goes'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.PleaseCross','Please cross at'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.TheStreetIs','The {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.And','and'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.ForThisManoeuvre','For this manoeuvre we recommend that you'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftOne2','Take first available left '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftTwo2','Take second available left '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftThree2','Take third available left '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftFour2','Take fourth available left '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightOne2','Take first available right '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightTwo2','Take second available right '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightThree2','Take third available right '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightFour2','Take fourth available right '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.AtMiniRoundabout2','at mini-roundabout '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LocalPath','unnamed path '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Street','street '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Path','path '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ChargeAdultAndCycle','Charge for adult and cycle:'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.UTurn','Make a U-Turn on'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleRouteText.TimeBasedAccessRestriction','Time based access restriction:'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.BearRightToJoin','Bear right to join'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Board','Board'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.CertainTimes','applies to your journey at certain times.'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.CertainTimesNoCharge','A charge applies at certain times.'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Charge','Charge: '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.CongestionCharge','congestion charge zone'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Continue','Go on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ContinueFor',', continue for'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ContinueMiniRoundabout','At mini-roundabout continue on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.DepartingAt','departing at:'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.End','End'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Enter','Enter'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Exit','Exit'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.FerryCrossing','Ferry crossing'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Follow','Follow'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.For',', for'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.HighTraffic','High traffic levels likely on this road - see map'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ImmediatelyBearLeft','Immediately bear left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ImmediatelyBearRight','Immediately bear right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ImmediatelyTurnLeft','Immediately take first available left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ImmediatelyTurnLeftOnto','Immediately turn left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ImmediatelyTurnRight','Immediately take first available right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ImmediatelyTurnRightOnto','Immediately turn right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.IntermediateFerry','Arrive at intermediate ferry terminal'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Join','Join'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Leave','Starting from'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LeaveFerry','Leave ferry'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LeaveMotorway','leave motorway'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LeftMiniRoundabout','Turn left at mini-roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LeftMiniRoundabout2','Turn left at mini-roundabout'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LocalRoad','local road'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Miles','miles'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.NotApplicable','-'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.NotAvailable','Not available'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.OnTo','on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.PlanStop','Plan to stop for a 15 minute break every 2 hours on a long journey. This time is not included in the above itinerary.'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RightMiniRoundabout','Turn right at mini-roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RightMiniRoundabout2','Turn right at mini-roundabout'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitEight','Take eighth available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitFive','Take fifth available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitFour','Take fourth available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitNine','Take ninth available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitOne','Take first available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitSeven','Take seventh available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitSix','Take sixth available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitTen','Take tenth available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitThree','Take third available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.RoundaboutExitTwo','Take second available exit off roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.StraightOn','straight on'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ThroughRoute','Follow the road on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.To','to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Toll','Toll:'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.Towards','towards'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftFour','Take fourth available left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftInDistance','Turn left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftOne','Take first available left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftThree','Take third available left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftToJoin','Turn left to join'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnLeftTwo','Take second available left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightFour','Take fourth available right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightInDistance','Turn right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightOne','Take first available right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightThree','Take third available right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightToJoin','Turn right to join'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.TurnRightTwo','Take second available right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.UnspecifedWaitForFerry','Arrive at ferry terminal and wait for ferry (no timetable available)'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.UntilJunction','until junction'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.UTurnMiniRoundabout','U-turn at mini-roundabout on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.UTurnMiniRoundabout2','U-turn at mini-roundabout'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ViaArriveAt','Arrive at'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.WaitAtTerminal','Arrive at ferry terminal and wait'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.WaitForFerry','Arrive at ferry terminal and wait to board'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.WhereRoadSplits','where the road splits'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.ArriveAt','Arrive at '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.AtJunctionJoin','at junction'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.AtJunctionLeave','At junction'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.AtMiniRoundabout','At mini-roundabout '
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.BearingLeft','bearing left'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.BearingRight','bearing right'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.BearLeft','Bear left on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.BearLeftToJoin','Bear left to join'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.BearRight','Bear right on to'
EXEC AddContent @Group, @CultEn, @Collection, 'RouteText.LondonCongestionChargeAdditionalText','<br><strong><font color="RED">Note: </font></strong>The London Congestion Charging Zone is changing on the 4th January 2011.  Transport Direct will reflect the new zone and charges from late December 2010.  Meanwhile please <a href="http://www.tfl.gov.uk/roadusers/congestioncharging/17094.aspx" target="_blank">Click here for more detail <img src="/Web2/App_Themes/TransportDirect/images/gifs/newwindow.gif" alt="(opens in new window)" title="(opens in new window)"></a>.'

--------------------------------------------------------------------------------------------------------------------------------
-- Add Cycle journey details - Cycle Attributes list
--------------------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.NoCycleAttributes','NoCycleAttributes'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Motorway','motorway'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.ARoad','A road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.BRoad','B road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.MinorRoad','minor road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LocalStreet','local street'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Alley','alley'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PrivateRoad','private road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PedestrianisedStreet','a pedestrianised street'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TollRoad','toll road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.SingleCarriageway','single carriageway'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.DualCarriageway','dual carriageway'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.SlipRoad','slip road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Roundabout','roundabout'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.EnclosedTrafficAreaLink','EnclosedTrafficAreaLink'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TrafficIslandLinkAtJunction','TrafficIslandLinkAtJunction'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TrafficIslandLink','TrafficIslandLink'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Ferry','ferry'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LimitedAccess','limited access'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.ProhibitedAccess','prohibited access'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Footpath','footpath'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Cyclepath','cyclepath'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Bridlepath','bridlepath'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CandriveAtoB','CandriveAtoB'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CandriveBtoA','CandriveBtoA'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CanenteratAi.e.noentryatB','CanenteratAi.e.noentryatB'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CanenteratBi.e.noentryatA','CanenteratBi.e.noentryatA'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Superlink','Superlink'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TrunkRoad','trunk road'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TurnSuperlink','TurnSuperlink'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.ConnectingLink','ConnectingLink'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Towpath','towpath'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient2-AtoBuphill','Gradient2-AtoBuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient2-BtoAuphill','Gradient2-BtoAuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient3-AtoBuphill','Gradient3-AtoBuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient3-BtoAuphill','Gradient3-BtoAuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient4-AtoBuphill','Gradient4-AtoBuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient4-BtoAuphill','Gradient4-BtoAuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient5-AtoBuphill','Gradient5-AtoBuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gradient5-BtoAuphill','Gradient5-BtoAuphill'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Ford','through a ford'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Gate','through a gate'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LevelCrossing','across a level crossing'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Bridge','over a bridge'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Tunnel','through a tunnel'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CalmingUnavoidablebyBike','CalmingUnavoidablebyBike'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Footbridge','over a footbridge'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Unused','unused'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.SharedUseFootpath','a shared use footpath'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.FootpathOnly','a footpath - please walk your bike'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CyclesOnly','a cycle only path'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PrivateAccess','is private access'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Parkland','through a park'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Subway','through a subway'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.RaisableBarrier','past a raisable barrier'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.HoopLiftBarrier','over a hoop barrier'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CattleGrid','across a cattle grid'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Stile','over a stile'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.HoopThroughBarrier','over a hoop barrier'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Humps','over traffic calming humps'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Cushions','over traffic calming cushions'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Chicane','through a chicane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PinchPoint','through a pinch point'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Pelican','pelican crossing'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Toucan','toucan crossing'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Zebra','zebra crossing'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.WalkaboutManoeuvre','At this point you may need to walk your bike'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.AdvancedManoeuvre','Extra care should be taken with the road junctions at this point'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.ProhibitedManoeuvre','ProhibitedManoeuvre'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CycleLaneAtoB','an on road cycle lane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CycleLaneBtoA','an on road cycle lane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.BusLaneAtoB','a shared use bus lane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.BusLaneBtoA','a shared use bus lane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.NarrowAtoB','a narrow on road cycle lane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.NarrowBtoA','a narrow on road cycle lane'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.DedicatedAtoB','a dedicated cycle track'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.DedicatedBtoA','a dedicated cycle track'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.UnpavedAtoB','is unpaved'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.UnpavedBtoA','is unpaved'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.WhenDryAtoB','may be wet in winter'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.WhenDryBtoA','may be wet in winter'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.FirmAtoB','is firm'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.FirmBtoA','is firm'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PavedAtoB','is paved'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PavedBtoA','is paved'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LooseAtoB','has a loose surface'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LooseBtoA','has a loose surface'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CobblesAtoB','is cobbled'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.CobblesBtoA','is cobbled'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.MixedAtoB','MixedAtoB'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.MixedBtoA','MixedBtoA'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.RoughAtoB','has a rough surface'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.RoughBtoA','has a rough surface'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.BlocksAtoB','has blocks'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.BlocksBtoA','has blocks'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.IndividualRecommendation','IndividualRecommendation'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LARecommended','LARecommended'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.RecommendedForSchools','RecommendedForSchools'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.OtherRecommendation','OtherRecommendation'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.WellLit','is well lit'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.LightingPresent','has street lighting'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.PartialLighting','is partially lit'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Nolighting','has no lighting'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Busy','is generally busy'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Very','is very'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Quiet','is generally quiet'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TrafficFree','is traffic free'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.SeldomPolicedUrbanArea','SeldomPolicedUrbanArea'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.IsolatedArea','is in an isolated area'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.NeighbourhoodWatch','has a neighbourhood watch scheme'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Cctv MonitoredArea','is a monitored area'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.NormallySafeInDaylight','normally safe in daylight'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.NormallySafeAtNight','normally safe at night'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.IncidentsHaveOccuredInArea','incidents have occured in area'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.FrequentlyPolicedUrbanArea','frequently policed urban area'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Steps','steps - cycle should be carried'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.Channelalongsidesteps','steps with a channel'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.TurnRestriction','TurnRestriction'
EXEC AddContent @Group, @CultEn, @Collection,'CycleAttribute.MiniRoundabout','mini roundabout'



------------------------------------------------------------------------------------------------------------------
-- Cycle Planner Path Images
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'CyclePlanner.DetailCycleControl.Instruction.ManoeuvreImage.AltText','Take care complex manoeuvre'
EXEC AddContent @Group, @CultEn, @Collection, 'CyclePlanner.DetailCycleControl.Instruction.ManoeuvreImage','cycle/roadexclamation.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'CyclePlanner.DetailCycleControl.Instruction.CyclePathImage','cycle/CyclePath.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'CyclePlanner.DetailCycleControl.Instruction.CyclePathImage.AltText','Journey follows cycle specific infrastructure'
EXEC AddContent @Group, @CultEn, @Collection, 'CyclePlanner.DetailCycleControl.Instruction.CycleRouteImage','cycle/CycleRoute.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'CyclePlanner.DetailCycleControl.Instruction.CycleRouteImage.AltText','Journey follows recommended cycle route'

------------------------------------------------------------------------------------------------------------------
-- StopEvent Result Control Text
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.HeaderRow.Select.Text','Select route'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.HeaderRow.Departure.Text','Departure time'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.HeaderRow.Service.Text','Service'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.HeaderRow.Arrive.Text','Arrival time'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.HeaderRow.JourneyTime.Text','Journey time'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.JourneyDirection.Outward.Text','Outward'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.JourneyDirection.Return.Text','Return'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.JourneyHeader.Outward.Text','{0} to {1}'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.JourneyHeader.Return.Text','{0} to {1}'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.JourneyHeader.ArriveBy.Text','Arriving by {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'StopEventOutput.JourneyHeader.LeavingAt.Text','Leaving at {0}'

	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.HeaderRow.Select.Text','Sélectionnez la voie'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.HeaderRow.Departure.Text','Heure de départ'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.HeaderRow.Service.Text','Service'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.HeaderRow.Arrive.Text','	Heure d''arrivée'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.HeaderRow.JourneyTime.Text','Horaires du trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.JourneyDirection.Outward.Text','Aller'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.JourneyDirection.Return.Text','Retour'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.JourneyHeader.Outward.Text','{0} et {1}'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.JourneyHeader.Return.Text','{0} et {1}'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.JourneyHeader.ArriveBy.Text','Arrivant par {0}'
	EXEC AddContent @Group, @CultFr, @Collection, 'StopEventOutput.JourneyHeader.LeavingAt.Text','Laissant à {0}'

GO

-- =============================================
-- Content script to add Google Analytics tag resource data
-- =============================================

------------------------------------------------
-- Analytics and Adverts content, all added to the group 'Analytics'
------------------------------------------------

DECLARE @Group varchar(100) = 'Analytics'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

-- Example format for Analytics tag content:
-- group, language, collection(PageId), resourceKey, value

-- DON'T FORGET TO REPLACE ALL ' WITH '' IN THE TAG


-- Live analytics tag
EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Host', 
'
<script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push([''_setAccount'', ''UA-23241082-1'']);
  _gaq.push([''_trackPageview'']);

  (function() {
    var ga = document.createElement(''script''); ga.type = ''text/javascript''; ga.async = true;
    ga.src = (''https:'' == document.location.protocol ? ''https://ssl'' : ''http://www'') + ''.google-analytics.com/ga.js'';
    var s = document.getElementsByTagName(''script'')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>
'
EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Tracker', 
''


-- Live adverts tag
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Service', 
'
<script type="text/javascript">

	var googletag = googletag || {};
	googletag.cmd = googletag.cmd || [];

	(function() {
		var gads = document.createElement(''script'');
		gads.async = true;
		gads.type = ''text/javascript'';
		var useSSL = ''https:'' == document.location.protocol;
		gads.src = (useSSL ? ''https:'' : ''http:'') +''//www.googletagservices.com/tag/js/gpt.js'';
		var node = document.getElementsByTagName(''script'')[0];
		node.parentNode.insertBefore(gads, node);
	})();

</script>
'
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Placeholders', 
'
<script type="text/javascript">

	googletag.cmd.push(function() {

	  //Adslot 1 declaration
      var slot1= googletag.defineSlot(''/7064/og/spectators/travel'', [[300,100]], ''top-ad'').addService(googletag.pubads());

      //Adslot 2 declaration
      <!-- var slot2= googletag.defineSlot(''/7064/og/spectators/travel'', [[300,250]], ''page-advert-3rdrail'').addService(googletag.pubads()); -->

      googletag.pubads().setTargeting(''lang'',[''en'']);
      googletag.pubads().enableAsyncRendering();
      googletag.enableServices();

	});

</script>
'

GO

-- =============================================
-- Script Template
-- =============================================

DECLARE @Group varchar(100) = 'Mobile'
DECLARE @Collection varchar(100) = 'General'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Tidy up first, helps keep content table clean,
-- and ensures this script contains complete control of content for this Group
EXEC DeleteAllGroupContent @Group

------------------------------------------------------------------------------------------------------------------
-- Page headings
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'MobileDefault.Heading.Text', 'Travel planner'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileError.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileError.Heading.ScreenReader.Text', 'Error'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileSorry.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileSorry.Heading.ScreenReader.Text', 'Sorry'
EXEC AddContent @Group, @CultEn, @Collection, 'MobilePageNotFound.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobilePageNotFound.Heading.ScreenReader.Text', 'Page Not Found'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileInput.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileInput.Heading.ScreenReader.Text', 'Journey Input'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileSummary.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileSummary.Heading.ScreenReader.Text', 'Journey Summary'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileDetail.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileDetail.Heading.ScreenReader.Text', 'Journey Detail'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileDirection.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileDirection.Heading.ScreenReader.Text', 'Journey Direction'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileMap.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileMap.Heading.ScreenReader.Text', 'Map'
EXEC AddContent @Group, @CultEn, @Collection, 'MobileTravelNews.Heading.Text', ''
EXEC AddContent @Group, @CultEn, @Collection, 'MobileTravelNews.Heading.ScreenReader.Text', 'Travel News'

	EXEC AddContent @Group, @CultFr, @Collection, 'MobileDefault.Heading.Text', 'Outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'MobileInput.Heading.ScreenReader.Text', 'Entrez un trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'MobileSummary.Heading.ScreenReader.Text', 'Résumé du trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'MobileDetail.Heading.ScreenReader.Text', 'Détails du trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'MobileDirection.Heading.ScreenReader.Text', 'Destination du trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'MobileMap.Heading.ScreenReader.Text', 'Carte'
	EXEC AddContent @Group, @CultFr, @Collection, 'MobileTravelNews.Heading.ScreenReader.Text', 'Actualité des transports'


------------------------------------------------------------------------------------------------------------------
-- Header
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Logo.ImageUrl','logos/logo-pink.png'
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Logo.AlternateText','London2012'
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Logo.ToolTip','Official London 2012 web site'
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Language.Link.Text.En','English'
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Language.Link.ToolTip.En','English'
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Language.Link.Text.Fr','Français'
EXEC AddContent @Group, @CultEn, @Collection, 'HeaderControl.Language.Link.ToolTip.Fr','Français'


------------------------------------------------------------------------------------------------------------------
-- Default
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'Default.PublicTransportModeButton.Text', 'Public transport'
	EXEC AddContent @Group, @CultFr, @Collection, 'Default.PublicTransportModeButton.Text', 'Transport en commun'
EXEC AddContent @Group, @CultEn, @Collection, 'Default.PublicTransportModeButton.ToolTip', 'Public transport'
	EXEC AddContent @Group, @CultFr, @Collection, 'Default.PublicTransportModeButton.ToolTip', 'Transport en commun'
EXEC AddContent @Group, @CultEn, @Collection, 'Default.CycleModeButton.Text', 'Cycle'
	EXEC AddContent @Group, @CultFr, @Collection, 'Default.CycleModeButton.Text', 'Vélo'
EXEC AddContent @Group, @CultEn, @Collection, 'Default.CycleModeButton.ToolTip', 'Cycle'
	EXEC AddContent @Group, @CultFr, @Collection, 'Default.CycleModeButton.ToolTip', 'Vélo'
EXEC AddContent @Group, @CultEn, @Collection, 'Default.TravelNewsButton.Text', 'Travel news'
	EXEC AddContent @Group, @CultFr, @Collection, 'Default.TravelNewsButton.Text', 'Actualité des transports'
EXEC AddContent @Group, @CultEn, @Collection, 'Default.TravelNewsButton.ToolTip', 'Travel news'
	EXEC AddContent @Group, @CultFr, @Collection, 'Default.TravelNewsButton.ToolTip', 'Actualité des transports'

EXEC AddContent @Group, @CultEn, @Collection, 'PublicJourneyOptions.MobiltyOptionsHeading.Text', 'Select to view accessibility options'
EXEC AddContent @Group, @CultEn, @Collection, 'PublicJourneyOptions.MobiltyOptionsLegend.Text', 'Select to view accessibility options'

	EXEC AddContent @Group, @CultFr, @Collection, 'PublicJourneyOptions.MobiltyOptionsHeading.Text', 'Sélectionner pour accéder aux options d’accessibilité'
	EXEC AddContent @Group, @CultFr, @Collection, 'PublicJourneyOptions.MobiltyOptionsLegend.Text', 'Sélectionner pour accéder aux options d’accessibilité'


------------------------------------------------------------------------------------------------------------------
-- Input
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Location.From.Text', 'From'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Location.From.Watermark', 'Your start location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Location.To.Text', 'To'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Location.To.Watermark', 'Your end location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Location.Venues.Text', 'Venues'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Location.Venues.ToolTip', 'Choose a venue'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Location.From.Text', 'Départ'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Location.From.Watermark', 'Lieu de départ'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Location.To.Text', 'Arrivée'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Location.To.Watermark', 'Lieu d''arrivée'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Location.Venues.Text', 'Sites'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Location.Venues.ToolTip', 'Choisir un site'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.Text', 'Choose a cycle parking location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.Disabled.Text', 'Select a venue first and then choose a cycle parking location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.Map.Text', 'Choose a cycle parking location from the venue map.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LocationPark.Text', 'Sélectionnez un parc à vélo'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LocationPark.Map.Text', 'Sélectionnez un parc à vélo sur le plan du site.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.Map.Text', 'Choose preferred cycle park'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.DropDown.NoParks', 'No cycle parking at this venue'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.ValidationError.NoParks', 'There is currently no cycle parking at this venue. You can plan a journey by public transport.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LocationPark.ValidationError.SelectPark', 'Please choose a cycle parking location before planning your journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.TypeOfRouteHeading.Text', 'Choose your type of route'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LocationPark.Map.Text', 'Choisir un parc à vélos préféré'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LocationPark.DropDown.NoParks', 'Aucun parc à vélo sur ce site'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LocationPark.ValidationError.NoParks', 'Il n''y a actuellement aucun parc à vélo sur ce site. Vous pouvez néanmoins planifier votre trajet par transports en commun.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LocationPark.ValidationError.SelectPark', 'Veuillez sélectionner un parc à vélo avant de planifier votre trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.TypeOfRouteHeading.Text', 'Choisir son type de trajet'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Date.Outward.Text', 'Date'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Time.Outward.Text', 'Time'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Date.SetDate.Text', 'Set date'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Date.SetDate.ToolTip', 'Set date'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Time.ArrivalTime.Text', 'Arrival time'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Time.ArrivalTime.ToolTip', 'Set arrival time'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Time.DepartureTime.Text', 'Departure time'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Time.DepartureTime.ToolTip', 'Set departure time'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Date.SetDate.Text', 'Choisir la date'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Date.SetDate.ToolTip', 'Choisir la date'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Time.ArrivalTime.Text', 'Heure d''arrivée'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Time.ArrivalTime.ToolTip', 'Choisir une heure d''arrivée'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Time.DepartureTime.Text', 'Heure de départ'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Time.DepartureTime.ToolTip', 'Choisir une heure de départ'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LeaveAt.Text', 'Leave'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.ArriveBy.Text', 'Arrive'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LeaveAt.Text', 'Partir'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.ArriveBy.Text', 'Arriver'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Now.Link.Text', 'Now'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Now.Link.ToolTip', 'Set date and time to now'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Now.Link.Text', 'Dès que possible'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Now.Link.ToolTip', 'Choisir la date et l''heure actuelle'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.TravelFromToVenueToggle.ImageUrl','arrows/Up_down.png'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.TravelFromToVenueToggle.AlternateText','Switch locations'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.TravelFromToVenueToggle.AlternateText','Inverser point de départ et destination'
	
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.PlanJourney.Text', 'Plan my journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.PlanJourney.ToolTip', 'Plan my journey'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.PlanJourney.Text', 'Planifier mon trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.PlanJourney.ToolTip', 'Planifier mon trajet'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.LoadingMessage.Text', 'Please wait while we check your travel choices'
	
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.LoadingMessage.Text', 'Veuillez patienter pendant la vérification de vos choix de trajet'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.Text', 'Back'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.ToolTip', 'Back'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileDefault.Text', 'Travel planner home'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileDefault.ToolTip', 'Travel planner home'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileInput.Text', 'Back to input'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileInput.ToolTip', 'Back to input'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileSummary.Text', 'Back to results'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileSummary.ToolTip', 'Back to results'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileDetail.Text', 'Back to details'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Back.MobileDetail.ToolTip', 'Back to details'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.Text', 'Retour'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.ToolTip', 'Retour'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileDefault.Text', 'Accueil de l''outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileDefault.ToolTip', 'Accueil de l''outil de planification de trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileInput.Text', 'Retour à la saisie'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileInput.ToolTip', 'Retour à la saisie'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileSummary.Text', 'Retour aux résultats'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileSummary.ToolTip', 'Retour aux résultats'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileDetail.Text', 'Retour aux détails'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Back.MobileDetail.ToolTip', 'Retour aux détails'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.Text', 'Next'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.ToolTip', 'Next'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileMap.Text', 'View my location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileMap.ToolTip', 'View my location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileMapJourney.Text', 'View map'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileMapJourney.ToolTip', 'View map'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileDirection.Text', 'Detailed directions'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileDirection.ToolTip', 'Detailed directions'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileTravelNews.Text', 'Travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.Next.MobileTravelNews.ToolTip', 'Travel news'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.Text', 'Suivant'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.ToolTip', 'Suivant'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileMap.Text', 'Voir ma localisation'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileMap.ToolTip', 'Voir ma localisation'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileMapJourney.Text', 'Voir un plan'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileMapJourney.ToolTip', 'Voir un plan'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileDirection.Text', 'Trajet détaillé'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileDirection.ToolTip', 'Trajet détaillé'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileTravelNews.Text', 'Actualité des transports'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyInput.Next.MobileTravelNews.ToolTip', 'Actualité des transports'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.GetTime.Text', 'Get Time'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.SelectDay.Text', 'Select Day'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.SelectVenue.Text', 'Select Venue'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.SelectVenue.AllVenuesButton.Text', 'All Venues'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyInput.SelectCyclePark.Text', 'Select Cycle Park'

EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.OriginAndDestinationOverlaps', 'The venues you have selected are in the same location. Your best transport option is likely to be a walk between the venues.<br />Within some venues, such as the Olympic Park, disabled spectators will be able to make use of Games Mobility. This free service will be easy to find inside the venue and will loan out manual wheelchairs and mobility scooters.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DateTimeIsBeforeEvent', 'The travel planner enables you to plan a journey between 18 July and 14 September 2012 only. Please select a date in this period.'
EXEC AddContent @Group, @CultEn, @Collection,'ValidateAndRun.DateTimeIsAfterEvent',	'The travel planner enables you to plan a journey between 18 July and 14 September 2012 only. Please select a date in this period.'

	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.OriginAndDestinationOverlaps', 'Les sites que vous avez sélectionnés sont proches l''un de l''autre. La meilleure façon de vous déplacer est probablement la marche.<br />À l''intérieur de certains sites, comme le Parc olympique, les spectateurs à mobilité réduite pourront profiter du Games Mobility. Ce service gratuit de prêt de fauteuils roulants manuels et de scooters est facile à trouver à l''intérieur du site.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DateTimeIsBeforeEvent', 'L''outil de planification de trajet vous permet de définir un itinéraire entre le 18 juillet et le 14 septembre 2012 uniquement. Veuillez sélectionner un jour située entre ces dates.'
	EXEC AddContent @Group, @CultFr, @Collection,'ValidateAndRun.DateTimeIsAfterEvent',	'L''outil de planification de trajet vous permet de définir un itinéraire entre le 18 juillet et le 14 septembre 2012 uniquement. Veuillez sélectionner un jour située entre ces dates.'

------------------------------------------------------------------------------------------------------------------
-- Summary
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LoadingImage.Imageurl', 'presentation/hourglass_small.gif'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LoadingImage.AlternateText', 'Please wait...'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LoadingImage.ToolTip', 'Please wait...'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LoadingMessage.Text', 'Please wait while we prepare your journey plan'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LoadingImage.AlternateText', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LoadingImage.ToolTip', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LoadingMessage.Text', 'Veuillez patienter pendant que nous élaborons votre trajet'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LongWaitMessage.Text', 'If the results do not appear within 30 seconds, '
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LongWaitMessageLink.Text', 'please click this link.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LongWaitMessageLink.ToolTip', 'If the results do not appear within 30 seconds, please click this link.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LongWaitMessage.Text', 'Si les résultats n''apparaissent pas dans les 30 prochaines secondes, '
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LongWaitMessageLink.Text', 'merci de cliquer sur ce lien.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LongWaitMessageLink.ToolTip', 'Si les résultats n''apparaissent pas dans les 30 prochaines secondes, merci de cliquer sur ce lien'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.NoResultsFound.Error', 'An error occurred while attempting to find journey options using the details you have entered. Please try again later.'
--EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.NoResultsFound.UserInfo', 'Sorry, the travel planner is unable to find a journey. You may wish to try again using a different time.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.NoResultsFound.UserInfo', ''

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.NoResultsFound.Error', 'Une erreur s''est produite lors de la recherche d''options de trajet. Veuillez recommencer ultérieurement.'
--	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.NoResultsFound.UserInfo', 'Désolé, l''outil de planification de trajet ne peut définir d''itinéraire. Réessayez en utilisant un horaire différent.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.NoResultsFound.UserInfo', ''

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.SelectJourney.Show.ToolTip','Select to view journey details'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.JourneyWebNoResults', 'Sorry, the travel planner is currently unable to find a journey. You may wish to try again using a different date or time.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.SelectJourney.Show.ToolTip','Cliquez pour voir le détail de votre trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.JourneyWebNoResults', 'Désolé, l''outil de planification de trajet ne peut définir d''itinéraire. Vous pouvez essayer de changer de date ou d''horaire.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CyclePlannerInternalError', 'The travel planner is unable to plan a cycle journey using the details you have entered. This may be because your start point is on a road with restrictions - please choose another start location.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CyclePlannerPartialReturn', 'The travel planner is unable to plan a cycle journey using the details you have entered. This may be because your start point is on a road with restrictions - please choose another start location.'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyOutput.Message.CyclePlannerNoResults', 'The travel planner is unable to plan a cycle journey using the details you have entered. This may be because your start point is on a road with restrictions - please choose another start location.'
EXEC AddContent @Group, @CultEn, @Collection, 'CycleJourneyLocations.CycleParkNoneFound.Text', 'No cycle parking is available at the time you have chosen. Cycle parking is normally open a few hours either side of an event. Please try again.'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CyclePlannerInternalError', 'L''outil de planification de trajet ne peut définir d''itinéraire à vélo à l''aide des informations entrées. Votre point de départ est peut-être situé sur une route avec restriction d''accès - veuillez choisir un autre point de départ.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CyclePlannerPartialReturn', 'L''outil de planification de trajet ne peut définir d''itinéraire à vélo à l''aide des informations entrées. Votre point de départ est peut-être situé sur une route avec restriction d''accès - veuillez choisir un autre point de départ.'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyOutput.Message.CyclePlannerNoResults', 'L''outil de planification de trajet ne peut définir d''itinéraire à vélo à l''aide des informations entrées. Votre point de départ est peut-être situé sur une route avec restriction d''accès - veuillez choisir un autre point de départ.'
	EXEC AddContent @Group, @CultFr, @Collection, 'CycleJourneyLocations.CycleParkNoneFound.Text', 'Aucun parc à vélo n''est disponible à l''horaire sélectionné. Le parc à vélo est ouvert quelques heures avant et après les épreuves. Merci de recommencer.'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.EarlierJourney.Outward.Text','Earlier'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.EarlierJourney.Outward.ToolTip','Get earlier journeys'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LaterJourney.Outward.Text','Later'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.LaterJourney.Outward.ToolTip','Get later journeys'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.EarlierJourney.Outward.Text','Plus tôt'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LaterJourney.Outward.Text','Plus tard'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.LaterJourney.Outward.ToolTip','Trouver des trajets à un horaire ultérieur'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.PlanReturnJourney.Text','Plan return journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneySummary.PlanReturnJourney.ToolTip','Plan return journey'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.PlanReturnJourney.Text','Planifiez votre retour'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneySummary.PlanReturnJourney.ToolTip','Planifiez votre retour'

------------------------------------------------------------------------------------------------------------------
-- Detail
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.JourneyPaging.Next.Text', 'Next'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.JourneyPaging.Next.ToolTip', 'Next journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.JourneyPaging.Previous.Text', 'Previous'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.JourneyPaging.Previous.ToolTip', 'Previous journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.JourneyPaging.Heading.Text', 'Journey'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyDetail.JourneyPaging.Next.Text', 'Suivant'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyDetail.JourneyPaging.Next.ToolTip', 'Trajet suivant'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyDetail.JourneyPaging.Previous.Text', 'Précédent'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyDetail.JourneyPaging.Previous.ToolTip', 'Trajet précédent'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyDetail.JourneyPaging.Heading.Text', 'Trajet'

EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.VenueMapPage.Heading.Text', 'Venue map'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyDetail.venueMapPage.InfoLabel.Text', ''

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyDetail.VenueMapPage.Heading.Text', 'Plan du site'

------------------------------------------------------------------------------------------------------------------
-- Map
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyMap.MapLoading.Text', 'Please wait...'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyMap.UseLocation.Text', 'Use my location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyMap.UseLocation.ToolTip', 'Use my location'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyMap.ViewJourney.Text', 'View journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyMap.ViewJourney.ToolTip', 'View journey'
EXEC AddContent @Group, @CultEn, @Collection, 'JourneyMap.MapInfo.NonJavascript', 'Maps can only be displayed with javascript enabled'

	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyMap.MapLoading.Text', 'Veuillez patienter...'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyMap.UseLocation.Text', 'Utiliser ma localisation'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyMap.UseLocation.ToolTip', 'Utiliser ma localisation'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyMap.ViewJourney.Text', 'Voir le trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyMap.ViewJourney.ToolTip', 'Voir le trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'JourneyMap.MapInfo.NonJavascript', 'Les cartes ne peuvent s''afficher que si le javascript est activé'

------------------------------------------------------------------------------------------------------------------
-- Travl News
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.ProvidedBy.Text', 'Travel news provided by <a href="http://www.transportdirect.info" rel="external">Transport Direct</a>'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LondonUnderground.ProvidedBy.Text', 'London Underground service details provided by <a href="http://m.tfl.gov.uk/" rel="external">Transport for London</a>'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.NewsModeOptionsLegend.Text', 'Choose travel news filter'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.DisplayedFor.Venues', 'Travel news displayed for {0}'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.DisplayedFor.AllVenues', 'Travel news displayed for All venues'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.LoadingMessage.Text', 'Please wait while we retrieve the latest travel news'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNews.FilterButtonNonJS.Text', 'Go'
EXEC AddContent @Group, @CultEn, @Collection, 'TravelNewsDetail.Heading.Text', 'Travel news details'

	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.ProvidedBy.Text', 'Informations sur la circulation fournies par <a href="http://www.transportdirect.info" rel="external">Transport Direct</a>'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LondonUnderground.ProvidedBy.Text', 'Informations sur le service de métro du London Underground fournies par <a href="http://m.tfl.gov.uk/" rel="external">Transport for London</a>'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.NewsModeOptionsLegend.Text', 'Choisir un filtre pour le trajet'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.DisplayedFor.Venues', 'Informations sur le trajet pour {0}'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.DisplayedFor.AllVenues', 'Informations sur le trajet pour Tous les sites'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.LoadingMessage.Text', 'Merci de patienter, nous cherchons les informations sur la circulation les plus récentes'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNews.FilterButtonNonJS.Text', 'Aller vers la page des résultats'
	EXEC AddContent @Group, @CultFr, @Collection, 'TravelNewsDetail.Heading.Text', 'Informations sur la circulation'

------------------------------------------------------------------------------------------------------------------
-- Error
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'Error.HeadingTitle.Text', 'We are sorry, an error has occurred. Please try again later.'
EXEC AddContent @Group, @CultEn, @Collection, 'Error.Message.Text', 'This may be due to a technical problem which we will do our best to resolve.<br /><br />Please <a class="error" href="{0}">select this link</a> to return to the travel planner and try again.<br /><br />'

	EXEC AddContent @Group, @CultFr, @Collection, 'Error.HeadingTitle.Text', 'Veuillez nous excuser, une erreur s''est produite. Merci de recommencer ultérieurement.'
	EXEC AddContent @Group, @CultFr, @Collection, 'Error.Message.Text', ' Il s''agit apparemment d''un problème technique et nous mettons tout en œuvre pour résoudre ce désagrément.<br /><br />Veuillez <a class="error" href="{0}"> cliquer sur ce lien</ a> pour retourner à l''outil de planification de parcours spectateur et réessayer.<br /><br />'

------------------------------------------------------------------------------------------------------------------
-- Sorry
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'Sorry.HeadingTitle.Text', 'Sorry, the travel planner is unexpectedly busy at this time. You may wish to return later to plan your journey.'
EXEC AddContent @Group, @CultEn, @Collection, 'Sorry.Message.Text', '<br /><br />'
	
	EXEC AddContent @Group, @CultFr, @Collection, 'Sorry.HeadingTitle.Text', 'Désolé, l''outil de planification de trajet est exceptionnellement occupé. Veuillez recommencer votre demande ultérieurement.'
	EXEC AddContent @Group, @CultFr, @Collection, 'Sorry.Message.Text', '<br /><br />'

------------------------------------------------------------------------------------------------------------------
-- PageNotFound
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection, 'PageNotFound.HeadingTitle.Text', 'Sorry, the page you have requested has not been found.'
EXEC AddContent @Group, @CultEn, @Collection, 'PageNotFound.Message.Text', 'Please try the following options:<br /><br /><a class="error" href="{0}">Go to the London 2012 homepage</a> or <br /><a class="error" href="{1}">use our site map</a><br /><br />'

	EXEC AddContent @Group, @CultFr, @Collection, 'PageNotFound.HeadingTitle.Text', 'Désolé, la page que vous avez demandée est introuvable.'
	EXEC AddContent @Group, @CultFr, @Collection, 'PageNotFound.Message.Text', 'Veuillez essayer les options suivantes:<br /><br /><a class="error" href="{0}">Allez sur la page d''accueil de Londres 2012</a> ou <br /><a class="error" href="{1}">utilisez notre carte du site</a><br /><br />'

------------------------------------------------------------------------------------------------------------------
-- Landing Page
------------------------------------------------------------------------------------------------------------------
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidLocations.Mobile.Text', 'At least one location entered must be a <strong>London 2012 venue</strong>. Please use the venue button to select a venue.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidDestination.Mobile.Text', 'The location entered in the ''To'' box must be a <strong>London 2012 venue</strong>. Please use the venue button to select a venue.'
EXEC AddContent @Group, @CultEn, @Collection,'Landing.Message.InvalidOrigin.Mobile.Text', 'The location entered in the ''From'' box must be a <strong>London 2012 venue</strong>.  Please use the venue button to select a venue.'

------------------------------------------------------------------------------------------------------------------
-- Cycle Maps
------------------------------------------------------------------------------------------------------------------
-- Maps - Brands Hatch
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100BND.CycleParkM.Url', 'maps/CycleParksMaps/8100BRH_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100BND.CycleParkM.AlternateText', 'Map of cycle parking for Brands Hatch'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100BND.CycleParkM.AlternateText', 'Brands Hatch - plan des parcs à vélo'

-- Maps - Olympic park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OPK.CycleParkM.Url', 'maps/CycleParksMaps/8100OPK_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OPK.CycleParkM.AlternateText', 'Map of cycle parking for Olympic Park'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100OPK.CycleParkM.AlternateText', 'Olympic Park - plan des parcs à vélo'

-- Maps - Victoria Park Live (Olympic park)
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100VPL.CycleParkM.Url', 'maps/CycleParksMaps/8100VPL_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100VPL.CycleParkM.AlternateText', 'Map of cycle parking for Victoria Park Live Site'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100VPL.CycleParkM.AlternateText', 'Victoria Park Live Site - plan des parcs à vélo'

-- Maps - Greenwich park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.CycleParkM.Url', 'maps/CycleParksMaps/8100GRP_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100GRP.CycleParkM.AlternateText', 'Map of cycle parking for Greenwich Park'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100GRP.CycleParkM.AlternateText', 'Greenwich Park - plan des parcs à vélo'

-- Maps - Earls Court
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.CycleParkM.Url', 'maps/CycleParksMaps/8100EAR_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EAR.CycleParkM.AlternateText', 'Map of cycle parking for Earls Court'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100EAR.CycleParkM.AlternateText', 'Earls Court - plan des parcs à vélo'

-- Maps - Eton Dorney
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100ETD.CycleParkM.Url', 'maps/CycleParksMaps/8100ETD_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100ETD.CycleParkM.AlternateText', 'Map of cycle parking for Eton Dorney'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100ETD.CycleParkM.AlternateText', 'Eton Dorney - plan des parcs à vélo'

-- Maps - ExCeL
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EXL.CycleParkM.Url', 'maps/CycleParksMaps/8100EXL_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100EXL.CycleParkM.AlternateText', 'Map of cycle parks for ExCeL'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100EXL.CycleParkM.AlternateText', 'ExCeL - plan des parcs à vélo'

-- Maps - Hadleigh Farm
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAD.CycleParkM.Url', 'maps/CycleParksMaps/8100HAD_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAD.CycleParkM.AlternateText', 'Map of cycle parking for Hadleigh Farm'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100HAD.CycleParkM.AlternateText', 'Hadleigh Farm - plan des parcs à vélo'

-- Maps - Hampden Park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAM.CycleParkM.Url', 'maps/CycleParksMaps/8100HAM_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HAM.CycleParkM.AlternateText', 'Map of cycle parking for Hampden Park'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100HAM.CycleParkM.AlternateText', 'Hampden Park - plan des parcs à vélo'

-- Maps - Horse Guards Parade
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.CycleParkM.Url', 'maps/CycleParksMaps/8100HGP_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HGP.CycleParkM.AlternateText', 'Map of cycle parking for Horse Guards Parade'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100HGP.CycleParkM.AlternateText', 'Horse Guards Parade - plan des parcs à vélo'

-- Maps - Hyde Park
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.CycleParkM.Url', 'maps/CycleParksMaps/8100HYD_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HYD.CycleParkM.AlternateText', 'Map of cycle parking for Hyde Park'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100HYD.CycleParkM.AlternateText', 'Hyde Park - plan des parcs à vélo'

-- Maps - Hyde Park Live
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HPL.CycleParkM.Url', 'maps/CycleParksMaps/8100HYD_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100HPL.CycleParkM.AlternateText', 'Map of cycle parking for Hyde Park Live'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100HPL.CycleParkM.AlternateText', 'Hyde Park - plan des parcs à vélo'

-- Maps - Lords Cricket Ground
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LCG.CycleParkM.Url', 'maps/CycleParksMaps/8100LCG_CycleParkMapsM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LCG.CycleParkM.AlternateText', 'Map of cycle parking for Lords Cricket Ground'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100LCG.CycleParkM.AlternateText', 'Lords Cricket Ground - plan des parcs à vélo'

-- Maps - Lee Valley White Water Centre
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LVC.CycleParkM.Url', 'maps/CycleParksMaps/8100LVC_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100LVC.CycleParkM.AlternateText', 'Map of cycle parking for Lee Valley White Water Centre'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100LVC.CycleParkM.AlternateText', 'Lee Valley White Water Centre - plan des parcs à vélo'

-- Maps - Millennium Stadium
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MIL.CycleParkM.Url', 'maps/CycleParksMaps/8100MIL_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MIL.CycleParkM.AlternateText', 'Map of cycle parking for Millennium Stadium'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100MIL.CycleParkM.AlternateText', 'Millennium Stadium - plan des parcs à vélo'

-- Maps - The Mall
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.CycleParkM.Url', 'maps/CycleParksMaps/8100MAL_CycleParksMap.png'
--EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MAL.CycleParkM.AlternateText', 'Map of cycle parking for The Mall'
--	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100MAL.CycleParkM.AlternateText', 'The Mall - plan des parcs à vélo'

-- Maps - The Mall - North
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.CycleParkM.Url', 'maps/CycleParksMaps/8100MLL_CycleParksMapNorthM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLN.CycleParkM.AlternateText', 'Map of cycle parking for The Mall - North'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100MLN.CycleParkM.AlternateText', 'The Mall - plan des parcs à vélo'

-- Maps - The Mall - South
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.CycleParkM.Url', 'maps/CycleParksMaps/8100MLL_CycleParksMapSouthM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100MLS.CycleParkM.AlternateText', 'Map of cycle parking for The Mall - South'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100MLS.CycleParkM.AlternateText', 'The Mall - plan des parcs à vélo'

-- Maps - North Greenwich Arena
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.CycleParkM.Url', 'maps/CycleParksMaps/8100NGA_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100NGA.CycleParkM.AlternateText', 'Map of cycle parking for North Greenwich Arena'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100NGA.CycleParkM.AlternateText', 'North Greenwich Arena - plan des parcs à vélo'

-- Maps - Old Trafford
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OLD.CycleParkM.Url', 'maps/CycleParksMaps/8100OLD_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100OLD.CycleParkM.AlternateText', 'Map of cycle parking for Old Trafford'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100OLD.CycleParkM.AlternateText', 'Old Trafford - plan des parcs à vélo'

-- Maps - The Royal Artillery Barracks
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.CycleParkM.Url', 'maps/CycleParksMaps/8100RAB_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100RAB.CycleParkM.AlternateText', 'Map of cycle parking for The Royal Artillery Barracks'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100RAB.CycleParkM.AlternateText', 'The Royal Artillery Barracks - plan des parcs à vélo'

-- Maps - Woolwich Live (nearr The Royal Artillery Barracks)
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WOL.CycleParkM.Url', 'maps/CycleParksMaps/8100RAB_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WOL.CycleParkM.AlternateText', 'Map of cycle parking for Woolwich Live Site'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100WOL.CycleParkM.AlternateText', 'Woolwich Live Site - plan des parcs à vélo'

-- Maps - Weymouth and Portland - The Nothe
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WAP.CycleParkM.Url', 'maps/CycleParksMaps/8100WAP_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WAP.CycleParkM.AlternateText', 'Map of cycle parking for Weymouth and Portland - The Nothe'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100WAP.CycleParkM.AlternateText', 'Weymouth and Portland - The Nothe - plan des parcs à vélo'

-- Maps - Weymouth Live
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WLB.CycleParkM.Url', 'maps/CycleParksMaps/8100WAP_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WLB.CycleParkM.AlternateText', 'Map of cycle parking for Weymouth Live'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100WLB.CycleParkM.AlternateText', 'Weymouth Live - plan des parcs à vélo'

-- Maps - Wembley Arena
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEA.CycleParkM.Url', 'maps/CycleParksMaps/8100WEA_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEA.CycleParkM.AlternateText', 'Map of cycle parking for Wembley Arena'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100WEA.CycleParkM.AlternateText', 'Wembley Arena - plan des parcs à vélo'

-- Maps - Wembley Stadium
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEM.CycleParkM.Url', 'maps/CycleParksMaps/8100WEM_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WEM.CycleParkM.AlternateText', 'Map of cycle parking for Wembley Stadium'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100WEM.CycleParkM.AlternateText', 'Wembley Stadium - plan des parcs à vélo'

-- Maps - Wimbledon
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WIM.CycleParkM.Url', 'maps/CycleParksMaps/8100WIM_CycleParksMapM.png'
EXEC AddContent @Group, @CultEn, @Collection,'JourneyLocations.8100WIM.CycleParkM.AlternateText', 'Map of cycle parking for Wimbledon'

	EXEC AddContent @Group, @CultFr, @Collection,'JourneyLocations.8100WIM.CycleParkM.AlternateText', 'Wimbledon - plan des parcs à vélo'

GO

-- =============================================
-- Script Template
-- =============================================

USE SJPContent
Go


DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO






-- =============================================
-- SCRIPT TO UPDATE DEV MACHINE SPECIFIC SETTINGS - OVERWRITES PRODUCTION SETTINGS IN DB PROJECT IF BUILD CONFIGURATION = DEBUG
-- =============================================

-- Analytics content, all added to the group 'Analytics'

USE SJPContent
Go

DECLARE @Group varchar(100) = 'Analytics'
DECLARE @CultEn varchar(2) = 'en'
DECLARE @CultFr varchar(2) = 'fr'

-- Live analytics tag
EXEC AddContent @Group, @CultEn, 'Default', 'Analytics.Tag.Host', ''

-- Live adverts tag
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Service', ''
EXEC AddContent @Group, @CultEn, 'Default', 'Adverts.Tag.Placeholders', ''

GO
