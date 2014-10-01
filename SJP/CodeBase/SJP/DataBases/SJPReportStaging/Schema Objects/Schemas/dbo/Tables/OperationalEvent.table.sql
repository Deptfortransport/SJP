CREATE TABLE [dbo].[OperationalEvent] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [SessionId]    NVARCHAR (88)  NULL,
    [Message]      VARCHAR (1500) NULL,
    [MachineName]  VARCHAR (50)   NULL,
    [AssemblyName] VARCHAR (50)   NULL,
    [MethodName]   VARCHAR (50)   NULL,
    [TypeName]     VARCHAR (50)   NULL,
    [Level]        VARCHAR (50)   NULL,
    [Category]     VARCHAR (50)   NULL,
    [Target]       VARCHAR (50)   NULL,
    [TimeLogged]   DATETIME       NULL
);


