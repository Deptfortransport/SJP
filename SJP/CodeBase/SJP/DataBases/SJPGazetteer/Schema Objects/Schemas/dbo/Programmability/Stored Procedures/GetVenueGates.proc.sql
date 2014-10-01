
CREATE PROCEDURE [dbo].[GetVenueGates] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT EntranceNaPTAN, EntranceName, Easting, Northing, AvailableFrom, AvailableTo
	FROM SJPVenueGates
END