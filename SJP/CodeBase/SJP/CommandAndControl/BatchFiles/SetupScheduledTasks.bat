echo off
REM ................................................................................!
REM                                                                                 !
REM                                                                                 !
REM Setup Scheduled Tasks                                                           !
REM                                                                                 !
REM Version 1.0                                                                     !
REM Created 21/03/2011 By RB                                                        !  
REM                                                                                 !   
REM                                                                                 !
REM This script sets up several scheduled tasks from the xml scripts referenced.    !
REM Please make sure the <UserId> used in the scripts is set up on the target server!
REM and has "Logon as batch job" privilege in 					    !
REM - "Windows Settings\Security Settings\Local Policies\User Rights Assignment\"   !
REM NB TASKS ARE CREATED USING FILENAME (LESS EXTENSION) AS TASK NAME		    !
REM ................................................................................!
REM Date     !  Version  ! Description	                      	
REM 13/03    !  1.0      ! original                                
REM          !           !         	                 

set currDate=
set currTime=
set time=
set timenow=
set date=
set targetSystem="localhost"

set retval=

echo 1 - Setting up date and time for log file name

REM get todays date for use later
	for /F "tokens=1-4 delims=/ " %%i in ('date /t') do (
	rem set Day=%%j
	rem set Month=%%k
	rem set Year=%%l
	set currDate=%%l%%k%%j
	)

REM get current time for use later

	for /F "tokens=1-2 delims=: " %%i in ('time /t') do (
	set xcopyhour=%%i
	set xcopymins=%%j
	set currTime=%%i%%j
	)

set logfile=SetupScheduledTasks_%computername%_%currdate%%currTime%.log
echo 1 - Finished setting up date and time for log file name >> %logfile%

if not "%1"=="" ( 
set targetSystem= %1
)
echo target system = %targetSystem%  >> %logfile%

echo 2 - For each scheduled task xml setup file in the ScheduledTasks\ subfolder, setup the task using filename as taskname >> %logfile%
echo 3 - For each scheduled task xml setup file in the ScheduledTasks\ subfolder, setup the task using filename as taskname

for %%a IN (ScheduledTasks\*.xml) do (
	echo Deleting %%~na
	echo Deleting %%~na  >> %logfile%
	SCHTASKS /Delete /S %targetSystem% /TN %%~na /F  >> %logfile%

	echo Creating %%~na
	echo Creating %%~na >> %logfile%
	SCHTASKS /Create /S %targetSystem% /TN %%~na /F /RU "sjp_user" /RP "!password!1" /XML %%a  >> %logfile%
)

pause
