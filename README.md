# 🍽️ BabyPOS

**BabyPOS** เป็นระบบ POS (Point of Sale) สำหรับร้านอาหารและคาเฟ่ ที่พัฒนาด้วย Clean Architecture ประกอบด้วย Web UI, REST API, และ Database แบบครบวงจร

---

## 🚀 Quick Start

### 1. รัน API Server
```powershell
cd BabyPOS-API
dotnet run
# API: https://localhost:7001
# Swagger: https://localhost:7001/swagger
```

### 2. รัน Web Application
```powershell
cd BabyPOS-Web2
dotnet run  
# Web: https://localhost:7000
```

### 3. เข้าใช้งาน
- **Customer**: เข้า `/shops` เลือกร้าน → ดูเมนู → สั่งอาหาร
- **Admin**: Login → จัดการร้าน → จัดการเมนู → ดูออเดอร์

---

## 🎯 Core Features

### 🛍️ Customer Experience
- **Shop Selection**: เลือกร้านค้าจากรายการทั้งหมด
- **Menu Browsing**: ดูเมนูอาหารแบบ Grid 4 คอลัมน์ พร้อมรูปภาพ
- **Category Filter**: แยกหมวดหมู่ (อาหาร, ของหวาน, เครื่องดื่ม, อื่นๆ)
- **On Hold System**: ระบบ Toggle รายการ "On Hold" (สีส้ม/ขาว)

### 🔧 Admin Management  
- **Shop Management**: จัดการข้อมูลร้านค้า
- **Menu Management**: เพิ่ม/แก้ไข/ลบ เมนูอาหาร
- **Table Management**: จัดการโต๊ะในร้าน
- **Order Tracking**: ติดตามออเดอร์แบบ Real-time

### 🎨 Theme System
- **Pastel Theme**: ธีมสีพาสเทลสวยงาม เหมาะสำหรับร้านอาหาร
- **Theme Switching**: เปลี่ยนธีมได้แบบ Real-time
- **Responsive Design**: รองรับทุกขนาดหน้าจอ

---

## 🏗️ Solution Architecture

### Projects Overview
```
BabyPOS/
├── 🌐 BabyPOS-Web2/          # Blazor Server (Main Frontend)
├── 🌐 BabyPOS-Web/           # Blazor WebAssembly (Legacy)
├── 🔌 BabyPOS-API/           # ASP.NET Core Web API
├── 🧪 BabyPOS-API.Tests/     # API Unit Tests
├── 🧪 BabyPOS-Web.Tests/     # Web Unit Tests
├── 📚 docs/                  # Complete Documentation
└── 📋 README.md              # This file
```

### Technology Stack
- **Frontend**: Blazor Server, Bootstrap 5, Font Awesome
- **Backend**: ASP.NET Core 8, Entity Framework Core
- **Database**: SQL Server (LocalDB for development)
- **Authentication**: JWT Token-based
- **Architecture**: Clean Architecture + Domain-Driven Design

---

## 📋 Current Features (v2.0)

### ✅ Completed
- ✅ **Complete Theme System** (Pastel + Default themes)
- ✅ **4-Column Food Grid** with responsive design
- ✅ **On Hold Toggle System** (Orange/White button states)
- ✅ **Navigation Flow** (shops → food-list → shop-orders)
- ✅ **JWT Authentication** with secure token storage
- ✅ **Table Management** CRUD operations
- ✅ **Food Categories** (Main Dish, Dessert, Drink, Other)
- ✅ **Image Support** for menu items
- ✅ **Customer vs Admin** different interfaces

### 🔄 In Progress
- 🔄 Real-time Order Updates with SignalR
- 🔄 Print Receipt Functionality
- 🔄 Inventory Management Integration

### 📅 Planned Features
- 📅 Payment Gateway Integration
- 📅 QR Code Menu for tables
- 📅 Sales Reporting Dashboard
- 📅 Multi-restaurant Support
- 📅 Mobile App (React Native/Flutter)

---

## 🎨 Design Philosophy

### UI/UX Principles
- **Blazor Component First**: ใช้ Blazor Components แทน HTML/JS
- **Clean & Modern**: ดีไซน์สะอาด ใช้งานง่าย
- **Responsive**: รองรับทุกอุปกรณ์
- **Accessible**: ปฏิบัติตาม Web Accessibility Guidelines

### Code Quality
- **Clean Architecture**: แยกชั้น Business Logic, Data Access, UI
- **SOLID Principles**: ออกแบบโค้ดแบบ Maintainable
- **Unit Testing**: ครอบคลุม Business Logic และ API
- **Documentation**: เอกสารครบถ้วน

---

## 🚀 Development Setup

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 หรือ VS Code
- SQL Server (LocalDB)
- Git

