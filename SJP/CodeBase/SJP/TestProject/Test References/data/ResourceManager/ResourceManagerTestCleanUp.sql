--SQLHelperTest Clean up



if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_ContentGroup') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to ContentGroup table
		TRUNCATE TABLE [ContentGroup]
		
		-- insert data in ContentGroup table
		INSERT INTO [ContentGroup]
				([GroupId]
				,[Name])
			SELECT *
			FROM dbo.[temp_ContentGroup]

		-- and delete the temp tables
		DROP TABLE [temp_ContentGroup]
		
	END

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_Content') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to Content table
		TRUNCATE TABLE [Content]
		
		-- insert data in Content table
		INSERT INTO [Content]
				([GroupId]
				   ,[CultureCode]
				   ,[ControlName]
				   ,[PropertyName]
				   ,[ContentValue])
			SELECT *
			FROM dbo.[temp_Content]

		
		-- and delete the temp tables
		DROP TABLE [temp_Content]
		
	END

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_ContentOverride') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to ContentOverride table
		TRUNCATE TABLE [ContentOverride]
		
		-- insert data in ContentOverride table
		INSERT INTO [ContentOverride]
				([GroupId]
				   ,[CultureCode]
				   ,[ControlName]
				   ,[PropertyName]
				   ,[ContentValue]
				   ,[StartDate]
				   ,[EndDate])
			SELECT *
			FROM dbo.[temp_ContentOverride]

		
		-- and delete the temp tables
		DROP TABLE [temp_ContentOverride]
		
	END

