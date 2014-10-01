CREATE PROCEDURE [dbo].[GetUnknownLocation]
	@searchstring varchar(100)
AS
BEGIN
	-- Check if it is a postcode (strip out any spaces in name for easier searching)
	IF EXISTS (SELECT TOP(1) Name 
			     FROM [dbo].[SJPPostcodeLocations] 
			    WHERE Replace([Name], ' ', '') = @searchstring)
		SELECT TOP(1) 
			 [DATASETID]
			,[ParentID]
			,Replace([Name], ' ', '') as [Name]
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
		FROM [dbo].[SJPPostcodeLocations]
		WHERE Replace([Name], ' ', '') =  @searchstring
	ELSE
	-- Else its a non-postcode
		SELECT TOP(1) 
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
		WHERE DisplayName = @searchstring
END