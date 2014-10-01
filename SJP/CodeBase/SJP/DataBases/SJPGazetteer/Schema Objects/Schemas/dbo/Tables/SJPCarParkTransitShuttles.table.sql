CREATE TABLE [dbo].[SJPCarParkTransitShuttles] (
    [TransitShuttleID]          NVARCHAR (50)  NOT NULL,
    [ToVenue]                   BIT            NOT NULL,
    [ModeOfTransport]           NVARCHAR (20)  NOT NULL,
    [TransitDuration]           INT            NOT NULL,
    [IsScheduledService]        BIT            NOT NULL,
    [IsPRMOnly]                 BIT            NOT NULL,
    [VenueGateNaPTAN]           NVARCHAR (20)  NOT NULL,
    [ServiceFrequency]          INT            NOT NULL,
    [FirstServiceOfDay]         TIME (0)       NOT NULL,
    [LastServiceOfDay]          TIME (0)       NOT NULL
);









