-- =============================================
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[GetChangeTable]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetCycleAttributes]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetNPTGAdminAreas]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetNPTGDistricts]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRoutes]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRouteEnds]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetStopAccessibilityLinks]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetTravelcards]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetUndergroundStatus]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetZones]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetZoneStops]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetZoneModes]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[TravelNewsAll]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[TravelNewsHeadlines]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[TravelNewsVenues]				TO [SJP_User]

GRANT EXECUTE ON [dbo].[ImportSJPStopAccessibilityLinks] TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPTravelcardData]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPTravelNewsData]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPUndergroundStatusData]	TO [SJP_User]

GO
