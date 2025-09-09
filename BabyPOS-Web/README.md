# BabyPOS-Web

Frontend Blazor WebAssembly สำหรับระบบ BabyPOS

## Features
- หน้า Home (ShopList): แสดงร้านค้า, รูปอาหาร, ปุ่มดูเมนู
- Shop Management: จัดการร้าน, เพิ่มร้าน, Carousel รูปอาหาร
- Food List: แสดงเมนูอาหาร, filter ตามหมวดหมู่, SPA navigation
- Login/Register: ระบบล็อกอิน, JWT
- Responsive UI (Bootstrap)
- Configurable API base URL (รองรับ external API)

## โครงสร้าง
- Pages/: Razor Pages หลัก (ShopList, ShopManagement, FoodList, ฯลฯ)
- Components/Shop/: CardCarousel, ShopCard
- Models/: ShopVM, MenuItemVM
- wwwroot/: Static files, config

## การรัน
```powershell
dotnet run --project .\BabyPOS-Web\BabyPOS-Web.csproj
```

## Config
- `wwwroot/config.json` สำหรับตั้งค่า API base URL
