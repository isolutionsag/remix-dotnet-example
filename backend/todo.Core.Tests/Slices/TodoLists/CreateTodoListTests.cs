using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoLists;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Slices.TodoLists;

public class CreateTodoListTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidDto_ShouldAddCategoryAndSaveChanges()
    {
        // arrange
        var repository = A.Fake<ITodoListRepository>();
        var dto = DtoHelper.ValidTodoListCreateDto();
        var sut = new CreateTodoList(repository);

        // act
        var result = await sut.ExecuteAsync(dto);

        // assert
        result.IsSuccess().Should().BeTrue();
        A.CallTo(() => repository.Add(A<TodoList>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyDto_ShouldAddCategoryAndSaveChanges()
    {
        // arrange
        var repository = A.Fake<ITodoListRepository>();
        var dto = DtoHelper.ValidEmptyTodoListCreateDto();
        var sut = new CreateTodoList(repository);

        // act
        var result = await sut.ExecuteAsync(dto);

        // assert
        result.IsSuccess().Should().BeTrue();
        A.CallTo(() => repository.Add(A<TodoList>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
    }
}