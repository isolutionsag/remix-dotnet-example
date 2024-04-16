using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListCategories;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Slices.TodoListCategories
{
    public class UpdateTodoListCategoryTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidDto_ShouldAddCategoryAndSaveChanges()
        {
            // arrange
            var repository = A.Fake<ITodoListCategoryRepository>();
            var dto = DtoHelper.ValidTodoListCategoryDto();
            var sut = new UpdateTodoListCategory(repository);

            // act
            var result = await sut.ExecuteAsync(dto);

            // assert
            result.IsSuccess().Should().BeTrue();
            A.CallTo(() => repository.Update(A<TodoListCategory>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidDto_ShouldNotAddCategory()
        {
            // arrange
            var repository = A.Fake<ITodoListCategoryRepository>();
            var dto = DtoHelper.InvalidTodoListCategoryDto();
            var sut = new UpdateTodoListCategory(repository);

            // act
            var result = await sut.ExecuteAsync(dto);

            // assert
            result.IsSuccess().Should().BeFalse();
            A.CallTo(() => repository.Update(A<TodoListCategory>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustNotHaveHappened();
        }
    }
}
