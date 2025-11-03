@"
Write-Host "🚀 Starting Full Stack Application..." -ForegroundColor Magenta

# Start Backend
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd backend/src/Presentation/Tekus.API; dotnet run"

# Wait 5 seconds
Start-Sleep -Seconds 5

# Start Frontend
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd frontend; npm run dev"

Write-Host "✅ Backend: http://localhost:5000" -ForegroundColor Green
Write-Host "✅ Frontend: http://localhost:5173" -ForegroundColor Cyan
"@ | Out-File -FilePath "scripts/start-all.ps1" -Encoding utf8