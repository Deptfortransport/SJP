CREATE PROCEDURE [dbo].[GetLocations]
AS
BEGIN

	SELECT
	   [DATASETID]
      ,[ParentID]
      ,[Name]
      ,[DisplayName]
      ,[Type]
      ,Case When [Type] <> 'EXCHANGE GROUP' THEN [Naptan] ELSE dbo.GetGroupNaPTANs([DATASETID]) END [Naptan]
      ,[LocalityID]
      ,[Easting]
      ,[Northing]
      ,[NearestTOID]
      ,[NearestPointE]
      ,[NearestPointN]
	  ,[AdminAreaID]
      ,[DistrictID]
	FROM [dbo].[SJPNonPostcodeLocations]

END