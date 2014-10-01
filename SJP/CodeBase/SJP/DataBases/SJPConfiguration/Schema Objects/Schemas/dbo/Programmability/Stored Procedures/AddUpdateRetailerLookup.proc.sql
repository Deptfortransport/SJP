CREATE PROCEDURE [dbo].[AddUpdateRetailerLookup]
	@OperatorCode [char](10),
	@Mode [char](10),
	@RetailerId [char](20)
AS
BEGIN
	IF NOT EXISTS (SELECT * 
				     FROM [dbo].[RetailerLookup] 
					WHERE [RetailerId] = @RetailerId 
					  AND [Mode] = @Mode
					  AND [OperatorCode] = @OperatorCode) 			   
		BEGIN
			INSERT INTO [dbo].[RetailerLookup] ([OperatorCode], [Mode], [RetailerId])
				 VALUES (@OperatorCode, @Mode, @RetailerId)
		END
END