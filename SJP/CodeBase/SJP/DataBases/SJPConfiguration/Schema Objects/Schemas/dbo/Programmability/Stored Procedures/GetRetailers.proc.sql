CREATE PROCEDURE [dbo].[GetRetailers]
AS
BEGIN
	SELECT [RetailerId],
		   [Name],
		   [WebsiteURL],
		   [HandoffURL],
		   [DisplayURL],
		   [PhoneNumber],
		   [PhoneNumberDisplay],
		   [ResourceKey]
	  FROM [dbo].[Retailers]
END