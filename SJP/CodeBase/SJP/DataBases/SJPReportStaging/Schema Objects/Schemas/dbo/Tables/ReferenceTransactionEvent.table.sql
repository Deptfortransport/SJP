CREATE TABLE [dbo].[ReferenceTransactionEvent] (
    [Submitted]             DATETIME     NOT NULL,
    [EventType]             VARCHAR (50) NOT NULL,
    [ServiceLevelAgreement] BIT          NULL,
    [SessionId]             VARCHAR (50) NULL,
    [TimeLogged]            DATETIME     NULL,
    [Successful]            BIT          NULL,
    [MachineName]           VARCHAR (50) NULL
);

