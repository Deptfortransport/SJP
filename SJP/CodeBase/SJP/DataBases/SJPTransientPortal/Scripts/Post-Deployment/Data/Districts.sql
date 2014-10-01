-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


BEGIN TRANSACTION
DELETE FROM dbo.[Districts]


BULK INSERT dbo.[Districts] FROM '$(RemotePath)Districts.csv' WITH
(FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)Districts.fmt') -- Using Header row

COMMIT TRANSACTION