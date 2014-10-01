CREATE PROCEDURE [dbo].[AddLandingPageEvent] (@LPPCode varchar(50), 
				      @LPSCode varchar(50), 
                                      @TimeLogged datetime,
				      @SessionId nvarchar(88),
				      @UserLoggedOn bit) 
AS

	SET NOCOUNT OFF

	DECLARE @localized_string_UnableToInsert AS NVARCHAR(256)
   	SET @localized_string_UnableToInsert = 'Unable to Insert a new record into LandingPageEvent Table'

	INSERT INTO LandingPageEvent (LPPCode, LPSCode, TimeLogged, SessionID, UserLoggedon)
	VALUES (@LPPCode, @LPSCode, @TimeLogged, @SessionId, @UserLoggedOn)
		
	IF @@error <> 0
	    BEGIN
	        RAISERROR (@localized_string_UnableToInsert, 1,1)
		RETURN -1
	    END
	ELSE
	    RETURN @@rowcount
	

-- Create stored procedure for rolling up events onto reporting database