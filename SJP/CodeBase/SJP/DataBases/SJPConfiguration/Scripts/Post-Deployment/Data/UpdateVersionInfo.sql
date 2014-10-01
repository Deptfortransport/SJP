-- =============================================
-- Script Template
-- =============================================


USE [SJPConfiguration] 
GO


DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO

