rmdir package /s /q
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" ..\src\Lux.sln /p:Configuration=Release /m
mkdir .\package\lib\net45
copy ..\src\Lux\bin\Release\Lux*.dll .\package\lib\net45\