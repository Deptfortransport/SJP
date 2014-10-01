--create procedure
CREATE PROCEDURE [dbo].[AddOperationalEvent] (@SessionId nvarchar(88), @Message varchar(1500), @MachineName varchar(50), @AssemblyName varchar(50), @MethodName varchar(50), @TypeName varchar(50), @Level varchar(50), @Category varchar(50), @Target varchar(50), @TimeLogged datetime)
As
    set nocount off

    declare @localized_string_UnableToInsertOperationalEvent AS nvarchar(256)
    set @localized_string_UnableToInsertOperationalEvent = 'Unable to Insert a new record into OperationalEvent Table'

    Insert into OperationalEvent (SessionId, Message, MachineName, AssemblyName, MethodName, TypeName, Level, Category, Target, TimeLogged)
    Values (@SessionId, @Message, @MachineName, @AssemblyName, @MethodName, @TypeName, @Level, @Category, @Target, @TimeLogged)

    if @@error <> 0
    Begin
        raiserror (@localized_string_UnableToInsertOperationalEvent, 1,1)
        return -1
    end
    else
    begin
        return @@rowcount
    End