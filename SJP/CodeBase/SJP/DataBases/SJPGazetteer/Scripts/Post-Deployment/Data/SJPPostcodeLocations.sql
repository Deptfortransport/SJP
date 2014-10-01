-- =============================================
-- Script Template
-- =============================================

USE SJPGazetteer
Go

BEGIN TRANSACTION
DELETE FROM dbo.[SJPPostcodeLocations]


BULK INSERT dbo.[SJPPostcodeLocations] FROM '$(RemotePath)SJPPostcodeLocations.csv' WITH
(KEEPIDENTITY, FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)SJPLocations.fmt') -- Using Header row

COMMIT TRANSACTION

