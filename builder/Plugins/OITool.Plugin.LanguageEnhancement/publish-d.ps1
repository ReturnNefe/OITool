# Build with NET6.0 SDK
# Platform: All
# Configure: Debug

Write-Host "  All Platform Build For OITool.Plugin.LanguageEnhancement  " -ForegroundColor Gray -BackgroundColor DarkCyan

try {
    Push-Location
    Set-Location ../../../src/Unit/Plugins/OITool.Plugin.LanguageEnhancement/
    dotnet publish OITool.Plugin.LanguageEnhancement.csproj -c Debug --no-self-contained --framework net6.0 --output bin/Debug/net6.0/publish/
}
finally {
    Pop-Location
}