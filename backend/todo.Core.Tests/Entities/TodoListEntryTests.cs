using FluentAssertions;
using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Entities;

public class TodoListEntryEntryTests
{
    [Fact]
    public void Create_Should_Return_Valid_DomainOperationResult_When_Valid_Dto()
    {
        // act
        var result = TodoListEntry.Create(DtoHelper.ValidTodoListEntryCreateDto());

        // assert
        result.Should().NotBeNull();
        result.Result.Should().NotBeNull();
        result.ValidationFailures.Should().BeEmpty();
    }

    [Fact]
    public void Create_Should_Validate_Data()
    {
        // arrange
        var invalidDto = new TodoListEntryCreateDto(string.Empty, string.Empty, Guid.NewGuid());

        // act
        var result = TodoListEntry.Create(invalidDto);

        // assert
        result.ValidationFailures.Should().ContainSingle(e => e.PropertyName == nameof(TodoListEntry.Title));
    }

    [Fact]
    public void Create_Should_Validate_Empty_ParentId()
    {
        // arrange
        var invalidDto = new TodoListEntryCreateDto("My title", string.Empty, Guid.Empty);

        // act
        var result = TodoListEntry.Create(invalidDto);

        // assert
        result.ValidationFailures.Should().ContainSingle(e => e.PropertyName == nameof(TodoListEntry.ParentId));
    }

    [Fact]
    public void UpdateOrDelete_Should_Validate_Empty_ParentId()
    {
        // arrange
        var invalidDto = new TodoListEntryDto(Guid.NewGuid(), "My Todo List", string.Empty, false, Guid.Empty, null);

        // act
        var result = TodoListEntry.UpdateOrDelete(invalidDto);

        // assert
        result.ValidationFailures.Should().ContainSingle(e => e.PropertyName == nameof(TodoListEntry.ParentId));
    }

    [Fact]
    public void UpdateOrDelete_Should_Return_Valid_DomainOperationResult_When_Valid_Dto()
    {
        // act
        var result = TodoListEntry.UpdateOrDelete(DtoHelper.ValidTodoListEntryDto());

        // assert
        result.Should().NotBeNull();
        result.Result.Should().NotBeNull();
        result.ValidationFailures.Should().BeEmpty();
    }

    [Fact]
    public void ToDto_Should_Return_Mapped_Dto()
    {
        // arrange
        var expectedId = Guid.NewGuid();
        var expectedParentId = Guid.NewGuid();
        var expectedTitle = Guid.NewGuid().ToString();
        var expectedDescription = Guid.NewGuid().ToString();
        var sut = new TodoListEntry
        {
            Id = expectedId,
            ParentId = expectedParentId,
            Title = expectedTitle,
            Description = expectedDescription
        };

        // act
        var result = sut.ToDto();

        // assert
        result.Should().BeOfType<TodoListEntryDto>();
        result.Id.Should().Be(expectedId);
        result.ParentId.Should().Be(expectedParentId);
        result.Title.Should().Be(expectedTitle);
        result.Description.Should().Be(expectedDescription);
    }
}