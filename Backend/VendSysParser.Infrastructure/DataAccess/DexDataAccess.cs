using System.Reflection;
using Microsoft.Data.SqlClient;
using Dapper;
using VendSysParser.Core.Models;

namespace VendSysParser.Infrastructure.DataAccess;

public class DexDataAccess
{
    private static string GetEmbeddedSqlScript(string scriptName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"VendSysParser.Infrastructure.Scripts.{scriptName}";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"SQL script '{scriptName}' not found as embedded resource");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static async Task<int> SaveDexMeterAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string machineId,
        string machineSerialNumber,
        decimal valueOfPaidVends)
    {
        var sql = GetEmbeddedSqlScript("InsertDEXMeter.sql");

        var dexMeterId = await connection.QuerySingleAsync<int>(
            sql,
            new
            {
                MachineID = machineId,
                DEXDateTime = DateTime.UtcNow,
                MachineSerialNumber = machineSerialNumber,
                ValueOfPaidVends = valueOfPaidVends
            },
            transaction);

        return dexMeterId;
    }

    public static async Task SaveDexLaneMeterAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        int dexMeterId,
        DEXLaneMeter product)
    {
        var sql = GetEmbeddedSqlScript("InsertDEXLaneMeter.sql");

        await connection.ExecuteAsync(
            sql,
            new
            {
                DEXMeterId = dexMeterId,
                product.ProductIdentifier,
                product.Price,
                product.NumberOfVends,
                product.ValueOfPaidSales
            },
            transaction);
    }
}