@echo off
chcp 65001 >nul
echo 正在發布應用程式...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o bin\Release\net8.0\publish

if %ERRORLEVEL% NEQ 0 (
    echo 發布失敗！
    pause
    exit /b 1
)

echo.
echo 發布完成！請在 Inno Setup Compiler 中打開 setup.iss 並編譯。
echo.
pause
