CREATE PROCEDURE [dbo].[AddChangeNotificationTable]
	@table varchar(100) 
AS
BEGIN

	-- Add tables that didn't previously exist
	IF NOT EXISTS (SELECT * FROM [dbo].[ChangeNotification]
						   WHERE [table] = @table)
		BEGIN
			INSERT INTO [dbo].[ChangeNotification] ([table], [version])
				 VALUES (@table, 1)
		END
END