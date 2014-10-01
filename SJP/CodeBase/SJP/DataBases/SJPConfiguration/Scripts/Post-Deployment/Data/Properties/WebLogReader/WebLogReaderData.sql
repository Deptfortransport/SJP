-- =============================================
-- Script Template
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'WebLogReader' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'WebLogReader'
DECLARE @GID varchar(50) = 'Reporting'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

EXEC AddUpdateProperty 'WebLogReader.WebLogFolders', 'W3SVC1 W3SVC3', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC1.LogDirectory', 'D:\info\logs\LogFiles\W3SVC1', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC1.ArchiveDirectory', 'D:\info\logs\LogFiles\W3SVC1\Archive', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC3.LogDirectory', 'D:\info\logs\LogFiles\W3SVC3', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC3.ArchiveDirectory', 'D:\info\logs\LogFiles\W3SVC3\Archive', @AID, @GID

EXEC AddUpdateProperty 'WebLogReader.WebPageExtensions', 'asp aspx htm html pdf ashx axd [none]', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.NonPageMinimumBytes', '5000000', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ClientIPExcludes', '', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.UseLocalTime', 'false', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.RolloverPeriod', 'Hourly', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.Ranges', 'RANGE1 RANGE2', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE1.Min', '100', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE1.Max', '399', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE2.Min', '500', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE2.Max', '599', @AID, @GID

EXEC AddUpdateProperty 'WebLogReader.UserExperienceVisitor.UserAgent', 'SiteCon', @AID, @GID -- Not case sensitive
EXEC AddUpdateProperty 'WebLogReader.Cookie.SessionId.StartMarker', 'ASP.NET_SessionId=', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.Cookie.SessionId.EndMarker', ';', @AID, @GID

GO