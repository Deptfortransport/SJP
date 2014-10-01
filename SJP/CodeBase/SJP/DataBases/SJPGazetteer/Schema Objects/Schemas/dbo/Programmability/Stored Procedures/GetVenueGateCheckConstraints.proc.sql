
CREATE PROCEDURE [dbo].[GetVenueGateCheckConstraints] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT CheckConstraintID, CheckConstraintName, [Entry], Process, Congestion, AverageDelay, GateNaptan
	FROM SJPVenueGateCheckConstraints
END