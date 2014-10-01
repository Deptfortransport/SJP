
CREATE  PROCEDURE [dbo].[ImportSJPGNATLocationsData]
(
	@XML text
)
AS
/*
SP Name: ImportSJPGNATLocations
Input : XML
Output: None
Description:  It takes XML data as string and inserts the data into SJP GNAT Locations.
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(100)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  '/GNATLocationsData/GNATLocation'

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJPGNAT Locations table
	DELETE FROM dbo.SJPGNATLocations

	-- Insert GNAT locations data
	INSERT INTO dbo.SJPGNATLocations(
			[StopNaptan]
           ,[StopName] 
           ,[StopAreaNaPTAN]
           ,[StopOperator]
           ,[WheelchairAccess]
           ,[AssistanceService]
           ,[WEFDate]
           ,[WEUDate]
           ,[MOFRStartTime]
           ,[MOFREndTime]
           ,[SatStartTime]
           ,[SatEndTime]
           ,[SunStartTime]
           ,[SunEndTime]
		   ,[StopCountry]
		   ,[AdministrativeAreaCode]
		   ,[NPTGDistrictCode]
		   ,[StopType])
	SELECT
			X.[StopNaPTAN]
           ,X.[StopName] 
           ,X.[StopAreaNaPTANType]
           ,X.[StopOperator]
           ,CASE WHEN X.[WheelchairAccess] = 'Yes' THEN 1 ELSE 0 END
           ,CASE WHEN X.[AssistanceService] = 'Yes' THEN 1 ELSE 0 END
           ,CAST ((X.[WEFDate]) AS DATE)
           ,CAST ((X.[WEUDate]) AS DATE)
           ,CAST ((X.[MOFRStartTime]) AS TIME)
           ,CAST ((X.[MOFREndTime]) AS TIME)
           ,CAST ((X.[SatStartTime]) AS TIME)
           ,CAST ((X.[SatEndTime]) AS TIME)
           ,CAST ((X.[SunStartTime]) AS TIME)
           ,CAST ((X.[SunEndTime]) AS TIME)
		   ,CASE x.[Country] WHEN 'Wales' THEN 'Wal'
				WHEN 'Scotland' THEN 'Sco'
				ELSE 'Eng'
			END
		   ,x.[AdministrativeAreaCode]
		   ,x.[NptgDistrictCode]
		   ,dbo.GetGNATStopType(X.[StopNaPTAN],X.[StopOperator])
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		   [StopNaPTAN] [nvarchar](20),
           [StopName] [nvarchar](150),
           [StopAreaNaPTANType] [nvarchar](20),
           [StopOperator] [nvarchar](4),
           [WheelchairAccess] [nvarchar](4),
           [AssistanceService] [nvarchar](4),
           [WEFDate] [nvarchar](6),
           [WEUDate] [nvarchar](6),
           [MOFRStartTime] [nvarchar](6),
           [MOFREndTime] [nvarchar](6),
           [SatStartTime] [nvarchar](6),
           [SatEndTime] [nvarchar](6),
           [SunStartTime] [nvarchar](6),
           [SunEndTime] [nvarchar](6),
		   [Country] [nvarchar](20),
		   [AdministrativeAreaCode] int,
		   [NptgDistrictCode] int
	) X 

COMMIT TRANSACTION 

-- Removing xml doc from memorry
EXEC sp_xml_removedocument @DocID