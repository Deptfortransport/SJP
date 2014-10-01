
CREATE  PROCEDURE [dbo].[AddEBCCalculationEvent] (@Submitted datetime, @SessionId varchar(50), @TimeLogged datetime, @Successful bit)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into EBCCalculationEvent Table'

    Insert into EBCCalculationEvent (Submitted, SessionId, TimeLogged, Success)
    Values (@Submitted, @SessionId, @TimeLogged, @Successful)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End