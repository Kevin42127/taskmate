@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    TodoApp 发布脚本
echo ========================================
echo.

REM 检查 .NET SDK
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo [错误] 未找到 .NET SDK，请先安装 .NET 8.0 SDK
    echo 下载地址: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

REM 创建发布目录
if not exist "publish" mkdir publish
if not exist "publish\logs" mkdir publish\logs

echo [1/3] 清理旧的发布文件...
if exist "publish\win-x64" rmdir /s /q "publish\win-x64"
if exist "publish\portable" rmdir /s /q "publish\portable"

echo [2/3] 发布自包含单文件版本...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64 -p:PublishTrimmed=false --verbosity normal
if errorlevel 1 (
    echo [错误] 发布失败，请检查错误信息
    pause
    exit /b 1
)

echo [3/3] 发布便携版本...
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish/portable --verbosity normal
if errorlevel 1 (
    echo [错误] 发布失败，请检查错误信息
    pause
    exit /b 1
)

REM 获取文件大小信息
for %%F in ("publish\win-x64\TodoApp.exe") do set "exe_size=%%~zF"
for %%F in ("publish\portable\TodoApp.exe") do set "portable_size=%%~zF"

echo.
echo ========================================
echo           发布完成！
echo ========================================
echo.
echo 发布信息：
echo   发布时间: %date% %time%
echo   自包含版: publish\win-x64\TodoApp.exe
echo   文件大小: %exe_size% 字节
echo   便携版:   publish\portable\TodoApp.exe
echo   文件大小: %portable_size% 字节
echo.
echo 使用说明：
echo   1. 自包含版：可直接运行，无需安装 .NET 运行时
echo   2. 便携版：需要目标机器安装 .NET 8.0 运行时
echo.
echo 推荐分发：自包含版
echo.

REM 询问是否打开发布文件夹
set /p "open_folder=是否打开发布文件夹? (Y/N): "
if /i "%open_folder%"=="Y" (
    start "" "publish\win-x64"
)

echo.
pause
