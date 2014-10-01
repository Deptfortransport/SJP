CREATE  PROCEDURE [dbo].[ImportSJPParkAndRideToids]
(
	@XML text
)
AS
/*
SP Name: ImportSJPParkAndRideToids
Input : XML string as text
Output: None
Description:  It takes xml data as string and inserts the data into SJP park and ride location tables.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(100)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML

-- setting the node 
SET @XMLPathData =  '/SJPParkAndRideToidList/SJPParkAndRideToids'

-- starting transaction 
BEGIN TRANSACTION

	DELETE FROM dbo.SJPCarParkToids
	
	-- Update car parks
	INSERT INTO dbo.SJPCarParkToids(
			[ParkAndRideID]
           ,[ToTOID] 
           ,[FromTOID])
    SELECT
			X.[ParkAndRideID]
           ,X.[ToTOID] 
           ,X.[FromTOID]
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		   [ParkAndRideID] [nvarchar](50),
           [ToTOID] [nvarchar](50),
           [FromTOID] [nvarchar](50)
	) X
	 
COMMIT TRANSACTION 

-- Removing xml doc from memory
EXEC sp_xml_removedocument @DocID