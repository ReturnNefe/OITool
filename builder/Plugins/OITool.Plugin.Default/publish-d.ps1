# Build with NET6.0 SDK
# Platform: All
# Configure: Debug

Write-Host "  All Platform Build For OITool.Plugin.Default  " -ForegroundColor Gray -BackgroundColor DarkCyan

try {
    Push-Location
    Set-Location ../../../src/Unit/Plugins/OITool.Plugin.Default/
    dotnet publish OITool.Plugin.Default.csproj -c Debug --no-self-contained --framework net6.0 --output bin/Debug/net6.0/publish/
}
finally {
    Pop-Location
}