-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go

BEGIN TRANSACTION
DELETE FROM dbo.[AdminAreas]


BULK INSERT dbo.[AdminAreas] FROM '$(RemotePath)AdminAreas.csv' WITH
(FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)AdminAreas.fmt') -- Using Header row

COMMIT TRANSACTION