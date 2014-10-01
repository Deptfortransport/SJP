CREATE PROCEDURE [dbo].[ImportSJPTravelcardData]
(
	@XML text
)
AS
/*
SP Name: [ImportSJPTravelcardData]
Input : XML
Output: None
Description:  It takes XML data as string and inserts the data into [SJPTravelcards]
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(200)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete existing data
	DELETE FROM [dbo].[SJPZoneAreaPoints]
	DELETE FROM [dbo].[SJPZoneAreas]
	DELETE FROM [dbo].[SJPZoneModes]
	DELETE FROM [dbo].[SJPZoneStops]
	DELETE FROM [dbo].[SJPZones]

	DELETE FROM [dbo].[SJPRouteEndStops]
	DELETE FROM [dbo].[SJPRouteEndZones]
	DELETE FROM [dbo].[SJPRouteModes]
	DELETE FROM [dbo].[SJPRoutes]

	DELETE FROM [dbo].[SJPTravelcardZones]
	DELETE FROM [dbo].[SJPTravelcardRoutes]
	DELETE FROM [dbo].[SJPTravelcards]

	--------------------------------ZONES-----------------------------------
	-- Insert SJPZones data
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone'

	INSERT INTO [dbo].[SJPZones](
		 [ZoneID]
		,[ZoneName])
	SELECT
			X.[ZoneID]
           ,X.[ZoneName] 
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[ZoneID] [NVARCHAR] (20),
		[ZoneName] [NVARCHAR] (50)
	) X 
	
	-- Insert SJPZoneAreas data (Outer zone)
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/OuterZoneBoundary'

	INSERT INTO [dbo].[SJPZoneAreas](
		 [ZoneAreaID]
		,[ZoneID]
		,[IsOuterZoneArea])
	SELECT
			X.[ZoneAreaID] + '_OUT' -- Indicate it is the outer zone
		   ,X.[ZoneID]
           ,1 -- True
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneAreaID] [NVARCHAR] (20) '../../ZoneID',
		[ZoneID] [NVARCHAR] (20) '../../ZoneID'
	) X 
	
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/OuterZoneBoundary/Point'

	INSERT INTO [dbo].[SJPZoneAreaPoints](
		 [ZoneAreaID]
		,[Easting]
		,[Northing])
	SELECT
			X.[ZoneAreaID] + '_OUT' -- Indicate it is the outer zone
		   ,X.[Easting]
           ,X.[Northing]
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneAreaID] [NVARCHAR] (20) '../../../ZoneID',
		[Easting] INT,
		[Northing] INT
	) X 
	
	-- Insert SJPZoneAreas data (Inner zone)
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/InnerZoneBoundary'

	INSERT INTO [dbo].[SJPZoneAreas](
		 [ZoneAreaID]
		,[ZoneID]
		,[IsOuterZoneArea])
	SELECT
			X.[ZoneAreaID] + '_IN' -- Indicate it is the outer zone
		   ,X.[ZoneID]
           ,1 -- True
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneAreaID] [NVARCHAR] (20) '../../ZoneID',
		[ZoneID] [NVARCHAR] (20) '../../ZoneID'
	) X 
	
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/InnerZoneBoundary/Point'

	INSERT INTO [dbo].[SJPZoneAreaPoints](
		 [ZoneAreaID]
		,[Easting]
		,[Northing])
	SELECT
			X.[ZoneAreaID] + '_IN' -- Indicate it is the inner zone
		   ,X.[Easting]
           ,X.[Northing]
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneAreaID] [NVARCHAR] (20) '../../../ZoneID',
		[Easting] INT,
		[Northing] INT
	) X 
	
	-- Insert SJPZoneModes data
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/ZoneAreaModesIncluded/Mode'

	INSERT INTO [dbo].[SJPZoneModes](
		 [ZoneID]
		,[ModeOfTransport]
		,[IsExcluded])
	SELECT
		    X.[ZoneID]
		   ,X.[Mode]
           ,0 -- Included modes
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneID] [NVARCHAR] (20) '../../../ZoneID',
		[Mode] [NVARCHAR] (20) 'text()'
	) X 

	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/ZoneAreaModesExcluded/Mode'

	INSERT INTO [dbo].[SJPZoneModes](
		 [ZoneID]
		,[ModeOfTransport]
		,[IsExcluded])
	SELECT
		    X.[ZoneID]
		   ,X.[Mode]
           ,1 -- Excluded modes
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneID] [NVARCHAR] (20) '../../../ZoneID',
		[Mode] [NVARCHAR] (20) 'text()'
	) X 
	
	-- Insert SJPZoneStops data
	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/ZoneStopsIncluded/StopID'

	INSERT INTO [dbo].[SJPZoneStops](
		 [ZoneID]
		,[NaPTAN]
		,[IsExcluded])
	SELECT
		    X.[ZoneID]
		   ,X.[StopID]
           ,0 -- Included stops
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneID] [NVARCHAR] (20) '../../../ZoneID',
		[StopID] [NVARCHAR] (20) 'text()'
	) X 

	SET @XMLPathData =  '/SJPTravelCards/Zones/Zone/ZoneArea/ZoneStopsExcluded/StopID'

	INSERT INTO [dbo].[SJPZoneStops](
		 [ZoneID]
		,[NaPTAN]
		,[IsExcluded])
	SELECT
		    X.[ZoneID]
		   ,X.[StopID]
           ,1 -- Excluded stops
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[ZoneID] [NVARCHAR] (20) '../../../ZoneID',
		[StopID] [NVARCHAR] (20) 'text()'
	) X 
	
	--------------------------------ROUTES-----------------------------------
	-- Insert SJPRoutes data
	SET @XMLPathData =  '/SJPTravelCards/Routes/Route'

	INSERT INTO [dbo].[SJPRoutes](
		 [RouteID]
		,[RouteName])
	SELECT
			X.[RouteID]
           ,X.[RouteName] 
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[RouteID] [NVARCHAR] (20),
		[RouteName] [NVARCHAR] (50)
	) X 

	-- Insert SJPRouteEndZones data
	SET @XMLPathData =  '/SJPTravelCards/Routes/Route/EndA/RouteEnd/RouteZones/ZoneID'

	INSERT INTO [dbo].[SJPRouteEndZones](
		 [RouteID]
		,[ZoneID]
		,[IsEndA])
	SELECT
			X.[RouteID]
           ,X.[ZoneID]
		   ,1	-- This is End A
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[RouteID] [NVARCHAR] (20) '../../../../RouteID',
		[ZoneID] [NVARCHAR] (20) 'text()'
	) X 

	SET @XMLPathData =  '/SJPTravelCards/Routes/Route/EndB/RouteEnd/RouteZones/ZoneID'

	INSERT INTO [dbo].[SJPRouteEndZones](
		 [RouteID]
		,[ZoneID]
		,[IsEndA])
	SELECT
			X.[RouteID]
           ,X.[ZoneID]
		   ,0	-- This is End B
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[RouteID] [NVARCHAR] (20) '../../../../RouteID',
		[ZoneID] [NVARCHAR] (20) 'text()'
	) X 

	-- Insert SJPRouteEndStops data
	SET @XMLPathData =  '/SJPTravelCards/Routes/Route/EndA/RouteEnd/RouteStops/StopID'

	INSERT INTO [dbo].[SJPRouteEndStops](
		 [RouteID]
		,[NaPTAN]
		,[IsEndA])
	SELECT
			X.[RouteID]
           ,X.[StopID]
		   ,1	-- This is End A
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[RouteID] [NVARCHAR] (20) '../../../../RouteID',
		[StopID] [NVARCHAR] (20) 'text()'
	) X 

	SET @XMLPathData =  '/SJPTravelCards/Routes/Route/EndB/RouteEnd/RouteStops/StopID'

	INSERT INTO [dbo].[SJPRouteEndStops](
		 [RouteID]
		,[NaPTAN]
		,[IsEndA])
	SELECT
			X.[RouteID]
           ,X.[StopID]
		   ,0	-- This is End B
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[RouteID] [NVARCHAR] (20) '../../../../RouteID',
		[StopID] [NVARCHAR] (20) 'text()'
	) X 

	-- Insert SJPRouteModes data
	SET @XMLPathData =  '/SJPTravelCards/Routes/Route/RouteModesIncluded/Mode'

	INSERT INTO [dbo].[SJPRouteModes](
		 [RouteID]
		,[ModeOfTransport]
		,[IsExcluded])
	SELECT
		    X.[RouteID]
		   ,X.[Mode]
           ,0 -- Included
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[RouteID] [NVARCHAR] (20) '../../RouteID',
		[Mode] [NVARCHAR] (20) 'text()'
	) X 

	SET @XMLPathData =  '/SJPTravelCards/Routes/Route/RouteModesExcluded/Mode'

	INSERT INTO [dbo].[SJPRouteModes](
		 [RouteID]
		,[ModeOfTransport]
		,[IsExcluded])
	SELECT
		    X.[RouteID]
		   ,X.[Mode]
           ,1 -- Excluded
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[RouteID] [NVARCHAR] (20) '../../RouteID',
		[Mode] [NVARCHAR] (20) 'text()'
	) X 

	--------------------------------TRAVELCARDS-----------------------------------
	-- Insert SJPTravelcards data
	SET @XMLPathData =  '/SJPTravelCards/TravelCards/TravelCard'

	INSERT INTO [dbo].[SJPTravelcards](
		 [TravelCardID]
		,[TravelCardName]
		,[ValidFrom]
		,[ValidTo])
	SELECT
			X.[TravelCardID]
           ,X.[TravelCardName] 
           ,CAST ((X.[ValidFrom]) AS DATE)
           ,CAST ((X.[ValidTo]) AS DATE)
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[TravelCardID] [NVARCHAR] (20),
		[TravelCardName] [NVARCHAR] (50),
		[ValidFrom] [DATE],
		[ValidTo] [DATE]
	) X 

	-- Insert SJPTravelcardZones data
	SET @XMLPathData =  '/SJPTravelCards/TravelCards/TravelCard/TravelCardZonesIncluded/ZoneID'

	INSERT INTO [dbo].[SJPTravelcardZones](
		 [TravelCardID]
		,[ZoneID]
		,[IsExcluded])
	SELECT
		    X.[TravelCardID]
		   ,X.[ZoneID]
           ,0 -- Included
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[TravelCardID] [NVARCHAR] (20) '../../TravelCardID',
		[ZoneID] [NVARCHAR] (20) 'text()'
	) X
	
	SET @XMLPathData =  '/SJPTravelCards/TravelCards/TravelCard/TravelCardZonesExcluded/ZoneID'

	INSERT INTO [dbo].[SJPTravelcardZones](
		 [TravelCardID]
		,[ZoneID]
		,[IsExcluded])
	SELECT
		    X.[TravelCardID]
		   ,X.[ZoneID]
           ,1 -- Excluded
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[TravelCardID] [NVARCHAR] (20) '../../TravelCardID',
		[ZoneID] [NVARCHAR] (20) 'text()'
	) X 

	-- Insert SJPTravelcardRoutes data
	SET @XMLPathData =  '/SJPTravelCards/TravelCards/TravelCard/TravelCardRoutesIncluded/RouteID'

	INSERT INTO [dbo].[SJPTravelcardRoutes](
		 [TravelCardID]
		,[RouteID]
		,[IsExcluded])
	SELECT
		    X.[TravelCardID]
		   ,X.[RouteID]
           ,0 -- Included
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[TravelCardID] [NVARCHAR] (20) '../../TravelCardID',
		[RouteID] [NVARCHAR] (20) 'text()'
	) X 

	SET @XMLPathData =  '/SJPTravelCards/TravelCards/TravelCard/TravelCardRoutesExcluded/RouteID'

	INSERT INTO [dbo].[SJPTravelcardRoutes](
		 [TravelCardID]
		,[RouteID]
		,[IsExcluded])
	SELECT
		    X.[TravelCardID]
		   ,X.[RouteID]
           ,1 -- Excluded
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	[TravelCardID] [NVARCHAR] (20) '../../TravelCardID',
		[RouteID] [NVARCHAR] (20) 'text()'
	) X 
		

COMMIT TRANSACTION 

-- Removing xml doc from memory
EXEC sp_xml_removedocument @DocID
