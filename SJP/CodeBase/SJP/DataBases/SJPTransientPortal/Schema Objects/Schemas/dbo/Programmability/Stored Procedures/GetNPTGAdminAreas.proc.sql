CREATE PROCEDURE [dbo].[GetNPTGAdminAreas]
AS
	SELECT [AdministrativeAreaCode]
		  ,[AreaName]
		  ,[Country]
		  ,[RegionCode]
	 FROM [dbo].[AdminAreas]
	 WHERE [Country] != 'GRE'
RETURN 0