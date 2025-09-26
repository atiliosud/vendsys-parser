namespace VendSysParser.Core.Models;

public class DEXLaneMeter
{
    public int Id { get; set; }
    public int DEXMeterId { get; set; }
    public string ProductIdentifier { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int NumberOfVends { get; set; }
    public decimal ValueOfPaidSales { get; set; }
}