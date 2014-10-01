CREATE PROCEDURE [dbo].[GetVenueCarParkInformation]
	@CarParkID nvarchar(20)
AS
	SELECT  [CarParkID],
			[CultureCode],
			[InformationText]
	FROM SJPCarParkInformation
	WHERE CarParkID = @CarParkID
RETURN 0