# BabyPOS-Web2

Blazor Server-side Web Application สำหรับระบบ BabyPOS (ร้านอาหาร/คาเฟ่)

## 🚀 Features

### 🎨 Theme System
- **Pastel Theme**: ธีมสีพาสเทลสวยงาม เหมาะสำหรับร้านอาหารและคาเฟ่
- **Theme Switching**: สลับธีมได้แบบ Real-time ผ่าน ThemeSelector component
- **Responsive Design**: รองรับการแสดงผลในทุกขนาดหน้าจอ

### 🛍️ Core Functionality
- **Shop Management**: เรียกดูร้านค้าทั้งหมด
- **Food Menu**: แสดงเมนูอาหารแบบ Grid 4 คอลัมน์ พร้อมรูปภาพ
- **Order Management**: จัดการออเดอร์และสถานะ On Hold
- **Table Management**: จัดการโต๊ะในร้าน
- **User Authentication**: ระบบ Login/Logout พร้อม JWT Token

### 🎯 Customer Experience
- **Easy Navigation**: การนำทางที่เข้าใจง่าย (shops → food-list → shop-orders)
- **Visual Food Cards**: การ์ดแสดงอาหารพร้อมรูปภาพ ราคา และหมวดหมู่
- **Interactive Buttons**: ปุ่ม On Hold แบบ Toggle (สีส้ม/ขาว)
- **Category Tabs**: แยกหมวดหมู่อาหาร (ทั้งหมด, อาหาร, ของหวาน, เครื่องดื่ม, อื่นๆ)

## 🏗️ Architecture

```
BabyPOS-Web2/
├── Pages/                    # Razor Pages
│   ├── Shops.razor          # หน้าร้านค้าทั้งหมด
│   └── FoodList.razor       # หน้าเมนูอาหาร (4 cards/row)
├── Presentation/
│   ├── Pages/               # เพิ่มเติม Razor Pages
│   └── Components/          # Reusable Components
│       ├── ThemeSelector.razor   # ตัวเลือกธีม
│       └── UserMenu.razor        # เมนูผู้ใช้
├── Application/             # Business Logic
├── Infrastructure/         # External Services
│   └── Services/
│       ├── IApiService.cs   # API Client Interface
│       ├── ApiService.cs    # HTTP Client สำหรับ BabyPOS-API
│       └── IThemeService.cs # Theme Management
├── Domain/                  # Domain Models
├── Models/                  # DTOs และ ViewModels
└── wwwroot/
    └── css/
        ├── pastel-theme.css # Pastel Theme Styles
        └── site.css         # Base Styles
```

## 🎨 Theme System

### การใช้งาน ThemeService
```csharp
@inject IThemeService ThemeService

// ใน Component
var currentTheme = await ThemeService.GetCurrentThemeAsync();
await ThemeService.SetThemeAsync("pastel");
```

### CSS Theme Classes
```css
/* สำหรับ Pastel Theme */
.theme-pastel {
    /* HSL Color Variables */
    --pastel-primary: hsl(142, 76%, 36%);
    --pastel-card-bg: hsl(0, 0%, 100%);
    /* ... */
}

/* สำหรับ Default Theme */
.theme-default {
    /* Standard Bootstrap Colors */
}
```

## 🔄 API Integration

เชื่อมต่อกับ **BabyPOS-API** ผ่าน `ApiService`:

```csharp
// ตัวอย่างการใช้งาน
var shops = await ApiService.GetShopsAsync();
var menuItems = await ApiService.GetShopMenuItemsAsync(shopId);
var order = await ApiService.CreateOrderAsync(orderData);
```

## 🚀 การรัน

### Development
```powershell
cd BabyPOS-Web2
dotnet run
```

### Build
```powershell
dotnet build
```

### Watch (Hot Reload)
```powershell
dotnet watch run
```

## 🧪 Testing

```powershell
dotnet test ../BabyPOS-Web.Tests/
```

## ⚙️ Configuration

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7001"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## 📱 Responsive Design

### Grid Layout
- **Desktop**: 4 cards per row (col-md-3)
- **Tablet**: 2 cards per row
- **Mobile**: 1 card per row

### Card Specifications
- **Min Height**: 380px
- **Image Size**: 160px × 160px (square)
- **Equal Heights**: Flexbox layout สำหรับความสม่ำเสมอ

## 🎯 User Flows

### Customer Flow
1. **Shops** (`/shops`) → เลือกร้าน
2. **Food List** (`/food-list/{shopId}`) → เลือกอาหาร/Toggle On Hold
3. **Shop Orders** (`/shop-orders/{shopId}`) → ดูออเดอร์

### Admin Flow
- **Login** → **Shop Management** → **Food Management** → **Order Management**

## 🎨 Design System

### Colors (Pastel Theme)
- **Primary**: `hsl(142, 76%, 36%)` (สีเขียว)
- **Warning**: `hsl(38, 92%, 50%)` (สีส้ม - สำหรับ On Hold)
- **Card Background**: `hsl(0, 0%, 100%)` (สีขาว)
- **Text**: `hsl(222.2, 84%, 4.9%)` (สีเทาเข้ม)

### Button States
- **Edit (Normal)**: ตัวอักษรสีส้ม พื้นหลังขาว (`btn-outline-warning`)
- **On Hold**: ตัวอักษรขาว พื้นหลังส้ม (`btn-warning`)
- **Login**: ตัวอักษรขาว เส้นขอบขาว (`btn-outline-primary`)

## 📋 Dependencies

### Main Packages
- **Microsoft.AspNetCore.Components.WebAssembly.Server**
- **Microsoft.EntityFrameworkCore** (ถ้ามี local data)
- **System.Net.Http.Json** (สำหรับ API calls)

### Frontend Libraries
- **Bootstrap 5.3** (Grid, Components)
- **Font Awesome** (Icons)
- **Inter Font** (Typography)

## 🔄 Recent Updates

### v2.0 (Latest)
- ✅ Pastel Theme System เสร็จสมบูรณ์
- ✅ 4-column Grid Layout สำหรับ Food Cards
- ✅ On Hold Toggle Functionality (สีส้ม/ขาว)
- ✅ Navigation Flow แก้ไข (shop-orders → food-list)
- ✅ Login Button สีขาว (แก้ปัญหามองไม่เห็น)
- ✅ Remove "Add Shop" button จากหน้า Customer

### Upcoming Features
- 🔄 Real-time Order Updates
- 🔄 Print Receipt Functionality
- 🔄 Multiple Payment Methods
- 🔄 Inventory Management Integration

## 🤝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.

## 🔗 Related Projects

- **[BabyPOS-API](../BabyPOS-API/)**: Backend REST API
- **[BabyPOS-Web](../BabyPOS-Web/)**: WebAssembly Version (Legacy)
- **[Docs](../docs/)**: Complete Documentation
