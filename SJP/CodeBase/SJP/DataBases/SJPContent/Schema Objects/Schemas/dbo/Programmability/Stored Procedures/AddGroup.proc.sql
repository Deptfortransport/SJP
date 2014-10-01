CREATE PROCEDURE [dbo].[AddGroup]
(
	@Group varchar(100)
)
AS
BEGIN

	DECLARE @GroupId int

	IF NOT EXISTS (SELECT GroupId 
				     FROM ContentGroup 
					WHERE [Name] = @Group)
		BEGIN
			SET @GroupId = (SELECT ISNULL(Max(GroupId),0) + 1 FROM ContentGroup)

			INSERT INTO ContentGroup(GroupId, [Name]) 
				 VALUES (@GroupId, @Group)
		END
END
