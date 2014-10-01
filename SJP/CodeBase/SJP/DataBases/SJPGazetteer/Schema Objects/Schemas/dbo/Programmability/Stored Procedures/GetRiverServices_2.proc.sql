
CREATE PROCEDURE [dbo].[GetRiverServices]
AS
	BEGIN
		SELECT VenueNaPTAN, VenuePierNaPTAN, RemotePierNaPTAN, VenuePierName, RemotePierName
		FROM dbo.SJPRiverServices
	END