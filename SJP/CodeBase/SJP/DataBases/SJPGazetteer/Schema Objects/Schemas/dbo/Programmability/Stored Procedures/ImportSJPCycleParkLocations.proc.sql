CREATE  PROCEDURE [dbo].[ImportSJPCycleParkLocations]
(
	@XML text
)
AS
/*
SP Name: ImportSJPCycleParkLocations
Input : XML string as text
Output: None
Description:  It takes xml data as string and inserts the data into SJP Cycle park tables.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(100)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  '/SJPCycleParkLocations/OlympicVenue/OlympicCyclePark'

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJPCycleParksCycleParkAvailability table
	DELETE FROM dbo.SJPCycleParksCycleParkAvailability

	-- deleting from SJPCycleParkAvailability table 
	DELETE FROM dbo.SJPCycleParkAvailability

	--Deleting existing SJPCycleParks table
	DELETE FROM  dbo.SJPCycleParks
	
	
	-- Insert cycle parks
	INSERT INTO dbo.SJPCycleParks(
			[CycleParkID]
           ,[CycleParkName]
           ,[VenueServed]
           ,[CycleParkMapURL]
           ,[NumberOfSpaces]
           ,[CycleToEasting]
           ,[CycleToNorthing]
           ,[CycleFromEasting]
           ,[CycleFromNorthing]
           ,[StorageType]
		   ,[WalkToGateDuration]
		   ,[VenueEntranceGate]
		   ,[WalkFromGateDuration]
		   ,[VenueExitGate]) 
	SELECT
		X.CycleParkID,
		X.[CycleParkName],
		X.[VenueServed],
		X.[CycleParkMapURL],
		X.[NumberOfSpaces],
		X.[CycleToEasting],
		X.[CycleToNorthing],
		X.[CycleFromEasting],
		X.[CycleFromNorthing],
		X.[StorageType],
		X.[WalkToGateDuration],
		X.[VenueEntranceGate],
		X.[WalkFromGateDuration],
		X.[VenueExitGate]
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[CycleParkID] [nvarchar](50),
		[CycleParkName] [nvarchar](150),
		[VenueServed] [nvarchar](20) '../VenueNaPTAN',
		[CycleParkMapURL] [nvarchar](150) '../VenueCycleParkingMapURL',
		[NumberOfSpaces] [int] ,
		[CycleToEasting] [int] 'CycleToCoordinate/Easting',
		[CycleToNorthing] [int] 'CycleToCoordinate/Northing',
		[CycleFromEasting] [int] 'CycleFromCoordinate/Easting',
		[CycleFromNorthing] [int]'CycleFromCoordinate/Northing',
		[StorageType] [nvarchar](50) 'CycleStorageType',
		[WalkToGateDuration] [time](0) 'VenueGatesToBeUsed/WalkToGateDuration',
		[VenueEntranceGate] [nvarchar](50) 'VenueGatesToBeUsed/VenueEntranceGate',
		[WalkFromGateDuration] [time](0) 'VenueGatesToBeUsed/WalkFromGateDuration',
		[VenueExitGate] [nvarchar](50) 'VenueGatesToBeUsed/VenueExitGate'
	) X 
	
SET @XMLPathData =  '/SJPCycleParkLocations/OlympicVenue/OlympicCyclePark/AvailabilityConditions/AvailabilityCondition'

	-- Insert availability conditions
	INSERT INTO dbo.SJPCycleParkAvailability([AvailabilityID],
		[FromDate],
		[ToDate],
		[DailyOpeningTime],
		[DailyClosingTime],
		[DaysOfWeek]) 
	SELECT DISTINCT 
		X.[AvailabilityID],
		X.[FromDate],
		X.[ToDate],
		X.[DailyOpeningTime],
		X.[DailyClosingTime],
		X.[DaysOfWeek]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[AvailabilityID] [nvarchar](50),
		[FromDate] [date] ,
		[ToDate] [date] ,
		[DailyOpeningTime] [time](0),
		[DailyClosingTime] [time](0),
		[DaysOfWeek] [nvarchar](30)
	) X
	
	-- For any cycle parks where no Availability Conditions have been defined create a default
	INSERT INTO dbo.SJPCycleParkAvailability([AvailabilityID],
		[FromDate],
		[ToDate],
		[DailyOpeningTime],
		[DailyClosingTime],
		[DaysOfWeek])
	VALUES ('DEFAULT','2000-01-01','2100-01-01','00:00:00','23:59:59','Everyday')

SET @XMLPathData =  '/SJPCycleParkLocations/OlympicVenue/OlympicCyclePark'
	
	INSERT INTO dbo.SJPCycleParksCycleParkAvailability([CycleParkID],
	[AvailabilityID])
	SELECT
		X.[CycleParkID],
		'DEFAULT'
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[CycleParkID] [nvarchar](50)   'CycleParkID'
	) X


SET @XMLPathData =  '/SJPCycleParkLocations/OlympicVenue/OlympicCyclePark/AvailabilityConditions/AvailabilityCondition'

	-- Insert availability conditions
	INSERT INTO dbo.SJPCycleParksCycleParkAvailability([CycleParkID],
		[AvailabilityID])
	SELECT
		X.[CycleParkID],
		ISNULL (X.[AvailabilityID],'DEFAULT')
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[CycleParkID] [nvarchar](50)   '../../CycleParkID',
		[AvailabilityID] [nvarchar](50) 'AvailabilityID'
	) X  

	-- Delete Defaults for parks that had had availability defined
	DELETE
	FROM dbo.SJPCycleParksCycleParkAvailability
	WHERE CycleParkID IN 
	(	   
		SELECT CycleParkID
		FROM dbo.SJPCycleParksCycleParkAvailability
		GROUP BY CycleParkID
		HAVING (Count(CycleParkID) > 1)
	)
	AND AvailabilityID = 'DEFAULT'
	
COMMIT TRANSACTION 

-- Removing xml doc from memorry
EXEC sp_xml_removedocument @DocID

GO