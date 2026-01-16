@echo off
echo ========================================
echo    TodoApp 清理脚本
echo ========================================
echo.

echo [1/4] 清理发布文件...
if exist "publish" (
    echo   删除发布目录...
    rmdir /s /q "publish"
    echo   ✓ 发布目录已清理
) else (
    echo   ✓ 发布目录不存在
)

echo [2/4] 清理打包文件...
if exist "packages" (
    echo   删除打包目录...
    rmdir /s /q "packages"
    echo   ✓ 打包目录已清理
) else (
    echo   ✓ 打包目录不存在
)

echo [3/4] 清理构建文件...
if exist "bin" (
    echo   删除构建目录...
    rmdir /s /q "bin"
    echo   ✓ 构建目录已清理
) else (
    echo   ✓ 构建目录不存在
)

if exist "obj" (
    echo   删除对象目录...
    rmdir /s /q "obj"
    echo   ✓ 对象目录已清理
) else (
    echo   ✓ 对象目录不存在
)

echo [4/4] 清理日志文件...
if exist "*.log" (
    echo   删除日志文件...
    del /q *.log
    echo   ✓ 日志文件已清理
) else (
    echo   ✓ 无日志文件
)

echo.
echo ========================================
echo           清理完成！
echo ========================================
echo.
echo 所有临时文件和构建产物已清理
echo.
pause
