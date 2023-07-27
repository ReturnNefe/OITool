# Build with NET6.0 SDK
# Platform: Linux-arm64
# Configure: Debug

try {
    Push-Location
    Set-Location ../../src/OITool.Server/
    dotnet publish OITool.Server.csproj -c Debug --no-self-contained --framework net6.0 --runtime linux-arm64 --output bin/Debug/net6.0/publish/linux-arm64
}
finally {
    Pop-Location
}