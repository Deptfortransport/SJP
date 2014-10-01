ALTER TABLE [dbo].[SJPChecksumMonitoringResults]
    ADD CONSTRAINT [FK_SJPChecksumMonitoringResults_ChecksumMonitoringItems] FOREIGN KEY ([MonitoringItemID]) REFERENCES [dbo].[ChecksumMonitoringItems] ([ItemID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

