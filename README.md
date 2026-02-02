# 🛍️ KASHOP E-Commerce Backend API (ASP.NET Core)

**KASHOP** is a RESTful E-Commerce Web API built with **ASP.NET Core** using a clean **Layered Architecture**:
- **PL** (Presentation Layer)
- **BLL** (Business Logic Layer)
- **DAL** (Data Access Layer)

The project includes **Authentication & Authorization** using **ASP.NET Identity + JWT**, payment integration via **Stripe**, and **Localization (EN/AR)** using `?lang=ar`.

---

## 📌 API Documentation (Postman) — Source of Truth
All endpoints with examples (requests/responses) are documented here:

- Public Docs: https://documenter.getpostman.com/view/50742279/2sB3dSPopS  
- Workspace Docs: https://www.postman.com/hmoodabuomar2018-9999836/workspace/asp12/documentation/50742279-cc66be41-fec6-48ba-8488-5dd39cf48bbd  

---

## 🚀 Features
- 🔐 **Authentication & Authorization**: ASP.NET Identity + JWT Bearer
- 🧑‍⚖️ **Role-based Access Control** (Admin/Customer + any custom roles)
- 🛒 **E-Commerce Modules**: Products, Categories, Orders, Reviews (and more as implemented)
- 💳 **Stripe Payment Integration**
- 🌍 **Localization**: English / Arabic via `?lang=ar`
- 🌱 **Seed Data** runs on startup using `ISeedData`
- 📄 **Swagger / OpenAPI** (enabled in Development)

---

## 🧠 Tech Stack
- Backend: ASP.NET Core (C#)
- Database: SQL Server
- ORM: Entity Framework Core
- Auth: ASP.NET Identity + JWT
- Payments: Stripe
- Mapping: Mapster

---

## 🏗️ Architecture
This project follows a layered approach to separate concerns:
- **Controllers (PL):** HTTP endpoints and request handling
- **Services (BLL):** business logic and validation
- **Repositories / DbContext (DAL):** data access and persistence
- **DTOs:** request/response models to keep API clean and stable

---

## 🗂️ Project Structure
- `KASHOP.PL`  → Controllers, Middleware, Program.cs
- `KASHOP.BLL` → Services, Business Rules, Mapster Configurations
- `KASHOP.DAL` → DbContext, Entities/Models, Repositories, DTOs, Utilities

---

## 🔐 Authentication & Authorization

### ✅ Auth Endpoints (Confirmed from your code)
Base route: `api/auth/account`

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/account/login` | Login and get JWT |
| POST | `/api/auth/account/register` | Register a new user |
| GET  | `/api/auth/account/confirmemail?token=...&userId=...` | Confirm Email |

> ✅ For all other endpoints (Admin/Customer/Products/Orders/Cart/Reviews...), check Postman docs above.

---

## 🌍 Localization
Supported cultures:
- `en` (default)
- `ar`

Usage:
- Add `?lang=ar` to any request.

Example:
- `/api/customer/products?lang=ar`

---

## 💳 Stripe Configuration
Stripe key is read from:
- `Stripe:SecretKey`

> ⚠️ Never commit real Stripe keys to a public repository.

---

## 🛠️ Installation & Setup

### ✅ Prerequisites
- .NET SDK
- SQL Server
- Visual Studio 2022 / VS Code

### ⚙️ Configuration
Set the following keys using **Environment Variables** (recommended for production) or local development config:

- `ConnectionStrings:DefaultConnection`
- `Jwt:SecretKey`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Stripe:SecretKey`

> ⚠️ IMPORTANT: Do NOT commit real secrets (JWT/Stripe/DB passwords) to GitHub.

### 🧭 Run Migrations
```bash
dotnet ef database update --project KASHOP.DAL --startup-project KASHOP.PL
