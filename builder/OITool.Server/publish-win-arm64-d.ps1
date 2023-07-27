# Build with NET6.0 SDK
# Platform: Win-arm64
# Configure: Debug

try {
    Push-Location
    Set-Location ../../src/OITool.Server/
    dotnet publish OITool.Server.csproj -c Debug --no-self-contained --framework net6.0 --runtime win-arm64 --output bin/Debug/net6.0/publish/win-arm64
}
finally {
    Pop-Location
}