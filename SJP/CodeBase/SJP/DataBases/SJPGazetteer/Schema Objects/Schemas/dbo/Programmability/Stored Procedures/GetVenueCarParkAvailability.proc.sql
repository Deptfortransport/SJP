CREATE PROCEDURE [dbo].[GetVenueCarParkAvailability]
	@CarParkID nvarchar(20)
AS

	SELECT  [SJPCarParkAvailability].[AvailabilityID],
			[SJPCarParksCarParkAvailability].[CarParkID],
			[FromDate],
			[ToDate],
			[DailyOpeningTime],
			[DailyClosingTime],
			[DaysOfWeek]
	  FROM SJPCarParkAvailability
INNER JOIN SJPCarParksCarParkAvailability
		ON SJPCarParkAvailability.AvailabilityID = SJPCarParksCarParkAvailability.AvailabilityID
	 WHERE SJPCarParksCarParkAvailability.CarParkID = @CarParkID

RETURN 0