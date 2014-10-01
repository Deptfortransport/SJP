CREATE FUNCTION [dbo].[GetGroupNaPTANs]
(
	@DATASETID nvarchar(50)
)
RETURNS varchar(max)
AS
BEGIN
	-- Get the naptans for all the stations in the group
	DECLARE @NaPTANsInGroup varchar(max)

	SELECT    @NaPTANsInGroup = coalesce(@NaPTANsInGroup + ',', '') + StationInGroup.Naptan
	FROM [dbo].[SJPNonPostcodeLocations] GroupStation
	INNER JOIN [dbo].[SJPNonPostcodeLocations] StationInGroup
	on GroupStation.DATASETID = StationInGroup.ParentID
	WHERE GroupStation.DATASETID = @DATASETID

	return @NaPTANsInGroup
END