CREATE  PROCEDURE [dbo].[AddNoResultsEvent] 
(
	@Submitted datetime, 
	@SessionId varchar(50),
	@UserLoggedOn bit, 
	@TimeLogged datetime
 )
As
    set nocount off

    declare @localized_string_UnableToInsert AS varchar(60)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into NoResultsEventt Table'

    Insert into NoResultsEvent (Submitted, SessionId, UserLoggedOn, TimeLogged)
    Values (@Submitted, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End