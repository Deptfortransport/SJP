CREATE PROCEDURE [dbo].[GetRoutes]
AS
BEGIN

		SELECT  r.[RouteID]
			   ,r.[RouteName]
			   ,rm.[ModeOfTransport]
			   ,rm.[IsExcluded]
		  FROM [dbo].[SJPRoutes] r
	 LEFT JOIN [dbo].[SJPRouteModes] rm
			ON r.[RouteID] = rm.[RouteID]

END