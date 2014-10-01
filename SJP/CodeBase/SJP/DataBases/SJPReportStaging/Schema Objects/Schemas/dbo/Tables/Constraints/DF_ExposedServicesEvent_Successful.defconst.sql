ALTER TABLE [dbo].[ExposedServicesEvent]
    ADD CONSTRAINT [DF_ExposedServicesEvent_Successful] DEFAULT ((0)) FOR [Successful];

