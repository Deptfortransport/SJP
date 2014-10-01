ALTER TABLE [dbo].[WorkloadEvent]
    ADD CONSTRAINT [DF_WorkloadEvent] DEFAULT ((0)) FOR [PartnerId];

