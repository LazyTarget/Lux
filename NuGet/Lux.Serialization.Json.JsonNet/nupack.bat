rmdir package /s /q
mkdir .\package\lib\net45
copy ..\..\src\Lux.Serialization.Json.JsonNet\bin\Release\Lux.Serialization.Json.JsonNet.dll .\package\lib\net45\

copy .\Lux.Serialization.Json.JsonNet.nuspec .\package\Lux.Serialization.Json.JsonNet.nuspec
..\nuget pack .\package\Lux.Serialization.Json.JsonNet.nuspec
mkdir .\releases
move .\Lux.Serialization.Json.JsonNet.*.nupkg .\releases\
rmdir package /s /q