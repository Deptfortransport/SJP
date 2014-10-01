
CREATE PROCEDURE [dbo].[GetVenueGateNavigationPaths] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT NavigationPathId, NavigationPathName, FromNaptan, ToNaptan, TransferDuration, TransferDistance, GateNaptan
	FROM SJPVenueGateNavigationPaths
END