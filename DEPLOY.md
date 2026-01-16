# 部署指南

## 📤 推送到 GitHub

### 1. 初始化 Git（如果還沒有）

```bash
git init
git add .
git commit -m "初始提交：待辦事項管理器"
```

### 2. 連接到 GitHub 倉庫

```bash
git remote add origin https://github.com/Kevin42127/todolist.git
git branch -M main
git push -u origin main
```

### 3. 後續更新

```bash
git add .
git commit -m "更新：描述變更內容"
git push
```

## 🚀 使用 GitHub Releases 發布安裝檔案

### 建立 Release

1. 前往 https://github.com/Kevin42127/todolist/releases
2. 點擊 "Create a new release"
3. 填寫資訊：
   - **Tag**: `v1.0.0`
   - **Title**: `v1.0.0 - 初始版本`
   - **Description**: 
     ```markdown
     ## 新功能
     - 待辦事項管理
     - 標籤系統
     - 搜尋和篩選
     - 自動儲存
     ```
4. 上傳 `installer/TodoApp-Setup.exe` 到 "Attach binaries"
5. 點擊 "Publish release"

### 下載連結格式

- 最新版本：`https://github.com/Kevin42127/todolist/releases/latest/download/TodoApp-Setup.exe`
- 指定版本：`https://github.com/Kevin42127/todolist/releases/download/v1.0.0/TodoApp-Setup.exe`

## 🌐 部署到 Vercel

### 方法一：使用 Vercel CLI

```bash
# 安裝 Vercel CLI
npm i -g vercel

# 登入
vercel login

# 部署
vercel

# 生產環境部署
vercel --prod
```

### 方法二：使用 GitHub 整合

1. 前往 https://vercel.com
2. 使用 GitHub 登入
3. 點擊 "Add New Project"
4. 選擇 `Kevin42127/todolist` 倉庫
5. 配置：
   - **Framework Preset**: Other
   - **Root Directory**: `download`
   - **Build Command**: (留空)
   - **Output Directory**: `download`
6. 點擊 "Deploy"

### Vercel 配置說明

`vercel.json` 已配置：
- 根路徑 `/` 指向 `download/index.html`
- 靜態檔案服務
- 快取策略優化

### 自訂網域（可選）

1. 在 Vercel 專案設定中
2. 前往 "Domains"
3. 添加你的網域
4. 按照指示設定 DNS

## 📋 檢查清單

- [x] 更新 `.gitignore`
- [x] 更新下載頁面連結
- [x] 創建 Vercel 配置
- [ ] 推送到 GitHub
- [ ] 建立 GitHub Release
- [ ] 上傳安裝檔案
- [ ] 部署到 Vercel
- [ ] 測試下載連結

## 🔄 更新流程

1. 更新程式碼
2. 提交並推送：
   ```bash
   git add .
   git commit -m "更新說明"
   git push
   ```
3. 編譯安裝程式：
   ```bash
   build-setup.bat
   ```
4. 建立新的 GitHub Release 並上傳安裝檔案
5. Vercel 會自動重新部署（如果已連接 GitHub）
