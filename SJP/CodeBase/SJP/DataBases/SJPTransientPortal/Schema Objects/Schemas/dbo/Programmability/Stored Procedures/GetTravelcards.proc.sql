CREATE PROCEDURE [dbo].[GetTravelcards]
AS
BEGIN

	 SELECT	 tc.[TravelCardID]
			,tc.[TravelCardName]
			,tc.[ValidFrom]
			,tc.[ValidTo]
			,tcz.[ZoneID]
			,tcz.[IsExcluded] as [ZoneExcluded]
			,tcr.[RouteID]
			,tcr.[IsExcluded] as [RouteExcluded]
	   FROM [dbo].[SJPTravelcards] tc
  LEFT JOIN [dbo].[SJPTravelcardZones] tcz
		 ON tc.[TravelCardID] = tcz.[TravelCardID]
  LEFT JOIN [dbo].[SJPTravelcardRoutes] tcr
		 ON tc.[TravelCardID] = tcr.[TravelCardID]

END