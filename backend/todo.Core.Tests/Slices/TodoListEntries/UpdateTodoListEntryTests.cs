using FakeItEasy;
using FluentAssertions;
using todo.Core.Entities;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListEntries;
using todo.Core.Tests.TestHelpers;

namespace todo.Core.Tests.Slices.TodoListEntries
{
    public class UpdateTodoListTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidDto_ShouldAddCategoryAndSaveChanges()
        {
            // arrange
            var repository = A.Fake<ITodoListEntryRepository>();
            var dto = DtoHelper.ValidTodoListEntryDto();
            var sut = new UpdateTodoListEntry(repository);

            // act
            var result = await sut.ExecuteAsync(dto);

            // assert
            result.IsSuccess().Should().BeTrue();
            A.CallTo(() => repository.Update(A<TodoListEntry>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidDto_ShouldNotAddCategory()
        {
            // arrange
            var repository = A.Fake<ITodoListEntryRepository>();
            var dto = DtoHelper.InvalidTodoListEntryDto();
            var sut = new UpdateTodoListEntry(repository);

            // act
            var result = await sut.ExecuteAsync(dto);

            // assert
            result.IsSuccess().Should().BeFalse();
            A.CallTo(() => repository.Update(A<TodoListEntry>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => repository.SaveChangesAsync(default)).MustNotHaveHappened();
        }
    }
}
