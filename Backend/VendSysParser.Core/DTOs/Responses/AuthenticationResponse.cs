namespace VendSysParser.Core.DTOs.Responses;

public class AuthenticationResponse
{
    public string Message { get; set; } = string.Empty;

    public AuthenticationResponse() { }

    public AuthenticationResponse(string message)
    {
        Message = message;
    }
}