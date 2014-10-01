CREATE PROCEDURE [dbo].[GetZones]
AS
BEGIN

		 SELECT  z.[ZoneID]
				,z.[ZoneName]
				,za.[ZoneAreaID]
				,za.[IsOuterZoneArea]
				,zap.[Easting]
				,zap.[Northing]
		   FROM [dbo].[SJPZones] z
	  LEFT JOIN [dbo].[SJPZoneAreas] za
			 ON z.[ZoneID] = za.[ZoneID]
	  LEFT JOIN [dbo].[SJPZoneAreaPoints] zap
			 ON za.[ZoneAreaID] = zap.[ZoneAreaID]

END