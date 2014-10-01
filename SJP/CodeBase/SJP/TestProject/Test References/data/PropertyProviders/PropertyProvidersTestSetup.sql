-- DatabasePropertyProvider test Set up

-- SQL moves all existing Property data into temp tables
-- and then deletes all data from the existing tables
-- Creates helper stored procedures

USE [SJPConfiguration]


BEGIN
-- Delete Temp table so we start cleanly
if exists (select * from [dbo].[sysobjects] where id = object_id(N'dbo.temp_Properties') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[temp_Properties]
		
END


-- Copy Property data in to new temp table
BEGIN
	SELECT * INTO [dbo].[temp_Properties] FROM [Properties]
END



-- Delete the existing Properties data
BEGIN
	TRUNCATE TABLE [Properties]
END

--Insert properties

INSERT INTO Properties 
	VALUES ('propertyservice.version','1','<DEFAULT>','<DEFAULT>'),
	('propertyservice.refreshrate','1000','<DEFAULT>','<DEFAULT>'),
	('test.propertyservice.standard.message','hello group','TestApp','')


-- TestGetProperties
EXEC ('CREATE PROCEDURE GetProperty
	@pName varchar(50)
AS
SELECT * FROM PROPERTIES WHERE pName = @pName')


-- TestLoad1
-- Add the same property for a different group (this should have no effect)
EXEC ('CREATE PROCEDURE DatabasePropertyProviderTestLoad1
AS
UPDATE properties SET pvalue=2 WHERE pname = ''propertyservice.version''
INSERT INTO properties VALUES (''test.propertyservice.standard.message'', ''hello new group'', '''', ''TestGroup'')')

-- Test no version property in database
EXEC ('CREATE PROCEDURE DatabasePropertyProviderTestLoad2
AS
DELETE FROM properties WHERE pname = ''propertyservice.version''')



-- TestLoad2
-- Add the same property for a specific application id (should should affect the value read)
EXEC ('CREATE PROCEDURE DatabasePropertyProviderTestLoad3
AS
UPDATE properties SET pvalue=''ab'' WHERE pname = ''propertyservice.version''')




	