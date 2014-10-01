-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddRetailerHandoffEvent] (@RetailerId varchar(50), @SessionId nvarchar(88), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into RetailerHandoffEvent Table'

    Insert into RetailerHandoffEvent (RetailerId, SessionId, UserLoggedOn, TimeLogged)
    Values (@RetailerId, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End