# 🌿 GreenRoots — Tree Plantation Platform

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-ORM-green)](https://docs.microsoft.com/en-us/ef/core/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> A community-driven tree plantation platform to help fight climate change — one tree at a time.

---

## 🌍 About the Project

**GreenRoots** is a full-stack web application built with **ASP.NET Core MVC**. It allows users to submit tree-planting requests for specific locations while administrators manage and track the plantation progress through a secure management portal.

This project was built as an interview-ready showcase demonstrating best practices in:
- Clean N-Tier Architecture (Models → Services → Controllers → Views)
- Secure Authentication & Role-Based Authorization
- Entity Framework Core data management
- Modern, responsive UI with Bootstrap 5

---

## ✨ Features

| Feature | Description |
|---|---|
| 🔐 Secure Auth | Cookie-based authentication with BCrypt password hashing |
| 👤 RBAC | Role-based access for **Users** and **Admins** |
| 📊 Dashboards | Personalized stats: Total Requested, Pending, Planted |
| 🌱 Tree Requests | Submit requests with location, tree count, and message |
| ✅ Status Management | Admin can mark requests as Paid or Planted |
| 🛡️ CSRF Protection | Auto AntiForgery tokens on all forms |
| 📱 Responsive UI | Mobile-first Bootstrap 5 design |

---

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core MVC
- **ORM**: Entity Framework Core 9.0
- **Database**: SQL Server (LocalDB for development)
- **Frontend**: Razor Views (`.cshtml`), Bootstrap 5
- **Security**: BCrypt.Net-Next, ASP.NET Core Cookie Authentication

---

## 🗂️ Project Structure

```text
GreenRoots/
├── Controllers/        # MVC Controllers (Account, Dashboard, Requests, Home)
├── Data/               # EF Core DbContext
├── DTOs/               # Data Transfer Objects
├── Models/             # Entity Models (User, TreeRequest)
├── Services/           # Business Logic (AuthService, TreeRequestService)
├── Views/              # Razor Views (.cshtml)
│   ├── Account/        # Login, Register, AccessDenied
│   ├── Dashboard/      # UserIndex, AdminIndex
│   ├── Home/           # Index (Landing Page)
│   ├── Requests/       # Submit, MyRequests
│   └── Shared/         # _Layout.cshtml
├── wwwroot/            # Static assets (CSS, JS, images)
├── appsettings.json    # App configuration
└── Program.cs          # App entry point & middleware
```

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server Express with LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

### 1. Clone the Repository
```bash
git clone https://github.com/harsha-vardhan-hub/GreenRoots.git
cd GreenRoots
```

### 2. Configure the Application
The app uses `appsettings.json` for configuration. Create `appsettings.Development.json` (already git-ignored) with your local secrets:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  }
}
```

> **Note**: Never commit real secrets or connection strings to version control. Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables for production.

### 3. Run the Application
```bash
dotnet run
```

The database schema is created **automatically** on first launch via `EnsureCreated()`. No manual migrations required!

Navigate to `http://localhost:5139` in your browser.

---

## 🧪 Testing the Application

1. **Register** a new user account via `/Account/Register`
2. **Submit** a tree planting request via the *Plant a Tree* page
3. Simulate payment in the interactive modal
4. **Register an Admin** account (check "Register as Admin" — for demo purposes)
5. Login as Admin → view all requests → click **Mark Paid** or **Mark Planted**


## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

*Built with 💚 to make the world a greener place.*
