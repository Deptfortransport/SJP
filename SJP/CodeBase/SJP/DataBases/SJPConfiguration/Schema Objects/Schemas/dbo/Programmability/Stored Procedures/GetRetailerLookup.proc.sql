CREATE PROCEDURE [dbo].[GetRetailerLookup]
AS
BEGIN
	SELECT 	[OperatorCode], 
		    [Mode], 
			[RetailerId]
	  FROM	[dbo].[RetailerLookup]		
END