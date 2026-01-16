@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    TodoApp 安装程序生成脚本
echo ========================================
echo.

REM 检查发布目录
if not exist "publish\win-x64" (
    echo [错误] 未找到发布文件，请先运行发布脚本
    echo 运行命令: publish.bat
    pause
    exit /b 1
)

REM 创建安装程序目录
if not exist "installer" mkdir installer

echo [1/3] 检查 Inno Setup 安装...
REM 检查多个可能的安装位置
set "INNO_PATH="
if exist "C:\Program Files (x86)\Inno Setup 6\iscc.exe" (
    set "INNO_PATH=C:\Program Files (x86)\Inno Setup 6\iscc.exe"
) else if exist "C:\Program Files\Inno Setup 6\iscc.exe" (
    set "INNO_PATH=C:\Program Files\Inno Setup 6\iscc.exe"
) else if exist "E:\.NET\Inno Setup 6\iscc.exe" (
    set "INNO_PATH=E:\.NET\Inno Setup 6\iscc.exe"
) else (
    echo [错误] 未找到 Inno Setup
    echo.
    echo 请确认 Inno Setup 安装在以下位置之一：
    echo   - C:\Program Files (x86)\Inno Setup 6\
    echo   - C:\Program Files\Inno Setup 6\
    echo   - E:\.NET\Inno Setup 6\
    echo.
    echo 或手动指定 iscc.exe 的完整路径
    pause
    exit /b 1
)

echo [2/3] 清理旧的安装程序...
if exist "installer\TodoApp-Setup.exe" del "installer\TodoApp-Setup.exe"

echo [3/3] 编译安装程序...
echo 正在编译 setup.iss...
echo 使用编译器: %INNO_PATH%
"%INNO_PATH%" "setup.iss" /Q

if errorlevel 1 (
    echo [错误] 编译失败，请检查 setup.iss 文件
    pause
    exit /b 1
)

REM 检查生成的文件
if exist "installer\TodoApp-Setup.exe" (
    echo.
    echo ========================================
    echo           安装程序生成成功！
    echo ========================================
    echo.
    
    REM 获取文件大小
    for %%F in ("installer\TodoApp-Setup.exe") do set "setup_size=%%~zF"
    
    echo 安装程序信息：
    echo   文件名: TodoApp-Setup.exe
    echo   位置:   installer\TodoApp-Setup.exe
    echo   大小:   !setup_size! 字节
    echo.
    echo 使用说明：
    echo   1. 双击 TodoApp-Setup.exe
    echo   2. 按照安装向导进行安装
    echo   3. 安装完成后可在开始菜单找到"待辦事項管理器"
    echo.
    
    REM 询问是否运行安装程序
    set /p "run_setup=是否运行安装程序测试? (Y/N): "
    if /i "!run_setup!"=="Y" (
        echo 正在启动安装程序...
        start "" "installer\TodoApp-Setup.exe"
    )
    
    REM 询问是否打开文件夹
    set /p "open_folder=是否打开发布文件夹? (Y/N): "
    if /i "!open_folder!"=="Y" (
        start "" "installer"
    )
    
) else (
    echo [错误] 未找到生成的安装程序文件
    pause
    exit /b 1
)

echo.
echo 安装程序生成脚本执行完成
pause
