CREATE TABLE [dbo].[JourneyPlanRequestEvent] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [JourneyPlanRequestId] VARCHAR (50)  NULL,
    [Air]                  BIT           NULL,
    [Bus]                  BIT           NULL,
    [Car]                  BIT           NULL,
    [Coach]                BIT           NULL,
    [Cycle]                BIT           NULL,
    [Drt]                  BIT           NULL,
    [Ferry]                BIT           NULL,
    [Metro]                BIT           NULL,
    [Rail]                 BIT           NULL,
    [Taxi]                 BIT           NULL,
    [Tram]                 BIT           NULL,
    [Underground]          BIT           NULL,
    [Walk]                 BIT           NULL,
    [SessionId]            NVARCHAR (88) NULL,
    [UserLoggedOn]         BIT           NULL,
    [TimeLogged]           DATETIME      NULL
);

