
CREATE PROCEDURE [dbo].[GetAllVenueRiverSerivces]
AS
	BEGIN
		SELECT VenueNaPTAN, VenuePierNaPTAN, RemotePierNaPTAN, VenuePierName, RemotePierName
		FROM dbo.SJPRiverServices
	END