### Clone & Run
```powershell
# 1. Clone repository
git clone https://github.com/vvasantg/BabyPOS.git
cd BabyPOS

# 2. Setup Database
cd BabyPOS-API
dotnet ef database update

# 3. Run API (Terminal 1)
dotnet run
# API จะรันที่ https://localhost:7001

# 4. Run Web (Terminal 2)  
cd ../BabyPOS-Web2
dotnet run
# Web จะรันที่ https://localhost:7000
```

### Testing
```powershell
# API Tests
dotnet test BabyPOS-API.Tests/

# Web Tests  
dotnet test BabyPOS-Web.Tests/

# All Tests
dotnet test
```

---

## 📖 Documentation

### Complete Guides
- [1. 📋 Overview](docs/01-overview.md) - ภาพรวมระบบ
- [2. 🏗️ Solution Structure](docs/02-solution-structure.md) - โครงสร้างโปรเจกต์
- [3. ⭐ Features](docs/03-features.md) - ฟีเจอร์ทั้งหมด
- [4. 🔧 System Modules](docs/04-system-modules.md) - โมดูลระบบ
- [5. 🎨 Order Flow & UI Design](docs/05-order-flow-ui.md) - การออกแบบ UI
- [6. 🛍️ Shop Management](docs/06-shop-management.md) - การจัดการร้าน
- [7. 📊 Reporting & Dashboard](docs/07-reporting-dashboard.md) - รายงานและแดชบอร์ด
- [8. 🖥️ Main Screen](docs/08-main-screen.md) - หน้าจอหลัก
- [9. 🗺️ Roadmap](docs/09-roadmap.md) - แผนการพัฒนา
- [10. 💻 Implementation Notes](docs/10-implementation-notes.md) - หมายเหตุการพัฒนา
- [11. 🚀 Future Enhancements](docs/11-future-enhancements.md) - การพัฒนาในอนาคต
- [12. 📞 Contact & Contribution](docs/12-contact.md) - การติดต่อและมีส่วนร่วม
- [13. 📄 License](docs/13-license.md) - สัญญาอนุญาต

### Project READMEs
- [🌐 BabyPOS-Web2](BabyPOS-Web2/README.md) - Blazor Server Frontend
- [🔌 BabyPOS-API](BabyPOS-API/README.md) - REST API Backend
- [🌐 BabyPOS-Web](BabyPOS-Web/README.md) - WebAssembly Frontend (Legacy)

---

## 🎯 User Flows

### 👥 Customer Journey
```
1. /shops → เลือกร้านที่ต้องการ
2. /food-list/{shopId} → ดูเมนู + Toggle On Hold
3. /shop-orders/{shopId} → ดูออเดอร์ที่สั่ง
```

### 👨‍💼 Admin Journey
```  
1. /login → เข้าสู่ระบบ
2. /shops-management → จัดการร้าน
3. /food-list/{shopId} → จัดการเมนู + โต๊ะ
4. /shop-orders/{shopId} → จัดการออเดอร์
```

---

## 🎨 Theme Showcase

### Pastel Theme (Default)
- **Primary**: สีเขียวพาสเทล `hsl(142, 76%, 36%)`
- **Cards**: สีขาวสะอาด พร้อมเงาอ่อนๆ
- **Buttons**: สีสันสดใส แต่ไม่ฉูดฉาด
- **Typography**: Inter font, clean และ readable

### Component Examples
```html
<!-- Theme Selector -->
<ThemeSelector />

<!-- Food Card with On Hold -->
<div class="card">
  <img src="food.jpg" />
  <div class="card-body">
    <h5>ผัดไทย</h5>
    <div class="food-price">฿45</div>
    <button class="btn btn-outline-warning">แก้ไข</button>
  </div>
</div>
```

---

## 🤝 Contributing

### Development Workflow
1. **Fork** the repository
2. **Create** feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** changes (`git commit -m 'Add amazing feature'`)
4. **Push** to branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### Code Standards
- ใช้ **C# Conventions** standard
- **Component-based Architecture** สำหรับ Blazor
- **Clean Architecture** สำหรับ business logic
- **Unit Tests** สำหรับ feature ใหม่

---

## 📞 Support & Contact

### Issues & Bugs
- 🐛 **Bug Reports**: [GitHub Issues](https://github.com/vvasantg/BabyPOS/issues)
- 💡 **Feature Requests**: [GitHub Discussions](https://github.com/vvasantg/BabyPOS/discussions)

### Community
- 📧 **Email**: [developer contact]
- 💬 **Discord**: [community server]
- 📱 **Twitter**: [@BabyPOS_Dev]

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **Microsoft**: สำหรับ .NET และ Blazor framework
- **Bootstrap Team**: สำหรับ UI components
- **Font Awesome**: สำหรับ beautiful icons
- **Community**: สำหรับ feedback และ contributions

---

> **⭐ ถ้าโปรเจกต์นี้มีประโยชน์ กรุณา Star บน GitHub เพื่อสนับสนุนนักพัฒนา!**
