CREATE PROCEDURE [dbo].[GetPostcodeLocations]
AS
BEGIN
	SELECT
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
END