CREATE PROCEDURE [dbo].[AddRTTIInternalEvent]
(
	@StartTime datetime,
	@EndTime datetime,
	@NumberOfRetries int,
	@Successful bit
)
AS

INSERT INTO RTTIInternalEvent(StartTime, EndTime, NumberOfRetries, Successful)
VALUES(@StartTime, @EndTime, @NumberOfRetries, @Successful)