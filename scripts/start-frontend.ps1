@"
Write-Host "🎨 Starting Frontend..." -ForegroundColor Cyan
cd frontend
npm run dev
"@ | Out-File -FilePath "scripts/start-frontend.ps1" -Encoding utf8