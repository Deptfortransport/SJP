CREATE TABLE [dbo].[GazetteerEvent] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [EventCategory] VARCHAR (50)  NULL,
    [SessionId]     NVARCHAR (88) NULL,
    [UserLoggedOn]  BIT           NULL,
    [TimeLogged]    DATETIME      NULL,
    [Submitted]     DATETIME      NULL
);

