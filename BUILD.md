# TodoApp 构建和发布指南

## 快速开始

### 🚀 一键发布流程

1. **完整发布流程**（推荐）
   ```cmd
   clean && publish-all && package
   ```

2. **单平台发布**
   ```cmd
   publish.bat
   ```

3. **多平台发布**
   ```cmd
   publish-all.bat
   ```

4. **打包发布文件**
   ```cmd
   package.bat
   ```

5. **清理所有文件**
   ```cmd
   clean.bat
   ```

## 📁 脚本说明

### publish.bat - 单平台发布
- ✅ 检查 .NET SDK
- ✅ 发布自包含版和便携版
- ✅ 显示文件大小信息
- ✅ 错误处理和提示

### publish-all.bat - 多平台发布
- ✅ 支持 6 个平台：Win-x64、Win-x86、Linux-x64、macOS-x64、Linux-ARM64
- ✅ 自动清理旧文件
- ✅ 文件大小统计
- ✅ 详细的错误处理
- ✅ 平台说明

### package.bat - 打包脚本
- ✅ 自动版本号（基于日期）
- ✅ 多格式支持（ZIP、TAR.GZ）
- ✅ 平台特定的打包
- ✅ 版本化管理

### clean.bat - 清理脚本
- ✅ 清理所有构建产物
- ✅ 清理发布文件
- ✅ 清理日志文件
- ✅ 安全的删除操作

## 🔧 手动发布

#### 单文件发布（推荐）
```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

# Windows x86
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -o ./publish

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

# macOS x64
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

#### 便携式发布
```bash
# 需要安装 .NET 8.0 运行时
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish
```

## 📦 发布选项说明

| 参数 | 说明 |
|------|------|
| `-c Release` | 发布版本（优化）|
| `-r win-x64` | 目标平台 |
| `--self-contained true` | 包含 .NET 运行时 |
| `-p:PublishSingleFile=true` | 单文件发布 |
| `-p:PublishTrimmed=false` | 禁用裁剪，提高兼容性 |
| `--verbosity minimal` | 最小化输出信息 |
| `-o ./publish` | 输出目录 |

## 🎯 目标平台

- `win-x64` - Windows 64位
- `win-x86` - Windows 32位  
- `linux-x64` - Linux 64位
- `osx-x64` - macOS 64位
- `linux-arm64` - Linux ARM64
- `osx-arm64` - macOS ARM64

## 📁 文件结构

发布后的文件结构：
```
publish/
├── win-x64/
│   └── TodoApp.exe
├── win-x86/
│   └── TodoApp.exe
├── linux-x64/
│   └── TodoApp
├── osx-x64/
│   └── TodoApp
└── linux-arm64/
    └── TodoApp

packages/
└── archives/
    ├── TodoApp-Windows-x64-YYYY-MM-DD.zip
    ├── TodoApp-Windows-x86-YYYY-MM-DD.zip
    ├── TodoApp-Linux-x64-YYYY-MM-DD.tar.gz
    ├── TodoApp-macOS-YYYY-MM-DD.tar.gz
    └── TodoApp-Linux-ARM64-YYYY-MM-DD.tar.gz
```

## 📤 分发

### Windows
- **单文件**：直接分享 `.exe` 文件
- **安装程序**：使用 Inno Setup 创建的 `.exe` 安装包
- **ZIP包**：使用 `package.bat` 创建的压缩包

### Linux
- **TAR.GZ**：推荐格式，包含权限信息
- **AppImage**：创建便携应用
- **DEB/RPM**：发行版包格式

### macOS
- **TAR.GZ**：源码编译后的应用
- **DMG**：磁盘映像格式
- **APP**：macOS 应用包

## ⚠️ 注意事项

1. **图标**：确保 `Assets/app.ico` 存在且有效
2. **版本**：更新 `TodoApp.csproj` 中的版本号
3. **测试**：在目标平台上测试发布的应用
4. **签名**：考虑对可执行文件进行数字签名
5. **依赖**：确保目标系统有必要的运行库
6. **清理**：发布前使用 `clean.bat` 清理旧文件

## 🎉 推荐工作流

```bash
# 1. 清理旧文件
clean

# 2. 发布所有平台
publish-all

# 3. 打包发布文件
package

# 4. 测试各平台版本
# （手动测试）

# 5. 创建安装程序（可选）
# 使用 Inno Setup 编译 setup.iss
```
