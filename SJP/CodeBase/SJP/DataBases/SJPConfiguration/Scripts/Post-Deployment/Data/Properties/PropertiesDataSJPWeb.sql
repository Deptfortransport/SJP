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
