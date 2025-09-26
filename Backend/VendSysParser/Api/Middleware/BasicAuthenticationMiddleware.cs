using VendSysParser.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace VendSysParser.Api.Middleware;

public class BasicAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuthService authService)
    {
        var endpoint = context.GetEndpoint();
        var isAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

        if (isAnonymous)
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        try
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = System.Text.Encoding.UTF8.GetString(decodedBytes);
            var parts = credentials.Split(':', 2);

            if (parts.Length != 2)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var request = new VendSysParser.Core.DTOs.Requests.AuthenticationRequest
            {
                Username = parts[0],
                Password = parts[1]
            };

            if (!authService.ValidateCredentials(request))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
        catch
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}