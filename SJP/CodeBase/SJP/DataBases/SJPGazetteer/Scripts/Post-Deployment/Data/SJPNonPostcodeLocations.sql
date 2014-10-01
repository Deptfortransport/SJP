-- =============================================
-- Script Template
-- =============================================

USE SJPGazetteer
Go

BEGIN TRANSACTION
DELETE FROM dbo.[SJPNonPostcodeLocations]


BULK INSERT dbo.[SJPNonPostcodeLocations] FROM '$(RemotePath)SJPNonPostcodeLocations.csv' WITH
(KEEPIDENTITY, FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)SJPLocations.fmt') -- Using Header row

COMMIT TRANSACTION

