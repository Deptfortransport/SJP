-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddLocationRequestEvent](@JourneyPlanRequestId varchar(50), @PrepositionCategory varchar(50), @AdminAreaCode varchar(50), @RegionCode varchar(50), @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into LocationRequestEvent Table'

    Insert into LocationRequestEvent (JourneyPlanRequestId, PrepositionCategory, AdminAreaCode, RegionCode, TimeLogged)
    Values (@JourneyPlanRequestId, @PrepositionCategory, @AdminAreaCode, @RegionCode, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End