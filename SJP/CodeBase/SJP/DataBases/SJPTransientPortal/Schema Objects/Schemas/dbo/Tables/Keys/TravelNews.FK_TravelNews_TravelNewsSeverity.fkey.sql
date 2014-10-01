ALTER TABLE [dbo].[TravelNews]  
	ADD  CONSTRAINT [FK_TravelNews_TravelNewsSeverity] 
	FOREIGN KEY([SeverityLevel])
	REFERENCES [dbo].[TravelNewsSeverity] ([SeverityID])
