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
