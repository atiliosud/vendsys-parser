namespace VendSysParser.Core.DTOs.Requests;

public class AuthenticationRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public static AuthenticationRequest? FromAuthorizationHeader(string? authHeader)
    {
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
            return null;

        try
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = System.Text.Encoding.UTF8.GetString(decodedBytes);
            var parts = credentials.Split(':', 2);

            if (parts.Length != 2)
                return null;

            return new AuthenticationRequest
            {
                Username = parts[0],
                Password = parts[1]
            };
        }
        catch
        {
            return null;
        }
    }
}