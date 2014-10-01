ALTER TABLE [dbo].[Accessibility]
    ADD CONSTRAINT [FK_Accessibility_Link] FOREIGN KEY ([LinkID]) REFERENCES [dbo].[Link] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

