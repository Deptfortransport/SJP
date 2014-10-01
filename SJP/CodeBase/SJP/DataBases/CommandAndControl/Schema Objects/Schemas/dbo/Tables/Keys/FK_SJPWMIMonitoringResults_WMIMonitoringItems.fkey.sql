ALTER TABLE [dbo].[SJPWMIMonitoringResults]
    ADD CONSTRAINT [FK_SJPWMIMonitoringResults_WMIMonitoringItems] FOREIGN KEY ([MonitoringItemID]) REFERENCES [dbo].[WMIMonitoringItems] ([ItemID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

