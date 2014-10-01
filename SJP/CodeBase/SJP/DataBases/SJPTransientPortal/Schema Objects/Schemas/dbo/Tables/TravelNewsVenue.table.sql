﻿CREATE TABLE [dbo].[TravelNewsVenue]
(
	[VenueNaPTAN] [varchar](25) NOT NULL,
	[UID] [varchar](25) NOT NULL
 CONSTRAINT [PK_TravelNewsVenue] PRIMARY KEY CLUSTERED 
(
	[VenueNaPTAN] ASC,
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]