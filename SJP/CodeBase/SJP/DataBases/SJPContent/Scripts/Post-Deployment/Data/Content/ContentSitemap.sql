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