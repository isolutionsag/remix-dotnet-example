using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListCategories;

public class UpdateTodoListCategory(ITodoListCategoryRepository repository) : IUpdateTodoListCategory
{
    public async Task<DomainOperationResult<TodoListCategory>> ExecuteAsync(TodoListCategoryDto dto)
    {
        var todoListCategory = TodoListCategory.UpdateOrDelete(dto);

        if (todoListCategory.IsSuccess())
        {
            repository.Update(todoListCategory.Result);
            await repository.SaveChangesAsync();
        }

        return todoListCategory;
    }
}