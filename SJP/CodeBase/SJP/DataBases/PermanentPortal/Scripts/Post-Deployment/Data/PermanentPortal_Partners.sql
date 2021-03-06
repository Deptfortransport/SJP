/*
This script was created by Visual Studio on 20/04/2011 at 08:45.
Run this script on [OVS2\SQLEXPRESS.PermanentPortal] to make it the same as [nvs10.PermanentPortal].
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[properties] DROP CONSTRAINT [FK_properties_Partner]
DELETE FROM [dbo].[Partner]
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (0, N'TransportDirect', N'TransportDirect', N'TransportDirect', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (1, N'VisitBritain', N'VisitBritain', N'VisitBritain', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (2, N'BBC', N'BBC', N'BBC', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (3, N'GNER', N'GNER', N'GNER', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (4, N'DirectGov', N'DirectGov', N'DirectGov', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (5, N'lastminute', N'lastminute', N'iframes', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (6, N'BusinessLink', N'BusinessLink', N'BusinessLink', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (7, N'BusinessGateway', N'BusinessGateway', N'BusinessGateway', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (101, N'EnhancedExposedWebServiceTest', N'EnhancedExposedWebServiceTest', N'EnhancedExposedWebServiceTest', N'uwV6Rgwmsf8WDC5Zh2P7szt91g0OCqFzT74dkFJpSP3jZht1K8zOIeO7IkoU0DrgnLn2evkRQGp9pT2U2jjIKg==')
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (102, N'Lauren', N'Lauren', N'Lauren', N'h7lw0A+jj1p+4f2EeVEvBGrRj1oBW8vRITr8UlHlCFE=')
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (103, N'Kizoom', N'Kizoom', N'Kizoom', N'yTPXZ7yXCAXmlL+t3eZ//y8RYWaHne3nLyo2afRpyCY=')
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (200, N'AOCycle', N'AOCycle', N'AOCycle', NULL)
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (998, N'EnhancedExposedPartnerNotAuthorised', N'EnhancedExposedPartnerNotAuthorised', N'EnhancedExposedPartnerNotAuthorised', N'uwV6Rgwmsf8WDC5Zh2P7szt91g0OCqFzT74dkFJpSP3jZht1K8zOIeO7IkoU0DrgnLn2evkRQGp9pT2U2jjIKg==')
INSERT INTO [dbo].[Partner] ([PartnerId], [HostName], [PartnerName], [Channel], [PartnerPassword]) VALUES (999, N'EnhancedExposedPartnerAuthorised', N'EnhancedExposedPartnerAuthorised', N'EnhancedExposedPartnerAuthorised', N'uwV6Rgwmsf8WDC5Zh2P7szt91g0OCqFzT74dkFJpSP3jZht1K8zOIeO7IkoU0DrgnLn2evkRQGp9pT2U2jjIKg==')
ALTER TABLE [dbo].[properties] ADD CONSTRAINT [FK_properties_Partner] FOREIGN KEY ([PartnerId]) REFERENCES [dbo].[Partner] ([PartnerId])
COMMIT TRANSACTION
