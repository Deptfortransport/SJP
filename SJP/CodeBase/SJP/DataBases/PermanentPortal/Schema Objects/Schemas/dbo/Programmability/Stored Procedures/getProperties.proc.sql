
CREATE PROCEDURE getProperties @AID char(50), @GID char(50), @optionalpname varchar(255) = null
AS

--create temporary copy of the properties from each of the properties stored procs
CREATE TABLE #propapplicationId
(
	pname varchar(500),pvalue varchar(2000)
)
INSERT INTO #propapplicationId 
EXEC SelectApplicationPropertiesWithPartnerId @AID

CREATE TABLE #propgroupId
(
	pname varchar(500),pvalue varchar(2000)
)
INSERT INTO #propgroupId 
EXEC SelectGroupPropertiesWithPartnerId @GID

CREATE TABLE #propglobal
(
	pname varchar(500),pvalue varchar(2000)
)
INSERT INTO #propglobal 
EXEC SelectGlobalPropertiesWithPartnerId

--create temp table to hold the properties for the provided AID and GID
CREATE TABLE #properties
(
	pname varchar(500),pvalue varchar(2000)
)

DECLARE @pname varchar(500), @pvalue varchar(2000)

DECLARE cursorapplicationId CURSOR FOR SELECT * from #propapplicationId
OPEN cursorapplicationId
FETCH NEXT FROM cursorapplicationId INTO @pname, @pvalue

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	--remove duplicate pname (case sensitive collation) and insert into temp properties table
	IF not exists(SELECT pname FROM #properties WHERE pname COLLATE SQL_Latin1_General_CP1_CS_AS = @pname)
	BEGIN
		INSERT INTO #properties (pname, pvalue)
		VALUES( @pname, @pvalue)
	END

	FETCH NEXT FROM cursorapplicationId INTO @pname, @pvalue
END

CLOSE cursorapplicationId
DEALLOCATE cursorapplicationId 

DECLARE cursorgroupId CURSOR FOR SELECT * from #propgroupId
OPEN cursorgroupId
FETCH NEXT FROM cursorgroupId INTO @pname, @pvalue

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	--remove duplicate pname (case sensitive collation) and insert into temp properties table
	IF not exists(SELECT pname FROM #properties WHERE pname COLLATE SQL_Latin1_General_CP1_CS_AS = @pname)
	BEGIN
		INSERT INTO #properties (pname, pvalue)
		VALUES( @pname, @pvalue)
	END

	FETCH NEXT FROM cursorgroupId INTO @pname, @pvalue
END

CLOSE cursorgroupId
DEALLOCATE cursorgroupId 


DECLARE cursorglobal CURSOR FOR SELECT * from #propglobal
OPEN cursorglobal
FETCH NEXT FROM cursorglobal INTO @pname, @pvalue

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	--remove duplicate pname (case sensitive collation) and insert into temp properties table
	IF not exists(SELECT pname FROM #properties WHERE pname COLLATE SQL_Latin1_General_CP1_CS_AS = @pname)
	BEGIN
		INSERT INTO #properties (pname, pvalue)
		VALUES( @pname, @pvalue)
	END

	FETCH NEXT FROM cursorglobal INTO @pname, @pvalue
END

CLOSE cursorglobal
DEALLOCATE cursorglobal 

--check with optional pname parameter supplied
IF (@optionalpname IS NULL)
BEGIN
	-- select all rows
	SELECT pname, pvalue
	FROM #properties
END
ELSE
BEGIN
	-- select properties that match pname parameter
	SELECT pname, pvalue
	FROM #properties
	WHERE pname LIKE '%' + @optionalpname
END

-- tidy up temp tables
DROP TABLE #propapplicationId
DROP TABLE #propgroupId
DROP TABLE #propglobal
DROP TABLE #properties