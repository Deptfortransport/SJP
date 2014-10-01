CREATE PROCEDURE [dbo].[GetLatestMonitoringResults]
AS

SELECT [SJP_Server],'CHECKSUM' as MonitorType, [MonitoringItemID],
[Description],[CheckTime],[ValueAtCheck],[IsInRed]
FROM [dbo].[SJPChecksumMonitoringResults] A
 WHERE EXISTS(SELECT *
				FROM (SELECT [SJP_Server], [MonitoringItemID], MAX([CheckTime]) AS [MaxCheckTime]
		              FROM [dbo].[SJPChecksumMonitoringResults]
		             GROUP BY [SJP_Server], [MonitoringItemID]) B
		       WHERE B.MaxCheckTime = A.CheckTime AND B.MonitoringItemID = A.MonitoringItemID)

UNION

SELECT [SJP_Server],'DATABASE' as MonitorType, [MonitoringItemID],
[Description],[CheckTime],[ValueAtCheck],[IsInRed]
FROM [dbo].[SJPDatabaseMonitoringResults] A
 WHERE EXISTS(SELECT *
				FROM (SELECT [SJP_Server], [MonitoringItemID], MAX([CheckTime]) AS [MaxCheckTime]
		              FROM [dbo].[SJPDatabaseMonitoringResults]
		             GROUP BY [SJP_Server], [MonitoringItemID]) B
		       WHERE B.MaxCheckTime = A.CheckTime AND B.MonitoringItemID = A.MonitoringItemID)

UNION

SELECT [SJP_Server],'FILE' as MonitorType, [MonitoringItemID],
[Description],[CheckTime],[ValueAtCheck],[IsInRed]
FROM [dbo].[SJPFileMonitoringResults] A
 WHERE EXISTS(SELECT *
				FROM (SELECT [SJP_Server], [MonitoringItemID], MAX([CheckTime]) AS [MaxCheckTime]
		              FROM [dbo].[SJPFileMonitoringResults]
		             GROUP BY [SJP_Server], [MonitoringItemID]) B
		       WHERE B.MaxCheckTime = A.CheckTime AND B.MonitoringItemID = A.MonitoringItemID)

UNION

SELECT [SJP_Server],'WMI' as MonitorType, [MonitoringItemID],
[Description],[CheckTime],[ValueAtCheck],[IsInRed]
FROM [dbo].[SJPWMIMonitoringResults] A
 WHERE EXISTS(SELECT *
				FROM (SELECT [SJP_Server], [MonitoringItemID], MAX([CheckTime]) AS [MaxCheckTime]
		              FROM [dbo].[SJPWMIMonitoringResults]
		             GROUP BY [SJP_Server], [MonitoringItemID]) B
		       WHERE B.MaxCheckTime = A.CheckTime AND B.MonitoringItemID = A.MonitoringItemID)
		       
ORDER BY [SJP_Server] asc,MonitorType asc, [MonitoringItemID] asc

RETURN 0