 

[Setup]
AppName= LandisUserInterface
AppVersion= 1.0
DefaultDirName={pf}\Landis-II 6.0
DefaultGroupName= LandisUserInterface
;UninstallDisplayIcon={app}\MyProg.exe
Compression=lzma2
SolidCompression=yes
OutputDir=userdocs:Inno Setup Examples Output

[Files]
Source: "C:\"; DestDir: "{app}"


[Icons]
;Name: "{group}\My Program"; Filename: "{app}\MyProg.exe"