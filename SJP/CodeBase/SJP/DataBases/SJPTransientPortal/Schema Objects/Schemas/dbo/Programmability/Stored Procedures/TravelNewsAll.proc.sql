CREATE PROCEDURE [dbo].[TravelNewsAll]
AS
BEGIN

	SELECT
		TN.UID,
		TN.SeverityLevel,
		TNS.SeverityDescription,
		TN.SeverityLevelOlympic,
		TNSO.SeverityDescription SeverityDescriptionOlympic,
		TN.PublicTransportOperator,
		ISNULL(TN.PublicTransportOperator, '-') Operator,
		TN.ModeOfTransport,
		TN.Regions,
		TN.Location,
		TN.Regions + ' (' + TN.Location + ')' RegionsLocation,
		TN.IncidentType,
		TN.HeadlineText,
		TN.DetailText,
		TN.TravelAdviceOlympicText,
		TN.IncidentStatus,
		TN.Easting,
		TN.Northing,
		TN.ReportedDateTime,
		TN.StartDateTime,
		DateDiff(mi, TN.StartDateTime, GetDate()) StartToNowMinDiff,
		TN.LastModifiedDateTime,
		TN.ClearedDateTime,
		TN.ExpiryDateTime,
		TN.PlannedIncident,
		TN.RoadType,
		TN.IncidentParent,
		TN.CarriagewayDirection,
		TN.RoadNumber,
		TN.DayMask,
		TN.DailyStartTime,
		TN.DailyEndTime,
		TN.ItemChangeStatus,
		dbo.IsTravelNewsItemActive(TN.DayMask,TN.DailyStartTime, TN.DailyEndTime,GETDATE()) IncidentActiveStatus
	FROM (
			(
				(
					(
						TravelNews TN 
					
						INNER JOIN TravelNewsSeverity TNS ON TN.SeverityLevel = TNS.SeverityID
					)
					INNER JOIN TravelNewsSeverity TNSO ON TN.SeverityLevelOlympic = TNSO.SeverityID
				)
				INNER JOIN TravelNewsDataSource TNDS1 ON TN.UID = TNDS1.UID
			)
			INNER JOIN TravelNewsDataSources TNDSS ON TNDS1.DataSourceId = TNDSS.DataSourceId
		 )
	WHERE TNDSS.Trusted = 1
	ORDER BY SeverityLevel ASC, StartDateTime DESC

END
