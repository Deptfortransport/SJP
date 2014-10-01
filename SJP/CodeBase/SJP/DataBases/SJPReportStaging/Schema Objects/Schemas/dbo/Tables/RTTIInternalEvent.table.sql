CREATE TABLE [dbo].[RTTIInternalEvent] (
    [InternalEventID] INT      IDENTITY (1, 1) NOT NULL,
    [StartTime]       DATETIME NOT NULL,
    [EndTime]         DATETIME NOT NULL,
    [NumberOfRetries] INT      NOT NULL,
    [Successful]      BIT      NOT NULL,
    [TimeLogged]      DATETIME NOT NULL
);

