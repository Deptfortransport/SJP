/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\data\PermanentPortal_Partners.sql

:r .\data\PermanentPortal_Properties.sql

:r .\data\PermanentPortal_FTP_CONFIGURATION.sql

:r .\data\PermanentPortal_IMPORT_CONFIGURATION.sql

:r .\data\SJP_PermanentPortal_Properties.sql

:r .\Permissions\PermanentPortalPermissions.sql

:r .\Data\UpdateVersionInfo.sql



--NB This script MUST be the last one included as it updates changes made by previous scripts.
:r .\EnvironmentSettings.sql
