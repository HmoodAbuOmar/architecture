# KASHOP E-Commerce API

A backend REST API for an e-commerce platform built with **ASP.NET Core**.  
The project uses a layered architecture and supports authentication, payments, and localization.

## Project Structure
- `KASHOP.PL` (API / Controllers)
- `KASHOP.BLL` (Business Logic / Services)
- `KASHOP.DAL` (Data Access / EF Core)

## Key Features
- Authentication & Authorization using **ASP.NET Identity + JWT**
- Role-based access (Admin/Customer + any custom roles)
- SQL Server + **Entity Framework Core**
- **Stripe** integration for payments
- Localization (EN/AR) using `?lang=ar`
- Swagger/OpenAPI (Development)
- Database seeding on startup (`ISeedData`)
- **CORS Policy enabled** for frontend access

## Tech Stack
- ASP.NET Core
- SQL Server
- Entity Framework Core
- ASP.NET Identity
- JWT Bearer Authentication
- Stripe
