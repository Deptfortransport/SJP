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

GRANT EXECUTE ON [dbo].[GetChangeTable]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetCycleAttributes]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetNPTGAdminAreas]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetNPTGDistricts]				TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRoutes]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetRouteEnds]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetStopAccessibilityLinks]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetTravelcards]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetUndergroundStatus]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetZones]						TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetZoneStops]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[GetZoneModes]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[TravelNewsAll]					TO [SJP_User]
GRANT EXECUTE ON [dbo].[TravelNewsHeadlines]			TO [SJP_User]
GRANT EXECUTE ON [dbo].[TravelNewsVenues]				TO [SJP_User]

GRANT EXECUTE ON [dbo].[ImportSJPStopAccessibilityLinks] TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPTravelcardData]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPTravelNewsData]		TO [SJP_User]
GRANT EXECUTE ON [dbo].[ImportSJPUndergroundStatusData]	TO [SJP_User]

GO

-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


EXEC AddChangeNotificationTable 'TravelNewsImport'
EXEC AddChangeNotificationTable 'UndergroundStatusImport'

GO

-- =============================================
-- Script Template
-- =============================================

USE [SJPTransientPortal] 

GO

DELETE FROM SJPEventDates

-- Remove the dates as needed
-- The Dates below ranges from 1 Jun, 2012 to 31 Aug, 2012

INSERT INTO SJPEventDates
		VALUES  ('2012/06/01'),
				('2012/06/02'),
				('2012/06/03'),
				('2012/06/04'),
				('2012/06/05'),
				('2012/06/06'),
				('2012/06/07'),
				('2012/06/08'),
				('2012/06/09'),
				('2012/06/10'),
				('2012/06/11'),
				('2012/06/12'),
				('2012/06/13'),
				('2012/06/14'),
				('2012/06/15'),
				('2012/06/16'),
				('2012/06/17'),
				('2012/06/18'),
				('2012/06/19'),
				('2012/06/20'),
				('2012/06/21'),
				('2012/06/22'),
				('2012/06/23'),
				('2012/06/24'),
				('2012/06/25'),
				('2012/06/26'),
				('2012/06/27'),
				('2012/06/28'),
				('2012/06/29'),
				('2012/06/30'),
				('2012/07/01'),
				('2012/07/02'),
				('2012/07/03'),
				('2012/07/04'),
				('2012/07/05'),
				('2012/07/06'),
				('2012/07/07'),
				('2012/07/08'),
				('2012/07/09'),
				('2012/07/10'),
				('2012/07/11'),
				('2012/07/12'),
				('2012/07/13'),
				('2012/07/14'),
				('2012/07/15'),
				('2012/07/16'),
				('2012/07/17'),
				('2012/07/18'),
				('2012/07/19'),
				('2012/07/20'),
				('2012/07/21'),
				('2012/07/22'),
				('2012/07/23'),
				('2012/07/24'),
				('2012/07/25'),
				('2012/07/26'),
				('2012/07/27'),
				('2012/07/28'),
				('2012/07/29'),
				('2012/07/30'),
				('2012/07/31'),
				('2012/08/01'),
				('2012/08/02'),
				('2012/08/03'),
				('2012/08/04'),
				('2012/08/05'),
				('2012/08/06'),
				('2012/08/07'),
				('2012/08/08'),
				('2012/08/09'),
				('2012/08/10'),
				('2012/08/11'),
				('2012/08/12'),
				('2012/08/13'),
				('2012/08/14'),
				('2012/08/15'),
				('2012/08/16'),
				('2012/08/17'),
				('2012/08/18'),
				('2012/08/19'),
				('2012/08/20'),
				('2012/08/21'),
				('2012/08/22'),
				('2012/08/23'),
				('2012/08/24'),
				('2012/08/25'),
				('2012/08/26'),
				('2012/08/27'),
				('2012/08/28'),
				('2012/08/29'),
				('2012/08/30'),
				('2012/08/31')


-- =============================================
-- Script Template - Adds DropDownLists data
-- =============================================

USE SJPTransientPortal
GO

------------------------------------------------
-- Clear data
------------------------------------------------
DELETE FROM [dbo].[DropDownLists]

