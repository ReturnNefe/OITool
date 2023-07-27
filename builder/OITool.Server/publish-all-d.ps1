# Build for all platforms

Write-Host "  All Platform Build For OITool.Server  " -ForegroundColor Gray -BackgroundColor DarkCyan

Write-Host "Publishing Linux-arm64 Debug" -ForegroundColor DarkYellow
./publish-linux-arm64-d.ps1

Write-Host "Publishing Linux-x64 Debug" -ForegroundColor DarkYellow
./publish-linux-x64-d.ps1

Write-Host "Publishing OSX-x64 Debug" -ForegroundColor DarkYellow
./publish-osx-x64-d.ps1

Write-Host "Publishing Win-arm64 Debug" -ForegroundColor DarkYellow
./publish-win-arm64-d.ps1

Write-Host "Publishing Win-x64 Debug" -ForegroundColor DarkYellow
./publish-win-x64-d.ps1

Write-Host "Publishing Win-x86 Debug" -ForegroundColor DarkYellow
./publish-win-x86-d.ps1