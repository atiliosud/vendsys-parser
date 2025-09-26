namespace VendSysParser.Core.Models;

public class DEXMeter
{
    public int Id { get; set; }
    public string MachineID { get; set; } = string.Empty;
    public DateTime DEXDateTime { get; set; }
    public string MachineSerialNumber { get; set; } = string.Empty;
    public decimal ValueOfPaidVends { get; set; }
    public List<DEXLaneMeter> LaneMeters { get; set; } = new();
}