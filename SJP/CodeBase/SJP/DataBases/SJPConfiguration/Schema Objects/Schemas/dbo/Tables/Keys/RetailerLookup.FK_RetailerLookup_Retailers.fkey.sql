ALTER TABLE [dbo].[RetailerLookup]
	ADD CONSTRAINT [FK_RetailerLookup_Retailers] 
	   FOREIGN KEY ([RetailerId])
	    REFERENCES [dbo].[Retailers] ([RetailerId])