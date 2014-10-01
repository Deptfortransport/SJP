ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [AirInterchange_Log], FILENAME = 'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\AirInterchange_1.LDF', SIZE = 1024 KB, FILEGROWTH = 10 %);



