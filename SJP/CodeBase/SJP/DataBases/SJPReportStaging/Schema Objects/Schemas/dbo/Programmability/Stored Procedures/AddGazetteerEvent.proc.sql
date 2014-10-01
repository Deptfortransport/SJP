-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddGazetteerEvent] (@EventCategory varchar(50), @SessionId nvarchar(88), @UserLoggedOn bit, @Submitted datetime, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into GazetteerEvent Table'

    Insert into GazetteerEvent (EventCategory, SessionId, UserLoggedOn, Submitted, TimeLogged)
    Values (@EventCategory, @SessionId,	@UserLoggedOn, @Submitted, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End