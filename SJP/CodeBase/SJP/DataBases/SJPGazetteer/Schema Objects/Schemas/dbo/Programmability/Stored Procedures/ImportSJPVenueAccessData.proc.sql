CREATE PROCEDURE [dbo].[ImportSJPVenueAccessData]
(
	@XML text
)
AS

/*
SP Name: ImportSJPVenueGateData
Input : XML string as text
Output: None
Description:  It takes xml data as string and inserts the data into SJP venue additional data.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(200)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  'VenuesAccessData/Venue/VenueAccessPeriod/VenueAccessGatewayStations/VenueStation'

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJPVenueAccessData table
	DELETE FROM [dbo].[SJPVenueAccessData]
	DELETE FROM [dbo].[SJPVenueAccessTransfers]

	-- Insert venue access data
	INSERT INTO [dbo].[SJPVenueAccessData](
			[VenueNaPTAN]
           ,[VenueName]
           ,[AccessFrom]
           ,[AccessTo]
           ,[AccessDuration]
           ,[StationNaPTAN]
           ,[StationName]
           ) 
  	SELECT
		 X.[VenueNaPTAN]
		,X.[VenueName]
		,X.[AccessPeriodFromDate]
		,X.[AccessPeriodUntilDate]
		,X.[AccessToVenueDuration]
		,X.[VenueAccessStationNaPTAN]
		,X.[VenueAccessStationName]
	FROM
	OPENXML (@DocID,@XMLPathData , 2)
	WITH
	(
		[VenueNaPTAN]              [nvarchar] (20)  '../../../VenueNaPTAN',
		[VenueName]                [nvarchar] (150) '../../../VenueName',
		[AccessPeriodFromDate]     [datetime]       '../../AccessPeriodFromDate',
		[AccessPeriodUntilDate]    [datetime]       '../../AccessPeriodUntilDate',
		[AccessToVenueDuration]    [time]     (0)   '../AccessToVenueDuration',
		[VenueAccessStationNaPTAN] [nvarchar] (20),
		[VenueAccessStationName]   [nvarchar] (150)
	) X 


SET @XMLPathData =  'VenuesAccessData/Venue/VenueAccessPeriod/VenueAccessGatewayStations/VenueStation/VenueAccessStationOutwardDescription'

	-- Insert transfers to venue
	INSERT INTO [dbo].[SJPVenueAccessTransfers](
			 [VenueNaPTAN]
			,[StationNaPTAN]
			,[ToVenue]
			,[CultureCode]
			,[TransferDescription]
		)
	SELECT
		 X.[VenueNaPTAN]
		,X.[VenueAccessStationNaPTAN]
		,1 -- ToVenue = True
		,X.[VenueAccessTextLanguage]
		,X.[VenueAccessDescription]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[VenueNaPTAN]              [nvarchar] (20) '../../../../VenueNaPTAN',
		[VenueAccessStationNaPTAN] [nvarchar] (20) '../VenueAccessStationNaPTAN',
		[VenueAccessTextLanguage]  [char] (2),
		[VenueAccessDescription]   [nvarchar] (max)
	) X 


SET @XMLPathData =  'VenuesAccessData/Venue/VenueAccessPeriod/VenueAccessGatewayStations/VenueStation/VenueAccessStationReturnDescription'

	-- Insert transfers from venue
	INSERT INTO [dbo].[SJPVenueAccessTransfers](
			 [VenueNaPTAN]
			,[StationNaPTAN]
			,[ToVenue]
			,[CultureCode]
			,[TransferDescription]
		)
	SELECT
		 X.[VenueNaPTAN]
		,X.[VenueAccessStationNaPTAN]
		,0 -- ToVenue = False
		,X.[VenueAccessTextLanguage]
		,X.[VenueAccessDescription]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[VenueNaPTAN]              [nvarchar] (20) '../../../../VenueNaPTAN',
		[VenueAccessStationNaPTAN] [nvarchar] (20) '../VenueAccessStationNaPTAN',
		[VenueAccessTextLanguage]  [char] (2),
		[VenueAccessDescription]   [nvarchar] (max)
	) X 



COMMIT TRANSACTION 

-- Removing xml doc from memory
EXEC sp_xml_removedocument @DocID