ALTER TABLE [dbo].[SJPDatabaseMonitoringResults]
    ADD CONSTRAINT [FK_SJPDatabaseMonitoringResults_DatabaseMonitoringItems] FOREIGN KEY ([MonitoringItemID]) REFERENCES [dbo].[DatabaseMonitoringItems] ([ItemID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

