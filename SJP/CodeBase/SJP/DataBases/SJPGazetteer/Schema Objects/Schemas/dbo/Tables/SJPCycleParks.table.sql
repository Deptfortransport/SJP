CREATE TABLE [dbo].[SJPCycleParks] (
    [CycleParkID]        NVARCHAR (50)  NOT NULL,
    [CycleParkName]      NVARCHAR (150) NOT NULL,
    [VenueServed]        NVARCHAR (20)  NOT NULL,
    [CycleParkMapURL]    NVARCHAR (150) NULL,
    [NumberOfSpaces]     INT            NOT NULL,
    [CycleToEasting]     INT            NOT NULL,
    [CycleToNorthing]    INT            NOT NULL,
    [CycleFromEasting]   INT            NOT NULL,
    [CycleFromNorthing]  INT            NOT NULL,
    [StorageType]        NVARCHAR (50)  NULL,
    [WalkToGateDuration] TIME (0)       NULL,
    [VenueEntranceGate]  NVARCHAR (50)  NULL,
	[WalkFromGateDuration] TIME (0)     NULL,
    [VenueExitGate]		 NVARCHAR (50)  NULL
);



