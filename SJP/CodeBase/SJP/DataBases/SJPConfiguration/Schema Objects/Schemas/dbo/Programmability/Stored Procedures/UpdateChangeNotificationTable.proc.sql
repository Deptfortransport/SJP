CREATE PROCEDURE [dbo].[UpdateChangeNotificationTable]
( 
	@tableList varchar(1000) 
) 
AS 
BEGIN 

    CREATE TABLE #updateTables 
	(
		tablename varchar(100) COLLATE database_default NOT NULL
	)
			    
	DECLARE @currTable varchar(100), @pos int 
        
    SET @tableList = LTRIM(RTRIM(@tableList)) + ',' 
    SET @pos = CHARINDEX(',', @tableList, 1) 
        
    IF REPLACE(@tableList, ',', '') <> '' 
        BEGIN 
            WHILE @pos > 0 
                BEGIN 
                    SET @currTable = LTRIM(RTRIM(LEFT(@tableList, @pos - 1))) 
                    IF @currTable <> '' 
                       BEGIN 
                          INSERT INTO #updateTables VALUES (@currTable) 
                       END 
                    SET @tableList = RIGHT(@tableList, LEN(@tableList) - @pos) 
                    SET @pos = CHARINDEX(',', @tableList, 1) 
                END 
        END 

    BEGIN TRANSACTION

	-- Update tables that did exist
    UPDATE [dbo].[ChangeNotification]
    SET    [version] = [version] + 1 
    WHERE  [table] in 
            (SELECT tablename FROM #updateTables) 

    -- Add tables that didn't previously exist
    INSERT INTO [dbo].[ChangeNotification] ([table], [version])
    SELECT 		tablename, 1 
    FROM		#updateTables
    WHERE 		not EXISTS(SELECT * 
							 FROM [dbo].[ChangeNotification] CN
							WHERE CN.[Table] = #updateTables.tablename)
    
    COMMIT TRANSACTION
    
END 