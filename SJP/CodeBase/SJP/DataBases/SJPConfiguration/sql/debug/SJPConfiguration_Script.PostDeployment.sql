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

-- =============================================
-- Script to add stored procedure permissions to user
-- =============================================

GRANT EXECUTE ON [dbo].[GetVersion]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetChangeTable]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRetailers]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRetailerLookup]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectApplicationProperties]	TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGlobalProperties]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[SelectGroupProperties]			TO [SJP_User]

GO

-- =============================================
-- Script Template
-- =============================================


USE [SJPConfiguration] 
GO

IF NOT EXISTS (SELECT * FROM ReferenceNum)
INSERT INTO ReferenceNum VALUES (0)

-- =============================================
-- Script Template
-- =============================================

USE [SJPConfiguration] 
GO


--EXEC AddChangeNotificationTable 'XXX'


GO

----------------------------------------------------
-- PROPERTIES
----------------------------------------------------
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

-- SJPWeb and SJPMobile
-- =============================================
-- Script to add properties data
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPWeb' and 'SJPMobile' properties
-- Also refer to the application specific properties defined in PropertiesDataSJPWeb.sql and PropertiesDataSJPMobile.sql
------------------------------------------------

DECLARE @AID varchar(50) = '<Default>'
DECLARE @GID varchar(50) = 'UserPortal'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

-- Site Version
EXEC AddUpdateProperty 'Site.VersionNumber', '2.7.1', @AID, @GID

-- Site Mode - Default to Paralympics switch date
EXEC AddUpdateProperty 'Site.DefaultSiteMode.Switch.Date', '13/08/2012 00:00', @AID, @GID

-- State Server
EXEC AddUpdateProperty 'StateServer.RetriesMax', '10', @AID, @GID
EXEC AddUpdateProperty 'StateServer.SleepIntervalMilliSecs', '250', @AID, @GID
EXEC AddUpdateProperty 'StateServer.ExpiryTimeMins', '300', @AID, @GID

-- Site redirect
-- Regex values come from http://detectmobilebrowsers.com version taken on 3/7/2012
EXEC AddUpdateProperty 'SiteRedirector.RegexB', 'android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|meego.+mobile|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino', @AID, @GID
EXEC AddUpdateProperty 'SiteRedirector.RegexV', '1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-', @AID, @GID

-- Data Notification - Groups
EXEC AddUpdateProperty 'DataNotification.PollingInterval.Seconds', '60', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.Groups', 'Configuration,Content,Gazetteer,TravelNews,UndergroundNews', @AID, @GID

-- Data Notification - Tables
EXEC AddUpdateProperty 'DataNotification.Configuration.Database', 'DefaultDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.Configuration.Tables', '', @AID, @GID

EXEC AddUpdateProperty 'DataNotification.Content.Database', 'ContentDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.Content.Tables', 'Content,ContentGroup,ContentOverride', @AID, @GID

EXEC AddUpdateProperty 'DataNotification.Gazetteer.Database', 'GazetteerDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.Gazetteer.Tables', '', @AID, @GID

EXEC AddUpdateProperty 'DataNotification.TravelNews.Database', 'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.TravelNews.Tables', 'TravelNewsImport', @AID, @GID

EXEC AddUpdateProperty 'DataNotification.UndergroundNews.Database', 'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.UndergroundNews.Tables', 'UndergroundStatusImport', @AID, @GID


-- Content
EXEC AddUpdateProperty 'Content.DailyDataRefreshTime', '0300', @AID, @GID

-- Cookies
EXEC AddUpdateProperty 'Cookie.RepeatVisitor.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Cookie.ExpiryTimeSpan.Seconds', '15778463', @AID, @GID
EXEC AddUpdateProperty 'Cookie.UserAgent.HeaderKey', 'User-Agent', @AID, @GID
EXEC AddUpdateProperty 'Cookie.UserAgent.Robot.Pattern', '', @AID, @GID
EXEC AddUpdateProperty 'Cookie.UserAgent.Robot.RegularExpression', '', @AID, @GID
EXEC AddUpdateProperty 'Cookie.LoadJourneyRequest.Switch', 'true', @AID, @GID

-- Cookies - Policy link
EXEC AddUpdateProperty 'Cookie.CookiePolicy.Hyperlink.VisibleFrom.Date', '14/05/2012', @AID, @GID

-- Location Service
EXEC AddUpdateProperty 'LocationService.Cache.LoadLocations', 'true', @AID, @GID
EXEC AddUpdateProperty 'LocationService.Cache.LoadPostcodes', 'true', @AID, @GID

EXEC AddUpdateProperty 'LocationService.NaptanPrefix.Airport', '9200', @AID, @GID
EXEC AddUpdateProperty 'LocationService.NaptanPrefix.Coach', '9000', @AID, @GID
EXEC AddUpdateProperty 'LocationService.NaptanPrefix.Rail', '9100', @AID, @GID

EXEC AddUpdateProperty 'LocationService.SearchLocations.InCache', 'true', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsLimit.Count.Max', '1000', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.Count.Max', '20', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.GroupStationsLimit.Count', '100', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.RailStationsLimit.Count', '5', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.CoachStationsLimit.Count', '2', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.TramStationsLimit.Count', '5', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.FerryStationsLimit.Count', '100', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SearchLocationsShow.AirportStationsLimit.Count', '100', @AID, @GID

EXEC AddUpdateProperty 'LocationService.CommonWords','rail,station,stations,bus,airport,terminal,Coach,tramway,tramlink,dlr,subway,spt,steam,rly,underground,metrolink,tram,rhdr,supertram,metro', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SimilarityIndex.NoCommonWords.Min', '0.5', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SimilarityIndex.NoCommonWordsAndSpace.Min', '0.5', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SimilarityIndex.IndividualWords.Min', '0.65', @AID, @GID
EXEC AddUpdateProperty 'LocationService.SimilarityIndex.ChildLocalityAtEnd', 'true', @AID, @GID

EXEC AddUpdateProperty 'LocationService.CoordinateLocation.LocalitySearch.EastingPadding.Metres', '100000', @AID, @GID
EXEC AddUpdateProperty 'LocationService.CoordinateLocation.LocalitySearch.NorthingPadding.Metres', '100000', @AID, @GID
EXEC AddUpdateProperty 'LocationService.CoordinateLocation.LocationName.AppendLocalityName.Switch', 'false', @AID, @GID

-- Journey Validate and Runner
EXEC AddUpdateProperty 'JourneyPlanner.Switch.CyclePlanner.Available', 'true', @AID, @GID

EXEC AddUpdateProperty 'JourneyPlanner.Validate.Switch.DatesInGamesDateRange', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyPlanner.Validate.Switch.OneLocationIsVenue', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyPlanner.Validate.Switch.CycleLocationsDistance', 'true', @AID, @GID

EXEC AddUpdateProperty 'JourneyPlanner.Validate.Games.StartDate', '2012/07/18 00:00:00', @AID, @GID
EXEC AddUpdateProperty 'JourneyPlanner.Validate.Games.EndDate', '2012/09/14 23:59:59', @AID, @GID

EXEC AddUpdateProperty 'JourneyPlanner.Validate.Locations.CycleDistance.Metres', '5000000', @AID, @GID

