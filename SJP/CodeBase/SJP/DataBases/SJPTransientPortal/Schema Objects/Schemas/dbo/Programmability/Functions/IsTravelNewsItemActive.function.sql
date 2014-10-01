CREATE FUNCTION [dbo].[IsTravelNewsItemActive]
(	
	@NewsItemDaymask	varchar(14), 
	@NewsItemStartTime	TIME, 
	@NewsItemEndTime	TIME,
	@ComparisonDate    DATETIME
)
RETURNS Bit
AS
BEGIN
		--Clean up input parameters - Default to active all the time so we will SHOW incidents if no details given
	If @NewsItemStartTime IS NULL
		BEGIN
			SET @NewsItemStartTime = '00:00:00'
		END
	If @NewsItemEndTime IS NULL
		BEGIN
			SET @NewsItemEndTime = '23:59:59'
		END
	If (@NewsItemDaymask = '' OR  @NewsItemDaymask IS NULL)
		BEGIN
			SET @NewsItemDaymask = 'SuMoTuWeThFrSa'
		END

	DECLARE @COMPARISONTIME TIME, @NewsItemStartTimeAdjusted TIME
	DECLARE @COMPARISONDAY CHAR(2), @COMPARISONDAYMINUS1 CHAR(2)
	DECLARE @ADVANCEDNOTIFICATIONMINS INT, @RETURNVAL BIT
	
	
	--Adjust Start time for the advanced notification period defined in Properties DB
	SET @ADVANCEDNOTIFICATIONMINS = ISNULL((SELECT pValue FROM [SJPConfiguration].[dbo].[properties] WHERE pName = 'TravelNews.IncidentStatus.Active.AdvancedNotification.Minutes'),0) 
	SET @NewsItemStartTimeAdjusted = DATEADD(mi,(0-@ADVANCEDNOTIFICATIONMINS), @NewsItemStartTime)
	If (@NewsItemStartTimeAdjusted > @NewsItemStartTime)
		--If start time after adjustment > orig start time we've gone back over midnight
		--dont want to do that so just set it to midnight
		BEGIN
			SET @NewsItemStartTimeAdjusted = '00:00:00'
		END
	--Get time part of comparison date by simply putting it in TIME data type
	SET @COMPARISONTIME = @ComparisonDate
	--Get 2 char day parts for comparison
	SET @COMPARISONDAY = LEFT(DATENAME(dw, @ComparisonDate),2)
	SET @COMPARISONDAYMINUS1 = LEFT(DATENAME(dw, DATEADD(day, -1, @ComparisonDate)),2)

	If (@NewsItemStartTime > @NewsItemEndTime ) 
		--Start time is after end time - overmidnight incident
		BEGIN
			--Return true if day matches and time between news start time and midnight...
			If (CHARINDEX(@COMPARISONDAY,@NewsItemDaymask) <> 0   
				AND @COMPARISONTIME > @NewsItemStartTimeAdjusted)  
				--or if day - 1 matches and time is between midnight and news end time...
				OR (CHARINDEX(@COMPARISONDAYMINUS1,@NewsItemDaymask) <> 0 
					AND @COMPARISONTIME < @NewsItemEndTime)
					SET @RETURNVAL = 1
			Else
				--return false
					SET @RETURNVAL = 0
		END
	Else
		--Start time before end time - nice and simple not overmidnight
		BEGIN
			--Return true if day matches and time between news 
			--item start time and news item end time...
			If (CHARINDEX(@COMPARISONDAY,@NewsItemDaymask) <> 0 
				AND (@COMPARISONTIME BETWEEN @NewsItemStartTimeAdjusted AND @NewsItemEndTime))  
					SET @RETURNVAL = 1
			Else
					SET @RETURNVAL = 0
		END
	RETURN @RETURNVAL
END
