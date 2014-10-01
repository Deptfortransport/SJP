CREATE TABLE [dbo].[SJPNonPostcodeLocations]
(
	[ID]            INT           IDENTITY (1, 1) NOT NULL,
    [DATASETID]     VARCHAR (50)  NOT NULL,
    [ParentID]      VARCHAR (50)  NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [DisplayName]   VARCHAR (100) NOT NULL,
    [Type]          VARCHAR (20)  NOT NULL,
    [Naptan]        VARCHAR (20)  NULL,
    [LocalityID]    VARCHAR (20)  NULL,
    [Easting]       FLOAT         NOT NULL,
    [Northing]      FLOAT         NOT NULL,
    [NearestTOID]   VARCHAR (50)  NULL,
    [NearestPointE] FLOAT         NULL,
    [NearestPointN] FLOAT         NULL,
	[AdminAreaID]   INT           NULL,
	[DistrictID]    INT           NULL
)