-- Journey Control (CJP and CyclePlanner)
EXEC AddUpdateProperty 'JourneyControl.Log.NoJourneyResponses', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Log.JourneyWebFailures', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Log.TTBOFailures', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Log.CJPFailures', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Log.RoadEngineFailures', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Log.CyclePlannerFailures', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Log.DisplayableMessages', 'true', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.Code.CJP.OK', '0', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CJP.NoPublicJourney', '18', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CJP.JourneysRejected', '19', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CJP.AwkwardOvernightRejected','30', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CJP.RoadEngineOK', '100', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CJP.RoadEngineMin', '100', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CJP.RoadEngineMax', '199', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.Code.JourneyWeb.MajorNoResults', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.JourneyWeb.MinorPast', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.JourneyWeb.MinorFuture', '2', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.JourneyWeb.MajorGeneral', '9', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.JourneyWeb.MinorDisplayable', '2', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.Code.TTBO.MajorOK', '0', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.TTBO.MinorOK', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.TTBO.MinorNoResults', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.TTBO.NoTimetableServiceFound', '302', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.Code.CTP.OK', '100', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CTP.UndeterminedError', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CTP.SystemException', '2', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CTP.OperationNotSupported', '3', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CTP.InvalidRequest', '4', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CTP.ErrorConnectingToDatabase', '5', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.Code.CTP.NoJourneyCouldBeFound', '6', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.Notes.TrapezeRegions', 'S,Y,NW,SW,W,WM', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.NullTollLink.Value', 'No URL has been set for this operator', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CongestionWarning.Value', '30', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.ToidPrefix', 'osgb', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CJP.TimeoutMillisecs', '60000', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.RiverService.Interchange.Max.Minutes', '60', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.RiverService.Interchange.Minutes', '15', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.Algorithm.Public', 'Default', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.Algorithm.Public.MinChanges', 'MinChanges', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.DrtIsRequired', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.Sequence', '3', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.Sequence.RiverServicePlannerMode', '3', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.InterchangeSpeed', '0', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingSpeed.MetresPerMin', '90', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingSpeed.Assistance.MetresPerMin', '80', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingSpeed.StepFree.MetresPerMin', '80', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingSpeed.StepFreeAssistance.MetresPerMin', '80', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.MaxWalkingTime.Minutes', '30', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingDistance.Assistance.Metres', '2000', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingDistance.StepFree.Metres', '2000', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.WalkingDistance.StepFreeAssistance.Metres', '2000', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.RoutingGuideInfluenced', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.RoutingGuideCompliantJourneysOnly', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.RouteCodes', '', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.OlympicRequest', 'true', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Off', 'Normal', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Outward', 'AccessMajorEvent', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Return', 'LeaveMajorEvent', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Outward.Accessible.DoNotUseUnderground', 'AccessMajorEvent', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Return.Accessible.DoNotUseUnderground', 'LeaveMajorEvent', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.DontForceCoach.OriginDestinationLondon', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.DontForceCoach.Accessible.OriginDestinationLondon', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.DontForceCoach.Accessible.FewerChanges', 'true', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.Algorithm.Private', 'Fastest', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.AvoidMotorways', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.AvoidFerries', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.AvoidTolls', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.DrivingSpeed.KmPerHour', '112', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.DoNotUseMotorways', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.FuelConsumption.MetresPerLitre', '13807', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.FuelPrice.PencePerLitre', '1150', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CJP.JourneyRequest.RemoveAwkwardOvernight', 'true', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.TimeoutMillisecs', '120000', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyRequest.IncludeToids', 'false', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.Algorithm', 'QuickestV913', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuickestV913.DllPath', 'D:\TransportDirect\Services\CycleHost\', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuickestV913.Dll', 'td.cp.CyclePenaltyFunctions.v3.dll', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuickestV913.Prefix', 'TransportDirect.JourneyPlanning.CyclePenaltyFunctions', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuickestV913.Suffix', 'QuickestV913', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuietestV913.DllPath', 'D:\TransportDirect\Services\CycleHost\', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuietestV913.Dll', 'td.cp.CyclePenaltyFunctions.v3.dll', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuietestV913.Prefix', 'TransportDirect.JourneyPlanning.CyclePenaltyFunctions', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.QuietestV913.Suffix', 'QuietestV913', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.RecreationalV913.DllPath', 'D:\TransportDirect\Services\CycleHost\', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.RecreationalV913.Dll', 'td.cp.CyclePenaltyFunctions.v3.dll', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.RecreationalV913.Prefix', 'TransportDirect.JourneyPlanning.CyclePenaltyFunctions', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.PenaltyFunction.RecreationalV913.Suffix', 'RecreationalV913', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.NumberOfPreferences', '15', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.0', '850', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.1', '11000', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.2', 'Congestion', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.3', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.4', 'Bicycle', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.5', '19', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.6', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.7', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.8', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.9', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.10', '', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.11', '', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.12', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.13', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.UserPreference.Preference.14', 'false', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyResultSetting.IncludeToids', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyResultSetting.IncludeGeometry', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyResultSetting.IncludeText', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyResultSetting.PointSeperator', ' ', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyResultSetting.EastingNorthingSeperator', ',', @AID, @GID

EXEC AddUpdateProperty 'JourneyControl.CyclePlanner.JourneyResult.IncludeLatitudeLongitude', 'true', @AID, @GID

-- Cycle Planner Service
EXEC AddUpdateProperty 'CyclePlanner.WebService.URL','http://localhost:666/cycleplannerservice/service.asmx', @AID, @GID
EXEC AddUpdateProperty 'CyclePlanner.WebService.Timeout.Seconds', '120', @AID, @GID
EXEC AddUpdateProperty 'CyclePlanner.WebService.MaxReceivedMessageSize','653560', @AID, @GID

-- Coordinate Convertor Service
EXEC AddUpdateProperty 'CoordinateConvertor.WebService.URL', 'http://localhost:666/CoordinateConvertorService/CoordinateConvertor.asmx', @AID, @GID
EXEC AddUpdateProperty 'CoordinateConvertor.WebService.Timeout.Seconds', '30', @AID, @GID

-- Stop Event Service
EXEC AddUpdateProperty 'JourneyControl.StopEvents.CJP.TimeoutMillisecs', '30000', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.StopEvents.JourneyRequest.IncludeLocationFilter', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.StopEvents.JourneyRequest.RealTimeRequired', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.StopEvents.JourneyRequest.Range', '4', @AID, @GID
EXEC AddUpdateProperty 'JourneyControl.StopEvents.JourneyRequest.Replan.Range', '6', @AID, @GID

-- Retail
EXEC AddUpdateProperty 'Retail.RetailHandoffXml.Schema.Path','D:\inetpub\wwwroot\SJPWeb\Schemas\SJPBookingSystemHandoff.xsd', @AID, @GID
EXEC AddUpdateProperty 'Retail.RetailHandoffXml.Xmlns', 'http://www.transportdirect.info/SJPBookingSystemHandoff', @AID, @GID
EXEC AddUpdateProperty 'Retail.RetailHandoffXml.Xmlns.Xs', 'http://www.w3.org/2001/XMLSchema', @AID, @GID
EXEC AddUpdateProperty 'Retail.Retailers.ShowTestRetailers.Switch', 'false', @AID, @GID

-- Retail - Travelcard
EXEC AddUpdateProperty 'Retail.Travelcard.ProcessJourneyLeg.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Retail.Travelcard.RetailHandoffXml.IncludeTravelcard.Switch', 'false', @AID, @GID

