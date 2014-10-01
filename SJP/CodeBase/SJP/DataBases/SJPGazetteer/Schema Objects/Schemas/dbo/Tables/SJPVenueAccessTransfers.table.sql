CREATE TABLE [dbo].[SJPVenueAccessTransfers]
(
	[VenueNaPTAN]			NVARCHAR (20)   NOT NULL,
	[StationNaPTAN]			NVARCHAR (20)   NOT NULL,
    [ToVenue]               BIT             NOT NULL,
	[CultureCode]			CHAR (2)		NOT NULL DEFAULT('en'),
	[TransferDescription]   NVARCHAR (MAX)  NOT NULL

CONSTRAINT [pk_SJPVenueAccessTransfers] PRIMARY KEY CLUSTERED 
(
	[VenueNaPTAN] ASC,
	[StationNaPTAN] ASC,
	[ToVenue] ASC,
	[CultureCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
