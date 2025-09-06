# ASCO_Backend
# 🚀 ASCO Services API

A **.NET 8 Web API** for managing users, organizations, and JWT-based authentication. This project demonstrates secure authentication, role-based authorization, and follows a clean architecture with Repository and Service layers.

---

## 📂 Project Structure
/AESCO ├── Controllers/ ├── DbContext/ ├── Models/ ├── Repositories/ ├── Services/ └── Program.cs


---

## 🛠️ Tech Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core (PostgreSQL)
- JWT Authentication
- Swagger (OpenAPI) for API documentation
- Postman for basic testing.
---

## 🚧 Features

- ✅ JWT Authentication & Authorization
- ✅ Role-based Access Control
- ✅ User Management (CRUD)
- ✅ Organization Management (CRUD)
- ✅ Secure Password Hashing with BCrypt
- ✅ TPT Inheritance in EF Core
- ✅ Clean Architecture (Repository + Service Pattern)
- ✅ API Documentation with Swagger

---

## 📦 Setup Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker](https://www.docker.com/)

---

### 1. Clone the Repository

```bash
git clone https://github.com/ibtehaj421/AESCO_Backend.git
cd AESCO_Backend

```
### 2. Application Settings
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ascodb;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Key": "your-secure-jwt-key",
    "Issuer": "ASCOServices"
  }
}

### 3. Run migrations and update database
```bash
dotnet ef database update
```

### 4. Run the project basic
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

### 5. Run Docker compose
```bash
--to only run the container with backend

docker-compose up backend

--to only run the db container

docker-compose up db

--run backend and db together

docker-compose up backend db

--to run the tests

docker-compose run tests

--to run everything

docker compose up --build
```

