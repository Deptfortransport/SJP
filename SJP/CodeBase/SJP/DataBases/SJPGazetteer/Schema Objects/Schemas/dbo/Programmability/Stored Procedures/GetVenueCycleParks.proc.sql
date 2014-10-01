CREATE PROCEDURE [dbo].[GetVenueCycleParks]
	@VenueNaPTAN nvarchar(10)
AS
	SELECT 
			[CycleParkID],
			[CycleParkName],
			[VenueServed],
			[CycleParkMapURL],
			[NumberOfSpaces],
			[CycleToEasting],
			[CycleToNorthing],
			[CycleFromEasting],
			[CycleFromNorthing],
			[StorageType],
			[WalkToGateDuration],
			[VenueEntranceGate],
			[WalkFromGateDuration],
			[VenueExitGate]
	  FROM SJPCycleParks
	 WHERE VenueServed = @VenueNaPTAN
RETURN 0