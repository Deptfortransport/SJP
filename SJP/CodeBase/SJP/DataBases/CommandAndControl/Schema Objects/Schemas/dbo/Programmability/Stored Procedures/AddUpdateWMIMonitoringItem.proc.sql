CREATE PROCEDURE [dbo].[AddUpdateWMIMonitoringItem]
(	@ItemId int, @CheckInterval int, @Enabled bit, @Description varchar(200), 
	@WQLQuery varchar(300), @RedCondition varchar(200) )
AS
BEGIN
	IF EXISTS (SELECT * FROM [WMIMonitoringItems] 
					 WHERE [ItemID] = @ItemId)
		--Update the configured monitoring item if it already exists		
		BEGIN
			UPDATE [WMIMonitoringItems] 
					 SET [CheckInterval] = @CheckInterval, [Enabled] = @Enabled, 
					 [WQLQuery] = @WQLQuery , [RedCondition] = @RedCondition, [Description] = @Description
					 WHERE [ItemID] = @ItemId
		END
	ELSE
		--Insert the monitoring item
		BEGIN
			SET IDENTITY_INSERT [dbo].[WMIMonitoringItems] ON

		    INSERT INTO [dbo].[WMIMonitoringItems] 
			([ItemID],[CheckInterval],[Enabled],[Description],[WQLQuery],[RedCondition]) 
			VALUES (@ItemId,@CheckInterval,@Enabled,@Description,@WQLQuery,@RedCondition)

			SET IDENTITY_INSERT [dbo].[WMIMonitoringItems] OFF
		END
END
