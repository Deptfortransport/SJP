ALTER TABLE [dbo].[SJPPierVenueNavigationPath]
    ADD CONSTRAINT [FK_SJPPierVenueNavigationPath_SJPVenueAdditionalData] FOREIGN KEY ([VenueNaPTAN]) REFERENCES [dbo].[SJPVenueAdditionalData] ([VenueNaPTAN]) ON DELETE NO ACTION ON UPDATE NO ACTION;

