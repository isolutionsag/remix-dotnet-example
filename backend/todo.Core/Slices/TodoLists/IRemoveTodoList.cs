using todo.Core.Entities;

namespace todo.Core.Slices.TodoLists;

public interface IRemoveTodoList : IDomainOperationAsync<TodoList, Guid>
{
}