﻿/*
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
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[GetAllVenueCarParks]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetAllVenueCycleParks]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetAllVenueRiverSerivces]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetAlternativeSuggestionList]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetChangeTable]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetCycleVenueParkAvailability]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetGNATAdminAreas]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetGNATList]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetGNATStopType]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetGroupLocation]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetGroupNaPTANs]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetLocalityLocation]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetLocalityLocations]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetLocations]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetNaptanLocation]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetPierToVenueNavigationPaths]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetPostcodeLocation]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetPostcodeLocations]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRiverServices]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetTransitShuttleAvailability]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetUnknownLocation]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueAccessData]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueCarParkAvailability]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueCarParkInformation]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueCarParkTransitShuttles]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueCarParkTransitShuttleTransfers]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueCycleParks]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueGateCheckConstraints]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueGateMaps]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueGateNavigationPaths]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueGates]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenueLocations]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVenuesList]							TO [SJP_User]

GRANT EXECUTE ON [dbo].[ImportSJPAdditionalVenueData]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPCycleParkLocations]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPGNATAdminAreasData]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPGNATLocationsData]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPParkAndRideLocations]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPParkAndRideToids]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPVenueAccessData]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPVenueGateData]					TO [SJP_User]

GO

-- =============================================
-- Script Template
-- =============================================

--EXEC AddChangeNotificationTable 'XXX'

USE SJPGazetteer
Go


GO

-- =============================================
-- Script Template
-- =============================================

USE SJPGazetteer
Go

BEGIN TRANSACTION
DELETE FROM dbo.[SJPNonPostcodeLocations]


BULK INSERT dbo.[SJPNonPostcodeLocations] FROM '$(RemotePath)SJPNonPostcodeLocations.csv' WITH
(KEEPIDENTITY, FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)SJPLocations.fmt') -- Using Header row

COMMIT TRANSACTION

-- =============================================
-- Script Template
-- =============================================

USE SJPGazetteer
Go

BEGIN TRANSACTION
DELETE FROM dbo.[SJPPostcodeLocations]


BULK INSERT dbo.[SJPPostcodeLocations] FROM '$(RemotePath)SJPPostcodeLocations.csv' WITH
(KEEPIDENTITY, FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)SJPLocations.fmt') -- Using Header row

COMMIT TRANSACTION


-- =============================================
-- Script Template
-- =============================================

USE SJPGazetteer
Go

DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO

