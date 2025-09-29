# VendSys DEX Parser

Full-stack application for parsing DEX (Data Exchange) files from vending machines following Nayax/VendSys specifications.

**Developed by:** Atilio Camargo Moreira
**Contact:** atiliosud@gmail.com

## Quick Start

### 1. Backend (ASP.NET Core 9)
```bash
cd Backend/VendSysParser
dotnet restore
dotnet run
# API runs on https://localhost:7297
```

### 2. Frontend (React + TypeScript)
```bash
cd Frontend
npm install
npm run dev
# App runs on http://localhost:5173
```

### 3. Testing with Sample Files
Use the DEX files in `/Samples` folder to test the parsing functionality:
- Login with credentials: `vendsys` / `NFsZGmHAGWJSZ#RuvdiV`
- Upload any `.txt` file from the Samples folder
- View parsed data stored in SQL Server LocalDB

## Project Structure

```
VendSysParser/
├── Backend/                 # ASP.NET Core 9 Minimal API
├── Frontend/                # React + TypeScript + Vite
├── Database/                # SQL scripts and backup
│   ├── CreateTables.sql    # Database schema
│   ├── StoredProcedures.sql # Stored procs for data operations
│   └── VendSysDEX.bak      # Database backup file
├── Samples/                 # DEX test files (.txt)
├── docs/                    # Documentation
└── README.md               # This file
```

## Features

- **Authentication**: HTTP Basic Auth
- **DEX Parsing**: Extract machine data from vending machine files
- **Clean Architecture**: Separated frontend/backend with proper APIs
- **Nayax Style Guide**: Professional UI following brand guidelines
- **Internationalization**: Multi-language support ready

## Parsed DEX Data

The system extracts and stores:
- **Machine Info**: Serial number, Machine ID
- **Sales Data**: Total paid vends value
- **Product Data**: Identifier, price, vend count, sales value

## Documentation

- **Backend Details**: See [Backend/README.md](Backend/README.md)
- **Frontend Details**: See [Frontend/README.md](Frontend/README.md)
- **Database Setup**: See [Database/README.md](Database/README.md)

## Sample DEX Files

Test files are available in the `/Samples` directory. These represent real vending machine data exports that the system can parse and process.

---

**Built for efficient vending machine data management**