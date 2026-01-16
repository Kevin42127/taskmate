@echo off
echo ========================================
echo    TodoApp 下载页面启动脚本
echo ========================================
echo.

REM 检查 Python 是否安装
python --version >nul 2>&1
if errorlevel 1 (
    echo [错误] 未找到 Python，无法启动本地服务器
    echo.
    echo 请安装 Python 或使用其他方式：
    echo 1. 安装 Python: https://www.python.org/downloads/
    echo 2. 直接双击 download/index.html 文件
    echo.
    pause
    exit /b 1
)

echo 正在启动本地服务器...
echo 服务器地址: http://localhost:8000
echo 按 Ctrl+C 停止服务器
echo.

REM 切换到 download 目录并启动 HTTP 服务器
cd download
python -m http.server 8000

pause
