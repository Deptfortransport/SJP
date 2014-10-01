-- =============================================
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[GetVersion]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetChangeTable]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRetailers]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRetailerLookup]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectApplicationProperties]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGlobalProperties]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGroupProperties]			TO [SJP_User]

GO