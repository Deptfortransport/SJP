CREATE  PROCEDURE [dbo].[AddCyclePlannerRequestEvent] (@CyclePlannerRequestId varchar(50), @Cycle bit, @SessionId varchar(50), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into CyclePlannerRequestEvent Table'

    Insert into CyclePlannerRequestEvent (CyclePlannerRequestId, Cycle, SessionId, UserLoggedOn, TimeLogged)
    Values (@CyclePlannerRequestId, @Cycle, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End