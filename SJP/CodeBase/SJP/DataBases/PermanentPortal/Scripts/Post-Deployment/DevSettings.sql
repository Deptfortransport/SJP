-- =============================================
-- Script Template
-- =============================================

UPDATE [dbo].[properties] SET [pValue]=N'C:/Program Files/7-Zip/7z.exe' WHERE [pName]=N'datagateway.zipprocessor.pkunzipexecutablepath' AND [AID]=N'' AND [GID]=N'DataGateway' AND [PartnerId]=0 AND [ThemeId]=1
UPDATE [dbo].[properties] SET [pValue]=N'Data Source=.\SQLEXPRESS;Initial Catalog=PermanentPortal;Integrated Security=True;Pooling=False' WHERE [pName]=N'DefaultDB' AND [AID]=N'<DEFAULT>' AND [GID]=N'<DEFAULT>' AND [PartnerId]=0 AND [ThemeId]=1
UPDATE [dbo].[properties] SET [pValue]=N'Data Source=.\SQLEXPRESS;Initial Catalog=SJPGazetteer;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1' WHERE [pName]=N'SJPGazetteerDB' AND [AID]=N'<DEFAULT>' AND [GID]=N'<DEFAULT>' AND [PartnerId]=0 AND [ThemeId]=1
UPDATE [dbo].[properties] SET [pValue]=N'Data Source=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1' WHERE [pName]=N'SJPTransientPortalDB' AND [AID]=N'<DEFAULT>' AND [GID]=N'<DEFAULT>' AND [PartnerId]=0 AND [ThemeId]=1
