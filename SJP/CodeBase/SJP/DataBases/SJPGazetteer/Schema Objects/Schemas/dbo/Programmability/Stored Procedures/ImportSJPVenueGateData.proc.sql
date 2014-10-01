
CREATE  PROCEDURE [dbo].[ImportSJPVenueGateData]
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
SET @XMLPathData =  'PublicationDelivery/dataObjects/CompositeFrame/frames/SiteFrame/pointsOfInterest/PointOfInterest/entrances/PointOfInterestEntrance'

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJPVenueGates table
	DELETE FROM dbo.SJPVenueGates

	-- deleting from SJPVenueGateCheckConstraints table 
	DELETE FROM dbo.SJPVenueGateCheckConstraints

	--Deleting existing SJPVenueGateNavigationPaths table
	DELETE FROM  dbo.SJPVenueGateNavigationPaths
	
	-- Insert additional venue data
	INSERT INTO dbo.SJPVenueGates(
			[EntranceNaPTAN]
           ,[EntranceName]
           ,[Easting]
           ,[Northing]
           ,[AvailableFrom]
           ,[AvailableTo]) 
  	SELECT
		 SUBSTRING(X.[Id],6,(LEN(X.[Id]) - 5)) 
		,X.[Name]
		,CAST (SUBSTRING(X.[Coordinates],1,6) AS INT)
        ,CAST (SUBSTRING(X.[Coordinates],8,6) AS INT)
        ,CAST ((X.[FromDate]) AS DATETIME)
        ,CAST ((X.[ToDate]) AS DATETIME)
	FROM
	OPENXML (@DocID,@XMLPathData , 2)
	WITH
	(
		[Id] [nvarchar](50),
		[Name] [nvarchar] (150),
		[Coordinates] [nvarchar] (13) 'Centroid/Location/Coordinates',
		[FromDate] [datetime] 'validityConditions/AvailabilityCondition/FromDate',
		[ToDate] [datetime] 'validityConditions/AvailabilityCondition/ToDate'
	) X 
	
SET @XMLPathData =  'PublicationDelivery/dataObjects/CompositeFrame/frames/SiteFrame/pointsOfInterest/PointOfInterest/entrances/PointOfInterestEntrance/checkConstraints/CheckConstraint'

	INSERT INTO dbo.SJPVenueGateCheckConstraints (
		 [CheckConstraintID]
        ,[CheckConstraintName]
        ,[Entry]
        ,[Process]
        ,[Congestion]
        ,[AverageDelay]
        ,[GateNaptan]
	) 
	SELECT DISTINCT 
		 X.[Id] 
        ,X.[Name]
        ,CASE WHEN X.CheckDirection = 'forwards' THEN 1 ELSE 0 END
        ,X.[CheckProcess]
        ,X.[Congestion]
        ,CASE WHEN CAST(SUBSTRING(X.[AverageDelay],3,CHARINDEX('M',X.[AverageDelay])-3) as INT) > 59 
			THEN CAST( 
			CAST(CAST((CAST(SUBSTRING(X.[AverageDelay],3,CHARINDEX('M',X.[AverageDelay])-3) as INT)/60) as INT) as varchar(2))
				+ 
				':' 
				+ 
				CAST(CAST((CAST(SUBSTRING(X.[AverageDelay],3,CHARINDEX('M',X.[AverageDelay])-3) as INT)%60) as INT) as varchar(2))
				+
				':'
				+
				SUBSTRING(X.[AverageDelay],CHARINDEX('M',X.[AverageDelay])+1,CHARINDEX('S',X.[AverageDelay]) - CHARINDEX('M',X.[AverageDelay]) -1) 
			as TIME)
		    ELSE
		    CAST('00:' + REPLACE(REPLACE(REPLACE(X.[AverageDelay],'PT',''),'M',':'),'S','') AS TIME)
		END
        ,SUBSTRING(X.[VenueGate],6,(LEN(X.[VenueGate])-5))
 	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[Id] [nvarchar](50) ,
		[Name] [nvarchar](150),
		[CheckDirection] [nvarchar] (20),
		[CheckProcess] [nvarchar](50),
		[Congestion] [nvarchar](50),
		[AverageDelay] [nvarchar] (20) 'delays/CheckConstraintDelay/AverageDelay',
		[VenueGate] [nvarchar] (20) '../../Id'
	) X

SET @XMLPathData =  'PublicationDelivery/dataObjects/CompositeFrame/frames/SiteFrame/navigationPaths/NavigationPath'

	-- Insert venue gate navigation path data
	INSERT INTO dbo.SJPVenueGateNavigationPaths(
		 [NavigationPathID]
        ,[NavigationPathName]
        ,[FromNaPTAN]
        ,[ToNaPTAN]
        ,[TransferDuration]
        ,[TransferDistance]
        ,[GateNaptan]
	) 
	SELECT DISTINCT 
		 X.[Id]
        ,X.[Name]
        ,ISNULL(SUBSTRING(X.[FromEntranceRef],6,LEN(X.[FromEntranceRef])-5),SUBSTRING(X.[FromPlaceRef],6,LEN(X.[FromPlaceRef])-5))
        ,ISNULL(SUBSTRING(X.[ToEntranceRef],6,LEN(X.[ToEntranceRef])-5),SUBSTRING(X.[ToPlaceRef],6,LEN(X.[ToPlaceRef])-5))
        ,CASE WHEN CAST(SUBSTRING(X.[DefaultDuration],3,CHARINDEX('M',X.[DefaultDuration])-3) as INT) > 59 
			THEN CAST( 
			CAST(CAST((CAST(SUBSTRING(X.[DefaultDuration],3,CHARINDEX('M',X.[DefaultDuration])-3) as INT)/60) as INT) as varchar(2))
				+ 
				':' 
				+ 
				CAST(CAST((CAST(SUBSTRING(X.[DefaultDuration],3,CHARINDEX('M',X.[DefaultDuration])-3) as INT)%60) as INT) as varchar(2))
				+
				':'
				+
				SUBSTRING(X.[DefaultDuration],CHARINDEX('M',X.[DefaultDuration])+1,CHARINDEX('S',X.[DefaultDuration]) - CHARINDEX('M',[DefaultDuration]) -1) 
			as TIME)
		    ELSE
		    CAST('00:' + REPLACE(REPLACE(REPLACE(X.[DefaultDuration],'PT',''),'M',':'),'S','') AS TIME)
			END
        ,CAST (X.[Distance] AS INT)
        ,ISNULL(SUBSTRING(X.[FromEntranceRef],6,LEN(X.[FromEntranceRef])-5),SUBSTRING(X.[ToEntranceRef],6,LEN(X.[ToEntranceRef])-5))
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[Id] [nvarchar](50),
		[Name] [nvarchar](150),
		[FromEntranceRef] [nvarchar] (30) 'From/EntranceRef/@ref',
		[FromPlaceRef] [nvarchar] (30) 'From/PlaceRef/@ref',
		[ToEntranceRef] [nvarchar] (30) 'To/EntranceRef/@ref',
		[ToPlaceRef] [nvarchar] (30) 'To/PlaceRef/@ref',
		[DefaultDuration] [nvarchar](30) 'TransferDuration/DefaultDuration',
		[Distance] [int]
	) X

COMMIT TRANSACTION 

-- Removing xml doc from memory
EXEC sp_xml_removedocument @DocID