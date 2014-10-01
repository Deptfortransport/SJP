--PropertyProviderTest Clean up

-- SQL removes all the temporary data from the Properties table 
-- and then moves the actual data back in from the temporary tables

USE [SJPConfiguration]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_Properties') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to DropDownGaz tables
		TRUNCATE TABLE [Properties]
		
		
		-- insert data in DropDownGaz tables
		INSERT INTO [Properties] SELECT * FROM [temp_Properties]
		
		
	
		-- and delete the temp tables
		DROP TABLE [temp_Properties]
		
	END


if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.DatabasePropertyProviderTestLoad1') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.DatabasePropertyProviderTestLoad2') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.DatabasePropertyProviderTestLoad3') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.GetProperty') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP PROCEDURE DatabasePropertyProviderTestLoad1
		DROP PROCEDURE DatabasePropertyProviderTestLoad2
		DROP PROCEDURE DatabasePropertyProviderTestLoad3
		DROP PROCEDURE GetProperty
	END

-- Following is just as precaution as during database provider tests GetVersion and SelectGlobalProperties stored proc name gets changed
-- to immulate sql exceptions
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.GetVersion1') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	EXEC sp_rename 'GetVersion1', 'GetVersion'
END

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.SelectGlobalProperties1') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	EXEC sp_rename 'SelectGlobalProperties1', 'SelectGlobalProperties'
END

