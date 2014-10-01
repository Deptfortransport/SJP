CREATE PROCEDURE [dbo].[GetGNATAdminAreas]
AS
BEGIN
	SELECT 
		 [AdministrativeAreaCode]
		,[DistrictCode]
		,[StepFree]
		,[Assistance]
	FROM [dbo].[SJPGNATAdminAreas]
END