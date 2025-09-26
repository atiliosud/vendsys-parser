namespace VendSysParser.Application.Services;

public interface ILocalizationService
{
    string ID1SegmentNotFound();
    string InvalidID1SegmentFormat();
    string VA1SegmentNotFound();
    string InvalidVA1SegmentFormat();
    string InvalidVA101Value(string value);
    string DexFileUploadedSuccessfully(int dexMeterId, string fileName);
    string ErrorProcessingDexFile();
    string AuthenticationFailed(string username);
    string CredentialsValidationError(string username);
}