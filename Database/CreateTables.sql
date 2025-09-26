-- Create VendSysDEX Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VendSysDEX')
BEGIN
    CREATE DATABASE VendSysDEX;
END
GO

USE VendSysDEX;
GO

-- Drop existing tables if they exist
IF OBJECT_ID('dbo.DEXLaneMeter', 'U') IS NOT NULL
    DROP TABLE dbo.DEXLaneMeter;
GO

IF OBJECT_ID('dbo.DEXMeter', 'U') IS NOT NULL
    DROP TABLE dbo.DEXMeter;
GO

-- Create DEXMeter table (Main machine audit data)
CREATE TABLE DEXMeter (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MachineID NVARCHAR(20) NOT NULL,           -- ID106 from DEX file
    DEXDateTime DATETIME2 NOT NULL,            -- Timestamp of the reading
    MachineSerialNumber NVARCHAR(20) NOT NULL, -- ID101 from DEX file
    ValueOfPaidVends DECIMAL(10,2) NOT NULL,   -- VA101 (divided by 100)
    CONSTRAINT UK_DEXMeter_Machine_DateTime UNIQUE (MachineID, DEXDateTime)
);
GO

-- Create DEXLaneMeter table (Product/lane data)
CREATE TABLE DEXLaneMeter (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DEXMeterId INT NOT NULL,                   -- FK to DEXMeter
    ProductIdentifier NVARCHAR(6) NOT NULL,    -- PA101
    Price DECIMAL(10,2) NOT NULL,              -- PA102 (divided by 100)
    NumberOfVends INT NOT NULL,                -- PA201
    ValueOfPaidSales DECIMAL(10,2) NOT NULL,   -- PA202 (divided by 100)
    CONSTRAINT FK_DEXLaneMeter_DEXMeter FOREIGN KEY (DEXMeterId) REFERENCES DEXMeter(Id)
);
GO

-- Create indexes for better performance
CREATE INDEX IX_DEXMeter_MachineID ON DEXMeter(MachineID);
GO

CREATE INDEX IX_DEXMeter_DEXDateTime ON DEXMeter(DEXDateTime);
GO

CREATE INDEX IX_DEXLaneMeter_DEXMeterId ON DEXLaneMeter(DEXMeterId);
GO

PRINT 'Tables created successfully';