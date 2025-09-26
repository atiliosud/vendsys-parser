using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VendSysParser.Application.Services;
using VendSysParser.Core.DTOs.Requests;

namespace VendSysParser.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly Mock<ILocalizationService> _mockLocalization;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<AuthService>>();
        _mockLocalization = new Mock<ILocalizationService>();

        // Setup default configuration values
        _mockConfiguration.Setup(x => x["Authentication:Username"]).Returns("vendsys");
        _mockConfiguration.Setup(x => x["Authentication:Password"]).Returns("NFsZGmHAGWJSZ#RuvdiV");

        _service = new AuthService(_mockConfiguration.Object, _mockLogger.Object, _mockLocalization.Object);
    }

    [Fact]
    public void ValidateCredentials_ValidCredentials_ShouldReturnTrue()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "vendsys",
            Password = "NFsZGmHAGWJSZ#RuvdiV"
        };

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateCredentials_InvalidUsername_ShouldReturnFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "wronguser",
            Password = "NFsZGmHAGWJSZ#RuvdiV"
        };

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCredentials_InvalidPassword_ShouldReturnFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "vendsys",
            Password = "wrongpassword"
        };

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCredentials_EmptyUsername_ShouldReturnFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "",
            Password = "NFsZGmHAGWJSZ#RuvdiV"
        };

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCredentials_EmptyPassword_ShouldReturnFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "vendsys",
            Password = ""
        };

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCredentials_NullRequest_ShouldReturnFalse()
    {
        // Arrange
        AuthenticationRequest request = null;

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCredentials_MissingConfigurationUsername_ShouldReturnFalse()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(x => x["Authentication:Username"]).Returns((string)null);
        mockConfig.Setup(x => x["Authentication:Password"]).Returns("NFsZGmHAGWJSZ#RuvdiV");

        var service = new AuthService(mockConfig.Object, _mockLogger.Object, _mockLocalization.Object);

        var request = new AuthenticationRequest
        {
            Username = "vendsys",
            Password = "NFsZGmHAGWJSZ#RuvdiV"
        };

        // Act
        var result = service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCredentials_MissingConfigurationPassword_ShouldReturnFalse()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(x => x["Authentication:Username"]).Returns("vendsys");
        mockConfig.Setup(x => x["Authentication:Password"]).Returns((string)null);

        var service = new AuthService(mockConfig.Object, _mockLogger.Object, _mockLocalization.Object);

        var request = new AuthenticationRequest
        {
            Username = "vendsys",
            Password = "NFsZGmHAGWJSZ#RuvdiV"
        };

        // Act
        var result = service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("VENDSYS", "NFsZGmHAGWJSZ#RuvdiV")] // Different case username
    [InlineData("vendsys", "nfsZGmHAGWJSZ#RuvdiV")] // Different case password
    [InlineData("vendsys ", "NFsZGmHAGWJSZ#RuvdiV")] // Username with trailing space
    [InlineData("vendsys", " NFsZGmHAGWJSZ#RuvdiV")] // Password with leading space
    public void ValidateCredentials_CaseSensitiveAndWhitespace_ShouldReturnFalse(string username, string password)
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = username,
            Password = password
        };

        // Act
        var result = _service.ValidateCredentials(request);

        // Assert
        Assert.False(result);
    }
}