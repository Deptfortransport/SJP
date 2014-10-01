CREATE PROCEDURE [dbo].[GetTransitShuttleAvailability]
	@TransitShuttleID nvarchar(20)
AS

	SELECT  [SJPTransitShuttlesAvailability].[TransitShuttleAvailabilityID],
			[SJPCarParkTransitShuttlesTransitShuttlesAvailability].[TransitShuttleID],
			[FromDate],
			[ToDate],
			[DailyStartTime],
			[DailyEndTime],
			[DaysOfWeek]
	  FROM SJPTransitShuttlesAvailability
INNER JOIN SJPCarParkTransitShuttlesTransitShuttlesAvailability
		ON SJPTransitShuttlesAvailability.TransitShuttleAvailabilityID = SJPCarParkTransitShuttlesTransitShuttlesAvailability.TransitShuttleAvailabilityID
	 WHERE SJPCarParkTransitShuttlesTransitShuttlesAvailability.TransitShuttleID = @TransitShuttleID

RETURN 0