CREATE TABLE [dbo].[SJPVenueAdditionalData] (
    [VenueNaPTAN]                 NVARCHAR (20)  NOT NULL,
    [UseNaPTANforJourneyPlanning] BIT            NOT NULL,
    [CycleToVenueDistance]        INT            NOT NULL,
    [VenueMapURL]                 NVARCHAR (300) NULL,
    [VenueWalkingRoutesMapURL]    NVARCHAR (300) NULL,
    [VenueTravelNewsRegion]       NVARCHAR (50)  NOT NULL,
    [AccesibleJourneyToVenue]     BIT            NOT NULL,
	[VenueRiverServiceAvailable]  NVARCHAR (10)  NOT NULL,
	[VenueGroupId]                NVARCHAR (30)  NOT NULL,
	[VenueGroupName]              NVARCHAR (100)  NOT NULL,
);



