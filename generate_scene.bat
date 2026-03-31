@echo off
REM Script to run Unity in batch mode and generate the complete game scene

set UNITY_PATH=C:\Program Files\Unity\Hub\Editor\6000.3.9f1\Editor\Unity.exe
set PROJECT_PATH=D:\PROJECTS\UNITY\chor-police\chor-police.sln

echo Starting Unity in batch mode...
echo Unity Path: %UNITY_PATH%
echo Project Path: %PROJECT_PATH%

"%UNITY_PATH%" -quit -batchmode -nographics -projectPath "D:\PROJECTS\UNITY\chor-police" -executeMethod CompleteSceneGenerator.GenerateCompleteGameScene -logFile "generate_scene.log"

echo Scene generation complete!
pause
