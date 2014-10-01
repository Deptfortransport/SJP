-- =============================================
-- Script Template
-- =============================================

USE SJPGazetteer
Go

DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO

