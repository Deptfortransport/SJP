ALTER TABLE [dbo].[StopEventRequestEvent]
    ADD CONSTRAINT [DF_StopEventRequestEvent_Successful] DEFAULT ((0)) FOR [Successful];

