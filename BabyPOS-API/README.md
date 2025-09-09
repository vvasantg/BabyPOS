# BabyPOS-API

RESTful API สำหรับระบบ BabyPOS (ร้านอาหาร/คาเฟ่)

## Features
- จัดการร้านค้า (Shops)
- จัดการเมนูอาหาร (Menu Items)
- จัดการออเดอร์ (Orders)
- ระบบผู้ใช้ (Users)
- JWT Authentication
- รองรับการเชื่อมต่อกับ Frontend/External API

## โครงสร้าง
- Controllers: API endpoints
- Models: โครงสร้างข้อมูลหลัก
- Data: EF Core DbContext
- Application/Infrastructure: Service, Repository
- Migrations: ไฟล์ฐานข้อมูล

## การรัน
```powershell
dotnet run --project .\BabyPOS-API\BabyPOS-API.csproj
```

## การทดสอบ
```powershell
dotnet test .\BabyPOS-API.Tests\BabyPOS-API.Tests.csproj
```

## Config
- `appsettings.json` สำหรับตั้งค่า DB, JWT, ฯลฯ
