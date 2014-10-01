--SJPReportStaging Clean up

-- SQL removes all the temporary data from the Report Staging tables

USE [SJPReportStaging]

DELETE FROM [dbo].[CyclePlannerRequestEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[CyclePlannerResultEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[DataGatewayEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[JourneyPlanRequestEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[JourneyPlanResultsEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[LandingPageEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[OperationalEvent]
      WHERE [SessionId] like 'Test%'
	     OR [Message] like 'Test%'

DELETE FROM [dbo].[PageEntryEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[ReferenceTransactionEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[RepeatVisitorEvent]
      WHERE [SessionIdOld] like 'Test%'
		 OR [SessionIdNew] like 'Test%'

DELETE FROM [dbo].[RetailerHandoffEvent]
      WHERE [SessionId] like 'Test%'

DELETE FROM [dbo].[StopEventRequestEvent]
      WHERE [RequestId] like 'Test%'

DELETE FROM [dbo].[WorkloadEvent]
      WHERE [PartnerId] = -999
