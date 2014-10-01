
CREATE PROCEDURE [dbo].[AddMapEvent] (@CommandCategory varchar(50), @Submitted datetime, @DisplayCategory varchar(50), @SessionId varchar(50), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into MapEvent Table'

    Insert into MapEvent (CommandCategory, Submitted, DisplayCategory, SessionId, UserLoggedOn, TimeLogged)
		Values (@CommandCategory, @Submitted, @DisplayCategory, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End