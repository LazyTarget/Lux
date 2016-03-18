"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" ..\src\Lux.sln /p:Configuration=Release /m

cd /d %~dp0
cd Lux
call .\nupack.bat

cd /d %~dp0
cd Lux.Diagnostics.Log4net
call .\nupack.bat

cd /d %~dp0