-- Retail - Combined Coach and Rail scenario
EXEC AddUpdateProperty 'Retail.CombinedCoachRail.CoachServiceOperatorCode', '5364', @AID, @GID
EXEC AddUpdateProperty 'Retail.CombinedCoachRail.RailServiceOperatorCode', 'RailSE', @AID, @GID
EXEC AddUpdateProperty 'Retail.CombinedCoachRail.CoachStationNaptan', '2400100073', @AID, @GID
EXEC AddUpdateProperty 'Retail.CombinedCoachRail.RailStationNaptan', '9100EBSFDOM', @AID, @GID

-- Header Links
EXEC AddUpdateProperty 'Header.Link.SkipToContent.Visible.Switch', 'true', @AID, @GID

-- EventDateControl
EXEC AddUpdateProperty 'EventDateControl.DropDownTime.Outward.Default', '09:00', @AID, @GID
EXEC AddUpdateProperty 'EventDateControl.DropDownTime.Return.Default', '17:00', @AID, @GID
EXEC AddUpdateProperty 'EventDateControl.DropDownTime.IntervalMinutes', '15', @AID, @GID
EXEC AddUpdateProperty 'EventDateControl.DropDownTime.OutwardReturnIntervalHours', '1', @AID, @GID
EXEC AddUpdateProperty 'EventDateControl.Now.Link.Switch', 'false', @AID, @GID

-- LocationControl
EXEC AddUpdateProperty 'LocationControl.VenueGrouping.Switch','true', @AID, @GID

-- Journey Options - Replan journey
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.Earlier.Interval.Minutes', '120', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.Later.Interval.Minutes', '1', @AID, @GID

EXEC AddUpdateProperty 'JourneyOptions.Replan.Earlier.Interval.Minutes', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.Replan.Later.Interval.Minutes', '1', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.Replan.Earlier.River.Interval.Minutes', '120', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.Replan.Later.River.Interval.Minutes', '1', @AID, @GID


-- Journey Details Leg - Instruction Underground color coding
EXEC AddUpdateProperty 'JourneyDetails.LondonUnderground.Operator','London Underground', @AID, @GID
EXEC AddUpdateProperty 'JourneyDetails.LondonUnderground.Service.ColorCode','true', @AID, @GID
EXEC AddUpdateProperty 'JourneyDetails.LondonDLR.Operator','Dockland Light Railway', @AID, @GID

-- Journey Details Leg - Suppress accessibility icons for stop naptans with following prefix, comma seperated list
EXEC AddUpdateProperty 'JourneyDetails.Accessibility.Icons.SuppressForNaPTANs','8100', @AID, @GID

-- Journey Details Leg - Identify a venue location naptan
EXEC AddUpdateProperty 'JourneyDetails.Location.Venue.Naptan.Prefix','8100', @AID, @GID

-- Journey Details Leg - Identify cable car location naptans (Greenwich MET, Greenwich PLT, Royal Docks MET, Royal Docks PLT)
EXEC AddUpdateProperty 'JourneyDetails.Location.CableCar.Naptans','9400ZZALGWP,9400ZZALGWP1,9400ZZALRDK,9400ZZALRDK1', @AID, @GID

-- Journey Details Leg - GPXLink Switch
EXEC AddUpdateProperty 'DetailsLegControl.GPXLink.Available.Switch','true', @AID, @GID

-- Journey Details Leg - ServiceNumber for Bus and Coach on Journey Result
EXEC AddUpdateProperty 'DetailsLegControl.ShowServiceNumber.Switch','true', @AID, @GID

-- Journey Details Leg - Check constraint minimum duration total for the queue mode and icon to be shown
EXEC AddUpdateProperty 'DetailsLegControl.CheckConstraint.ShowQueue.MinimumDuration.Seconds','120', @AID, @GID

-- Journey Details Leg - Leg minimum duration for the allow time for entering/exiting venue to be shown (only used for venue legs)
EXEC AddUpdateProperty 'DetailsLegControl.AllowTime.ShowDelay.MinimumDuration.Seconds','121', @AID, @GID

-- Cycle Detail Formatter
EXEC AddUpdateProperty 'CyclePlanner.CycleJourneyDetailsControl.ImmediateTurnDistance','161', @AID, @GID
EXEC AddUpdateProperty 'CyclePlanner.Display.AdditionalInstructionText','true', @AID, @GID

-- Car Detail Formatter
EXEC AddUpdateProperty 'Web.CarJourneyDetailsControl.ImmediateTurnDistance','161', @AID, @GID
EXEC AddUpdateProperty 'Web.CarJourneyDetailsControl.SlipRoadDistance','805', @AID, @GID
EXEC AddUpdateProperty 'CCN0602LondonCCzoneExtraTextVisible','false', @AID, @GID

-- Accessibility Options
EXEC AddUpdateProperty 'JourneyPlannerInput.CheckForGNATStation.Switch','true', @AID, @GID
EXEC AddUpdateProperty 'AccessibilityOptions.DistrictList.AdminAreaCode.London','82', @AID, @GID
EXEC AddUpdateProperty 'AccessibilityOptions.DistrictList.Visible.LondonOnly','true', @AID, @GID

-- The mall naptan
EXEC AddUpdateProperty 'JourneyPlannerInput.TheMallNaptan','8100MAL', @AID, @GID


-- Debug information
EXEC AddUpdateProperty 'Debug.Information.Show.Switch','false', @AID, @GID


GO
-- =============================================
-- Script to add properties data
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPWeb' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'SJPWeb'
DECLARE @GID varchar(50) = 'UserPortal'

-- State Server
EXEC AddUpdateProperty 'StateServer.ApplicationName', 'SJPStateServer', @AID, @GID

-- Styling
EXEC AddUpdateProperty 'Style.Version', 'VersionGTW', @AID, @GID

-- Google Analytics and Adverts
EXEC AddUpdateProperty 'Analytics.Tag.Include.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Adverts.Tag.Include.Switch', 'true', @AID, @GID

-- Canonical Tags
EXEC AddUpdateProperty 'Canonical.Tag.Include.Switch', 'false', @AID, @GID

-- Language Link
EXEC AddUpdateProperty 'Header.Link.Language.Visible.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Header.Link.Language.Paralympics.Visible.Switch', 'false', @AID, @GID

-- JourneyOptionTabContainer
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.PublicTransport.Disabled', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.RiverServices.Disabled', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.ParkAndRide.Disabled', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.BlueBadge.Disabled', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.Cycle.Disabled', 'false', @AID, @GID

EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.PublicTransport.Visible', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.RiverServices.Visible', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.ParkAndRide.Visible', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.BlueBadge.Visible', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptionTabContainer.Tabs.Cycle.Visible', 'true', @AID, @GID

-- JourneyOptionTabContainer - Additional Mobility Options
EXEC AddUpdateProperty 'JourneyOptionTabContainer.PublicJourneyOptions.AccessibilityOptions.Visible', 'true', @AID, @GID

-- Journey Options
EXEC AddUpdateProperty 'JourneyOptions.ShowPrinterFriendlyLink.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.ShowJourneyExpanded.Switch', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.ShowSingleJourneyExpanded.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.NotesDisplayed.MaxNumber', '10', @AID, @GID

-- Journey Options - Wait/Refresh
EXEC AddUpdateProperty 'JourneyOptions.Wait.RefreshTime.Seconds', '5', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.Wait.RefreshCount.Max', '9', @AID, @GID

