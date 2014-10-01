---------------------------------------------------------------------
-- Change stored procs
---------------------------------------------------------------------

CREATE  PROCEDURE [dbo].[AddWorkloadEvent] 
@Requested datetime, 
@TimeLogged datetime, 
@NumberRequested int, 
@PartnerId int = 0
As
    set nocount off

    declare @localized_string_UnableToInsertWorkloadEvent AS nvarchar(256)
    set @localized_string_UnableToInsertWorkloadEvent = 'Unable to Insert a new record into WorkloadEvent Table'

    Insert into WorkloadEvent (Requested, TimeLogged, NumberRequested, PartnerId)
    Values (@Requested, @TimeLogged, @NumberRequested, @PartnerId)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsertWorkloadEvent, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End