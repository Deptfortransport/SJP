CREATE PROCEDURE [dbo].[InsertFileMonitoringResult]
           (@MonitoringItemID int
           ,@SJP_Server varchar(50)
           ,@Description varchar(200)
           ,@CheckTime datetime
           ,@ValueAtCheck varchar(200)
           ,@IsInRed bit
		   ,@FileCreatedDateTime datetime
		   ,@FileModifiedDateTime datetime
		   ,@FileSize varchar(30)
		   ,@FileProductVersion varchar(30))
AS
IF NOT EXISTS (SELECT [MonitoringItemID],[SJP_Server],[Description],[CheckTime],[ValueAtCheck],[IsInRed]
  FROM [dbo].[SJPFileMonitoringResults]
  WHERE [SJP_Server] = @SJP_Server AND [MonitoringItemID] = @MonitoringItemID AND [CheckTime] = @CheckTime)
BEGIN

INSERT INTO [dbo].[SJPFileMonitoringResults]
           ([MonitoringItemID]
           ,[SJP_Server]
           ,[Description]
           ,[CheckTime]
           ,[ValueAtCheck]
           ,[IsInRed]
		   ,[FileCreatedDateTime]
		   ,[FileModifiedDateTime]
		   ,[FileSize]
		   ,[FileProductVersion])
     VALUES
           (@MonitoringItemID 
           ,@SJP_Server 
           ,@Description 
           ,@CheckTime 
           ,@ValueAtCheck 
           ,@IsInRed
		   ,@FileCreatedDateTime
		   ,@FileModifiedDateTime
		   ,@FileSize
		   ,@FileProductVersion )
END

RETURN 0