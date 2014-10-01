
CREATE PROCEDURE [dbo].[AddInternationalPlannerResultEvent] (@InternationalPlannerRequestId varchar(50),
		 @ResponseCategory varchar(50),
         @SessionId varchar(50),
         @UserLoggedOn bit, 
         @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into InternationalPlannerResultEvent Table'

    Insert into InternationalPlannerResultEvent (InternationalPlannerRequestId, ResponseCategory, SessionId, UserLoggedOn, TimeLogged)
		Values (@InternationalPlannerRequestId, @ResponseCategory, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End