EXEC AddUpdateProperty 'RiverServicesJourneyLocations.Wait.RefreshTime.Seconds','2', @AID, @GID
EXEC AddUpdateProperty 'RiverServicesJourneyLocations.Wait.RefreshCount.Max','9', @AID, @GID

-- Journey Options - Replan
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.Visible.Return.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.MaxJourneys.Visible', '8', @AID, @GID

EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.Visible.Return.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.MaxJourneys.Visible', '8', @AID, @GID

EXEC AddUpdateProperty 'RiverServiceResults.Replan.EarlierLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'RiverServiceResults.Replan.EarlierLink.Visible.Return.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'RiverServiceResults.Replan.EarlierLink.MaxJourneys.Visible', '15', @AID, @GID

EXEC AddUpdateProperty 'RiverServiceResults.Replan.LaterLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'RiverServiceResults.Replan.LaterLink.Visible.Return.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'RiverServiceResults.Replan.LaterLink.MaxJourneys.Visible', '15', @AID, @GID

EXEC AddUpdateProperty 'JourneyOptions.Replan.RetainPreviousJourneys.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.Replan.RetainPreviousJourneysWhenNoResults.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'JourneyOptions.Replan.RetainPreviousJourneys.River.Switch', 'true', @AID, @GID

-- Map
EXEC AddUpdateProperty 'Map.Journey.Cycle.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Map.Journey.Walk.Enabled.Switch', 'true', @AID, @GID

-- Journey Location Control
EXEC AddUpdateProperty 'LocationControl.LocationSuggest.ScriptId', 'SJPWeb_23052012', @AID, @GID

-- Journey Locations - Park And Ride
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.JouneyDate.Validate.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.JourneyTime.Validate.Switch', 'true', @AID, @GID

EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.ParkAndRide.DropDownTimeSlot.StartTime', '0600', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.ParkAndRide.DropDownTimeSlot.EndTime', '2300', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.ParkAndRide.DropDownTimeSlot.IntervalMinutes', '30', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.BlueBadge.DropDownTimeSlot.StartTime', '0600', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.BlueBadge.DropDownTimeSlot.EndTime', '2300', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.BlueBadge.DropDownTimeSlot.IntervalMinutes', '30', @AID, @GID

EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.ParkAndRide.Booking.Switch', 'true', @AID, @GID

-- Journey Locations - Cycle
EXEC AddUpdateProperty 'CycleJourneyLocations.JouneyDate.Validate.Switch', 'true', @AID, @GID

-- Journey Locations - Venue map clickable map switch
EXEC AddUpdateProperty 'RiverServicesJourneyLocations.VenueMap.Clickable.Switch','false', @AID, @GID
EXEC AddUpdateProperty 'CycleJourneyLocations.VenueMap.Clickable.Switch','false', @AID, @GID
EXEC AddUpdateProperty 'ParkAndRideJourneyLocations.VenueMap.Clickable.Switch','false', @AID, @GID

-- Journey Locations - Switch to hide/show the route images on the river services maps when user hovers over the remote pier
EXEC AddUpdateProperty 'RiverServicesJourneyLocations.VenueMap.MapRoutes.Switch','false', @AID, @GID

-- Widget - TopTips Promo
EXEC AddUpdateProperty 'Promos.TopTipsWidget.Visible','true', @AID, @GID

-- Widget - Travel News
EXEC AddUpdateProperty 'TravelNewsWidget.Headlines.Count','6', @AID, @GID
EXEC AddUpdateProperty 'TravelNewsWidget.JourneyBasedFilter.UseVenueNaptan','true', @AID, @GID 
EXEC AddUpdateProperty 'TravelNewsWidget.JourneyBasedFilter.UseVenueRegion','true', @AID, @GID 
EXEC AddUpdateProperty 'TravelNewsWidget.JourneyBasedFilter.UseJourneyDate','true', @AID, @GID
EXEC AddUpdateProperty 'TravelNewsWidget.Regions.FilterAndSort','London,South West', @AID, @GID

-- Side Bar Right Control - Journey Planner Input
EXEC AddUpdateProperty 'SideBarRightControl.JourneyPlannerInput.TopTipsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyPlannerInput.FAQWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyPlannerInput.WalkingWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyPlannerInput.VenueMapsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyPlannerInput.TravelNewsWidget.Visible','true', @AID, @GID

-- Side Bar Right Control - Journey Locations 
EXEC AddUpdateProperty 'SideBarRightControl.JourneyLocations.TopTipsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyLocations.FAQWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyLocations.AccessibleTravelWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyLocations.GamesTravelCardWidget.Visible','true', @AID, @GID

-- Side Bar Right Control - Journey Result 
EXEC AddUpdateProperty 'SideBarRightControl.JourneyOptions.TopTipsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyOptions.VenueMapsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyOptions.GamesTravelCardWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.JourneyOptions.TravelNewsWidget.Visible','true', @AID, @GID

-- Side Bar Right Control - Retailers 
EXEC AddUpdateProperty 'SideBarRightControl.Retailers.TopTipsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.Retailers.VenueMapsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.Retailers.GamesTravelCardWidget.Visible','true', @AID, @GID

-- Side Bar Right Control - Travel news 
EXEC AddUpdateProperty 'SideBarRightControl.TravelNews.TopTipsWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.TravelNews.JourneyPlannerWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.TravelNews.TravelNewsInfoWidget.Visible','true', @AID, @GID

-- Side Bar Right Control - Accessibility Options page
EXEC AddUpdateProperty 'SideBarRightControl.AccessibilityOptions.GBGNATMapWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.AccessibilityOptions.SEGNATMapWidget.Visible','true', @AID, @GID
EXEC AddUpdateProperty 'SideBarRightControl.AccessibilityOptions.LondonGNATMapWidget.Visible','true', @AID, @GID

-- Landing Page
EXEC AddUpdateProperty 'LandingPage.Location.Coordinate.LatitudeLongitude.Switch', 'true', @AID, @GID

-- Travel News
EXEC AddUpdateProperty 'TravelNews.Enabled.Switch', 'true', @AID, @GID

-- Underground News (Currently not implemented in SJPWeb but included for future enhancement similar to mobile)
EXEC AddUpdateProperty 'UndergroundNews.Enabled.Switch', 'true', @AID, @GID

-- Travel News - Auto Refresh
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.ShowRefreshLink.Switch', 'false', @AID, @GID
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Refresh.Seconds', '360', @AID, @GID

-- Travel News - Interactive map
EXEC AddUpdateProperty 'UKRegionImageMap.RegionIds', '1,2,3,4,5,6,7,8,9,10,11', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.ImageUrlResourceId', 'UKRegionImageMap.ImageUrl', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.ToolTip', 'UKRegionImageMap.ToolTip', @AID, @GID
-- All (ID is 0 but image map does not need to be defined)
-- London
-- East Anglia
-- East Midlands
-- South East
-- South West
-- West Midlands
-- Yorkshire and Humber
-- North East
-- North West
-- Scotland
-- Wales

