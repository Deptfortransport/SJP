CREATE TABLE [dbo].[EnhancedExposedServiceEvent] (
    [Id]                        INT           IDENTITY (1, 1) NOT NULL,
    [EESEPartnerId]             SMALLINT      NOT NULL,
    [EESEInternalTransactionId] VARCHAR (40)  NOT NULL,
    [EESEExternalTransactionId] VARCHAR (100) NOT NULL,
    [EESEServiceType]           VARCHAR (200) NOT NULL,
    [EESEOperationType]         VARCHAR (100) NOT NULL,
    [EESEEventTime]             DATETIME      NOT NULL,
    [EESEIsStartEvent]          BIT           NOT NULL,
    [EESECallSuccessful]        BIT           NOT NULL,
    [TimeLogged]                DATETIME      DEFAULT (getdate()) NOT NULL
);

