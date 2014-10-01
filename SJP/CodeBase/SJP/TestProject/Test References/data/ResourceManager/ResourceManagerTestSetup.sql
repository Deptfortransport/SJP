USE [SJPContent]




	-- Delete Temp table so we start cleanly
BEGIN
	if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_Content') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[temp_Content]


	if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_ContentGroup') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[temp_ContentGroup]


	if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_ContentOverride') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[temp_ContentOverride]
END



-- Copy data in to new temp tables
BEGIN
	SELECT * INTO [dbo].[temp_Content] FROM [Content]
	SELECT * INTO [dbo].[temp_ContentGroup] FROM [ContentGroup]
	SELECT * INTO [dbo].[temp_ContentOverride] FROM [ContentOverride]
END



-- Delete the existing tables
BEGIN
	TRUNCATE TABLE [Content]
	TRUNCATE TABLE [ContentOverride]
	TRUNCATE TABLE [ContentGroup]
	
END





