using VendSysParser.Core.DTOs.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VendSysParser.Application.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly ILocalizationService _localization;

    public AuthService(IConfiguration configuration, ILogger<AuthService> logger, ILocalizationService localization)
    {
        _configuration = configuration;
        _logger = logger;
        _localization = localization;
    }

    public bool ValidateCredentials(string? authorizationHeader)
    {
        var request = AuthenticationRequest.FromAuthorizationHeader(authorizationHeader);

        if (request == null)
        {
            return false;
        }

        return ValidateCredentials(request);
    }

    public bool ValidateCredentials(AuthenticationRequest request)
    {
        if (request == null)
        {
            return false;
        }

        try
        {
            var validUsername = _configuration["Authentication:Username"];
            var validPassword = _configuration["Authentication:Password"];

            var isValid = request.Username == validUsername && request.Password == validPassword;

            if (!isValid)
            {
                _logger.LogError(_localization.AuthenticationFailed(request.Username));
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localization.CredentialsValidationError(request.Username));
            return false;
        }
    }
}