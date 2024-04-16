using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoLists;

public class RemoveTodoList(ITodoListRepository repository) : IRemoveTodoList
{
    public async Task<DomainOperationResult<TodoList>> ExecuteAsync(Guid entityId)
    {
        var entity = repository.GetById(entityId);

        repository.Remove(entity);
        await repository.SaveChangesAsync();

        return DomainOperationResult<TodoList>.Success(entity);
    }
}