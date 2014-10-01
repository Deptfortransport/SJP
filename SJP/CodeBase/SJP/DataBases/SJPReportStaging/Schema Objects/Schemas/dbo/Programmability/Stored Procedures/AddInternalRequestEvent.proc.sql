CREATE PROCEDURE [dbo].[AddInternalRequestEvent] (
	@InternalRequestId varchar(50), 
	@SessionId nvarchar(88), 
	@Submitted datetime, 
	@InternalRequestType varchar(50), 
	@FunctionType char(2),
	@Success bit, 
	@RefTransaction bit, 
	@TimeLogged datetime)
as
    set nocount off

    declare @rowcount int
	declare @error int

    declare @localized_string_UnableToInsert as nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into InternalRequestEvent Table'

    insert into InternalRequestEvent
    (InternalRequestId, SessionId, Submitted, InternalRequestType, FunctionType, Success, RefTransaction, TimeLogged)
    values (@InternalRequestId, @SessionId, @Submitted, @InternalRequestType, @FunctionType, @Success, @RefTransaction, @TimeLogged)

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