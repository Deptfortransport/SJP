CREATE PROCEDURE [dbo].[AddUpdateRetailer]
	@RetailerId [char](20),
	@Name [char](50),
	@WebsiteURL [char](300),
	@HandoffURL [char](300),
	@DisplayURL [char](100),
	@PhoneNumber [char](100),
	@PhoneNumberDisplay [char](100),
	@ResourceKey [char](100)
AS
BEGIN
	IF NOT EXISTS (SELECT * 
				     FROM [dbo].[Retailers] 
					WHERE [RetailerId] = @RetailerId) 			   
		BEGIN
			INSERT INTO [dbo].[Retailers] (RetailerId, Name, WebsiteURL, HandoffURL, DisplayURL, PhoneNumber, PhoneNumberDisplay, ResourceKey)
				 VALUES (@RetailerId, @Name, @WebsiteURL, @HandoffURL, @DisplayURL, @PhoneNumber, @PhoneNumberDisplay, @ResourceKey)
		END
	ELSE
		BEGIN
			UPDATE [dbo].[Retailers]
			   SET [Name] = @Name,
				   [WebsiteURL] = @WebsiteURL,
				   [HandoffURL] = @HandoffURL,
				   [DisplayURL] = @DisplayURL,
				   [PhoneNumber] = @PhoneNumber,
				   [PhoneNumberDisplay] = @PhoneNumberDisplay,
				   [ResourceKey] = @ResourceKey
			 WHERE [RetailerID] = @RetailerId
		END	

END