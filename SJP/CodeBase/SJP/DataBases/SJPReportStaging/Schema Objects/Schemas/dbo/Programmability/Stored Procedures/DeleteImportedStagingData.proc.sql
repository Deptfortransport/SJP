CREATE PROCEDURE [dbo].[DeleteImportedStagingData]
	@StagingTableName varchar(30),
	@LatestImported DATETIME
AS

DECLARE @sqlStr VARCHAR (400),
        @count INT
        
SET @count = 0

WHILE @count < 15 BEGIN

  SET @sqlStr    = 'DELETE FROM ' + @StagingTableName + ' 
                    WHERE CONVERT( DATETIME, CONVERT( VARCHAR(10), TimeLogged, 110 ) ) <= 
                          CONVERT( DATETIME, ''' + (CONVERT( VARCHAR(10), (@LatestImported - @count), 110 )  ) + ''')
                    AND CONVERT( DATETIME, CONVERT( VARCHAR(10), TimeLogged, 110 ) ) > 
                        CONVERT( DATETIME, ''' + (CONVERT( VARCHAR(10), (@LatestImported- (@count+1)), 110 )  ) + ''')'

  EXEC( @sqlStr )

  --IF @@rowcount = 0 BREAK

  SET @count = @count +1

END