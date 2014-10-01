CREATE PROCEDURE [dbo].[GetAllVenueCarParks]
	
AS
	SELECT 
		cp.CarParkID,
		cp.CarParkName,
		cp.VenueServed,
		cp.MapOfSiteURL,
		cp.InterchangeDuration,
		cp.CoachSpaces,
		cp.CarSpaces,
		cp.DisabledSpaces,
		cp.BlueBadgeSpaces,
		cpt.ToTOID as DriveToToid,
		cpt.FromTOID as DriveFromToid
	FROM SJPCarParks cp
	LEFT OUTER JOIN SJPCarParkToids cpt
	ON cp.CarParkID = cpt.ParkAndRideID
RETURN 0