CREATE TABLE [dbo].[SJPVenueGateCheckConstraints] (
    [CheckConstraintID]   NVARCHAR (50)  NOT NULL,
    [CheckConstraintName] NVARCHAR (150) NOT NULL,
    [Entry]               BIT            NOT NULL,
    [Process]             NVARCHAR (50)  NOT NULL,
    [Congestion]          NVARCHAR (50)  NOT NULL,
    [AverageDelay]        TIME (0)       NOT NULL,
    [GateNaptan]          NVARCHAR (20)  NOT NULL
);





