CREATE PROCEDURE [dbo].[RTTIEventLog] (@StartTime datetime, @FinishTime datetime, @DataReceived bit) AS


INSERT INTO RTTIEvent
                      (StartTime, FinishTime, DataRecievedSucessfully)
VALUES     (@StartTime,@FinishTime, @DataReceived)