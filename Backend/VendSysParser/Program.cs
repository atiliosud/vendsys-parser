using VendSysParser.Application.Services;
using VendSysParser.Api.Endpoints;
using VendSysParser.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<DexParserService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure authorization (required for RequireAuthorization())
builder.Services.AddAuthorization();

// Configure logging to be minimal as per requirements
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Error);

var app = builder.Build();

// Enable CORS
app.UseCors();

// Add request logging middleware (first to capture all requests)
app.UseMiddleware<RequestLoggingMiddleware>();

// Add exception handling middleware (early in pipeline)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add custom authentication middleware
app.UseMiddleware<BasicAuthenticationMiddleware>();

// Use authorization
app.UseAuthorization();

// Map endpoints
app.MapApiEndpoints();

// Configure Kestrel to listen on port 5000
app.Urls.Add("http://localhost:5000");

app.Run();