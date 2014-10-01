-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


-- TravelNewsSeverity
IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsSeverity])
BEGIN
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(0, 'Critical')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(1, 'Serious')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(2, 'Very Severe')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(3, 'Severe')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(4, 'Medium')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(5, 'Slight')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(6, 'Very Slight')
END

---------------------------------------------------
-- Add a "no news available" item if no data exists
---------------------------------------------------
-- TravelNews 
IF NOT EXISTS (SELECT * FROM [dbo].[TravelNews])
BEGIN
	INSERT INTO [dbo].[TravelNews] ([UID], [SeverityLevel], [SeverityLevelOlympic], 
		[PublicTransportOperator], [ModeOfTransport], [Regions], [Location], [IncidentType], 
		[HeadlineText], [DetailText], [TravelAdviceOlympicText],
		[IncidentStatus], [Easting], [Northing], [ReportedDateTime], [StartDateTime], [LastModifiedDateTime],
		[ClearedDateTime], [ExpiryDateTime], [PlannedIncident], 
		[RoadType], [IncidentParent], [CarriagewayDirection], [RoadNumber],
		[DayMask], [DailyStartTime], [DailyEndTime], [ItemChangeStatus])
    VALUES
           ('RTM999980', 2, 2, 'N/A', 'Road', 'London', ' ', 'Incidents',
		   'We are unable to bring you Live Travel News at the moment. Please try later.', 
		   'We are unable to bring you Live Travel News at the moment. Please try later.',
		   'We are unable to bring you Live Travel News at the moment. Please try later.',
		   'O',0,0,'2011-05-01 00:00:00.000','2011-05-01 00:00:00.000','2011-05-01 00:00:00.000',
		   NULL,'2012-05-01 00:00:00.000',1,
		   '',NULL,NULL,NULL,
		   NULL,NULL,NULL,NULL)

	-- TravelNewsDataSources
	IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsDataSources] WHERE [DataSourceId] = 'DataSource1')
	BEGIN
		INSERT INTO [dbo].[TravelNewsDataSources] ([DataSourceId], [DataSourceName], [Trusted])
		VALUES ('DataSource1', 'DataSource1', 1)
	END

	-- TravelNewsDataSource
	IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsDataSource] WHERE [UID] = 'RTM999980')
	BEGIN
		INSERT INTO [dbo].[TravelNewsDataSource] ([UID], [DataSourceId])
		VALUES ('RTM999980', 'DataSource1')
	END

	-- TravelNewsRegion
	IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsRegion] WHERE [UID] = 'RTM999980')
	BEGIN
		INSERT INTO [dbo].[TravelNewsRegion] ([UID], [RegionName])
		VALUES ('RTM999980', 'London')
	END

END

GO