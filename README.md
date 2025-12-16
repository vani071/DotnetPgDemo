# DotnetPgDemo - .NET PostgreSQL API

A simple ASP.NET Core API demonstrating database operations with Entity Framework and PostgreSQL.

## Prerequisites

- .NET 10 SDK or later
- PostgreSQL 12 or later
- Visual Studio Code, Visual Studio, or any .NET IDE

## Installation & Setup

### 1. Clone or Open the Project

```bash
cd v:\repo\DotnetPgDemo
```

### 2. Configure Database Connection

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=DotnetPgDemo;Username=postgres;Password=your_password"
}
```

### 3. Restore NuGet Packages

```bash
dotnet restore
```

### 4. Apply Database Migrations

```bash
cd DotnetPgDemo.Api
dotnet ef database update
```

This will create the database and apply all migrations.

### 5. Run the Application

```bash
dotnet run
```

The API will start on `https://localhost:5064` (or `http://localhost:5064`).

## API Endpoints

### Create Person
**POST** `/api/people`

Request body:
```json
{
  "firstName": "John",
  "lastName": "Doe"
}
```

Response:
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe"
}
```

## Project Structure

- `DotnetPgDemo.Api/` - Main API project
  - `Controllers/` - API controllers
  - `Models/` - Database models and DbContext
  - `Migrations/` - Entity Framework migrations
  - `Program.cs` - Application startup configuration

## Environment Variables

Optional: Set environment-specific configurations in `appsettings.Development.json` for development.

## Troubleshooting

**Connection string issues**: Ensure PostgreSQL is running and the credentials are correct.

**Migration errors**: Run `dotnet ef database drop` to reset the database if needed, then apply migrations again.

**Port conflicts**: Change the port in `launchSettings.json` if port 5064 is already in use.

## Development

To create a new migration after model changes:

```bash
dotnet ef migrations add YourMigrationName
dotnet ef database update
```
