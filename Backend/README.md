# VendSys Parser - Backend API

ASP.NET Core 9 Minimal API for parsing DEX files from vending machines following Nayax/VendSys specifications.

**Developed by:** Atilio Camargo Moreira
**Contact:** atiliosud@gmail.com

## Overview

This backend API provides two main endpoints for authenticating users and processing DEX (Data Exchange) files from vending machines. The system extracts specific data segments and stores them in a SQL Server database using clean architecture principles.

## Features

- ✅ **HTTP Basic Authentication** - Secure credential validation
- ✅ **DEX File Processing** - Parse and extract machine data
- ✅ **Clean Architecture** - Separated layers with dependency injection
- ✅ **Internationalization** - .resx resource files (PT-BR/EN-US)
- ✅ **Unit Testing** - Comprehensive test coverage (29/29 tests passing)
- ✅ **Error Handling** - Centralized exception management
- ✅ **Request Logging** - Minimal, structured logging

## Technical Stack

- **Framework:** ASP.NET Core 9 (Minimal API)
- **Database:** SQL Server LocalDB/Express
- **ORM:** Dapper (Entity Framework prohibited)
- **Testing:** xUnit with Moq
- **Architecture:** Clean Architecture (without Repository pattern)

## API Endpoints

### 1. Authentication
```http
POST /api/authenticate
Authorization: Basic <base64-encoded-credentials>
```

**Response:**
- `200 OK` - Authentication successful
- `401 Unauthorized` - Invalid credentials

### 2. DEX File Upload
```http
POST /api/dex
Authorization: Basic <base64-encoded-credentials>
Content-Type: multipart/form-data

file: [DEX file content]
```

**Response:**
- `200 OK` - File processed successfully
- `400 Bad Request` - Invalid file format
- `401 Unauthorized` - Authentication failed

## Project Structure

```
Backend/
├── VendSysParser/                    # Main API project
│   ├── Program.cs                    # Application entry point
│   ├── Api/
│   │   ├── Endpoints/                # API endpoint definitions
│   │   └── Middleware/               # Custom middleware
│   └── appsettings.json             # Configuration
├── VendSysParser.Application/        # Business logic layer
│   └── Services/                     # Application services
├── VendSysParser.Core/              # Domain models and DTOs
│   ├── DTOs/                        # Data Transfer Objects
│   └── Models/                      # Domain models
├── VendSysParser.Infrastructure/    # Data access layer
│   ├── DataAccess/                  # Database operations
│   └── Scripts/                     # Embedded SQL scripts
├── VendSysParser.Tests/             # Unit tests
│   ├── Services/                    # Service tests
│   └── DTOs/                        # DTO tests
└── Resources/                       # Localization files
    └── Messages.pt-BR.resx         # Portuguese resources
```

## Configuration

### Database Connection
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VendSysDB;Integrated Security=True"
  }
}
```

### Authentication Credentials
```json
{
  "Authentication": {
    "Username": "vendsys",
    "Password": "NFsZGmHAGWJSZ#RuvdiV"
  }
}
```

## DEX File Format

The system processes DEX files with the following segments:

### Required Segments
- **ID1**: Machine identification
  - Position 1: Machine Serial Number (ID101)
  - Position 6: Machine ID (ID106)
- **VA1**: Value data
  - Position 1: Value of Paid Vends (VA101)
- **PA1**: Product data
  - Position 1: Product Identifier (PA101)
  - Position 2: Product Price (PA102)
- **PA2**: Product sales data
  - Position 1: Number of Vends (PA201)
  - Position 2: Value of Paid Sales (PA202)

### Sample DEX File
```
DXS*9V0001
ID1*1234567890*2*ID106*TEST_MACHINE*5*6
ST*001*0001
VA1*135
PA1*COLA*100
PA2*5*500
PA1*CHIPS*150
PA2*3*450
G85*1
SE*12*0001
DXE*1*1
```

## Database Schema

### DEXMeter Table
```sql
CREATE TABLE DEXMeter (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MachineID NVARCHAR(50) NOT NULL,
    DEXDateTime DATETIME2 NOT NULL,
    MachineSerialNumber NVARCHAR(50) NOT NULL,
    ValueOfPaidVends DECIMAL(18,2) NOT NULL
);
```

### DEXLaneMeter Table
```sql
CREATE TABLE DEXLaneMeter (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DEXMeterId INT NOT NULL,
    ProductIdentifier NVARCHAR(50) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    NumberOfVends INT NOT NULL,
    ValueOfPaidSales DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (DEXMeterId) REFERENCES DEXMeter(Id)
);
```

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server LocalDB or SQL Server Express
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/atiliosud/vendsys-parser.git
   cd vendsys-parser/Backend
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Setup database**
   - Run the SQL scripts in `/Database` folder
   - Update connection string in `appsettings.json`

4. **Run the application**
   ```bash
   dotnet run --project VendSysParser
   ```

5. **Run tests**
   ```bash
   dotnet test
   ```

The API will be available at `http://localhost:5000`

## Testing

### Unit Tests
The project includes comprehensive unit tests:
- **29 tests total** - All passing ✅
- **Service tests** - Authentication and DEX parsing logic
- **DTO tests** - Request/response model validation
- **Mock testing** - Using Moq framework for dependencies

Run tests:
```bash
dotnet test --verbosity normal
```

### Manual Testing
Use tools like Postman or curl to test the endpoints:

```bash
# Authentication test
curl -X POST http://localhost:5000/api/authenticate \
  -H "Authorization: Basic dmVuZHN5czpORnNaR21IQUdXSlNaI1J1dmRpVg=="

# DEX file upload test
curl -X POST http://localhost:5000/api/dex \
  -H "Authorization: Basic dmVuZHN5czpORnNaR21IQUdXSlNaI1J1dmRpVg==" \
  -F "file=@sample.dex"
```

## Architecture Decisions

### Clean Architecture
- **Separation of Concerns**: Each layer has specific responsibilities
- **Dependency Inversion**: Dependencies point inward toward the domain
- **No Repository Pattern**: Direct data access using Dapper (as per requirements)

### Minimal API Pattern
- **Lightweight**: No controller overhead
- **Modern approach**: Leverages .NET 9 features
- **Simple routing**: Direct endpoint mapping

### Error Handling
- **Global exception middleware**: Centralized error processing
- **Structured logging**: Minimal but informative logs
- **User-friendly responses**: Localized error messages

## Development Guidelines

### Code Standards
- **No hardcoded strings**: All text in .resx files
- **Minimal logging**: Only critical events
- **English in code**: PT-BR in user messages
- **Clean code**: KISS principle applied

### Prohibited Patterns
- Entity Framework Core
- Repository pattern
- CQRS/MediatR
- Complex DI containers
- JWT/OAuth (Basic Auth only)

## License

This project is developed for Atilio Camargo Moreira as a technical demonstration.

## Contact

**Atilio Camargo Moreira**
Email: atiliosud@gmail.com

---

*This README covers the backend API implementation. Frontend documentation will be added when the React application is developed.*