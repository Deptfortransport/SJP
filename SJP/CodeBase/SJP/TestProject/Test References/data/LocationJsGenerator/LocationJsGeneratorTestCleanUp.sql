--SJPNonPostcodeLocations Clean up

-- SQL removes all the temporary data from the SJPNonPostcodeLocations table
-- and then moves the actual data back in from the temporary tables

USE [SJPGazetteer]

if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_SJPNonPostcodeLocations') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- delete test data added to SJPNonPostcodeLocations table
		TRUNCATE TABLE [SJPNonPostcodeLocations]
		
		SET IDENTITY_INSERT dbo.[SJPNonPostcodeLocations] ON
		-- insert data in SJPNonPostcodeLocations table
		INSERT INTO [SJPNonPostcodeLocations]
				([ID]
				  ,[DATASETID]
				  ,[ParentID]
				  ,[Name]
				  ,[DisplayName]
				  ,[Type]
				  ,[Naptan]
				  ,[LocalityID]
				  ,[Easting]
				  ,[Northing]
				  ,[NearestTOID]
				  ,[NearestPointE]
				  ,[NearestPointN]
				  ,[AdminAreaID]
				  ,[DistrictID])
			SELECT *
			FROM dbo.[temp_SJPNonPostcodeLocations]

		SET IDENTITY_INSERT dbo.[SJPNonPostcodeLocations] OFF
				
	
		-- and delete the temp tables
		DROP TABLE [temp_SJPNonPostcodeLocations]
		
	END

