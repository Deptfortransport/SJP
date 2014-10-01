-- =============================================
-- Script Template
-- =============================================


------------------------------------------------
-- ConnectionString properties
------------------------------------------------

Use SJPConfiguration
Go

DECLARE @AID varchar(50) = '<Default>'
DECLARE @GID varchar(50) = '<Default>'

EXEC AddUpdateProperty 'DefaultDB', 'Data Source=localhost;Initial Catalog=SJPConfiguration;Pooling=True;Timeout=30;User id=SJP_User;Password=!password!1', @AID, @GID
EXEC AddUpdateProperty 'TransientPortalDB', 'Data Source=localhost;Initial Catalog=SJPTransientPortal;Pooling=True;Timeout=30;User id=SJP_User;Password=!password!1', @AID, @GID
EXEC AddUpdateProperty 'GazetteerDB', 'Data Source=localhost;Initial Catalog=SJPGazetteer;Pooling=True;Timeout=30;User id=SJP_User;Password=!password!1', @AID, @GID
EXEC AddUpdateProperty 'ContentDB', 'Data Source=localhost;Initial Catalog=SJPContent;Pooling=True;Timeout=30;User id=SJP_User;Password=!password!1', @AID, @GID
EXEC AddUpdateProperty 'ReportStagingDB', 'Data Source=MIS;Initial Catalog=SJPReportStaging;Timeout=30;User id=SJP_User;Password=!password!1', @AID, @GID
EXEC AddUpdateProperty 'CommandControlDB','Data Source=MIS;Initial Catalog=CommandAndControl;Pooling=True;Timeout=30;User id=SJP_User;Password=!password!1', @AID, @GID

GO