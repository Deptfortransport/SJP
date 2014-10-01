CREATE PROCEDURE [dbo].[ImportSJPGNATAdminAreasData]
(
	@XML text
)
AS
/*
SP Name: ImportSJPGNATAdminAreasData
Input : XML
Output: None
Description:  It takes XML data as string and inserts the data into SJPGNATAdminAreas.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(100)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  '/GNATAdminAreasData/GNATAdminArea'

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJPGNATAdminAreas table
	DELETE FROM [dbo].[SJPGNATAdminAreas]

	-- Insert GNAT AdminAreas data
	INSERT INTO [dbo].[SJPGNATAdminAreas](
			[AdministrativeAreaCode]
		   ,[DistrictCode]
		   ,[StepFree]
		   ,[Assistance])
	SELECT
			X.[AdministrativeAreaCode]
           ,X.[DistrictCode] 
           ,CASE WHEN X.[StepFree] = 'True' THEN 1 ELSE 0 END
           ,CASE WHEN X.[Assistance] = 'True' THEN 1 ELSE 0 END
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		   [AdministrativeAreaCode] [nvarchar](8),
		   [DistrictCode] [nvarchar](8),
		   [StepFree] [nvarchar](5),
           [Assistance] [nvarchar](5)
	) X 

COMMIT TRANSACTION 

-- Removing xml doc from memorry
EXEC sp_xml_removedocument @DocID