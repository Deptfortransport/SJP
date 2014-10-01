CREATE PROCEDURE [dbo].[GetChecksumMonitoringItems]
AS
BEGIN
SELECT [ItemID]
      ,[ChecksumRootPath]
      ,[CheckInterval]
      ,[Enabled]
      ,[Description]
	  ,[ExtensionsToIgnore]
      ,[RedCondition]
  FROM [dbo].[ChecksumMonitoringItems]
END