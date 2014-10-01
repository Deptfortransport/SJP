@ECHO OFF
REM ******************************************************************** 
REM Batch file used to simplify starting,
REM stoppping and testing the event receiver service.
REM Primarily to be used by the TNG control application.
REM ******************************************************************* 

IF "%1"=="test"  GOTO TEST 
IF "%1"=="start" GOTO START 
IF "%1"=="stop"  GOTO STOP
GOTO HELPMSG

:TEST
net start /test EventReceiver
GOTO END

:START
net start EventReceiver
GOTO END

:STOP
net stop EventReceiver
GOTO END


:HELPMSG

ECHO Command line usage :
ECHO     EventReceiver.bat  test
ECHO     EventReceiver.bat  start
ECHO     EventReceiver.bat  stop

:END
