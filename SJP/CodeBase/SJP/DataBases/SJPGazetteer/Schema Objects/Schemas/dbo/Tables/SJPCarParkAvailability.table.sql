CREATE TABLE [dbo].[SJPCarParkAvailability] (
    [AvailabilityID]   NVARCHAR (50) NOT NULL,
    [FromDate]         DATE          NOT NULL,
    [ToDate]           DATE          NOT NULL,
    [DailyOpeningTime] TIME (0)      NOT NULL,
    [DailyClosingTime] TIME (0)      NOT NULL,
    [DaysOfWeek]       NVARCHAR (30) NOT NULL
);



