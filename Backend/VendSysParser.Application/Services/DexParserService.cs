using System.Text.Json;
using Microsoft.Data.SqlClient;
using Dapper;
using VendSysParser.Core.Models;
using VendSysParser.Infrastructure.DataAccess;
using Microsoft.Extensions.Logging;

namespace VendSysParser.Application.Services;

public class DexParserService
{
    private readonly ILogger<DexParserService> _logger;
    private readonly ILocalizationService _localization;

    public DexParserService(ILogger<DexParserService> logger, ILocalizationService localization)
    {
        _logger = logger;
        _localization = localization;
    }

    public async Task<int> ParseAndSaveAsync(string dexContent, string connectionString)
    {
        try
        {
            var lines = dexContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Extract ID1 segment data
            var id1Line = lines.FirstOrDefault(l => l.StartsWith("ID1*"));
            if (id1Line == null)
            {
                throw new InvalidOperationException(_localization.ID1SegmentNotFound());
            }

            var id1Parts = id1Line.Split('*');
            if (id1Parts.Length < 6)
            {
                throw new InvalidOperationException(_localization.InvalidID1SegmentFormat());
            }

            // ID101 - Machine Serial Number (position 1 after ID1*)
            var machineSerialNumber = id1Parts[1];

            // ID106 - Machine ID (position 6 after ID1*), if empty use serial number
            var machineId = id1Parts.Length > 6 && !string.IsNullOrWhiteSpace(id1Parts[6])
                ? id1Parts[6]
                : machineSerialNumber;

            // Extract VA1 segment data
            var va1Line = lines.FirstOrDefault(l => l.StartsWith("VA1*"));
            if (va1Line == null)
            {
                throw new InvalidOperationException(_localization.VA1SegmentNotFound());
            }

            var va1Parts = va1Line.Split('*');
            if (va1Parts.Length < 2)
            {
                throw new InvalidOperationException(_localization.InvalidVA1SegmentFormat());
            }

            // VA101 - Value of Paid Vends (position 1 after VA1*)
            if (!decimal.TryParse(va1Parts[1], out var valueOfPaidVendsRaw))
            {
                throw new InvalidOperationException(_localization.InvalidVA101Value(va1Parts[1]));
            }
            var valueOfPaidVends = valueOfPaidVendsRaw / 100m; // Convert implied decimal

            // Extract products (PA1/PA2 pairs)
            var products = ExtractProducts(lines);

            // Save to database
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            try
            {
                // Insert into DEXMeter using Infrastructure
                var dexMeterId = await DexDataAccess.SaveDexMeterAsync(
                    connection,
                    transaction,
                    machineId,
                    machineSerialNumber,
                    valueOfPaidVends);

                // Insert products into DEXLaneMeter using Infrastructure
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        var laneMeter = new DEXLaneMeter
                        {
                            ProductIdentifier = product.ProductIdentifier,
                            Price = product.Price,
                            NumberOfVends = product.NumberOfVends,
                            ValueOfPaidSales = product.ValueOfPaidSales
                        };

                        await DexDataAccess.SaveDexLaneMeterAsync(
                            connection,
                            transaction,
                            dexMeterId,
                            laneMeter);
                    }
                }

                await transaction.CommitAsync();
                _logger.LogInformation(_localization.DexFileUploadedSuccessfully(dexMeterId, "DEX_FILE"));
                return dexMeterId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localization.ErrorProcessingDexFile());
            throw;
        }
    }

    private List<ProductData> ExtractProducts(string[] lines)
    {
        var products = new List<ProductData>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].StartsWith("PA1*"))
                continue;

            var pa1Parts = lines[i].Split('*');
            if (pa1Parts.Length < 3)
                continue;

            // PA101 - Product Identifier (position 1 after PA1*)
            var productIdentifier = pa1Parts[1];

            // PA102 - Product Price (position 2 after PA1*)
            if (!decimal.TryParse(pa1Parts[2], out var priceRaw))
                continue;
            var price = priceRaw / 100m; // Convert implied decimal

            // Look for corresponding PA2 segment
            if (i + 1 < lines.Length && lines[i + 1].StartsWith("PA2*"))
            {
                var pa2Parts = lines[i + 1].Split('*');
                if (pa2Parts.Length >= 3)
                {
                    // PA201 - Number of Vends (position 1 after PA2*)
                    if (!int.TryParse(pa2Parts[1], out var numberOfVends))
                        continue;

                    // PA202 - Value of Paid Sales (position 2 after PA2*)
                    if (!decimal.TryParse(pa2Parts[2], out var valueOfPaidSalesRaw))
                        continue;
                    var valueOfPaidSales = valueOfPaidSalesRaw / 100m; // Convert implied decimal

                    products.Add(new ProductData
                    {
                        ProductIdentifier = productIdentifier,
                        Price = price,
                        NumberOfVends = numberOfVends,
                        ValueOfPaidSales = valueOfPaidSales
                    });
                }
            }
        }

        return products;
    }

    private class ProductData
    {
        public string ProductIdentifier { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int NumberOfVends { get; set; }
        public decimal ValueOfPaidSales { get; set; }
    }
}