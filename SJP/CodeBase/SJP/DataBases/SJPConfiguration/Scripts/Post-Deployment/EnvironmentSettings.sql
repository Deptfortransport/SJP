-- =============================================
-- SCRIPT TO UPDATE DEV MACHINE SPECIFIC SETTINGS - OVERWRITES PRODUCTION SETTINGS IN DB PROJECT IF BUILD CONFIGURATION = DEBUG
-- =============================================

-- '<Default>', '<Default>'
EXEC AddUpdateProperty 'DefaultDB', 'Data Source=.\SQLEXPRESS;Initial Catalog=SJPConfiguration;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1', '<Default>', '<Default>'
EXEC AddUpdateProperty 'TransientPortalDB', 'Data Source=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1', '<Default>', '<Default>'
EXEC AddUpdateProperty 'GazetteerDB', 'Data Source=.\SQLEXPRESS;Initial Catalog=SJPGazetteer;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1', '<Default>', '<Default>'
EXEC AddUpdateProperty 'ContentDB', 'Data Source=.\SQLEXPRESS;Initial Catalog=SJPContent;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1', '<Default>', '<Default>'
EXEC AddUpdateProperty 'ReportStagingDB', 'Data Source=.\SQLEXPRESS;Initial Catalog=SJPReportStaging;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1', '<Default>', '<Default>'
EXEC AddUpdateProperty 'CommandControlDB','Data Source=.\SQLEXPRESS;Initial Catalog=CommandAndControl;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1', '<Default>', '<Default>'


-- 'SJPWeb' and 'SJPMobile', 'UserPortal'
EXEC AddUpdateProperty 'CoordinateConvertor.WebService.URL', 'http://localhost/SJPWebServices/CoordinateConvertorService/CoordinateConvertor.asmx', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'CoordinateConvertor.WebService.URL', 'http://localhost/SJPWebServices/CoordinateConvertorService/CoordinateConvertor.asmx', 'SJPMobile', 'UserPortal'

EXEC AddUpdateProperty 'CyclePlanner.WebService.URL','http://CP_JP/cycleplannerservice/service.asmx', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'CyclePlanner.WebService.URL','http://CP_JP/cycleplannerservice/service.asmx', 'SJPMobile', 'UserPortal'

EXEC AddUpdateProperty 'LocationService.Cache.LoadLocations', 'false', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'LocationService.Cache.LoadLocations', 'false', 'SJPMobile', 'UserPortal'

EXEC AddUpdateProperty 'LocationService.Cache.LoadPostcodes', 'false', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'LocationService.Cache.LoadPostcodes', 'false', 'SJPMobile', 'UserPortal'

EXEC AddUpdateProperty 'Debug.Information.Show.Switch','true', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Debug.Information.Show.Switch','true', 'SJPMobile', 'UserPortal'

EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Refresh.Seconds', '10', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Refresh.Seconds', '10', 'SJPMobile', 'UserPortal'

-- 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Retail.RetailHandoffXml.Schema.Path','D:\SJP\CodeBase\SJP\SJPWeb\Schemas\SJPBookingSystemHandoff.xsd', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Retail.Retailers.ShowTestRetailers.Switch', 'true', 'SJPWeb', 'UserPortal'

-- 'SJPMobile', 'UserPortal'
EXEC AddUpdateProperty 'EventDateControl.Now.Link.Switch', 'false', 'SJPMobile', 'UserPortal'

-- 'CoordinateConvertorService', 'UserPortal'
EXEC AddUpdateProperty 'Coordinate.Convertor.DataPath','D:\SJP\CodeBase\SJP\SJPWebServices\CoordinateConvertorService\bin\App_Data', 'CoordinateConvertorService', 'UserPortal'

-- Logging
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace','On', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace','On', 'SJPMobile', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'SJPMobile', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'CCAgent', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel','Verbose', 'CoordinateConvertorService', 'UserPortal'

EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'QUEUE1 FILE1', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'QUEUE1 FILE1', 'SJPMobile', 'UserPortal'

EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'QUEUE1 FILE1', 'SJPWeb', 'UserPortal'
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'QUEUE1 FILE1', 'SJPMobile', 'UserPortal'

--TravelNews & LUL locations - updates both locations to be local
EXEC AddUpdateProperty 'DataLoader.Transfer.TravelNews.Location.1', 'http://127.0.0.1/Data/TravelNews/TravelNews.xml', 'DataLoader', 'DataGateway'
EXEC AddUpdateProperty 'DataLoader.Transfer.TravelNews.Location.2', 'http://127.0.0.1/Data/TravelNews/TravelNews.xml', 'DataLoader', 'DataGateway'
EXEC AddUpdateProperty 'DataLoader.Transfer.LUL.Location.1', 'http://127.0.0.1/Data/LUL/LondonUnderground.xml', 'DataLoader', 'DataGateway'
EXEC AddUpdateProperty 'DataLoader.Transfer.LUL.Location.2', 'http://127.0.0.1/Data/LUL/LondonUnderground.xml', 'DataLoader', 'DataGateway'

-- Venue Incidents
EXEC AddUpdateProperty 'VenueIncidents.IncidentLandingPage.Location', 'http://localhost/SJPMobile/TravelNews.aspx?nv={0}&pn=0', 'VenueIncidents', 'FileCreation'

GO
