﻿CREATE TABLE [dbo].[JourneyPlanResultsVerboseEvent] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [JourneyPlanRequestId] VARCHAR (50)  NULL,
    [JourneyResultsData]   TEXT          NULL,
    [SessionId]            NVARCHAR (88) NULL,
    [UserLoggedOn]         BIT           NULL,
    [TimeLogged]           DATETIME      NULL
);

