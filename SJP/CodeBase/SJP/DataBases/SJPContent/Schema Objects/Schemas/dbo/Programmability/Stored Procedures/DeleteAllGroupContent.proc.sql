CREATE PROCEDURE [dbo].[DeleteAllGroupContent]
	@Group varchar(100)
AS
BEGIN

	-- Deletes all content for the Group specified, 
	-- but does not delete the group

	DECLARE @GroupId int
   
    --Look up the GroupId:
    SELECT @GroupId = GroupId 
      FROM [dbo].ContentGroup
     WHERE [Name] = @Group

	IF @GroupId IS NOT NULL
    	BEGIN

			-- Delete from ContentOverride
			IF EXISTS (SELECT * FROM [dbo].ContentOverride 
					   WHERE ContentOverride.GroupId = @GroupId )
			BEGIN
				DELETE FROM [dbo].ContentOverride
					  WHERE ContentOverride.GroupId = @GroupId
			END

			-- Delete from Content
			IF EXISTS (SELECT * FROM [dbo].Content 
					   WHERE Content.GroupId = @GroupId )
			BEGIN
				DELETE FROM [dbo].Content
				      WHERE Content.GroupId = @GroupId
			END

		END
END