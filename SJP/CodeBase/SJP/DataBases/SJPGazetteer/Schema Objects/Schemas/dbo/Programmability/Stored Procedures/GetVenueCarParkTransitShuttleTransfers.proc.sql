CREATE PROCEDURE [dbo].[GetVenueCarParkTransitShuttleTransfers]
	@TransitShuttleID nvarchar(50)
AS

	SELECT  [TransitShuttleID],
			[CultureCode] AS TransferLanguage,
			[TransferDescription] AS TransferText
	  FROM [SJPCarParkTransitShuttleTransfers]
	 WHERE [TransitShuttleID] = @TransitShuttleID

RETURN 0