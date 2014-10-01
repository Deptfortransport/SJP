ALTER TABLE [dbo].[ExposedServicesEvent]
    ADD CONSTRAINT [DF_ExposedServicesEvent_TimeLogged] DEFAULT (getdate()) FOR [TimeLogged];

