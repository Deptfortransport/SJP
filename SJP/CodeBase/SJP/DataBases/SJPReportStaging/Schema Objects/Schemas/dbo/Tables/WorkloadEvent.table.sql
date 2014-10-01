CREATE TABLE [dbo].[WorkloadEvent] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [Requested]       DATETIME NULL,
    [TimeLogged]      DATETIME NULL,
    [NumberRequested] INT      NOT NULL,
    [PartnerId]       INT      NOT NULL
);

