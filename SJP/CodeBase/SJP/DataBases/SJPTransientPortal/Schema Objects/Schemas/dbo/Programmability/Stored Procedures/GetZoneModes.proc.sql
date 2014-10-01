CREATE PROCEDURE [dbo].[GetZoneModes]
AS
BEGIN

		SELECT  z.[ZoneID]
			   ,zm.[ModeOfTransport]
			   ,zm.[IsExcluded]
		  FROM [dbo].[SJPZones] z
	 LEFT JOIN [dbo].[SJPZoneModes] zm
			ON z.[ZoneID] = zm.[ZoneID]

END
