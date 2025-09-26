using System.Diagnostics;

namespace VendSysParser.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestInfo = GetRequestInfo(context);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var responseInfo = GetResponseInfo(context, stopwatch.ElapsedMilliseconds);
            _logger.LogInformation("Request processed: {RequestInfo} -> {ResponseInfo}", requestInfo, responseInfo);
        }
    }

    private static string GetRequestInfo(HttpContext context)
    {
        var request = context.Request;
        return $"{request.Method} {request.Path}{request.QueryString} {request.Protocol}";
    }

    private static string GetResponseInfo(HttpContext context, long elapsedMs)
    {
        var response = context.Response;
        return $"{response.StatusCode} in {elapsedMs}ms";
    }
}