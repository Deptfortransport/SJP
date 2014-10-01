CREATE PROCEDURE [dbo].[GetFileMonitoringItems]
AS
BEGIN
SELECT [ItemID]
      ,[CheckInterval]
      ,[Enabled]
      ,[Description]
      ,[FullFilePath]
      ,[RedCondition]
  FROM [dbo].[FileMonitoringItems]
END