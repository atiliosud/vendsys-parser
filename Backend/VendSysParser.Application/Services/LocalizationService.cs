using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace VendSysParser.Application.Services;

public class LocalizationService : ILocalizationService
{
    private readonly ResourceManager _resourceManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalizationService(IHttpContextAccessor httpContextAccessor)
    {
        _resourceManager = new ResourceManager("VendSysParser.Resources.Messages", typeof(LocalizationService).Assembly);
        _httpContextAccessor = httpContextAccessor;
    }

    private CultureInfo GetCurrentCulture()
    {
        // Try to get culture from HTTP context first
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            var culture = httpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture;
            if (culture != null)
                return culture;
        }

        // Fallback to current thread culture, or default to en-US
        return CultureInfo.CurrentUICulture ?? new CultureInfo("en-US");
    }

    public string GetString(string key, params object[] args)
    {
        try
        {
            var culture = GetCurrentCulture();
            var message = _resourceManager.GetString(key, culture);

            if (string.IsNullOrEmpty(message))
            {
                // Fallback to key if resource not found
                return key;
            }

            // Format with arguments if provided
            return args.Length > 0 ? string.Format(culture, message, args) : message;
        }
        catch
        {
            // Return key as fallback if any error occurs
            return key;
        }
    }

    // Convenience methods for common strings
    public string AuthenticationSuccessful() => GetString("AuthenticationSuccessful");
    public string AuthenticationFailed(string username) => GetString("AuthenticationFailed", username);
    public string CredentialsValidationError(string username) => GetString("CredentialsValidationError", username);
    public string DexFileProcessedSuccessfully() => GetString("DexFileProcessedSuccessfully");
    public string DexFileUploadedSuccessfully(int dexMeterId, string fileName) => GetString("DexFileUploadedSuccessfully", dexMeterId, fileName);
    public string ErrorProcessingDexFile() => GetString("ErrorProcessingDexFile");
    public string ErrorProcessingDexFileUpload() => GetString("ErrorProcessingDexFileUpload");
    public string RequestMustBeMultipartFormData() => GetString("RequestMustBeMultipartFormData");
    public string NoFileProvidedOrEmpty() => GetString("NoFileProvidedOrEmpty");
    public string InvalidFileType(string allowedTypes) => GetString("InvalidFileType", allowedTypes);
    public string FileContentIsEmpty() => GetString("FileContentIsEmpty");
    public string DatabaseConnectionNotConfigured() => GetString("DatabaseConnectionNotConfigured");
    public string AnErrorOccurredProcessingDexFile() => GetString("AnErrorOccurredProcessingDexFile");
    public string ID1SegmentNotFound() => GetString("ID1SegmentNotFound");
    public string InvalidID1SegmentFormat() => GetString("InvalidID1SegmentFormat");
    public string VA1SegmentNotFound() => GetString("VA1SegmentNotFound");
    public string InvalidVA1SegmentFormat() => GetString("InvalidVA1SegmentFormat");
    public string InvalidVA101Value(string value) => GetString("InvalidVA101Value", value);
    public string InternalServerError() => GetString("InternalServerError");
    public string NoFileProvided() => GetString("NoFileProvided");
    public string OnlyTextFilesAllowed() => GetString("OnlyTextFilesAllowed");
}