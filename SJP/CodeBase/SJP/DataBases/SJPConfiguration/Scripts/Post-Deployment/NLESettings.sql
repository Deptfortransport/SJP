-- =============================================
-- SCRIPT TO UPDATE NLE SPECIFIC SETTINGS 
-- Not run during deployment in any configuration but to be copied with release files and 
-- manually run during deployment to NLE.
-- =============================================


USE [SJPConfiguration] 
GO


-- SJPWeb - Logging
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace','On', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'SJPWeb', 'UserPortal'

-- SJPWeb - Debug information mode on
EXEC AddUpdateProperty 'Debug.Information.Show.Switch','true', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Debug.Information.Show.Switch','true', 'SJPMobile', 'UserPortal'

-- CCAgent - Logging
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'CCAgent', 'UserPortal'

-- CoordinateConvertorService - Logging
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'CoordinateConvertorService', 'UserPortal'

-- Venue Incidents
EXEC AddUpdateProperty 'VenueIncidents.IncidentLandingPage.Location', 'http://m.test-travel.london2012.com/TravelNews.aspx?nv={0}&pn=0', 'VenueIncidents', 'FileCreation'

GO