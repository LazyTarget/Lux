rmdir package /s /q
mkdir .\package\lib\net45
copy ..\..\src\Lux\bin\Release\Lux.dll .\package\lib\net45\

copy .\Lux.nuspec .\package\Lux.nuspec
..\nuget pack .\package\Lux.nuspec
mkdir .\releases
move .\Lux.*.nupkg .\releases\
rmdir package /s /q