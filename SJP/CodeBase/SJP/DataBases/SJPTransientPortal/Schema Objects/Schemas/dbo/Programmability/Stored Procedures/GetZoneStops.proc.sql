CREATE PROCEDURE [dbo].[GetZoneStops]
AS
BEGIN

		 SELECT  z.[ZoneID]
				,zs.[NaPTAN]
				,zs.[IsExcluded]
		   FROM [dbo].[SJPZones] z
	  LEFT JOIN [dbo].[SJPZoneStops] zs
		     ON z.[ZoneID] = zs.[ZoneID]

END