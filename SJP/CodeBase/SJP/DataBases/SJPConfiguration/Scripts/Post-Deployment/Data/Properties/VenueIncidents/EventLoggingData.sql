-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'VenueIncidents' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'VenueIncidents'
DECLARE @GID varchar(50) = 'FileCreation'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Verbose', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'Off', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', '', @AID, @GID

GO