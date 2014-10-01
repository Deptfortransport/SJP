echo off
::.................................................................................!
:: Spectator Journey Planner                                                          !
::                                                                                 !
:: process webserver logs                                                          !
::                                                                                 !
:: Version 1.0                                                                     !
:: Created 25/09/2009 By SC                                                        !
::                                                                                 !   
:: INPUTS                                                                          !
:: None specific but relies on the userdomain environment variable                 !
::										   !
:: Internal                                                                        !
::                                                                                 !
:: OUPUTS                                                                          !
:: None                                                                            !
::                                                                                 !
::                                                                                 !
::.................................................................................!
:: Date     !  Version  ! Description	                                           !   	
:: 25/09    !  1.0      ! ORIGINAL                                                 !
::          !  1.1      ! updated to append server name on zipped files 	   !
::          !           !                   					   !
::          !           !                   					   !
::          !           !                   					   !
:: 

echo starting new WLR process
echo %date% %time% starting new WLR batch process >> D:\tdportal\Run_WLR.log
:: set server list for each environment
if %userdomain%==BBPTDPSIS ( 	set slist=w01 w02 
				set sdom=bbptdpsiw)
if %userdomain%==BBPTDPS ( 	set slist=w03 
				set sdom=bbptdpw)
if %userdomain%==ACPTDPS ( 	set slist=w01 w02 w03 w04 
				set sdom=acptdpw)

:: set other variables

echo server list = %slist% 
echo server domain is = %sdom%

echo work out required archive month

for /F "tokens=1-4 delims=/ " %%i in ('date /t') do ( set day=%%i 
set month=%%j 
set year=%%k
)

::Build temporary archive calendar file
:: the single > is reqd to create a new file in case one still exists
:: columns #1 current month number, #2 current month name, #3 archive month name
echo 01,jan,sep > archive_list.tmp 
echo 02,feb,oct >> archive_list.tmp
echo 03,mar,nov >> archive_list.tmp
echo 04,apr,dec >> archive_list.tmp
echo 05,may,jan >> archive_list.tmp
echo 06,jun,feb >> archive_list.tmp
echo 07,jul,mar >> archive_list.tmp
echo 08,aug,apr >> archive_list.tmp
echo 09,sep,may >> archive_list.tmp
echo 10,oct,jun >> archive_list.tmp
echo 11,nov,jul >> archive_list.tmp
echo 12,dec,aug >> archive_list.tmp

:: Loop and find archive month
for /f "tokens=1,2,3 delims=," %%x IN (archive_list.tmp) do ( 
if %%x==%month% ( 
set cur_mon=%%y
set arc_mon=%%z
goto match)
)
echo error no archive month found!
goto end

:match 
echo For the current month number of %month% with name of %cur_mon% the archive month is %arc_mon%
del archive_list.tmp

echo entering main section 

for %%a IN (%slist%) do (

echo 1 
	echo webserver to be processed=%%a
	REM following loop to rename files for transfer, get list except newest file
	REM this uses the Dir /B /O-D to get a bare list sorted by date - newest first then
	REM uses skip=1 to miss off the top most file....

	echo renaming of files underway... 
	for /f "skip=1" %%b IN ('dir /B /O-D \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\*.log') do (		
		REM label files ready for transfer
		ren \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%~nb%%~xb %%~nb%%~xb.ready_for_transfer
	)
	echo renaming of files completed 
echo 2
	REM TRANSFER Files and rename 
	echo transfer and renaming of files underway... 

	for /f %%c IN ('dir /B /O-D \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\*.ready_for_transfer') do (		
		REM rename remote file to indicate transfer in progress	
		ren \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%c %%~nc.transferring	
		REM copy files
		xcopy "\\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%~nc.transferring" D:\WebIISlogs\%%alogs\ /Q /Y >> templog.txt
		REM label newly arrived files after the transfer to indicate status
		ren D:\WebIISlogs\%%alogs\%%~nc.transferring %%~nc.copied_from_server
		REM label remote file to indicate transfer completed
		ren \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%~nc.transferring %%~nc.transferred
 	)
	del templog.txt
echo 3
	echo transfer and renaming files completed 

	REM Preparing files to be run in by WLR
	echo Preparing files to be run in by WLR...
	for /f %%c IN ('dir /B /O-D D:\WebIISlogs\%%alogs\*.copied_from_server') do ( 		
		REM label newly arrived files after the transfer to indicate status
  		ren D:\WebIISlogs\%%alogs\%%~nc.copied_from_server %%~nc
	)
	echo Preparing files to be run in by WLR completed
echo 4
	echo run of WLR commencing ....
	D:\TDPortal\Components\weblogreader\td.reportdataprovider.%%a.weblogreader.exe
	REM check all files processed 
	echo run of WLR finished.
echo 5 
	if exist D:\WebIISlogs\%%alogs\*.log goto error
echo 6
REM echo on
echo end of web servers outer loop for processing file to WLR
) 
echo entering archiving and compression phase

