# Build with NET6.0 SDK
# Platform: Linux-x64
# Configure: Debug

try {
    Push-Location
    Set-Location ../../src/OITool.CLI/
    dotnet publish OITool.CLI.csproj -c Debug --no-self-contained --framework net6.0 --runtime linux-x64 --output bin/Debug/net6.0/publish/linux-x64
}
finally {
    Pop-Location
}