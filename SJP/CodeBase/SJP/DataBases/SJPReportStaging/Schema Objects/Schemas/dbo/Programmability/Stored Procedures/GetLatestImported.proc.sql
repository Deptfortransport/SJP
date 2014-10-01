-- ---------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetLatestImported]
	@DataName varchar(50)
AS

	declare @RSDTID as int
    set @RSDTID = (Select RSDTID from ReportStagingDataType where RSDTName = @DataName)

Select 
	Max(RSDA.Event) as AuditDate
From 
	ReportStagingDataAudit RSDA
Where
	(RSDA.RSDATID =
	(SELECT RSDATID
	 FROM ReportStagingDataAuditType
	 WHERE RSDATName = 'LatestImported'))
	AND
	(RSDA.RSDTID = @RSDTID)