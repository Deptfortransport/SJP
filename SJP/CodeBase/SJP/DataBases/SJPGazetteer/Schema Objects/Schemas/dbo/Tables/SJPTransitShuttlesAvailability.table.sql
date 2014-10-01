CREATE TABLE [dbo].[SJPTransitShuttlesAvailability] (
    [TransitShuttleAvailabilityID]   NVARCHAR (50) NOT NULL,
    [FromDate]         DATE          NOT NULL,
    [ToDate]           DATE          NOT NULL,
    [DailyStartTime]   TIME (0)      NOT NULL,
    [DailyEndTime]     TIME (0)      NOT NULL,
    [DaysOfWeek]       NVARCHAR (30) NOT NULL
);