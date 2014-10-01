--SJPTransientPortal Clean up

-- SQL removes all the temporary data from the SJPTransientPortal Travel News tables

USE [SJPTransientPortal]

DELETE FROM [dbo].[TravelNewsVenue]
      WHERE [UID] like 'SJP%'

DELETE FROM [dbo].[TravelNewsRegion]
      WHERE [RegionName] like 'SJPTest%'

DELETE FROM [dbo].[TravelNewsDataSource]
      WHERE [DataSourceId] like 'SJPTest%'

DELETE FROM [dbo].[TravelNewsDataSources]
      WHERE [DataSourceId] like 'SJPTest%'

DELETE FROM [dbo].[TravelNews]
      WHERE [HeadlineText] like 'SJPTest%'
	    AND [DetailText] like 'SJPTest%'
