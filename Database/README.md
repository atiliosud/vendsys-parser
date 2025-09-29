# VendSys Database

SQL Server database for the VendSys DEX Parser application.

## Contents

- **CreateTables.sql** - Database schema with DEXMeter and DEXLaneMeter tables
- **StoredProcedures.sql** - Stored procedures for data operations
- **VendSysDEX.bak** - Complete database backup with sample data

## Database Schema

### Tables

**DEXMeter**

- Id (int, PK)
- MachineID (varchar(50))
- DEXDateTime (datetime2)
- MachineSerialNumber (varchar(50))
- ValueOfPaidVends (decimal(10,2))

**DEXLaneMeter**

- Id (int, PK)
- DEXMeterId (int, FK)
- ProductIdentifier (varchar(50))
- Price (decimal(10,2))
- NumberOfVends (int)
- ValueOfPaidSales (decimal(10,2))

## Quick Setup

### Option 1: Restore from Backup (Recommended)

```sql
-- Restore the database backup
RESTORE DATABASE VendSysDEX
FROM DISK = '[YourProjectPath]\VendSysParser\Database\VendSysDEX.bak'
WITH REPLACE,
MOVE 'VendSysDEX' TO 'C:\Users\[YourUsername]\VendSysDEX.mdf',
MOVE 'VendSysDEX_log' TO 'C:\Users\[YourUsername]\VendSysDEX_log.ldf'
```

### Option 2: Create from Scripts

1. Execute CreateTables.sql to create the database and tables
2. Execute StoredProcedures.sql to create the stored procedures

### Using SQL Server LocalDB

```bash
# Connect to LocalDB
sqlcmd -S "(LocalDB)\MSSQLLocalDB"

# Restore the backup
1> RESTORE DATABASE VendSysDEX FROM DISK = 'path\to\VendSysDEX.bak' WITH REPLACE
2> GO
```

## Connection String

```json
"Data Source=(LocalDB)\\MSSQLLocalDB;Database=VendSysDEX;Integrated Security=True"
```

## Stored Procedures

- **SaveDEXMeter** - Inserts DEX meter data
- **SaveDEXLaneMeter** - Inserts product lane data

## Sample Data

The backup contains sample DEX parsing results from test vending machines, demonstrating the data structure and relationships.