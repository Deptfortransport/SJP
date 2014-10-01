--DataServices Clean up

USE [SJPTransientPortal]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_CycleAttribute') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_AdminAreas') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_Districts') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_StopAccessibilityLinks') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_DropDownLists') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPEventDates') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to CycleAttribute table
		TRUNCATE TABLE [CycleAttribute]
		TRUNCATE TABLE [AdminAreas]
		TRUNCATE TABLE [Districts]
		TRUNCATE TABLE [StopAccessibilityLinks]
		TRUNCATE TABLE [DropDownLists]
		TRUNCATE TABLE [SJPEventDates]
		
		-- insert data in CycleAttribute table
		INSERT INTO [CycleAttribute]
				([CycleAttributeId]
				   ,[Description]
				   ,[Type]
				   ,[Group]
				   ,[Category]
				   ,[ResourceName]
				   ,[Mask]
				   ,[CycleInfrastructure]
				   ,[CycleRecommendedRoute]
				   ,[ShowAttribute])
			SELECT *
			FROM dbo.[temp_CycleAttribute]

		INSERT INTO [AdminAreas]
				([AdministrativeAreaCode]
				   ,[AtcoAreaCode]
				   ,[AreaName]
				   ,[AreaNameLang]
				   ,[ShortName]
				   ,[ShortNameLang]
				   ,[Country]
				   ,[RegionCode]
				   ,[MaximumLengthForShortNames ]
				   ,[National]
				   ,[ContactEmail]
				   ,[ContactTelephone]
				   ,[CreationDateTime]
				   ,[ModificationDateTime]
				   ,[RevisionNumber]
				   ,[Modification])
			SELECT *
			FROM dbo.[temp_AdminAreas]

		INSERT INTO [Districts]
				([DistrictCode]
				   ,[DistrictName]
				   ,[DistrictLang]
				   ,[AdministrativeAreaCode]
				   ,[CreationDateTime]
				   ,[ModificationDateTime]
				   ,[RevisionNumber]
				   ,[Modification])
			SELECT *
			FROM dbo.[temp_Districts]

		INSERT INTO [StopAccessibilityLinks]
				([StopNaPTAN]
				   ,[StopName]
				   ,[StopOperator]
				   ,[LinkUrl]
				   ,[WEFDate]
				   ,[WEUDate])
			SELECT *
			FROM dbo.[temp_StopAccessibilityLinks]

		INSERT INTO [DropDownLists]
				([DataSet]
				   ,[ResourceID]
				   ,[ItemValue]
				   ,[IsSelected]
				   ,[SortOrder])
			SELECT *
			FROM dbo.[temp_DropDownLists]

		INSERT INTO [SJPEventDates]
				([EventDate])
			SELECT *
			FROM dbo.[temp_SJPEventDates]
				
	
		-- and delete the temp tables
		DROP TABLE [temp_CycleAttribute]
		DROP TABLE [temp_AdminAreas]
		DROP TABLE [temp_Districts]
		DROP TABLE [temp_StopAccessibilityLinks]
		DROP TABLE [temp_DropDownLists]
		DROP TABLE [temp_SJPEventDates]
	END

