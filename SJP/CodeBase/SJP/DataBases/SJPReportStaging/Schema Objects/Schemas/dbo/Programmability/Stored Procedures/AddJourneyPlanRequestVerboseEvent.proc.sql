-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddJourneyPlanRequestVerboseEvent] (@JourneyPlanRequestId varchar(50), @JourneyRequestData ntext, @SessionId nvarchar(88), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into JourneyPlanRequestVerboseEvent Table'

    Insert into JourneyPlanRequestVerboseEvent (JourneyPlanRequestId, JourneyRequestData, SessionId, UserLoggedOn, TimeLogged)
    Values (@JourneyPlanRequestId, @JourneyRequestData, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End