CREATE PROCEDURE [dbo].[AddContentOverride]
(
	@Group varchar(100),
	@Language char(2),
	@ControlName varchar(60),
	@PropertyName varchar(100),
	@ContentValue text,
	@StartDate datetime,
	@EndDate datetime
)
AS
BEGIN

	DECLARE @GroupId int
   
    --Look up the GroupId:
    SELECT @GroupId = GroupId 
      FROM [dbo].ContentGroup
     WHERE [Name] = @Group

    --Check the group:
    IF @GroupId IS NULL
    	BEGIN
    		RAISERROR ('%s is an invalid group', 16, 1, @Group)
			RETURN 0
    	END

	IF EXISTS (SELECT * FROM [dbo].ContentOverride 
					   WHERE ContentOverride.PropertyName = @PropertyName 
						 AND ContentOverride.ControlName = @ControlName 
						 AND ContentOverride.CultureCode = @Language 
						 AND ContentOverride.GroupId = @GroupId )
		BEGIN
			UPDATE [dbo].ContentOverride
			   SET ContentValue = @ContentValue,
				   StartDate = @StartDate,
				   EndDate = @EndDate
			 WHERE ContentOverride.PropertyName = @PropertyName 
			   AND ContentOverride.ControlName = @ControlName 
			   AND ContentOverride.CultureCode = @Language 
			   AND ContentOverride.GroupId = @GroupId
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].ContentOverride ( GroupId, CultureCode, ControlName, PropertyName, ContentValue, StartDate, EndDate)
			VALUES ( @GroupId, @Language, @ControlName, @PropertyName, @ContentValue, @StartDate, @EndDate)
		END

END
