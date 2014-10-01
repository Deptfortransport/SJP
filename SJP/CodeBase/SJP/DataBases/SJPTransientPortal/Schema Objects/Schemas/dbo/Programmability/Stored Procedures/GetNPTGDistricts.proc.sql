CREATE PROCEDURE [dbo].[GetNPTGDistricts]
AS
	SELECT [DistrictCode]
      ,[DistrictName]
      ,[AdministrativeAreaCode]
	FROM [dbo].[Districts]
RETURN 0