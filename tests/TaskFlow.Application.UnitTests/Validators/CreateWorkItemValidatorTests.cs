using FluentValidation.TestHelper;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Validators;

namespace TaskFlow.Application.UnitTests.Validators;

public class CreateWorkItemValidatorTests
{
    private readonly CreateWorkItemValidator _validator;

    public CreateWorkItemValidatorTests()
    {
        _validator = new CreateWorkItemValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Null()
    {
        // Arrange
        var model = new CreateWorkItemDto { Title = null!, BoardId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        var model = new CreateWorkItemDto { Title = string.Empty, BoardId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_Maximum_Length()
    {
        // Arrange
        var model = new CreateWorkItemDto { Title = new string('a', 201), BoardId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Valid()
    {
        // Arrange
        var model = new CreateWorkItemDto { Title = "Valid Title", BoardId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Maximum_Length()
    {
        // Arrange
        var model = new CreateWorkItemDto 
        { 
            Title = "Valid Title",
            Description = new string('a', 1001),
            BoardId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Description_Is_Valid()
    {
        // Arrange
        var model = new CreateWorkItemDto 
        { 
            Title = "Valid Title",
            Description = "Valid Description",
            BoardId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Description_Is_Null()
    {
        // Arrange
        var model = new CreateWorkItemDto 
        { 
            Title = "Valid Title",
            Description = null,
            BoardId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}
