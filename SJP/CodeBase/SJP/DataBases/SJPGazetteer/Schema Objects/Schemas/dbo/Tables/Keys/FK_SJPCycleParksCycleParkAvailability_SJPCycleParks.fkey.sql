ALTER TABLE [dbo].[SJPCycleParksCycleParkAvailability]
    ADD CONSTRAINT [FK_SJPCycleParksCycleParkAvailability_SJPCycleParks] FOREIGN KEY ([CycleParkID]) REFERENCES [dbo].[SJPCycleParks] ([CycleParkID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

