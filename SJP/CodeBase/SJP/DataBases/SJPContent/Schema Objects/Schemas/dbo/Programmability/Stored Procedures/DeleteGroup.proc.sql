CREATE PROCEDURE [dbo].[DeleteGroup]
(
	@Group varchar(100)
)
AS
BEGIN

	DECLARE @GroupId int

	--Look up the GroupId:
    SELECT @GroupId = GroupId 
      FROM [dbo].ContentGroup
     WHERE [Name] = @Group

	IF @GroupId IS NOT NULL
		BEGIN
			-- Check if there is any Content using this group
			IF EXISTS (SELECT * 
						 FROM [dbo].Content
						WHERE GroupId = @GroupId)
				BEGIN
					RAISERROR ('Content exists for group %s, unable to delete group', 16, 1, @Group)
					RETURN 0
				END

			-- OK to delete group
			DELETE FROM [dbo].ContentGroup
				  WHERE [Name] = @Group

		END

END