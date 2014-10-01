-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddUserFeedbackEvent] (@SessionId [varchar] (100), @SubmittedTime datetime, @FeedbackType [varchar] (50), @AcknowledgedTime datetime, @AcknowledgmentSent bit, @UserLoggedOn bit, @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to insert a new record into UserFeedbackEvent table'

    Insert into UserFeedbackEvent (SessionId, SubmittedTime, FeedbackType,	AcknowledgedTime,	AcknowledgmentSent, UserLoggedOn, TimeLogged)
    Values (@SessionId, @SubmittedTime, @FeedbackType, @AcknowledgedTime, @AcknowledgmentSent, @UserLoggedOn, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End