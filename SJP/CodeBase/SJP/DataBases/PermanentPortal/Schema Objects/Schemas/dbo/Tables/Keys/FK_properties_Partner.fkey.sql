ALTER TABLE [dbo].[properties]
    ADD CONSTRAINT [FK_properties_Partner] FOREIGN KEY ([PartnerId]) REFERENCES [dbo].[Partner] ([PartnerId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

