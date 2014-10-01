-- =============================================
-- Script to add reporting properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'EventReceiver' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'EventReceiver'
DECLARE @GID varchar(50) = 'Reporting'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

-- Message Queues to monitor for events
EXEC AddUpdateProperty 'EventReceiver.Queue', 'SourceQueue1 SourceQueue2', @AID, @GID
EXEC AddUpdateProperty 'EventReceiver.Queue.SourceQueue1.Path', '.\Private$\SJPPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'EventReceiver.Queue.SourceQueue2.Path', '.\Private$\TDPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'EventReceiver.TimeBeforeRecovery.Millisecs', '60000', @AID, @GID

GO