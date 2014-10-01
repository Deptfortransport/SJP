CREATE PROCEDURE [dbo].[ImportSJPTravelNewsData]
	@XML text
AS

SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRANSACTION

DECLARE @DocID int
DECLARE @Version varchar(20)
DECLARE @RowCount int

EXEC sp_xml_preparedocument @DocID OUTPUT, @XML

SET @Version = NULL

SELECT TOP 1
	@Version = Version
FROM
OPENXML (@DocID, '/TrafficAndTravelNewsData', 2)
WITH
(
	Version varchar(50) '@version'
)

IF @Version IS NULL OR @Version <> 'IF15102'
BEGIN
	EXEC sp_xml_removedocument @DocID
	RAISERROR('The stored procedure ImportSJPTravelNewsData did not recognise the given XML file version', 11, 1)
END
ELSE
BEGIN
	SET @RowCount = 0

	DELETE FROM TravelNewsDataSource
	DELETE FROM TravelNewsRegion
	DELETE FROM TravelNews
	DELETE FROM TravelNewsToid
	DELETE FROM TravelNewsVenue

	INSERT INTO TravelNews
	(
		[UID],
		SeverityLevel,
		SeverityLevelOlympic,
		PublicTransportOperator,
		ModeOfTransport,
		Location,
		IncidentType,
		HeadlineText,
		DetailText,
		TravelAdviceOlympicText,
		IncidentStatus,
		Easting,
		Northing,
		ReportedDateTime,
		StartDateTime,
		LastModifiedDateTime,
		ClearedDateTime,
		ExpiryDateTime,
		PlannedIncident,
		RoadType,
		IncidentParent,
		CarriagewayDirection,
		RoadNumber,
		DayMask,
		DailyStartTime,
		DailyEndTime,
		ItemChangeStatus
	)
	SELECT DISTINCT
		X.[UID],
		TNS.SeverityID,
		TNSO.SeverityID,
		X.PublicTransportOperator,
		X.ModeOfTransport,
		X.Location,
		X.IncidentType,
		X.HeadlineText,
		X.DetailText,
		X.TravelAdviceOlympicText,
		X.IncidentStatus,
		X.Easting,
		X.Northing,
		X.ReportedDateTime,
		X.StartDateTime,
		X.LastModifiedDateTime,
		X.ClearedDateTime,
		X.ExpiryDateTime,
		CASE X.PlannedIncident WHEN 'true' THEN 1 ELSE 0 END,
		X.RoadType,
		X.IncidentParent,
		X.CarriagewayDirection,
		X.RoadNumber,
		X.DayMask,
		X.DailyStartTime,
		X.DailyEndTime,
		X.ItemChangeStatus
	FROM
	OPENXML (@DocID, '/TrafficAndTravelNewsData/Incident', 2)
	WITH
	(
		[UID] varchar(25) '@uid',
		SeverityLevel varchar(15) '@severity',
		SeverityLevelOlympic varchar(15) '@OlympicSeverity',
		PublicTransportOperator varchar(100) '@publicTransportOperator',
		ModeOfTransport varchar(50) '@modeOfTransport',
		Location varchar(100) '@location',
		IncidentType varchar(60) '@incidentType',
		HeadlineText varchar(150) '@headlineText',
		DetailText varchar(350) '@detailText',
		TravelAdviceOlympicText varchar(350) '@OlympicTravelAdvice',
		IncidentStatus varchar(1) '@incidentStatus',
		Easting decimal(18, 6) '@easting',
		Northing decimal(18, 6) '@northing',
		ReportedDateTime datetime '@reportedDatetime',
		StartDateTime datetime '@startDatetime',
		LastModifiedDateTime datetime '@lastModifiedDatetime',
		ClearedDateTime datetime '@clearedDatetime',
		ExpiryDateTime datetime '@expiryDatetime',
		PlannedIncident varchar(5) '@plannedIncident',
		RoadType varchar(10) '@roadType',
		IncidentParent varchar(25) '@parent_uid',
		CarriagewayDirection varchar(15) '@carriageway_direction',
		RoadNumber varchar(25) '@road_number',
		DayMask varchar(14) '@daymask',
		DailyStartTime time(7) '@daily_startDatetime',
		DailyEndTime time(7) '@daily_endDatetime',
		ItemChangeStatus varchar(9) '@incident_change_status'
	) X
	INNER JOIN TravelNewsSeverity TNS ON X.SeverityLevel = TNS.SeverityDescription
	INNER JOIN TravelNewsSeverity TNSO ON X.SeverityLevelOlympic = TNSO.SeverityDescription

	SET @RowCount = @@ROWCOUNT

	-- Regions for Incident
	INSERT INTO TravelNewsRegion
	(
		[UID],
		RegionName
	)
	SELECT DISTINCT
		[UID],
		CASE RegionName WHEN 'East' THEN 'East Anglia' ELSE RegionName END
	FROM
	OPENXML (@DocID, '/TrafficAndTravelNewsData/Incident/Region', 2)
	WITH
	(
		UID varchar(25) '@uid',
		RegionName varchar(50) '@regionName'
	)

	IF @@ROWCOUNT = 0
		SET @RowCount = 0

	UPDATE TravelNews
	SET Regions = ISNULL(TNR.Regions, '-')
	FROM TravelNews TN
	LEFT OUTER JOIN fn_TravelNewsRegion() TNR ON TN.UID = TNR.UID COLLATE database_default

	IF @@ROWCOUNT = 0
		SET @RowCount = 0


	-- Data Sources for Incident
	INSERT INTO TravelNewsDataSource
	(
		UID,
		DataSourceId
	)
	SELECT DISTINCT
		UID,
		DataSourceId
	FROM
		OPENXML (@DocID, '/TrafficAndTravelNewsData/Incident/IncidentDataSource', 2)
		WITH
		(
			UID varchar(25) '../@uid',
			DataSourceId varchar(50) 'text()'
		)


	IF @@ROWCOUNT = 0
		SET @RowCount = 0
		
	-- Toids affected for Incident
	INSERT INTO TravelNewsToid (
		TOID, 
		[UID])
	SELECT DISTINCT
		X.TOID, 
		X.[UID]
	FROM
		OPENXML (@DocID, '/TrafficAndTravelNewsData/Incident/TOIDsAffected/TOIDAffected', 2)
		WITH
		(
			TOID varchar(25) 'text()',
			[UID] varchar(25) '../../@uid'
		) X

	IF @@ROWCOUNT = 0
		SET @RowCount = 0
		
	-- Venues affected for Incident
	INSERT INTO TravelNewsVenue (
		VenueNaPTAN, 
		[UID])
	SELECT DISTINCT
		X.VenueNaPTAN, 
		X.[UID]
	FROM
		OPENXML (@DocID, '/TrafficAndTravelNewsData/Incident/VenuesAffected/VenueAffected', 2)
		WITH
		(
			VenueNaPTAN varchar(25) 'text()',
			[UID] varchar(25) '../../@uid'
		) X


	EXEC sp_xml_removedocument @DocID
	

	IF @@ERROR<>0
	ROLLBACK TRANSACTION
	ELSE
	BEGIN
		COMMIT TRANSACTION		
		
		UPDATE ChangeNotification
		SET Version = Version + 1
		WHERE [Table] = 'TravelNewsImport'
	END
END
