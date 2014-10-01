ALTER TABLE [dbo].[LegNode]
    ADD CONSTRAINT [FK_LegNode_Interchange] FOREIGN KEY ([InterchangeID]) REFERENCES [dbo].[Interchange] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

