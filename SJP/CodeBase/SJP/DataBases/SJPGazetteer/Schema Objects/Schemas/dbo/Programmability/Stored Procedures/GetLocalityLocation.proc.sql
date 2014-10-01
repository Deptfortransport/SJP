﻿CREATE PROCEDURE [dbo].[GetLocalityLocation]
	@localityID varchar(100) 
AS
BEGIN
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
	WHERE [DATASETID] = @localityID
END