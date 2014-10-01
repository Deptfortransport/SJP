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

:r .\Permissions\SJPGazetteerPermissions.sql

:r .\Data\ChangeNotificationData.sql

:r .\Data\SJPNonPostcodeLocations.sql
:r .\Data\SJPPostcodeLocations.sql

:r .\Data\UpdateVersionInfo.sql