using System.Globalization;
using Microsoft.AspNetCore.Localization;
using VendSysParser.Application.Services;
using VendSysParser.Api.Endpoints;
using VendSysParser.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure localization
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("pt-BR"),
    new CultureInfo("nl-NL"),
    new CultureInfo("fr-FR"),
    new CultureInfo("de-DE"),
    new CultureInfo("es-ES"),
    new CultureInfo("it-IT"),
    new CultureInfo("sv-SE"),
    new CultureInfo("tr-TR"),
    new CultureInfo("hu-HU"),
    new CultureInfo("ja-JP"),
    new CultureInfo("fi-FI")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Configure request culture providers
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
});

// Configure services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<DexParserService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Configure logging to be minimal as per requirements
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Error);

var app = builder.Build();

// Enable CORS
app.UseCors();

// Enable localization
app.UseRequestLocalization();

// Add request logging middleware (first to capture all requests)
app.UseMiddleware<RequestLoggingMiddleware>();

// Add exception handling middleware (early in pipeline)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add custom authentication middleware
app.UseMiddleware<BasicAuthenticationMiddleware>();

// Map endpoints
app.MapApiEndpoints();

// Configure Kestrel to listen on port 5000
app.Urls.Add("http://localhost:5000");

app.Run();