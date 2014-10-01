--SQLHelperTest Clean up



USE [SJPConfiguration]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.TestTable') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		
		TRUNCATE TABLE [TestTable]
		
		-- and delete the temp tables
		DROP TABLE [TestTable]
		
	END

DELETE FROM [dbo].[ChangeNotification] WHERE [Table] = 'TestTable'

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.GetALLTestData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.GetTestData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.LongRunningProc') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP PROCEDURE GetAllTestData
		DROP PROCEDURE GetTestData
		DROP PROCEDURE LongRunningProc
	END