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

:r .\Permissions\SJPContentPermissions.sql

:r .\Data\ChangeNotificationData.sql

:r .\Data\ContentGroups.sql

:r .\Data\Content\Content.sql

:r .\Data\Content\ContentSitemap.sql

:r .\Data\Content\ContentHeaderFooter.sql

:r .\Data\Content\ContentJourneyOutput.sql

:r .\Data\Content\ContentAnalytics.sql

:r .\Data\Content\ContentMobile.sql

:r .\Data\UpdateVersionInfo.sql





:r .\EnvironmentSettings.sql
