CREATE PROCEDURE [dbo].[GetAllVenueCycleParks]
	
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
RETURN 0