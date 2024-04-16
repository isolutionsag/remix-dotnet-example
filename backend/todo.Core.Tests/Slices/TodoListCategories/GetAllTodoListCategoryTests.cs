using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListCategories;

namespace todo.Core.Tests.Slices.TodoListCategories;

public class GetAllTodoListCategoryTests
{
    [Fact]
    public void Execute_ShouldReturnValues()
    {
        // arrange
        var repository = A.Fake<ITodoListCategoryRepository>();
        var sut = new GetAllTodoListCategory(repository);
        var expectedId = Guid.NewGuid();
        var expectedName = Guid.NewGuid().ToString();
        var categories = new List<TodoListCategory>
        {
            new TodoListCategory
            {
                Id = expectedId,
                Name = expectedName,
            }
        };
        A.CallTo(() => repository.GetAll()).Returns(categories);

        // act
        var result = sut.Execute();

        // assert
        A.CallTo(() => repository.GetAll()).MustHaveHappenedOnceExactly();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be(expectedName);
        result.First().Id.Should().Be(expectedId);
    }
}