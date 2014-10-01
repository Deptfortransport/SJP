CREATE TABLE [dbo].[MapEvent] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [CommandCategory] VARCHAR (50)  NULL,
    [Submitted]       DATETIME      NULL,
    [DisplayCategory] VARCHAR (50)  NULL,
    [SessionId]       NVARCHAR (88) NULL,
    [UserLoggedOn]    BIT           NULL,
    [TimeLogged]      DATETIME      NULL
);

