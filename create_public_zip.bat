ECHO | call "collectLuaScript.bat"

cd "public_package"

md "obs-studio\bin\64bit\"
md "obs-studio\data\obs-plugins\frontend-tools\scripts\"
copy "..\VTSAccess_UseLibraryLICENSE.txt" "obs-studio\bin\64bit\VTSAccess_UseLibraryLICENSE.txt" /y
copy "..\LICENSE" "obs-studio\bin\64bit\VTubeStudioAccessLICENSE.txt" /y
copy "..\VTubeStudioAccess\bin\Release\x64\VTubeStudioAccess.dll" "obs-studio\bin\64bit\VTubeStudioAccess.dll" /y
copy "..\VTubeStudioAccess\bin\Release\Newtonsoft.Json.dll" "obs-studio\bin\64bit\Newtonsoft.Json.dll" /y
copy "..\saveParam.lua" "obs-studio\data\obs-plugins\frontend-tools\scripts\saveParam.lua" /y

rem ���t���擾
for /f "tokens=1-3 delims=/ " %%a in ("%date%") do (
    set year=%%a
    set month=%%b
    set day=%%c
)

rem ���Ԃ��擾
for /f "tokens=1-3 delims=:." %%a in ("%time%") do (
    set hour=%%a
    set minute=%%b
    set second=%%c
)

rem ���t�Ǝ��Ԃ��������ăt�@�C�������쐬
set filename=VTSAccess_%year%%month%%day%_%hour%%minute%%second%.zip

ECHO | "C:\Program Files\7-Zip\7z.exe" a -tzip "%filename%" "obs-studio\"

pause