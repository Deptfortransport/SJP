CREATE USER [SJP_User] 
  FOR LOGIN [SJP_User]
GO

EXEC sp_addrolemember N'db_datareader', N'SJP_User'
GO

EXEC sp_addrolemember N'db_datawriter', N'SJP_User'
GO