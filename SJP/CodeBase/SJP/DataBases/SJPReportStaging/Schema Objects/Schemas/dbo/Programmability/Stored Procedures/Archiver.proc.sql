
CREATE PROCEDURE [dbo].[Archiver] 
AS

DECLARE @tableList VARCHAR(2000),
        @auditList VARCHAR(2000),
	@auditDate DATETIME, 
	@tempDate DATETIME,
        @pos INT,
        @i INT,
        @currTable varchar(200),
        @deleteDate DATETIME,
        @daystokeep INT,
	@rowstodelete INT,
        @sqlStr VARCHAR(2000),
        @timeMon VARCHAR(25),
        @err INT,
        @row INT,
        @errortext VARCHAR(255)

SET @timeMon = getDate()

UPDATE ReportStagingProperties SET pValue = 'Running: Validation' WHERE pName = 'ReportDataArchiver.Status'

-- The number of days data not to delete.
-- The delete will be done upto the latest data date - the days to keep
SELECT @daystokeep=pValue FROM ReportStagingProperties WHERE PName = 'ReportDataArchiver.DaysToKeep'

-- The number of rows to be deleted before each commit
SELECT @rowstodelete=pValue FROM ReportStagingProperties WHERE PName = 'ReportDataArchiver.RowsToDelete'

-- Catch any errors
SET @err = @@error

IF @err <> 0
BEGIN

  raiserror ('Report Staging Archiver: Archive failed, unable to access ReportStagingProperties Table.', 16, 1) WITH LOG
  UPDATE ReportStagingProperties SET pValue = 'Not Running' WHERE pName = 'ReportDataArchiver.Status'
  RETURN

END

-- Checks that all the last imported dates in the report data audit table are equal
-- This comfirms the importer completed sucessfully and give the date of the data that was
-- last imported.

-- Temporary Table
CREATE TABLE ##tempAuditDate (
       dataDate DATETIME NOT NULL
       )

INSERT INTO ##tempAuditDate (dataDate) VALUES (0)
SET @auditDate = 0

SELECT @auditList=pValue FROM ReportStagingProperties WHERE PName = 'ReportDataArchiver.AuditId'

WHILE 1=1
BEGIN
  
  SET @tempDate = 0

  IF len(@auditList) = 0 BREAK

  SET @pos = CharIndex (';',@auditList)

  -- If no ; on the end of the last entry.
  IF @pos = 0 
  BEGIN

    SET @i = @auditList
    SET @auditList = ''

  END
  ELSE
  BEGIN

    SET @i = substring (@auditList, 1, len(@auditList) - ( len(@auditList) - @pos) - 1 )
    SET @auditList = substring (@auditList, @pos + 1,len(@auditList) - @pos )

  END

  EXEC('UPDATE ##tempAuditDate SET dataDate = (SELECT CONVERT(datetime, convert(char(10),event,105),105) FROM ReportStagingDataAudit
                                      WHERE RSDATID = 2
                                      AND RSDTID = ' + @i + ')')

  SET @err = @@error

  IF @err <> 0
  BEGIN
    DROP TABLE ##tempAuditDate

    IF (@err = 229)
    BEGIN
      SET @errortext = 'Report Staging Archiver: Archive failed, unable to access ReportStagingDataAudit Table.'
    END
    ELSE IF (@err = 515)
    BEGIN
      SET @errortext = 'Report Staging Archiver: Archive failed, incorrect value in the ReportDataArchiver.AuditId field in the ReportStagingProperties Table.'
    END
    ELSE
    BEGIN
      SET @errortext = 'Report Staging Archiver: Unknown error in validation.'
    END

    raiserror (@errortext, 16, 1) WITH LOG
    UPDATE ReportStagingProperties SET pValue = 'Not Running' WHERE pName = 'ReportDataArchiver.Status'
    RETURN

  END

  SELECT @tempDate=dataDate FROM ##tempAuditDate

  IF (@auditDate = 0)
  BEGIN

    SET @auditDate = @tempDate

  END

  ELSE  IF (@auditDate <> @tempDate )
  BEGIN 

    DROP TABLE ##tempAuditDate
    SET @errortext = 'Report Staging Archiver: Validation failed due to last imported dates in the ReportStagingDataAudit table.'
    raiserror (@errortext, 16, 1) WITH LOG
    UPDATE ReportStagingProperties SET pValue = 'Not Running' WHERE pName = 'ReportDataArchiver.Status'
    RETURN

  END

