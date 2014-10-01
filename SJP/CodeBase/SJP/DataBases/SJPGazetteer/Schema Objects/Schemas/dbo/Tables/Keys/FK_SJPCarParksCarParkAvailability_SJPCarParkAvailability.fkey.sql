ALTER TABLE [dbo].[SJPCarParksCarParkAvailability]
    ADD CONSTRAINT [FK_SJPCarParksCarParkAvailability_SJPCarParkAvailability] FOREIGN KEY ([AvailabilityID]) REFERENCES [dbo].[SJPCarParkAvailability] ([AvailabilityID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

