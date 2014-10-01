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

:r .\Permissions\SJPTransientPortalPermissions.sql

:r .\Data\ChangeNotificationData.sql

:r .\Data\SJPEventDates.sql

:r .\Data\DropDownData.sql

:r .\Data\CycleAttributeData.sql

:r .\Data\TravelNewsDataSources.sql

:r .\Data\TravelNewsData.sql

:r .\Data\AdminArea.sql

:r .\Data\Districts.sql

:r .\Data\UpdateVersionInfo.sql