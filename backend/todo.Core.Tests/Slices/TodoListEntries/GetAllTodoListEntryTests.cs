using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListEntries;

namespace todo.Core.Tests.Slices.TodoListEntries;

public class GetAllTodoListTests
{
    [Fact]
    public void Execute_ShouldReturnValues()
    {
        // arrange
        var repository = A.Fake<ITodoListEntryRepository>();
        var sut = new GetAllTodoListEntry(repository);
        var expectedId = Guid.NewGuid();
        var expectedParentId = Guid.NewGuid();
        var expectedTitle = Guid.NewGuid().ToString();
        var expectedDescription = Guid.NewGuid().ToString();
        var categories = new List<TodoListEntry>
        {
            new TodoListEntry
            {
                Id = expectedId,
                Description = expectedDescription,
                ParentId =expectedParentId,
                Title = expectedTitle,
            }
        };
        A.CallTo(() => repository.GetAll()).Returns(categories);

        // act
        var result = sut.Execute(null);

        // assert
        A.CallTo(() => repository.GetAll()).MustHaveHappenedOnceExactly();
        result.Should().HaveCount(1);
        result.First().Title.Should().Be(expectedTitle);
        result.First().Description.Should().Be(expectedDescription);
        result.First().Id.Should().Be(expectedId);
        result.First().ParentId.Should().Be(expectedParentId);
    }
}