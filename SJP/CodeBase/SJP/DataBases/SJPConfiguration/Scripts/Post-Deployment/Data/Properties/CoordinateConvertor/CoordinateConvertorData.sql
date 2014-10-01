-- =============================================
-- Script to add properties data
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CoordinateConvertorService' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'CoordinateConvertorService'
DECLARE @GID varchar(50) = 'UserPortal'

-- Coordinate Convertor
EXEC AddUpdateProperty 'Coordinate.Convertor.DataPath','D:\inetpub\wwwroot\CoordinateConvertorService\bin', @AID, @GID
EXEC AddUpdateProperty 'Coordinate.Convertor.InitialiseString', 'GIQ.6.0', @AID, @GID

GO