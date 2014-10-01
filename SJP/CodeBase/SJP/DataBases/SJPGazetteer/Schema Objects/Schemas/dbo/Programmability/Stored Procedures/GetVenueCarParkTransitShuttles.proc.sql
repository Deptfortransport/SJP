CREATE PROCEDURE [dbo].[GetVenueCarParkTransitShuttles]
	@CarParkID nvarchar(20)
AS

	SELECT  [SJPCarParkTransitShuttles].[TransitShuttleID],
			[SJPCarParksCarParkTransitShuttles].[CarParkID],
			[ToVenue],
			[ModeOfTransport],
			[TransitDuration],
			[IsScheduledService],
			[IsPRMOnly],
			[VenueGateNaPTAN],
			[ServiceFrequency],
			[FirstServiceOfDay],
			[LastServiceOfDay]
	  FROM SJPCarParkTransitShuttles
INNER JOIN SJPCarParksCarParkTransitShuttles
        ON SJPCarParkTransitShuttles.TransitShuttleID = SJPCarParksCarParkTransitShuttles.TransitShuttleID
	 WHERE SJPCarParksCarParkTransitShuttles.CarParkID = @CarParkID

RETURN 0