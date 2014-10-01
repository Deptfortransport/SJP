CREATE  PROCEDURE [dbo].[ImportSJPParkAndRideLocations]
(
	@XML text
)
AS
/*
SP Name: ImportSJPParkAndRideLocations
Input : XML string as text
Output: None
Description:  It takes xml data as string and inserts the data into SJP park and ride location tables.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(150)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  '/SJPParkAndRideLocations/OlympicVenue/OlympicCarPark'

-- starting trasaction 
BEGIN TRANSACTION
	
    --Delete existing SJPCarParkTransitShuttlesTransitShuttlesAvailability table
	DELETE FROM  [dbo].[SJPCarParkTransitShuttlesTransitShuttlesAvailability]

    --Delete existing SJPTransitShuttlesAvailability table
	DELETE FROM  [dbo].[SJPTransitShuttlesAvailability]

	--Delete existing SJPCarParksCarParkTransitShuttles table
	DELETE FROM  [dbo].[SJPCarParksCarParkTransitShuttles]

	--Delete existing SJPCarParkTransitShuttleTransfers table
	DELETE FROM  [dbo].[SJPCarParkTransitShuttleTransfers]

	--Delete existing SJPCarParkTransitShuttles table
	DELETE FROM  [dbo].[SJPCarParkTransitShuttles]

	--Delete existing SJPCarParksCarParkAvailability table
	DELETE FROM [dbo].[SJPCarParksCarParkAvailability]
	
	--Delete existing SJPCarParkInformation table
	DELETE FROM [dbo].[SJPCarParkInformation]

	--Delete existing SJPCarParkAvailability table 
	DELETE FROM [dbo].[SJPCarParkAvailability]
	
	--Delete existing SJPCarParks table
	DELETE FROM  [dbo].[SJPCarParks]
	
	
	-- Insert car parks
	INSERT INTO [dbo].[SJPCarParks](
			[CarParkID],
			[CarParkName],
			[VenueServed],
			[MapOfSiteURL],
			[InterchangeDuration],
			[CoachSpaces],
			[CarSpaces],
			[DisabledSpaces],
			[BlueBadgeSpaces]) 
	SELECT
			[CarParkID],
			[CarParkName],
			[VenueServed],
			[MapOfSiteURL],
			[InterchangeDuration],
			ISNULL([CoachSpaces],0),
			ISNULL([CarSpaces],0),
			ISNULL([DisabledSpaces],0),
			ISNULL([BlueBadgeSpaces],0)
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[CarParkID] [nvarchar](50),
		[CarParkName] [nvarchar](150),
		[VenueServed] [nvarchar](20)  '../VenueNaPTAN',
		[MapOfSiteURL] [nvarchar](150),
		[InterchangeDuration] [int] 'ParkingInterchangeDuration',
		[CoachSpaces] [int] 'ParkingSpaces/CoachSpaces',
		[CarSpaces] [int] 'ParkingSpaces/CarSpaces',
		[DisabledSpaces] [int] 'ParkingSpaces/DisabledSpaces',
		[BlueBadgeSpaces] [int] 'ParkingSpaces/BlueBadgeSpaces'
	) X 
	
SET @XMLPathData =  '/SJPParkAndRideLocations/OlympicVenue/OlympicCarPark/AvailabilityConditions/AvailabilityCondition'

	-- Insert availability conditions
	INSERT INTO [dbo].[SJPCarParkAvailability](
		[AvailabilityID],
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
		[DaysOfWeek] [nvarchar](max)
	) X
	
	-- For any car parks where no Availability Conditions have been defined create a default
	INSERT INTO [dbo].[SJPCarParkAvailability](
		[AvailabilityID],
		[FromDate],
		[ToDate],
		[DailyOpeningTime],
		[DailyClosingTime],
		[DaysOfWeek])
	VALUES ('DEFAULT','2000-01-01','2100-01-01','00:00:00','00:00:00','Everyday')

SET @XMLPathData =  '/SJPParkAndRideLocations/OlympicVenue/OlympicCarPark'
	
	INSERT INTO [dbo].[SJPCarParksCarParkAvailability](
		[CarParkID],
		[AvailabilityID])
	SELECT
		X.[CarParkID],
		'DEFAULT'
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[CarParkID] [nvarchar](50)   'CarParkID'
	) X

SET @XMLPathData =  '/SJPParkAndRideLocations/OlympicVenue/OlympicCarPark/CarParkInformation'

	-- Insert Car Park Information 
	INSERT INTO [dbo].[SJPCarParkInformation](
		[CarParkID],
		[CultureCode],
		[InformationText])
	SELECT
		X.[CarParkID],
		X.[CarParkInformationLanguage],
		X.[CarParkInformationText]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[CarParkID]	[nvarchar] (50) '../CarParkID',
		[CarParkInformationLanguage]	[char]     (2),
		[CarParkInformationText]		[nvarchar] (max)
	) X

SET @XMLPathData =  '/SJPParkAndRideLocations/OlympicVenue/OlympicCarPark/AvailabilityConditions/AvailabilityCondition'

	-- Insert availability conditions
	INSERT INTO [dbo].[SJPCarParksCarParkAvailability](
		[CarParkID],
		[AvailabilityID])
	SELECT
		X.[CarParkID],
		ISNULL (X.[AvailabilityID],'DEFAULT')
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[CarParkID] [nvarchar](50) '../../CarParkID',
		[AvailabilityID] [nvarchar](50) 'AvailabilityID'
	) X 
	
	-- Delete Defaults for parks that had availability defined
	DELETE
	FROM [dbo].[SJPCarParksCarParkAvailability]
	WHERE CarParkID IN 
	(	   
		SELECT CarParkID
		FROM [dbo].[SJPCarParksCarParkAvailability]
		GROUP BY CarParkID
		HAVING (Count(CarParkID) > 1)
	)
	AND AvailabilityID = 'DEFAULT'

SET @XMLPathData =  '/SJPParkAndRideLocations/TransitShuttle'

	-- Insert Transit Shuttles
	INSERT INTO [dbo].[SJPCarParkTransitShuttles](
		[TransitShuttleID],
		[ToVenue],
		[ModeOfTransport],
		[TransitDuration],
		[IsScheduledService],
		[IsPRMOnly],
		[VenueGateNaPTAN],
		[ServiceFrequency],
		[FirstServiceOfDay],
		[LastServiceOfDay])
	SELECT
		X.[TransitShuttleID],
		X.[ToVenue],
		X.[ModeOfTransport],
		X.[TransitDuration],
		X.[IsScheduledService],
		X.[IsPRMOnly],
		X.[VenueGateToUse],
		X.[ServiceFrequency],
		X.[FirstServiceOfDay],
		X.[LastServiceOfDay]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[TransitShuttleID] [nvarchar](50),
		[ToVenue] [bit] ,
		[ModeOfTransport] [nvarchar](20),
		[TransitDuration] [int],
		[IsScheduledService] [bit],
		[IsPRMOnly] [bit],
		[VenueGateToUse] [nvarchar](20),
		[ServiceFrequency] [int] 'ShuttleFrequency/ServiceFrequency',
		[FirstServiceOfDay] [time](0) 'ShuttleFrequency/FirstOfDay',
		[LastServiceOfDay] [time](0) 'ShuttleFrequency/LastOfDay'
	) X  

SET @XMLPathData =  '/SJPParkAndRideLocations/TransitShuttle'

	-- Insert car park transit shuttles
	INSERT INTO [dbo].[SJPCarParksCarParkTransitShuttles](
		[CarParkID],
		[TransitShuttleID])
	SELECT
		X.[CarParkID],
		X.[TransitShuttleID]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[CarParkID] [nvarchar](50) 'CarParkServed',
		[TransitShuttleID] [nvarchar](50) 
	) X 
	
	INSERT INTO [dbo].[SJPCarParkTransitShuttlesTransitShuttlesAvailability](
		[TransitShuttleID],
		[TransitShuttleAvailabilityID])
	SELECT
		X.[TransitShuttleID],
		'DEFAULT'
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[TransitShuttleID] [nvarchar](50)   'TransitShuttleID'
	) X 

SET @XMLPathData =  '/SJPParkAndRideLocations/TransitShuttle/TransitJourneyDescription'

	-- Insert Transit Shuttle Transfer 
	INSERT INTO [dbo].[SJPCarParkTransitShuttleTransfers](
		[TransitShuttleID],
		[CultureCode],
		[TransferDescription])
	SELECT
		X.[TransitShuttleID],
		X.[TransitLanguage],
		X.[TransitText]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[TransitShuttleID]	[nvarchar] (50) '../TransitShuttleID',
		[TransitLanguage]	[char] (2),
		[TransitText]		[nvarchar] (max)
	) X

SET @XMLPathData =  '/SJPParkAndRideLocations/TransitShuttle/TransitShuttleAvailabilityConditions/TransitShuttleAvailabilityCondition'

	-- Insert transit shuttle availability conditions
	INSERT INTO [dbo].[SJPTransitShuttlesAvailability](
		[TransitShuttleAvailabilityID],
		[FromDate],
		[ToDate],
		[DailyStartTime],
		[DailyEndTime],
		[DaysOfWeek]) 
	SELECT DISTINCT 
		X.[TransitShuttleAvailabilityID],
		X.[FromDate],
		X.[ToDate],
		X.[DailyStartTime],
		X.[DailyEndTime],
		X.[DaysOfWeek]
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[TransitShuttleAvailabilityID] [nvarchar](50),
		[FromDate] [date] ,
		[ToDate] [date] ,
		[DailyStartTime] [time](0),
		[DailyEndTime] [time](0),
		[DaysOfWeek] [nvarchar](max)
	) X
	
	-- For any transit shuttles where no Availability Conditions have been defined create a default
	INSERT INTO [dbo].[SJPTransitShuttlesAvailability](
		[TransitShuttleAvailabilityID],
		[FromDate],
		[ToDate],
		[DailyStartTime],
		[DailyEndTime],
		[DaysOfWeek])
	VALUES ('DEFAULT','2000-01-01','2100-01-01','00:00:00','23:59:00','Everyday')

	-- Assign transit shuttle availabilty conditions to shuttles
	INSERT INTO [dbo].[SJPCarParkTransitShuttlesTransitShuttlesAvailability](
		[TransitShuttleID],
		[TransitShuttleAvailabilityID])
	SELECT
		X.[TransitShuttleID],
		ISNULL (X.[TransitShuttleAvailabilityID],'DEFAULT')
	FROM
	OPENXML (@DocID, @XMLPathData , 2)
	WITH
	(
		[TransitShuttleID] [nvarchar](50) '../../TransitShuttleID',
		[TransitShuttleAvailabilityID] [nvarchar](50)
	) X 
	
	-- Delete Defaults for parks that had availability defined
	DELETE
	FROM [dbo].[SJPCarParkTransitShuttlesTransitShuttlesAvailability]
	WHERE TransitShuttleID IN 
	(	   
		SELECT TransitShuttleID
		FROM [dbo].[SJPCarParkTransitShuttlesTransitShuttlesAvailability]
		GROUP BY TransitShuttleID
		HAVING (Count(TransitShuttleID) > 1)
	)
	AND TransitShuttleAvailabilityID = 'DEFAULT'

COMMIT TRANSACTION 

-- Removing xml doc from memorry
EXEC sp_xml_removedocument @DocID

GO