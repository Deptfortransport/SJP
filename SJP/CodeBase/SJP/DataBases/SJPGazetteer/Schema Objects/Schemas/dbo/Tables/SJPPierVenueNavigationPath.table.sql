CREATE TABLE [dbo].[SJPPierVenueNavigationPath] (
    [NavigationID]       NVARCHAR (50)  NOT NULL,
    [DefaultDuration]    TIME (0)       NOT NULL,
    [Distance]           INT            NOT NULL,
    [ToNaPTAN]           NVARCHAR (20)  NOT NULL,
    [FromNaPTAN]         NVARCHAR (20)  NOT NULL,
    [VenueNaPTAN]        NVARCHAR (20)  NOT NULL,
	[ToVenue]			 BIT			NOT NULL
);





