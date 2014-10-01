
CREATE PROCEDURE [dbo].[AddUpdateChecksumMonitoringItem]
(	@ItemId int, @CheckInterval int, @Enabled bit, @Description varchar(200), 
	@ChecksumRootPath varchar(300), @ExtensionsToIgnore varchar(200), @RedCondition varchar(200) )
AS
BEGIN
	IF EXISTS (SELECT * FROM [ChecksumMonitoringItems] 
					 WHERE [ItemID] = @ItemId)

		--Update the configured monitoring item if it already exists		
		BEGIN
			UPDATE [ChecksumMonitoringItems] 	 
					 SET [CheckInterval] = @CheckInterval, [Enabled] = @Enabled, 
					 [ChecksumRootPath] = @ChecksumRootPath , [ExtensionsToIgnore] = @ExtensionsToIgnore, 
					 [RedCondition] = @RedCondition, [Description] = @Description
					 WHERE [ItemID] = @ItemId
		END
	ELSE
		--Insert the monitoring item
		BEGIN
			SET IDENTITY_INSERT [dbo].[ChecksumMonitoringItems] ON

		    INSERT INTO [dbo].[ChecksumMonitoringItems] 
			([ItemID],[CheckInterval],[Enabled],[Description],[ChecksumRootPath],[ExtensionsToIgnore],[RedCondition]) 
			VALUES (@ItemId,@CheckInterval,@Enabled,@Description,@ChecksumRootPath,@ExtensionsToIgnore,@RedCondition)

			SET IDENTITY_INSERT [dbo].[ChecksumMonitoringItems] OFF
		END
END
GO
