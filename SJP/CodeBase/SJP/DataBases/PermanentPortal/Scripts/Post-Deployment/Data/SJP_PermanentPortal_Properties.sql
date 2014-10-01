-- =============================================
-- Script Template
-- =============================================

-- Database Connection Strings
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'SJPGazetteerDB', N'<DEFAULT>', N'<DEFAULT>', 0, 1, N'Data Source=PRIMARY_WU;Initial Catalog=SJPGazetteer;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'SJPTransientPortalDB', N'<DEFAULT>', N'<DEFAULT>', 0, 1, N'Data Source=PRIMARY_WU;Initial Catalog=SJPTransientPortal;Pooling=False;Timeout=30;User id=SJP_User;Password=!password!1')


-- SJP Cycle Park

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.feedname', N'', N'DataGateway', 0, 1, N'ply010')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.Name', N'', N'DataGateway', 0, 1, N'SJPCycleParkLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPCycleParkLocations.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPCycleParkLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPCycleParkLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPCycleParkLocations.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Park And Ride Locations

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.feedname', N'', N'DataGateway', 0, 1, N'zse435')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.Name', N'', N'DataGateway', 0, 1, N'SJPParkAndRideLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPParkAndRideLocations.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPParkAndRideLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPParkAndRideLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideLocations.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Venue Additional Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.feedname', N'', N'DataGateway', 0, 1, N'gqv678')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.Name', N'', N'DataGateway', 0, 1, N'SJPAdditionalVenueData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\AdditionalVenueData.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPAdditionalVenueData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPAdditionalVenueData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPAdditionalVenueData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Venue Gate Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.feedname', N'', N'DataGateway', 0, 1, N'tgb987')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.Name', N'', N'DataGateway', 0, 1, N'SJPVenueGateData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\NeTEx\NeTEx_publication.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPVenueGateData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.netex.org.uk/netex')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueGateData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP GNAT AdminAreas Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.feedname', N'', N'DataGateway', 0, 1, N'sul834')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.Name', N'', N'DataGateway', 0, 1, N'SJPGNATAdminAreasData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPGNATAdminAreas.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPGNATAdminAreasData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPGNATAdminAreas')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATAdminAreasData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP GNAT Locations Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.feedname', N'', N'DataGateway', 0, 1, N'mkw489')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.Name', N'', N'DataGateway', 0, 1, N'SJPGNATLocationsData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPGNATLocations.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPGNATLocationsData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPGNATLocations')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPGNATLocationsData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Stop Accessibility Links Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.database', N'', N'DataGateway', 0, 1, N'SJPTransientPortalDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.feedname', N'', N'DataGateway', 0, 1, N'mnb765')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.Name', N'', N'DataGateway', 0, 1, N'SJPStopAccessibilityLinksData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPStopAccessibilityLinks.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPStopAccessibilityLinks')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPStopAccessibilityLinks')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPStopAccessibilityLinksData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Travelcard Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.database', N'', N'DataGateway', 0, 1, N'SJPTransientPortalDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.feedname', N'', N'DataGateway', 0, 1, N'mbt156')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.Name', N'', N'DataGateway', 0, 1, N'SJPTravelcardData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPTravelcardData.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPTravelcardData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPTravelCards')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelcardData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Venue Access Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.feedname', N'', N'DataGateway', 0, 1, N'cft439')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.Name', N'', N'DataGateway', 0, 1, N'SJPVenueAccessData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPVenueAccessData.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPVenueAccessData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPVenueAccessData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPVenueAccessData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Park And Ride TOIDs

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.database', N'', N'DataGateway', 0, 1, N'SJPGazetteerDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.feedname', N'', N'DataGateway', 0, 1, N'zdf451')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.Name', N'', N'DataGateway', 0, 1, N'SJPParkAndRideToids')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\SJPParkAndRideToids.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPParkAndRideToids')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/SJPParkAndRideToids')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPParkAndRideToids.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')

-- SJP Travel News Data

INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.database', N'', N'DataGateway', 0, 1, N'SJPTransientPortalDB')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.feedname', N'', N'DataGateway', 0, 1, N'omg654')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.Name', N'', N'DataGateway', 0, 1, N'SJPTravelNewsData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.schemea', N'', N'DataGateway', 0, 1, N'D:\gateway\bin\xml\OlympicTravelNews.xsd')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.sqlcommandtimeout', N'', N'DataGateway', 0, 1, N'3000')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.storedprocedure', N'', N'DataGateway', 0, 1, N'ImportSJPTravelNewsData')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.xmlnamespace', N'', N'DataGateway', 0, 1, N'http://www.transportdirect.info/olympictravelnews')
INSERT INTO [dbo].[properties] ([pName], [AID], [GID], [PartnerId], [ThemeId], [pValue]) VALUES (N'datagateway.sqlimport.SJPTravelNewsData.xmlnamespacexsi', N'', N'DataGateway', 0, 1, N'http://www.w3.org/2001/XMLSchema-instance')