# BabyPOS-API.Tests

Unit tests สำหรับ BabyPOS-API

## Features
- ทดสอบ API endpoint (Shops, Menu, Orders, Users)
- ทดสอบ business logic/service
- Mock JWT, Mock DB

## การรัน
```powershell
dotnet test .\BabyPOS-API.Tests\BabyPOS-API.Tests.csproj
```

## หมายเหตุ
- ใช้ xUnit, Moq
- ทดสอบเฉพาะ logic และ endpoint สำคัญ
