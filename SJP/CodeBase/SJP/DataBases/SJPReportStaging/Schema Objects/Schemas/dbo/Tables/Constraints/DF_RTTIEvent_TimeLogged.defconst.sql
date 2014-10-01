ALTER TABLE [dbo].[RTTIEvent]
    ADD CONSTRAINT [DF_RTTIEvent_TimeLogged] DEFAULT (getdate()) FOR [TimeLogged];

