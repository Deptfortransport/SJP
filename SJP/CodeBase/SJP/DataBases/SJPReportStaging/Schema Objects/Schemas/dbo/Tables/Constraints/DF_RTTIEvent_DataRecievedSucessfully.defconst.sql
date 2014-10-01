ALTER TABLE [dbo].[RTTIEvent]
    ADD CONSTRAINT [DF_RTTIEvent_DataRecievedSucessfully] DEFAULT ((0)) FOR [DataRecievedSucessfully];

