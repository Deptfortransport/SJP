-- =============================================
-- Script Template
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'DataLoader' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'DataLoader'
DECLARE @GID varchar(50) = 'DataGateway'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

-- Data load configuration properties

-- TravelNews - Transfer and Load onto WorkUnit
EXEC AddUpdateProperty 'DataLoader.Configuration.TravelNews.Class.Transfer', 'SJP.DataLoader.XmlTransferTask', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.TravelNews.ClassAssembly.Transfer', 'sjp.dataloader.exe', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.TravelNews.Class.Load', 'SJP.DataLoader.DatabaseLoadTask', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.TravelNews.ClassAssembly.Load', 'sjp.dataloader.exe', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.TravelNews.Directory', 'D:\Temp\TravelNews', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.TravelNews.Directory.Clean', 'true', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.TravelNews.FileName.Save', 'TravelNews.xml', @AID, @GID

EXEC AddUpdateProperty 'DataLoader.Transfer.TravelNews.Locations', '1,2', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.TravelNews.Location.1', 'http://DG01/Data/TravelNews/TravelNews.xml', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.TravelNews.Location.2', 'http://DG02/Data/TravelNews/TravelNews.xml', @AID, @GID

EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.Xml.Validate', 'true', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.Xml.Schema', 'D:\SJP\Components\OlympicTravelNews.xsd', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.Xml.Namespace', 'http://www.transportdirect.info/olympictravelnews', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.Xml.NamespaceXsi', 'http://www.w3.org/2001/XMLSchema-instance', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.Database', 'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.DatabaseStoredProcedure', 'ImportSJPTravelNewsData', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.TravelNews.DatabaseTimeout', '3000', @AID, @GID

-- LULGateway (London Underground) - Transfer onto the Gateway
EXEC AddUpdateProperty 'DataLoader.Configuration.LULGateway.Class.Transfer', 'SJP.DataLoader.XmlTransferTask', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LULGateway.ClassAssembly.Transfer', 'sjp.dataloader.exe', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LULGateway.Directory', 'D:\inetpub\wwwroot\Data\LUL\', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LULGateway.Directory.Clean', 'false', @AID, @GID

EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.Locations', '1', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.Location.1', 'http://cloud.tfl.gov.uk/TrackerNet/LineStatus', @AID, @GID
--EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.Location.1.Username', '', @AID, @GID
--EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.Location.1.Password', '', @AID, @GID
--EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.Location.1.Domain', '', @AID, @GID
--EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.Location.1.Proxy', 'jwproxyserver:8080', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.LULGateway.FileName.Save', 'LondonUnderground.xml', @AID, @GID

-- LUL (London Underground) - Transfer and Load onto WorkUnit
EXEC AddUpdateProperty 'DataLoader.Configuration.LUL.Class.Transfer', 'SJP.DataLoader.XmlTransferTask', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LUL.ClassAssembly.Transfer', 'sjp.dataloader.exe', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LUL.Class.Load', 'SJP.DataLoader.DatabaseLoadTask', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LUL.ClassAssembly.Load', 'sjp.dataloader.exe', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LUL.Directory', 'D:\Temp\LUL', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Configuration.LUL.Directory.Clean', 'true', @AID, @GID

EXEC AddUpdateProperty 'DataLoader.Transfer.LUL.Locations', '1,2', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.LUL.Location.1', 'http://DG01/Data/LUL/LondonUnderground.xml', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.LUL.Location.2', 'http://DG02/Data/LUL/LondonUnderground.xml', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Transfer.LUL.FileName.Save', 'LondonUnderground.xml', @AID, @GID

EXEC AddUpdateProperty 'DataLoader.Load.LUL.Xml.Validate', 'false', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.LUL.Database', 'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.LUL.DatabaseStoredProcedure', 'ImportSJPUndergroundStatusData', @AID, @GID
EXEC AddUpdateProperty 'DataLoader.Load.LUL.DatabaseTimeout', '3000', @AID, @GID

GO