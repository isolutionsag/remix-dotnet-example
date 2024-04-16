using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListCategories;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Slices.TodoListCategories;

public class CreateTodoListCategoryTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidDto_ShouldAddCategoryAndSaveChanges()
    {
        // arrange
        var repository = A.Fake<ITodoListCategoryRepository>();
        var dto = DtoHelper.ValidTodoListCategoryCreateDto();
        var sut = new CreateTodoListCategory(repository);

        // act
        var result = await sut.ExecuteAsync(dto);

        // assert
        result.IsSuccess().Should().BeTrue();
        A.CallTo(() => repository.Add(A<TodoListCategory>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidDto_ShouldNotAddCategory()
    {
        // arrange
        var repository = A.Fake<ITodoListCategoryRepository>();
        var dto = DtoHelper.InvalidTodoListCategoryCreateDto();
        var sut = new CreateTodoListCategory(repository);

        // act
        var result = await sut.ExecuteAsync(dto);

        // assert
        result.IsSuccess().Should().BeFalse();
        A.CallTo(() => repository.Add(A<TodoListCategory>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => repository.SaveChangesAsync(default)).MustNotHaveHappened();
    }
}