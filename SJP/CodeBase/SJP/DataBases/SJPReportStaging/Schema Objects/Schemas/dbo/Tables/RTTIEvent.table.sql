CREATE TABLE [dbo].[RTTIEvent] (
    [Id]                      INT      IDENTITY (1, 1) NOT NULL,
    [StartTime]               DATETIME NULL,
    [FinishTime]              DATETIME NULL,
    [DataRecievedSucessfully] BIT      NOT NULL,
    [TimeLogged]              DATETIME NOT NULL
);

