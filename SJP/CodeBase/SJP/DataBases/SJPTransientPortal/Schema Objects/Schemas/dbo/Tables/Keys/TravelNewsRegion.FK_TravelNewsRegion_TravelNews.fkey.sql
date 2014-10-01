ALTER TABLE [dbo].[TravelNewsRegion]  
	ADD  CONSTRAINT [FK_TravelNewsRegion_TravelNews] 
	FOREIGN KEY([UID])
	REFERENCES [dbo].[TravelNews] ([UID])
