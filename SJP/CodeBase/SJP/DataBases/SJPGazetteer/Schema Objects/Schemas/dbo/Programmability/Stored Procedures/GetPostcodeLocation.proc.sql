CREATE PROCEDURE [dbo].[GetPostcodeLocation]
	@postcode varchar(10)
AS
BEGIN
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
	WHERE Replace([Name], ' ', '') = @postcode
END