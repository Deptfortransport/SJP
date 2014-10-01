ALTER TABLE [dbo].[SJPRiverServices]
    ADD CONSTRAINT [FK_SJPRiverServices_SJPVenueAdditionalData] FOREIGN KEY ([VenueNaPTAN]) REFERENCES [dbo].[SJPVenueAdditionalData] ([VenueNaPTAN]) ON DELETE NO ACTION ON UPDATE NO ACTION;

