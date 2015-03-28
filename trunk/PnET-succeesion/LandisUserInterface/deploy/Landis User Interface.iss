; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=Landis User Interface
AppVersion=1.0
DefaultDirName={pf}\Landis User Interface
DefaultGroupName=Landis User Interface
UninstallDisplayIcon={app}\MyProg.exe
Compression=lzma2
SolidCompression=yes

[Files]
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\Antlr3.Runtime.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\AxInterop.MapWinGIS.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\Crom.Controls.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\Interop.MapWinGIS.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\LandisUserInterface.exe"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\unvell.ReoGrid.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\unvell.ReoScript.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\ZedGraph.dll"; DestDir: "{app}"

Source: "LANDIS-II User Interface User Guide.docx"; DestDir: "{app}"; Flags: isreadme

[Icons]
Name: "{group}\Landis User Interface"; Filename: "{app}\LandisUserInterface.exe"
