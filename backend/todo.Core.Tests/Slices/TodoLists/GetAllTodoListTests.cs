using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoLists;

namespace todo.Core.Tests.Slices.TodoLists;

public class GetAllTodoListTests
{
    [Fact]
    public void Execute_ShouldReturnValues()
    {
        // arrange
        var repository = A.Fake<ITodoListRepository>();
        var sut = new GetAllTodoList(repository);
        var expectedId = Guid.NewGuid();
        var expectedCategoryId = Guid.NewGuid();
        var expectedName = Guid.NewGuid().ToString();
        var categories = new List<TodoList>
        {
            new()
            {
                Id = expectedId,
                Name = expectedName,
                CategoryId = expectedCategoryId
            }
        };
        A.CallTo(() => repository.GetAll()).Returns(categories);

        // act
        var result = sut.Execute(null);

        // assert
        A.CallTo(() => repository.GetAll()).MustHaveHappenedOnceExactly();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be(expectedName);
        result.First().Id.Should().Be(expectedId);
        result.First().CategoryId.Should().Be(expectedCategoryId);
    }
}