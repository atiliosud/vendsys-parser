namespace VendSysParser.Core.DTOs.Responses;

public class DexUploadResponse
{
    public string Message { get; set; } = string.Empty;
    public int DexMeterId { get; set; }

    public DexUploadResponse() { }

    public DexUploadResponse(string message, int dexMeterId)
    {
        Message = message;
        DexMeterId = dexMeterId;
    }
}