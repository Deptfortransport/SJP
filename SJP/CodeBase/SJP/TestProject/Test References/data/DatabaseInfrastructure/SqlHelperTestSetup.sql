USE [SJPConfiguration]

-- Test Table
BEGIN
CREATE TABLE TestTable(
	[RefID] [int] NOT NULL DEFAULT(0),
	[RefValue] [int] NOT NULL
)
END

BEGIN
-- Test Stored Procedures with no parameters
EXEC ('CREATE PROCEDURE GetALLTestData '
	+ 'AS '
    + 'SELECT * FROM TestTable')


-- Test Stored Procedures with no parameters
EXEC ('CREATE PROCEDURE GetTestData '
	+ '@RefId int '
	+ 'AS '
    + 'SELECT RefValue FROM TestTable WHERE RefId = @RefId')

-- Simulating long running stored procedure to test command time out 
EXEC ('CREATE PROCEDURE LongRunningProc '
	+ '@RefId int '
	+ 'AS '
	+ 'WAITFOR DELAY ''00:01'''
    + 'SELECT RefValue FROM TestTable WHERE RefId = @RefId')

END


-- TestTable data
INSERT INTO TestTable (RefId, RefValue)
VALUES (0,1),
	(1,2),
	(2,3),
	(3,4),
	(4,5)

-- Change Notification 
INSERT INTO [SJPConfiguration].[dbo].[ChangeNotification]
           ([Version]
           ,[Table])
     VALUES
           (0
           ,'TestTable')






