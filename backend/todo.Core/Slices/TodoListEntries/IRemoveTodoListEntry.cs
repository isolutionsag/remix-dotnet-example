using todo.Core.Entities;

namespace todo.Core.Slices.TodoListEntries;

public interface IRemoveTodoListEntry : IDomainOperationAsync<TodoListEntry, Guid>
{
}