CREATE PROCEDURE [dbo].[ImportSJPUndergroundStatusData]
(
	@XML text
)
AS
/*
SP Name: [[ImportSJPUndergroundStatusData]]
Input : XML
Output: None
Description:  It takes XML data as string and inserts the data into [UndergroundStatus]
*/

SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRANSACTION

DECLARE @DocID int
DECLARE @Version varchar(20)
DECLARE @RowCount int
DECLARE @XMLPathData varchar(200)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML

-- Starting trasaction 
BEGIN
	SET @RowCount = 0

	-- Delete existing data
	DELETE FROM [UndergroundStatus]

	SET @XMLPathData =  '/ArrayOfLineStatus/LineStatus'

	INSERT INTO [UndergroundStatus]
	(
		[LineStatusId], 
		[LineStatusDetails],
		[LineId], 
		[LineName], 
		[StatusId], 
		[StatusDescription], 
		[StatusIsActive], 
		[StatusCssClass]
	)
	SELECT DISTINCT
		X.[LineStatusId],
		X.[LineStatusDetails],
		X.[LineID],
		X.[LineName],
		X.[StatusId],
		X.[StatusDescription],
		CASE X.[StatusIsActive] WHEN 'true' THEN 1 ELSE 0 END AS [StatusIsActive],
		X.[StatusCssClass]
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[LineStatusId] varchar(10) '@ID',
		[LineStatusDetails] varchar(300) '@StatusDetails',
		[LineID] varchar(10) './Line/@ID',
		[LineName] varchar(30) './Line/@Name',
		[StatusId] varchar(100) './Status/@ID',
		[StatusDescription] varchar(30) './Status/@Description',
		[StatusIsActive] varchar(5) './Status/@IsActive',
		[StatusCssClass] varchar(30) './Status/@CssClass'
	) X
	
	SET @RowCount = @@ROWCOUNT
		
	EXEC sp_xml_removedocument @DocID
	

	IF @@ERROR<>0
	ROLLBACK TRANSACTION
	ELSE
	BEGIN
		COMMIT TRANSACTION		
		
		UPDATE ChangeNotification
		SET Version = Version + 1
		WHERE [Table] = 'UndergroundStatusImport'
	END
END