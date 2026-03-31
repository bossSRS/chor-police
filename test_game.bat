@echo off
REM Script to run Unity and play the Chor Police game

set UNITY_PATH=C:\Program Files\Unity\Hub\Editor\6000.3.9f1\Editor\Unity.exe
set PROJECT_PATH=D:\PROJECTS\UNITY\chor-police

echo Starting Unity and opening Chor Police game...
echo Unity Path: %UNITY_PATH%
echo Project Path: %PROJECT_PATH%

"%UNITY_PATH%" -projectPath "%PROJECT_PATH%" -executeMethod PlayTestRunner.RunPlayTest -logFile "play_test.log"

pause
