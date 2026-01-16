[Setup]
AppId={{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}
AppName=待辦事項管理器
AppVersion=1.0.0
AppPublisher=TodoApp
AppPublisherURL=https://github.com/your-repo
AppSupportURL=https://github.com/your-repo/issues
AppUpdatesURL=https://github.com/your-repo/releases
DefaultDirName={pf}\TodoApp
DefaultGroupName=TodoApp
AllowNoIcons=yes
LicenseFile=
OutputDir=installer
OutputBaseFilename=TodoApp-Setup
SetupIconFile=Assets\app.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\待辦事項管理器"; Filename: "{app}\TodoApp.exe"
Name: "{commondesktop}\待辦事項管理器"; Filename: "{app}\TodoApp.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\TodoApp.exe"; Description: "{cm:LaunchProgram}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}"
