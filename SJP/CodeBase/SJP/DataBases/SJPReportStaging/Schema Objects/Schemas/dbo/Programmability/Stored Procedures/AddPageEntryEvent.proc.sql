-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddPageEntryEvent] (@Page varchar(50), @SessionId varchar(50), @UserLoggedOn bit, @TimeLogged datetime, @ThemeID int)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into PageEntryEvent Table'

    Insert into PageEntryEvent (Page, SessionId, UserLoggedOn, TimeLogged, ThemeID)
    Values (@Page, @SessionId, @UserLoggedOn, @TimeLogged, @ThemeID)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End