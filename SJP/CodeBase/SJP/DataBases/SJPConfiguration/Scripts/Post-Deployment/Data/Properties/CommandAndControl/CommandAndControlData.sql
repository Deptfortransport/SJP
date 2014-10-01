-- =============================================
-- Script Template
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CCAgent' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'CCAgent'
DECLARE @GID varchar(50) = 'UserPortal'

-- Poll interval
EXEC AddUpdateProperty 'CCAgent.OverallPollIntervalSeconds',	'30', @AID, @GID

-- Data Notification - Groups
EXEC AddUpdateProperty 'DataNotification.PollingInterval.Seconds', '60', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.Groups', 'CommandControl', @AID, @GID

-- Data Notification - Tables
EXEC AddUpdateProperty 'DataNotification.CommandControl.Database', 'CommandControlDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.CommandControl.Tables', 'ChecksumMonitoringItems,DatabaseMonitoringItems,FileMonitoringItems,WMIMonitoringItems', @AID, @GID

GO