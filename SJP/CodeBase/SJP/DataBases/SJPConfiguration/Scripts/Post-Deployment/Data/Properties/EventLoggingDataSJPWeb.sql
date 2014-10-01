-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPWeb' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'SJPWeb'
DECLARE @GID varchar(50) = 'UserPortal'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', 'EVENTLOG1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', 'QUEUE1', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Name', 'Application', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Source', @AID, @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Machine', '.', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Path', '.\Private$\SJPPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Delivery', 'Recoverable', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Priority', 'Normal', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Warning', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1 EVENTLOG1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', 'PAGE JOURNEYREQUEST JOURNEYRESULTS CYCLEREQUEST CYCLERESULT REPEATVISITOR RETAIL LANDING GATEWAY REFTRANS WORKLOAD STOPEVENT NORESULTS', @AID, @GID

-- Custom Events Configuration
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Name', 'PageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Name', 'JourneyPlanRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Name', 'JourneyPlanResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Name', 'CyclePlannerRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Name', 'CyclePlannerResultEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Name', 'RepeatVisitorEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Name', 'RetailerHandoffEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Name', 'LandingPageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Name', 'DataGatewayEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Name', 'ReferenceTransactionEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Name', 'WorkloadEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Name', 'StopEventRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Name', 'NoResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Trace', 'On', @AID, @GID
GO