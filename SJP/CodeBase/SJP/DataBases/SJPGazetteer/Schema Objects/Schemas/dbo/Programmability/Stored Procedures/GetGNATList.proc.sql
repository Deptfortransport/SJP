CREATE PROCEDURE [dbo].[GetGNATList]
AS
BEGIN
	SELECT StopNaptan
		,StopName
		,WheelchairAccess
		,AssistanceService
		,StopOperator
		,StopCountry
		,AdministrativeAreaCode
		,NPTGDistrictCode
		,StopType
	FROM dbo.SJPGNATLocations
END