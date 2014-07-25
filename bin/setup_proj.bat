@set BIN_DIR=%~dp0
@set BASE_DIR=%BIN_DIR%..\
@set WIN_PROJ_DIR=%BASE_DIR%\Client
@set ANDROID_PROJ_DIR=%BASE_DIR%\ClientAndroid
@set WEB_PROJ_DIR=%BASE_DIR%\ClientWeb

mkdir %ANDROID_PROJ_DIR%
mkdir %WEB_PROJ_DIR%

mklink /j %ANDROID_PROJ_DIR%\Assets %WIN_PROJ_DIR%\Assets
mklink /j %ANDROID_PROJ_DIR%\ProjectSettings %WIN_PROJ_DIR%\ProjectSettings

mklink /j %WEB_PROJ_DIR%\Assets %WIN_PROJ_DIR%\Assets
mklink /j %WEB_PROJ_DIR%\ProjectSettings %WIN_PROJ_DIR%\ProjectSettings
