CREATE  PROCEDURE [dbo].[AddGradientProfileEvent] (@DisplayCategory varchar(50), @Submitted datetime, @SessionId varchar(50), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into GradientProfileEvent Table'

    Insert into GradientProfileEvent (DisplayCategory, Submitted, SessionId, UserLoggedOn, TimeLogged)
    Values (@DisplayCategory, @Submitted, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End