using ContactCore.DTOs;
using ContactCore.Validators;
using System.Numerics;

namespace ContactManagement;

public class ContactValidatorTests
{
    private readonly ContactValidator _validator;

    public ContactValidatorTests()
    {
        _validator = new ContactValidator();
    }

    [Fact]
    public void Validate_ValidContact_ShouldPass()
    {
        // Arrange
        var contact = new ContactDTO
        {
            Name = "John Doe",
            Phone = "123456789",
            Email = "john@example.com",
            DDD = "11"
        };

        // Act
        var result = _validator.Validate(contact);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "Invalid name")]
    [InlineData("J", "Name too short")]
    public void Validate_InvalidName_ShouldFail(string name, string testCase)
    {
        // Arrange
        var contact = new ContactDTO
        {
            Name = name,
            Phone = "123456789",
            Email = "john@example.com",
            DDD = "11"
        };

        // Act
        var result = _validator.Validate(contact);

        // Assert
        Assert.False(result.IsValid, testCase);
        Assert.Contains(result.Errors, error => error.PropertyName == "Name");
    }

    [Theory]
    [InlineData("invalid-email", "Invalid email format")]
    [InlineData("", "Empty email")]
    public void Validate_InvalidEmail_ShouldFail(string email, string testCase)
    {
        // Arrange
        var contact = new ContactDTO
        {
            Name = "John Doe",
            Phone = "123456789",
            Email = email,
            DDD = "11"
        };

        // Act
        var result = _validator.Validate(contact);

        // Assert
        Assert.False(result.IsValid, testCase);
        Assert.Contains(result.Errors, error => error.PropertyName == "Email");
    }

    [Theory]
    [InlineData("1234567", "Too short")]
    [InlineData("1234567890", "Too long")]
    [InlineData("abc123456", "Contains letters")]
    public void Validate_InvalidPhone_ShouldFail(string phone, string testCase)
    {
        // Arrange
        var contact = new ContactDTO
        {
            Name = "John Doe",
            Phone = phone,
            Email = "john@example.com",
            DDD = "11"
        };

        // Act
        var result = _validator.Validate(contact);

        // Assert
        Assert.False(result.IsValid, testCase);
        Assert.Contains(result.Errors, error => error.PropertyName == "Phone");
    }

    [Theory]
    [InlineData("1", "Too short")]
    [InlineData("123", "Too long")]
    [InlineData("ab", "Contains letters")]
    public void Validate_InvalidDDD_ShouldFail(string ddd, string testCase)
    {
        // Arrange
        var contact = new ContactDTO
        {
            Name = "John Doe",
            Phone = "123456789",
            Email = "john@example.com",
            DDD = ddd
        };

        // Act
        var result = _validator.Validate(contact);

        // Assert
        Assert.False(result.IsValid, testCase);
        Assert.Contains(result.Errors, error => error.PropertyName == "DDD");
    }
}