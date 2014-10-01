-- Create stored procs for inserts

CREATE PROCEDURE [dbo].[AddDataGatewayEvent] (@FeedId varchar(50), @SessionId nvarchar(88), @FileName varchar (100), @TimeStarted datetime, @TimeFinished datetime, @SuccessFlag bit, @ErrorCode int, @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into DataGatewayEvent Table'

    Insert into DataGatewayEvent (FeedId, SessionId, FileName, TimeStarted, TimeFinished, SuccessFlag, ErrorCode, UserLoggedOn, TimeLogged)
    Values (@FeedId, @SessionId, @FileName, @TimeStarted, @TimeFinished, @SuccessFlag, @ErrorCode, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End