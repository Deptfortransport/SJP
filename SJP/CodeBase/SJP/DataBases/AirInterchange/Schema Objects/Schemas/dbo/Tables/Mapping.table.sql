CREATE TABLE [dbo].[Mapping] (
    [ID]            INT          NOT NULL,
    [InterchangeID] INT          NOT NULL,
    [Precision]     VARCHAR (12) COLLATE Latin1_General_CI_AS NULL,
    [GeoCodeID]     VARCHAR (20) COLLATE Latin1_General_CI_AS NOT NULL,
    [GridType]      VARCHAR (12) COLLATE Latin1_General_CI_AS NULL,
    [Easting]       INT          NOT NULL,
    [Northing]      INT          NOT NULL,
    [Order]         INT          NOT NULL
);

