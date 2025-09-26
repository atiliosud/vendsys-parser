using Xunit;
using VendSysParser.Core.DTOs.Requests;

namespace VendSysParser.Tests.DTOs;

public class AuthenticationRequestTests
{
    [Fact]
    public void FromAuthorizationHeader_ValidBasicAuth_ShouldParseCredentials()
    {
        // Arrange
        var username = "vendsys";
        var password = "NFsZGmHAGWJSZ#RuvdiV";
        var credentials = $"{username}:{password}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic {encodedCredentials}";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(password, result.Password);
    }

    [Fact]
    public void FromAuthorizationHeader_NullHeader_ShouldReturnNull()
    {
        // Arrange
        string authHeader = null;

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromAuthorizationHeader_EmptyHeader_ShouldReturnNull()
    {
        // Arrange
        var authHeader = "";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromAuthorizationHeader_NotBasicAuth_ShouldReturnNull()
    {
        // Arrange
        var authHeader = "Bearer token123";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromAuthorizationHeader_InvalidBase64_ShouldReturnNull()
    {
        // Arrange
        var authHeader = "Basic InvalidBase64!@#";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromAuthorizationHeader_NoColonInCredentials_ShouldReturnNull()
    {
        // Arrange
        var credentials = "usernamepassword"; // No colon separator
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic {encodedCredentials}";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromAuthorizationHeader_EmptyUsername_ShouldParseCorrectly()
    {
        // Arrange
        var username = "";
        var password = "password";
        var credentials = $"{username}:{password}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic {encodedCredentials}";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(password, result.Password);
    }

    [Fact]
    public void FromAuthorizationHeader_EmptyPassword_ShouldParseCorrectly()
    {
        // Arrange
        var username = "username";
        var password = "";
        var credentials = $"{username}:{password}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic {encodedCredentials}";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(password, result.Password);
    }

    [Fact]
    public void FromAuthorizationHeader_PasswordWithColon_ShouldParseCorrectly()
    {
        // Arrange
        var username = "user";
        var password = "pass:word:with:colons";
        var credentials = $"{username}:{password}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic {encodedCredentials}";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(password, result.Password);
    }

    [Fact]
    public void FromAuthorizationHeader_SpecialCharactersInCredentials_ShouldParseCorrectly()
    {
        // Arrange
        var username = "vendsys";
        var password = "NFsZGmHAGWJSZ#RuvdiV"; // Password with special characters
        var credentials = $"{username}:{password}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic {encodedCredentials}";

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(password, result.Password);
    }

    [Fact]
    public void FromAuthorizationHeader_BasicWithExtraSpaces_ShouldParseCorrectly()
    {
        // Arrange
        var username = "vendsys";
        var password = "NFsZGmHAGWJSZ#RuvdiV";
        var credentials = $"{username}:{password}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        var authHeader = $"Basic   {encodedCredentials}   "; // Extra spaces

        // Act
        var result = AuthenticationRequest.FromAuthorizationHeader(authHeader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(password, result.Password);
    }
}