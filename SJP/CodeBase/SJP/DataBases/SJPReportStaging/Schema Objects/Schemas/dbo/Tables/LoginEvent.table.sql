CREATE TABLE [dbo].[LoginEvent] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [SessionId]    NVARCHAR (88) NULL,
    [UserLoggedOn] BIT           NULL,
    [TimeLogged]   DATETIME      NULL
);

