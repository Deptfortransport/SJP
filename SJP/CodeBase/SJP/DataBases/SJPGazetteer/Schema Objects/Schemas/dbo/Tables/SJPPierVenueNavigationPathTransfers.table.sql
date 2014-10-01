CREATE TABLE [dbo].[SJPPierVenueNavigationPathTransfers]
(
	[NavigationID]			NVARCHAR (50)   NOT NULL,
	[CultureCode]			CHAR (2)		NOT NULL DEFAULT('en'),
	[TransferDescription]   NVARCHAR (MAX)  NOT NULL
)
