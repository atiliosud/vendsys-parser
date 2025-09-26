USE VendSysDEX;
GO

-- Drop existing stored procedure if it exists
IF OBJECT_ID('dbo.SaveDEXData', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SaveDEXData;
GO

-- Create stored procedure to save DEX data
CREATE PROCEDURE SaveDEXData
    @MachineID NVARCHAR(20),
    @MachineSerialNumber NVARCHAR(20),
    @ValueOfPaidVends DECIMAL(10,2),
    @ProductsJson NVARCHAR(MAX) -- JSON array with products
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DEXMeterId INT;
    DECLARE @DEXDateTime DATETIME2 = GETUTCDATE();

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insert main meter record
        INSERT INTO DEXMeter (MachineID, DEXDateTime, MachineSerialNumber, ValueOfPaidVends)
        VALUES (@MachineID, @DEXDateTime, @MachineSerialNumber, @ValueOfPaidVends);

        SET @DEXMeterId = SCOPE_IDENTITY();

        -- Insert product records using JSON
        IF @ProductsJson IS NOT NULL AND @ProductsJson != ''
        BEGIN
            INSERT INTO DEXLaneMeter (DEXMeterId, ProductIdentifier, Price, NumberOfVends, ValueOfPaidSales)
            SELECT
                @DEXMeterId,
                JSON_VALUE(value, '$.ProductIdentifier'),
                CAST(JSON_VALUE(value, '$.Price') AS DECIMAL(10,2)),
                CAST(JSON_VALUE(value, '$.NumberOfVends') AS INT),
                CAST(JSON_VALUE(value, '$.ValueOfPaidSales') AS DECIMAL(10,2))
            FROM OPENJSON(@ProductsJson)
            WHERE JSON_VALUE(value, '$.ProductIdentifier') IS NOT NULL;
        END

        COMMIT TRANSACTION;

        -- Return the DEXMeterId
        SELECT @DEXMeterId as DEXMeterId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

PRINT 'Stored procedure created successfully';