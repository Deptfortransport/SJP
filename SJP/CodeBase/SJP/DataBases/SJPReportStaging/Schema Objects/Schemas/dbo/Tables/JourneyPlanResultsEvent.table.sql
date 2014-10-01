CREATE TABLE [dbo].[JourneyPlanResultsEvent] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [JourneyPlanRequestId] VARCHAR (50)  NULL,
    [ResponseCategory]     VARCHAR (50)  NULL,
    [SessionId]            NVARCHAR (88) NULL,
    [UserLoggedOn]         BIT           NULL,
    [TimeLogged]           DATETIME      NULL
);

