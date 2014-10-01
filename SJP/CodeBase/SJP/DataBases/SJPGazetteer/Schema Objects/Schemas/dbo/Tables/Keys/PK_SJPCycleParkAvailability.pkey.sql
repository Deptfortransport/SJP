ALTER TABLE [dbo].[SJPCycleParkAvailability]
    ADD CONSTRAINT [PK_SJPCycleParkAvailability] 
	PRIMARY KEY CLUSTERED ([AvailabilityID],[FromDate],[ToDate],[DailyOpeningTime],[DailyClosingTime] ASC) 
	WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, 
	STATISTICS_NORECOMPUTE = OFF);

