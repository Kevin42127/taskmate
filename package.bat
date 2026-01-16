@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    TodoApp 打包脚本
echo ========================================
echo.

REM 检查发布目录是否存在
if not exist "publish" (
    echo [错误] 未找到发布目录，请先运行发布脚本
    echo 运行命令: publish-all.bat
    pause
    exit /b 1
)

REM 创建打包目录
if not exist "packages" mkdir packages
if not exist "packages\archives" mkdir packages\archives

echo 开始打包...
echo.

REM 获取当前日期作为版本号
for /f "tokens=2 delims==" %%a in ('wmic os get localdatetime ^| find "DateTime"') do set "build_date=%%b"
set version_date=%build_date:~0,4%%build_date:~5,2%%build_date:~8,2%

echo [1/4] 打包 Windows x64...
cd publish\win-x64
if exist "TodoApp.exe" (
    ..\..\packages\archives\TodoApp-Windows-x64-%version_date%.zip" TodoApp.exe
    echo   创建: TodoApp-Windows-x64-%version_date%.zip
) else (
    echo   [警告] 未找到 TodoApp.exe
)

echo [2/4] 打包 Windows x86...
cd ..\win-x86
if exist "TodoApp.exe" (
    ..\..\packages\archives\TodoApp-Windows-x86-%version_date%.zip" TodoApp.exe
    echo   创建: TodoApp-Windows-x86-%version_date%.zip
) else (
    echo   [警告] 未找到 TodoApp.exe
)

echo [3/4] 打包 Linux x64...
cd ..\linux-x64
if exist "TodoApp" (
    ..\..\packages\archives\TodoApp-Linux-x64-%version_date%.tar.gz" TodoApp
    echo   创建: TodoApp-Linux-x64-%version_date%.tar.gz
) else (
    echo   [警告] 未找到 TodoApp
)

echo [4/4] 打包 macOS...
cd ..\osx-x64
if exist "TodoApp" (
    ..\..\packages\archives\TodoApp-macOS-%version_date%.tar.gz" TodoApp
    echo   创建: TodoApp-macOS-%version_date%.tar.gz
) else (
    echo   [警告] 未找到 TodoApp
)

REM 返回到根目录
cd ..\..

echo.
echo ========================================
echo           打包完成！
echo ========================================
echo.

REM 显示打包结果
echo 打包文件：
for %%f in (packages\archives\*.zip packages\archives\*.tar.gz) do echo   %%~nxf

echo.
echo 文件位置：packages\archives\
echo.
echo 说明：
echo   - Windows 版本为 ZIP 格式
echo   - Linux 和 macOS 版本为 TAR.GZ 格式
echo   - 版本号格式：YYYY-MM-DD
echo.

pause
