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
Source: "MapWinGIS-only-v4.9.3.4-Win32.exe"; DestDir: "{app}"; AfterInstall: RunOtherInstaller()
;Source: C:\Program Files\LANDIS-II\v6\bin\extensions\Landis.Extension.Succession.BiomassPnET.dll; DestDir: {app}\bin; Flags: replacesameversion
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\Antlr3.Runtime.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\AxInterop.MapWinGIS.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\Crom.Controls.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\Interop.MapWinGIS.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\LandisUserInterface.exe";  DestDir: "{app}"; Flags: replacesameversion
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\unvell.ReoGrid.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\unvell.ReoScript.dll"; DestDir: "{app}"
Source: "C:\Users\adebruij\Desktop\PnET-Succession\PnET-succeesion\LandisUserInterface\src\bin\Debug\ZedGraph.dll"; DestDir: "{app}"

Source: "LANDIS-II User Interface User Guide.docx"; DestDir: "{app}"; Flags: isreadme

[Icons]
Name: "{group}\Landis User Interface"; Filename: "{app}\LandisUserInterface.exe"

[Code]

procedure RunOtherInstaller();
var
  ResultCode: Integer;
begin
  if not Exec(ExpandConstant('{app}\MapWinGIS-only-v4.9.3.4-Win32.exe'), '', '', SW_SHOWNORMAL,
    ewWaitUntilTerminated, ResultCode)
  then
    MsgBox('Other installer failed to run!' + #13#10 +
      SysErrorMessage(ResultCode), mbError, MB_OK);
end;
