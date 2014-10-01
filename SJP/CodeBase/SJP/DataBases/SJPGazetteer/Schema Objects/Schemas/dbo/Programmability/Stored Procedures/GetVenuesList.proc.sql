CREATE PROCEDURE [dbo].[GetVenuesList]
AS
BEGIN
	SELECT DisplayName, Naptan
	FROM dbo.SJPNonPostcodeLocations
	WHERE [Type] = 'VENUEPOI'
END