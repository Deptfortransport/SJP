CREATE FUNCTION [dbo].[fn_TravelNewsRegion]
(
)
RETURNS @TravelNewsRegion TABLE
(
	UID varchar(25),
	Regions varchar(200)
)
AS
BEGIN
	DECLARE @UID varchar(25)
	DECLARE @RegionName varchar(25)
	DECLARE @LastUID varchar(25)
	DECLARE @Regions varchar(200)
	
	DECLARE LocalCursor CURSOR FORWARD_ONLY READ_ONLY FOR
	SELECT
		UID,
		RegionName
	FROM TravelNewsRegion
	ORDER BY UID, RegionName
	
	OPEN LocalCursor
	
	SET @LastUID = ''
	FETCH NEXT FROM LocalCursor INTO @UID, @RegionName
	
	WHILE @@Fetch_Status <> -1
	BEGIN
		IF @@Fetch_Status <> -2
		BEGIN
			IF @UID = @LastUID
				SET @Regions = @Regions + ', ' + @RegionName
			ELSE
				SET @Regions = @RegionName
		END
		
		SET @LastUID = @UID
		
		FETCH NEXT FROM LocalCursor INTO @UID, @RegionName
		
		IF @@Fetch_Status <> -1 AND @@Fetch_Status <> -2
		BEGIN
			IF @UID <> @LastUID
			BEGIN
				INSERT INTO @TravelNewsRegion
				(
					UID,
					Regions
				)
				VALUES
				(
					@LastUID,
					@Regions
				)
				
				SET @Regions = ''
			END
		END
		ELSE IF LEN(@Regions) > 0
		BEGIN
			INSERT INTO @TravelNewsRegion
			(
				UID,
				Regions
			)
			VALUES
			(
				@LastUID,
				@Regions
			)
			
			SET @Regions = ''
		END
	END
	
	CLOSE LocalCursor
	DEALLOCATE LocalCursor
	
	RETURN
END