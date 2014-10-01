CREATE PROCEDURE [dbo].[GetWMIMonitoringItems]
AS
BEGIN
SELECT [ItemID]
      ,[CheckInterval]
      ,[Enabled]
      ,[Description]
      ,[WQLQuery]
      ,[RedCondition]
  FROM .[WMIMonitoringItems]
END