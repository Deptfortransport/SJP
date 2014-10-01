CREATE PROCEDURE [dbo].[AddUpdateFileMonitoringItem]
(	@ItemId int, @CheckInterval int, @Enabled bit, @Description varchar(200), 
	@FullFilePath varchar(300), @RedCondition varchar(200) )
AS
BEGIN
	IF EXISTS (SELECT * FROM [FileMonitoringItems] 
					 WHERE [ItemID] = @ItemId)

		--Update the configured monitoring item if it already exists		
		BEGIN
			UPDATE [FileMonitoringItems] 	 
					 SET [CheckInterval] = @CheckInterval, [Enabled] = @Enabled, 
					 [FullFilePath] = @FullFilePath, [RedCondition] = @RedCondition, [Description] = @Description
					 WHERE [ItemID] = @ItemId
		END
	ELSE

		--Insert the monitoring item
		BEGIN
			SET IDENTITY_INSERT [dbo].[FileMonitoringItems] ON

		    INSERT INTO [dbo].[FileMonitoringItems] 
			([ItemID],[CheckInterval],[Enabled],[Description],[FullFilePath],[RedCondition]) 
			VALUES (@ItemId,@CheckInterval,@Enabled,@Description,@FullFilePath,@RedCondition)

			SET IDENTITY_INSERT [dbo].[FileMonitoringItems] OFF
		END
END
GO
