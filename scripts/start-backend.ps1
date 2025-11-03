@"
Write-Host "🚀 Starting Backend..." -ForegroundColor Green
cd backend/src/Presentation/Tekus.API
dotnet run
"@ | Out-File -FilePath "scripts/start-backend.ps1" -Encoding utf8