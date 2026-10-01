# 🏢 Company Profile

A work-in-progress Company Profile application built with **.NET 10**, combining an ASP.NET Core Minimal API backend with a Blazor Web App frontend.

The project is developed incrementally with a focus on clean architecture, security, maintainability, localization, and practical full-stack .NET development.

---

## 🚧 Project Status

This project is still under active development.

New features, improvements, security enhancements, UI work, and refactoring are continuously being added.

> Note: The current implementation is not the final version and may change as the project evolves.

---

## ✨ Features

### Backend

* .NET 10 Minimal APIs
* Native OpenAPI
* Scalar API documentation
* Entity Framework Core
* SQLite database
* ASP.NET Core Identity
* Cookie-based session authentication
* Secure session token hashing
* Session management
* Sliding and absolute session expiration
* User-Agent verification
* Configurable CORS
* DTO-based API design
* Arabic and English localization
* Validation with localized messages

### Frontend

* Blazor Web App
* Server-side and interactive Blazor capabilities
* API-based data consumption
* Arabic and English UI support
* Integration with the Company Profile API
* Reusable components and services
* Session-based authentication integration

---

## 📂 Solution Structure

```text
CompanyProfile
│
├── CompanyProfile.Api
│   ├── Endpoints
│   ├── Services
│   ├── Program.cs
│   └── appsettings.json
│
├── CompanyProfile.Shared
│   ├── Common
│   ├── DTOs
│   └── Resources
│
├── CompanyProfile.Infrastructure
│   ├── Data
│   ├── Entities
│   ├── Migrations
│   └── company.db
│
├── CompanyProfile.Web
│   ├── CompanyProfile.Web
│   │   ├── Components
│   │   ├── Services
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   └── CompanyProfile.Web.Client
│       ├── Components
│       ├── Pages
│       └── ...
│
└── CompanyProfile.slnx
```

---

## 🏗️ Architecture

The solution is separated into dedicated projects:

* **CompanyProfile.Api** — ASP.NET Core Minimal API and backend application services.
* **CompanyProfile.Web** — Blazor Web App responsible for the user interface.
* **CompanyProfile.Shared** — Shared DTOs, common models, and localization resources.
* **CompanyProfile.Infrastructure** — Database access, Entity Framework Core configuration, entities, and migrations.

The Blazor application communicates with the backend through the Company Profile API rather than accessing the database directly.

---

## 🔐 Authentication

The API uses **stateful cookie-based sessions** instead of JWT authentication.

Session management includes:

* Secure session cookies
* Session token hashing
* Sliding expiration
* Absolute expiration
* Session revocation
* User-Agent verification
* Active session management

The Blazor Web App integrates with the API authentication flow through the session cookie.

---

## 🌍 Localization

The application supports:

* 🇸🇦 Arabic
* 🇬🇧 English

Localization is used for both company content and validation messages.

---

## 🚀 Getting Started

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* Git

### Clone the repository

```bash
git clone https://github.com/NoofSaeed/CompanyProfile.git
cd CompanyProfile
```

### Run the API

```bash
cd CompanyProfile.Api
dotnet run
```

The API documentation is available through Scalar:

```text
https://localhost:<port>/scalar/v1
```

The port may differ depending on your local configuration.

### Run the Blazor Web App

Open another terminal:

```bash
cd CompanyProfile.Web
dotnet run
```

Make sure the API is running and that the API base URL configured in the Blazor application points to the correct API address.

---

## 🛠️ Technologies

### Backend

* C#
* .NET 10
* ASP.NET Core Minimal APIs
* Entity Framework Core
* SQLite
* ASP.NET Core Identity
* OpenAPI
* Scalar

### Frontend

* Blazor Web App
* Razor Components
* .NET 10
* C#

---

## 📌 Project Goals

The project is being built as a practical full-stack .NET application with an emphasis on:

* Clean and maintainable architecture
* Secure authentication and session management
* API-first communicationMO
* Reusable components
* Localization
* Simple project structure
* Incremental development

---

## ⭐ Feedback

This project is being built and improved step by step.

If you find it useful, feel free to ⭐ Star the repository and share your feedback.
