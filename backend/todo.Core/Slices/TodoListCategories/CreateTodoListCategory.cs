using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListCategories;

public class CreateTodoListCategory(ITodoListCategoryRepository repository) : ICreateTodoListCategory
{
    public async Task<DomainOperationResult<TodoListCategory>> ExecuteAsync(TodoListCategoryCreateDto dto)
    {
        var todoListCategory = TodoListCategory.Create(dto);

        if (todoListCategory.IsSuccess())
        {
            repository.Add(todoListCategory.Result);
            await repository.SaveChangesAsync();
        }

        return todoListCategory;
    }
}