ALTER TABLE [dbo].[TravelNewsDataSource]  
	ADD  CONSTRAINT [FK_TravelNewsDataSource_TravelNews] 
	FOREIGN KEY([UID])
	REFERENCES [dbo].[TravelNews] ([UID])
