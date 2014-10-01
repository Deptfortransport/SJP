ALTER TABLE [dbo].[SJPFileMonitoringResults]
    ADD CONSTRAINT [FK_SJPFileMonitoringResults_FileMonitoringItems] FOREIGN KEY ([MonitoringItemID]) REFERENCES [dbo].[FileMonitoringItems] ([ItemID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

