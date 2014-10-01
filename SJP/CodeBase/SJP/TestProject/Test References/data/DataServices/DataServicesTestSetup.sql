
USE [SJPTransientPortal]


BEGIN
-- Delete Temp table so we start cleanly
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_CycleAttribute') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_CycleAttribute]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_AdminAreas') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_AdminAreas]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_Districts') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_Districts]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_StopAccessibilityLinks') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_StopAccessibilityLinks]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_DropDownLists') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_DropDownLists]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPEventDates') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPEventDates]
	
END


-- Copy CycleAttribute data in to new temp tables
BEGIN
	SELECT * INTO [dbo].[temp_CycleAttribute] FROM [CycleAttribute]
	SELECT * INTO [dbo].[temp_AdminAreas] FROM [AdminAreas]
	SELECT * INTO [dbo].[temp_Districts] FROM [Districts]
	SELECT * INTO [dbo].[temp_StopAccessibilityLinks] FROM [StopAccessibilityLinks]
	SELECT * INTO [dbo].[temp_DropDownLists] FROM [DropDownLists]
	SELECT * INTO [dbo].[temp_SJPEventDates] FROM [SJPEventDates]
END



-- Delete the existing CycleAttribute
BEGIN
	TRUNCATE TABLE [CycleAttribute]
	TRUNCATE TABLE [AdminAreas]
	TRUNCATE TABLE [Districts]
	TRUNCATE TABLE [StopAccessibilityLinks]
	TRUNCATE TABLE [DropDownLists]
	TRUNCATE TABLE [SJPEventDates]
END

