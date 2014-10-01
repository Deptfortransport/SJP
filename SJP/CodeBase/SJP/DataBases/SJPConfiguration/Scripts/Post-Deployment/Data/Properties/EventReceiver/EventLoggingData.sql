-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'EventReceiver' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'EventReceiver'
DECLARE @GID varchar(50) = 'Reporting'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', 'SJPDB OPDB CJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Custom.SJPDB.Name', 'SJPCustomEventPublisher', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom.OPDB.Name', 'SJPOperationalEventPublisher', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom.CJPDB.Name', 'CJPCustomEventPublisher', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Verbose', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', 'PAGE JOURNEYREQUEST JOURNEYRESULTS CYCLEREQUEST CYCLERESULT REPEATVISITOR RETAIL LANDING GATEWAY REFTRANS WORKLOAD STOPEVENT ROE CJPJOURNEYWEB CJPLOCATION CJPINTERNAL NORESULTS', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Name', 'PageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'SJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Name', 'JourneyPlanRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Name', 'JourneyPlanResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Name', 'CyclePlannerRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Name', 'CyclePlannerResultEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Name', 'RepeatVisitorEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Name', 'RetailerHandoffEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Name', 'LandingPageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Name', 'DataGatewayEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Name', 'ReferenceTransactionEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Name', 'WorkloadEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Name', 'StopEventRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Name', 'ReceivedOperationalEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Publishers', 'OPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Trace', 'On', @AID, @GID


-- TDP CJP Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Name', 'JourneyWebRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Publishers', 'CJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Name', 'LocationRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Publishers', 'CJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Name', 'InternalRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Publishers', 'CJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Name', 'NoResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Trace', 'On', @AID, @GID

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

GO
