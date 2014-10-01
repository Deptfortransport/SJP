ALTER TABLE [dbo].[Mapping]
    ADD CONSTRAINT [FK_Mapping_Interchange] FOREIGN KEY ([InterchangeID]) REFERENCES [dbo].[Interchange] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

