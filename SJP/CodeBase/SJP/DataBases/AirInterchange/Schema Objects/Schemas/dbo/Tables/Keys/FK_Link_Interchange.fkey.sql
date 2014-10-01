ALTER TABLE [dbo].[Link]
    ADD CONSTRAINT [FK_Link_Interchange] FOREIGN KEY ([InterchangeID]) REFERENCES [dbo].[Interchange] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

