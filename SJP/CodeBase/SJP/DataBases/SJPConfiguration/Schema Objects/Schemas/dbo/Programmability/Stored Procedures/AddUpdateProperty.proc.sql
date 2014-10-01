-- =============================================
-- Stored procedure to add or update a property
-- =============================================
CREATE PROCEDURE [dbo].[AddUpdateProperty]
	@pName varchar(255), 
	@pValue varchar(2000),
	@AID varchar(50),
	@GID varchar(50)
AS
	IF not exists (select top 1 * from properties 
								 where pName = @pName 
								   and AID = @AID 
								   and GID = @GID)
	BEGIN
		insert into properties values (
			@pName, 
			@pValue, 
			@AID, 
			@GID)
	END
	ELSE
	BEGIN
		update properties 
		set pValue = @pValue
		where pName = @pName
		  and AID = @AID
		  and GID = @GID
	END	

RETURN 0