for %%a IN (%slist%) do ( echo server list=%%a )
echo 6a
for %%a IN (%slist%) do ( echo server list inner loop =%%a .
echo 6b .
	REM rename remote files as processed including month in extension for archiving purposes
	REM using files in the archive folder update remote files to mark them as processed
	REM Archive month name is %arc_mon%
	IF EXIST D:\WebIISlogs\%%alogs\archive\*.log (
		for /f %%c IN ('dir /B /O-D D:\WebIISlogs\%%alogs\archive\*.log') do ( 		
			REM label remote log files as ready for archive to indicate processed status
			ren \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%c.transferred %%c.archive_in_%arc_mon%
		)
	)
echo 7

	REM loop through and compress the processed log files	
	echo starting compression > D:\WebIISlogs\%%aLogs\Archive\compression.log
	for /f  %%b IN ('dir /B /O-D \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\*.archive_in_%arc_mon%') do (
		REM about to compress full=%%b name only=%%~nb
		if not exist D:\WebIISlogs\%%alogs\archive\%%~nb.%%a.zip (
			"C:\Program Files\7-Zip\7z.exe" a D:\WebIISlogs\%%alogs\archive\%%~nb.%%a.zip D:\WebIISlogs\%%alogs\archive\%%~nb >> D:\WebIISlogs\%%alogs\archive\compression.log
			del D:\WebIISlogs\%%aLogs\Archive\%%~nb >> D:\WebIISlogs\%%alogs\archive\compression.log
		)	
	REM del D:\WebIISlogs\%%aLogs\Archive\%%~nb.zip
	REM del D:\WebIISlogs\%%aLogs\Archive\%%~nb
	)
echo 8

REM removed section because server name now in compressed file name
REM	echo remove uncompressed copies of the previously compressed files	
REM	for %%b IN ("D:\WebIISlogs\%%aLogs\Archive\"*.zip) do (
REM		if exist D:\WebIISlogs\%%aLogs\Archive\%%~nb (
REM				REM deleting full=%%b target %%~nb.log
REM				del D:\WebIISlogs\%%aLogs\Archive\%%~nb >> D:\WebIISlogs\%%alogs\archive\compression.log
REM				)
REM	)

echo 9
) 
REM end of loop webserver loop
REM jump to end added for testing.
goto steve


	REM compress remote files that have been given an archive date
	echo starting remote compression > D:\WebIISlogs\%%aLogs\Archive\rcompression.log	
	for /f %%b IN ( 'dir /B /O-D \\%%a.%sdom%.local\Info\Logs\LogFiles\w3SVC1\*.archive_in_???' ) do (
		"C:\Program Files\7-Zip\7z.exe" a \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%b.zip \\%%a.%sdom%.local\Info\Logs\LogFiles\W3SVC1\%%b >> D:\WebIISlogs\%%alogs\archive\rcompression.log
	)
echo 10
::echo on

	echo deletion of remote files that have been now been zipped starting....
	for /f %%b IN ( 'dir /B /O-D \\%%a.%sdom%.local\Info\Logs\LogFiles\w3SVC1\*.archive_in_???.zip' ) do (
		REM full name=%%b file to be deleted=%%~nb
		IF EXIST '\\%%a.%sdom%.local\Info\Logs\LogFiles\w3SVC1\%%~nb' (
			echo deleting....
			del /Q \\%%a.%sdom%.local\Info\Logs\LogFiles\w3SVC1\%%~nb 
			echo deleted 
		)
	)
	echo delete remote files that have been now been zipped completed

echo 11
echo delete any remote files at the archive date 

:steve
echo end of archiving and compression section
) 

goto end

:error
echo an error has occured
echo an error has occured >> D:\tdportal\Run_WLR.log

goto end

:end
echo %date% %time% completed WLR batch process >> D:\tdportal\Run_WLR.log
