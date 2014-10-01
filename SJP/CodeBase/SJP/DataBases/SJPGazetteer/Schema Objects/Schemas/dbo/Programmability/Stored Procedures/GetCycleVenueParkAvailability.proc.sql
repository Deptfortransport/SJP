CREATE PROCEDURE [dbo].[GetCycleVenueParkAvailability]
	@CycleParkID nvarchar(20)
AS

	SELECT	[SJPCycleParkAvailability].[AvailabilityID],
			[SJPCycleParksCycleParkAvailability].[CycleParkID],
			[FromDate],
			[ToDate],
			[DailyOpeningTime],
			[DailyClosingTime],
			[DaysOfWeek]
	  FROM SJPCycleParkAvailability
INNER JOIN SJPCycleParksCycleParkAvailability
        ON SJPCycleParkAvailability.AvailabilityID = SJPCycleParksCycleParkAvailability.AvailabilityID
	 WHERE SJPCycleParksCycleParkAvailability.CycleParkID = @CycleParkID

RETURN 0