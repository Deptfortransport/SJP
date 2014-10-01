CREATE PROCEDURE [dbo].[GetDatabaseMonitoringItems]
AS
BEGIN
SELECT [ItemID]
      ,[CheckInterval]
      ,[Enabled]
      ,[Description]
      ,[SQLHelperDatabaseTarget]
      ,[SQLQuery]
      ,[RedCondition]
  FROM [dbo].[DatabaseMonitoringItems]
END