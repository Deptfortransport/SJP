-- =============================================
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[GetChangeTable]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetWMIMonitoringItems]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetFileMonitoringItems]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetDatabaseMonitoringItems]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetChecksumMonitoringItems]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[InsertWMIMonitoringResult]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[InsertFileMonitoringResult]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[InsertDatabaseMonitoringResult]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[InsertChecksumMonitoringResult]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetLatestMonitoringResults]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetLatestMonitoringResultsFiltered]		TO [SJP_User]

GO