EXEC AddUpdateProperty 'UKRegionImageMap.1.HighlightImageResourceId', 'UKRegionImageMap.London.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.1.x', '118', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.1.y', '170', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.2.x', '112', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.2.y', '170', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.3.x', '107', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.3.y', '172', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.4.x', '108', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.4.y', '176', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.5.x', '114', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.5.y', '178', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.6.x', '119', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.6.y', '177', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.7.x', '127', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.point.7.y', '174', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.points', '7', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.SelectedImageResourceId', 'UKRegionImageMap.London.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.1.ToolTip', 'DataServices.NewsRegionDrop.London', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.2.HighlightImageResourceId', 'UKRegionImageMap.East Anglia.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.1.x', '108', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.1.y', '147', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.10.x', '135', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.10.y', '137', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.11.x', '121', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.11.y', '138', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.12.x', '119', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.12.y', '143', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.13.x', '115', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.13.y', '148', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.2.x', '110', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.2.y', '149', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.3.x', '108', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.3.y', '156', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.4.x', '114', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.4.y', '156', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.5.x', '115', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.5.y', '159', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.6.x', '125', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.6.y', '159', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.7.x', '132', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.7.y', '165', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.8.x', '140', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.8.y', '152', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.9.x', '141', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.point.9.y', '143', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.points', '13', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.SelectedImageResourceId', 'UKRegionImageMap.East Anglia.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.2.ToolTip', 'DataServices.NewsRegionDrop.East Anglia', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.3.HighlightImageResourceId', 'UKRegionImageMap.East Midlands.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.1.x', '86', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.1.y', '129', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.10.x', '109', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.10.y', '150', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.11.x', '107', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.11.y', '154', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.12.x', '106', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.12.y', '159', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.13.x', '96', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.13.y', '162', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.14.x', '85', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.14.y', '133', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.2.x', '91', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.2.y', '130', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.3.x', '96', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.3.y', '131', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.4.x', '114', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.4.y', '124', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.5.x', '119', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.5.y', '132', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.6.x', '115', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.6.y', '137', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.7.x', '119', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.7.y', '141', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.8.x', '115', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.8.y', '146', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.9.x', '108', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.point.9.y', '147', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.points', '14', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.SelectedImageResourceId', 'UKRegionImageMap.East Midlands.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.3.ToolTip', 'DataServices.NewsRegionDrop.East Midlands', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.4.HighlightImageResourceId', 'UKRegionImageMap.South East.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.1.x', '140', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.1.y', '176', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.10.x', '124', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.10.y', '160', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.11.x', '132', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.11.y', '165', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.12.x', '129', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.12.y', '171', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.13.x', '122', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.13.y', '171', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.14.x', '117', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.14.y', '168', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.15.x', '111', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.15.y', '168', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.16.x', '107', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.16.y', '171', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.17.x', '107', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.17.y', '176', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.18.x', '111', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.18.y', '178', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.19.x', '117', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.19.y', '179', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.2.x', '127', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.2.y', '190', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.20.x', '120', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.20.y', '176', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.3.x', '97', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.3.y', '192', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.4.x', '94', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.4.y', '194', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.5.x', '89', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.5.y', '191', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.6.x', '91', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.6.y', '164', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.7.x', '105', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.7.y', '160', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.8.x', '112', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.8.y', '157', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.9.x', '116', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.point.9.y', '161', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.points', '20', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.SelectedImageResourceId', 'UKRegionImageMap.South East.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.4.ToolTip', 'DataServices.NewsRegionDrop.South East', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.5.HighlightImageResourceId', 'UKRegionImageMap.South West.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.1.x', '89', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.1.y', '163', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.2.x', '72', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.2.y', '169', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.3.x', '66', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.3.y', '181', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.4.x', '47', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.4.y', '181', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.5.x', '22', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.5.y', '210', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.6.x', '56', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.6.y', '204', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.7.x', '87', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.point.7.y', '192', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.points', '7', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.SelectedImageResourceId', 'UKRegionImageMap.South West.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.5.ToolTip', 'DataServices.NewsRegionDrop.South West', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.6.HighlightImageResourceId', 'UKRegionImageMap.West Midlands.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.1.x', '83', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.1.y', '135', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.2.x', '93', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.2.y', '155', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.3.x', '93', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.3.y', '162', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.4.x', '72', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.4.y', '166', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.5.x', '68', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.point.5.y', '138', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.points', '5', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.SelectedImageResourceId', 'UKRegionImageMap.West Midlands.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.6.ToolTip', 'DataServices.NewsRegionDrop.West Midlands', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.7.HighlightImageResourceId', 'UKRegionImageMap.Yorkshire and Humber.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.1.x', '102', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.1.y', '102', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.2.x', '95', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.2.y', '106', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.3.x', '91', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.3.y', '102', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.4.x', '82', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.4.y', '103', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.5.x', '78', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.5.y', '107', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.6.x', '86', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.6.y', '127', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.7.x', '95', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.7.y', '130', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.8.x', '113', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.8.y', '122', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.9.x', '108', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.point.9.y', '110', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.points', '9', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.SelectedImageResourceId', 'UKRegionImageMap.Yorkshire and Humber.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.7.ToolTip', 'DataServices.NewsRegionDrop.Yorkshire and Humber', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.8.HighlightImageResourceId', 'UKRegionImageMap.North East.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.1.x', '80', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.1.y', '71', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.2.x', '71', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.2.y', '86', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.3.x', '79', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.3.y', '103', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.4.x', '83', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.4.y', '102', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.5.x', '91', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.5.y', '101', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.6.x', '95', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.6.y', '105', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.7.x', '99', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.point.7.y', '102', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.points', '7', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.SelectedImageResourceId', 'UKRegionImageMap.North East.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.8.ToolTip', 'DataServices.NewsRegionDrop.North East', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.9.HighlightImageResourceId', 'UKRegionImageMap.North West.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.1.x', '65', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.1.y', '131', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.10.x', '71', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.10.y', '138', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.2.x', '67', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.2.y', '113', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.3.x', '57', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.3.y', '100', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.4.x', '60', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.4.y', '92', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.5.x', '72', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.5.y', '86', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.6.x', '79', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.6.y', '102', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.7.x', '76', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.7.y', '108', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.8.x', '83', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.8.y', '126', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.9.x', '83', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.point.9.y', '134', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.points', '10', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.SelectedImageResourceId', 'UKRegionImageMap.North West.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.9.ToolTip', 'DataServices.NewsRegionDrop.North West', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.10.HighlightImageResourceId', 'UKRegionImageMap.Scotland.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.1.x', '71', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.1.y', '82', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.10.x', '80', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.10.y', '69', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.2.x', '51', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.2.y', '95', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.3.x', '35', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.3.y', '97', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.4.x', '1', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.4.y', '41', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.5.x', '1', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.5.y', '1', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.6.x', '65', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.6.y', '1', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.7.x', '53', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.7.y', '17', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.8.x', '79', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.8.y', '22', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.9.x', '81', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.point.9.y', '28', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.points', '10', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.SelectedImageResourceId', 'UKRegionImageMap.Scotland.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.10.ToolTip', 'DataServices.NewsRegionDrop.Scotland', @AID, @GID

EXEC AddUpdateProperty 'UKRegionImageMap.11.HighlightImageResourceId', 'UKRegionImageMap.Wales.HighlightImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.1.x', '64', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.1.y', '129', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.2.x', '43', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.2.y', '128', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.3.x', '31', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.3.y', '170', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.4.x', '61', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.4.y', '177', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.5.x', '71', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.point.5.y', '170', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.points', '5', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.RegionType', 'polygon', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.SelectedImageResourceId', 'UKRegionImageMap.Wales.SelectedImage', @AID, @GID
EXEC AddUpdateProperty 'UKRegionImageMap.11.ToolTip', 'DataServices.NewsRegionDrop.Wales', @AID, @GID

