using FluentAssertions;
using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Entities;

public class TodoListTests
{
    [Fact]
    public void Create_Should_Return_Valid_DomainOperationResult_When_Valid_Dto()
    {
        // act
        var result = TodoList.Create(DtoHelper.ValidTodoListCreateDto());

        // assert
        result.Should().NotBeNull();
        result.Result.Should().NotBeNull();
        result.ValidationFailures.Should().BeEmpty();
    }

    [Fact]
    public void UpdateOrDelete_Should_Validate_Empty_CategoryId()
    {
        // arrange
        var invalidDto = new TodoListDto(Guid.NewGuid(), "My Todo List", Guid.Empty, null);

        // act
        var result = TodoList.UpdateOrDelete(invalidDto);

        // assert
        result.ValidationFailures.Should().ContainSingle(e => e.PropertyName == nameof(TodoList.CategoryId));
    }

    [Fact]
    public void UpdateOrDelete_Should_Return_Valid_DomainOperationResult_When_Valid_Dto()
    {
        // act
        var result = TodoList.UpdateOrDelete(DtoHelper.ValidTodoListDto());

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
        var expectedCategoryId = Guid.NewGuid();
        var expectedName = Guid.NewGuid().ToString();
        var sut = new TodoList
        {
            CategoryId = expectedCategoryId,
            Name = expectedName,
            Id = expectedId
        };

        // act
        var result = sut.ToDto();

        // assert
        result.Should().BeOfType<TodoListDto>();
        result.CategoryId.Should().Be(expectedCategoryId);
        result.Id.Should().Be(expectedId);
        result.Name.Should().Be(expectedName);
    }
}