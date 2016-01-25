call .\build.bat
copy .\Lux.nuspec .\package\Lux.nuspec
nuget pack .\package\Lux.nuspec
mkdir .\releases
move .\Lux.*.nupkg .\releases\