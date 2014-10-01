CREATE FUNCTION [dbo].[StripNaptan]
(
	@NaptanPlaceName varchar(100), 
	@Delimitter varchar(10)
)
RETURNS varchar(20)
AS
BEGIN
	DECLARE @CharPosition int 	
	DECLARE @tempNaptanPlaceName varchar(100)
	DECLARE @returnVal  varchar(20)
	
	SET @tempNaptanPlaceName = ISNULL(@NaptanPlaceName, '')
	
	IF LEN(@tempNaptanPlaceName) > 0
		BEGIN
			SET @CharPosition = CHARINDEX(@Delimitter, @tempNaptanPlaceName)	

			IF (@CharPosition > 0)
				SET @returnVal =  SUBSTRING(@tempNaptanPlaceName, 1, @CharPosition-1)
			ELSE
				SET @returnVal = @tempNaptanPlaceName
		END 
	ELSE
		SET @returnVal = ''
	
	RETURN @returnVal
END