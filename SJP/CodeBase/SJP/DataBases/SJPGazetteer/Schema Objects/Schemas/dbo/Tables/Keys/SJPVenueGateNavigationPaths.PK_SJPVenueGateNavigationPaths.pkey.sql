﻿ALTER TABLE [dbo].[SJPVenueGateNavigationPaths]
	ADD CONSTRAINT [PK_SJPVenueGateNavigationPaths]
	PRIMARY KEY ([NavigationPathId]) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
