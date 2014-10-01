-- =============================================
-- Script Template
-- =============================================

-- =============================================
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[getProperties]									TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetVersion]										TO [SJP_User]
GRANT EXECUTE ON [dbo].[ResetDataFeedImport]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectApplicationProperties]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectApplicationPropertiesWithPartnerId]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGlobalProperties]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGlobalPropertiesWithPartnerId]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGroupProperties]							TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGroupPropertiesWithPartnerId]				TO [SJP_User]

