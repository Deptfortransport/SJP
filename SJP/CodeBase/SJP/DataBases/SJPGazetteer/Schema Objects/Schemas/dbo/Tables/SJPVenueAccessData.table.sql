CREATE TABLE [dbo].[SJPVenueAccessData] (
    [VenueNaPTAN]    NVARCHAR (20)  NOT NULL,
    [VenueName]      NVARCHAR (150) NOT NULL,
    [AccessFrom]     DATETIME       NOT NULL,
    [AccessTo]       DATETIME       NOT NULL,
    [AccessDuration] TIME (0)       NOT NULL,
    [StationNaPTAN]  NVARCHAR (20)  NOT NULL,
    [StationName]    NVARCHAR (150) NOT NULL
);

