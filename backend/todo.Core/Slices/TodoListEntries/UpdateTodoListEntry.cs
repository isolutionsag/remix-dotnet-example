using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListEntries;

public class UpdateTodoListEntry(ITodoListEntryRepository repository) : IUpdateTodoListEntry
{
    public async Task<DomainOperationResult<TodoListEntry>> ExecuteAsync(TodoListEntryDto dto)
    {
        var todoListEntry = TodoListEntry.UpdateOrDelete(dto);

        if (todoListEntry.IsSuccess())
        {
            repository.Update(todoListEntry.Result);
            await repository.SaveChangesAsync();
        }

        return todoListEntry;
    }
}