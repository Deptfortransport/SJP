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
