CREATE PROCEDURE [dbo].[AddStopEventRequestEvent] (@Submitted datetime, @RequestId varchar(50), @RequestType varchar(50), @Successful bit, @TimeLogged datetime) AS


INSERT INTO StopEventRequestEvent
                      (Submitted, RequestId, RequestType, Successful, TimeLogged)
VALUES     (@Submitted, @RequestId, @RequestType, @Successful, @TimeLogged)