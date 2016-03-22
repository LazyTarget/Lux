rmdir package /s /q
mkdir .\package\lib\net45
copy ..\..\src\Lux.Diagnostics.Log4net\bin\Release\Lux.Diagnostics.Log4net.dll .\package\lib\net45\

copy .\Lux.Diagnostics.Log4net.nuspec .\package\Lux.Diagnostics.Log4net.nuspec
..\nuget pack .\package\Lux.Diagnostics.Log4net.nuspec
mkdir .\releases
move .\Lux.Diagnostics.Log4Net.*.nupkg .\releases\
rmdir package /s /q