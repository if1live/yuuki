@set BIN_DIR=%~dp0
@set BASE_DIR=%BIN_DIR%..\
@set WIN_PROJ_DIR=%BASE_DIR%Client
@set ANDROID_PROJ_DIR=%BASE_DIR%ClientAndroid
@set WEB_PROJ_DIR=%BASE_DIR%ClientWeb

@rem http://docs.unity3d.com/Manual/CommandLineArguments.html

@set UNITY_PATH="c:\Program Files (x86)\Unity\Editor\Unity.exe"

"%VS120COMNTOOLS%\..\IDE\devenv.exe" /build Release %BASE_DIR%Yuuki.sln
%UNITY_PATH% -quit -batchmode -projectPath %WIN_PROJ_DIR% -executeMethod ScriptBatch.BuildWinGame
%UNITY_PATH% -quit -batchmode -projectPath %ANDROID_PROJ_DIR% -executeMethod ScriptBatch.BuildAndroidGame
%UNITY_PATH% -quit -batchmode -projectPath %WEB_PROJ_DIR% -executeMethod ScriptBatch.BuildWebGame
