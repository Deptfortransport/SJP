-- =============================================
-- Script Template
-- =============================================
USE SJPConfiguration
GO

------------------------------------------------
-- 'VenueIncidents' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'VenueIncidents'
DECLARE @GID varchar(50) = 'FileCreation'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

-- Venue Incidents
EXEC AddUpdateProperty 'VenueIncidents.OutputFile.Location', 'D:\inetpub\wwwroot\Data\TravelNews\VenueIncidents.xml', @AID, @GID
EXEC AddUpdateProperty 'VenueIncidents.IncidentLandingPage.Location', 'http://m.travel.london2012.com/TravelNews.aspx?nv={0}&pn=0', @AID, @GID

GO