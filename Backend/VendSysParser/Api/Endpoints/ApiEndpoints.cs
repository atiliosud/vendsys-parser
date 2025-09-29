using VendSysParser.Core.DTOs.Requests;
using VendSysParser.Core.DTOs.Responses;
using VendSysParser.Application.Services;
using Microsoft.Extensions.Logging;

namespace VendSysParser.Api.Endpoints;

public static class ApiEndpoints
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        // Health endpoints
        app.MapGet("/", () => Results.Ok(new { status = "API is running", version = "1.0.0" }))
           .AllowAnonymous()
           .Produces(200);

        app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
           .AllowAnonymous()
           .Produces(200);

        // Authentication endpoint
        app.MapPost("/api/authenticate", AuthenticateUser)
           .AllowAnonymous()
           .Produces<AuthenticationResponse>(200)
           .Produces<ErrorResponse>(401)
           .Produces<ErrorResponse>(500);

        // DEX upload endpoint
        app.MapPost("/api/dex", UploadDexFile)
        .Produces<DexUploadResponse>(200)
        .Produces<ErrorResponse>(400)
        .Produces<ErrorResponse>(500)
        .DisableAntiforgery();
    }

    private static async Task<IResult> UploadDexFile(
        HttpContext context,
        DexParserService parser,
        IConfiguration config,
        ILogger<Program> logger,
        ILocalizationService localization)
    {
        try
        {
            if (!context.Request.HasFormContentType)
            {
                return Results.BadRequest(new ErrorResponse(localization.RequestMustBeMultipartFormData()));
            }

            var form = await context.Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
            {
                return Results.BadRequest(new ErrorResponse(localization.NoFileProvided()));
            }

            if (!file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) &&
                !file.FileName.EndsWith(".dex", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest(new ErrorResponse(localization.OnlyTextFilesAllowed()));
            }

            using var reader = new StreamReader(file.OpenReadStream());
            var content = await reader.ReadToEndAsync();

            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(localization.DatabaseConnectionNotConfigured());

            var dexMeterId = await parser.ParseAndSaveAsync(content, connectionString);

            logger.LogInformation("DEX file processed successfully. DexMeterId: {DexMeterId}", dexMeterId);

            return Results.Ok(new DexUploadResponse(localization.DexFileProcessedSuccessfully(), dexMeterId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing DEX file");
            return Results.StatusCode(500);
        }
    }

    private static IResult AuthenticateUser(
        HttpContext context,
        AuthService authService,
        ILocalizationService localization)
    {
        try
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
            var request = AuthenticationRequest.FromAuthorizationHeader(authHeader);

            if (request == null)
            {
                return Results.Unauthorized();
            }

            var isValid = authService.ValidateCredentials(request);
            if (!isValid)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new AuthenticationResponse(localization.AuthenticationSuccessful()));
        }
        catch (Exception)
        {
            return Results.StatusCode(500);
        }
    }

}