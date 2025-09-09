# BabyPOS Development Task List

## 1. Initial Setup
- Create solution/project structure: BabyPOS-Web (Blazor), BabyPOS-API (.NET Core), Database (PostgreSQL)
- Prepare config files for DB/API connection

## 2. Database Design
- Design ER Diagram: User, Role, Shop, Menu, Table, Order, OrderItem, AuditLog
- Create migration/scripts for PostgreSQL

## 3. API Development (BabyPOS-API)
- Create Entity/Model based on DB
- Implement Repository/Service Layer
- Create Controllers: Auth, Shop, Menu, Table, Order, Report
- Implement JWT Authentication
- Prepare endpoint for social login (stub only)
- Enable Swagger

## 4. UI Development (BabyPOS-Web)
- Create Blazor Project
- Design pages: Login/Register, Dashboard, Shop Management, Menu Management, Order, Report
- Connect to API via HttpClient

## 5. Core Features Implementation
- Member & Role system
- Shop management (CRUD)
- Menu management
- Table management
- Order system (open/add/close bill)
- Reporting system

## 6. Multi-language & Config
- Prepare resource files for Thai/English
- Separate config for DB/API

## 7. Testing & Validation
- Create Unit/Integration Tests for API
- Test UI flow

## 8. Dockerization
- Prepare Dockerfile/Compose for each tier

## 9. Documentation
- Update README.md and docs

## 10. Future Enhancements
- Prepare stubs for payment, notification, cloud storage, etc.

---

> Use this checklist to track development progress. Update status as tasks are completed.
