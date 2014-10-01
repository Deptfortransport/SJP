-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CCAgent' properties
------------------------------------------------
DECLARE @AID varchar(50) = 'CCAgent'
DECLARE @GID varchar(50) = 'UserPortal'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', 'EVENTLOG1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', 'QUEUE1', @AID, @GID

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
EXEC AddUpdateProperty 'Logging.Event.Custom', '', @AID, @GID

GO