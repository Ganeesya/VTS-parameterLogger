ECHO | call "collectLuaScript.bat"

md "public_package\obs-studio\bin\64bit\"
md "public_package\obs-studio\data\obs-plugins\frontend-tools\scripts\"
copy "VTSAccess_UseLibraryLICENSE.txt" "public_package\obs-studio\bin\64bit\VTSAccess_UseLibraryLICENSE.txt" /y
copy "LICENSE" "public_package\obs-studio\bin\64bit\VTubeStudioAccessLICENSE.txt" /y
copy "VTubeStudioAccess\bin\Release\x64\VTubeStudioAccess.dll" "public_package\obs-studio\bin\64bit\VTubeStudioAccess.dll" /y
copy "VTubeStudioAccess\bin\Release\Newtonsoft.Json.dll" "public_package\obs-studio\bin\64bit\Newtonsoft.Json.dll" /y
copy "saveParam.lua" "public_package\obs-studio\data\obs-plugins\frontend-tools\scripts\saveParam.lua" /y

rem ���t���擾
for /f "tokens=1-3 delims=/ " %%a in ("%date%") do (
    set year=%%c
    set month=%%a
    set day=%%b
)

rem ���Ԃ��擾
for /f "tokens=1-3 delims=:." %%a in ("%time%") do (
    set hour=%%a
    set minute=%%b
    set second=%%c
)

rem ���t�Ǝ��Ԃ��������ăt�@�C�������쐬
set filename=public_package\VTSAccess_%year%%month%%day%_%hour%%minute%%second%.zip

ECHO | "C:\Program Files\7-Zip\7z.exe" a -tzip %filename% "public_package\obs-studio\"

pause