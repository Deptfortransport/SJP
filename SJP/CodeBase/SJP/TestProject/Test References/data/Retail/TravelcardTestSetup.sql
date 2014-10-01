
USE [SJPTransientPortal]


BEGIN
-- Delete Temp table so we start cleanly
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZones') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPZones]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneAreas') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPZoneAreas]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneAreaPoints') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPZoneAreaPoints]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneStops') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPZoneStops]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPZoneModes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPZoneModes]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRoutes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPRoutes]
	
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRouteEndStops') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPRouteEndStops]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRouteEndZones') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPRouteEndZones]
	
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPRouteModes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPRouteModes]
	
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPTravelcards') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPTravelcards]
	
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPTravelcardZones') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPTravelcardZones]
	
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPTravelcardRoutes') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPTravelcardRoutes]
	
END


-- Copy exisiting data into new temp tables
BEGIN
	SELECT * INTO [dbo].[temp_SJPZones] FROM [SJPZones]
	SELECT * INTO [dbo].[temp_SJPZoneAreas] FROM [SJPZoneAreas]
	SELECT * INTO [dbo].[temp_SJPZoneAreaPoints] FROM [SJPZoneAreaPoints]
	SELECT * INTO [dbo].[temp_SJPZoneStops] FROM [SJPZoneStops]
	SELECT * INTO [dbo].[temp_SJPZoneModes] FROM [SJPZoneModes]
	SELECT * INTO [dbo].[temp_SJPRoutes] FROM [SJPRoutes]
	SELECT * INTO [dbo].[temp_SJPRouteEndStops] FROM [SJPRouteEndStops]
	SELECT * INTO [dbo].[temp_SJPRouteEndZones] FROM [SJPRouteEndZones]
	SELECT * INTO [dbo].[temp_SJPRouteModes] FROM [SJPRouteModes]
	SELECT * INTO [dbo].[temp_SJPTravelcards] FROM [SJPTravelcards]
	SELECT * INTO [dbo].[temp_SJPTravelcardZones] FROM [SJPTravelcardZones]
	SELECT * INTO [dbo].[temp_SJPTravelcardRoutes] FROM [SJPTravelcardRoutes]
END



-- Delete the existing data
BEGIN
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
END