------------------------------------------------
-- CycleRouteType dropdown
------------------------------------------------
INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CycleRouteType','Fastest','QuickestV913',0,2)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CycleRouteType','Quietest','QuietestV913',1,1)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CycleRouteType','Recreational','RecreationalV913',0,3)

------------------------------------------------
-- Travel News Regions dropdown
------------------------------------------------
-- IF order is changed, then ensure Properties for UKRegionImageMap are updated to reflect order
INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','All','All',0,1)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','London','London',1,2)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','EastAnglia','East Anglia',0,3)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','EastMidlands','East Midlands',0,4)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','SouthEast','South East',0,5)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','SouthWest','South West',0,6)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','WestMidlands','West Midlands',0,7)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','YorkshireandHumber','Yorkshire and Humber',0,8)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','NorthEast','North East',0,9)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','NorthWest','North West',0,10)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','Scotland','Scotland',0,11)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsRegionDrop','Wales','Wales',0,12)


------------------------------------------------
-- Country dropdown
------------------------------------------------
INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CountryDrop','Default','',1,1)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CountryDrop','England','Eng',0,2)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CountryDrop','Scotland','Sco',0,3)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('CountryDrop','Wales','Wal',0,4)


------------------------------------------------
-- Travel News view mode dropdown
------------------------------------------------
INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsViewMode','All','All',1,1)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsViewMode','LondonUnderground','LUL',0,2)

INSERT INTO [dbo].[DropDownLists]
           ([DataSet],[ResourceID],[ItemValue],[IsSelected],[SortOrder])
     VALUES
           ('NewsViewMode','Venue','Venue',0,3)



          

