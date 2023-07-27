# Build with NET6.0 SDK
# Platform: OSX-x64
# Configure: Debug

try {
    Push-Location
    Set-Location ../../src/OITool.Server/
    dotnet publish OITool.Server.csproj -c Debug --no-self-contained --framework net6.0 --runtime osx-x64 --output bin/Debug/net6.0/publish/osx-x64
}
finally {
    Pop-Location
}