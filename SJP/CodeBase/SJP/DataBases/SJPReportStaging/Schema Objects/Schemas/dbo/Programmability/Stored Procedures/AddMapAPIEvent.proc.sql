
CREATE PROCEDURE [dbo].[AddMapAPIEvent] (@CommandCategory varchar(50), @Submitted datetime, @SessionId varchar(50), @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into MapAPIEvent Table'

    Insert into MapAPIEvent (CommandCategory, Submitted, SessionId, TimeLogged)
		Values (@CommandCategory, @Submitted, @SessionId, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End