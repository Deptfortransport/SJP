-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DeleteStagingData]
	@StagingTableName varchar(30)
AS

DECLARE @SQL nvarchar(200)

SET @SQL=N'
DELETE FROM ' + @StagingTableName

EXEC sp_executesql @SQL