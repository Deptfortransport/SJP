CREATE TABLE [dbo].[LocationRequestEvent] (
    [Id]                   BIGINT       IDENTITY (1, 1) NOT NULL,
    [JourneyPlanRequestId] VARCHAR (50) NULL,
    [PrepositionCategory]  VARCHAR (50) NULL,
    [AdminAreaCode]        VARCHAR (50) NULL,
    [RegionCode]           VARCHAR (50) NULL,
    [TimeLogged]           DATETIME     NULL
);

