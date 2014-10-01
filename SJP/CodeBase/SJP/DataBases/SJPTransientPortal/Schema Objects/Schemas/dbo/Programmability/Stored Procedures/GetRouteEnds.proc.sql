CREATE PROCEDURE [dbo].[GetRouteEnds]
AS
BEGIN

		SELECT  r.[RouteID]
			   ,r.[RouteName]
			   ,res.[NaPTAN]
			   ,res.[IsEndA] as [StopIsEndA]
			   ,rez.[ZoneID] 
			   ,rez.[IsEndA] as [ZoneIsEndA]
		  FROM [dbo].[SJPRoutes] r
	 LEFT JOIN [dbo].[SJPRouteEndStops] res
			ON r.[RouteID] = res.[RouteID]
	 LEFT JOIN [dbo].[SJPRouteEndZones] rez
			ON r.[RouteID] = rez.[RouteID]

END