END

DROP TABLE ##tempAuditDate


---------------------------------------------------------------
---------------- Perform the actual delete ---------------------
----------------------------------------------------------------

UPDATE ReportStagingProperties SET pValue = 'Running: Starting Deletion' WHERE pName = 'ReportDataArchiver.Status'

-- Gets the list of tables to be deleted from
SELECT @tableList=pValue FROM ReportStagingProperties WHERE PName = 'ReportDataArchiver.Tables'

SET @i = 1
SET @deleteDate = @auditDate-@daystokeep

WHILE 1=1
BEGIN

  IF len(@tableList) = 0 BREAK

  SET @pos = CharIndex (';',@tableList)

  -- If no ; on the end of the last entry.
  IF @pos = 0 
  BEGIN

    SET @currTable = @tableList
    SET @tableList = ''

  END
  -- Sets currTable to be each of the table names in turn
  ELSE
  BEGIN

    SET @currTable = substring (@tableList, 1, len(@tableList) - ( len(@tableList) - @pos) - 1 )
    SET @tableList = substring (@tableList, @pos + 1,len(@tableList) - @pos )

  END

  EXEC('UPDATE ReportStagingProperties SET pValue = ''Running: Deleting From ' + @currTable + ''' WHERE pName = ''ReportDataArchiver.Status''')
  SET @errortext = 'Deleting From ' + @currTable
  raiserror (@errortext, 1, 1)

  -- Delete '@rowstodelete' number of rows and commit.  Continue looping until no rows are committed.
  WHILE 1=1 BEGIN

    BEGIN TRAN

      SET @sqlStr = ('DELETE TOP ('+ CONVERT(VARCHAR, @rowstodelete) +') FROM ' + @currTable + '
                        WHERE TimeLogged < CONVERT(DATETIME, ''' + CONVERT(VARCHAR(20), @deleteDate) + ''', 105)' ) 

      EXEC (@sqlStr)

      SELECT @err = @@error, @row = @@rowcount

      IF (@err <> 0)
      BEGIN

        SET @errortext = 'Report Staging Archiver: Unable to access table ' + @currTable
        raiserror (@errortext, 16, 1) WITH LOG
        UPDATE ReportStagingProperties SET pValue = 'Not Running' WHERE pName = 'ReportDataArchiver.Status'
        RETURN

      END

      IF (@row = 0) BREAK 

    COMMIT TRAN

  END

  COMMIT TRAN


END

UPDATE ReportStagingProperties SET pValue = 'Running: Completing' WHERE pName = 'ReportDataArchiver.Status'

-- Update the properties table
EXEC('UPDATE ReportStagingProperties SET pValue = getdate()
      WHERE pName = ''ReportDataArchiver.LastSuccess''')

raiserror ('Report Staging Archiver: Completed successfully', 1, 1) WITH LOG

SET @timeMon = DATEDIFF(mi, convert(datetime,@timeMon,100), getDate())

EXEC('UPDATE ReportStagingProperties SET pValue = ''' + @timeMon + ''' WHERE pName = ''ReportDataArchiver.LastRunTime''')

UPDATE ReportStagingProperties SET pValue = 'Not Running' WHERE pName = 'ReportDataArchiver.Status'



----------------------------------------------------------------
-- Update change catalogue
----------------------------------------------------------------