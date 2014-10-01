--DataServices Clean up

USE [SJPTransientPortal]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZones') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneAreas') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneAreaPoints') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneStops') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneModes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRoutes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRouteEndStops') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRouteEndZones') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRouteModes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPTravelcards') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPTravelcardZones') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPTravelcardRoutes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to table
		TRUNCATE TABLE [SJPZones]
		TRUNCATE TABLE [SJPZoneAreas]
		TRUNCATE TABLE [SJPZoneAreaPoints]
		TRUNCATE TABLE [SJPZoneStops]
		TRUNCATE TABLE [SJPZoneModes]
		TRUNCATE TABLE [SJPRoutes]
		TRUNCATE TABLE [SJPRouteEndStops]
		TRUNCATE TABLE [SJPRouteEndZones]
		TRUNCATE TABLE [SJPRouteModes]
		TRUNCATE TABLE [SJPTravelcards]
		TRUNCATE TABLE [SJPTravelcardZones]
		TRUNCATE TABLE [SJPTravelcardRoutes]
		
		-- insert data in table
		INSERT INTO [SJPTransientPortal].[dbo].[SJPZones]
           ([ZoneID]
           ,[ZoneName])
		SELECT * FROM [dbo].[temp_SJPZones]
        

		INSERT INTO [SJPTransientPortal].[dbo].[SJPZoneAreas]
				   ([ZoneAreaID]
				   ,[ZoneID]
				   ,[IsOuterZoneArea])
		SELECT * FROM [dbo].[temp_SJPZoneAreas]


		INSERT INTO [SJPTransientPortal].[dbo].[SJPZoneAreaPoints]
				   ([ZoneAreaID]
				   ,[Easting]
				   ,[Northing])
		SELECT * FROM [dbo].[temp_SJPZoneAreaPoints]
		     

		INSERT INTO [SJPTransientPortal].[dbo].[SJPZoneStops]
				   ([ZoneID]
				   ,[NaPTAN]
			   ,[IsExcluded])
		SELECT * FROM [dbo].[temp_SJPZoneStops]
		    

		INSERT INTO [SJPTransientPortal].[dbo].[SJPZoneModes]
				   ([ZoneID]
				   ,[ModeOfTransport]
				   ,[IsExcluded])
		SELECT * FROM [dbo].[temp_SJPZoneModes]

		
		INSERT INTO [SJPTransientPortal].[dbo].[SJPRoutes]
				   ([RouteID]
				   ,[RouteName])
		SELECT * FROM [dbo].[temp_SJPRoutes]


		INSERT INTO [SJPTransientPortal].[dbo].[SJPRouteEndStops]
				   ([RouteID]
				   ,[NaPTAN]
				   ,[IsEndA])
		SELECT * FROM [dbo].[temp_SJPRouteEndStops]


		INSERT INTO [SJPTransientPortal].[dbo].[SJPRouteEndZones]
				   ([RouteID]
				   ,[ZoneID]
				   ,[IsEndA])
		SELECT * FROM [dbo].[temp_SJPRouteEndZones]


		INSERT INTO [SJPTransientPortal].[dbo].[SJPRouteModes]
				   ([RouteID]
				   ,[ModeOfTransport]
				   ,[IsExcluded])
		SELECT * FROM [dbo].[temp_SJPRouteModes]

		INSERT INTO [SJPTransientPortal].[dbo].[SJPTravelcards]
				   ([TravelCardID]
				   ,[TravelCardName]
				   ,[ValidFrom]
				   ,[ValidTo])
		SELECT * FROM [dbo].[temp_SJPTravelcards]
		    

		INSERT INTO [SJPTransientPortal].[dbo].[SJPTravelcardZones]
				   ([TravelCardID]
				   ,[ZoneID]
				   ,[IsExcluded])
		SELECT * FROM [dbo].[temp_SJPTravelcardZones]
		  

		INSERT INTO [SJPTransientPortal].[dbo].[SJPTravelcardRoutes]
				   ([TravelCardID]
				   ,[RouteID]
				   ,[IsExcluded])
		SELECT * FROM [dbo].[temp_SJPTravelcardRoutes]
				
	
		-- and delete the temp tables
		DROP TABLE [dbo].[temp_SJPZones]
		DROP TABLE [dbo].[temp_SJPZoneAreas]
		DROP TABLE [dbo].[temp_SJPZoneAreaPoints]
		DROP TABLE [dbo].[temp_SJPZoneStops]
		DROP TABLE [dbo].[temp_SJPZoneModes]
		DROP TABLE [dbo].[temp_SJPRoutes]
		DROP TABLE [dbo].[temp_SJPRouteEndStops]
		DROP TABLE [dbo].[temp_SJPRouteEndZones]
		DROP TABLE [dbo].[temp_SJPRouteModes]
		DROP TABLE [dbo].[temp_SJPTravelcards]
		DROP TABLE [dbo].[temp_SJPTravelcardZones]
		DROP TABLE [dbo].[temp_SJPTravelcardRoutes]
	
	END

