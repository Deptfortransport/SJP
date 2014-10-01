CREATE PROCEDURE [dbo].[GetChangeTable]
	
AS
BEGIN
	  SELECT [Table], [Version] 
	    FROM [dbo].[ChangeNotification]
	ORDER BY [Table]
END