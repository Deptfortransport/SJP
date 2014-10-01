CREATE TABLE [dbo].[SJPCarParkTransitShuttleTransfers]
(
	[TransitShuttleID]      NVARCHAR (50)   NOT NULL,
	[CultureCode]			CHAR (2)		NOT NULL DEFAULT('en'),
	[TransferDescription]   NVARCHAR (MAX)  NOT NULL
)
