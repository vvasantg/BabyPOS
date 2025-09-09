# Start BabyPOS-API in background
$apiProcess = Start-Process "dotnet" "run --project ./BabyPOS-API/BabyPOS-API.csproj" -PassThru

# Wait for API to be ready (adjust if needed)
Start-Sleep -Seconds 7

# Run tests
$testResult = dotnet test ./BabyPOS-API.Tests/BabyPOS-API.Tests.csproj

# Stop API
Stop-Process -Id $apiProcess.Id

# Output test result
Write-Output $testResult
