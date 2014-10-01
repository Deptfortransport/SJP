CREATE PROCEDURE [dbo].[GetGroupLocation]
	@groupID varchar (100)
AS
BEGIN
	-- Get the naptans for all the stations in the group
	DECLARE @NaPTANsInGroup varchar(max)

	SELECT    @NaPTANsInGroup = coalesce(@NaPTANsInGroup + ',', '') + StationInGroup.Naptan
	FROM [dbo].[SJPNonPostcodeLocations] GroupStation
	INNER JOIN [dbo].[SJPNonPostcodeLocations] StationInGroup
	on GroupStation.DATASETID = StationInGroup.ParentID
	WHERE GroupStation.DATASETID = @groupID
	
	
	SELECT 
		 [DATASETID]
		,[ParentID]
		,[Name]
		,[DisplayName]
		,[Type]
		,@NaPTANsInGroup as Naptan
		,[LocalityID]
		,[Easting]
		,[Northing]
		,[NearestTOID]
		,[NearestPointE]
		,[NearestPointN] 
		,[AdminAreaID]
		,[DistrictID]
	FROM [dbo].[SJPNonPostcodeLocations]
	WHERE DATASETID = @groupID
END