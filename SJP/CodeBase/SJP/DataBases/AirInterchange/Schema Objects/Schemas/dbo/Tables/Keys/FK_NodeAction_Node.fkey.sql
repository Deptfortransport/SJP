﻿ALTER TABLE [dbo].[NodeAction]
    ADD CONSTRAINT [FK_NodeAction_Node] FOREIGN KEY ([LegNodeID]) REFERENCES [dbo].[LegNode] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

