CREATE PROCEDURE [dbo].[TravelNewsHeadlines]
AS
BEGIN

	SELECT
		TN.UID,
		TN.SeverityLevel,
		TN.HeadlineText,
		TN.SeverityLevel,
		TN.ModeOfTransport,
		TN.Regions,
		DateDiff(mi, TN.StartDateTime, GetDate()) StartToNowMinDiff
	FROM TravelNews TN
	WHERE TN.SeverityLevel < 4
	  AND TN.SeverityLevel <> 1
	  AND TN.StartDateTime <=getdate() 
	  AND TN.ExpiryDateTime >=getdate()
	  AND TN.IncidentStatus ='O'
	  AND ISNULL(TN.IncidentParent,'0') IN ('0','')
	  AND [dbo].IsTravelNewsItemActive(TN.DayMask,TN.DailyStartTime, TN.DailyEndTime,GETDATE()) = 1
			AND EXISTS (SELECT * FROM TravelNewsDataSource TNDS1
					INNER JOIN TravelNewsDataSources TNDSS
					ON TNDS1.DataSourceId = TNDSS.DataSourceId
					WHERE TNDS1.UID = TN.UID AND TNDSS.Trusted = 1)
	ORDER BY TN.SeverityLevel ASC, TN.StartDateTime DESC

END