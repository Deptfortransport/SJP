--SJPTransientPortal Clean up

-- SQL removes all the temporary data from the SJPTransientPortal Travel News tables

USE [SJPTransientPortal]

DECLARE @TestPrefix AS varchar(10), 
	    @IncidentId1 AS varchar(20),
		@IncidentId2 AS varchar(20),
		@IncidentId3 AS varchar(20),
		@DataSourceId AS varchar(20)

SET @TestPrefix = 'SJPTest'
SET @IncidentId1 = 'SJP0000001'
SET @IncidentId2 = 'SJP0000002'
SET @IncidentId3 = 'SJP0000003'
SET @DataSourceId = @TestPrefix + 'SJP'

IF NOT EXISTS (SELECT * FROM [dbo].[TravelNews] WHERE [UID] = @IncidentId1)
BEGIN

	-- Serious incident with "Test" search phrase
	INSERT INTO [dbo].[TravelNews] ([UID], [SeverityLevel], [SeverityLevelOlympic], 
		[PublicTransportOperator], [ModeOfTransport], [Regions], [Location], [IncidentType], 
		[HeadlineText], [DetailText], 
		[TravelAdviceOlympicText],
		[IncidentStatus], [Easting], [Northing], [ReportedDateTime], [StartDateTime], [LastModifiedDateTime],
		[ClearedDateTime], [ExpiryDateTime], [PlannedIncident], 
		[RoadType], [IncidentParent], [CarriagewayDirection], [RoadNumber],
		[DayMask], [DailyStartTime], [DailyEndTime], [ItemChangeStatus])
    VALUES
           (@IncidentId1, 1, 2,
		   'East London Line', 'Underground', 'London', 'East London Line London', 'Station/Port Disruption',
		   @TestPrefix + ' Test ' + 'East London Line closed until Summer 2010', @TestPrefix + ' Test ' + 'Line closed on East London Line between New Cross and Whitechapel, and between New Cross Gate and Whitechapel due to upgrade works. Until 30th June 2010',
		   @TestPrefix + ' Test ' + 'Travel advice olympic text',
		   'O',536716.000000,177131.000000,'2011-05-01 16:46:55.000','2011-05-01 00:00:00.000','2011-05-02 15:46:24.000',
		   NULL,'2012-09-01 23:59:00.000',1,
		   '',NULL,NULL,NULL,
		   NULL,NULL,NULL,NULL)

	-- Headline incident
	INSERT INTO [dbo].[TravelNews] 
    VALUES
           (@IncidentId2, 3, 3,
		   NULL, 'Road', 'London', 'A115 Stratford, London', 'Closures/ blockages',
		   @TestPrefix + 'A115 : Road closed at Stratford', @TestPrefix + 'Road closed both ways on Carpenters Road between the Gibbins Road junction and the White Post Lane junction in Stratford. Until 1st July 2012',
		   @TestPrefix + ' Test ' + 'Travel advice olympic text',
		   'O',538355.000000,183996.000000,'2011-05-01 16:46:55.000','2011-05-01 00:00:00.000','2011-05-02 15:46:24.000',
		   '2012-09-01 23:59:00.000','2012-09-01 23:59:00.000',1,
		   'A',NULL,NULL,NULL,
		   NULL,NULL,NULL,NULL)

	-- Child incident
	INSERT INTO [dbo].[TravelNews] 
    VALUES
           (@IncidentId3, 4, 4,
		   'East London Line', 'Underground', 'London', 'East London Line London', 'Station/Port Disruption',
		   @TestPrefix + 'East London Line closed until Summer 2010', @TestPrefix + 'Line closed on East London Line between New Cross and Whitechapel, and between New Cross Gate and Whitechapel due to upgrade works. Until 30th June 2010',
		   @TestPrefix + ' Test ' + 'Travel advice olympic text',
		   'O',536716.000000,177131.000000,'2011-05-01 16:46:55.000','2011-05-01 00:00:00.000','2011-05-02 15:46:24.000',
		   NULL,'2012-09-01 23:59:00.000',1,
		   '',@IncidentId1,NULL,NULL,
		   NULL,NULL,NULL,NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsDataSources] WHERE [DataSourceId] = @DataSourceId)
BEGIN
	INSERT INTO [dbo].[TravelNewsDataSources] ([DataSourceId], [DataSourceName], [Trusted])
    VALUES (@DataSourceId, @TestPrefix + 'SJP', 1)
END

IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsDataSource] WHERE [UID] = @IncidentId1)
BEGIN
	INSERT INTO [dbo].[TravelNewsDataSource] ([UID], [DataSourceId])
	VALUES (@IncidentId1, @DataSourceId)

	INSERT INTO [dbo].[TravelNewsDataSource] ([UID], [DataSourceId])
	VALUES (@IncidentId2, @DataSourceId)

	INSERT INTO [dbo].[TravelNewsDataSource] ([UID], [DataSourceId])
	VALUES (@IncidentId3, @DataSourceId)
END

IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsRegion] WHERE [UID] = @IncidentId1)
BEGIN
	INSERT INTO [dbo].[TravelNewsRegion] ([UID], [RegionName])
    VALUES (@IncidentId1, @TestPrefix + 'London')

	INSERT INTO [dbo].[TravelNewsRegion] ([UID], [RegionName])
    VALUES (@IncidentId2, @TestPrefix + 'London')

	INSERT INTO [dbo].[TravelNewsRegion] ([UID], [RegionName])
    VALUES (@IncidentId3, @TestPrefix + 'London')
END

IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsVenue] WHERE [UID] = @IncidentId1)
BEGIN
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100AQC', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100AWP', @IncidentId1)
	
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100BBA', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100BMX', @IncidentId1)
	
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100BND', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100BXH', @IncidentId1)
	
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100COV', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100EAR', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100ETD', @IncidentId1)
	
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100ETM', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100EXL', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100GRP', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100HAD', @IncidentId1)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100HAM', @IncidentId1)


	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100AQC', @IncidentId2)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100AWP', @IncidentId2)
	
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100BBA', @IncidentId2)


	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100AQC', @IncidentId3)

	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100AWP', @IncidentId3)
	
	INSERT INTO [dbo].[TravelNewsVenue] ([VenueNaPTAN], [UID])
    VALUES ('8100BBA', @IncidentId3)
END