using todo.Core.Dtos;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListCategories;

public class GetAllTodoListCategory(ITodoListCategoryRepository repository) : IGetAllTodoListCategory
{
    public IEnumerable<TodoListCategoryDto> Execute() => repository.GetAll().Select(x => x.ToDto());
}