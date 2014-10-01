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
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[AddCyclePlannerRequestEvent]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddCyclePlannerResultEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddDataGatewayEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddEBCCalculationEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddEnhancedExposedServiceEvent]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddExposedServicesEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddGazetteerEvent]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddGradientProfileEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddInternalRequestEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddInternationalPlannerEvent]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddInternationalPlannerRequestEvent]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddInternationalPlannerResultEvent]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddJourneyPlanRequestEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddJourneyPlanRequestVerboseEvent]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddJourneyPlanResultsEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddJourneyPlanResultsVerboseEvent]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddJourneyWebRequestEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddLandingPageEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddLocationRequestEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddLoginEvent]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddMapAPIEvent]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddMapEvent]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddNoResultsEvent]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddOperationalEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddPageEntryEvent]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddReferenceTransactionEvent]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddReferenceTransactionEventSiteStatus]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddRepeatVisitorEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddRepeatVisitorEventWebLogReader]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddRetailerHandoffEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddRTTIInternalEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddStopEventRequestEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddUserFeedbackEvent]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddUserPreferenceSaveEvent]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[AddWorkloadEvent]						TO [SJP_User]

GRANT EXECUTE ON [dbo].[Archiver]								TO [SJP_User]
GRANT EXECUTE ON [dbo].[DeleteImportedStagingData]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[DeleteStagingData]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetLatestImported]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetTableColumnLengths]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[RTTIEventLog]							TO [SJP_User]

GO

-- =============================================
-- Script Template
-- =============================================
DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO

