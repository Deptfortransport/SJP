CREATE  PROCEDURE [dbo].[ImportSJPStopAccessibilityLinks]
(
	@XML text
)
AS
/*
SP Name: [ImportSJPStopAccessibilityLinks]
Input : XML
Output: None
Description:  It takes XML data as string and inserts the data into StopAccessibilityLinks
*/

SET NOCOUNT ON

DECLARE @DocID int, @XMLPathData varchar(100)

-- Loading xml document 
EXEC sp_xml_preparedocument @DocID OUTPUT, @XML
-- setting the node 
SET @XMLPathData =  '/StopAccessibilityLinks/StopAccessibilityLink'

DECLARE @RowCount int
DECLARE @PrimaryStopNaPTAN NVARCHAR (20)
DECLARE @StopName NVARCHAR (100)
DECLARE @StopOperator char (10)
DECLARE @LinkUrl NVARCHAR (300)
DECLARE @WEFDate DATE
DECLARE @WEUDate DATE
DECLARE @OtherNaPTANs VARCHAR (200)
DECLARE @StopNaPTAN VARCHAR (20)

-- starting trasaction 
BEGIN TRANSACTION

	-- Delete from SJP StopAccessibilityLinks table
	DELETE FROM [dbo].[StopAccessibilityLinks]

	-- Insert SJP StopAccessibilityLinks data
	INSERT INTO [dbo].[StopAccessibilityLinks](
		 [StopNaPTAN]
		,[StopName]
		,[StopOperator]
		,[LinkUrl]
		,[WEFDate]
		,[WEUDate])
	SELECT
			X.[PrimaryStopNaPTAN]
           ,X.[StopName] 
           ,X.[StopOperator]
           ,X.[LinkUrl]
           ,CAST ((X.[WEFDate]) AS DATE)
           ,CAST ((X.[WEUDate]) AS DATE)
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(
		[PrimaryStopNaPTAN] [NVARCHAR] (20),
		[StopName] [NVARCHAR] (100),
		[StopOperator] [char] (10),
		[LinkUrl] [NVARCHAR] (300),
		[WEFDate] [DATE],
		[WEUDate] [DATE]
	) X 
	
	-- Insert the OtherNaPTAN rows containing the same details as its PrimaryStopNaPTAN
	DECLARE otherNaPTANs_cursor CURSOR FOR
	SELECT 
		ISNULL(LTRIM(RTRIM(X.[PrimaryStopNaPTAN])), '') , 
		ISNULL(LTRIM(RTRIM(X.[OtherNaPTAN])), '')
	FROM
	OPENXML (@DocID, @XMLPathData, 2)
	WITH
	(	
		[PrimaryStopNaPTAN] [NVARCHAR] (20),
		[OtherNaPTAN] [NVARCHAR] (200)
	) X

	WHERE LEN(ISNULL(LTRIM(RTRIM(X.[OtherNaPTAN])), '')) >0
	
		SET @RowCount = 0 
		OPEN otherNaPTANs_cursor
		-- Perform the first fetch
		FETCH NEXT FROM otherNaPTANs_cursor 	
				   INTO  @PrimaryStopNaPTAN
						,@OtherNaPTANs
		WHILE @@FETCH_STATUS = 0
		BEGIN
		-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
			
			-- This is executed as long as the previous fetch succeeds.
			SELECT @StopName = [StopName],
				   @StopOperator = [StopOperator], 
				   @LinkUrl = [LinkUrl], 
				   @WEFDate = [WEFDate],
				   @WEUDate = [WEUDate] 
			  FROM [dbo].[StopAccessibilityLinks] 
			 WHERE [StopNaPTAN] = @PrimaryStopNaPTAN
					
			IF LEN(@OtherNaPTANs) > 0 
			BEGIN
					-- Split the naptans and insert the rows
					DECLARE otherNaptan_cursor CURSOR FOR
					SELECT [Value] FROM [dbo].SJPSPlit(@OtherNaPTANs, '|')
					
					OPEN otherNaptan_cursor
				
					FETCH NEXT FROM otherNaptan_cursor 	INTO @StopNaPTAN
					
					WHILE @@FETCH_STATUS = 0
					BEGIN					
						SET @StopNaPTAN = dbo.StripNaptan(@StopNaPTAN, '|')
						
						INSERT INTO [dbo].[StopAccessibilityLinks]([StopNaPTAN],[StopName],[StopOperator],[LinkUrl],[WEFDate],[WEUDate])
						VALUES (@StopNaPTAN, @StopName, @StopOperator, @LinkUrl, @WEFDate, @WEUDate)
						
						FETCH NEXT FROM otherNaptan_cursor 	INTO @StopNaPTAN
					END
					
					CLOSE otherNaptan_cursor
					DEALLOCATE otherNaptan_cursor
	 		
			END 
		
			SET @RowCount = @RowCount + 1
			
			FETCH NEXT FROM otherNaPTANs_cursor
					   INTO  @PrimaryStopNaPTAN
							,@OtherNaPTANs
		END

	CLOSE otherNaPTANs_cursor
	DEALLOCATE otherNaPTANs_cursor
		
IF (@RowCount > 0)
		COMMIT TRANSACTION 
	ELSE
		ROLLBACK TRANSACTION
		
-- Removing xml doc from memory
EXEC sp_xml_removedocument @DocID
