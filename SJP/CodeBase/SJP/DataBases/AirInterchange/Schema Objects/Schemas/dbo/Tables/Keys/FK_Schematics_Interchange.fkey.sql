ALTER TABLE [dbo].[Schematics]
    ADD CONSTRAINT [FK_Schematics_Interchange] FOREIGN KEY ([InterchangeID]) REFERENCES [dbo].[Interchange] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

