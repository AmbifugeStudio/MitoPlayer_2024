[Setup]
AppName=MitoPlayer_2024
AppVersion=1.0
DefaultDirName={autopf}\MitoPlayer_2024
DefaultGroupName=MitoPlayer_2024
OutputDir=.
OutputBaseFilename=mitoplayer_setup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=none

[Files]
Source: "bin\x64\Release\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\MitoPlayer_2024"; Filename: "{app}\MitoPlayer_2024.exe"
Name: "{group}\Uninstall MitoPlayer_2024"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\MitoPlayer_2024.exe"; Description: "Futtatás telepítés után"; Flags: nowait postinstall skipifsilent