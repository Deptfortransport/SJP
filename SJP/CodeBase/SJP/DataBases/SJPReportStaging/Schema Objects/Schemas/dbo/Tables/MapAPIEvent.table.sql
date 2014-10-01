CREATE TABLE [dbo].[MapAPIEvent] (
    [Id]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [CommandCategory] VARCHAR (50) NULL,
    [Submitted]       DATETIME     NULL,
    [SessionId]       VARCHAR (50) NULL,
    [TimeLogged]      DATETIME     NULL
);