GO
-- =============================================
-- Script to add properties data
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPMobile' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'SJPMobile'
DECLARE @GID varchar(50) = 'UserPortal'

-- State Server
EXEC AddUpdateProperty 'StateServer.ApplicationName', 'SJPMobileStateServer', @AID, @GID

-- Styling
EXEC AddUpdateProperty 'Style.Version', 'Version', @AID, @GID

-- StyleSheets
EXEC AddUpdateProperty 'StyleSheet.Files.Keys', '', @AID, @GID

-- Javascripts
EXEC AddUpdateProperty 'Javscript.Files.Keys', 'jquery', @AID, @GID
EXEC AddUpdateProperty 'Javscript.File.jquery', 'https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js', @AID, @GID

-- Google Analytics and Adverts
EXEC AddUpdateProperty 'Analytics.Tag.Include.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Adverts.Tag.Include.Switch', 'false', @AID, @GID

-- Canonical Tags
EXEC AddUpdateProperty 'Canonical.Tag.Include.Switch', 'false', @AID, @GID

-- Language Link
EXEC AddUpdateProperty 'Header.Link.Language.Visible.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Header.Link.Language.Paralympics.Visible.Switch', 'false', @AID, @GID

-- JourneyOptionTabContainer - Cycle
EXEC AddUpdateProperty 'CyclePlanner.Enabled.Switch', 'true', @AID, @GID

-- JourneyOptionTabContainer - Additional Mobility Options
EXEC AddUpdateProperty 'PublicJourneyOptions.AccessibilityOptions.Visible', 'true', @AID, @GID

-- Journey Location Control
EXEC AddUpdateProperty 'LocationControl.LocationSuggest.ScriptId', 'SJPMobile_23052012', @AID, @GID

-- Journey Location - Toggle
EXEC AddUpdateProperty 'JourneyLocations.Toggle.Enabled.Switch', 'true', @AID, @GID

-- Journey Options - Wait/Refresh
EXEC AddUpdateProperty 'JourneySummary.Wait.RefreshTime.Seconds', '5', @AID, @GID
EXEC AddUpdateProperty 'JourneySummary.Wait.RefreshCount.Max', '6', @AID, @GID

-- Journey Options - Return Journey button
EXEC AddUpdateProperty 'JourneySummary.PlanReturnJourney.Visible.Switch', 'true', @AID, @GID

-- Journey Options - Replan
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.EarlierLink.MaxJourneys.Visible', '4', @AID, @GID

EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.Visible.Outward.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'DetailsSummaryControl.Replan.LaterLink.MaxJourneys.Visible', '4', @AID, @GID

EXEC AddUpdateProperty 'JourneySummary.Replan.RetainPreviousJourneys.Switch', 'false', @AID, @GID
EXEC AddUpdateProperty 'JourneySummary.Replan.RetainPreviousJourneysWhenNoResults.Switch', 'true', @AID, @GID

-- Map
EXEC AddUpdateProperty 'Map.Input.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Map.Journey.Cycle.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'Map.Journey.Walk.Enabled.Switch', 'true', @AID, @GID

-- Landing Page
EXEC AddUpdateProperty 'LandingPage.Location.Coordinate.LatitudeLongitude.Switch', 'true', @AID, @GID

-- Travel News
EXEC AddUpdateProperty 'TravelNews.Enabled.Switch', 'true', @AID, @GID

-- Travel News - Auto Refresh
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Enabled.Switch', 'true', @AID, @GID
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.ShowRefreshLink.Switch', 'false', @AID, @GID
EXEC AddUpdateProperty 'TravelNews.AutoRefresh.Refresh.Seconds', '360', @AID, @GID

-- Underground News
EXEC AddUpdateProperty 'UndergroundNews.Enabled.Switch', 'true', @AID, @GID

GO

-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPWeb' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'SJPWeb'
DECLARE @GID varchar(50) = 'UserPortal'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', 'EVENTLOG1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', 'QUEUE1', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Name', 'Application', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Source', @AID, @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Machine', '.', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Path', '.\Private$\SJPPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Delivery', 'Recoverable', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Priority', 'Normal', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Warning', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1 EVENTLOG1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', 'PAGE JOURNEYREQUEST JOURNEYRESULTS CYCLEREQUEST CYCLERESULT REPEATVISITOR RETAIL LANDING GATEWAY REFTRANS WORKLOAD STOPEVENT NORESULTS', @AID, @GID

-- Custom Events Configuration
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Name', 'PageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Name', 'JourneyPlanRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Name', 'JourneyPlanResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Name', 'CyclePlannerRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Name', 'CyclePlannerResultEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Name', 'RepeatVisitorEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Name', 'RetailerHandoffEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Name', 'LandingPageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Name', 'DataGatewayEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Name', 'ReferenceTransactionEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Name', 'WorkloadEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Name', 'StopEventRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Name', 'NoResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Trace', 'On', @AID, @GID
GO
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'SJPWeb' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'SJPMobile'
DECLARE @GID varchar(50) = 'UserPortal'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', 'EVENTLOG1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', 'QUEUE1', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Name', 'Application', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Source', @AID, @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Machine', '.', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Path', '.\Private$\SJPPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Delivery', 'Recoverable', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Priority', 'Normal', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Warning', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1 EVENTLOG1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', 'PAGE JOURNEYREQUEST JOURNEYRESULTS CYCLEREQUEST CYCLERESULT REPEATVISITOR RETAIL LANDING GATEWAY REFTRANS WORKLOAD STOPEVENT NORESULTS', @AID, @GID

-- Custom Events Configuration
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Name', 'PageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Name', 'JourneyPlanRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Name', 'JourneyPlanResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Name', 'CyclePlannerRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Name', 'CyclePlannerResultEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Name', 'RepeatVisitorEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Name', 'RetailerHandoffEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Name', 'LandingPageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Name', 'DataGatewayEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Name', 'ReferenceTransactionEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Name', 'WorkloadEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Name', 'StopEventRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Name', 'NoResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Publishers', 'QUEUE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Trace', 'On', @AID, @GID

GO

-- Command and Control
-- =============================================
-- Script Template
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CCAgent' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'CCAgent'
DECLARE @GID varchar(50) = 'UserPortal'

-- Poll interval
EXEC AddUpdateProperty 'CCAgent.OverallPollIntervalSeconds',	'30', @AID, @GID

-- Data Notification - Groups
EXEC AddUpdateProperty 'DataNotification.PollingInterval.Seconds', '60', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.Groups', 'CommandControl', @AID, @GID

-- Data Notification - Tables
EXEC AddUpdateProperty 'DataNotification.CommandControl.Database', 'CommandControlDB', @AID, @GID
EXEC AddUpdateProperty 'DataNotification.CommandControl.Tables', 'ChecksumMonitoringItems,DatabaseMonitoringItems,FileMonitoringItems,WMIMonitoringItems', @AID, @GID

GO
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CCAgent' properties
------------------------------------------------
DECLARE @AID varchar(50) = 'CCAgent'
DECLARE @GID varchar(50) = 'UserPortal'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', 'EVENTLOG1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', 'QUEUE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Name', 'Application', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Source', @AID, @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog.EVENTLOG1.Machine', '.', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Path', '.\Private$\SJPPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Delivery', 'Recoverable', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue.QUEUE1.Priority', 'Normal', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Warning', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1 EVENTLOG1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', '', @AID, @GID

