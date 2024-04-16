using todo.Core.Dtos;

namespace todo.Core.Slices.TodoListEntries;

public interface IGetAllTodoListEntry
{
    IEnumerable<TodoListEntryDto> Execute(Guid? id = null);
}