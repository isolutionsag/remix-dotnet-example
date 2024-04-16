using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListCategories;

namespace todo.Core.Tests.Slices.TodoListCategories
{
    public class RemoveTodoListCategoryTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidId_ShouldRemoveCategoryAndSaveChanges()
        {
            // Arrange
            var repository = A.Fake<ITodoListCategoryRepository>();
            var categoryId = Guid.NewGuid(); // Create a valid category ID
            var sut = new RemoveTodoListCategory(repository);

            // Act
            var result = await sut.ExecuteAsync(categoryId);

            // Assert
            result.IsSuccess().Should().BeTrue();
            A.CallTo(() => repository.Remove(A<TodoListCategory>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
        }
    }
}
