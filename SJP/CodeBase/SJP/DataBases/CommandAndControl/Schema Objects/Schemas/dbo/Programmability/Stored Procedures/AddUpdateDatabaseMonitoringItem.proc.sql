
CREATE PROCEDURE [dbo].[AddUpdateDatabaseMonitoringItem]
(	@ItemId int, @CheckInterval int, @Enabled bit, @Description varchar(200), 
	@SQLHelperDatabaseTarget varchar(200), @SQLQuery varchar(300), @RedCondition varchar(200) )
AS
BEGIN
	IF EXISTS (SELECT * FROM [DatabaseMonitoringItems] 
					 WHERE [ItemID] = @ItemId)

		--Update the configured monitoring item if it already exists		
		BEGIN
			UPDATE [DatabaseMonitoringItems] 	 
					 SET [CheckInterval] = @CheckInterval, [Enabled] = @Enabled,[SQLHelperDatabaseTarget] = @SQLHelperDatabaseTarget,
					 [SQLQuery] = @SQLQuery, [RedCondition] = @RedCondition,[Description] = @Description
					 WHERE [ItemID] = @ItemId
		END
	ELSE
		--Insert the monitoring item
		BEGIN
			SET IDENTITY_INSERT [dbo].[DatabaseMonitoringItems] ON

		    INSERT INTO [dbo].[DatabaseMonitoringItems] 
			([ItemID],[CheckInterval],[Enabled],[Description],[SQLHelperDatabaseTarget],[SQLQuery],[RedCondition]) 
			VALUES (@ItemId,@CheckInterval,@Enabled,@Description,@SQLHelperDatabaseTarget,@SQLQuery,@RedCondition)

			SET IDENTITY_INSERT [dbo].[DatabaseMonitoringItems] OFF
		END
END
GO
