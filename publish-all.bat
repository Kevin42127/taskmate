@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    TodoApp 多平台发布脚本
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

echo 开始发布...
echo.

echo [1/6] 清理旧的发布文件...
for /d %%d in (publish\*) do rmdir /s /q "%%d"

echo [2/6] 发布 Windows x64...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64 -p:PublishTrimmed=false --verbosity minimal
if errorlevel 1 goto :error

echo [3/6] 发布 Windows x86...
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x86 -p:PublishTrimmed=false --verbosity minimal
if errorlevel 1 goto :error

echo [4/6] 发布 Linux x64...
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/linux-x64 -p:PublishTrimmed=false --verbosity minimal
if errorlevel 1 goto :error

echo [5/6] 发布 macOS x64...
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/osx-x64 -p:PublishTrimmed=false --verbosity minimal
if errorlevel 1 goto :error

echo [6/6] 发布 Linux ARM64...
dotnet publish -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true -o ./publish/linux-arm64 -p:PublishTrimmed=false --verbosity minimal
if errorlevel 1 goto :error

echo.
echo ========================================
echo           发布完成！
echo ========================================
echo.

REM 获取文件大小信息
set total_size=0
for /r %%d in (publish\*) do (
    for %%f in ("%%d\TodoApp*") do (
        if exist "%%f" (
            for %%I in ("%%f") do set /a "total_size+=%%~zI"
        )
    )
)

REM 显示发布结果
echo 发布信息：
echo   发布时间: %date% %time%
echo.
echo 发布文件：
for /d %%d in (publish\*) do (
    echo   %%d:
    for %%f in ("%%d\*") do echo     - %%~nxf
)
echo.
echo 总文件大小: %total_size% 字节
echo.
echo 平台说明：
echo   Windows x64: 适用于大多数 Windows 系统
echo   Windows x86: 适用于 32 位 Windows 系统
echo   Linux x64:   适用于大多数 Linux 发行版
echo   macOS x64:   适用于 Intel Mac
echo   Linux ARM64: 适用于 ARM Linux 设备
echo.
echo 推荐分发：根据目标用户选择对应平台
echo.

REM 询问是否打开发布文件夹
set /p "open_folder=是否打开发布文件夹? (Y/N): "
if /i "%open_folder%"=="Y" start "" "publish"
goto :end

:error
echo.
echo [错误] 发布过程中出现错误，请检查上面的错误信息
pause
exit /b 1

:end
echo.
echo 发布脚本执行完成
pause
