using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListEntries;

public class CreateTodoListEntry(ITodoListEntryRepository repository) : ICreateTodoListEntry
{
    public async Task<DomainOperationResult<TodoListEntry>> ExecuteAsync(TodoListEntryCreateDto dto)
    {
        var todoListEntry = TodoListEntry.Create(dto);

        if (todoListEntry.IsSuccess())
        {
            repository.Add(todoListEntry.Result);
            await repository.SaveChangesAsync();
        }

        return todoListEntry;
    }
}