CREATE PROCEDURE [dbo].[GetLocalityLocations]
	@eastingMin float, 
	@eastingMax float, 
	@northingMin float, 
	@northingMax float
AS
BEGIN
	-- Retrieve all locality locations which are within the easting northing grid square
	SELECT
	   [DATASETID]
      ,[ParentID]
      ,[Name]
      ,[DisplayName]
      ,[Type]
      ,[Naptan]
      ,[LocalityID]
      ,[Easting]
      ,[Northing]
      ,[NearestTOID]
      ,[NearestPointE]
      ,[NearestPointN]
	  ,[AdminAreaID]
      ,[DistrictID]
	 FROM [dbo].[SJPNonPostcodeLocations]
	WHERE [Type] = 'LOCALITY'
	  AND [Easting] > @eastingMin 
	  AND [Easting] < @eastingMax
	  AND [Northing] > @northingMin
	  AND [Northing] < @northingMax
END