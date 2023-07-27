# Build with NET6.0 SDK
# Platform: Win-x64
# Configure: Debug

try {
    Push-Location
    Set-Location ../../src/OITool.CLI/
    dotnet publish OITool.CLI.csproj -c Debug --no-self-contained --framework net6.0 --runtime win-x64 --output bin/Debug/net6.0/publish/win-x64
}
finally {
    Pop-Location
}