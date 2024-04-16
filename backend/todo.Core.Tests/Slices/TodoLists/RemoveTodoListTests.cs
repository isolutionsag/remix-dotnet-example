using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoLists;

namespace todo.Core.Tests.Slices.TodoLists
{
    public class RemoveTodoListTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidId_ShouldRemoveCategoryAndSaveChanges()
        {
            // Arrange
            var repository = A.Fake<ITodoListRepository>();
            var categoryId = Guid.NewGuid(); // Create a valid category ID
            var sut = new RemoveTodoList(repository);

            // Act
            var result = await sut.ExecuteAsync(categoryId);

            // Assert
            result.IsSuccess().Should().BeTrue();
            A.CallTo(() => repository.Remove(A<TodoList>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
        }
    }
}
