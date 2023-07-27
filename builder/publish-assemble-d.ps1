# Build and Assemble for all platforms

Write-Host "  All Platform Assemble  " -ForegroundColor Gray -BackgroundColor DarkMagenta

# Build
try {
    Push-Location
    Set-Location OITool.Interface/
    ./publish-d.ps1
}
finally {
    Pop-Location
}

try {
    Push-Location
    Set-Location OITool.CLI/
    ./publish-all-d.ps1
}
finally {
    Pop-Location
}

try {
    Push-Location
    Set-Location OITool.Server/
    ./publish-all-d.ps1
}
finally {
    Pop-Location
}

try {
    Push-Location
    Set-Location Plugins/OITool.Plugin.Default/
    ./publish-d.ps1
}
finally {
    Pop-Location
}

# Assemble
try {
    Push-Location
    Set-Location ../
    
    Move-Item -Path src/OITool.CLI/bin/Debug/net6.0/publish/linux-arm64 -Destination build/linux-arm64
    Move-Item -Path src/OITool.Server/bin/Debug/net6.0/publish/linux-arm64 -Destination build/linux-arm64/server
    Copy-Item -Path src/Unit/Plugins/OITool.Plugin.Default/bin/Debug/net6.0/publish -Destination build/linux-arm64/server/plugins/OITool.Plugin.Default -Recurse
    
    Move-Item -Path src/OITool.CLI/bin/Debug/net6.0/publish/linux-x64 -Destination build/linux-x64
    Move-Item -Path src/OITool.Server/bin/Debug/net6.0/publish/linux-x64 -Destination build/linux-x64/server
    Copy-Item -Path src/Unit/Plugins/OITool.Plugin.Default/bin/Debug/net6.0/publish -Destination build/linux-x64/server/plugins/OITool.Plugin.Default -Recurse
    
    Move-Item -Path src/OITool.CLI/bin/Debug/net6.0/publish/osx-x64 -Destination build/osx-x64
    Move-Item -Path src/OITool.Server/bin/Debug/net6.0/publish/osx-x64 -Destination build/osx-x64/server
    Copy-Item -Path src/Unit/Plugins/OITool.Plugin.Default/bin/Debug/net6.0/publish -Destination build/osx-x64/server/plugins/OITool.Plugin.Default -Recurse
    
    Move-Item -Path src/OITool.CLI/bin/Debug/net6.0/publish/win-arm64 -Destination build/win-arm64
    Move-Item -Path src/OITool.Server/bin/Debug/net6.0/publish/win-arm64 -Destination build/win-arm64/server
    Copy-Item -Path src/Unit/Plugins/OITool.Plugin.Default/bin/Debug/net6.0/publish -Destination build/win-arm64/server/plugins/OITool.Plugin.Default -Recurse
    
    Move-Item -Path src/OITool.CLI/bin/Debug/net6.0/publish/win-x64 -Destination build/win-x64
    Move-Item -Path src/OITool.Server/bin/Debug/net6.0/publish/win-x64 -Destination build/win-x64/server
    Copy-Item -Path src/Unit/Plugins/OITool.Plugin.Default/bin/Debug/net6.0/publish -Destination build/win-x64/server/plugins/OITool.Plugin.Default -Recurse
    
    Move-Item -Path src/OITool.CLI/bin/Debug/net6.0/publish/win-x86 -Destination build/win-x86
    Move-Item -Path src/OITool.Server/bin/Debug/net6.0/publish/win-x86 -Destination build/win-x86/server
    Copy-Item -Path src/Unit/Plugins/OITool.Plugin.Default/bin/Debug/net6.0/publish -Destination build/win-x86/server/plugins/OITool.Plugin.Default -Recurse
    
}
finally {
    Pop-Location
}