using todo.Core.Entities;

namespace todo.Core.Slices.TodoListCategories;

public interface IRemoveTodoListCategory : IDomainOperationAsync<TodoListCategory, Guid>
{
}