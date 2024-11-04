
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ContactAPI.Controllers;
using ContactCore.Interfaces;
using ContactCore.DTOs;
using ContactCore.Entity;
using ContactCore.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace ContactManagement;

public class ContactsControllerTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly Mock<IValidator<ContactDTO>> _mockValidator;
    private readonly ContactController _controller;

    public ContactsControllerTests()
    {
        _mockRepo = new Mock<IContactRepository>();
        _mockValidator = new Mock<IValidator<ContactDTO>>();
        _controller = new ContactController(_mockRepo.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Id = 1, Name = "Test User", Phone = "123456789", Email = "test@example.com", DDD = "11" }
        };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contacts);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContacts = Assert.IsAssignableFrom<IEnumerable<Contact>>(okResult.Value);
        Assert.Single(returnedContacts);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var contact = new Contact { Id = 1, Name = "Test User", Phone = "123456789", Email = "test@example.com", DDD = "11" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contact);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContact = Assert.IsType<Contact>(okResult.Value);
        Assert.Equal(contact.Id, returnedContact.Id);
    }

    [Fact]
    public async Task Create_WithValidContact_ReturnsCreatedAtAction()
    {
        // Arrange
        var contactDto = new ContactDTO 
        {
            Name = "Test User",
            Phone = "123456789",
            Email = "test@example.com",
            DDD = "11"
        };
        var contact = new Contact { Id = 1, Name = "Test User", Phone = "123456789", Email = "test@example.com", DDD = "11" };

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ContactDTO>(), default))
            .ReturnsAsync(new ValidationResult());
        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Contact>())).ReturnsAsync(contact);

        // Act
        var result = await _controller.Create(contactDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetById", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task Update_WithValidContact_ReturnsNoContent()
    {
        // Arrange
        var contactDto = new ContactDTO
        {
            Name = "Test User",
            Phone = "123456789",
            Email = "test@example.com",
            DDD = "11"
        }; var existingContact = new Contact { Id = 1, Name = "Test User", Phone = "123456789", Email = "test@example.com", DDD = "11" };

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ContactDTO>(), default))
            .ReturnsAsync(new ValidationResult());
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingContact);

        // Act
        var result = await _controller.Update(1, contactDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var contact = new Contact { Id = 1, Name = "Test User", Phone = "123456789", Email = "test@example.com", DDD = "11" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contact);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}
