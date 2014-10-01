ALTER TABLE [dbo].[RetailerLookup]
	ADD CONSTRAINT [CK_RetailerLookup] 
	CHECK  (([Mode] = 'Underground' or ([Mode] = 'Tram' or ([Mode] = 'Rail' or ([Mode] = 'Metro' or ([Mode] = 'Ferry' or ([Mode] = 'Drt' or ([Mode] = 'Bus' or ([Mode] = 'Coach' or [Mode] = 'Air')))))))))

