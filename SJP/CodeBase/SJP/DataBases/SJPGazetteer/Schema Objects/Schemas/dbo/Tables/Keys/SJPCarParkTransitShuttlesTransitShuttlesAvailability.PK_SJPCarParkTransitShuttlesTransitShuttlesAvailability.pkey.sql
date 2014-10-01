ALTER TABLE [dbo].[SJPCarParkTransitShuttlesTransitShuttlesAvailability]
	ADD CONSTRAINT [PK_SJPCarParkTransitShuttlesTransitShuttlesAvailability]
	PRIMARY KEY ([TransitShuttleID],[TransitShuttleAvailabilityID]) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
