ALTER TABLE [dbo].[RTTIInternalEvent]
    ADD CONSTRAINT [DF_RTTIInternalEvent_TimeLogged] DEFAULT (getdate()) FOR [TimeLogged];

