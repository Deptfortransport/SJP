-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetTableColumnLengths]
	@TableName nvarchar(128)
AS
	SELECT column_name, character_maximum_length length 
	FROM information_schema.columns 
	WHERE table_name = @TableName 
	AND character_maximum_length is not null