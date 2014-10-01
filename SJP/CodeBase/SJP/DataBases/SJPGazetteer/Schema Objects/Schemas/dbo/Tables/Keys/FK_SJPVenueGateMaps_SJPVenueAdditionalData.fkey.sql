ALTER TABLE [dbo].[SJPVenueGateMaps]
    ADD CONSTRAINT [FK_SJPVenueGateMaps_SJPVenueAdditionalData] FOREIGN KEY ([VenueNaPTAN]) REFERENCES [dbo].[SJPVenueAdditionalData] ([VenueNaPTAN]) ON DELETE NO ACTION ON UPDATE NO ACTION;

