using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListCategories;

public class RemoveTodoListCategory(ITodoListCategoryRepository repository) : IRemoveTodoListCategory
{
    public async Task<DomainOperationResult<TodoListCategory>> ExecuteAsync(Guid id)
    {
        var entity = repository.GetById(id);

        repository.Remove(entity);
        await repository.SaveChangesAsync();

        return DomainOperationResult<TodoListCategory>.Success(entity);
    }
}
