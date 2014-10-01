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