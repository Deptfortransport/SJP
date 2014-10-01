CREATE TABLE [dbo].[UserPreferenceSaveEvent] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [EventCategory] VARCHAR (50)  NULL,
    [SessionId]     NVARCHAR (88) NULL,
    [TimeLogged]    DATETIME      NULL
);

