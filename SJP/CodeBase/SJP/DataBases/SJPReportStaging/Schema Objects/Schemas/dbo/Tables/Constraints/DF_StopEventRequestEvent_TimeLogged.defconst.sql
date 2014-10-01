ALTER TABLE [dbo].[StopEventRequestEvent]
    ADD CONSTRAINT [DF_StopEventRequestEvent_TimeLogged] DEFAULT (getdate()) FOR [TimeLogged];

