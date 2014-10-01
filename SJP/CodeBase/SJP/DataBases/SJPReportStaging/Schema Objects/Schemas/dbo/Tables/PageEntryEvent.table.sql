CREATE TABLE [dbo].[PageEntryEvent] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Page]         VARCHAR (50)  NULL,
    [SessionId]    NVARCHAR (88) NULL,
    [UserLoggedOn] BIT           NULL,
    [TimeLogged]   DATETIME      NULL,
    [ThemeID]      INT           DEFAULT ((0)) NULL
);

