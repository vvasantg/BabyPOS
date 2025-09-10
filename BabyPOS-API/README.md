# BabyPOS-API

🍽️ RESTful API สำหรับระบบ BabyPOS (ร้านอาหาร/คาเฟ่)

## 🚀 Features

### 🏪 Core Business Logic
- **Shop Management**: จัดการข้อมูลร้านค้า
- **Menu Items**: จัดการเมนูอาหาร พร้อมหมวดหมู่และรูปภาพ
- **Order System**: ระบบออเดอร์แบบ Real-time
- **Table Management**: จัดการโต๊ะในร้าน
- **User Authentication**: JWT Token-based Authentication

### 🔐 Security
- **JWT Authentication**: Secure token-based auth
- **Role-based Authorization**: Admin/Customer roles
- **CORS Policy**: Cross-origin resource sharing
- **Input Validation**: Data validation และ sanitization

### 📊 Data Management
- **Entity Framework Core**: ORM สำหรับฐานข้อมูล
- **SQL Server**: Production database
- **Migrations**: Database schema versioning
- **Seed Data**: ข้อมูลตัวอย่างสำหรับ development

## 🏗️ Architecture

### Clean Architecture Pattern
```
BabyPOS-API/
├── Controllers/              # API Endpoints
│   ├── ShopsController.cs        # ร้านค้า CRUD
│   ├── MenuItemsController.cs    # เมนูอาหาร CRUD
│   ├── OrdersController.cs       # ระบบออเดอร์
│   ├── TablesController.cs       # จัดการโต๊ะ
│   └── UsersController.cs        # Authentication
├── Models/                   # Domain Models
│   ├── Shop.cs                   # โมเดลร้านค้า
│   ├── MenuItem.cs               # โมเดลเมนูอาหาร
│   ├── Order.cs                  # โมเดลออเดอร์
│   ├── OrderItem.cs             # รายการในออเดอร์
│   ├── Table.cs                  # โมเดลโต๊ะ
│   └── User.cs                   # โมเดลผู้ใช้
├── Data/                     # Data Access Layer
│   └── AppDbContext.cs           # EF Core DbContext
├── Application/              # Business Logic
│   └── Services/                 # Business Services
├── Infrastructure/          # External Services
│   └── Repositories/             # Data Repositories
├── Migrations/              # EF Core Migrations
└── Properties/
    └── launchSettings.json       # Development settings
```

## 📡 API Endpoints

### 🏪 Shops
```http
GET    /api/shops                 # ดูร้านค้าทั้งหมด
GET    /api/shops/{id}            # ดูร้านค้าตาม ID
POST   /api/shops                 # สร้างร้านใหม่
PUT    /api/shops/{id}            # แก้ไขร้าน
DELETE /api/shops/{id}            # ลบร้าน
```

### 🍽️ Menu Items
```http
GET    /api/menu-items            # ดูเมนูทั้งหมด
GET    /api/menu-items/{id}       # ดูเมนูตาม ID
GET    /api/shops/{shopId}/menu-items  # ดูเมนูของร้าน
POST   /api/menu-items            # สร้างเมนูใหม่
PUT    /api/menu-items/{id}       # แก้ไขเมนู
DELETE /api/menu-items/{id}       # ลบเมนู
```

### 📋 Orders
```http
GET    /api/orders                # ดูออเดอร์ทั้งหมด
GET    /api/orders/{id}           # ดูออเดอร์ตาม ID
GET    /api/shops/{shopId}/orders # ดูออเดอร์ของร้าน
POST   /api/orders                # สร้างออเดอร์ใหม่
PUT    /api/orders/{id}           # แก้ไขออเดอร์
DELETE /api/orders/{id}           # ลบออเดอร์
```

### 🪑 Tables
```http
GET    /api/tables                # ดูโต๊ะทั้งหมด
GET    /api/shops/{shopId}/tables # ดูโต๊ะของร้าน
POST   /api/tables                # สร้างโต๊ะใหม่
DELETE /api/tables/{id}           # ลบโต๊ะ
```

