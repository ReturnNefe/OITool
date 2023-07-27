# Build with NET6.0 SDK
# Platform: Win-x86
# Configure: Debug

try {
    Push-Location
    Set-Location ../../src/OITool.Server/
    dotnet publish OITool.Server.csproj -c Debug --no-self-contained --framework net6.0 --runtime win-x86 --output bin/Debug/net6.0/publish/win-x86
}
finally {
    Pop-Location
}