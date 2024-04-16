using FluentAssertions;
using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Entities;

public class TodoListCategoryTests
{
    [Fact]
    public void Create_Should_Return_Valid_DomainOperationResult_When_Valid_Dto()
    {
        // act
        var result = TodoListCategory.Create(DtoHelper.ValidTodoListCategoryCreateDto());

        // assert
        result.Should().NotBeNull();
        result.Result.Should().NotBeNull();
        result.ValidationFailures.Should().BeEmpty();
    }

    [Fact]
    public void Create_Should_Validate_Data()
    {
        // arrange
        var invalidDto = new TodoListCategoryCreateDto(string.Empty);

        // act
        var result = TodoListCategory.Create(invalidDto);

        // assert
        result.ValidationFailures.Should().ContainSingle(e => e.PropertyName == nameof(TodoListCategory.Name));
    }

    [Fact]
    public void UpdateOrDelete_Should_Validate_Empty_Name()
    {
        // arrange
        var invalidDto = new TodoListCategoryDto(Guid.NewGuid(), string.Empty);

        // act
        var result = TodoListCategory.UpdateOrDelete(invalidDto);

        // assert
        result.ValidationFailures.Should().ContainSingle(e => e.PropertyName == nameof(TodoListCategory.Name));
    }

    [Fact]
    public void ToDto_Should_Return_Mapped_Dto()
    {
        // arrange
        var expectedId = Guid.NewGuid();
        var expectedName = Guid.NewGuid().ToString();
        var sut = new TodoListCategory
        {
            Id = expectedId,
            Name = expectedName,
        };

        // act
        var result = sut.ToDto();

        // assert
        result.Should().BeOfType<TodoListCategoryDto>();
        result.Id.Should().Be(expectedId);
        result.Name.Should().Be(expectedName);
    }
}