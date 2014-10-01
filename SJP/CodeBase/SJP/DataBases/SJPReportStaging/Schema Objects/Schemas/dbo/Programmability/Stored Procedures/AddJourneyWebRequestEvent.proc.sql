-- Stored procs must be there so no need to check

CREATE PROCEDURE [dbo].[AddJourneyWebRequestEvent] (
	@JourneyWebRequestId varchar(50), 
	@SessionId nvarchar(88), 
	@Submitted datetime, 
	@RequestType varchar(50),
	@RegionCode varchar(50), 
	@Success bit, 
	@RefTransaction bit, 
	@TimeLogged datetime)
as
    set nocount off

    declare @rowcount int
	declare @error int

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into JourneyWebRequestEvent Table'

    Insert into JourneyWebRequestEvent (JourneyWebRequestId, SessionId, Submitted, RequestType, RegionCode, Success, RefTransaction, TimeLogged) 
    Values (@JourneyWebRequestId, @SessionId, @Submitted, @RequestType, @RegionCode, @Success, @RefTransaction, @TimeLogged)

	select @error = @@error, @rowcount = @@rowcount

    if @error <> 0
    begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @rowcount
    end