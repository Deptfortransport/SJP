CREATE TABLE [dbo].[SJPVenueGates] (
    [EntranceNaPTAN] NVARCHAR (20)  NOT NULL,
    [EntranceName]   NVARCHAR (150) NOT NULL,
    [Easting]        INT            NOT NULL,
    [Northing]       INT            NOT NULL,
    [AvailableFrom]  DATETIME       NULL,
    [AvailableTo]    DATETIME       NULL
);



