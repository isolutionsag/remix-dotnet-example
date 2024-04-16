using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListEntries;

namespace todo.Core.Tests.Slices.TodoListEntries
{
    public class RemoveTodoListTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidId_ShouldRemoveCategoryAndSaveChanges()
        {
            // Arrange
            var repository = A.Fake<ITodoListEntryRepository>();
            var categoryId = Guid.NewGuid(); // Create a valid category ID
            var sut = new RemoveTodoListEntry(repository);

            // Act
            var result = await sut.ExecuteAsync(categoryId);

            // Assert
            result.IsSuccess().Should().BeTrue();
            A.CallTo(() => repository.Remove(A<TodoListEntry>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
        }
    }
}