GO

-- Coordinate Convertor
-- =============================================
-- Script to add properties data
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CoordinateConvertorService' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'CoordinateConvertorService'
DECLARE @GID varchar(50) = 'UserPortal'

-- Coordinate Convertor
EXEC AddUpdateProperty 'Coordinate.Convertor.DataPath','D:\inetpub\wwwroot\CoordinateConvertorService\bin', @AID, @GID
EXEC AddUpdateProperty 'Coordinate.Convertor.InitialiseString', 'GIQ.6.0', @AID, @GID

GO
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'CoordinateConvertorService' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'CoordinateConvertorService'
DECLARE @GID varchar(50) = 'UserPortal'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Warning', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'Off', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', '', @AID, @GID

GO

-- Data Services
-- =============================================
-- Script Template
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'DataServices' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'DataServices'
DECLARE @GID varchar(50) = 'UserPortal'

-- SJP Event dates - to populate the calendars
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.SJPEventDates.db',	'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.SJPEventDates.query','SELECT eventdate FROM SJPEventDates', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.SJPEventDates.type','2', @AID, @GID

-- SJP Cycle Route types - to populate the cycle route types on journey locations page
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.CycleRouteType.db',	'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.CycleRouteType.query','SELECT ResourceID, ItemValue, IsSelected FROM DropDownLists WHERE DataSet = ''CycleRouteType'' ORDER BY SortOrder', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.CycleRouteType.type','3', @AID, @GID

-- SJP Travel News regions
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.NewsRegionDrop.db',	'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.NewsRegionDrop.query','SELECT ResourceID, ItemValue, IsSelected FROM DropDownLists WHERE DataSet = ''NewsRegionDrop'' ORDER BY SortOrder', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.NewsRegionDrop.type','3', @AID, @GID

-- NPTG Countries
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.CountryDrop.db',	'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.CountryDrop.query','SELECT ResourceID, ItemValue, IsSelected FROM DropDownLists WHERE DataSet = ''CountryDrop'' ORDER BY SortOrder', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.CountryDrop.type','3', @AID, @GID

-- SJP Travel News mode
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.NewsViewMode.db',	'TransientPortalDB', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.NewsViewMode.query','SELECT ResourceID, ItemValue, IsSelected FROM DropDownLists WHERE DataSet = ''NewsViewMode'' ORDER BY SortOrder', @AID, @GID
EXEC AddUpdateProperty 'SJP.UserPortal.DataServices.NewsViewMode.type','3', @AID, @GID

GO

-- Event Receiver
-- =============================================
-- Script to add reporting properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'EventReceiver' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'EventReceiver'
DECLARE @GID varchar(50) = 'Reporting'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

-- Message Queues to monitor for events
EXEC AddUpdateProperty 'EventReceiver.Queue', 'SourceQueue1 SourceQueue2', @AID, @GID
EXEC AddUpdateProperty 'EventReceiver.Queue.SourceQueue1.Path', '.\Private$\SJPPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'EventReceiver.Queue.SourceQueue2.Path', '.\Private$\TDPrimaryQueue', @AID, @GID
EXEC AddUpdateProperty 'EventReceiver.TimeBeforeRecovery.Millisecs', '60000', @AID, @GID

GO
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'EventReceiver' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'EventReceiver'
DECLARE @GID varchar(50) = 'Reporting'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', 'SJPDB OPDB CJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Custom.SJPDB.Name', 'SJPCustomEventPublisher', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom.OPDB.Name', 'SJPOperationalEventPublisher', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom.CJPDB.Name', 'CJPCustomEventPublisher', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Verbose', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', 'PAGE JOURNEYREQUEST JOURNEYRESULTS CYCLEREQUEST CYCLERESULT REPEATVISITOR RETAIL LANDING GATEWAY REFTRANS WORKLOAD STOPEVENT ROE CJPJOURNEYWEB CJPLOCATION CJPINTERNAL NORESULTS', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Name', 'PageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Publishers', 'SJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.PAGE.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Name', 'JourneyPlanRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Name', 'JourneyPlanResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.JOURNEYRESULTS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Name', 'CyclePlannerRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLEREQUEST.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Name', 'CyclePlannerResultEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CYCLERESULT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Name', 'RepeatVisitorEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REPEATVISITOR.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Name', 'RetailerHandoffEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.RETAIL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Name', 'LandingPageEntryEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.LANDING.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Name', 'DataGatewayEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.GATEWAY.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Name', 'ReferenceTransactionEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.REFTRANS.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Name', 'WorkloadEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Name', 'StopEventRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.STOPEVENT.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Name', 'ReceivedOperationalEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Publishers', 'OPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.ROE.Trace', 'On', @AID, @GID


-- TDP CJP Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Name', 'JourneyWebRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Publishers', 'CJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPJOURNEYWEB.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Name', 'LocationRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Publishers', 'CJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPLOCATION.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Name', 'InternalRequestEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Publishers', 'CJPDB FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.CJPINTERNAL.Trace', 'On', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Name', 'NoResultsEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.NORESULTS.Trace', 'On', @AID, @GID

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

GO

-- Web Log Reader
-- =============================================
-- Script Template
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'WebLogReader' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'WebLogReader'
DECLARE @GID varchar(50) = 'Reporting'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

EXEC AddUpdateProperty 'WebLogReader.WebLogFolders', 'W3SVC1 W3SVC3', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC1.LogDirectory', 'D:\info\logs\LogFiles\W3SVC1', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC1.ArchiveDirectory', 'D:\info\logs\LogFiles\W3SVC1\Archive', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC3.LogDirectory', 'D:\info\logs\LogFiles\W3SVC3', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.W3SVC3.ArchiveDirectory', 'D:\info\logs\LogFiles\W3SVC3\Archive', @AID, @GID

EXEC AddUpdateProperty 'WebLogReader.WebPageExtensions', 'asp aspx htm html pdf ashx axd [none]', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.NonPageMinimumBytes', '5000000', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ClientIPExcludes', '', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.UseLocalTime', 'false', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.RolloverPeriod', 'Hourly', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.Ranges', 'RANGE1 RANGE2', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE1.Min', '100', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE1.Max', '399', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE2.Min', '500', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.ValidStatusCode.RANGE2.Max', '599', @AID, @GID

EXEC AddUpdateProperty 'WebLogReader.UserExperienceVisitor.UserAgent', 'SiteCon', @AID, @GID -- Not case sensitive
EXEC AddUpdateProperty 'WebLogReader.Cookie.SessionId.StartMarker', 'ASP.NET_SessionId=', @AID, @GID
EXEC AddUpdateProperty 'WebLogReader.Cookie.SessionId.EndMarker', ';', @AID, @GID

GO
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'WebLogReader' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'WebLogReader'
DECLARE @GID varchar(50) = 'Reporting'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', 'SJPDB OPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Custom.SJPDB.Name', 'SJPCustomEventPublisher', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom.OPDB.Name', 'SJPOperationalEventPublisher', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Verbose', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'On', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', 'WORKLOAD', @AID, @GID

EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Name', 'WorkloadEvent', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Assembly', 'sjp.reporting.events', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Publishers', 'SJPDB', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom.WORKLOAD.Trace', 'On', @AID, @GID

GO

-- Data Loader
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
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'DataLoader' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'DataLoader'
DECLARE @GID varchar(50) = 'DataGateway'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Verbose', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'Off', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', '', @AID, @GID

