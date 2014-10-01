-----------------------------------------------------------------------------------------------
-- AddRepeatVisitorEvent
-----------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[AddRepeatVisitorEvent] 
(
@RepeatVistorType varchar(20), 
@LastVisited datetime, 
@SessionIdOld varchar(50), 
@SessionIdNew varchar(50),
@DomainName varchar(100),
@UserAgent varchar(200),
@ThemeId int,
@TimeLogged datetime
)
As
    set nocount off

    declare @localized_string_UnableToInsert AS nvarchar(256)
    set @localized_string_UnableToInsert = 'Unable to Insert a new record into RepeatVisitorEvent Table'

    Insert into RepeatVisitorEvent (RepeatVistorType, LastVisited, SessionIdOld, SessionIdNew, DomainName, UserAgent, ThemeId, TimeLogged)
    Values (@RepeatVistorType, @LastVisited, @SessionIdOld, @SessionIdNew, @DomainName, @UserAgent, @ThemeId, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsert, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End