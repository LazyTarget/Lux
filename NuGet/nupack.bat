rmdir package /s /q
call .\build.bat
mkdir .\package\lib\net46
copy ..\Lux\bin\Release\Lux*.dll .\package\lib\net46\
copy .\Lux.nuspec .\package\Lux.nuspec
nuget pack .\package\Lux.nuspec
mkdir .\releases
move .\Lux.*.nupkg .\releases\