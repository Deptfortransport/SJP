﻿ALTER TABLE [dbo].[SJPVenueGateMaps]
	ADD CONSTRAINT [PK_SJPVenueGateMaps]
	PRIMARY KEY ([VenueNaPTAN],[VenueGate]) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