-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (0, N'No attributes', N'Link', N'ITN', N'None', N'CycleAttribute.NoCycleAttributes', N'0x00000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (1, N'Motorway', N'Link', N'ITN', N'Type', N'CycleAttribute.Motorway', N'0x00000001', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (2, N'A Road', N'Link', N'ITN', N'Type', N'CycleAttribute.ARoad', N'0x00000002', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (3, N'B Road', N'Link', N'ITN', N'Type', N'CycleAttribute.BRoad', N'0x00000004', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (4, N'Minor Road', N'Link', N'ITN', N'Type', N'CycleAttribute.MinorRoad', N'0x00000008', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (5, N'Local Street', N'Link', N'ITN', N'Type', N'CycleAttribute.LocalStreet', N'0x00000010', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (6, N'Alley', N'Link', N'ITN', N'Type', N'CycleAttribute.Alley', N'0x00000020', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (7, N'Private Road', N'Link', N'ITN', N'Type', N'CycleAttribute.PrivateRoad', N'0x00000040', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (8, N'Pedestrianised Street', N'Link', N'ITN', N'Type', N'CycleAttribute.PedestrianisedStreet', N'0x00000080', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (9, N'Toll Road', N'Link', N'ITN', N'Type', N'CycleAttribute.TollRoad', N'0x00000100', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (10, N'Single Carriageway', N'Link', N'ITN', N'Type', N'CycleAttribute.SingleCarriageway', N'0x00000200', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (11, N'Dual Carriageway', N'Link', N'ITN', N'Type', N'CycleAttribute.DualCarriageway', N'0x00000400', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (12, N'Slip Road', N'Link', N'ITN', N'Type', N'CycleAttribute.SlipRoad', N'0x00000800', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (13, N'Roundabout', N'Link', N'ITN', N'Roundabout', N'CycleAttribute.Roundabout', N'0x00001000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (14, N'Enclosed Traffic Area Link', N'Link', N'ITN', N'Type', N'CycleAttribute.EnclosedTrafficAreaLink', N'0x00002000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (15, N'Traffic Island Link At Junction', N'Link', N'ITN', N'Type', N'CycleAttribute.TrafficIslandLinkAtJunction', N'0x00004000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (16, N'Traffic Island Link', N'Link', N'ITN', N'Type', N'CycleAttribute.TrafficIslandLink', N'0x00008000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (17, N'Ferry', N'Link', N'ITN', N'Type', N'CycleAttribute.Ferry', N'0x00010000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (18, N'Limited Access', N'Link', N'ITN', N'Type', N'CycleAttribute.LimitedAccess', N'0x00020000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (19, N'Prohibited Access', N'Link', N'ITN', N'Type', N'CycleAttribute.ProhibitedAccess', N'0x00040000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (20, N'Footpath', N'Link', N'ITN', N'Type', N'CycleAttribute.Footpath', N'0x00080000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (21, N'Cyclepath', N'Link', N'ITN', N'Type', N'CycleAttribute.Cyclepath', N'0x00100000', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (22, N'Bridlepath', N'Link', N'ITN', N'Type', N'CycleAttribute.Bridlepath', N'0x00200000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (23, N'Can drive A to B', N'Link', N'ITN', N'Type', N'CycleAttribute.CandriveAtoB', N'0x00400000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (24, N'Can drive B to A', N'Link', N'ITN', N'Type', N'CycleAttribute.CandriveBtoA', N'0x00800000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (25, N'Can enter at A i.e. no entry at B', N'Link', N'ITN', N'Type', N'CycleAttribute.CanenteratAi.e.noentryatB', N'0x01000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (26, N'Can enter at B i.e. no entry at A', N'Link', N'ITN', N'Type', N'CycleAttribute.CanenteratBi.e.noentryatA', N'0x02000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (27, N'Superlink', N'Link', N'ITN', N'Type', N'CycleAttribute.Superlink', N'0x04000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (28, N'Trunk Road', N'Link', N'ITN', N'Type', N'CycleAttribute.TrunkRoad', N'0x08000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (29, N'Turn Superlink', N'Link', N'ITN', N'Type', N'CycleAttribute.TurnSuperlink', N'0x10000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (30, N'Connecting Link', N'Link', N'ITN', N'Type', N'CycleAttribute.ConnectingLink', N'0x20000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (31, N'Towpath', N'Link', N'ITN', N'Type', N'CycleAttribute.Towpath', N'0x40000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (32, N'Gradient 2 - A to B uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient2-AtoBuphill', N'0x00000001', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (33, N'Gradient 2 - B to A uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient2-BtoAuphill', N'0x00000002', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (34, N'Gradient 3 - A to B uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient3-AtoBuphill', N'0x00000004', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (35, N'Gradient 3 - B to A uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient3-BtoAuphill', N'0x00000008', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (36, N'Gradient 4 - A to B uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient4-AtoBuphill', N'0x00000010', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (37, N'Gradient 4 - B to A uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient4-BtoAuphill', N'0x00000020', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (38, N'Gradient 5 - A to B uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient5-AtoBuphill', N'0x00000040', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (39, N'Gradient 5 - B to A uphill', N'Link', N'User0', N'Characteristic', N'CycleAttribute.Gradient5-BtoAuphill', N'0x00000080', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (40, N'Ford', N'Link', N'User0', N'Barrier', N'CycleAttribute.Ford', N'0x00000100', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (41, N'Gate', N'Link', N'User0', N'Barrier', N'CycleAttribute.Gate', N'0x00000200', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (42, N'Level Crossing', N'Link', N'User0', N'Barrier', N'CycleAttribute.LevelCrossing', N'0x00000400', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (43, N'Bridge', N'Link', N'User0', N'Barrier', N'CycleAttribute.Bridge', N'0x00000800', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (44, N'Tunnel', N'Link', N'User0', N'Barrier', N'CycleAttribute.Tunnel', N'0x00001000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (45, N'Calming Unavoidable by Bike', N'Link', N'User0', N'Barrier', N'CycleAttribute.CalmingUnavoidablebyBike', N'0x00002000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (46, N'Footbridge', N'Link', N'User0', N'Barrier', N'CycleAttribute.Footbridge', N'0x00004000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (47, N'Unused', N'Link', N'User0', N'None', N'CycleAttribute.Unused', N'0x00008000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (48, N'Shared Use Footpath', N'Link', N'User0', N'Type', N'CycleAttribute.SharedUseFootpath', N'0x00010000', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (49, N'Footpath Only', N'Link', N'User0', N'Type', N'CycleAttribute.FootpathOnly', N'0x00020000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (50, N'Cycles Only', N'Link', N'User0', N'Type', N'CycleAttribute.CyclesOnly', N'0x00040000', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (51, N'Private Access', N'Link', N'User0', N'Characteristic', N'CycleAttribute.PrivateAccess', N'0x00080000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (52, N'Unused', N'Link', N'User0', N'None', N'CycleAttribute.Unused', N'0x00100000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (53, N'Parkland', N'Link', N'User0', N'Barrier', N'CycleAttribute.Parkland', N'0x00200000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (54, N'Subway', N'Link', N'User0', N'Barrier', N'CycleAttribute.Subway', N'0x00400000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (55, N'Raisable Barrier', N'Link', N'User0', N'Barrier', N'CycleAttribute.RaisableBarrier', N'0x00800000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (56, N'Hoop Lift Barrier', N'Link', N'User0', N'Barrier', N'CycleAttribute.HoopLiftBarrier', N'0x01000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (57, N'Cattle Grid', N'Link', N'User0', N'Barrier', N'CycleAttribute.CattleGrid', N'0x02000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (58, N'Stile', N'Link', N'User0', N'Barrier', N'CycleAttribute.Stile', N'0x04000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (59, N'Hoop Through Barrier', N'Link', N'User0', N'Barrier', N'CycleAttribute.HoopThroughBarrier', N'0x08000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (60, N'Humps', N'Link', N'User0', N'Barrier', N'CycleAttribute.Humps', N'0x10000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (61, N'Cushions', N'Link', N'User0', N'Barrier', N'CycleAttribute.Cushions', N'0x20000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (62, N'Chicane', N'Link', N'User0', N'Barrier', N'CycleAttribute.Chicane', N'0x40000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (63, N'Pinch Point', N'Link', N'User0', N'Barrier', N'CycleAttribute.PinchPoint', N'0x80000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (64, N'Pelican', N'Link', N'User1', N'Crossing', N'CycleAttribute.Pelican', N'0x00000001', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (65, N'Toucan', N'Link', N'User1', N'Crossing', N'CycleAttribute.Toucan', N'0x00000002', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (66, N'Zebra', N'Link', N'User1', N'Crossing', N'CycleAttribute.Zebra', N'0x00000004', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (67, N'Walkabout Manoeuvre', N'Stopover', N'User1', N'Manoeuvrability', N'CycleAttribute.WalkaboutManoeuvre', N'0x00000008', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (68, N'Advanced Manoeuvre', N'Stopover', N'User1', N'Manoeuvrability', N'CycleAttribute.AdvancedManoeuvre', N'0x00000010', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (69, N'Prohibited Manoeuvre', N'Stopover', N'User1', N'Manoeuvrability', N'CycleAttribute.ProhibitedManoeuvre', N'0x00000020', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (70, N'Cycle Lane A to B', N'Link', N'User1', N'Type', N'CycleAttribute.CycleLaneAtoB', N'0x00000040', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (71, N'Cycle Lane B to A', N'Link', N'User1', N'Type', N'CycleAttribute.CycleLaneBtoA', N'0x00000080', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (72, N'Bus Lane A to B', N'Link', N'User1', N'Type', N'CycleAttribute.BusLaneAtoB', N'0x00000100', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (73, N'Bus Lane B to A', N'Link', N'User1', N'Type', N'CycleAttribute.BusLaneBtoA', N'0x00000200', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (74, N'Narrow A to B', N'Link', N'User1', N'Type', N'CycleAttribute.NarrowAtoB', N'0x00000400', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (75, N'Narrow B to A', N'Link', N'User1', N'Type', N'CycleAttribute.NarrowBtoA', N'0x00000800', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (76, N'Dedicated A to B', N'Link', N'User1', N'Type', N'CycleAttribute.DedicatedAtoB', N'0x00001000', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (77, N'Dedicated B to A', N'Link', N'User1', N'Type', N'CycleAttribute.DedicatedBtoA', N'0x00002000', 1, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (78, N'Unpaved A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.UnpavedAtoB', N'0x00004000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (79, N'Unpaved B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.UnpavedBtoA', N'0x00008000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (80, N'When Dry A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.WhenDryAtoB', N'0x00010000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (81, N'When Dry B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.WhenDryBtoA', N'0x00020000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (82, N'Firm A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.FirmAtoB', N'0x00040000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (83, N'Firm B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.FirmBtoA', N'0x00080000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (84, N'Paved A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.PavedAtoB', N'0x00100000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (85, N'Paved B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.PavedBtoA', N'0x00200000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (86, N'Loose A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.LooseAtoB', N'0x00400000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (87, N'Loose B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.LooseBtoA', N'0x00800000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (88, N'Cobbles A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.CobblesAtoB', N'0x01000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (89, N'Cobbles B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.CobblesBtoA', N'0x02000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (90, N'Mixed A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.MixedAtoB', N'0x04000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (91, N'Mixed B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.MixedBtoA', N'0x08000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (92, N'Rough A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.RoughAtoB', N'0x10000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (93, N'Rough B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.RoughBtoA', N'0x20000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (94, N'Blocks A to B', N'Link', N'User1', N'Characteristic', N'CycleAttribute.BlocksAtoB', N'0x40000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (95, N'Blocks B to A', N'Link', N'User1', N'Characteristic', N'CycleAttribute.BlocksBtoA', N'0x80000000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (96, N'Individual Recommendation', N'Link', N'User2', N'None', N'CycleAttribute.IndividualRecommendation', N'0x00000001', 0, 1, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (97, N'LA Recommended', N'Link', N'User2', N'None', N'CycleAttribute.LARecommended', N'0x00000002', 0, 1, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (98, N'Recommended For Schools', N'Link', N'User2', N'None', N'CycleAttribute.RecommendedForSchools', N'0x00000004', 0, 1, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (99, N'Other Recommendation', N'Link', N'User2', N'None', N'CycleAttribute.OtherRecommendation', N'0x00000008', 0, 1, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (100, N'Well Lit', N'Link', N'User2', N'Characteristic', N'CycleAttribute.WellLit', N'0x00000010', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (101, N'Lighting Present', N'Link', N'User2', N'Characteristic', N'CycleAttribute.LightingPresent', N'0x00000020', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (102, N'Partial Lighting', N'Link', N'User2', N'Characteristic', N'CycleAttribute.PartialLighting', N'0x00000040', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (103, N'No lighting', N'Link', N'User2', N'Characteristic', N'CycleAttribute.Nolighting', N'0x00000080', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (104, N'Busy', N'Link', N'User2', N'Characteristic', N'CycleAttribute.Busy', N'0x00000100', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (105, N'Very', N'Link', N'User2', N'Characteristic', N'CycleAttribute.Very', N'0x00000200', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (106, N'Quiet', N'Link', N'User2', N'Characteristic', N'CycleAttribute.Quiet', N'0x00000400', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (107, N'Traffic Free', N'Link', N'User2', N'Characteristic', N'CycleAttribute.TrafficFree', N'0x00000800', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (108, N'Seldom Policed Urban Area', N'Link', N'User2', N'Characteristic', N'CycleAttribute.SeldomPolicedUrbanArea', N'0x00001000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (109, N'Isolated Area', N'Link', N'User2', N'Characteristic', N'CycleAttribute.IsolatedArea', N'0x00002000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (110, N'Neighbourhood Watch', N'Link', N'User2', N'Characteristic', N'CycleAttribute.NeighbourhoodWatch', N'0x00004000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (111, N'Cctv Monitored Area', N'Link', N'User2', N'Characteristic', N'CycleAttribute.Cctv MonitoredArea', N'0x00008000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (112, N'Normally Safe In Daylight', N'Link', N'User2', N'Characteristic', N'CycleAttribute.NormallySafeInDaylight', N'0x00010000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (113, N'Normally Safe At Night', N'Link', N'User2', N'Characteristic', N'CycleAttribute.NormallySafeAtNight', N'0x00020000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (114, N'Incidents Have Occured In Area', N'Link', N'User2', N'Characteristic', N'CycleAttribute.IncidentsHave OccuredInArea', N'0x00040000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (115, N'Frequently Policed Urban Area', N'Link', N'User2', N'Characteristic', N'CycleAttribute.FrequentlyPolicedUrbanArea', N'0x00080000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (116, N'Steps', N'Link', N'User2', N'Type', N'CycleAttribute.Steps', N'0x00100000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (117, N'Channel alongside steps', N'Link', N'User2', N'Type', N'CycleAttribute.Channelalongsidesteps', N'0x00200000', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (118, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x00400000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (119, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x00800000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (120, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x01000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (121, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x02000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (122, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x04000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (123, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x08000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (124, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x10000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (125, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x20000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (126, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x40000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (127, N'Unused', N'Link', N'User2', N'None', N'CycleAttribute.Unused', N'0x80000000', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (128, N'Turn Restriction', N'Node', N'ITN', N'Type', N'CycleAttribute.TurnRestriction', N'0x00000001', 0, 0, 0)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (129, N'Mini Roundabout', N'Node', N'ITN', N'Roundabout', N'CycleAttribute.MiniRoundabout', N'0x00000002', 0, 0, 1)
INSERT INTO [dbo].[CycleAttribute] ([CycleAttributeId], [Description], [Type], [Group], [Category], [ResourceName], [Mask], [CycleInfrastructure], [CycleRecommendedRoute], [ShowAttribute]) VALUES (130, N'Through Route', N'Node', N'ITN', N'None', N'CycleAttribute.ThroughRoute', N'0x00000004', 0, 0, 0)
COMMIT TRANSACTION

-- =============================================
-- Inserts TravelNewsDataSources
-- =============================================

-- **********************************************************************
-- IMPORTANT
-- The Insert statements are copied from TDP script MDS014_TraveNewsDataSources.sql
-- These should be copied from MDS014
-- **********************************************************************

USE [SJPTransientPortal]
GO

BEGIN TRANSACTION

-- Clear existing datasources so this script retains complete control 
DELETE [dbo].[TravelNewsDataSources]

INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (2)', 'Government Agency (2)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (5)', 'Eyewitness (5)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (6)', 'Eyewitness (6)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (7)', 'Eyewitness (7)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (10)', 'Operator (10)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (11)', 'Operator (11)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (12)', 'Emergency Services (12)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (13)', 'Local Authority (13)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (14)', 'Operator (14)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Camera (15)', 'Camera (15)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Camera (16)', 'Camera (16)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (17)', 'Emergency Services (17)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (19)', 'Local Authority (19)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Utility Company (22)', 'Utility Company (22)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (23)', 'Government Agency (23)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (25)', 'Operator (25)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (26)', 'Emergency Services (26)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (27)', 'Government Agency (27)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (28)', 'Eyewitness (28)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (31)', 'Local Authority (31)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (34)', 'Government Agency (34)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (36)', 'Operator (36)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (38)', 'Eyewitness (38)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (39)', 'Eyewitness (39)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (40)', 'Emergency Services (40)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (45)', 'Operator (45)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (47)', 'Government Agency (47)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (50)', 'Eyewitness (50)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (57)', 'Operator (57)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (58)', 'Operator (58)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (59)', 'Operator (59)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (60)', 'Operator (60)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (62)', 'Operator (62)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Utility Company (63)', 'Utility Company (63)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (64)', 'Eyewitness (64)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Operator (66)', 'Operator (66)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (67)', 'Local Authority (67)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (68)', 'Eyewitness (68)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Sensor (69)', 'Sensor (69)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (70)', 'Local Authority (70)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (71)', 'Government Agency (71)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (72)', 'Government Agency (72)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (73)', 'Government Agency (73)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (74)', 'Government Agency (74)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (75)', 'Eyewitness (75)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Camera (76)', 'Camera (76)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Sensor (77)', 'Sensor (77)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (78)', 'Eyewitness (78)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (79)', 'Eyewitness (79)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Sensor (80)', 'Sensor (80)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (81)', 'Eyewitness (81)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (82)', 'Eyewitness (82)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (82)', 'Government Agency (82)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (83)', 'Eyewitness (83)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (86)', 'Government Agency (86)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (88)', 'Government Agency (88)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (90)', 'Local Authority (90)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Emergency Services (92)', 'Emergency Services (92)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (93)', 'Government Agency (93)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Local Authority (94)', 'Local Authority (94)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (95)', 'Government Agency (95)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (96)', 'Government Agency (96)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (97)', 'Government Agency (97)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (98)', 'Government Agency (98)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (100)', 'Eyewitness (100)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (102)', 'Government Agency (102)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Government Agency (103)', 'Government Agency (103)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Eyewitness (108)', 'Eyewitness (108)', 1)
INSERT INTO dbo.TravelNewsDataSources (DataSourceId, DataSourceName, Trusted) VALUES ('Bus Company (109)', 'Bus Company (109)', 1)

COMMIT TRANSACTION

-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


-- TravelNewsSeverity
IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsSeverity])
BEGIN
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(0, 'Critical')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(1, 'Serious')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(2, 'Very Severe')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(3, 'Severe')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(4, 'Medium')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(5, 'Slight')
	INSERT INTO [dbo].[TravelNewsSeverity] VALUES(6, 'Very Slight')
END

---------------------------------------------------
-- Add a "no news available" item if no data exists
---------------------------------------------------
-- TravelNews 
IF NOT EXISTS (SELECT * FROM [dbo].[TravelNews])
BEGIN
	INSERT INTO [dbo].[TravelNews] ([UID], [SeverityLevel], [SeverityLevelOlympic], 
		[PublicTransportOperator], [ModeOfTransport], [Regions], [Location], [IncidentType], 
		[HeadlineText], [DetailText], [TravelAdviceOlympicText],
		[IncidentStatus], [Easting], [Northing], [ReportedDateTime], [StartDateTime], [LastModifiedDateTime],
		[ClearedDateTime], [ExpiryDateTime], [PlannedIncident], 
		[RoadType], [IncidentParent], [CarriagewayDirection], [RoadNumber],
		[DayMask], [DailyStartTime], [DailyEndTime], [ItemChangeStatus])
    VALUES
           ('RTM999980', 2, 2, 'N/A', 'Road', 'London', ' ', 'Incidents',
		   'We are unable to bring you Live Travel News at the moment. Please try later.', 
		   'We are unable to bring you Live Travel News at the moment. Please try later.',
		   'We are unable to bring you Live Travel News at the moment. Please try later.',
		   'O',0,0,'2011-05-01 00:00:00.000','2011-05-01 00:00:00.000','2011-05-01 00:00:00.000',
		   NULL,'2012-05-01 00:00:00.000',1,
		   '',NULL,NULL,NULL,
		   NULL,NULL,NULL,NULL)

	-- TravelNewsDataSources
	IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsDataSources] WHERE [DataSourceId] = 'DataSource1')
	BEGIN
		INSERT INTO [dbo].[TravelNewsDataSources] ([DataSourceId], [DataSourceName], [Trusted])
		VALUES ('DataSource1', 'DataSource1', 1)
	END

	-- TravelNewsDataSource
	IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsDataSource] WHERE [UID] = 'RTM999980')
	BEGIN
		INSERT INTO [dbo].[TravelNewsDataSource] ([UID], [DataSourceId])
		VALUES ('RTM999980', 'DataSource1')
	END

	-- TravelNewsRegion
	IF NOT EXISTS (SELECT * FROM [dbo].[TravelNewsRegion] WHERE [UID] = 'RTM999980')
	BEGIN
		INSERT INTO [dbo].[TravelNewsRegion] ([UID], [RegionName])
		VALUES ('RTM999980', 'London')
	END

END

GO

-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go

BEGIN TRANSACTION
DELETE FROM dbo.[AdminAreas]


BULK INSERT dbo.[AdminAreas] FROM '$(RemotePath)AdminAreas.csv' WITH
(FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)AdminAreas.fmt') -- Using Header row

COMMIT TRANSACTION

-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


BEGIN TRANSACTION
DELETE FROM dbo.[Districts]


BULK INSERT dbo.[Districts] FROM '$(RemotePath)Districts.csv' WITH
(FIELDTERMINATOR = ',' ,
FIRSTROW = 2,
FormatFile = '$(RemotePath)Districts.fmt') -- Using Header row

COMMIT TRANSACTION

-- =============================================
-- Script Template
-- =============================================

USE SJPTransientPortal
Go


DELETE [VersionInfo]
GO
INSERT INTO [VersionInfo] ([DatabaseVersionInfo])
     VALUES ('Build175')
GO

