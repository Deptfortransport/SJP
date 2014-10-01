-- =============================================
-- Script to add properties data
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPMobile' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'SJPMobile'
DECLARE @GID varchar(50) = 'UserPortal'

-- State Server
EXEC AddUpdateProperty 'StateServer.ApplicationName', 'SJPMobileStateServer', @AID, @GID

-- Styling
EXEC AddUpdateProperty 'Style.Version', 'Version', @AID, @GID

-- StyleSheets
EXEC AddUpdateProperty 'StyleSheet.Files.Keys', '', @AID, @GID

-- Javascripts
EXEC AddUpdateProperty 'Javscript.Files.Keys', 'jquery', @AID, @GID
EXEC AddUpdateProperty 'Javscript.File.jquery', 'https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js', @AID, @GID

-- Google Analytics and Adverts
EXEC AddUpdateProperty 'Analytics.Tag.Include.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Adverts.Tag.Include.Switch', 'false', @AID, @GID

-- Canonical Tags
EXEC AddUpdateProperty 'Canonical.Tag.Include.Switch', 'false', @AID, @GID

-- Language Link
EXEC AddUpdateProperty 'Header.Link.Language.Visible.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Header.Link.Language.Paralympics.Visible.Switch', 'false', @AID, @GID

-- JourneyOptionTabContainer - Cycle
EXEC AddUpdateProperty 'CyclePlanner.Enabled.Switch', 'true', @AID, @GID

-- JourneyOptionTabContainer - Additional Mobility Options
EXEC AddUpdateProperty 'PublicJourneyOptions.AccessibilityOptions.Visible', 'true', @AID, @GID

-- Journey Location Control
EXEC AddUpdateProperty 'LocationControl.LocationSuggest.ScriptId', 'SJPMobile_23052012', @AID, @GID

-- Journey Location - Toggle
EXEC AddUpdateProperty 'JourneyLocations.Toggle.Enabled.Switch', 'true', @AID, @GID

-- Journey Options - Wait/Refresh
EXEC AddUpdateProperty 'JourneySummary.Wait.RefreshTime.Seconds', '5', @AID, @GID
EXEC AddUpdateProperty 'JourneySummary.Wait.RefreshCount.Max', '6', @AID, @GID

-- Journey Options - Return Journey button
EXEC AddUpdateProperty 'JourneySummary.PlanReturnJourney.Visible.Switch', 'true', @AID, @GID

-- Journey Options - Replan
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.MaxJourneys.Visible', '4', @AID, @GID

EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.MaxJourneys.Visible', '4', @AID, @GID

EXEC AddUpdateProperty 'JourneySummary.Replan.RetainPreviousJourneys.Switch', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneySummary.Replan.RetainPreviousJourneysWhenNoResults.Switch', 'true', @AID, @GID

-- Map
EXEC AddUpdateProperty 'Map.Input.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Map.Journey.Cycle.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Map.Journey.Walk.Enabled.Switch', 'true', @AID, @GID

-- Landing Page
EXEC AddUpdateProperty 'LandingPage.Location.Coordinate.LatitudeLongitude.Switch', 'true', @AID, @GID

-- Travel News
EXEC AddUpdateProperty 'TravelNews.Enabled.Switch', 'true', @AID, @GID

-- Travel News - Auto Refresh
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.ShowRefreshLink.Switch', 'false', @AID, @GID
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Refresh.Seconds', '360', @AID, @GID

-- Underground News
EXEC AddUpdateProperty 'UndergroundNews.Enabled.Switch', 'true', @AID, @GID

GO
