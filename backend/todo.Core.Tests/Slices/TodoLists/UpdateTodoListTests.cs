using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoLists;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Slices.TodoLists
{
    public class UpdateTodoListTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidDto_ShouldAddCategoryAndSaveChanges()
        {
            // arrange
            var repository = A.Fake<ITodoListRepository>();
            var dto = DtoHelper.ValidTodoListDto();
            var sut = new UpdateTodoList(repository);

            // act
            var result = await sut.ExecuteAsync(dto);

            // assert
            result.IsSuccess().Should().BeTrue();
            A.CallTo(() => repository.Update(A<TodoList>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidDto_ShouldNotAddCategory()
        {
            // arrange
            var repository = A.Fake<ITodoListRepository>();
            var dto = DtoHelper.InvalidTodoListDto();
            var sut = new UpdateTodoList(repository);

            // act
            var result = await sut.ExecuteAsync(dto);

            // assert
            result.IsSuccess().Should().BeFalse();
            A.CallTo(() => repository.Update(A<TodoList>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustNotHaveHappened();
        }
    }
}