### 👤 Users & Auth
```http
POST   /api/users/register        # สมัครสมาชิก
POST   /api/users/login           # เข้าสู่ระบบ
GET    /api/users/profile         # ดูโปรไฟล์ (ต้อง Auth)
```

## 🗄️ Database Schema

### Core Entities
```sql
-- Shops (ร้านค้า)
Shops: Id, Name, CreatedAt

-- MenuItems (เมนูอาหาร)  
MenuItems: Id, Name, Price, Category, ImagePath, ShopId

-- Orders (ออเดอร์)
Orders: Id, TotalAmount, Status, CreatedAt, ShopId, TableId

-- OrderItems (รายการในออเดอร์)
OrderItems: Id, Quantity, UnitPrice, OrderId, MenuItemId

-- Tables (โต๊ะ)
Tables: Id, Name, ShopId

-- Users (ผู้ใช้)
Users: Id, Username, PasswordHash, Role, CreatedAt
```

### Food Categories
```csharp
public enum FoodCategory
{
    MainDish,    // อาหาร
    Dessert,     // ของหวาน  
    Drink,       // เครื่องดื่ม
    Other        // อื่นๆ
}
```

## 🚀 การรัน

### Development
```powershell
cd BabyPOS-API
dotnet run
```

### การเข้าถึง
- **API**: `https://localhost:7001`
- **Swagger UI**: `https://localhost:7001/swagger`

### Database Migration
```powershell
# สร้าง Migration ใหม่
dotnet ef migrations add MigrationName

# Update Database
dotnet ef database update
```

## 🧪 Testing

### Unit Tests
```powershell
dotnet test ../BabyPOS-API.Tests/
```

### API Testing
- **Postman Collection**: [BabyPOS-API.http](BabyPOS-API.http)
- **Swagger UI**: Built-in API documentation

## ⚙️ Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BabyPOSDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "BabyPOS-API",
    "Audience": "BabyPOS-Web",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

### Development Settings
```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

## 📦 Dependencies

### Main Packages
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Swashbuckle.AspNetCore" />
<PackageReference Include="Microsoft.AspNetCore.Cors" />
```

## 🔄 Recent Updates

### v2.0 (Latest)
- ✅ JWT Authentication ระบบสมบูรณ์
- ✅ Table Management API เพิ่มเติม
- ✅ Food Category Enum (MainDish, Dessert, Drink, Other)
- ✅ Image Path support สำหรับ Menu Items
- ✅ Order Status Management
- ✅ CORS Configuration สำหรับ Web Frontend
- ✅ Swagger Documentation ครบถ้วน

### Database Migrations
- **InitialCreate**: โครงสร้างฐานข้อมูลพื้นฐาน
- **AllFeatures**: เพิ่ม Tables, Users, Order relations
- **AddFoodCategoryToMenuItem**: เพิ่มหมวดหมู่อาหาร
- **AddImagePathToMenuItem**: รองรับรูปภาพ

## 🔧 Development Tools

### Debugging
```powershell
# ดู logs แบบ real-time
dotnet run --verbosity detailed

# เช็ค database connection
dotnet ef database drop  # ลบฐานข้อมูล
dotnet ef database update  # สร้างใหม่
```

### Code Quality
- **API Versioning**: Ready for v2, v3 APIs
- **Exception Handling**: Global exception middleware
- **Logging**: Structured logging with Serilog (optional)
- **Validation**: Model validation attributes

## 🌐 Integration

### Frontend Compatibility
- **BabyPOS-Web2**: Blazor Server (Primary)
- **BabyPOS-Web**: Blazor WebAssembly (Legacy)
- **Mobile Apps**: รองรับการเชื่อมต่อ REST API

### External Services
- **Image Storage**: Local file system (development)
- **Payment Gateway**: พร้อมสำหรับ integration
- **Reporting**: Export ข้อมูลเป็น JSON/CSV

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.

## 🔗 Related Projects

- **[BabyPOS-Web2](../BabyPOS-Web2/)**: Blazor Server Frontend
- **[BabyPOS-Web](../BabyPOS-Web/)**: WebAssembly Frontend (Legacy)
- **[Tests](../BabyPOS-API.Tests/)**: API Unit Tests
- **[Documentation](../docs/)**: Complete System Documentation
