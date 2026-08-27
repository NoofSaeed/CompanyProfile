# 🏢 Company Profile API (.NET 10 & Minimal APIs)

A comprehensive, production-ready educational Web API project representing a Company Profile. Built using **.NET 10** and the latest **Minimal APIs** standards. The project implements a multi-project architecture to enforce **Separation of Concerns**, making it highly optimized for frontend integration (React, Angular, Vue).

### ✨ Key Features
* **Native OpenAPI Specification**: Replaced legacy Swagger libraries with the built-in, native OpenAPI generation introduced in .NET 10.
* **Scalar UI Integration**: Out-of-the-box integration with the beautiful and modern Scalar API reference client.
* **Multi-Project Architecture**: Structured into 3 decoupled layers (API, Core, and Infrastructure) mimicking enterprise setups.
* **DTO Pattern Enforcement**: Database models are strictly isolated. All operations communicate via Data Transfer Objects (DTOs) for maximum security.
* **CORS Pre-configured**: Pre-configured Cross-Origin Resource Sharing policies to seamlessly hook into your local frontend development servers.
* **Full Admin CRUD Capabilities**: Complete endpoints designed for both public views and admin dashboard manipulations (Manage info, add/edit/delete services).

### 📂 Solution Breakdown
1. **`CompanyProfile.Core`**: Pure class library containing domain Entities and DTO records with zero external framework dependencies.
2. **`CompanyProfile.Infrastructure`**: Data access layer managing SQLite database persistence and seeding via Entity Framework Core.
3. **`CompanyProfile.API`**: The application entry point hosting the Minimal API endpoints, middleware routing, and configuration.

### 🚀 Getting Started
Ensure you have the .NET 10 SDK installed, then run the following in your terminal:

```bash
# Navigate to the API entry project
cd CompanyProfile.API

# Run the project
dotnet run
```
*Your browser will automatically open the Scalar UI doc page at:* `https://localhost:7158/scalar/v1` *(or your custom configured localhost port).*
