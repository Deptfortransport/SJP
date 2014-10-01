-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddJourneyPlanResultsVerboseEvent] (@JourneyPlanRequestId varchar(50), @JourneyResultsData ntext, @SessionId nvarchar(88), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into JourneyPlanResultsVerboseEvent Table'

    Insert into JourneyPlanResultsVerboseEvent (JourneyPlanRequestId, JourneyResultsData, SessionId, UserLoggedOn, TimeLogged)
    Values (@JourneyPlanRequestId, @JourneyResultsData, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End