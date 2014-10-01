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

:r .\Permissions\SJPConfigurationPermissions.sql

:r .\Data\ReferenceNumData.sql

:r .\Data\ChangeNotificationData.sql

----------------------------------------------------
-- PROPERTIES
----------------------------------------------------
:r .\Data\Properties\DatabaseData.sql

-- SJPWeb and SJPMobile
:r .\Data\Properties\PropertiesData.sql
:r .\Data\Properties\PropertiesDataSJPWeb.sql
:r .\Data\Properties\PropertiesDataSJPMobile.sql

:r .\Data\Properties\EventLoggingDataSJPWeb.sql
:r .\Data\Properties\EventLoggingDataSJPMobile.sql

-- Command and Control
:r .\Data\Properties\CommandAndControl\CommandAndControlData.sql
:r .\Data\Properties\CommandAndControl\EventLoggingData.sql

-- Coordinate Convertor
:r .\Data\Properties\CoordinateConvertor\CoordinateConvertorData.sql
:r .\Data\Properties\CoordinateConvertor\EventLoggingData.sql

-- Data Services
:r .\Data\Properties\DataServices\DataServices.sql

-- Event Receiver
:r .\Data\Properties\EventReceiver\EventReceiverData.sql
:r .\Data\Properties\EventReceiver\EventLoggingData.sql

-- Web Log Reader
:r .\Data\Properties\WebLogReader\WebLogReaderData.sql
:r .\Data\Properties\WebLogReader\EventLoggingData.sql

-- Data Loader
:r .\Data\Properties\DataLoader\DataLoaderData.sql
:r .\Data\Properties\DataLoader\EventLoggingData.sql

--Venue Incidents
:r .\Data\Properties\VenueIncidents\VenueIncidents.sql
:r .\Data\Properties\VenueIncidents\EventLoggingData.sql

----------------------------------------------------

:r .\Data\RetailersData.sql

:r .\Data\UpdateVersionInfo.sql


--NB This script MUST be the last one included as it updates changes made by previous scripts.
:r .\EnvironmentSettings.sql
