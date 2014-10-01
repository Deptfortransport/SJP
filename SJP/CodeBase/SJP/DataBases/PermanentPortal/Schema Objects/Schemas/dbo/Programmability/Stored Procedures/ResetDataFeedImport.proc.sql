CREATE PROCEDURE [dbo].[ResetDataFeedImport]
AS
BEGIN
	-- Resets the last imported date time for data feeds
	UPDATE [dbo].[FTP_CONFIGURATION]
       SET DATA_FEED_DATETIME = CAST('2000-01-01' AS DATETIME)
END
RETURN 0