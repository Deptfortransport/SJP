-- =============================================
-- Script Template
-- =============================================

Use CommandAndControl
Go


EXEC AddChangeNotificationTable 'ChecksumMonitoringItems'
EXEC AddChangeNotificationTable 'DatabaseMonitoringItems'
EXEC AddChangeNotificationTable 'FileMonitoringItems'
EXEC AddChangeNotificationTable 'WMIMonitoringItems'

GO