GO

--Venue Incidents
-- =============================================
-- Script Template
-- =============================================
USE SJPConfiguration
GO

------------------------------------------------
-- 'VenueIncidents' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'VenueIncidents'
DECLARE @GID varchar(50) = 'FileCreation'

-- Property Service
EXEC AddUpdateProperty 'propertyservice.version', '1', @AID, @GID
EXEC AddUpdateProperty 'propertyservice.refreshrate', '300000', @AID, @GID

-- Venue Incidents
EXEC AddUpdateProperty 'VenueIncidents.OutputFile.Location', 'D:\inetpub\wwwroot\Data\TravelNews\VenueIncidents.xml', @AID, @GID
EXEC AddUpdateProperty 'VenueIncidents.IncidentLandingPage.Location', 'http://m.travel.london2012.com/TravelNews.aspx?nv={0}&pn=0', @AID, @GID

GO
-- =============================================
-- Script to add event logging properties
-- =============================================

USE SJPConfiguration
GO

------------------------------------------------
-- 'VenueIncidents' properties
------------------------------------------------

DECLARE @AID varchar(50) = 'VenueIncidents'
DECLARE @GID varchar(50) = 'FileCreation'

-- Publishers
EXEC AddUpdateProperty 'Logging.Publisher.Default',	'FILE1', @AID, @GID

EXEC AddUpdateProperty 'Logging.Publisher.Console', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Custom', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Email', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.EventLog', '', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.Queue', '', @AID, @GID

-- Publisher Configurations
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Directory', 'D:\SJP', @AID, @GID
EXEC AddUpdateProperty 'Logging.Publisher.File.FILE1.Rotation', '20000', @AID, @GID

-- Event Logging Level and Publisher mapping
EXEC AddUpdateProperty 'Logging.Event.Operational.TraceLevel', 'Verbose', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Verbose.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Info.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Warning.Publishers', 'FILE1', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Operational.Error.Publishers', 'FILE1', @AID, @GID

-- Custom Events
EXEC AddUpdateProperty 'Logging.Event.Custom.Trace', 'Off', @AID, @GID
EXEC AddUpdateProperty 'Logging.Event.Custom', '', @AID, @GID

GO

----------------------------------------------------

-- =============================================
-- Script to add Retailers
-- =============================================

-----------------------------------
-- The configuration property "Retail.Retailers.ShowTestRetailers.Switch" is used to display the test retailers
-----------------------------------


USE [SJPConfiguration] 
GO


-- Tidy up first, helps keep retailer tables clean,
-- and ensures this script contains complete control of the retailers in use
DELETE FROM [dbo].[RetailerLookup]
DELETE FROM [dbo].[Retailers]

-----------------------------------
-- Add Retailers 
-----------------------------------
-- EXEC AddUpdateRetailer @RetailerId, @Name, @WebsiteURL, @HandoffURL, @DisplayURL, @ResourceKey

-----------------------------------
-- Live retailer sites
-----------------------------------
-- Coach
EXEC AddUpdateRetailer 'DMT', 'DirectManagedTransport', 'http://www.firstgroup.com', 'http://www.firstgroupgamestravel.com/direct-coaching', '', '+448449212012', '0844 921 2012', 'Retailers.Retailer.Coach.DirectManagedTransport'

-- Rail
EXEC AddUpdateRetailer 'NR', 'NationalRail', 'http://www.nationalrail.co.uk/', 'http://tickets.nationalrailgamestravel.co.uk/sjp/sjplanding.aspx', '', '+448446932898', '0844 693 2898', 'Retailers.Retailer.Rail.NationalRail'

-- Ferry
EXEC AddUpdateRetailer 'CC', 'CityCruises', 'http://www.citycruises.co.uk', 'http://www.citycruisesgamestravel.co.uk', '', '+442077400400', '0207 7400 400', 'Retailers.Retailer.Ferry.CityCruises'
EXEC AddUpdateRetailer 'TC', 'ThamesClipper', 'http://www.thamesclippers.com', 'https://booking.thamesclippers.com/gamestravel', '', '+442070012200', '0207 001 2200', 'Retailers.Retailer.Ferry.ThamesClipper'


-----------------------------------
-- Test retailer sites (these must be prefixed with TEST and Property[Retail.Retailers.ShowTestRetailers.Switch] in configuration be true to display retailers)
-----------------------------------
-- Coach
EXEC AddUpdateRetailer 'TEST_DMT', 'DirectManagedTransport', 'http://www.firstgroup.com', 'http://dmt.spideronline.co.uk/direct-coaching', '', '', '', 'Retailers.Retailer.Coach.DirectManagedTransport.Test'

-- Rail
EXEC AddUpdateRetailer 'TEST_NR', 'NationalRail', 'http://www.nationalrail.co.uk/', 'http://tickets.nationalrailgamestravel.co.uk', '', '', '', 'Retailers.Retailer.Rail.NationalRail.Test'
EXEC AddUpdateRetailer 'TEST_WBT', 'WebTIS', 'http://tickets.redspottedhanky.com/', 'http://uat-tickets.redspottedhanky.com/sjp/sjplanding.aspx', '', '', '', 'Retailers.Retailer.Rail.WebTIS.Test'

-- Ferry
EXEC AddUpdateRetailer 'TEST_CC', 'CityCruises', 'http://www.citycruises.co.uk', 'http://citycruisescloud.cloudapp.net', '', '', '', 'Retailers.Retailer.Ferry.CityCruises.Test'
EXEC AddUpdateRetailer 'TEST_TC', 'ThamesClipper', 'http://www.thamesclippers.com', 'https://booking.thamesclippers.com/gamestravel', '', '', '', 'Retailers.Retailer.Ferry.ThamesClipper.Test'


-----------------------------------
-- Add Retailer Lookup
-----------------------------------
-- EXEC AddUpdateRetailerLookup @OperatorCode, @Mode, @RetailerId

-- Live retailer sites
EXEC AddUpdateRetailerLookup 'FOLY', 'Coach', 'DMT'
EXEC AddUpdateRetailerLookup '5364', 'Coach', 'DMT'

EXEC AddUpdateRetailerLookup 'NONE', 'Rail',  'NR'

EXEC AddUpdateRetailerLookup 'CCR', 'Ferry', 'CC'
EXEC AddUpdateRetailerLookup 'THC', 'Ferry', 'TC'
EXEC AddUpdateRetailerLookup 'TRS', 'Ferry', 'CC'

-- Test retailer sites
EXEC AddUpdateRetailerLookup 'FOLY', 'Coach', 'TEST_DMT'
EXEC AddUpdateRetailerLookup '5364', 'Coach', 'TEST_DMT'

EXEC AddUpdateRetailerLookup 'NONE', 'Rail',  'TEST_NR'
EXEC AddUpdateRetailerLookup 'NONE', 'Rail',  'TEST_WBT'

EXEC AddUpdateRetailerLookup 'CCR', 'Ferry', 'TEST_CC'
EXEC AddUpdateRetailerLookup 'THC', 'Ferry', 'TEST_TC'
EXEC AddUpdateRetailerLookup 'TRS', 'Ferry', 'TEST_CC'


GO

-- =============================================
-- Script Template
-- =============================================


USE [SJPConfiguration] 
GO


DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO



--NB This script MUST be the last one included as it updates changes made by previous scripts.
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
