
USE [SJPGazetteer]


BEGIN
-- Delete Temp table so we start cleanly
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPNonPostcodeLocations') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_SJPNonPostcodeLocations]
	
END


-- Copy SJPNonPostcodeLocations data in to new temp tables
BEGIN
	SELECT * INTO [dbo].[temp_SJPNonPostcodeLocations] FROM [SJPNonPostcodeLocations]
	
END



-- Delete the existing SJPNonPostcodeLocations
BEGIN
	TRUNCATE TABLE [SJPNonPostcodeLocations]
END
