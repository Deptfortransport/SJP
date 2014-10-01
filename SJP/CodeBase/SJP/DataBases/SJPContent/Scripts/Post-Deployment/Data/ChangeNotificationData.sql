-- =============================================
-- Script Template
-- =============================================


USE SJPContent
Go


EXEC AddChangeNotificationTable 'Content'
EXEC AddChangeNotificationTable 'ContentGroup'
EXEC AddChangeNotificationTable 'ContentOverride'

GO