rmdir package /s /q
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" ..\Lux.sln /p:Configuration=Release /m
mkdir .\package\lib\net46
copy ..\src\Lux\bin\Release\Lux*.dll .\package\lib\net46\