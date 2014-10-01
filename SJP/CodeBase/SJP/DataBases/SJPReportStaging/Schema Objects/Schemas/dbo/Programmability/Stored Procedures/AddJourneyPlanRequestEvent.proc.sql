-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddJourneyPlanRequestEvent] (@JourneyPlanRequestId varchar(50), @Air bit, @Bus bit, @Car bit, @Coach bit, @Cycle bit, @Drt bit, @Ferry bit, @Metro bit, @Rail bit, @Taxi bit, @Tram bit, @Underground bit, @Walk bit, @SessionId nvarchar(88), @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into JourneyPlanRequestEvent Table'

    Insert into JourneyPlanRequestEvent (JourneyPlanRequestId, Air, Bus, Car, Coach, Cycle, Drt, Ferry, Metro, Rail, Taxi, Tram, Underground, Walk, SessionId, UserLoggedOn, TimeLogged)
    Values (@JourneyPlanRequestId, @Air, @Bus, @Car, @Coach, @Cycle, @Drt, @Ferry, @Metro, @Rail, @Taxi, @Tram, @Underground, @Walk, @SessionId, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End