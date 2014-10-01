
CREATE  PROCEDURE AddReferenceTransactionEventSiteStatus 
(@EventType varchar(50), 
 @ServiceLevelAgreement bit, 
 @Submitted datetime, 
 @SessionId varchar(50), 
 @TimeLogged datetime, 
 @Successful bit)
As
    set nocount off

    declare @localized_string_UnableToInsert AS varchar(60)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into ReferenceTransactionEvent Table'

    Insert into ReferenceTransactionEvent (Submitted, EventType, ServiceLevelAgreement, SessionId, TimeLogged, Successful)
    Values (@Submitted, @EventType, @ServiceLevelAgreement, @SessionId, @TimeLogged, @Successful)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End