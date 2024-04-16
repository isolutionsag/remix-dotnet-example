using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListEntries;

public class RemoveTodoListEntry(ITodoListEntryRepository repository) : IRemoveTodoListEntry
{
    public async Task<DomainOperationResult<TodoListEntry>> ExecuteAsync(Guid id)
    {
        var entity = repository.GetById(id);

        repository.Remove(entity);
        await repository.SaveChangesAsync();

        return DomainOperationResult<TodoListEntry>.Success(entity);
    }
}