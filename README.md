# AI 聊天助手

基於 Avalonia UI 和 Groq AI 的跨平台桌面應用程式，支援 Windows、macOS 和 Linux。

## 功能特色

- 跨平台支援（Windows、macOS、Linux）
- 暗色主題界面
- 整合 Groq AI API
- 即時聊天對話
- 配置管理（API Key）
- 統一通知系統

## 技術棧

- **框架**: Avalonia UI 11.0
- **語言**: C# (.NET 8)
- **AI 服務**: Groq AI API
- **架構**: MVVM 模式

## 專案結構

```
├── AIChatApp.csproj          # 專案檔
├── Program.cs                 # 應用程式入口
├── App.axaml                  # 應用程式資源定義
├── App.axaml.cs               # 應用程式邏輯
├── Views/                     # 視圖
│   └── MainWindow.axaml       # 主視窗 UI
│   └── MainWindow.axaml.cs    # 主視窗邏輯
├── ViewModels/                # 視圖模型
│   └── MainWindowViewModel.cs
├── Models/                    # 資料模型
│   └── ChatMessage.cs
│   └── GroqApiModels.cs
├── Services/                  # 服務層
│   └── GroqApiService.cs      # Groq API 服務
│   └── ConfigService.cs       # 配置管理
│   └── NotificationService.cs # 通知服務
└── Utils/                     # 工具類
    └── Constants.cs
    └── Converters.cs
```

## 建置與執行

### 前置需求

- .NET 8 SDK 或更高版本
- Groq API Key（從 https://console.groq.com 取得）

### 建置專案

```bash
dotnet restore
dotnet build
```

### 執行應用程式

#### 方法一：使用 dotnet run（開發模式）

```bash
dotnet run
```

#### 方法二：直接執行編譯後的程式（推薦）

**Windows:**
```bash
# 前往編譯輸出目錄
cd bin\Debug\net8.0

# 執行應用程式
.\AIChatApp.exe
```

或直接在檔案總管中雙擊 `bin\Debug\net8.0\AIChatApp.exe`

**macOS/Linux:**
```bash
cd bin/Debug/net8.0
./AIChatApp
```

#### 方法三：建立桌面捷徑（Windows）

1. 前往 `bin\Debug\net8.0\` 目錄
2. 右鍵點擊 `AIChatApp.exe`
3. 選擇「建立捷徑」
4. 將捷徑拖曳到桌面

**注意：** 首次執行時，應用程式會自動提示您輸入 Groq API Key。

## 使用說明

### 首次使用 - 填入 API Key

1. **啟動應用程式**
   - 執行 `dotnet run` 或直接執行編譯後的 `AIChatApp.exe`

2. **填入 Groq API Key**
   - 首次啟動時會自動彈出設定對話框
   - 或點擊右上角的「⚙ 設定」按鈕
   - 在輸入框中填入您的 Groq API Key
   - 點擊「儲存」按鈕

3. **取得 API Key**
   - 前往 https://console.groq.com
   - 註冊或登入帳號
   - 在 API Keys 頁面建立新的 API Key
   - 複製 API Key 並貼到應用程式中

### 開始聊天

1. **輸入訊息**
   - 在底部輸入框中輸入您想問的問題
   - 或按 `Enter` 鍵快速發送

2. **發送訊息**
   - 點擊「發送」按鈕
   - 或按 `Enter` 鍵（不按 Shift）

3. **查看回應**
   - AI 回應會顯示在聊天區域
   - 用戶訊息顯示在右側（藍色）
   - AI 回應顯示在左側（灰色）
   - 錯誤訊息顯示在中間（紅色）

### 修改 API Key

- 隨時點擊右上角的「⚙ 設定」按鈕
- 修改 API Key 後點擊「儲存」
- 新的 API Key 會立即生效

## 配置

API Key 儲存在應用程式資料目錄：
- Windows: `%APPDATA%\AIChatApp\config.json`
- macOS: `~/Library/Application Support/AIChatApp/config.json`
- Linux: `~/.config/AIChatApp/config.json`

## 授權

版權所有 © AIChatApp

