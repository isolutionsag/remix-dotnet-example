using todo.Core.Dtos;
using todo.Core.Entities;

namespace todo.Core.Slices.TodoListEntries;

public interface ICreateTodoListEntry : IDomainOperationAsync<TodoListEntry, TodoListEntryCreateDto>
{
}